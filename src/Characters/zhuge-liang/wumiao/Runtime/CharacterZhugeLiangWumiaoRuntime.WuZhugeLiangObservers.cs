#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeLiangWumiaoRuntime
{
    internal bool ValidWuZgl(ProgramSkillFrame f) => f.WuZhugeLiang is { } r && r.Source == WuZglSource(f) && r.GameplayHash == f.GameplayHash && f.InstructionIndex == 1 && r.WindowId == (f.WindowContext?.ParentFrameId ?? 0) && WuZhugeLiangComposition.Handles(r.Operation) && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == r.Operation && r.Before >= 0 && r.After >= r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && r.Targets.Distinct().Count() == r.Targets.Count && r.Targets.All(_coreTableSeatQueriesOperations.IsValidPlayerSeat) && r.Cursor >= 0 && r.Cursor <= r.Targets.Count && _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhugeLiangStartedEvent>().Count(e => e == new WuZhugeLiangStartedEvent(f.Id, r.Source, r.GameplayHash, r.Operation, r.ActualTurn, r.TurnOwner, r.WindowId, r.ActionId, r.UseId)) == 1;
    internal void SealWuZgl(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuZhugeLiang = f.WuZhugeLiang!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence))
        {
            var fact = new WuZhugeLiangMovementSignedEvent(id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<WuZhugeLiangMovementSignedEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }

    internal bool DrainWuZgl(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal bool WuZglFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidWuZgl(f))
            return false;
        if (CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return f.WuZhugeLiang!.Stage == WuZhugeLiangStage.Drawing && f.NativeDraw is not null;
        if (child is RecoveryReplacementFrame replacement)
            return replacement.ParentFrameId == f.Id && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, replacement, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && hp.Change.ParentFrameId == f.Id && (_coreProgramRecoveryReceiptsOperations.ProgramRecoveryReceiptMatches(f, hp, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()) || f.WuZhugeLiang is { Operation: SkillProgramEffectOp.ResolveWuZhugeLiangJincui } r && hp.Change.Kind == HpChangeKind.Setting && hp.Change.Id == r.HpChangeId && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.HpBefore == r.HpBefore && hp.Change.HpAfter == r.HpAfter && _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhugeLiangHpSetEvent>().Count(e => e.FrameId == f.Id && e.ChangeId == hp.Change.Id && e.Before == r.HpBefore && e.After == r.HpAfter) == 1);
        if (child is CardsMovedTriggerWindowFrame moved)
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.Movements.Count > 0 && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreProgramEventHistoryOperations.ProgramEventHistory<WuZhugeLiangMovementSignedEvent>().Contains(new(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool WuZglObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && this.WuZglFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? WuZglObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.WuZglFirstChild(f, c), (i) => this.WuZglObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 30300)]
    internal bool IsWuZglDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && WuZglObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 30300)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 30300)]
    internal bool IsWuZglRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame r)
    {
        if (f.WuZhugeLiang is null || !ValidWuZgl(f))
            return false;
        if (r.ParentFrameId == f.Id)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, r, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == r.Id);
        return i >= 0 && child > i + 1 && WuZglFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && WuZglObserverSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(r, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 30300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 30300)]
    internal bool ContinuePaidWuZgl(ProgramSkillFrame f) => f.WuZhugeLiang is { Paid: true } && ValidWuZgl(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 30300)]
    internal bool ResumeWuZgl(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuZhugeLiang: not null } f || f.Id != id)
            return false;
        ContinueWuZgl(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 30300)]
    internal bool ReturnWuZglMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeWuZgl(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 30300)]
    internal bool AdvanceWuZglChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WuZglObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
