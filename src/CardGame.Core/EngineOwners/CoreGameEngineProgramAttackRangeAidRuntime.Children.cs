#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramAttackRangeAidRuntime
{
    internal bool ProgramAttackRangeAidChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.AttackRangeAid?.Payment is not { } payment || !ValidProgramAttackRangeAid(frame))
            return false;
        if (CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, frame))
            return payment.Stage == ProgramAttackRangeAidPaymentStage.Drawing && frame.NativeDraw is not null;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(frame, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(frame, hp);
        if (child is CardsMovedTriggerWindowFrame moved)
            return payment.Stage == ProgramAttackRangeAidPaymentStage.MovementChildren && frame.PendingMovementContinuation is not null && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(frame, moved) && moved.Batch.Movements.All(move => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) && move.Sequence > payment.Before && move.Sequence <= payment.After);
        return ProgramChildWindowRuntime.MatchesStateChangeChild(frame.Id, child, _coreProgramEventHistoryOperations.Value.ProgramHistory);
    }

    private bool ProgramAttackRangeAidSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, childIndex =>
    {
        if (_coreObserverSubtreesOperations.IsMovementObserverEdge(childIndex) ||
            _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(childIndex))
            return true;

        var parent = _resolutionStackOperations._resolutionStack[childIndex - 1];
        var child = _resolutionStackOperations._resolutionStack[childIndex];
        return _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child) ||
            parent is ProgramSkillFrame frame && ProgramAttackRangeAidChild(frame, child) ||
            CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, parent) ||
            _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    }, _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix);

    private ProgramSkillFrame? ProgramAttackRangeAidRoot() =>
        _coreObserverSubtreesOperations.FindProgramObserverRoot(ProgramAttackRangeAidChild, ProgramAttackRangeAidSuffix);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 56900)]
    internal bool ProgramAttackRangeAidDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && ProgramAttackRangeAidRoot()is { } frame && _resolutionStackOperations._resolutionStack.FindIndex(child => child.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(child => child.Id == frame.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 56900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 56900)]
    internal bool ProgramAttackRangeAidRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame child)
    {
        if (!ValidProgramAttackRangeAid(frame))
            return false;
        if (_coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(frame, child))
            return true;
        var a = _resolutionStackOperations._resolutionStack.FindIndex(candidate => candidate.Id == frame.Id);
        var b = _resolutionStackOperations._resolutionStack.FindIndex(candidate => candidate.Id == child.Id);
        return a >= 0 && b > a + 1 && ProgramAttackRangeAidChild(frame, _resolutionStackOperations._resolutionStack[a + 1]) && ProgramAttackRangeAidSuffix(a) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, _resolutionStackOperations._resolutionStack[b - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 56900)]
    internal bool AdvanceProgramAttackRangeAidChild() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ProgramAttackRangeAidRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
