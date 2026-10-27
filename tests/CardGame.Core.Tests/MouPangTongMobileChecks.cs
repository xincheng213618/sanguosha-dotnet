using System.Runtime.CompilerServices;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;
internal static class MouPangTongMobileChecks
{
    private static ShanheStartingCard C(CardKind kind, Suit suit, int rank) => new(kind, suit, rank);

    public static void OrderedNativeNiepanUpgradeAndAllTargetDiscardCold()
    {
        var s = Scene.Start("rebirth-upgrade", true); s.Play();
        var original = s.V().Hand.Select(c => c.Id).Concat(s.V().Equipment.Select(c => c.Id)).ToArray();
        s.Driver("down-and-dying");
        s.Reach(() => s.P?.Kind == DecisionKind.RescueDying && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("skill-id") == Scene.Niepan));
        s.Need(s.V().IsChained && s.V().IsFaceDown && s.V().Hp <= 0, "Real native commands establish both statuses and enter own Dying."); s.Cold();
        s.Answer(c => c.Parameters.GetValueOrDefault("response") == "program-trigger" && c.Parameters.GetValueOrDefault("skill-id") == Scene.Niepan); s.Play();
        var upgraded = s.E<MouPangTongUpgradedEvent>().Single();
        s.Need(s.V() is { Hp: 2, IsChained: false, IsFaceDown: false, HandCount: 2, Equipment.Count: 0 } &&
            original.All(id => s.G.CardMovements.Any(m => m.CardId == id && m.To == CardLocation.DiscardPile)) &&
            s.E<RecoveryAppliedEvent>().Any(e => e.TargetSeat == 0) &&
            Array.FindLastIndex(s.G.Events.Select(e => e.Payload).ToArray(), e => e is RecoveryAppliedEvent) < Array.FindIndex(s.G.Events.Select(e => e.Payload).ToArray(), e => e is MouPangTongUpgradedEvent),
            "Niepan performs real owned-zone discard, Draw2, native recovery to two, restoration and only then permanent upgrade.");
        var all = s.Actions().First(a => a.Kind == LegalActionKind.IronChain && a.ConversionSource?.SkillId == Scene.Lianhuan && a.TargetSeats.SequenceEqual([0, 1, 2, 3]));
        s.Native(all);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Scene.Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        var queue = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.MouPangTongTargets is not null);
        s.Need(queue.MouPangTongTargets!.Cursor == 1 && s.V().Hp == 2 && s.E<MouPangTongLianhuanPaidEvent>().Length == 0,
            "Level two has no HP payment; its actual self-target discard child pauses the owning frozen four-target queue.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.E<MouPangTongTargetDiscardedEvent>().Select(e => e.TargetSeat).SequenceEqual([0, 1, 2, 3]) &&
            Enumerable.Range(0, 4).All(i => s.V(i).IsChained) && s.V().Hp == 2 && s.E<MouPangTongUpgradedEvent>().Single() == upgraded &&
            s.E<MouPangTongTargetsCompletedEvent>().Single().TargetsVisited == 4,
            "Cold native movement-child return resumes the next target exactly once, preserves the upgrade and applies the real four-target IronChain.");
    }

    private sealed class Scene(string name, GameEngine game, ContentRegistry registry)
    {
        public const string Hero = "mou:pang-tong-mobile", Lianhuan = "mou-mobile:lianhuan", Niepan = "mou-mobile:niepan", Watch = "fixture:moupt-watch", Late = "fixture:moupt-late";
        private const string DriverSkill = "fixture:moupt-driver", Quiet = "fixture:moupt-quiet", Ally = "fixture:moupt-ally", EnemyA = "fixture:moupt-enemy-a", EnemyB = "fixture:moupt-enemy-b", Deck = "fixture:moupt-deck";
        public GameEngine G { get; private set; } = game;
        public ContentRegistry R => registry;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public LegalAction[] Actions() => G.GetHumanLegalActions().ToArray();
        public void Native(LegalAction a) => Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource });
        public void Driver(string id) => Accept(new UseProgramSkillCommand(0, DriverSkill, id, [], [], G.Revision, P!.PromptId));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var c = p.Choices.FirstOrDefault(predicate); Need(p.PlayerSeat == 0 && c is not null, "Only the published human choice is answered."); Accept(new AnswerPromptCommand(0, p.PromptId, c!.Id, G.Revision)); }
        public void Reach(Func<bool> done, [CallerLineNumber] int line = 0)
        {
            for (var i = 0; i < 160; i++)
            {
                if (done()) return; Need(G.State.Winner == Winner.None, "Unexpected terminal cutoff at " + line);
                var p = P;
                if (p is null || p.PlayerSeat != 0) Accept(new AdvanceOneStepCommand(G.Revision));
                else if (p.Kind == DecisionKind.PlayCard) Accept(new EndPlayPhaseCommand(0, G.Revision, p.PromptId));
                else if (p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die");
                else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else throw new InvalidOperationException("Unexpected real human child at " + line + Diagnostic());
            }
            throw new InvalidOperationException("Bounded native fixture failed to reach " + line + Diagnostic());
        }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), Zones = G.CreateCardZoneDiagnostics(), Moves = G.CardMovements,
            Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == State(), "Cold restore preserves the exact native Dying or actual discard-child cursor and safe views."); }
        public void Accept(GameCommand c)
        { try { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected command"); } catch (Exception e) { throw new InvalidOperationException(name + ": " + e.Message + Diagnostic(), e); } }
        public void Need(bool yes, string why) { if (!yes) throw new InvalidOperationException(why + Diagnostic()); }
        private string Diagnostic() => JsonSerializer.Serialize(new { name, G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, Prompt = P,
            Players = Enumerable.Range(0, 4).Select(i => new { i, V(i).Hp, V(i).HandCount, V(i).IsChained }), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())),
            Facts = G.Events.Where(e => e.Payload.GetType().Name.StartsWith("MouPangTong", StringComparison.Ordinal)).TakeLast(20).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
        public static Scene Start(string name, bool rebirth)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] hand = [C(CardKind.Slash, Suit.Club, 5), C(CardKind.Dodge, Suit.Club, 6), C(CardKind.IronChain, Suit.Spade, 9)];
            var setup = new ShanheBattleSetup(7, 1, "moupt-" + name, Hero, 3, 0, 0, 0, 0, rebirth ? [DriverSkill, Watch] : [DriverSkill, Watch, Late], hand, rebirth ? [C(CardKind.Crossbow, Suit.Spade, 8)] : [],
                [new(EnemyA, Hp: 12) { StartingHand = [C(CardKind.Slash, Suit.Spade, 4), C(CardKind.Slash, Suit.Spade, 5)] }, new(EnemyB, Hp: 12) { StartingHand = [C(CardKind.Slash, Suit.Spade, 4), C(CardKind.Slash, Suit.Spade, 5)] }],
                [new(Ally, Hp: 12) { StartingHand = [C(CardKind.Slash, Suit.Spade, 4), C(CardKind.Slash, Suit.Spade, 5)] }], new string('e', 64));
            var options = new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 };
            var s = new Scene(name, GameEngine.CreateStandard(options, r), r); s.Accept(new StartGameCommand()); return s;
        }
        private sealed class Fixture : IGameContentPackage
        {
            public PackageManifest Manifest { get; } = new("fixture-mou-pang-tong-native", new(1, 0, 0));
            public void Register(IContentRegistryBuilder b)
            {
                CharacterFixtureContent.Register(b, Hero);
                foreach (var kind in new[] { CardKind.Slash, CardKind.Dodge, CardKind.IronChain, CardKind.Crossbow })
                { var c = CardCatalog.Get(kind); b.AddCard(new("fixture:moupt-card-" + kind, c.DisplayName, c.CategoryName, c.Description, kind)); }
                var definitions = new object[] {
                    new { id = DriverSkill, revision = 1, activations = new[] { new { id = "down-and-dying", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                        effects = new object[] { new { op = "setChainedState", target = "owner", chained = true }, new { op = "setFaceState", target = "owner", faceDown = true }, new { op = "loseHp", target = "owner", amount = 3 } } } } },
                    new { id = Quiet, revision = 1, triggers = new[] { new { id = "quiet", window = "afterNormalDraw", subject = "owner", optional = false, effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } } } },
                    new { id = Watch, revision = 1, triggers = new[] { new { id = "real-discard-child", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" }, movementReasons = new[] { "skill-program.mou-mobile:lianhuan.discard" }, movementOccurrence = "perBatch", optional = false,
                        effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "movement-hold", options = new[] { new { id = "continue" } } } } } } },
                    new { id = Late, revision = 1, triggers = new[] { new { id = "actual-late-target", window = "cardUseTargetsFinalized", ownerRelation = "actor", cardKinds = new[] { "ironChain" }, priority = -1000, usageScope = "game", usageLimit = 1, optional = false,
                        effects = new object[] { new { op = "selectTarget", target = "owner", targetKind = "otherLegalCurrentCardTarget" }, new { op = "addCurrentCardUseTarget", target = "selectedTarget" } } } } }
                };
                var labels = new[] { DriverSkill, Quiet, Watch, Late }.ToDictionary(id => id, id => new { name = id, description = "谋庞统真实原生流程夹具", optionLabels = id == Watch ? new Dictionary<string, string> { ["continue"] = "继续原生弃牌结算" } : new Dictionary<string, string>() });
                var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
                foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, labels[id].description) { Program = p, ProgramPresentation = catalog.Presentations[id] });
                foreach (var (id, faction) in new[] { (Ally, "shu"), (EnemyA, "wei"), (EnemyB, "wei") }) b.AddGeneral(new(id, "真实同行", "", "standard:none", faction, 12, [Quiet]));
                b.AddDeck(new(Deck, "固定梅花实体库存", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("fixture:moupt-card-Dodge", Suit.Club, 7)).ToArray() });
                b.AddMode(new(ShanheBattleSetup.BattleModeId, "谋庞统原生小局", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Ally, EnemyA, EnemyB], ModeKind: ContentModeKind.Team,
                    TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
            }
        }
    }
}
