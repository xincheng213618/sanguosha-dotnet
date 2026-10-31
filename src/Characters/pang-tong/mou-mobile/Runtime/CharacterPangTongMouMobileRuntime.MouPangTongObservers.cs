#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangTongMouMobileRuntime
{
    internal bool DrainMouPt(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal bool MouPtFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouPtTargets(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.Movements.Count > 0 && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreProgramEventHistoryOperations.ProgramEventHistory<MouPangTongMovementSignedEvent>().Contains(new(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)));
        if (child is HpChangedTriggerWindowFrame hp)
            return CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && hp.Change.ParentFrameId == f.Id && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReceiptMatches(f, hp, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouPtObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame p && this.MouPtFirstChild(p, _resolutionStackOperations2._resolutionStack()[n]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouPtObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.MouPtFirstChild(f, child), (i) => this.MouPtObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 30500)]
    internal bool IsMouPtDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouPtObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 30500)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 30500)]
    internal bool IsMouPtRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.MouPangTongTargets is null || !ValidMouPtTargets(f))
            return false;
        if (recovery.ParentFrameId == f.Id)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return index >= 0 && child > index + 1 && MouPtFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && MouPtObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 30500)]
    internal bool ContinueMouPtPaidSource(ProgramSkillFrame f) => f.MouPangTongTargets is not null && ValidMouPtTargets(f) || f.SkillId == MouPangTongComposition.Lianhuan && f.TriggerId == "pay-for-chain" && f.InstructionIndex is 1 or 2 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Any(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == f.OwnerSeat && e.Amount == 1) || f.SkillId == MouPangTongComposition.Niepan && f.TriggerId == "rebirth" && f.InstructionIndex is >= 1 and <= 6 && f.WindowContext is { } context && _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().Any(d => d.Id == context.ParentFrameId && d.VictimSeat == f.OwnerSeat) && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(f.OwnerSeat, f.SkillId, "rebirth", SkillUsageScope.Game) == 1;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 30500)]
    internal bool FinishMouPtDeadSource(ProgramSkillFrame f) => f.MouPangTongTargets is not null && ValidMouPtTargets(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 30500)]
    internal bool ResumeMouPt(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouPangTongTargets: not null } f || f.Id != id)
            return false;
        ContinueMouPtTargets(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 30500)]
    internal bool ReturnMouPtMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouPt(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 30500)]
    internal bool AdvanceMouPtChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouPtObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
