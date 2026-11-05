#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanLeRuntime
{
    internal bool DiouRootMatches(ProgramSkillFrame f)
    {
        if (f.Diou is not { } r || f.SkillId != DiouSkill || f.TriggerId != DiouBinding || f.ActivationId != DiouBinding || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Claim.Source != DiouSource(f) || r.Claim.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveDiouTanbanUse || f.WindowContext is not { } c || c.ParentFrameId != r.WindowId || c.Window != SkillProgramTriggerWindow.CardUseCompleted || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.WindowId)is not ProgramCardTriggerWindowFrame w || w.ParentFrameId != r.Claim.NativeParentId || !ValidDiouClaim(r.Claim, w.Action) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)?.DiouClaims?.Contains(r.Claim) != true || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c, w))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == w.Id && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiouStartedEvent>().Count(e => e == new DiouStartedEvent(f.Id, r.Claim, w.Id)) == 1;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29042)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29042)]
    internal bool PermitPaidDiou(ProgramSkillFrame f)
    {
        if (f.Diou is { Paid: true })
        {
            return true;
        }

        return f.Diou is null && f.InstructionIndex is 0 or 1 && f.TriggerId == DiouBinding && f.WindowContext is { } c && IsIssuedDiouCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex), c);
    }
    internal bool DiouDrawMovement(ProgramSkillFrame f, CardMovementRecord m) => f.Diou is { Stage: DiouStage.DrawChildren, DrawOutcome: { } o } && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter && (o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == DiouDrawReason || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(f.OwnerSeat) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
    private void CapturePaidDiouCursorSuspensions(ProgramSkillFrame issuer)
    {
        var receipt = issuer.Diou!;
        var mother = _resolutionStackOperations._resolutionStack.Single(frame => frame.Id == receipt.WindowId)
            as ProgramCardTriggerWindowFrame ?? throw new InvalidOperationException("Diou lost its original completed-use window.");
        if (mother.AttackOwnerFrameId is { } ownerId && mother.ParentFrameId == ownerId &&
            _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId) is { Action: { } action, CardAttack: not null } original)
        {
            var dodge = mother.CompletedResponseReturn is { Kind: ProgramCompletedResponseKind.Dodge, IsCommitted: false };
            if (dodge ? mother.Action.ParentActionId == action.ActionId :
                mother.Continuation == ProgramCardContinuation.CompletedSlash &&
                mother.Action.ActionId == action.ActionId && original.Step == ResolutionFrameStep.Completed)
                _nativeCursorSuspensions.Suspend(ownerId, issuer.Id, NativeCursorScope.AttackAndContinuations);
        }

        CapturePaidDiouRescueSuspensions(issuer, mother);
    }

    private void CapturePaidDiouRescueSuspensions(ProgramSkillFrame issuer, ProgramCardTriggerWindowFrame mother)
    {
        if (mother.Continuation != ProgramCardContinuation.CompletedCard || mother.CompletedResponseReturn is not null ||
            _coreCardUseLifecycleOperations.LifecycleCardUse(mother.ParentFrameId) is not
                { Step: ResolutionFrameStep.Completed, Action: { } action, DyingResponse: { } rescue } original ||
            _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == rescue.ResolutionId) is not DyingFrame dying ||
            original.CardKind is not (CardKind.Peach or CardKind.Alcohol) || action.EffectiveKind != original.CardKind ||
            original.SourceSeat != action.ActorSeat || action.ActorSeat != issuer.Diou!.Claim.ActorSeat ||
            !original.TargetSeats.SequenceEqual([dying.VictimSeat]) || rescue.ResponderSeat != original.SourceSeat ||
            (original.CardKind == CardKind.Peach ? !rescue.UsedPeach : !rescue.UsedAlcohol || original.SourceSeat != dying.VictimSeat))
            return;
        var dyingIndex = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == dying.Id);
        if (dyingIndex < 0 || _resolutionStackOperations._resolutionStack.ElementAtOrDefault(dyingIndex + 1)?.Id != original.Id ||
            _resolutionStackOperations._resolutionStack.ElementAtOrDefault(dyingIndex + 2)?.Id != mother.Id ||
            _resolutionStackOperations._resolutionStack.ElementAtOrDefault(dyingIndex + 3)?.Id != issuer.Id)
            return;
        _nativeCursorSuspensions.Suspend(dying.Id, issuer.Id, NativeCursorScope.Dying);

        ResolutionFrame? owner;
        DamageFrame? damage = null;
        if (dying.Continuation == DyingContinuationKind.Damage)
        {
            if (dyingIndex < 1 || _resolutionStackOperations._resolutionStack[dyingIndex - 1] is not DamageFrame attempt ||
                attempt.Id != dying.ParentFrameId || attempt.TargetSeat != dying.VictimSeat || attempt.Amount <= 0)
                return;
            damage = attempt;
            owner = _resolutionStackOperations._resolutionStack.Take(dyingIndex - 1)
                .SingleOrDefault(frame => frame.Id == attempt.ParentFrameId);
        }
        else if (dying.Continuation == DyingContinuationKind.AttackHpLoss)
            owner = dyingIndex > 0 && _resolutionStackOperations._resolutionStack[dyingIndex - 1].Id == dying.ParentFrameId
                ? _resolutionStackOperations._resolutionStack[dyingIndex - 1] : null;
        else
            return;
        var attack = owner switch
        {
            CardUseFrame use => use.CardAttack,
            JudgmentFrame judgment => judgment.CardAttack,
            ProgramSkillFrame program => program.CardAttack,
            _ => null
        };
        if (attack is not null && attack.TargetSeat == dying.VictimSeat &&
            (attack.IsSourceLess || dying.KillerSeat == attack.SourceSeat) &&
            (damage is null || damage.SourceSeat == attack.SourceSeat && damage.Amount == attack.DamageAmount && attack.DamageWasApplied))
            _nativeCursorSuspensions.Suspend(owner!.Id, issuer.Id, NativeCursorScope.AttackAndContinuations);
    }

    internal void RestoreDiouParentAttack(ProgramSkillFrame f)
    {
        RestoreDiouRescueAttack(f);
        if (f.Diou is not { Paid: true, Returned: true, UseId: > 0 } receipt ||
            _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == receipt.WindowId) is not
                ProgramCardTriggerWindowFrame { AttackOwnerFrameId: { } ownerId } ||
            !_nativeCursorSuspensions.OwnsSuspension(ownerId, f.Id, NativeCursorScope.AttackAndContinuations))
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(receipt.UseId) is not null ||
            _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId) is not { Action: { } action, CardAttack: not null } ||
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } active && active.ResolutionId != ownerId)
            throw new InvalidOperationException("Diou cannot restore another attack or return before its exact native child has retired.");
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack,
                _kernelCoreMainOperations.CreateCardAttackHandle2(ownerId)))
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = _kernelCoreMainOperations.CreateCardAttackHandle2(ownerId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiouParentAttackRestoredEvent(
                f.Id, receipt.UseId, ownerId, action.ActionId));
        }
    }

    internal ResolutionFrame? DiouRescueAttackOwner(ProgramSkillFrame issuer, out DyingFrame? dying)
    {
        dying = _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().SingleOrDefault(frame =>
            _nativeCursorSuspensions.OwnsSuspension(frame.Id, issuer.Id, NativeCursorScope.Dying));
        return dying is null ? null : _resolutionStackOperations._resolutionStack.SingleOrDefault(frame =>
            _nativeCursorSuspensions.OwnsSuspension(frame.Id, issuer.Id, NativeCursorScope.AttackAndContinuations));
    }

    internal void RestoreDiouRescueAttack(ProgramSkillFrame f)
    {
        if (f.Diou is not { Paid: true, Returned: true } receipt || DiouRescueAttackOwner(f, out var dying) is not { } owner)
            return;
        if (receipt.UseId > 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(receipt.UseId) is not null ||
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } active && active.ResolutionId != owner.Id)
            throw new InvalidOperationException("Diou cannot restore another rescue attack or expose its dying cursor before the exact child returned.");
        _coreCardAttackStateOperations.UpdateCardAttackState(owner.Id, state => state! with { Active = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiouRescueAttackReturnedEvent(
            f.Id, receipt.UseId, dying!.Id, owner.Id, (owner as CardUseFrame)?.Action?.ActionId));
    }

    internal bool DiouFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Diou is not { Paid: true } r || !DiouRootMatches(f))
            return false;
        if (child is CardUseFrame use)
            return r.Stage == DiouStage.NativeUse && use.Id == r.UseId && use.DiouOrigin?.ProgramFrameId == f.Id && IsIssuedDiouUse(use);
        if (r.Stage == DiouStage.Drawing && f.NativeDraw is not null)
            return CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && (r.Stage == DiouStage.NativeChildren && moved.ResumeProgramFrameId == f.Id && IsDiouNativeSettlementBatch(f, moved.Batch) || r.Stage == DiouStage.DrawChildren && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => DiouDrawMovement(f, m)));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
    }

    internal bool DiouNativeWindowEdge(int index)
    {
        if (_resolutionStackOperations._resolutionStack[index - 1] is CardUseFrame use && IsIssuedDiouUse(use) && _resolutionStackOperations._resolutionStack[index] is ProgramCardTriggerWindowFrame w)
            return w.ParentFrameId == use.Id && w.Action.ActionId == use.Action!.ActionId && w.Action.ActorSeat == use.SourceSeat && w.Action.EffectiveKind == use.CardKind;
        if (_resolutionStackOperations._resolutionStack[index - 1] is ProgramCardTriggerWindowFrame parent && _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is { } native && IsIssuedDiouUse(native) && parent.Action.ActionId == native.Action!.ActionId && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame child && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count)
        {
            var c = parent.Candidates[parent.CandidateIndex];
            return CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(child, CoreGameEngineCardActionsRuntime.ToSharedCandidate(c)) && child.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(parent, c);
        }

        return _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame lifecycle && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame f && lifecycle.CandidateIndex >= 0 && lifecycle.CandidateIndex < lifecycle.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, lifecycle.Candidates[lifecycle.CandidateIndex]) && f.WindowContext is { } context && context.ParentFrameId == lifecycle.Id && context.Window == lifecycle.Window;
    }

    internal bool DiouObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || this.DiouNativeWindowEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame p && (this.DiouFirstChild(p, _resolutionStackOperations2._resolutionStack()[n]) || p.Zenrun is not null && _characterSunHongOlOperations.ZenrunFirstChild(p, _resolutionStackOperations2._resolutionStack()[n])) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? DiouObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.DiouFirstChild(f, child), (index) => this.DiouObserverSuffix(index));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 480)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 470)]
    internal bool IsDiouRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.Diou is null || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && DiouFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && DiouObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29042)]
    internal bool AdvanceDiouChildren()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { Diou: { Stage: DiouStage.NativeChildren, Returned: true } } f)
        {
            ContinueDiou(f.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return true;
        }

        return DiouObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    }
}
