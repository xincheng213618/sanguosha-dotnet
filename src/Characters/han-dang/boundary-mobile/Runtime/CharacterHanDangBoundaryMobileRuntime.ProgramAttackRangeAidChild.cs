#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanDangBoundaryMobileRuntime
{
    internal bool ProgramAttackRangeAidChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.AttackRangeAid?.Payment is not { } payment || !ValidProgramAttackRangeAid(frame))
            return false;
        if (CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, frame))
            return payment.Stage == ProgramAttackRangeAidPaymentStage.Drawing && frame.NativeDraw is not null;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(frame, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(frame, hp);
        if (child is CardsMovedTriggerWindowFrame moved)
            return payment.Stage == ProgramAttackRangeAidPaymentStage.MovementChildren && frame.PendingMovementContinuation is not null && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(frame, moved) && moved.Batch.Movements.All(move => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) && move.Sequence > payment.Before && move.Sequence <= payment.After);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == frame.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(entry => entry.Change.Id == state.Id && entry.Change.ParentFrameId == frame.Id));
    }

    internal bool ProgramAttackRangeAidSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _resolutionStackOperations2._resolutionStack()[i - 1] is ProgramSkillFrame frame && this.ProgramAttackRangeAidChild(frame, _resolutionStackOperations2._resolutionStack()[i]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? ProgramAttackRangeAidRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((frame, child) => this.ProgramAttackRangeAidChild(frame, child), (index) => this.ProgramAttackRangeAidSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 56900)]
    internal bool ProgramAttackRangeAidDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ProgramAttackRangeAidRoot()is { } frame && _resolutionStackOperations._resolutionStack.FindIndex(child => child.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(child => child.Id == frame.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 56900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 56900)]
    internal bool ProgramAttackRangeAidRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame child)
    {
        if (!ValidProgramAttackRangeAid(frame))
            return false;
        if (_coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(frame, child, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var a = _resolutionStackOperations._resolutionStack.FindIndex(candidate => candidate.Id == frame.Id);
        var b = _resolutionStackOperations._resolutionStack.FindIndex(candidate => candidate.Id == child.Id);
        return a >= 0 && b > a + 1 && ProgramAttackRangeAidChild(frame, _resolutionStackOperations._resolutionStack[a + 1]) && ProgramAttackRangeAidSuffix(a) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, _resolutionStackOperations._resolutionStack[b - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 56900)]
    internal bool AdvanceProgramAttackRangeAidChild() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ProgramAttackRangeAidRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
