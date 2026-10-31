#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceChunlaoJudgment3161Runtime
{
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeJudgmentPhase, 24921)]
    internal bool BeginShanheSourceChunlaoJudgmentDraw()
    {
        if (_coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects.HasFlag(GameEngine.DelayedTurnEffects.SkipJudgmentPhase))
            return false;
        var consumed = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceChunlaoJudgmentConsumedEvent>().Select(e => e.WineUseId).ToHashSet();
        var due = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramRoundPileAlcoholIssuedEvent>().Where(e => e.Source.SkillId == ShanheSourceJiangdongTrioIds.Chunlao && e.VictimSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && !consumed.Contains(e.CardUseFrameId)).ToArray();
        if (due.Length == 0 || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat].IsAlive)
            return false;
        if (_resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("Source3161 phase replacement requires the original clean post-Preparation boundary.");
        if (_coreActualPhaseEndsOperations.TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind.Preparation, ActualPhaseEndContinuation.Preparation))
            return true;
        var current = _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat];
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase = TurnPhase.Draw;
        _coreActualDiscardRecoveryPhasesOperations.StartActualDiscardRecoveryPhase(ActualDiscardRecoveryPhaseKind.Draw, current.Seat);
        var frame = new DrawPhaseObligationFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, current.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, false, (int)_coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects, DrawPhaseObligationStage.Starting)
        {
            DiscardRecoveryPhase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase(),
            ShanheSourceChunlaoJudgmentDraw = new(due[0].CardUseFrameId, current.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, (int)_coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects)
        };
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        foreach (var debt in due)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceChunlaoJudgmentConsumedEvent(debt.CardUseFrameId, frame.Id, current.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
        _coreMatchLogOperations.AddLog("PhaseChanged", $"{(current.Name)} 的判定阶段改为摸牌阶段。", current.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseChangedEvent(TurnPhase.Draw, current.Seat));
        _coreRuntimeOperations.AdvanceRuntimeFrame(frame.Id);
        return true;
    }

    internal bool TryCompleteShanheSourceChunlaoJudgmentDraw(DrawPhaseObligationFrame frame)
    {
        if (frame.ShanheSourceChunlaoJudgmentDraw is not { } r)
            return false;
        if (r.OwnerSeat != frame.OwnerSeat || r.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.ActualTurn != frame.ActualTurnNumber || frame.ParentFrameId is not null || frame.IsExtra || !_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceChunlaoJudgmentConsumedEvent>().Any(e => e.WineUseId == r.AnchorWineUseId && e.DrawFrameId == frame.Id && e.OwnerSeat == r.OwnerSeat && e.ActualTurn == r.ActualTurn) || !_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramRoundPileAlcoholIssuedEvent>().Any(e => e.CardUseFrameId == r.AnchorWineUseId && e.VictimSeat == r.OwnerSeat && e.Source.SkillId == ShanheSourceJiangdongTrioIds.Chunlao))
            throw new InvalidOperationException("Source3161 replacement Draw lost its exact paid Wine and normal-turn return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceChunlaoJudgmentCompletedEvent(frame.Id, r.AnchorWineUseId, r.OwnerSeat, r.ActualTurn, frame.DrawCount));
        _coreActualTurnsOperations.CompleteTurnStart(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects, skippedActualJudgment: true);
        return true;
    }
}
