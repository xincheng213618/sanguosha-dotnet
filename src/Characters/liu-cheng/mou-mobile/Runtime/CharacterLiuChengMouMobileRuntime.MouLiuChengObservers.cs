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
    internal bool IsMouLiuChengSuspendedCompletedAttack(CardUseFrame original, long? requiredWindowId = null)
    {
        // The completed mother owns its exact suspended interval through requested
        // Draw1, gain children and the separate native followup. This predicate
        // never queries an active-token getter, preventing recursive cursors.
        if (original.Action is not { } action || original.CardAttack is null)
            return false;
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>())
        {
            if (f.MouLiuChengCompletion is not { Paid: true } r || requiredWindowId is { } exactWindow && r.WindowId != exactWindow || r.Stage is not (MouLiuChengStage.UseChoice or MouLiuChengStage.NativeIssuing or MouLiuChengStage.NativeUse or MouLiuChengStage.NativeChildren or MouLiuChengStage.Drawing or MouLiuChengStage.DrawChildren) || !MouLiuChengComposition.IsSkill(f.SkillId) || f.TriggerId != "after-use" || f.ActivationId != "after-use" || r.Claim.Source != MouLiuChengSource(f) || r.Claim.GameplayHash != f.GameplayHash || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.WindowId)is not ProgramCardTriggerWindowFrame mother || mother.ParentFrameId != original.Id || mother.AttackOwnerFrameId != original.Id || r.Claim.NativeParentId != original.Id || r.Claim.ActionId != mother.Action.ActionId || original.MouLiuChengClaims?.Contains(r.Claim) != true || !ValidMouLiuChengClaim(r.Claim, mother.Action) || f.WindowContext?.ParentFrameId != mother.Id || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengCompletionStartedEvent>().Count(e => e == new MouLiuChengCompletionStartedEvent(f.Id, r.Claim, mother.Id, r.MarkBefore, r.MarkAfter)) != 1)
                continue;
            var response = mother.CompletedResponseReturn is { Kind: ProgramCompletedResponseKind.Dodge, IsCommitted: false };
            if (response ? mother.Action.ParentActionId != action.ActionId : mother.Continuation != ProgramCardContinuation.CompletedSlash || mother.Action.ActionId != action.ActionId || original.Step != ResolutionFrameStep.Completed)
                continue;
            var fi = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
            if (fi < 1 || _resolutionStackOperations._resolutionStack[fi - 1].Id != mother.Id || _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == original.Id) >= fi - 1)
                continue;
            if (r.Stage == MouLiuChengStage.NativeUse && (_coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { MouLiuChengOrigin: { } origin, Action: { } childAction } child || _resolutionStackOperations._resolutionStack.ElementAtOrDefault(fi + 1)?.Id != child.Id || origin.ProgramFrameId != f.Id || origin.UseId != child.Id || origin.ActionId != childAction.ActionId || origin.Source != r.Claim.Source || origin.GameplayHash != f.GameplayHash || childAction.ParentActionId != action.ActionId || childAction.PhysicalCards.Count != 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengNativeIssuedEvent>().Count(e => e.FrameId == f.Id && e.UseId == child.Id && e.ActionId == childAction.ActionId && e.ActorSeat == f.OwnerSeat && e.Kind == child.CardKind) != 1))
                continue;
            if (r.Stage == MouLiuChengStage.NativeChildren && (!r.Returned || r.UseId > 0 && (_coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengNativeReturnedEvent>().Count(e => e.FrameId == f.Id && e.UseId == r.UseId && e.ActionId == r.ActionId) != 1)))
                continue;
            return true;
        }

        return false;
    }

    internal void RestoreMouLiuChengParentAttack(ProgramSkillFrame f)
    {
        if (f.MouLiuChengCompletion is not { Paid: true, Returned: true, UseId: > 0 } r || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.WindowId)is not ProgramCardTriggerWindowFrame w || w.AttackOwnerFrameId is not { } originalId)
            return;
        var response = w.CompletedResponseReturn is { Kind: ProgramCompletedResponseKind.Dodge, IsCommitted: false };
        if (!response && w.Continuation != ProgramCardContinuation.CompletedSlash)
            return;
        if (!MouLiuChengRootMatches(f) || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengNativeReturnedEvent>().Count(e => e == new MouLiuChengNativeReturnedEvent(f.Id, r.UseId, r.ActionId)) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == r.UseId) != 1 || w.ParentFrameId != originalId || _coreCardUseLifecycleOperations.LifecycleCardUse(originalId)is not { Action: { } a, CardAttack: not null } original || (response ? w.Action.ParentActionId != a.ActionId : w.Action.ActionId != a.ActionId || original.Step != ResolutionFrameStep.Completed) || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } active && active.ResolutionId != originalId)
            throw new InvalidOperationException("MouLiuCheng cannot restore another attack or return before its exact native child has retired.");
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, _kernelCoreMainOperations.CreateCardAttackHandle2(originalId)))
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = _kernelCoreMainOperations.CreateCardAttackHandle2(originalId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengParentAttackRestoredEvent(f.Id, r.UseId, originalId, a.ActionId));
        }
    }

    internal bool IsMouLiuChengSuspendedCounterspell(NullificationWindowFrame window)
    {
        // A true counterspell completion can itself issue MouLiuCheng's native trick.
        // Keep the old response cursor on its exact frame while the separate
        // child gets the active Nullification window; do not hide any sibling.
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>())
        {
            if (f.MouLiuChengCompletion is not { Paid: true, Stage: MouLiuChengStage.NativeUse } r || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.WindowId)is not ProgramCardTriggerWindowFrame mother || mother.CompletedResponseReturn is not { Kind: ProgramCompletedResponseKind.Nullification, IsCommitted: false } || mother.ParentFrameId != window.Id || r.Claim.NativeParentId != window.Id || r.Claim.ActionId != mother.Action.ActionId || window.MouLiuChengClaims?.Contains(r.Claim) != true || r.Claim.Source != MouLiuChengSource(f) || r.Claim.GameplayHash != f.GameplayHash || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { MouLiuChengOrigin: { } origin, Action: { } a } child || origin.ProgramFrameId != f.Id || origin.UseId != child.Id || origin.ActionId != a.ActionId || origin.Source != r.Claim.Source || origin.GameplayHash != f.GameplayHash || a.PhysicalCards.Count != 0 || a.ParentActionId != _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)?.Action?.ActionId || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengCompletionStartedEvent>().Count(e => e.FrameId == f.Id && e.WindowId == mother.Id && e.Claim == r.Claim) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengNativeIssuedEvent>().Count(e => e.FrameId == f.Id && e.UseId == child.Id && e.ActionId == a.ActionId && e.ActorSeat == f.OwnerSeat && e.Kind == child.CardKind) != 1)
                continue;
            return true;
        }

        return false;
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
