#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHaoPuOlRuntime
{
    internal bool ZhenyingFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Zhenying is not { Stage: ZhenyingStage.AdjustmentChildren } r || f.PendingMovementContinuation is null || !ValidZhenying(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.PaymentBefore && m.Sequence <= r.PaymentAfter && (m.Reason.Value is ZhenyingDiscardReason or ZhenyingDrawReason || m.Reason == CardMoveReasons.Reshuffle));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? ZhenyingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.ZhenyingFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26500)]
    internal bool AdvanceZhenyingSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhenyingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
