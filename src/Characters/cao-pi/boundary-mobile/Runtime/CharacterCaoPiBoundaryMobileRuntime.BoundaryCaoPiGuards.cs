#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoPiBoundaryMobileRuntime
{
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 54500)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 54500)]
    internal bool BoundaryFangzhuRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.BoundaryFangzhu is { Stage: BoundaryFangzhuStage.DiscardChildren } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && r.Selected.Any(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.SilverLion && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.Equipment(r.Target)));
    internal bool BoundaryFangzhuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BoundaryFangzhu is not { } r)
            return false;
        if (r.Stage == BoundaryFangzhuStage.DrawRequest)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (r.Stage is not (BoundaryFangzhuStage.DiscardChildren or BoundaryFangzhuStage.DrawChildren))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return BoundaryFangzhuRecovery(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    }

    internal ProgramSkillFrame? BoundaryFangzhuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.BoundaryFangzhuFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 54500)]
    internal bool BoundaryFangzhuDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && BoundaryFangzhuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 54500)]
    internal bool AdvanceBoundaryFangzhuObservers() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BoundaryFangzhuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
