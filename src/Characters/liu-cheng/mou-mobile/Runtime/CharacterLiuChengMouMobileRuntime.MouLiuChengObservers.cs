#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuChengMouMobileRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 29401)]
    internal void BindMouLiuChengIssuedCard(IGameEvent fact)
    {
        if (fact is CardUseDeclaredEvent e && _coreCardUseLifecycleOperations.LifecycleCardUse(e.ResolutionId)is { } use)
            BindMouLiuChengNativeUse(use);
    }

    internal bool MouLiuChengRootMatches(ProgramSkillFrame f)
    {
        if (f.MouLiuChengCompletion is not { } r || !MouLiuChengComposition.IsSkill(f.SkillId) || f.TriggerId != "after-use" || f.ActivationId != "after-use" || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Claim.Source != MouLiuChengSource(f) || r.Claim.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveMouLiuChengCompletion || f.WindowContext is not { } c || c.ParentFrameId != r.WindowId || c.Window != SkillProgramTriggerWindow.CardUseCompleted || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.WindowId)is not ProgramCardTriggerWindowFrame w || w.ParentFrameId != r.Claim.NativeParentId || !ValidMouLiuChengClaim(r.Claim, w.Action) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)?.MouLiuChengClaims?.Contains(r.Claim) != true || !MouLiuChengCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c, w))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == w.Id && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengCompletionStartedEvent>().Count(e => e == new MouLiuChengCompletionStartedEvent(f.Id, r.Claim, w.Id, r.MarkBefore, r.MarkAfter)) == 1;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29401)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29401)]
    internal bool PermitPaidMouLiuCheng(ProgramSkillFrame f)
    {
        if (f.MouLiuChengCompletion is not { Paid: true })
            return false;
        return true;
    }
    internal bool MouLiuChengDrawMovement(ProgramSkillFrame f, CardMovementRecord m) => f.MouLiuChengCompletion is { Stage: MouLiuChengStage.DrawChildren, DrawOutcome: { } o } && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter && (o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MouLiuChengDrawReason || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(f.OwnerSeat) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
    internal void RestoreMouLiuChengParentAttack(ProgramSkillFrame f)
    {
        if (f.MouLiuChengCompletion is not { Paid: true, Returned: true, UseId: > 0 } r ||
            _resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == r.WindowId) is not ProgramCardTriggerWindowFrame window ||
            window.AttackOwnerFrameId is not { } originalId ||
            !_nativeCursorSuspensions.OwnsSuspension(originalId, f.Id, NativeCursorScope.AttackAndContinuations))
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId) is not null ||
            _coreCardUseLifecycleOperations.LifecycleCardUse(originalId) is not { Action: { } action, CardAttack: not null } ||
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } active && active.ResolutionId != originalId)
            throw new InvalidOperationException("MouLiuCheng cannot restore another attack or return before its exact native child has retired.");
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack,
                _kernelCoreMainOperations.CreateCardAttackHandle2(originalId)))
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = _kernelCoreMainOperations.CreateCardAttackHandle2(originalId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(
                new MouLiuChengParentAttackRestoredEvent(f.Id, r.UseId, originalId, action.ActionId));
        }
    }

    internal bool MouLiuChengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouLiuChengCompletion is not { Paid: true } r || !MouLiuChengRootMatches(f))
            return false;
        if (child is CardUseFrame use)
            return r.Stage == MouLiuChengStage.NativeUse && use.Id == r.UseId && use.MouLiuChengOrigin?.ProgramFrameId == f.Id && IsIssuedMouLiuChengUse(use);
        if (r.Stage == MouLiuChengStage.Drawing && f.NativeDraw is not null)
            return CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && (r.Stage == MouLiuChengStage.NativeChildren && moved.ResumeProgramFrameId == f.Id && IsMouLiuChengNativeSettlementBatch(f, moved.Batch) || r.Stage == MouLiuChengStage.DrawChildren && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => MouLiuChengDrawMovement(f, m)));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
    }

    internal bool MouLiuChengNativeWindowEdge(int index)
    {
        if (_resolutionStackOperations._resolutionStack[index - 1] is CardUseFrame use && IsIssuedMouLiuChengUse(use) && _resolutionStackOperations._resolutionStack[index] is ProgramCardTriggerWindowFrame w)
            return w.ParentFrameId == use.Id && w.Action.ActionId == use.Action!.ActionId && w.Action.ActorSeat == use.SourceSeat && w.Action.EffectiveKind == use.CardKind;
        if (_resolutionStackOperations._resolutionStack[index - 1] is ProgramCardTriggerWindowFrame parent && _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is { } native && IsIssuedMouLiuChengUse(native) && parent.Action.ActionId == native.Action!.ActionId && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame child && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count)
        {
            var c = parent.Candidates[parent.CandidateIndex];
            return CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(child, CoreGameEngineCardActionsRuntime.ToSharedCandidate(c)) && child.WindowContext is { } nativeContext && (MouLiuChengComposition.IsSkill(child.SkillId) ? MouLiuChengCandidateMatches(CoreGameEngineCardActionsRuntime.ToSharedCandidate(c), nativeContext, parent) : nativeContext == _coreCardActionsOperations.CreateCardActionProgramContext2(parent, c));
        }

        return _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame lifecycle && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame f && lifecycle.CandidateIndex >= 0 && lifecycle.CandidateIndex < lifecycle.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, lifecycle.Candidates[lifecycle.CandidateIndex]) && f.WindowContext is { } context && context.ParentFrameId == lifecycle.Id && context.Window == lifecycle.Window;
    }

    internal bool MouLiuChengObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || this.MouLiuChengNativeWindowEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame p && (this.MouLiuChengFirstChild(p, _resolutionStackOperations2._resolutionStack()[n]) || p.Zenrun is not null && _characterSunHongOlOperations.ZenrunFirstChild(p, _resolutionStackOperations2._resolutionStack()[n])) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MouLiuChengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.MouLiuChengFirstChild(f, child), (index) => this.MouLiuChengObserverSuffix(index));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 490)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 480)]
    internal bool IsMouLiuChengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.MouLiuChengCompletion is null || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && MouLiuChengFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && MouLiuChengObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29401)]
    internal bool AdvanceMouLiuChengChildren()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { MouLiuChengCompletion: { Stage: MouLiuChengStage.NativeChildren, Returned: true } } f)
        {
            ContinueMouLiuCheng(f.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return true;
        }

        return MouLiuChengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    }
}
