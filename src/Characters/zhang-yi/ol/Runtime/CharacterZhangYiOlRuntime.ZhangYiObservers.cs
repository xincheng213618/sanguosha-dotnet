#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangYiOlRuntime
{

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 910)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 850)]
    internal bool IsZhangYiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.ZhangYi is { Stage: ZhangYiStage.KangruiRecovery, Choice: "recover", RecoveryRequested: > 0 } r && ContinuePaidZhangYi(f) && recovery.ParentFrameId == f.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, f.Id) && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == r.RecipientSeat && recovery.Attempt.Amount == r.RecoveryRequested && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == r.InstructionIndex;
    internal bool ZhangYiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ZhangYi is not { } r)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return IsZhangYiRecoveryParent(f, recovery);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == ZhangYiStage.DianjunCleanupChildren && moved.ResumeProgramFrameId == f.Id && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > r.CleanupBefore && m.Sequence <= r.CleanupAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)) || r.Stage == ZhangYiStage.KangruiDraw && moved.ResumeProgramFrameId == f.Id && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count == 1 && moved.Batch.Movements.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ZhangYiDrawReason && m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == ZhangYiStage.DianjunCleanupChildren && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.Program || r.Stage == ZhangYiStage.KangruiRecovery && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.TargetSeat == r.RecipientSeat && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount > 0;
        if (child is BeforeDamageProgramWindowFrame before)
            return r.Stage == ZhangYiStage.DianjunDamage && f.AttackAttempt is { SourceLess: true } && (before.ContinuationAttackResolutionId ?? before.ParentFrameId) == f.Id && before.TargetSeat == f.OwnerSeat;
        if (child is DamageFrame damage)
            return r.Stage == ZhangYiStage.DianjunDamage && f.AttackAttempt is { SourceLess: true } && damage.ParentFrameId == f.Id && damage.TargetSeat == f.OwnerSeat;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && ContinuePaidZhangYi(f);
    }

    internal bool ZhangYiObserverSuffix(int root) => _coreObserverSubtreesOperations.HasObserverSuffix(root, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? ZhangYiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.ZhangYiFirstChild(f, child), (i) => this.ZhangYiObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26400)]
    internal bool AdvanceZhangYiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhangYiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
