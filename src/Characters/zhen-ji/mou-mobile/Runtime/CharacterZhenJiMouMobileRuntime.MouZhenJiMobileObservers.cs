#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhenJiMouMobileRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 391)]
    internal bool IsMouZhenJiLuoshenMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op == SkillProgramEffectOp.ResolveMouZhenJiLuoshen && f.MouZhenJiLuoshen is { Stage: MouZhenJiLuoshenStage.MovementChildren } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidMouZhenJiLuoshen(f);
    internal bool MouZhenJiLuoshenFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouZhenJiLuoshen is not { Stage: MouZhenJiLuoshenStage.MovementChildren } r || !ValidMouZhenJiLuoshen(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && IsMouZhenJiLuoshenMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        return child is ProgramLifecycleTriggerWindowFrame state && state.Window == SkillProgramTriggerWindow.SkillsChanged && state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal bool MouZhenJiLuoshenObserverEdge(int n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[n - 1], _resolutionStackOperations._resolutionStack[n]) || _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && MouZhenJiLuoshenFirstChild(f, _resolutionStackOperations._resolutionStack[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]);
    internal bool MouZhenJiLuoshenObserverSuffix(int index) => _resolutionStackOperations._resolutionStack[index + 1] is DyingFrame dying && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, dying) || _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _characterZhenJiMouMobileOperations.MouZhenJiLuoshenObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouZhenJiLuoshenObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhenJiMouMobileOperations.MouZhenJiLuoshenFirstChild(f, c), (n) => _characterZhenJiMouMobileOperations.MouZhenJiLuoshenObserverSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1371)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1481)]
    internal bool HasMouZhenJiLuoshenDamageObserver(long id) => MouZhenJiLuoshenObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1501)]
    internal bool HasMouZhenJiLuoshenBeforeDamageObserver(long id) => MouZhenJiLuoshenObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1331)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 761)]
    internal bool IsMouZhenJiLuoshenDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouZhenJiLuoshenObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 940)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 880)]
    internal bool IsMouZhenJiLuoshenRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return index >= 0 && child > index + 1 && MouZhenJiLuoshenFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && MouZhenJiLuoshenObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29900)]
    internal bool ResumeMouZhenJiLuoshen(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZhenJiLuoshen: not null } f || f.Id != id)
            return false;
        ContinueMouZhenJiLuoshen(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29900)]
    internal bool ReturnMouZhenJiLuoshenMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouZhenJiLuoshen(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29900)]
    internal bool AdvanceMouZhenJiLuoshenChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouZhenJiLuoshenObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29900)]
    internal bool ContinueAcceptedMouZhenJiLuoshen(ProgramSkillFrame f) => f.MouZhenJiLuoshen is { StartingSeat: not null } && ValidMouZhenJiLuoshen(f);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 29900)]
    internal void AssertMouZhenJiLuoshen(ProgramSkillFrame f)
    {
        if (f.MouZhenJiLuoshen is not null && !ValidMouZhenJiLuoshen(f))
            throw new InvalidOperationException("Mou Luoshen lost its owning source, frozen roster or participant cursor.");
    }
}
