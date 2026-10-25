using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDirectSkillDamageChecks
{
    private const string Owner = "fixture:jiyi-owner", Driver = "fixture:jiyi-driver", Quiet = "fixture:jiyi-quiet";
    private const string Watch = "fixture:jiyi-use-watch";
    private const string Deck = "fixture:jiyi-deck", Qiangxi = "boundary:qiangxi", Fanci = "shanhe:fanci";
    private static readonly string[] Tiers = [StandardShanheCampaignCatalog.CurrentJiyiSkillId,
        StandardShanheCampaignCatalog.CurrentJiyi2SkillId, StandardShanheCampaignCatalog.CurrentJiyi3SkillId];

    public static void QiangxiFanciAndRealCardUsesRetainTheirDamageOrigins()
    {
        var s = Scene.Start([.. Tiers, Fanci]);
        s.Use(Qiangxi, "receive-damage", [1]);
        var hits = s.E<DamageAppliedEvent>();
        Need(hits.Select(hit => (hit.TargetSeat, hit.Amount, hit.SourceLess))
                .SequenceEqual([(0, 1, true), (1, 7, false), (2, 7, false), (3, 7, false), (1, 7, false)]) &&
            s.V().Hp == 19 && s.E<ProgramSkillResolvedEvent>().Count(e => e.SkillId == Qiangxi && e.Completed) == 1,
            "The genuine source-less Qiangxi cost stays one; its nested production Fanci and original skill hit each receive all three independent tiers once.");
        var modifiers = s.Modifiers();
        Need(modifiers.Length == 12 && modifiers.All(e => e.Source.OwnerSeat == 0 &&
                e.Source.BindingId == "direct-skill-damage" &&
                e.Source.SkillInstanceId == "campaign:shanhe-tactic:" + e.Source.SkillId) &&
            modifiers.Select(e => e.Source.SkillId).ToHashSet().SetEquals(Tiers) &&
            s.E<SkillUsageConsumedEvent>().Count(e => e.SkillId == Fanci && e.SkillOwnerSeat == 0) == 1,
            "Each real skill segment keeps the equipped tactic's actual acquisition binding and Fanci's original paid turn allowance.");

        var material = s.Card(CardKind.Slash, [2]);
        var use = s.VirtualSlash(2);
        Need(use is { CardId: 0, CardKind: CardKind.Slash, PhysicalCardIds.Count: 0,
                Action: { Type: CardActionType.Use, ActorSeat: 0, EffectiveKind: CardKind.Slash, PhysicalCards.Count: 0 } } &&
            s.E<DamageAppliedEvent>().TakeLast(2).All(e => e is { SourceSeat: 0, TargetSeat: 2, Amount: 1 } &&
                e.SourceCardUseResolutionId is not null) && s.Modifiers().Length == modifiers.Length &&
            s.E<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == 0) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) == 1,
            "Both an actual physical Slash and an actual zero-entity Slash Use remain card damage, even though the latter has no Card entity and was requested by a skill.");
        Need(s.G.CardMovements.Count(move => move.CardId == material && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing) == 1,
            "The real Slash material is paid once through native movement.");
    }

    public static void ThreeTiersAndElementalPropagationAddOnlyAtTheSkillOrigin()
    {
        for (var index = 0; index < Tiers.Length; index++)
        {
            var tier = Scene.Start([Tiers[index]]); tier.Use(Driver, "hit", [1]);
            Need(tier.E<DamageAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 1 } hit] && hit.Amount == index + 2 &&
                tier.Modifiers() is [{ Source.OwnerSeat: 0, BaseAmount: 1 } modifier] && modifier.ModifiedAmount == index + 2,
                "Each independently equipped production tier applies its own +1/+2/+3 to one actual program damage instruction.");
        }
        var s = Scene.Start([Tiers[2]]);
        s.Card(CardKind.IronChain, [1, 2]);
        Need(s.V(1).IsChained && s.V(2).IsChained && !s.V(3).IsChained,
            "A real paid Iron Chain links exactly the two intended native damage recipients.");
        s.Use(Driver, "fire", [1]);
        Need(s.E<DamageAppliedEvent>().Select(e => (e.TargetSeat, e.Amount, e.Nature))
                .SequenceEqual([(1, 4, DamageNature.Fire), (2, 4, DamageNature.Fire)]) &&
            s.Modifiers() is [{ TargetSeat: 1, CardKind: null, BaseAmount: 1, ModifiedAmount: 4 }] &&
            s.E<ChainedDamagePropagatedEvent>().Length == 1 && !s.V(1).IsChained && !s.V(2).IsChained,
            "Direct elemental skill damage receives +3 at its actual origin and carries the finalized four through the native chain without applying +3 again.");
        s.Card(CardKind.FireSlash, [1]);
        Need(s.E<DamageAppliedEvent>().Last() is { TargetSeat: 1, Amount: 1, Nature: DamageNature.Fire } && s.Modifiers().Length == 1,
            "A later real elemental card hit does not inherit the preceding skill's classification or bonus.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }
    private static string Peer(int seat) => "fixture:jiyi-peer-" + seat;

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        private PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(0).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public ProgramCardDamageModifiedEvent[] Modifiers() => E<ProgramCardDamageModifiedEvent>()
            .Where(e => Tiers.Contains(e.Source.SkillId)).ToArray();
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Jiyi command."); }
        private void Until(Func<bool> done)
        {
            for (var step = 0; step < 350; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The bounded Jiyi battle ended before its expected boundary.");
                Need(P is not { PlayerSeat: 0, Kind: DecisionKind.PlayCard },
                    "The expected native Jiyi child must be observed before returning to ordinary Play.");
                if (P is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Kind is DecisionKind.RescueDying or DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification
                        ? prompt.Choices.First(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die")
                        : prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native Jiyi boundary.");
        }
        private void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public int Card(CardKind kind, int[] targets)
        {
            var card = V().Hand.First(c => c.Kind == kind);
            var action = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.TargetSeats.SequenceEqual(targets));
            Need(G.PredictPublicActionDamage(0, action, targets[0]).DamageAmount == (kind == CardKind.IronChain ? 0 : 1),
                "Public actual-card estimates also exclude direct-skill bonuses.");
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play(); return card.Id;
        }
        public void Use(string skill, string activation, int[] targets)
        { Accept(new UseProgramSkillCommand(0, skill, activation, [], targets, G.Revision, P!.PromptId)); Play(); }
        public CardUseFrame VirtualSlash(int target)
        {
            Accept(new UseProgramSkillCommand(0, Driver, "virtual", [], [target], G.Revision, P!.PromptId));
            Until(() => G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.CardId == 0 && f.CardKind == CardKind.Slash));
            var use = G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == 0 && f.CardKind == CardKind.Slash);
            Play(); return use;
        }
        public static Scene Start(string[] tactics)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheDirectSkillDamagePackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture:jiyi", Owner, 20, 0, 0, 0, 0,
                [Qiangxi], [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.FireSlash, Suit.Spade, 8), new(CardKind.IronChain, Suit.Club, 10)], [],
                Enumerable.Range(1, 3).Select(seat => new ShanheCombatantDefinition(Peer(seat)) { StartingHand = [] }).ToArray(), [], new string('a', 64))
                { TacticSkillIds = tactics };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-jiyi", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, ["boundary:dian-wei"]);
            string Read(string name)
            {
                using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms." + name) ??
                    throw new InvalidOperationException("Missing production Shanhe reaction bundle.");
                using var reader = new StreamReader(stream); return reader.ReadToEnd();
            }
            var reactions = SkillProgramCatalog.Load(Read("shanhe-reactions.rules.json"), Read("shanhe-reactions.presentation.json"));
            builder.AddSkill(new(Fanci, reactions.Presentations[Fanci].Name, reactions.Presentations[Fanci].Description)
            { Program = reactions.Programs[Fanci], Tags = SkillTag.Locked });
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[
                    {"id":"range","query":"attackRange","operation":"add","value":10,"priority":0},
                    {"id":"slash-limit","query":"slashLimit","operation":"add","value":10,"priority":0}],"activations":[
                    {"id":"hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                    {"id":"fire","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1,"nature":"fire"}]},
                    {"id":"virtual","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"distanceUnlimitedAgainstTarget","useCardActionWindows":true}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"observe-actual-slash-use","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["slash"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"native-use","options":[{"id":"continue"}]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet, Watch }.ToDictionary(id => id, id => new { name = "真实技能伤害驱动", description = "原生技能伤害与实际用牌边界",
                        optionLabels = id == Watch ? new Dictionary<string, string> { ["continue"] = "继续" } : new Dictionary<string, string>() }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "技艺使用者", "supporter", Driver, "qun", 20, [Watch]));
            foreach (var seat in Enumerable.Range(1, 3)) builder.AddGeneral(new(Peer(seat), "固定敌方", "supporter", Quiet, "wei", 30));
            builder.AddDeck(new(Deck, "固定实体闪", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 96).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "技艺真实伤害", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
