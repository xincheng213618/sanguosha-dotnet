using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBowenTacticChecks
{
    private const string Owner = "fixture:bowen-owner", Enemy = "fixture:bowen-enemy";
    private const string Quiet = "fixture:bowen-quiet", Deck = "fixture:bowen-deck";
    private const string GainReason = "skill-program.obtain-deck-cards";
    private static readonly string[] Tactics =
    [
        StandardShanheBowenTacticsPackage.BowenSkillId,
        StandardShanheBowenTacticsPackage.Bowen2SkillId,
        StandardShanheBowenTacticsPackage.Bowen3SkillId
    ];

    public static void ThreeTiersGainTricksAtOwnActualTurnStartsSeparatelyFromDraw()
    {
        var s = Scene.Start();
        var firstTurn = s.G.State.TurnNumber;
        var gains = s.GainEvents;
        Need(gains.Length == 3 && gains.Select(e => e.Count).Order().SequenceEqual([1, 2, 3]) &&
            s.GainMoves.Length == 6 && s.GainMoves.All(IsTrickGain) &&
            s.NormalDraws(0).Length == 2 && s.V(0).HandCount == 8 &&
            s.Events<CardsDrawnEvent>().Where(e => e.PlayerSeat == 0).Sum(e => e.Count) == 2 &&
            Tactics.All(skill => s.Resolved(skill) == 1),
            "All three equipped tiers really obtain 1+2+3 matching trick entities at the first own turn start, followed by the separate ordinary two-card draw.");
        var initialGainIds = s.GainMoves.Select(move => move.CardId).ToHashSet();
        Need(initialGainIds.Count == 6 && initialGainIds.IsSubsetOf(s.V(0).Hand.Select(card => card.Id)) &&
            !initialGainIds.Overlaps(s.NormalDraws(0).Select(move => move.CardId)),
            "The six gained tricks are actual distinct deck entities in hand, rather than duplicate ordinary draw records or generated substitutes.");

        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.NormalDraws(1).Length == 2);
        Need(s.G.State.CurrentSeat == 1 && s.GainEvents.Length == 3 && s.GainMoves.Length == 6 &&
            s.V(0).HandCount == 8 && Tactics.All(skill => s.Resolved(skill) == 1),
            "The opponent reaches and completes its actual normal draw without firing any of the owner's turn-start trick gains.");
        s.Until(() => s.G.State.TurnNumber > firstTurn && s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        gains = s.GainEvents;
        Need(gains.Length == 6 && gains.Skip(3).Select(e => e.Count).Order().SequenceEqual([1, 2, 3]) &&
            s.GainMoves.Length == 12 && s.GainMoves.All(IsTrickGain) &&
            s.GainMoves.Select(move => move.CardId).Distinct().Count() == 12 &&
            s.GainMoves.Count(move => move.TurnNumber == firstTurn) == 6 &&
            s.GainMoves.Count(move => move.TurnNumber == s.G.State.TurnNumber) == 6 &&
            s.NormalDraws(0).Length == 4 && s.NormalDraws(1).Length == 2 && s.V(0).HandCount == 16 &&
            s.Events<CardsDrawnEvent>().Where(e => e.PlayerSeat == 0).Sum(e => e.Count) == 4 &&
            Tactics.All(skill => s.Resolved(skill) == 2),
            "The next real own turn repeats each tier once, obtaining six new physical tricks while both ordinary draw phases remain exactly two cards.");
    }

    private static bool IsTrickGain(CardMovementRecord move) => move.From == CardLocation.DrawPile &&
        move.To == CardLocation.Hand(0) && move.CardKind is CardKind.Duel or CardKind.DrawTwo;
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0).Players[seat];
        public IEnumerable<T> Events<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public DeckCardsObtainedEvent[] GainEvents => Events<DeckCardsObtainedEvent>().Where(e => e.OwnerSeat == 0).ToArray();
        public CardMovementRecord[] GainMoves => G.CardMovements.Where(move =>
            move.To == CardLocation.Hand(0) && move.Reason.Value == GainReason).ToArray();
        public CardMovementRecord[] NormalDraws(int seat) => G.CardMovements.Where(move =>
            move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(seat) && move.Reason == CardMoveReasons.Draw).ToArray();
        public int Resolved(string skill) => Events<ProgramBindingResolvedEvent>().Count(e =>
            e.SkillId == skill && e.OwnerSeat == 0 && e.Activated && e.Completed &&
            e.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow);
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected real Bowen fixture command."); }
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 160; step++)
            {
                if (done()) return;
                Need(G.State.Status != EngineStatus.Completed && P is not { PlayerSeat: 0 },
                    "Unexpected boundary in the small real Bowen battle: " + JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded real Bowen turn boundary: " + JsonSerializer.Serialize(P));
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheBowenTacticsPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture:bowen-tactics", Owner, 20, 0, 0, 0, 0, [], [], [],
                [new(Enemy) { StartingHand = [] }], [], new string('b', 64)) { TacticSkillIds = Tactics };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = true,
                AdvanceAfterHumanCommands = false, MaxTurns = 6, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Accept(new StartGameCommand());
            scene.Until(() => scene.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-bowen-tactics", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Quiet] = new { name = "安静守军", description = "跳过出牌阶段。" } }
            }));
            builder.AddSkill(new(Quiet, "安静守军", "跳过出牌阶段。")
                { Program = catalog.Programs[Quiet], ProgramPresentation = catalog.Presentations[Quiet] });
            builder.AddGeneral(new(Owner, "博闻使用者", "supporter", "standard:none", "qun", 20));
            builder.AddGeneral(new(Enemy, "安静守军", "supporter", Quiet, "qun", 20));
            string[] kinds = ["standard:duel", "standard:slash", "standard:draw_two", "standard:dodge"];
            builder.AddDeck(new(Deck, "混合真实牌堆", 0, 2, [])
            { PhysicalCards = Enumerable.Range(0, 80).Select(index => new ContentDeckPhysicalCard(kinds[index % kinds.Length],
                (Suit)(index % 4), index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "博闻实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
