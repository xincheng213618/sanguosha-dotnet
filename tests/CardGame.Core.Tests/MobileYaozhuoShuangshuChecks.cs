using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileYaozhuoShuangshuChecks
{
    private const string Owner = "fixture:yaozhuo-owner", Peer = "fixture:yaozhuo-peer";
    private const string Yaozhuo = "mobile:yaozhuo", Sisters = "happy:shen-da-qiao-xiao-qiao", Shuangshu = "happy:shuangshu";
    private const string XuHuang = "boundary:xu-huang", Jiezi = "boundary:jiezi-current";
    private const string JieziDrawReason = "program.least-hand-marker.draw";
    private const string Deck = "fixture:yaozhuo-shuangshu-deck";

    internal static void WonYaozhuoSkipsInsertedDrawBeforeNaturalDrawCold() => Run(withJiezi: false);

    internal static void WonYaozhuoReturnsThroughJieziBeforeNaturalDrawCold() => Run(withJiezi: true);

    private static void Run(bool withJiezi)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(withJiezi));
        var peer = new ShanheCombatantDefinition(Peer) { StartingHand = [] };
        var setup = new ShanheBattleSetup(47, 1, "yaozhuo-shuangshu", withJiezi ? XuHuang : Owner, 4, 0, 0, 0, 0,
            withJiezi ? [Yaozhuo] : [],
            [new(CardKind.Dodge, Suit.Heart, 13)], [], [peer, peer],
            [new(Sisters) { StartingHand = [new(CardKind.Dodge, Suit.Club, 2)] }], new string('a', 64))
            { DefaultGodFactionId = "wu", ExtraSkillSlots = withJiezi ? 1 : 0 };
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
        var jieziFrameId = withJiezi ? ObserveJiezi(target) : (long?)null;
        Reach(TargetPlayStarted);

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

        if (jieziFrameId is { } observerId)
        {
            var benefits = history.OfType<ProgramLeastHandMarkerOrDrawEvent>().Where(fact => fact.SkillId == Jiezi).ToArray();
            var gained = game.CardMovements.Where(move => move.Reason.Value == JieziDrawReason).ToArray();
            Need(benefits is [{ OwnerSeat: 0, TargetSeat: 0, GrantedMarker: false } benefit] && benefit.FrameId == observerId &&
                gained is [var card] && card.TurnNumber == turn && card.From == CardLocation.DrawPile && card.To == CardLocation.Hand(0) &&
                card.Sequence < drawn[0].Sequence &&
                Array.IndexOf(history, skipBoundaries[0]) < Array.IndexOf(history, benefit) && Array.IndexOf(history, benefit) < Array.IndexOf(history, schedule[1]),
                "The restored Jiezi selection draws one physical card once before the same Shuangshu phase closes and the natural Draw occurs.");
            Need(history.OfType<ProgramBindingResolvedEvent>().Count(fact => fact.FrameId == observerId && fact.SkillId == Jiezi &&
                fact.OwnerSeat == 0 && fact.Window == SkillProgramTriggerWindow.DrawPhaseSkipped && fact.Activated && fact.Completed) == 1,
                "The original skipped Draw window sees exactly one completed Jiezi binding.");
        }

        bool TargetPlayStarted() => game.State.CurrentSeat == target && game.State.Phase == TurnPhase.Play;
        PendingDecision? HumanPrompt() => game.CreateSnapshot(0).PendingDecision;

        long ObserveJiezi(int skippedSeat)
        {
            Reach(() => HumanPrompt()?.SkillPrompt?.SkillId == Jiezi || TargetPlayStarted());
            Need(HumanPrompt() is { PlayerSeat: 0, SkillPrompt.SkillId: Jiezi } &&
                HumanPrompt()!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"),
                "The real boundary Xu Huang must receive Jiezi during the skipped inserted Draw, before the sisters' natural Play.");
            Choose("activate");
            Reach(() => HumanPrompt()?.SkillPrompt?.SkillId == Jiezi || TargetPlayStarted());
            var prompt = HumanPrompt();
            Need(prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Jiezi } && prompt.Choices.Any(choice =>
                choice.Parameters.GetValueOrDefault("program-action") == "select-targets" && choice.Targets.SequenceEqual([0])),
                "Activated Jiezi publishes its original target selection for the real Draw1 branch.");

            var turnNumber = game.State.TurnNumber;
            var facts = game.Events.Select(item => item.Payload).ToArray();
            var started = facts.OfType<ProgramPhaseScheduledEvent>().Single(fact => fact.SkillId == Shuangshu &&
                fact.OwnerSeat == skippedSeat && fact.Phase == TurnPhase.Draw && fact.Started);
            var skipped = facts.OfType<ActualPhaseSkippedEvent>().Single(fact => fact.TurnNumber == turnNumber &&
                fact.ActorSeat == skippedSeat && fact.Kind == ActualDiscardRecoveryPhaseKind.Draw);
            var obligation = game.ResolutionStack.OfType<DrawPhaseObligationFrame>().Single();
            var window = game.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(frame => frame.Id == obligation.ActiveWindowFrameId);
            var observer = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Jiezi);
            var players = game.CreateSnapshot(0).Players;
            Need(players[0].GeneralId == XuHuang && players[0].HandCount > players.Min(player => player.HandCount) &&
                skipped.IsExtra && skipped.NativeFrameId == started.FrameId && obligation.PhaseOccurrenceFrameId == started.FrameId &&
                obligation is { IsExtra: true, Skipped: true } && obligation.OwnerSeat == skippedSeat && obligation.ActualTurnNumber == turnNumber &&
                window is { Window: SkillProgramTriggerWindow.DrawPhaseSkipped, Continuation: ProgramLifecycleContinuation.ResumeDrawPhaseObligation } &&
                window.ResumeDrawPhaseObligationFrameId == obligation.Id && observer.OwnerSeat == 0 && observer.WindowContext?.ParentFrameId == window.Id &&
                facts.OfType<DrawPhaseSkippedEvent>().Single(fact => fact.ActualTurnNumber == turnNumber && fact.OwnerSeat == skippedSeat).FrameId == obligation.Id &&
                !facts.OfType<ProgramLeastHandMarkerOrDrawEvent>().Any(fact => fact.SkillId == Jiezi),
                "The unpaid target choice belongs to Jiezi's observer of the same exact Shuangshu schedule and skipped Draw obligation.");

            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Need(HumanPrompt()?.PromptId == prompt!.PromptId && game.ResolutionStack.OfType<DrawPhaseObligationFrame>().Single() is { } restored &&
                restored.Id == obligation.Id && restored.ActiveWindowFrameId == window.Id && restored.PhaseOccurrenceFrameId == started.FrameId &&
                game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Jiezi).Id == observer.Id &&
                game.Events.Select(item => item.Payload).OfType<ActualPhaseSkippedEvent>().Count(fact => fact == skipped) == 1,
                "Cold restore keeps the original skip fact, observer return and exact target choice open on that schedule.");
            Choose("select-targets", 0);
            return observer.Id;
        }

        void Choose(string action, int? chosenSeat = null)
        {
            var prompt = HumanPrompt()!;
            var choice = prompt.Choices.Single(option => option.Parameters.GetValueOrDefault("program-action") == action &&
                (chosenSeat is null || option.Targets.SequenceEqual([chosenSeat.Value])));
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));
        }

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

    private sealed class Fixture(bool withJiezi) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-yaozhuo-shuangshu", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            // Li Yan's registered Duliang makes the native phase journal observable without a participant using it.
            CharacterFixtureContent.Register(builder, Sisters, "classic:li-yan", "mobile:li-yan", "mobile:shi-chang-shi");
            if (withJiezi) CharacterFixtureContent.Register(builder, XuHuang);
            builder.AddGeneral(new(Owner, "谣诼使用者", "", Yaozhuo, "wei", 4));
            builder.AddGeneral(new(Peer, "固定同桌", "", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定闪牌堆", 0, 2, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "谣诼遇双姝", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: withJiezi ? [XuHuang, Sisters, Peer] : [Owner, Sisters, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
