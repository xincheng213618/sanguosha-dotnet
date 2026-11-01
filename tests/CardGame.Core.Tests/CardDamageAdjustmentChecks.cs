using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class CardDamageAdjustmentChecks
{
    private const string Hero = "fixture:card-adjust-owner", Driver = "fixture:card-adjust-driver", Boost = "fixture:card-adjust-boost",
        Watch = "fixture:card-adjust-watch", Reduce = "fixture:card-adjust-reduce", Quiet = "fixture:card-adjust-quiet";

    public static void PaidUseAndTargetBonusesRemainSeparateFromCurrentDamageCold()
    {
        var s = Scene.Start();
        s.Use("chain", 1); s.Play(); s.Use("chain", 2); s.Play();
        var material = s.Card(CardKind.FireSlash);
        s.PlayCard(material, [1, 3]);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == material);
        s.Need(use.CardDamageAdjustments is IList<CardDamageAdjustment> { IsReadOnly: true, Count: 2 } rows &&
            rows.All(row => row.CardKinds is IList<CardKind> { IsReadOnly: true }) &&
            rows.Single(row => row.TargetSeat is null) is { Amount: 2, DamageLayer: SkillProgramDamageLayer.AdditionalDamage } &&
            rows.Single(row => row.TargetSeat == 1) is { Amount: 3, DamageLayer: SkillProgramDamageLayer.CardDamage } &&
            s.Players()[0].Hp == 11 &&
            !s.Players()[0].SkillGrants.Grants.Any(g => g.SkillId == Boost && g.IsEnabled) &&
            s.E<DamageAppliedEvent>() is [{ TargetSeat: 1, Amount: 5, Nature: DamageNature.Fire }],
            "The committed window freezes twelve HP before a real HP-loss child: windowHp still issues two at live eleven HP. A one-attempt reduction changes six to five, then source loss preserves the paid rows.");
        s.Cold();
        s.Need(s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == use.Id).CardDamageAdjustments?.Sum(row => row.Amount) == 5,
            "Cold command replay reconstructs the paid use rows after their skill source has disappeared.");
        s.Answer(); s.Play();
        s.Need(s.E<DamageAppliedEvent>().Select(e => (e.TargetSeat, e.Amount, e.Nature)).SequenceEqual(
                [(1, 5, DamageNature.Fire), (2, 5, DamageNature.Fire), (3, 3, DamageNature.Fire)]) &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
            "The chain inherits five without adding the rows twice; the next original target gets only the whole-use bonus, without the earlier target row or attempt reduction.");
        var before = s.E<DamageAppliedEvent>().Length;
        s.PlayCard(s.Card(CardKind.FireSlash), [1]); s.Play();
        s.Need(s.E<DamageAppliedEvent>().Skip(before).ToArray() is [{ TargetSeat: 1, Amount: 1 }] &&
            !s.G.ResolutionStack.OfType<CardUseFrame>().Any(),
            "A later native use does not inherit either a retired use's rows or its current-damage reduction.");

        var duel = Scene.Start();
        duel.PlayCard(duel.Card(CardKind.Duel), [1]);
        duel.Reach(() => duel.P?.SkillPrompt?.SkillId == Watch);
        var duelUse = duel.G.ResolutionStack.OfType<CardUseFrame>().Single();
        duel.Need(duelUse.CardKind == CardKind.Duel && duelUse.CardDamageAdjustments?.Single(row => row.TargetSeat == 1).Amount == 3 &&
            duel.E<DamageAppliedEvent>() is [{ TargetSeat: 1, Amount: 5 }],
            "The same real current-target declaration works for Duel, without a Slash-only or damage-source ownership gate.");
        duel.Cold(); duel.Answer(); duel.Play();
        duel.Need(duel.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == duelUse.Id) == 1 && duel.E<DamageAppliedEvent>().Length == 1,
            "The Duel's original native return completes once after cold recovery.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public T[] E<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public IReadOnlyList<CharacterState> Players() => (IReadOnlyList<CharacterState>)FixtureRuntimeAccess.Engine.GetField("_players", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(G)!;
        public int Card(CardKind kind) => G.CreateSnapshot(0).Players[0].Hand.First(card => card.Kind == kind).Id;
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Use(string id, int target) => Accept(new UseProgramSkillCommand(0, Driver, id, [], [target], G.Revision, P!.PromptId));
        public void PlayCard(int id, int[] targets)
        {
            var action = G.GetHumanLegalActions().First(a => a.CardId == id && a.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, id, targets, G.Revision, P!.PromptId, action.PlayedCardKind));
        }
        public void Answer() => Accept(new AnswerPromptCommand(0, P!.PromptId,
            P.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == "continue").Id, G.Revision));
        private void Accept(GameCommand command)
        {
            var result = G.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The shared damage fixture command was rejected.");
        }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 140; i++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None && P is not { PlayerSeat: 0 }, "Unexpected human or terminal boundary.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The shared damage fixture exceeded its native cursor.");
        }
        public void Cold()
        {
            var state = State();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(state == State(), "Cold recovery preserves views, original use adjustments, payment and real child windows.");
        }
        private string State() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat))),
            Frames = G.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType())),
            Facts = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), G.CardMovements
        });
        public void Need(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message + JsonSerializer.Serialize(new { G.State, Prompt = P, Facts = G.Events.TakeLast(8).Select(e => e.Payload.ToString()) }));
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard C(CardKind kind) => new(kind, Suit.Heart, 7);
            var setup = new ShanheBattleSetup(7, 1, "shared-card-adjustment", Hero, 12, 0, 0, 0, 0, [],
                [C(CardKind.FireSlash), C(CardKind.FireSlash), C(CardKind.Duel)], [],
                [new("fixture:card-adjust-enemy-a", Hp: 14) { StartingHand = [] }, new("fixture:card-adjust-enemy-b", Hp: 14) { StartingHand = [] }],
                [new("fixture:card-adjust-ally", Hp: 10) { StartingHand = [] }], new string('d', 64));
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8
            }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-card-damage-adjustments", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Driver}}}","revision":1,"modifiers":[{"id":"range","query":"attackRange","operation":"set","value":4,"priority":0},{"id":"quota","query":"slashLimit","operation":"add","value":4,"priority":0},{"id":"targets","query":"cardTargetCount","operation":"add","value":1,"priority":0,"cardKinds":["fireSlash"]}],"activations":[{"id":"chain","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"setChainedState","target":"selectedTarget","chained":true}]}]},
              {"id":"{{{Boost}}}","revision":1,"triggers":[
                {"id":"whole","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["fireSlash","duel"],"optional":false,"effects":[{"op":"loseHp","target":"owner","amount":1},{"op":"addCurrentCardDamage","target":"owner","targetScope":"use","amountExpr":["-",["windowHp","owner"],10],"cardKinds":["fireSlash","duel"]}]},
                {"id":"target","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["fireSlash","duel"],"optional":false,"effects":[{"op":"addCurrentCardDamage","target":"owner","targetScope":"currentTarget","amount":3,"damageLayer":"cardDamage","cardKinds":["fireSlash","duel"],"condition":{"kind":"expression","expr":["==",["hp","eventTarget"],10]}}]}]},
              {"id":"{{{Reduce}}}","revision":1,"triggers":[{"id":"attempt-only","window":"beforeDamageApplied","subject":"damageSource","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"reduceCurrentDamage","target":"owner","amount":1}]}]},
              {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"paid-loss","window":"afterDamageApplied","subject":"damageSource","damageOccurrence":"perDamage","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{{Boost}}}"]}},{"op":"chooseOption","target":"owner","resultBind":"pause","options":[{"id":"continue"}]}]}]},
              {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
            """, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Boost, Reduce, Watch, Quiet }.ToDictionary(id => id, id => new
                { name = id, description = "Shared use and damage boundaries", optionLabels = id == Watch ? new Dictionary<string, string> { ["continue"] = "继续" } : [] })
            }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Hero, "共享伤害来源", "supporter", Driver, "qun", 12, [Boost, Reduce, Watch]));
            foreach (var peer in new[] { "fixture:card-adjust-ally", "fixture:card-adjust-enemy-a", "fixture:card-adjust-enemy-b" })
                builder.AddGeneral(new(peer, peer, "supporter", Quiet, "wei", 14));
            foreach (var kind in new[] { CardKind.Slash, CardKind.FireSlash, CardKind.Duel })
            { var card = CardCatalog.Get(kind); builder.AddCard(new("fixture:card-adjust-" + kind, card.DisplayName, card.CategoryName, card.Description, kind)); }
            builder.AddDeck(new("fixture:card-adjust-deck", "固定实体", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("fixture:card-adjust-Slash", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "共享用牌修正", 4, 4, new Dictionary<string, int>(), "fixture:card-adjust-deck",
                GeneralPoolIds: [Hero, "fixture:card-adjust-ally", "fixture:card-adjust-enemy-a", "fixture:card-adjust-enemy-b"], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
