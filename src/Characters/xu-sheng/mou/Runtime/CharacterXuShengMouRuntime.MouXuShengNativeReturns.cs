#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuShengMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 34900)]
    internal bool MouXuShengMovementReturn(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => (effect?.Op is SkillProgramEffectOp.ResolveMouXuShengQinqiang or SkillProgramEffectOp.ResolveMouXuShengYizhen) && MouXuSheng.Valid(f) && f.MouXuSheng is { Stage: MouXuShengStage.DiscardChildren or MouXuShengStage.DrawChildren or MouXuShengStage.ClaimChildren } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouXuShengFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouXuSheng.Valid(f) || f.MouXuSheng is not { } r)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouXuShengStage.Drawing && f.NativeDraw is not null;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } pending && MouXuShengMovementReturn(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouXuShengNativeEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouXuShengFirstNativeChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouXuShengNativeSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterXuShengMouOperations.MouXuShengNativeEdge(i), (i, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouXuShengNativeRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterXuShengMouOperations.MouXuShengFirstNativeChild(f, c), (n) => _characterXuShengMouOperations.MouXuShengNativeSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 34900)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 34900)]
    internal bool MouXuShengDamageWindow(long id) => MouXuShengNativeRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 34900)]
    internal bool MouXuShengBeforeDamage(long id) => MouXuShengNativeRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 34900)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 34900)]
    internal bool MouXuShengDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouXuShengNativeRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 34900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 34900)]
    internal bool MouXuShengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouXuShengFirstNativeChild(f, recovery))
            return true;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && MouXuShengFirstNativeChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && MouXuShengNativeSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 34900)]
    internal bool MouXuShengAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouXuShengNativeRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
