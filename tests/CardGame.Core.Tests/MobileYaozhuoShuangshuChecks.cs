using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileYaozhuoShuangshuChecks
{
    private const string Owner = "fixture:yaozhuo-owner", Peer = "fixture:yaozhuo-peer";
    private const string Yaozhuo = "mobile:yaozhuo", Sisters = "happy:shen-da-qiao-xiao-qiao", Shuangshu = "happy:shuangshu";
    private const string Deck = "fixture:yaozhuo-shuangshu-deck";

    internal static void WonYaozhuoSkipsInsertedDrawBeforeNaturalDrawCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var peer = new ShanheCombatantDefinition(Peer) { StartingHand = [] };
        var setup = new ShanheBattleSetup(47, 1, "yaozhuo-shuangshu", Owner, 4, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Heart, 13)], [], [peer, peer],
            [new(Sisters) { StartingHand = [new(CardKind.Dodge, Suit.Club, 2)] }], new string('a', 64))
            { DefaultGodFactionId = "wu" };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        Submit(new StartGameCommand());
        Reach(() => HumanPrompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        var target = game.CreateSnapshot(0).Players.Single(player => player.GeneralId == Sisters).Seat;
        var cost = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Rank == 13).Id;
        var opposed = game.CreateSnapshot(target).Players[target].Hand.Single().Id;
        Submit(new UseProgramSkillCommand(0, Yaozhuo, "contest", [cost], [target], game.Revision, HumanPrompt()!.PromptId));
        Reach(() => HumanPrompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        var result = game.Events.Select(item => item.Payload).OfType<PindianResultDeterminedEvent>().Single();
        Need(result.SkillId == Yaozhuo && result.Result.SourceSeat == 0 && result.Result.OpponentSeat == target && result.Result.SourceWon &&
            result.Result.SourceCardId == cost && result.Result.OpponentCardId == opposed,
            "The real Yaozhuo activation wins its native Pindian against the Shuangshu owner.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Submit(new EndPlayPhaseCommand(0, game.Revision, HumanPrompt()!.PromptId));
        Reach(() => game.State.CurrentSeat == target && game.State.Phase == TurnPhase.Play);

        var turn = game.State.TurnNumber;
        var history = game.Events.Select(item => item.Payload).ToArray();
        var skippedPhases = history.OfType<ActualPhaseSkippedEvent>().Where(fact => fact.TurnNumber == turn &&
            fact.ActorSeat == target && fact.Kind == ActualDiscardRecoveryPhaseKind.Draw).ToArray();
        var schedule = history.OfType<ProgramPhaseScheduledEvent>().Where(fact => fact.SkillId == Shuangshu &&
            fact.OwnerSeat == target && fact.Phase == TurnPhase.Draw).ToArray();
        var skipBoundaries = history.OfType<DrawPhaseSkippedEvent>().Where(fact => fact.ActualTurnNumber == turn &&
            fact.OwnerSeat == target).ToArray();
        Need(schedule is [{ Started: true } start, { Started: false } end] && start.FrameId == end.FrameId &&
            skippedPhases is [{ IsExtra: true } skipped] && skipped.NativeFrameId == start.FrameId &&
            skipBoundaries is [{ IsExtra: true } skippedBoundary] &&
            Array.IndexOf(history, start) < Array.IndexOf(history, skipped) &&
            Array.IndexOf(history, skipped) < Array.IndexOf(history, skippedBoundary) && Array.IndexOf(history, skippedBoundary) < Array.IndexOf(history, end),
            "The Yaozhuo skip must belong to the inserted Shuangshu Draw. Observed skipped Draw frame IDs were " +
            string.Join(", ", skippedPhases.Select(fact => fact.NativeFrameId?.ToString() ?? "normal")) + ".");

        var phases = history.OfType<ActualDiscardRecoveryPhaseStartedEvent>().Where(fact =>
            fact.Phase.TurnNumber == turn && fact.Phase.ActorSeat == target).ToArray();
        var preparations = phases.Where(fact => fact.Phase.Kind == ActualDiscardRecoveryPhaseKind.Preparation).ToArray();
        var resumedPreparations = history.OfType<ActualDiscardRecoveryPhaseRestoredEvent>().Where(fact =>
            fact.ProducerFrameId == schedule[0].FrameId && fact.Phase.Kind == ActualDiscardRecoveryPhaseKind.Preparation).ToArray();
        var judgments = phases.Where(fact => fact.Phase.Kind == ActualDiscardRecoveryPhaseKind.Judgment).ToArray();
        var draws = phases.Where(fact => fact.Phase.Kind == ActualDiscardRecoveryPhaseKind.Draw).ToArray();
        var drawn = game.CardMovements.Where(move => move.TurnNumber == turn && move.From == CardLocation.DrawPile &&
            move.To == CardLocation.Hand(target) && move.Reason == CardMoveReasons.Draw).ToArray();
        var handCount = game.CreateSnapshot(target).Players[target].HandCount;

        Need(preparations is [var preparation] && resumedPreparations is [var resumed] && resumed.Phase == preparation.Phase &&
            judgments is [var judgment] && draws is [var draw] &&
            Array.IndexOf(history, schedule[1]) < Array.IndexOf(history, resumed) &&
            Array.IndexOf(history, resumed) < Array.IndexOf(history, judgment) && Array.IndexOf(history, judgment) < Array.IndexOf(history, draw) &&
            drawn is [_, _] && drawn.All(move => move.Sequence > draw.MovementSequenceBefore) &&
            handCount == 2,
            $"The only actual Draw entry and its two physical cards belong to the normal phase after Preparation. " +
            $"Schedule ended at {Array.IndexOf(history, schedule[1])}, phases {string.Join(", ", phases.Select(fact => $"{fact.Phase.Kind}@{Array.IndexOf(history, fact)}:{fact.MovementSequenceBefore}"))}; " +
            $"draw movement sequences {string.Join(", ", drawn.Select(move => move.Sequence))}; hand count {handCount}.");

        PendingDecision? HumanPrompt() => game.CreateSnapshot(0).PendingDecision;

        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 140 && !ready(); step++)
            {
                Need(HumanPrompt() is null && game.State.Winner == Winner.None, "The fixture reached an unexpected public command boundary.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The fixture did not reach the expected Pindian or actual turn boundary.");
        }

        void Submit(GameCommand command)
        {
            var accepted = game.Submit(command);
            Need(accepted.Accepted, accepted.Error?.Message ?? "Rejected a public fixture command.");
        }
    }

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-yaozhuo-shuangshu", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            // Li Yan's registered Duliang makes the native phase journal observable without a participant using it.
            CharacterFixtureContent.Register(builder, Sisters, "classic:li-yan", "mobile:li-yan", "mobile:shi-chang-shi");
            builder.AddGeneral(new(Owner, "谣诼使用者", "", Yaozhuo, "wei", 4));
            builder.AddGeneral(new(Peer, "固定同桌", "", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定闪牌堆", 0, 2, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "谣诼遇双姝", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Sisters, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
