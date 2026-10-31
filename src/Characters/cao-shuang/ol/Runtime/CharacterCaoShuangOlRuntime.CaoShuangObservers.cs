#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoShuangOlRuntime
{

    internal bool CaoShuangFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.CaoShuangTuogu is null && root.CaoShuangShanzhuan is null)
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skill)
            return skill.ResumeProgramFrameId == root.Id && skill.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && CanContinuePaidCaoShuang(root);
        if (root.CaoShuangTuogu is { Granted: true } t && child is CardsMovedTriggerWindowFrame cleanup)
            return cleanup.ResumeProgramFrameId == root.Id && cleanup.Batch.ParentFrameId == root.Id && cleanup.Batch.OriginOwnerSeat == root.OwnerSeat && cleanup.Batch.OriginSkillId == root.SkillId && cleanup.Batch.OriginSkillInstanceId == root.SkillInstanceId && cleanup.Batch.Movements.Count > 0 && cleanup.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > t.SequenceBefore && m.Sequence <= t.SequenceAfter);
        if (root.CaoShuangShanzhuan is not { Placed: true } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId == root.Id && moved.Batch.ParentFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.CardId == r.CardId && m.Reason.Value == CaoShuangPlacementReason);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == root.Id && hp.Change.ParentFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == r.TargetSeat && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && recovery.ParentFrameId == root.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, root.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == new CardMoveReason(CaoShuangPlacementReason) && recovery.Attempt.SourceSeat == r.TargetSeat && recovery.Attempt.TargetSeat == r.TargetSeat && recovery.Attempt.Amount == 1;
    }

    internal ProgramSkillFrame? CaoShuangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.CaoShuangFirstChild(root, child), (child) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(child));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26060)]
    internal bool TryAdvanceCaoShuangSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && CaoShuangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
