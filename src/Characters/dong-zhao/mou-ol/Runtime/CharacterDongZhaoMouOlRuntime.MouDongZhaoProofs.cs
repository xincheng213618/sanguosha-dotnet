#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongZhaoMouOlRuntime
{
    internal bool IsMouDongZhaoMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect is not null && MouDongZhaoComposition.Handles(effect.Op) && ValidMouDongZhao(f) && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && f.MouDongZhao!.Stage is MouDongZhaoStage.DrawChildren or MouDongZhaoStage.GiftChildren or MouDongZhaoStage.DiscardChildren or MouDongZhaoStage.MoveChildren;
    internal bool MouDongZhaoFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouDongZhao(f) || f.MouDongZhao is not { } r)
            return false;
        if (CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouDongZhaoStage.Drawing && f.NativeDraw is not null;
        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f))
            return r.Stage is MouDongZhaoStage.GiftChildren or MouDongZhaoStage.MoveChildren;
        if (child is HpChangedTriggerWindowFrame hp)
            return CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } pending && IsMouDongZhaoMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDongZhaoMovementSignedEvent>().Count(e => e.FrameId == f.Id && e.CardId == m.CardId && e.Kind == m.CardKind && e.From == m.From && e.To == m.To && e.Reason == m.Reason && e.Sequence == m.Sequence) == 1);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouDongZhaoObserverEdge(int index) => _coreObserverSubtreesOperations.IsMovementObserverEdge(index) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(index) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(index) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[index - 1], _resolutionStackOperations._resolutionStack[index]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]);
    internal bool MouDongZhaoObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => this.MouDongZhaoObserverEdge(n), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MouDongZhaoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.MouDongZhaoFirstChild(f, c), (n) => this.MouDongZhaoObserverSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 33500)]
    internal bool IsMouDongZhaoDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouDongZhaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 33500)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 33500)]
    internal bool IsMouDongZhaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (ValidMouDongZhao(f) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f))
            return true;
        var root = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return root >= 0 && child > root + 1 && MouDongZhaoFirstChild(f, _resolutionStackOperations._resolutionStack[root + 1]) && MouDongZhaoObserverSuffix(root) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 33500)]
    internal bool AdvanceMouDongZhaoChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouDongZhaoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
