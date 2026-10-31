#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiaXuMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 36500)]
    internal bool MouJxMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MouJiaXu is { } r && effect?.Op == r.Operation && MouJiaXu.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouJxFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouJiaXu.Valid(f) || f.MouJiaXu is not { } r)
            return false;
        if (child is CardUseFrame u)
            return MouJxNativeRidesOn(u, f);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouJxMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouJiaXuStage.Drawing;
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Operation == SkillProgramEffectOp.RestoreMouJiaXuRushi && r.Stage == MouJiaXuStage.HealthChildren && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && hp.Change.ParentFrameId == f.Id && hp.Change.Id == r.HpChangeId && hp.Change.Kind == HpChangeKind.Setting && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.HpBefore == r.HpBefore && hp.Change.HpAfter == r.HpAfter && _coreProgramEventHistoryOperations.ProgramEventHistory<MouJiaXuHpSetEvent>().Count(e => e.FrameId == f.Id && e.RecordFrameId == r.Record!.FrameId && e.ChangeId == hp.Change.Id && e.Before == r.HpBefore && e.After == r.HpAfter) == 1;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouJxEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouJxFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouJxSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterJiaXuMouOperations.MouJxEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouJxObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterJiaXuMouOperations.MouJxFirstChild(f, c), (n) => _characterJiaXuMouOperations.MouJxSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 36500)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 36500)]
    internal bool MouJxDamageWindow(long id) => MouJxObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 36500)]
    internal bool MouJxBeforeDamage(long id) => MouJxObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 36500)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 36500)]
    internal bool MouJxDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouJxObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 36500)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 36500)]
    internal bool MouJxRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouJxFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouJxFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouJxSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 36500)]
    internal bool MouJxAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouJxObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
