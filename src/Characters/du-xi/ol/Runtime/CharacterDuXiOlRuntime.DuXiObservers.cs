#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuXiOlRuntime
{
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 70)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 60)]
    internal bool IsDuXiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.DuXiQuxi is { Stage: DuXiQuxiStage.ObtainChildren, ObtainedFrom.Zone: CardZoneKind.Equipment, ObtainedCardId: { } id } r && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.SilverLion && recovery.ParentFrameId == f.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, f.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == DuXiObtainReason && recovery.Attempt.SourceSeat == r.MoreHandSeat && recovery.Attempt.TargetSeat == r.MoreHandSeat && recovery.Attempt.Amount == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<DuXiQuxiCardObtainedEvent>().Any(e => e.FrameId == f.Id && e.CardId == id && e.SequenceAfter == r.SequenceAfter);

    internal bool DuXiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.DuXiQuxi is not { } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == DuXiQuxiStage.ObtainChildren && moved.ResumeProgramFrameId == f.Id && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements is [var movement] && movement.CardId == r.ObtainedCardId && movement.From == r.ObtainedFrom && movement.Reason.Value == DuXiObtainReason && movement.Sequence == r.SequenceAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(movement);
        if (child is RecoveryReplacementFrame recovery)
            return IsDuXiRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == DuXiQuxiStage.ObtainChildren && r.ObtainedFrom?.Zone == CardZoneKind.Equipment && r.ObtainedCardId is { } lion && _coreAdvancedLifecycleOperations.GetAdvancedCard(lion).Kind == CardKind.SilverLion && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == r.MoreHandSeat && hp.Change.Amount == 1;
        if (child is ProgramLifecycleTriggerWindowFrame { CharacterStateContinuation: CharacterStateContinuation.Program } state)
            return r.Stage == DuXiQuxiStage.FaceDownChildren && state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal bool DuXiObserverSuffix(int rootIndex) => _coreObserverSubtreesOperations.HasObserverSuffix(rootIndex, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? DuXiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterDuXiOlOperations.DuXiFirstChild(f, c), (i) => _characterDuXiOlOperations.DuXiObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 400)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 510)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 540)]
    internal bool HasDuXiDamageObserver(long id) => DuXiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26300)]
    internal bool AdvanceDuXiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && DuXiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
