#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuRanBoundaryMobileRuntime
{

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 53900)]
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 53900)]
    internal bool BoundaryDanshouMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.BoundaryDanshou is { Stage: BoundaryDanshouStage.PaidChildren } && e?.Op == SkillProgramEffectOp.PrepareBoundaryZhuRanDanshou && f.PendingMovementContinuation == p && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 53900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 53900)]
    internal bool BoundaryDanshouRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.BoundaryDanshou is { Stage: BoundaryDanshouStage.PaidChildren } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && r.Selected.Any(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.SilverLion && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.Equipment(f.OwnerSeat)));
    internal bool BoundaryDanshouFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BoundaryDanshou is not { Stage: BoundaryDanshouStage.PaidChildren } r)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return BoundaryDanshouRecovery(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    }

    internal ProgramSkillFrame? BoundaryDanshouObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhuRanBoundaryMobileOperations.BoundaryDanshouFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 53900)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 53900)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 53900)]
    internal bool BoundaryDanshouDamageObserver(long id) => BoundaryDanshouObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 53900)]
    internal bool BoundaryDanshouDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && BoundaryDanshouObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 53900)]
    internal bool AdvanceBoundaryDanshouObservers() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BoundaryDanshouObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
