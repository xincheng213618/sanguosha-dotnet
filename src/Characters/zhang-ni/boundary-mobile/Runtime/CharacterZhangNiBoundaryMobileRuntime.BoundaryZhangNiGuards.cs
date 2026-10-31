#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangNiBoundaryMobileRuntime
{

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 54200)]
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 54200)]
    internal bool BoundaryWurongGiftMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.BoundaryWurong is { Stage: BoundaryWurongStage.GiftChildren } r && e is { Op: SkillProgramEffectOp.ResolveBoundaryZhangNiWurong, StateId: "gifts" } && f.PendingMovementContinuation == p && p.SubjectSeat == r.Giver && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 54200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 54200)]
    internal bool BoundaryWurongGiftRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.BoundaryWurong is { Stage: BoundaryWurongStage.GiftChildren } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && r.Selected.Any(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.SilverLion && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.Equipment(r.Giver)));
    internal bool BoundaryWurongFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BoundaryWurong is not { Stage: BoundaryWurongStage.GiftChildren } r)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return BoundaryWurongGiftRecovery(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    }

    internal ProgramSkillFrame? BoundaryWurongObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhangNiBoundaryMobileOperations.BoundaryWurongFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 54200)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 54200)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 54200)]
    internal bool BoundaryWurongDamageObserver(long id) => BoundaryWurongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 54200)]
    internal bool BoundaryWurongDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && BoundaryWurongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 54200)]
    internal bool AdvanceBoundaryWurongObservers() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BoundaryWurongObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
