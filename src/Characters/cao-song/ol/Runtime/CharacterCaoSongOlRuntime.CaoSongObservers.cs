#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoSongOlRuntime
{
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 40)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 30)]
    internal bool IsCaoSongLiluRecoveryParent(ProgramSkillFrame producer, RecoveryReplacementFrame recovery) => producer.CaoSongLilu is { Stage: CaoSongLiluStage.RecoveryChildren, Grows: true, GiftCount: > 0 } receipt && receipt.Source == CaoSongSource(producer) && recovery.ParentFrameId == producer.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, producer.Id) && recovery.Attempt.Completion == new RecoveryAttemptCompletion(RecoveryAttemptProducer.Program, producer.InstructionIndex) && recovery.Attempt.SourceSeat == producer.OwnerSeat && recovery.Attempt.TargetSeat == producer.OwnerSeat && recovery.Attempt.Amount == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoSongLiluGiftCommittedEvent>().Any(e => e.ProgramFrameId == producer.Id && e.Source == receipt.Source && e.Grows);
    internal bool CaoSongLiluFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.CaoSongLilu is not { } receipt)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId == root.Id && moved.Batch.ParentFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > receipt.SequenceBefore && m.Sequence <= receipt.SequenceAfter && m.Reason.Value == (receipt.Stage == CaoSongLiluStage.Drawing ? LiluDrawReason : LiluGiftReason));
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Amount == 1 && (receipt.Stage == CaoSongLiluStage.MaximumChildren && hp.Change.Kind == HpChangeKind.MaximumHp || receipt.Stage == CaoSongLiluStage.RecoveryChildren && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == root.OwnerSeat);
        if (child is RecoveryReplacementFrame recovery)
            return IsCaoSongLiluRecoveryParent(root, recovery);
        return child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? CaoSongLiluObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.CaoSongLiluFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26030)]
    internal bool TryAdvanceCaoSongLiluSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && CaoSongLiluObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
