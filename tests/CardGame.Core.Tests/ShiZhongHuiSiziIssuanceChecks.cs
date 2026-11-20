using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class ShiZhongHuiSiziIssuanceChecks
{
    private const string Hero = "mobile:shi-zhong-hui", Sizi = "mobile:shi-sizi";
    private const string Driver = "fixture:sizi-issued-health", Peer = "fixture:sizi-issued-peer", Deck = "fixture:sizi-issued-deck";
    private const string EndingDraw = "skill-program.mobile:shi-sizi.ending-draw";

    public static void OnlyThePayingSiziHolderSettlesItsFirstActualTurnCold()
    {
        var (game, content) = Start();
        Settle(game);
        var turn = game.State.TurnNumber;
        Require(Facts<TurnStartedEvent>(game).Last() == new TurnStartedEvent(turn, 0) &&
            game.CreateSnapshot(1).Players[1].Skills?.Any(skill => skill.ContentId == Sizi) == true,
            "The first actual turn belongs to Shi Zhong Hui, with a separate living Sizi holder at seat one.");

        Accept(game, new UseProgramSkillCommand(0, Driver, "lose", [], [], game.Revision, P(game)!.PromptId));
        Settle(game);
        var xiezhiResolved = Facts<PlayerMarkerChangedEvent>(game).Any(entry => entry.PlayerSeat == 0 && entry.Reason == "skill-program.mobile:shi-xiezhi.gain") ||
            Facts<ShiZhHuiXiezhiOverflowEvent>(game).Any(entry => entry.OwnerSeat == 0);
        Require(Facts<ProgramSkillHpLostEvent>(game).Single(entry => entry.SkillId == Driver) is { TargetSeat: 0, Amount: 2, RemainingHp: 2 } &&
            xiezhiResolved,
            "Actual loss of two HP enters Xiezhi and makes enough native charge before the Sizi payment.");
        Accept(game, new UseProgramSkillCommand(0, Sizi, "burn-charge", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Sizi } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("count") == "2"));
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)) &&
            !Facts<ShiZhHuiSiziActivatedEvent>(restored).Any(), "Cold recovery keeps the same unpaid native Sizi choice.");

        game = restored;
        Answer(game, choice => choice.Parameters.GetValueOrDefault("count") == "2");
        Settle(game);
        var activations = Facts<ShiZhHuiSiziActivatedEvent>(game).ToArray();
        Require(activations is [{ Source.OwnerSeat: 0, Consumed: 2, OwnerHp: 2 }] && activations[0].ActualTurn == turn &&
            Facts<PlayerMarkerChangedEvent>(game).Count(entry => entry is { PlayerSeat: 0, Delta: -2, Reason: "skill-program.mobile:shi-sizi.consume" }) == 1,
            "Only seat zero really pays two charge in its original actual turn; seat one only holds the skill.");
        var beforeEnding = game.CardMovements.Count;
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        for (var step = 0; step < 100 && !Facts<TurnStartedEvent>(game).Any(entry => entry.TurnNumber == turn + 1); step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        Require(Facts<TurnEndedEvent>(game).Any(entry => entry.TurnNumber == turn && entry.ActorSeat == 0) &&
            Facts<TurnStartedEvent>(game).Any(entry => entry.TurnNumber == turn + 1 && entry.ActorSeat == 1),
            "The already paid effect finishes the first actual ending before the second holder can start a turn.");

        var draws = game.CardMovements.Skip(beforeEnding).Where(move => move.Reason.Value == EndingDraw).ToArray();
        Require(draws.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) == 2,
            "The original payer really draws the two cards for the completed first ending.");
        var elapsed = Facts<ShiZhHuiSiziElapsedEvent>(game).Where(entry => entry.ActivationOrdinal == activations[0].Ordinal).ToArray();
        Require(draws.Length == 2 && elapsed is [{ Elapsed: 1 }] &&
            !Facts<ShiZhHuiSiziExpiredEvent>(game).Any(entry => entry.ActivationOrdinal == activations[0].Ordinal),
            $"Only the payer can draw on the first ending of its two-turn Sizi. Draw seats: [{string.Join(", ", draws.Select(move => move.To.OwnerSeat))}]. Elapsed: [{string.Join(", ", elapsed.Select(entry => entry.Elapsed))}].");
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(617, 1, "sizi-issued-ending", Hero, 4, 0, 0, 0, 0,
            [Driver], [], [], [new(Peer, Hp: 4, ExtraSkillIds: [Sizi]) { StartingHand = [] },
                new(Peer, Hp: 4) { StartingHand = [] }, new(Peer, Hp: 4) { StartingHand = [] }], [], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-sizi-issued-ending", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
              "activations":[{"id":"lose","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,
                "targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"loseHp","target":"owner","amount":2}]}]}]}
            """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "真实体力支付", description = "失去2点体力。" } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Driver, "真实体力支付", "失去2点体力。")
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Peer, "同桌角色", "", "standard:none", "qun", 4));
            builder.AddDeck(new(Deck, "固定实体闪牌堆", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 70)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "肆恣原收费者", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
