#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangShiPingOlRuntime
{
    internal bool TryBeginZhangShiPingHongjiPhase(ProgramSkillFrame frame, TurnPhase phase, int recipientSeat)
    {
        if (!HasHongjiCapability || frame.ZhangShiPingHongji is not { Stage: HongjiStage.ExtraPhase, Promise: { } promise } receipt || receipt.Phase != phase || phase is not (TurnPhase.Draw or TurnPhase.Play) || receipt.RecipientSeat != recipientSeat || recipientSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || recipientSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || receipt.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[recipientSeat].IsAlive || receipt.SuspendedSchedule != _programPhaseScheduleOperations._programPhaseSchedule || receipt.ReturnPhase != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase || receipt.ReturnActualPhase != _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase() || receipt.ReturnActualPhase is not { } actual || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(actual) || actual.ActorSeat != recipientSeat || actual.Kind != (phase == TurnPhase.Draw ? ActualDiscardRecoveryPhaseKind.Draw : ActualDiscardRecoveryPhaseKind.Play) || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^1] != frame || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != receipt.ParentWindowId || frame.WindowContext?.ParentFrameId != parent.Id || parent.OwnerSeat != recipientSeat || parent.Window != (phase == TurnPhase.Draw ? SkillProgramTriggerWindow.DrawPhaseEnded : SkillProgramTriggerWindow.PlayEnding) || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhangShiPingHongjiConsumedEvent>().Any(e => e.FrameId == frame.Id && e.PromiseFrameId == promise.FrameId && e.Source == receipt.Source && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.RecipientSeat == recipientSeat && e.Phase == phase && e.ParentWindowId == parent.Id))
            return false;
        var drawParents = _resolutionStackOperations._resolutionStack.Take(_resolutionStackOperations._resolutionStack.Count - 2).ToArray();
        if (drawParents.Length > 0 && (phase != TurnPhase.Draw || drawParents.Any(parentFrame => parentFrame is not DrawPhaseObligationFrame) || !_coreDrawPhaseObligationsOperations.Value.HasDrawPhaseObligationBoundaryFrame() || parent.ResumeDrawPhaseObligationFrameId != drawParents[^1].Id))
            return false;
        receipt = receipt with
        {
            DrawPhaseParents = drawParents.Cast<DrawPhaseObligationFrame>().ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ZhangShiPingHongji = receipt });
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ProgramSkill);
        _coreRuntimeOperations.PopResolutionFrame(parent.Id, ResolutionFrameKind.ProgramLifecycleTriggerWindow);
        foreach (var drawParent in receipt.DrawPhaseParents.Reverse())
            _coreRuntimeOperations.PopResolutionFrame(drawParent.Id, ResolutionFrameKind.DrawPhaseObligation);
        _programPhaseScheduleOperations._programPhaseSchedule = new(frame, parent, phase, SkillProgramPhaseContinuation.BeforeNormalPreparation)
        {
            DiscardRecoveryReturnPhase = receipt.ReturnActualPhase,
            AfterTurnEndedReturn = receipt.SuspendedSchedule?.AfterTurnEndedReturn
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPhaseScheduledEvent(frame.Id, frame.SkillId, frame.TriggerId!, frame.OwnerSeat, phase, Started: true));
        if (phase == TurnPhase.Play)
            _coreActualTurnsOperations.EnterPlayPhase(_coreMatchParticipantsPlayersAccess._players[recipientSeat]);
        else
        {
            _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase = TurnPhase.Draw;
            _coreActualDiscardRecoveryPhasesOperations.StartActualDiscardRecoveryPhase(ActualDiscardRecoveryPhaseKind.Draw, recipientSeat);
            if (!_coreProgramLifecycleOperations.TryBeginDrawPhaseProgramWindow(_coreMatchParticipantsPlayersAccess._players[recipientSeat], false))
            {
                _characterDuFuRenOlOperations.ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind.Draw, recipientSeat);
                _characterChengGongYingOlOperations.ConsumeChengGongYingDraw(recipientSeat);
                _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[recipientSeat], _characterCaoXiOlOperations.FreezeAndCaptureCaoXiNativeDrawBase(_coreMatchParticipantsPlayersAccess._players[recipientSeat]), true);
                _coreActualTurnsOperations.CompleteTurnStartAfterDraw(_coreMatchParticipantsPlayersAccess._players[recipientSeat], GameEngine.DelayedTurnEffects.None);
            }
        }

        return true;
    }

    internal bool TryResumeZhangShiPingHongjiPhase()
    {
        if (_programPhaseScheduleOperations._programPhaseSchedule is not { Frame.ZhangShiPingHongji: { Stage: HongjiStage.ExtraPhase } receipt } schedule)
            return false;
        var frame = schedule.Frame;
        if (schedule.ParentFrame is not { } parent || receipt.ParentWindowId != parent.Id || receipt.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || receipt.RecipientSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || receipt.RecipientSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || schedule.Phase != receipt.Phase || _resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || receipt.ReturnActualPhase is not { } original || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(original) || original.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || original.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || original.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat)
            throw new InvalidOperationException("Hongji lost its exact completed inserted phase and native parent return.");
        _programPhaseScheduleOperations._programPhaseSchedule = receipt.SuspendedSchedule;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPhaseScheduledEvent(frame.Id, frame.SkillId, frame.TriggerId!, frame.OwnerSeat, schedule.Phase, Started: false));
        if (receipt.DrawPhaseParents.Count > 0)
        {
            if (receipt.Phase != TurnPhase.Draw || parent.ResumeDrawPhaseObligationFrameId != receipt.DrawPhaseParents[^1].Id)
                throw new InvalidOperationException("Hongji lost its frozen actual draw producer chain.");
            _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase = receipt.ReturnPhase;
            if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[receipt.RecipientSeat].IsAlive)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualDiscardRecoveryPhaseRestoredEvent(frame.Id, original));
            else
                _coreActualDiscardRecoveryPhasesOperations.ClearActualDiscardRecoveryPhase();
            foreach (var drawParent in receipt.DrawPhaseParents)
                _coreRuntimeOperations.PushRuntimeFrame(drawParent);
            _coreRuntimeOperations.PushRuntimeFrame(parent);
            _coreRuntimeOperations.PushRuntimeFrame(frame);
            ResumeZhangShiPingHongjiPhase(frame.Id);
            return true;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[receipt.RecipientSeat].IsAlive)
        {
            _coreActualDiscardRecoveryPhasesOperations.ClearActualDiscardRecoveryPhase();
            _coreProgramLifecycleOperations.CompleteDetachedProgramBinding(frame, completed: false);
            if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _programPhaseScheduleOperations._programPhaseSchedule?.AfterTurnEndedReturn is not null)
            {
                _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase = receipt.ReturnPhase;
                _coreProgramLifecycleOperations.ResumeScheduledProgramPhase();
                return true;
            }

            _coreProgramLifecycleOperations.CancelScheduledProgramPhase();
            if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
                _coreActualTurnsOperations.FinalizeEndTurn(_coreMatchParticipantsPlayersAccess._players[receipt.RecipientSeat]);
            return true;
        }

        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase = receipt.ReturnPhase;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualDiscardRecoveryPhaseRestoredEvent(frame.Id, original));
        _coreRuntimeOperations.PushRuntimeFrame(parent);
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        ResumeZhangShiPingHongjiPhase(frame.Id);
        return true;
    }

    internal bool TryCancelZhangShiPingHongjiPhase()
    {
        if (_programPhaseScheduleOperations._programPhaseSchedule is not { Frame.ZhangShiPingHongji: { Stage: HongjiStage.ExtraPhase } receipt } schedule)
            return false;
        _programPhaseScheduleOperations._programPhaseSchedule = receipt.SuspendedSchedule;
        _coreActualDiscardRecoveryPhasesOperations.ClearActualDiscardRecoveryPhase();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPhaseScheduledEvent(schedule.Frame.Id, schedule.Frame.SkillId, schedule.Frame.TriggerId!, schedule.Frame.OwnerSeat, schedule.Phase, Started: false));
        _coreProgramLifecycleOperations.CompleteDetachedProgramBinding(schedule.Frame, completed: false);
        _coreProgramLifecycleOperations.CancelScheduledProgramPhase();
        return true;
    }
}
