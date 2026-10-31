#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangHeMouMobileRuntime
{
    internal bool MouQiaobianFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouQiaobian.Valid(f) || f.MouZhangHe is not { } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        var removals = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.CardKind == CardKind.SilverLion && m.From.Zone == CardZoneKind.Equipment && m.Sequence > r.Before && m.Sequence <= r.After && (m.Reason.Value == MouQiaobianRuntime.CostReason || m.Reason.Value == MouQiaobianRuntime.FieldReason || m.Reason == CardMoveReasons.EquipmentReplace)).ToArray();
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Amount == 1 && removals.Any(m => m.From.OwnerSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Completion.MoveReason == m.Reason);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == hp.Change.TargetSeat && removals.Any(m => m.From.OwnerSeat == hp.Change.TargetSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.ResolutionId > f.Id && e.ResolutionId < hp.Change.Id && e.PlayerSeat == m.From.OwnerSeat && e.Reason == m.Reason && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter));
        return CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f) || child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouQiaobianNativeEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouQiaobianFirstNativeChild(f, _resolutionStackOperations._resolutionStack[i]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouQiaobianNativeSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterZhangHeMouMobileOperations.MouQiaobianNativeEdge(i), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? MouQiaobianNativeRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhangHeMouMobileOperations.MouQiaobianFirstNativeChild(f, c), (n) => _characterZhangHeMouMobileOperations.MouQiaobianNativeSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 34200)]
    internal bool MouQiaobianDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouQiaobianNativeRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 34200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 34200)]
    internal bool MouQiaobianRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouQiaobianFirstNativeChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouQiaobianFirstNativeChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouQiaobianNativeSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 34200)]
    internal bool MouQiaobianAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouQiaobianNativeRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
