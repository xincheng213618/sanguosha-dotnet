using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlGaoGanJuguanDrawChecks
{
    private const string Hero = "ol:gao-gan", Juguan = "ol:juguan";
    private const string Peer = "fixture:juguan-draw-peer", Deck = "fixture:juguan-draw-deck";

    internal static void NativeDuelGrantsTheNextActualDrawOnceCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(31, 1, "juguan-draw", Hero, 4, 0, 0, 0, 0, [],
            [new(CardKind.Alcohol, Suit.Spade, 3)], [], [new(Peer) { StartingHand = [] }], [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6
        }, content);
        Submit(game, new StartGameCommand());
        var play = ReachPlay(game);
        var originalTurn = game.State.TurnNumber;
        RequireDraw(game, 2);

        var material = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Alcohol);
        var duel = game.GetLegalActions(0).Single(action => action.CardId == material.Id && action.PlayedCardKind == CardKind.Duel &&
            action.ConversionSource?.SkillId == Juguan && action.TargetSeats.SequenceEqual([1]));
        Submit(game, new PlayCardCommand(0, material.Id, duel.TargetSeats, game.Revision, play.PromptId, duel.PlayedCardKind)
            { ConversionSource = duel.ConversionSource });
        play = ReachPlay(game);
        var issued = Facts<JuguanUseIssuedEvent>(game).Single().Receipt;
        var victim = Facts<JuguanVictimDamagedEvent>(game).Single();
        Require(issued.ActualTurn == originalTurn && issued.Source == duel.ConversionSource && issued.MaterialCardId == material.Id &&
            issued.DeclaredKind == CardKind.Duel && victim is { OwnerSeat: 0, VictimSeat: 1, ActualDamageSourceSeat: 0, Amount: 1 } &&
            victim.CardUseFrameId == issued.CardUseFrameId && victim.CardActionId == issued.CardActionId &&
            game.CardMovements.Count(move => move.CardId == material.Id && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == issued.CardUseFrameId && fact.CardId == material.Id) == 1,
            "The original physical hand card pays for one native Juguan Duel and its actual damage records the original victim.");
        game = Cold(game, content);
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        play = ReachPlay(game, originalTurn);
        var rewardedTurn = game.State.TurnNumber;
        var rewardedDraw = RequireDraw(game, 4);
        Require(Facts<JuguanDeadlineSettledEvent>(game).SequenceEqual([new(0, rewardedTurn, Qualified: true)]) &&
            Facts<JuguanDrawPhaseBoundEvent>(game).SequenceEqual([new(0, rewardedTurn, rewardedDraw)]) &&
            !Facts<JuguanRetaliationRecordedEvent>(game).Any(),
            "The next actual owner turn settles the original victim at its deadline and binds the four physical draws to that Draw phase.");

        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        ReachPlay(game, rewardedTurn);
        RequireDraw(game, 2);
        Require(Facts<JuguanDeadlineSettledEvent>(game).SequenceEqual(
                [new(0, rewardedTurn, Qualified: true), new(0, game.State.TurnNumber, Qualified: false)]) &&
            Facts<JuguanDrawPhaseBoundEvent>(game).SequenceEqual([new(0, rewardedTurn, rewardedDraw)]) &&
            Facts<JuguanUseIssuedEvent>(game).Single().Receipt == issued && Facts<JuguanVictimDamagedEvent>(game).Single() == victim,
            "The original paid use grants one reward and the following actual owner Draw returns to two cards.");
        _ = Cold(game, content);
    }

    private static ActualDiscardRecoveryPhaseKey RequireDraw(GameEngine game, int count)
    {
        var turn = game.State.TurnNumber;
        var draw = Facts<ActualDiscardRecoveryPhaseStartedEvent>(game).Single(fact =>
            fact.Phase is { ActualTurnOwnerSeat: 0, ActorSeat: 0, Kind: ActualDiscardRecoveryPhaseKind.Draw } && fact.Phase.TurnNumber == turn);
        var physical = game.CardMovements.Where(move => move.Sequence > draw.MovementSequenceBefore && move.TurnNumber == turn &&
            move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw).ToArray();
        Require(physical.Length == count && physical.Select(move => move.CardId).Distinct().Count() == count,
            $"Normal Draw before Gao Gan's Play in turn {turn} must move {count} physical cards, observed {physical.Length}.");
        return draw.Phase;
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var replay = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(replay.CardMovements.SequenceEqual(game.CardMovements) &&
            Facts<JuguanUseIssuedEvent>(replay).SequenceEqual(Facts<JuguanUseIssuedEvent>(game)) &&
            Facts<JuguanVictimDamagedEvent>(replay).SequenceEqual(Facts<JuguanVictimDamagedEvent>(game)) &&
            Facts<JuguanDeadlineSettledEvent>(replay).SequenceEqual(Facts<JuguanDeadlineSettledEvent>(game)) &&
            Facts<JuguanDrawPhaseBoundEvent>(replay).SequenceEqual(Facts<JuguanDrawPhaseBoundEvent>(game)) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(replay.CreateSnapshot(seat)) ==
                SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold command replay preserves both views, the physical card trail and the original Juguan facts.");
        return replay;
    }

    private static PendingDecision ReachPlay(GameEngine game, int afterTurn = 0)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.State.TurnNumber > afterTurn && game.ResolutionStack.Count == 0 &&
                game.CreateSnapshot(0).PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } play)
                return play;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException($"The fixed Juguan fixture missed Gao Gan's Play after turn {afterTurn}.");
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "The Juguan fixture command was rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:gao-gan-juguan-draw", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Peer, "同桌", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "拒关固定普通摸牌", 0, 2, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "拒关实际摸牌", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
