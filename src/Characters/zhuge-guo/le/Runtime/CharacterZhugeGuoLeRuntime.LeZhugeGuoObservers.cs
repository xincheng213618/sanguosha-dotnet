#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeGuoLeRuntime
{
    internal bool IsLeChengyanMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op == SkillProgramEffectOp.ResolveLeZhugeGuoChengyan && f.LeChengyan is { Stage: LeChengyanStage.RevealChildren or LeChengyanStage.SecondDrawChildren } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidLeChengyan(f);
    internal bool LeChengyanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.LeChengyan is not { Stage: LeChengyanStage.RevealChildren or LeChengyanStage.SecondDrawChildren } r || !ValidLeChengyan(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && IsLeChengyanMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        return child is ProgramLifecycleTriggerWindowFrame state && state.Window == SkillProgramTriggerWindow.SkillsChanged && state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal bool LeChengyanObserverEdge(int n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[n - 1], _resolutionStackOperations._resolutionStack[n]) || _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && LeChengyanFirstChild(f, _resolutionStackOperations._resolutionStack[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]);
    internal bool LeChengyanObserverSuffix(int index) => _resolutionStackOperations._resolutionStack[index + 1] is DyingFrame dying && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, dying) || _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => this.LeChengyanObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? LeChengyanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.LeChengyanFirstChild(f, c), (n) => this.LeChengyanObserverSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 30200)]
    internal bool IsLeChengyanDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && LeChengyanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 30200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 30200)]
    internal bool IsLeChengyanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return index >= 0 && child > index + 1 && LeChengyanFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && LeChengyanObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 30200)]
    internal bool ResumeLeChengyan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LeChengyan: not null } f || f.Id != id)
            return false;
        ContinueLeChengyan(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 30200)]
    internal bool ReturnLeChengyanMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeLeChengyan(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 30200)]
    internal bool AdvanceLeChengyanChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LeChengyanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 30200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 30200)]
    internal bool ContinueAcceptedLeChengyan(ProgramSkillFrame f) => f.LeChengyan is not null && ValidLeChengyan(f);
}
