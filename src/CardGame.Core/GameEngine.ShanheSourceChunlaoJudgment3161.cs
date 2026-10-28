namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeJudgmentPhase, 24921)]
    private bool BeginShanheSourceChunlaoJudgmentDraw()
    {
        if (_pendingTurnDelayedEffects.HasFlag(DelayedTurnEffects.SkipJudgmentPhase)) return false;
        var consumed = ProgramEventHistory<ShanheSourceChunlaoJudgmentConsumedEvent>().Select(e => e.WineUseId).ToHashSet();
        var due = ProgramEventHistory<ProgramRoundPileAlcoholIssuedEvent>().Where(e =>
            e.Source.SkillId == ShanheSourceJiangdongTrioIds.Chunlao && e.VictimSeat == _currentSeat && !consumed.Contains(e.CardUseFrameId)).ToArray();
        if (due.Length == 0 || _winner != Winner.None || !_players[_currentSeat].IsAlive) return false;
        if (_resolutionStack.Count != 0 || _pendingDecision is not null)
            throw new InvalidOperationException("Source3161 phase replacement requires the original clean post-Preparation boundary.");
        if (TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind.Preparation, ActualPhaseEndContinuation.Preparation)) return true;
        var current = _players[_currentSeat];
        _phase = TurnPhase.Draw;
        StartActualDiscardRecoveryPhase(ActualDiscardRecoveryPhaseKind.Draw, current.Seat);
        var frame = new DrawPhaseObligationFrame(++_resolutionSequence, current.Seat, _turnNumber, false,
            (int)_pendingTurnDelayedEffects, DrawPhaseObligationStage.Starting)
        {
            DiscardRecoveryPhase = CurrentActualDiscardRecoveryPhase(),
            ShanheSourceChunlaoJudgmentDraw = new(due[0].CardUseFrameId, current.Seat, _turnNumber, (int)_pendingTurnDelayedEffects)
        };
        PushRuntimeFrame(frame);
        foreach (var debt in due)
            AdvanceEventRulesAndQueueFact(new ShanheSourceChunlaoJudgmentConsumedEvent(debt.CardUseFrameId, frame.Id, current.Seat, _turnNumber));
        AddLog("PhaseChanged", $"{current.Name} 的判定阶段改为摸牌阶段。", current.Seat);
        AdvanceEventRulesAndQueueFact(new PhaseChangedEvent(TurnPhase.Draw, current.Seat));
        AdvanceRuntimeFrame(frame.Id); return true;
    }

    // Called only after the exact owned Draw and all of its native children
    // have completed and its frame has been removed by the normal Draw host.
    private bool TryCompleteShanheSourceChunlaoJudgmentDraw(DrawPhaseObligationFrame frame)
    {
        if (frame.ShanheSourceChunlaoJudgmentDraw is not { } r) return false;
        if (r.OwnerSeat != frame.OwnerSeat || r.OwnerSeat != _currentSeat || r.ActualTurn != _turnNumber ||
            r.ActualTurn != frame.ActualTurnNumber || frame.ParentFrameId is not null || frame.IsExtra ||
            !ProgramEventHistory<ShanheSourceChunlaoJudgmentConsumedEvent>().Any(e => e.WineUseId == r.AnchorWineUseId &&
                e.DrawFrameId == frame.Id && e.OwnerSeat == r.OwnerSeat && e.ActualTurn == r.ActualTurn) ||
            !ProgramEventHistory<ProgramRoundPileAlcoholIssuedEvent>().Any(e => e.CardUseFrameId == r.AnchorWineUseId &&
                e.VictimSeat == r.OwnerSeat && e.Source.SkillId == ShanheSourceJiangdongTrioIds.Chunlao))
            throw new InvalidOperationException("Source3161 replacement Draw lost its exact paid Wine and normal-turn return.");
        AdvanceEventRulesAndQueueFact(new ShanheSourceChunlaoJudgmentCompletedEvent(frame.Id, r.AnchorWineUseId,
            r.OwnerSeat, r.ActualTurn, frame.DrawCount));
        CompleteTurnStart(_players[frame.OwnerSeat], _pendingTurnDelayedEffects, skippedActualJudgment: true);
        return true;
    }
}
