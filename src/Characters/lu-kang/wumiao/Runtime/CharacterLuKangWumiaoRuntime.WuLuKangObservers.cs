#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuKangWumiaoRuntime
{
    internal void SealWuLuKangMovements(long id, long before, bool privateReserve = false)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangShenduan = f.WuLuKangShenduan is { } s ? s with { After = after } : null, WuLuKangKegou = f.WuLuKangKegou is { } k ? k with { After = after } : null });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= after && m.Reason.Value != WuLuKangReserveReason))
        {
            var fact = new WuLuKangMovementSignedEvent(id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuLuKangMovementSignedEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }

    internal bool WuLuKangSignedMovement(ProgramSkillFrame f, CardMovementRecord m) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuLuKangMovementSignedEvent>().Count(e => e == new WuLuKangMovementSignedEvent(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)) == 1;
    internal bool IsWuLuKangMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && (e?.Op == SkillProgramEffectOp.ResolveWuLuKangShenduan && f.WuLuKangShenduan is { Stage: not WuLuKangShenduanStage.DiscardChoice and not WuLuKangShenduanStage.Drawing } && ValidWuLuKangShenduan(f) || e?.Op == SkillProgramEffectOp.ResolveWuLuKangKegou && f.WuLuKangKegou is { Stage: WuLuKangKegouStage.GainChildren or WuLuKangKegouStage.NativeSlashChildren } && ValidWuLuKangKegou(f));
    internal bool DrainWuLuKangChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal bool WuLuKangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.WuLuKangShenduan is { } r && ValidWuLuKangShenduan(f))
        {
            if (CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
                return r.Stage == WuLuKangShenduanStage.Drawing && f.NativeDraw is not null;
            if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f))
                return r.Stage == WuLuKangShenduanStage.DiscardChildren && r.Payment is not null;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && r.Stage == WuLuKangShenduanStage.DiscardChildren && r.Payment is { CostKind: CardKind.SilverLion, CostFrom.Zone: CardZoneKind.Equipment } && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Kind == HpChangeKind.Recovery;
        }
        else if (f.WuLuKangKegou is { } k && ValidWuLuKangKegou(f))
        {
            if (child is PindianFrame pair)
                return k.Stage == WuLuKangKegouStage.Pindian && pair.Id == k.PindianId && pair.ParentFrameId == f.Id && pair.SourceSeat == f.OwnerSeat && pair.OpponentSeat == k.OpponentSeat;
            if (child is CardUseFrame { WuLuKangKegouSlash: { } o } use)
                return k.Stage == WuLuKangKegouStage.NativeSlash && o.ProgramFrameId == f.Id && IsIssuedWuLuKangKegouUse(use);
        }
        else
            return false;
        if (f.PendingMovementContinuation is { } pending && IsWuLuKangMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending) && child is CardsMovedTriggerWindowFrame moved)
        {
            var before = f.WuLuKangShenduan?.Before ?? f.WuLuKangKegou!.Before;
            var after = f.WuLuKangShenduan?.After ?? f.WuLuKangKegou!.After;
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > before && m.Sequence <= after && WuLuKangSignedMovement(f, m));
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool WuLuKangObserverEdge(int n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[n - 1], _resolutionStackOperations._resolutionStack[n]) || _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && WuLuKangFirstChild(f, _resolutionStackOperations._resolutionStack[n]) || WuLuKangPindianEdge(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]);
    internal bool WuLuKangObserverSuffix(int index) => _resolutionStackOperations._resolutionStack[index + 1] is DyingFrame dying && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, dying) || _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => this.WuLuKangObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? WuLuKangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.WuLuKangFirstChild(f, c), (n) => this.WuLuKangObserverSuffix(n));
    internal bool HasWuLuKangBeforeDamageObserver(long id) => WuLuKangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 750)]
    internal bool IsWuLuKangDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && WuLuKangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 530)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 520)]
    internal bool IsWuLuKangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return index >= 0 && child > index + 1 && WuLuKangFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && WuLuKangObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29700)]
    internal bool ResumeWuLuKang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id)
            return false;
        if (f.WuLuKangShenduan is not null)
        {
            ContinueWuLuKangShenduan(id);
            return true;
        }

        if (f.WuLuKangKegou is not null)
        {
            ContinueWuLuKangKegou(id);
            return true;
        }

        return false;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29700)]
    internal bool ReturnWuLuKangMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeWuLuKang(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29700)]
    internal bool AdvanceWuLuKangChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WuLuKangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29700)]
    internal bool PermitAcceptedWuLuKang(ProgramSkillFrame f) => f.WuLuKangShenduan is { } r && (r.Payment is not null || r.WindowStage is WuLuKangPindianStage.PostComparison or WuLuKangPindianStage.GroupClose) && ValidWuLuKangShenduan(f) || f.WuLuKangKegou is { PindianId: not null } && ValidWuLuKangKegou(f) || PermitWuLuKangIssuedCleanup(f);
    internal bool PermitWuLuKangIssuedCleanup(ProgramSkillFrame f) => f.WuLuKangShenduan is null && f.TriggerId is not null && f.InstructionIndex == 1 && f.WindowContext is { WuLuKangPindian.Stage: WuLuKangPindianStage.GroupClose } c && IsIssuedWuLuKangCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c);
    internal void AssertWuLuKangPairProcessing(PindianFrame pair)
    {
        if (!IsWuLuKangPairSubtree(pair))
            throw new InvalidOperationException("Shenduan cannot authorize an unrelated native Pindian subtree.");
        var expected = new HashSet<int>(pair.ParentProcessingCardIds ?? []);
        foreach (var id in WuLuKangHeldProcessing(pair))
            expected.Add(id);
        var start = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == pair.Id);
        foreach (var frame in _resolutionStackOperations._resolutionStack.Skip(start + 1))
        {
            if (frame is CardUseFrame { Action: { } action } use)
                foreach (var cost in action.PhysicalCards.Where(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Processing))
                {
                    var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cost.CardId);
                    if (m is null || m.From != cost.From || m.To != CardLocation.Processing || m.CardKind != cost.CardKind || !expected.Add(cost.CardId))
                        throw new InvalidOperationException("Shenduan observer cannot claim an unpaid or ancestor native card.");
                }

            if (frame is ProgramCardTriggerWindowFrame { ResponseCompletion: not null } response && _coreResponseCompletionOperations.ValidResponseCompletionWindow(response))
                foreach (var cost in response.Action.PhysicalCards.Where(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Processing))
                    expected.Add(cost.CardId);
        }

        if (!expected.Order().SequenceEqual(_cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Select(c => c.Id).Order()))
            throw new InvalidOperationException("Shenduan lost its exact native or observer processing materials.");
    }
}
