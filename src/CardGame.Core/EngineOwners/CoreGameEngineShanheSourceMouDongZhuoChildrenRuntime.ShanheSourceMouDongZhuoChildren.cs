#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoChildrenRuntime
{
    internal bool SourceMouFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!_coreShanheSourceMouDongZhuoOperations.ValidSourceMou(f) || f.ShanheSourceMou is not { Paid: true } r)
            return false;
        if (child is CardUseFrame wine)
            return r.Stage == ShanheSourceMouStage.Wine && r.WineUseId == wine.Id && _coreShanheSourceMouDongZhuoWineOperations.IsShanheSourceMouWine(wine.Id);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } pending)
            return _coreShanheSourceMouDongZhuoOperations.SourceMouMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending) && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(move => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(move) && move.Sequence > r.Before && move.Sequence <= r.After);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && (r.Stage == ShanheSourceMouStage.ThresholdChildren && r.Ordinal == 3 && hp.Change.TargetSeat == r.Actor && hp.Change.Kind == HpChangeKind.Loss || r.Stage == ShanheSourceMouStage.MaximumChildren && hp.Change.TargetSeat == r.Payer && hp.Change.Kind is HpChangeKind.MaximumHp or HpChangeKind.Loss || r.Stage is ShanheSourceMouStage.OwnChildren or ShanheSourceMouStage.PeerChildren && hp.Change.TargetSeat == r.Payer && hp.Change.Kind == HpChangeKind.Recovery);
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == "skill.shanhe-source-mou.discard" && recovery.Attempt.TargetSeat == r.Payer && r.Cards.Any(card => card.Kind == CardKind.SilverLion && card.From == CardLocation.Equipment(r.Payer));
        if (child is DyingFrame dying)
            return r.Stage == ShanheSourceMouStage.ThresholdChildren && r.Ordinal == 3 && dying.VictimSeat == r.Actor && _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool SourceMouEdge(int index) => _coreObserverSubtreesOperations.IsMovementObserverEdge(index) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(index) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(index) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[index - 1], _resolutionStackOperations._resolutionStack[index]) || _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame parent && SourceMouFirstChild(parent, _resolutionStackOperations._resolutionStack[index]) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[index - 1], _resolutionStackOperations._resolutionStack[index]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]);
    internal bool SourceMouSuffix(int index) => _resolutionStackOperations._resolutionStack[index + 1] is DyingFrame dying && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, dying) || _coreObserverSubtreesOperations.HasObserverSuffix(index, (edge) => this.SourceMouEdge(edge), (edge, frame) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(edge, frame));
    internal ProgramSkillFrame? SourceMouRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((frame, child) => this.SourceMouFirstChild(frame, child), (index) => this.SourceMouSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 62053)]
    internal bool SourceMouDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && SourceMouRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 62053)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 62053)]
    internal bool SourceMouRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (SourceMouFirstChild(f, recovery))
            return true;
        var root = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == recovery.Id);
        return root >= 0 && child > root + 1 && SourceMouFirstChild(f, _resolutionStackOperations._resolutionStack[root + 1]) && SourceMouSuffix(root) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 62053)]
    internal bool AdvanceSourceMouChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && SourceMouRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
