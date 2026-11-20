using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlLuYuShengRunweiHandLimitChecks
{
    private const string Hero = "ol:lu-yu-sheng", Runwei = "ol:runwei";
    private const string Peer = "fixture:runwei-hand-limit-peer", Deck = "fixture:runwei-hand-limit-deck";
    private const string RunweiDraw = "skill-program.lu-yu-sheng.draw";

    internal static void ForeignDrawReducesOnlyOriginalDiscardHandLimitCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var peerHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Dodge, Suit.Club, 2), 4).ToArray();
        var setup = new ShanheBattleSetup(31, 1, "runwei-hand-limit", Hero, 3, 0, 0, 0, 0, [], [], [],
            [new(Peer, Hp: 3) { StartingHand = peerHand }], [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5
        }, registry);

        Submit(game, new StartGameCommand());
        EndOwnPlay(game);
        Reach(game, () => OptionalRunwei(game));
        var firstTurn = game.State.TurnNumber;
        var originalHand = SnapshotHand(game, 1);
        var sourceHand = SnapshotHand(game, 0);
        Require(game.State.CurrentSeat == 1 && game.State.Phase == TurnPhase.Discard &&
            game.CreateSnapshot(1).Players[1] is { Hp: 3, MaxHp: 4 } && originalHand.Count == 6 &&
            MovesIntoHand(game, firstTurn, 1, CardMoveReasons.Draw.Value).Length == 2,
            "The wounded recipient reaches its original Discard after four opening cards and two actual normal draws.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Reach(game, () => Prompt(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option") == "draw") == true);

        var original = game;
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == Runwei);
        var phase = frame.LuYuSheng?.Phase;
        Require(frame.OwnerSeat == 0 && frame.TriggerId == "foreign-discard" &&
            frame.LuYuSheng is { Stage: LuYuShengStage.RunweiChoice, Payer: 1, Branch: null } &&
            phase is { Kind: ActualDiscardRecoveryPhaseKind.Discard, ActorSeat: 1, ActualTurnOwnerSeat: 1 } &&
            phase.TurnNumber == firstTurn && Facts<ActualDiscardRecoveryPhaseStartedEvent>(game).Count(fact => fact.Phase == phase) == 1 &&
            game.CreateSnapshot(1).PendingDecision is null && !Facts<LuYuShengRunweiPhaseIssuedEvent>(game).Any(),
            "Only Lu Yu Sheng chooses at the original real Discard phase, before issuing a consequence to its actor.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(original.CreateCheckpoint())), registry);
        Require(SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(original.CreateSnapshot(0)) &&
            SnapshotJson.Serialize(game.CreateSnapshot(1)) == SnapshotJson.Serialize(original.CreateSnapshot(1)) &&
            JsonSerializer.Serialize(game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.Id == frame.Id).LuYuSheng) ==
                JsonSerializer.Serialize(frame.LuYuSheng) && game.CardMovements.SequenceEqual(original.CardMovements),
            "Cold restore preserves the original foreign phase, private payer scope and unpaid branch.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == "draw");
        Reach(game, () => TurnEnded(game, firstTurn));

        var grant = Facts<LuYuShengRunweiPhaseIssuedEvent>(game).Single();
        var gained = MovesIntoHand(game, firstTurn, 1, RunweiDraw).Single();
        var recordedMove = Facts<LuYuShengMovementIssuedEvent>(game).Single(fact => fact.FrameId == frame.Id);
        Require(grant.FrameId == frame.Id && grant.Phase == phase && grant.Delta == -1 && grant.Source == frame.LuYuSheng!.Source &&
            Facts<LuYuShengRunweiChosenEvent>(game).Single() == new LuYuShengRunweiChosenEvent(frame.Id, 1, "draw") &&
            recordedMove is { Count: 1, Payer: 1 } && recordedMove.Before < gained.Sequence && gained.Sequence <= recordedMove.After &&
            originalHand.Add(gained.CardId) && sourceHand.SetEquals(SnapshotHand(game, 0)),
            "The original issuer's choice moves one distinct physical card to the recipient and binds one minus-one grant to that phase.");
        CheckDiscard(game, firstTurn, originalHand, keptCount: 2);

        EndOwnPlay(game);
        Reach(game, () => OptionalRunwei(game));
        var nextTurn = game.State.TurnNumber;
        var nextHand = SnapshotHand(game, 1);
        var nextPhase = Facts<ActualDiscardRecoveryPhaseStartedEvent>(game).Single(fact =>
            fact.Phase is { ActorSeat: 1, Kind: ActualDiscardRecoveryPhaseKind.Discard } && fact.Phase.TurnNumber == nextTurn).Phase;
        Require(nextTurn > firstTurn && nextPhase != phase && nextHand.Count == 4 &&
            MovesIntoHand(game, nextTurn, 1, CardMoveReasons.Draw.Value).Length == 2,
            "The next actual Discard starts at a different phase with the two retained and two newly drawn cards.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") != "activate");
        Reach(game, () => TurnEnded(game, nextTurn));
        CheckDiscard(game, nextTurn, nextHand, keptCount: 3);
        Require(Facts<LuYuShengRunweiPhaseIssuedEvent>(game).SequenceEqual([grant]) && MovesIntoHand(game, nextTurn, 1, RunweiDraw).Length == 0,
            "The previous phase consequence does not run or reduce the next actual Discard after Lu Yu Sheng declines.");
    }

    private static void CheckDiscard(GameEngine game, int turn, HashSet<int> before, int keptCount)
    {
        var moved = game.CardMovements.Where(move => move.TurnNumber == turn && move.Reason == CardMoveReasons.HandLimitDiscard &&
            move.From == CardLocation.Hand(1) && move.To == CardLocation.DiscardPile).Select(move => move.CardId).ToArray();
        var kept = SnapshotHand(game, 1);
        Require(moved.Length == before.Count - keptCount && moved.Distinct().Count() == moved.Length && moved.All(before.Contains) &&
            kept.Count == keptCount && kept.SetEquals(before.Except(moved)),
            $"The native Discard in turn {turn} pays precisely its required original entities and keeps exactly {keptCount} others.");
    }

    private static PendingDecision? Prompt(GameEngine game) => game.CreateSnapshot(0).PendingDecision;
    private static bool OptionalRunwei(GameEngine game) => Prompt(game) is { PlayerSeat: 0 } prompt &&
        prompt.SkillPrompt?.SkillId == Runwei && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    private static bool TurnEnded(GameEngine game, int turn) => Facts<TurnEndedEvent>(game).Any(fact => fact.ActorSeat == 1 && fact.TurnNumber == turn);
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static HashSet<int> SnapshotHand(GameEngine game, int seat) => game.CreateSnapshot(seat).Players[seat].Hand.Select(card => card.Id).ToHashSet();
    private static CardMovementRecord[] MovesIntoHand(GameEngine game, int turn, int seat, string reason) => game.CardMovements.Where(move =>
        move.TurnNumber == turn && move.Reason.Value == reason && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(seat)).ToArray();

    private static void EndOwnPlay(GameEngine game)
    {
        Reach(game, () => Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && game.ResolutionStack.Count == 0);
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, Prompt(game)!.PromptId));
    }

    private static void Reach(GameEngine game, Func<bool> ready)
    {
        for (var step = 0; step < 100 && !ready(); step++)
        {
            Require(Prompt(game) is null && game.State.Winner == Winner.None,
                $"Unexpected Runwei fixture boundary in turn {game.State.TurnNumber}, phase {game.State.Phase}.");
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        Require(ready(), "The fixed Runwei fixture did not reach its expected actual phase.");
    }

    private static void Answer(GameEngine game, Func<PromptChoice, bool> pick)
    {
        var prompt = Prompt(game)!;
        Submit(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(pick).Id, game.Revision));
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "The Runwei fixture command failed.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:lu-yu-sheng-runwei-hand-limit", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Peer, "受伤同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "润微固定用牌", 0, 2, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2), 28).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "润微实际弃牌", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
