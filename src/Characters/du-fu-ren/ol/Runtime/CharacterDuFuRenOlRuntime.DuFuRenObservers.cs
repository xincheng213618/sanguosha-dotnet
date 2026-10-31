#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuFuRenOlRuntime
{

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 670)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 610)]
    internal bool IsDuFuRenRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.DuFuRen is not { } r || !_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement || recovery.Attempt.SourceSeat != f.OwnerSeat || recovery.Attempt.Amount != 1)
            return false;
        return r.Stage == DuFuRenStage.RecoveryChildren && recovery.Attempt.TargetSeat == r.RecipientSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == 1 || r.Stage == DuFuRenStage.GiftChildren && r.From?.Zone == CardZoneKind.Equipment && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == DuFuRenGiftReason && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == r.GiftSequence && m.CardId == r.CardId && m.CardKind == CardKind.SilverLion);
    }

    internal bool DuFuRenFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.DuFuRen is not { } r || root.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == DuFuRenStage.GiftChildren && moved.Batch.ParentFrameId == root.Id && (moved.ResumeProgramFrameId == root.Id || moved.Batch.AwaitingProgramFrameId == root.Id) && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements is [var m] && m.Sequence == r.GiftSequence && m.CardId == r.CardId && m.From == r.From && m.To == CardLocation.Hand(r.RecipientSeat!.Value) && m.Reason.Value == DuFuRenGiftReason;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is RecoveryReplacementFrame recovery)
            return IsDuFuRenRecoveryParent(root, recovery);
        if (child is not HpChangedTriggerWindowFrame hp || hp.Change.ParentFrameId != root.Id || hp.ResumeFrameId != root.Id || hp.Continuation != PostEventContinuation.AwaitedProgramMovement || hp.Change.Kind != HpChangeKind.Recovery || hp.Change.Amount != 1 || hp.Change.SourceSeat != root.OwnerSeat)
            return false;
        return r.Stage == DuFuRenStage.RecoveryChildren && hp.Change.TargetSeat == r.RecipientSeat || r.Stage == DuFuRenStage.GiftChildren && r.From?.Zone == CardZoneKind.Equipment && hp.Change.TargetSeat == root.OwnerSeat && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == r.GiftSequence && m.CardId == r.CardId && m.CardKind == CardKind.SilverLion);
    }

    internal ProgramSkillFrame? DuFuRenObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.DuFuRenFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26520)]
    internal bool AdvanceDuFuRenSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && DuFuRenObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
