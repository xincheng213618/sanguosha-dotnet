#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongChengMouRuntime
{
    internal bool MouDcMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MouDongCheng is { } r && MouDongCheng.Valid(f) && effect?.Op == r.Operation && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouDcFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouDongCheng.Valid(f) || f.MouDongCheng is not { } r)
            return false;
        if (child is CardUseFrame use)
            return MouDcUseRidesOn(f, use);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouDcMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (child is RecoveryReplacementFrame replacement)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(replacement, f);
        if (child is HpChangedTriggerWindowFrame hp)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReceiptMatches(f, hp, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()) || hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && r.Stage == MouDongChengStage.SlotChildren && hp.Change.Kind == HpChangeKind.Recovery && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.PlayerSeat == hp.Change.TargetSeat && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter && e.Reason.Value == "equipment.slot-abolished");
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id) || state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram);
    }

    internal bool MouDcEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouDcFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouDcSuffix(int i) => _resolutionStackOperations._resolutionStack[i + 1] is DyingFrame d && i + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _characterDongChengMouOperations.MouDcEdge(n), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MouDcRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterDongChengMouOperations.MouDcFirstChild(f, c), (n) => _characterDongChengMouOperations.MouDcSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 37400)]
    internal bool MouDcDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouDcRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 37400)]
    internal bool AdvanceMouDcChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouDcRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 37400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 37400)]
    internal bool MouDcRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return i >= 0 && c > i && MouDongCheng.Valid(f) && MouDcFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && (c == i + 1 || MouDcSuffix(i)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }
}
