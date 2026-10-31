namespace CardGame.Core;

/// <summary>
/// Detaches an issued program and its exact parent while an inserted phase runs,
/// then restores that same paid continuation. The existing schedule is the only
/// pending state; native phase producers retain their own effects and receipts.
/// </summary>
[ProgramRuntimeModule]
internal sealed class ProgramPhaseSchedulingRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramWindowCursors cursors, IProgramTurnQueries turn, IProgramPhaseQueries phaseQueries, ISelectionParticipantQueries participants,
    IProgramPhaseSchedulingFacts facts, IProgramPhaseScheduleState state, IProgramPhaseNativeEntries entries,
    IProgramPhaseNativeReturns returns, IProgramPhasePipelineHooks hooks,
    IActualDiscardRecoveryPhaseQueries actualPhases, IActualDiscardRecoveryPhaseOperations phaseOperations)
{
    internal SkillProgramStepOutcome ScheduleProgramPhase(ProgramSkillFrame frame, TurnPhase phase,
        SkillProgramPhaseContinuation continuation, int? beneficiarySeat = null)
    {
        var playEndingGrant = frame.WindowContext?.Window == SkillProgramTriggerWindow.PlayEnding;
        if (phase is not (TurnPhase.Play or TurnPhase.Draw) || continuation != SkillProgramPhaseContinuation.BeforeNormalPreparation ||
            frame.WindowContext is not { } context ||
            context.Window is not (SkillProgramTriggerWindow.TurnStartBeforeNormalFlow or SkillProgramTriggerWindow.PlayEnding) ||
            context.OwnerSeat != frame.OwnerSeat || state.Current is not null ||
            frames.ActiveProgram(frame.Id) != frame ||
            playEndingGrant && (phase != TurnPhase.Play || beneficiarySeat is null))
            throw new InvalidOperationException("The configured phase insertion does not match a clean turn-start or play-ending boundary.");
        // A play-ending grant belongs to the living current turn player; a
        // turn-start insertion keeps inserting for the program owner itself.
        var beneficiary = playEndingGrant ? beneficiarySeat!.Value : frame.OwnerSeat;
        if (playEndingGrant && (beneficiary != turn.CurrentSeat || !participants.IsAlive(beneficiary)))
            throw new InvalidOperationException("A play-ending phase insertion requires the living current turn player.");

        if (frames.ParentOf(frame.Id) is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId)
            throw new InvalidOperationException("The inserted phase lost its lifecycle parent window.");
        frames.Pop(frame.Id, ResolutionFrameKind.ProgramSkill);
        frames.Pop(parent.Id, ResolutionFrameKind.ProgramLifecycleTriggerWindow);
        state.Current = new ProgramPhaseSchedule(frame, parent, phase, continuation)
        { DiscardRecoveryReturnPhase = playEndingGrant ? null : actualPhases.Current() };
        events.AdvanceFact(new ProgramPhaseScheduledEvent(
            frame.Id, frame.SkillId, frame.TriggerId!, frame.OwnerSeat, phase, Started: true));
        if (phase == TurnPhase.Play) entries.EnterPlayPhase(beneficiary);
        else
        {
            phaseOperations.Start(ActualDiscardRecoveryPhaseKind.Draw, frame.OwnerSeat);
            if (!entries.TryBeginDrawPhaseProgramWindow(frame.OwnerSeat))
            {
                entries.ConsumeDuFuRenDrawPhase(frame.OwnerSeat);
                entries.ConsumeChengGongYingDraw(frame.OwnerSeat);
                entries.DrawNormalCards(frame.OwnerSeat);
                entries.CompleteTurnStartAfterDraw(frame.OwnerSeat);
            }
        }
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool IsEndOwnedExtraPlayPhase() =>
        phaseQueries.Phase == TurnPhase.Play && state.Current is
            { Phase: TurnPhase.Play, Continuation: SkillProgramPhaseContinuation.AfterActualTurnEnd, ParentFrame: null,
              AfterTurnEndedReturn: { } parent, Frame: { ZhangYi: { Stage: ZhangYiStage.DianjunExtraPlay } receipt } frame } &&
        parent.TurnNumber == turn.TurnNumber && parent.OwnerSeat == turn.CurrentSeat && frame.OwnerSeat == turn.CurrentSeat &&
        receipt.ParentWindowId == parent.Id && receipt.ActualTurnNumber == turn.TurnNumber &&
        frame.WindowContext is { Window: SkillProgramTriggerWindow.AfterTurnEnded } context && context.ParentFrameId == parent.Id &&
        parent.AfterTurnEnded?.CurrentChild?.FrameId == frame.Id;

    internal void ExpireEndOwnedPlayEffects(ProgramSkillFrame frame)
    {
        if (frames.ActiveProgram(frame.Id) != frame ||
            frame.ZhangYi is not { Stage: ZhangYiStage.DianjunCleanupChildren } receipt ||
            receipt.ActualTurnNumber != turn.TurnNumber || phaseQueries.Phase != TurnPhase.Finished ||
            frames.ParentOf(frame.Id) is not DeferredTurnEndFrame parent ||
            !facts.MatchesAfterTurnEndedChild(parent, frame, frame.WindowContext!))
            throw new InvalidOperationException("An end-owned phase cleanup lost its original turn and paid child.");
        returns.ExpireActualTurnEndEffects(frame.OwnerSeat);
        returns.ExpirePlayPhaseSkillGrantsForEndedTurn(turn.TurnNumber);
        returns.ExpireActualTurnSkillGrants(turn.TurnNumber, turn.CurrentSeat);
    }

    internal bool TryBeginZhangYiDianjunExtraPlay(ProgramSkillFrame frame)
    {
        if (state.Current is not null || frame.ZhangYi is not { Stage: ZhangYiStage.DianjunExtraPlay } receipt ||
            phaseQueries.Phase != TurnPhase.Finished || frame.OwnerSeat != turn.CurrentSeat || !participants.IsAlive(frame.OwnerSeat) ||
            receipt.ActualTurnNumber != turn.TurnNumber || frames.Top != frame ||
            frames.ParentOf(frame.Id) is not DeferredTurnEndFrame parent || frames.ParentOf(parent.Id) is not null ||
            !facts.MatchesAfterTurnEndedChild(parent, frame, frame.WindowContext!)) return false;
        frames.Pop(frame.Id, ResolutionFrameKind.ProgramSkill);
        frames.Pop(parent.Id, ResolutionFrameKind.DeferredTurnEnd);
        state.Current = new ProgramPhaseSchedule(frame, null, TurnPhase.Play, SkillProgramPhaseContinuation.AfterActualTurnEnd)
        { AfterTurnEndedReturn = parent };
        events.AdvanceFact(new ProgramPhaseScheduledEvent(
            frame.Id, frame.SkillId, frame.TriggerId!, frame.OwnerSeat, TurnPhase.Play, Started: true));
        entries.EnterPlayPhase(frame.OwnerSeat);
        return true;
    }

    internal bool ResumeScheduledProgramPhase()
    {
        if (hooks.ResumeScheduledPhase()) return true;
        if (returns.TryResumeZhangShiPingHongjiPhase()) return true;
        if (state.Current is not { } schedule) return false;
        var frame = returns.FreezeGrantedEntityPhaseEnd(schedule.Frame);
        state.Current = null;
        events.AdvanceFact(new ProgramPhaseScheduledEvent(
            frame.Id, frame.SkillId, frame.TriggerId!, frame.OwnerSeat, schedule.Phase, Started: false));
        if (schedule.AfterTurnEndedReturn is { } ended)
        {
            if (schedule.ParentFrame is not null || schedule.Continuation != SkillProgramPhaseContinuation.AfterActualTurnEnd ||
                schedule.Phase != TurnPhase.Play || frames.Top is not null ||
                frame.ZhangYi is not { Stage: ZhangYiStage.DianjunExtraPlay } receipt ||
                receipt.ParentWindowId != ended.Id || receipt.ActualTurnNumber != turn.TurnNumber ||
                ended.OwnerSeat != turn.CurrentSeat || ended.TurnNumber != turn.TurnNumber)
                throw new InvalidOperationException("The completed end-owned phase lost its typed actual-turn return.");
            phaseOperations.Clear();
            state.RestorePhase(TurnPhase.Finished);
            frames.Push(ended);
            if (!facts.MatchesAfterTurnEndedChild(ended, frame, frame.WindowContext!))
                throw new InvalidOperationException("The completed phase lost its original after-turn child cursor.");
            frames.Push(frame);
            returns.ResumeZhangYiDianjunExtraPlay(frame.Id);
            return true;
        }
        var parent = schedule.ParentFrame ?? throw new InvalidOperationException("An inserted phase lost its lifecycle return.");
        if (facts.Winner != Winner.None || !participants.IsAlive(frame.OwnerSeat))
        {
            phaseOperations.Clear();
            returns.CompleteDetachedProgramBinding(frame, completed: false);
            return true;
        }
        phaseOperations.Restore(schedule);
        // Turn-start insertion temporarily entered Play. Restore its original
        // coarse phase before normal draw and after-draw triggers resume.
        if (parent.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)
            state.RestorePhase(TurnPhase.Draw);
        frames.Push(parent);
        if (!facts.HasRuntimeSkillInstance(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId) &&
            !facts.IsReturningGrantedEntityPhase(frame) && !hooks.ContinueDetachedProgramBinding(frame))
        {
            returns.CompleteDetachedProgramBinding(frame, completed: false);
            cursors.AdvanceProgramLifecycleCursor(parent);
            frames.AdvanceTop<ProgramLifecycleTriggerWindowFrame>();
            return true;
        }
        frames.Push(frame);
        frames.AdvanceProgram(frame.Id);
        return true;
    }

    internal void CancelScheduledProgramPhase()
    {
        if (hooks.CancelScheduledPhase()) return;
        if (returns.TryCancelZhangShiPingHongjiPhase()) return;
        if (state.Current is not { } schedule) return;
        state.Current = null;
        phaseOperations.Clear();
        events.AdvanceFact(new ProgramPhaseScheduledEvent(
            schedule.Frame.Id,
            schedule.Frame.SkillId,
            schedule.Frame.TriggerId!,
            schedule.Frame.OwnerSeat,
            schedule.Phase,
            Started: false));
        returns.CompleteDetachedProgramBinding(schedule.Frame, completed: false);
    }
}
