#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiWanOlRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26720)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26720)]
    internal bool ContinueIssuedLiWan(ProgramSkillFrame f)
    {
        if (f.LiWan is not null)
        {
            return true;
        }

        return f.InstructionIndex is 0 or 1 && f.TriggerId is { } id && f.ActivationId == id && f.WindowContext is { } context && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && IsIssuedLiWanCandidate(new(f.OwnerSeat, f.SkillId, id, f.SkillInstanceId, f.GameplayHash, 0, context.OccurrenceIndex)) && CanRunIssuedLiWanCandidate(new(f.OwnerSeat, f.SkillId, id, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context);
    }

    internal bool LiWanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.LiWan is not { Stage: LiWanStage.Children } r || f.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.Reason.Value is LianjuGainReason or SilvDrawReason or SilvRefundReason || m.Reason == CardMoveReasons.Reshuffle));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        return false;
    }

    // None of the four direct operations removes equipment or recovers HP. Recovery children belong to their exact nested gain observer.
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 700)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 640)]
    internal bool IsLiWanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => false;
    internal ProgramSkillFrame? LiWanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.LiWanFirstChild(f, child), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26720)]
    internal bool AdvanceLiWanSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiWanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
