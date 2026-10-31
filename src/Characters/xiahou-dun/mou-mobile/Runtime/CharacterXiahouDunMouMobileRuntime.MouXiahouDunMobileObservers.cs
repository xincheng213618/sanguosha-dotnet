#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouDunMouMobileRuntime
{
    internal bool ValidMouXiahouDun(ProgramSkillFrame f)
    {
        if (f.MouXiahouDunMobile is not { } r || r.Source != MouXiahouDunSource(f) || r.GameplayHash != f.GameplayHash || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Index < 0 || r.Targets is not System.Collections.IList { IsReadOnly: true } || r.Cards is not System.Collections.IList { IsReadOnly: true } || r.Cards.Distinct().Count() != r.Cards.Count || r.Targets.Distinct().Count() != r.Targets.Count || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouDunMobileStartedEvent>().Count(e => e == new MouXiahouDunMobileStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.Phase)) != 1)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveMouXiahouDunGanglie)
        {
            if (f.SkillId != MouXiahouDunGanglie || f.ActivationId != "retaliate-all" || f.TriggerId is not null || f.WindowContext is not null || r.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || r.Cards.Count != 0 || r.Pile is not null || r.Index >= Math.Max(1, r.Targets.Count) || r.Stage is not (MouXiahouDunMobileStage.SelectTargets or MouXiahouDunMobileStage.Damaging))
                return false;
            return r.Stage != MouXiahouDunMobileStage.Damaging || r.Targets.Count > 0 && r.Targets.Where((s, i) => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouDunMobileGanglieSpentEvent>().Count(e => e == new MouXiahouDunMobileGanglieSpentEvent(f.Id, f.OwnerSeat, s, i)) != 1).Any() == false;
        }

        if (f.SkillId != MouXiahouDunQingjian || f.ActivationId != f.TriggerId || f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat || r.Phase is not null || r.Targets.Count != 0)
            return false;
        if (r.Op == SkillProgramEffectOp.CollectMouXiahouDunQingjian)
            return f.TriggerId == "collect-non-use" && c.Window == SkillProgramTriggerWindow.DiscardPileReceived && (r.Pile is null || r.Pile.OwnerSeat == f.OwnerSeat && r.Pile.SkillId == f.SkillId && r.Pile.SkillInstanceId == f.SkillInstanceId) && r.Cards.Count <= 1;
        return r.Op == SkillProgramEffectOp.DistributeMouXiahouDunQingjian && f.TriggerId == "distribute-qingjian" && c.Window == SkillProgramTriggerWindow.PlayEnding && (r.Pile is null && r.Cards.Count == 0 || r.Pile is { } pile && pile.OwnerSeat == f.OwnerSeat && pile.SkillId == f.SkillId && pile.SkillInstanceId == f.SkillInstanceId && r.Index < r.Cards.Count && r.Cards.Where((id, i) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouDunMobileRosterEvent>().Count(e => e == new MouXiahouDunMobileRosterEvent(f.Id, i, id, pile.Location)) != 1).Any() == false);
    }

    internal bool IsIssuedMouXiahouDunPile(PublicPersistentPileSource source) => source.SkillId == MouXiahouDunQingjian && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.MouXiahouDunMobile is { Op: SkillProgramEffectOp.DistributeMouXiahouDunQingjian } r && r.Pile == source && ValidMouXiahouDun(f));
    internal void SealMouXiahouDunMovements(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouXiahouDunMobile = f.MouXiahouDunMobile!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray())
        {
            var fact = new MouXiahouDunMobileMovementEvent(id, m.CardId, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouDunMobileMovementEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }
    internal bool IsMouXiahouDunMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.MouXiahouDunMobile is { Stage: MouXiahouDunMobileStage.MovementChildren } r && e?.Op == r.Op && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidMouXiahouDun(f);
    internal bool MouXiahouDunFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouXiahouDun(f) || f.MouXiahouDunMobile is not { } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && IsMouXiahouDunMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouDunMobileMovementEvent>().Contains(new(f.Id, m.CardId, m.From, m.To, m.Reason, m.Sequence)));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouXiahouDunObserverEdge(int n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[n - 1], _resolutionStackOperations._resolutionStack[n]) || _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && MouXiahouDunFirstChild(f, _resolutionStackOperations._resolutionStack[n]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]);
    internal bool MouXiahouDunObserverSuffix(int i) => _resolutionStackOperations._resolutionStack[i + 1] is DyingFrame dying && i + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i + 1, dying) || _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => this.MouXiahouDunObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouXiahouDunObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.MouXiahouDunFirstChild(f, c), (n) => this.MouXiahouDunObserverSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 31200)]
    internal bool IsMouXiahouDunDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && MouXiahouDunObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 31200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 31200)]
    internal bool IsMouXiahouDunRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return index >= 0 && child > index + 1 && MouXiahouDunFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && MouXiahouDunObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 31200)]
    internal bool ResumeMouXiahouDun(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouXiahouDunMobile: { } r } f || f.Id != id)
            return false;
        if (r.Stage == MouXiahouDunMobileStage.Damaging)
        {
            if (f.AttackAttempt is not null || f.AttackReturn is not null)
                return true;
            if (!ValidMouXiahouDun(f) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouDunMobileDamageIssuedEvent>().Count(e => e == new MouXiahouDunMobileDamageIssuedEvent(f.Id, r.Index, f.OwnerSeat, r.Targets[r.Index])) != 1)
                throw new InvalidOperationException("Ganglie requires the exact completed native damage before its next target.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouDunMobile = r with { Index = r.Index + 1 } });
            IssueMouXiahouDunDamage(f);
            return true;
        }

        ContinueMouXiahouDunMovement(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 31200)]
    internal bool ReturnMouXiahouDunMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouXiahouDun(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 31200)]
    internal bool AdvanceMouXiahouDunChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouXiahouDunObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 31200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 31200)]
    internal bool ContinueAcceptedMouXiahouDun(ProgramSkillFrame f) => f.MouXiahouDunMobile is { Stage: not MouXiahouDunMobileStage.SelectTargets } && ValidMouXiahouDun(f);
}
