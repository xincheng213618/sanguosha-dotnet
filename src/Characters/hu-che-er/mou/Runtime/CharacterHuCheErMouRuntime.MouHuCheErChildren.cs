#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuCheErMouRuntime
{
    internal bool MouHuCheErMovement(ProgramSkillFrame f, SkillProgramEffect? op, ProgramMovementContinuation p) => f.MouHuCheEr is not null && op?.Op == SkillProgramEffectOp.ResolveMouHuCheErKongwu && ValidMouHuCheEr(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouHuCheErFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouHuCheEr(f) || f.MouHuCheEr is not { } r)
            return false;
        if (CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouHuCheErStage.Drawing && f.NativeDraw is not null;
        if (child is CardUseFrame use)
            return r.Stage == MouHuCheErStage.NativeSlash && use.MouHuCheErSlash is { } o && o.FrameId == f.Id && use.Id == r.UseId && IsIssuedMouHuCheErUse(use);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouHuCheErMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && (r.Stage == MouHuCheErStage.SlashChildren || m.Sequence > r.Before && m.Sequence <= r.After));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouHuCheErEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouHuCheErFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]);
    internal bool MouHuCheErSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterHuCheErMouOperations.MouHuCheErEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouHuCheErRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterHuCheErMouOperations.MouHuCheErFirstChild(f, c), (n) => _characterHuCheErMouOperations.MouHuCheErSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 35800)]
    internal bool MouHuCheErDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouHuCheErRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 35800)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 35800)]
    internal bool MouHuCheErRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouHuCheErFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouHuCheErSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 35800)]
    internal bool MouHuCheErAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouHuCheErRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
