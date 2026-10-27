using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheKunlunMirrorChecks
{
    private const string Skill = StandardShanheKunlunMirrorPackage.SkillId, Driver = "fixture:kunlun-driver",
        Owner = "fixture:kunlun-owner", Peer = "fixture:kunlun-peer", Deck = "fixture:kunlun-deck";

    public static void FirstRealTrickLearnsItsNameAndTreasureMemorySurvivesRemovalAndDifferentEntity()
    {
        var scene = Scene.Start("memory"); var mirror = scene.View(1).Equipment.Single(card => card.Kind == CardKind.KunlunMirror).Id;
        var replacement = scene.View(0).Hand.Single(card => card.Kind == CardKind.KunlunMirror).Id;
        Need(EquipmentCatalog.Get(CardKind.KunlunMirror).Slot == EquipmentSlot.Treasure &&
            scene.View(1).Equipment.Any(card => card.Kind == CardKind.RenwangShield) && scene.Has(1),
            "The actual Kunlun Mirror is a treasure, coexists with a real armor and grants its own equipment instance.");
        var first = scene.Card(CardKind.Duel, [1]); scene.Play();
        var learned = scene.Learned.Single();
        Need(scene.View(1).Hp == 11 && learned.Receipt is { Learning: true, Amount: 1,
                Source: { OwnerSeat: 1, SkillId: Skill }, Origin: { EffectiveKind: CardKind.Duel, CardUserSeat: 0, SourceSeat: 0, TargetSeat: 1 } } &&
            learned.Receipt.Source.SkillInstanceId == "equipment:shanhe-kunlun-mirror:" + mirror &&
            learned.Receipt.Origin.NativeCardId == first && learned.Receipt.Origin.AttackResolutionId == scene.UseId(first) &&
            learned.Receipt.Origin.CardActionId is > 0 && scene.Prevented.Length == 0,
            "The first positive native Duel damage really applies before its original effective name and treasure source are learned.");
        scene.Use("disable-armor", [], [1]); scene.Play();
        var second = scene.Card(CardKind.Duel, [1]); scene.Play();
        Need(scene.View(1).Hp == 11 && scene.Prevented.Length == 1 &&
            scene.Events<CardTargetRestrictionGrantedEvent>().Any(fact => fact.Restriction.TargetSeat == 1 &&
                fact.Restriction.Restriction == SkillProgramCardTargetRestriction.ArmorIneffectiveForTurn),
            "An actual turn armor restriction does not suppress the treasure's native same-name prevention.");
        scene.Card(CardKind.Snatch, [1], mirror); scene.Play();
        Need(!scene.Has(1) && scene.View(0).Hand.Any(card => card.Id == mirror) && scene.Learned.Length == 1,
            "A real Snatch removes the original treasure grant while retaining this person's one learned-name fact.");
        scene.Card(CardKind.Duel, [1]); scene.Play();
        Need(scene.View(1).Hp == 10 && scene.Learned.Length == 1 && scene.Prevented.Length == 1,
            "Remembered names do not protect a person while the mirror is absent, nor learn again from an absent source.");
        scene.Use("place", [mirror], [1]); scene.Play(); scene.Card(CardKind.Duel, [1]); scene.Play();
        Need(scene.View(1).Hp == 10 && scene.Prevented.Length == 2 && scene.Learned.Single() == learned,
            "Re-equipping the same real treasure entity reuses this person's current-game memory.");
        scene.Use("place", [replacement], [1]); scene.Play(); scene.Card(CardKind.Duel, [1]); scene.Play();
        Need(scene.View(1).Hp == 10 && scene.Prevented.Length == 3 && scene.Learned.Single() == learned &&
            scene.Prevented.Last().Receipt.Source.SkillInstanceId == "equipment:shanhe-kunlun-mirror:" + replacement &&
            scene.View(1).Equipment.Single(card => card.Kind == CardKind.KunlunMirror).Id == replacement,
            "A different equipped mirror instance prevents the learned name without issuing a second learning or borrowing the old grant.");
        scene.Card(CardKind.BarbarianAssault); scene.Play();
        scene.Card(CardKind.Slash, [1]); scene.Play(); scene.Use("skill-hit", [], [1]); scene.Play();
        Need(scene.View(1).Hp == 7 && scene.Learned.Length == 2 &&
            scene.Learned.Select(fact => fact.Receipt.Origin.EffectiveKind).Order().SequenceEqual(new[] { CardKind.Duel, CardKind.BarbarianAssault }.Order()) &&
            scene.Prevented.Length == 3 && scene.Events<ProgramDamagePreventedEvent>().Count(fact => fact.SkillId == Skill) == 3 &&
            scene.Game.CardMovements.Count(move => move.CardId == second && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
            scene.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == scene.UseId(second)) == 1 && scene.Game.State.Winner == Winner.None,
            "A different trick pays its first damage and learns separately; an actual basic Slash and raw skill damage remain unrelated, while prevented Duel uses still pay and finish exactly once.");
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot View(int seat) => Game.CreateSnapshot(seat).Players.Single(player => player.Seat == seat);
        public T[] Events<T>() => Game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public KunlunMirrorTrickLearnedEvent[] Learned => Events<KunlunMirrorTrickLearnedEvent>();
        public KunlunMirrorPreventionIssuedEvent[] Prevented => Events<KunlunMirrorPreventionIssuedEvent>();
        public bool Has(int seat) => View(seat).Skills?.Any(skill => skill.ContentId == Skill) == true;
        public long UseId(int card) => Events<CardUseDeclaredEvent>().Single(fact => fact.CardId == card).ResolutionId;
        public void Submit(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Kunlun Mirror command."); }
        public void Use(string activation, int[] cards, int[] targets) => Submit(new UseProgramSkillCommand(0, Driver,
            activation, cards, targets, Game.Revision, Prompt!.PromptId));
        public int Card(CardKind kind, int[]? targets = null, int? publicTarget = null)
        {
            var card = View(0).Hand.First(card => card.Kind == kind);
            var action = Game.GetHumanLegalActions().First(action => action.CardId == card.Id && action.Kind != LegalActionKind.Recast &&
                action.TargetCardId == publicTarget && (targets is null || action.TargetSeats.SequenceEqual(targets)));
            Submit(new PlayCardCommand(0, card.Id, targets ?? action.TargetSeats, Game.Revision, Prompt!.PromptId,
                action.PlayedCardKind, publicTarget)); return card.Id;
        }
        public int FireAttack(int target)
        {
            var discard = View(0).Hand.First(card => card.Kind == CardKind.Dodge).Id;
            var fire = Card(CardKind.FireAttack, [target]);
            Reach(() => Prompt is { Kind: DecisionKind.FireAttackDiscard });
            var prompt = Prompt!; var choice = prompt.Choices.Single(choice => choice.Cards.SequenceEqual([discard]));
            Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision)); Play(); return fire;
        }
        public void Play() => Reach(() => Prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && Game.ResolutionStack.Count == 0);
        public void NextOwnPlay()
        {
            var turn = Game.State.TurnNumber;
            Submit(new EndPlayPhaseCommand(0, Game.Revision, Prompt!.PromptId));
            Reach(() => Prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && Game.ResolutionStack.Count == 0 && Game.State.TurnNumber > turn);
        }
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 440; step++)
            {
                if (ready()) return;
                Need(Game.State.Winner == Winner.None, "The native mirror fixture must remain a bounded live encounter.");
                if (Prompt is { } prompt && !(prompt.Kind == DecisionKind.PlayCard && Game.ResolutionStack.Count != 0))
                    throw new InvalidOperationException("Unexpected mirror boundary: " + JsonSerializer.Serialize(new
                    { prompt.Kind, prompt.PlayerSeat, Skill = prompt.SkillPrompt?.SkillId, Game.State.CurrentSeat, Game.State.Phase }));
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Kunlun Mirror exceeded its bounded native flow.");
        }
        public static Scene Start(string scenario)
        {
            var chain = scenario == "chain"; var lightning = scenario == "lightning"; var count = chain ? 3 : 2;
            var mirrorSeat = lightning ? 0 : chain ? 2 : 1;
            var cards = lightning ? new[] { new ShanheStartingCard(CardKind.Lightning, Suit.Spade, 1), new(CardKind.Lightning, Suit.Spade, 1) } :
                chain ? new[] { new ShanheStartingCard(CardKind.FireAttack, Suit.Heart, 3), new(CardKind.FireAttack, Suit.Heart, 4),
                    new(CardKind.IronChain, Suit.Club, 6), new(CardKind.Dodge, Suit.Spade, 5), new(CardKind.Dodge, Suit.Spade, 6) } :
                Enumerable.Range(1, 5).Select(rank => new ShanheStartingCard(CardKind.Duel, Suit.Diamond, rank)).Concat(new[] {
                    new ShanheStartingCard(CardKind.BarbarianAssault, Suit.Spade, 7), new(CardKind.Slash, Suit.Spade, 7),
                    new(CardKind.Snatch, Suit.Club, 3), new(CardKind.KunlunMirror, Suit.Spade, 12) }).ToArray();
            var enemies = Enumerable.Range(1, count - 1).Select(seat => new ShanheCombatantDefinition(Peer)
            {
                StartingHand = chain ? [new(CardKind.Dodge, Suit.Spade, 2)] : [],
                StartingEquipment = seat == mirrorSeat ? scenario == "memory"
                    ? [new(CardKind.KunlunMirror, Suit.Spade, 12), new(CardKind.RenwangShield, Suit.Club, 2)]
                    : [new(CardKind.KunlunMirror, Suit.Spade, 12)] : []
            }).ToArray();
            var setup = new ShanheBattleSetup(71, 1, "kunlun-native", Owner, 20, 0, 0, 0, 0, [], cards,
                lightning ? [new(CardKind.KunlunMirror, Suit.Spade, 12)] : scenario == "memory" ? [new(CardKind.QinggangSword, Suit.Spade, 6)] : [],
                enemies, [], new string('a', 64));
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheKunlunMirrorPackage(), new Fixture(count));
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = count, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6
            }, registry));
            scene.Submit(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture(int count) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:kunlun-native", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var programs = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[
                  {"id":"place","minCards":1,"maxCards":1,"sourceZones":["hand"],"cardCategories":["equipment"],"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                   "effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"mirror"},{"op":"placeSelectedEquipment","target":"selectedTarget","sourceBind":"mirror"}]},
                  {"id":"disable-armor","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                   "effects":[{"op":"grantTurnCardTargetRestriction","target":"selectedTarget","targetRestriction":"armorIneffectiveForTurn"}]},
                  {"id":"skill-hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                   "effects":[{"op":"damage","target":"selectedTarget","amount":1}]}
                ]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "原生宝物检查", description = "实际装备变更与非锦囊伤害。" } } }));
            builder.AddSkill(new(Driver, programs.Presentations[Driver].Name, programs.Presentations[Driver].Description)
            { Program = programs.Programs[Driver], ProgramPresentation = programs.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "实际出牌者", "supporter", Driver, "shu", 20));
            builder.AddGeneral(new(Peer, "实际受伤者", "supporter", "standard:none", "wei", 12));
            builder.AddDeck(new(Deck, "固定黑桃五判定", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 100).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 5)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "昆仑镜真实牌流", count, count, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = count - 1 }));
        }
    }
    private static void Need(bool yes, string reason) { if (!yes) throw new InvalidOperationException(reason); }
}
