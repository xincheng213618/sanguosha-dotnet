#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangXiuMouOlRuntime
{
    internal bool MouZhangXiuOlParentMatches(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } w)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (n < 1 || _resolutionStackOperations._resolutionStack[n - 1].Id != w.ParentFrameId)
            return false;
        return _resolutionStackOperations._resolutionStack[n - 1] switch
        {
            ProgramLifecycleTriggerWindowFrame p => p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
            PlayPhaseStartingBoundaryFrame p => w.Window == SkillProgramTriggerWindow.PlayPhaseStarting && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && p.Items[p.ItemIndex].Candidate is { } c && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c),
            TurnEndingBoundaryFrame p => w.Window == SkillProgramTriggerWindow.TurnEnding && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && p.Items[p.ItemIndex].Candidate is { } c && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c),
            ProgramCardTriggerWindowFrame p => p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, CoreGameEngineCardActionsRuntime.ToSharedCandidate(p.Candidates[p.CandidateIndex])) && p.ParentFrameId == w.CardUse?.ParentCardUseFrameId && p.Action.ActionId == w.CardUse.CardActionId,
            _ => false
        };
    }

    internal bool ValidMouZhangXiuOl(ProgramSkillFrame f)
    {
        if (f.MouZhangXiuOl is not { } r || r.Source != MouZhangXiuOlSource(f) || r.GameplayHash != f.GameplayHash || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.WindowContext is not { } w || !Enum.IsDefined(r.Stage) || !MouZhangXiuOlComposition.Handles(r.Operation) || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f).Op != r.Operation || !MouZhangXiuOlParentMatches(f) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuOlStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == f.GameplayHash && e.Operation == r.Operation) != 1)
            return false;
        if (r.Operation is SkillProgramEffectOp.ResolveMouZhangXiuQiulie or SkillProgramEffectOp.UseMouZhangXiuQiulieSlash)
        {
            if (f.SkillId != MouZhangXiuOlComposition.Qiulie || r.Phase is not { } p || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) || p.ActorSeat != f.OwnerSeat || p.ActualTurnOwnerSeat != f.OwnerSeat || p.Kind != MouZhangXiuOlComposition.Phase(w.Window))
                return false;
            if (r.Stage == MouZhangXiuOlStage.Target)
                return r.Operation == SkillProgramEffectOp.ResolveMouZhangXiuQiulie && r.TargetSeat == -1 && r.PaidCardId is null;
            if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) || r.TargetSeat == f.OwnerSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuOlHuntChosenEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.TargetSeat == r.TargetSeat && e.ActualTurn == p.TurnNumber) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuOlPhaseOfferedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.Phase == p && e.TargetSeat == r.TargetSeat) != 1)
                return false;
            if (r.Stage == MouZhangXiuOlStage.Cost)
                return r.PaidCardId is null && r.UseId == 0;
            if (!MouZhangXiuOlPaymentMatches(f))
                return false;
            if (r.Stage == MouZhangXiuOlStage.CostChildren || r.Stage == MouZhangXiuOlStage.Issuing)
                return r.UseId == 0 && !r.NativeReturned;
            return r.Stage is MouZhangXiuOlStage.NativeUse or MouZhangXiuOlStage.Children && r.UseId > 0 && r.ActionId > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuOlQiulieIssuedEvent>().Count(e => e.Origin.FrameId == f.Id && e.Origin.Source == r.Source && e.Origin.GameplayHash == f.GameplayHash && e.Origin.UseId == r.UseId && e.Origin.ActionId == r.ActionId && e.Origin.TargetSeat == r.TargetSeat && e.Origin.Phase == p) == 1;
        }

        if (w.CardUse?.ParentCardUseFrameId != r.UseId || w.CardUse.CardActionId != r.ActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { Action: { } a } use || a.ActionId != r.ActionId)
            return false;
        if (r.Operation == SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash)
            return f.SkillId == MouZhangXiuOlComposition.Qiulie && w.Window == SkillProgramTriggerWindow.CardUseBeforeTargetEffects && use.MouZhangXiuOlQiulie is { } origin && origin.TargetSeat == r.TargetSeat && origin.Source.OwnerSeat == f.OwnerSeat && origin.Source.SkillInstanceId == f.SkillInstanceId && origin.GameplayHash == f.GameplayHash && IsMouZhangXiuOlSlash(use) && (r.Stage == MouZhangXiuOlStage.Defense && r.PaidCardId is null || r.Stage == MouZhangXiuOlStage.DefenseChildren && MouZhangXiuOlPaymentMatches(f));
        if (f.SkillId != MouZhangXiuOlComposition.Zhuijiao || MouZhangXiuOlEntry(use, new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0))is not { } entry)
            return false;
        if (r.Operation == SkillProgramEffectOp.ResolveMouZhangXiuZhuijiao)
            return w.Window == SkillProgramTriggerWindow.CardUseCommitted && (r.Stage == MouZhangXiuOlStage.Drawing && r.Draw is null || r.Stage == MouZhangXiuOlStage.DrawChildren && r.Draw is { RequestedCount: 1 } d && d.RecipientSeat == f.OwnerSeat && r.Before == d.SequenceBefore && r.After == d.SequenceAfter && _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuOlDrawReturnedEvent>().Count(e => e.FrameId == f.Id && e.UseId == use.Id && e.Requested == 1 && e.Actual == d.ActualDrawIds.Count && e.Replacement == d.ReplacementGainedIds.Count) == 1);
        return r.Operation == SkillProgramEffectOp.CompleteMouZhangXiuZhuijiao && w.Window == SkillProgramTriggerWindow.CardUseCompleted && entry.Boosted && entry.DrawReturned && entry.PenaltyIssued && !MouZhangXiuOlUseDamaged(use.Id, null) && (r.Stage == MouZhangXiuOlStage.Penalty && r.PaidCardId is null || r.Stage == MouZhangXiuOlStage.PenaltyChildren && MouZhangXiuOlPaymentMatches(f));
    }

    internal bool MouZhangXiuOlPaymentMatches(ProgramSkillFrame f)
    {
        var r = f.MouZhangXiuOl!;
        if (r.PaidCardId is not { } card || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            return false;
        var defense = r.Operation == SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash;
        var payer = defense ? r.TargetSeat : f.OwnerSeat;
        var reason = defense ? MouZhangXiuOlDefenseReason : r.Operation == SkillProgramEffectOp.CompleteMouZhangXiuZhuijiao ? MouZhangXiuOlPenaltyReason : MouZhangXiuOlCostReason;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuOlDiscardPaidEvent>().Count(e => e.FrameId == f.Id && e.PayerSeat == payer && e.CardId == card && e.Before == r.Before && e.After == r.After && e.Defense == defense) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == card && m.From.OwnerSeat == payer && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == reason) == 1;
    }
    internal bool IsMouZhangXiuOlMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation movement) => f.MouZhangXiuOl is { Stage: MouZhangXiuOlStage.CostChildren or MouZhangXiuOlStage.DefenseChildren or MouZhangXiuOlStage.PenaltyChildren or MouZhangXiuOlStage.DrawChildren or MouZhangXiuOlStage.Children } r && effect?.Op == r.Operation && movement == f.PendingMovementContinuation && movement.SubjectSeat == f.OwnerSeat && movement.BeforeCount == 0 && movement.CoverageResultBind is null && ValidMouZhangXiuOl(f);
    internal bool MouZhangXiuOlLionPayment(ProgramSkillFrame f, int seat, CardMoveReason? reason) => f.MouZhangXiuOl is { PaidCardId: { } card } r && r.Stage is MouZhangXiuOlStage.CostChildren or MouZhangXiuOlStage.DefenseChildren or MouZhangXiuOlStage.PenaltyChildren && seat == (r.Operation == SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash ? r.TargetSeat : f.OwnerSeat) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == card && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(seat) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason == reason);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 33000)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 33000)]
    internal bool IsMouZhangXiuOlRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => ValidMouZhangXiuOl(f) && f.PendingMovementContinuation is not null && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && MouZhangXiuOlLionPayment(f, recovery.Attempt.TargetSeat, recovery.Attempt.Completion.MoveReason);
    internal bool MouZhangXiuOlFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouZhangXiuOl is not { } r || !ValidMouZhangXiuOl(f))
            return false;
        if (CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouZhangXiuOlStage.Drawing && f.NativeDraw is not null;
        if (child is RecoveryReplacementFrame recovery)
            return IsMouZhangXiuOlRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == hp.Change.TargetSeat && MouZhangXiuOlLionPayment(f, hp.Change.TargetSeat, new(r.Operation == SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash ? MouZhangXiuOlDefenseReason : r.Operation == SkillProgramEffectOp.CompleteMouZhangXiuZhuijiao ? MouZhangXiuOlPenaltyReason : MouZhangXiuOlCostReason));
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } pending && IsMouZhangXiuOlMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (r.Stage == MouZhangXiuOlStage.NativeUse && child is CardUseFrame native)
            return IsMouZhangXiuOlSlash(native);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouZhangXiuOlNativeEdge(int n)
    {
        if (_resolutionStackOperations._resolutionStack[n - 1] is CardUseFrame { MouZhangXiuOlQiulie: not null } use && IsMouZhangXiuOlSlash(use) && _resolutionStackOperations._resolutionStack[n] is ProgramCardTriggerWindowFrame window)
            return window.ParentFrameId == use.Id && window.Action.ActionId == use.Action!.ActionId;
        if (_resolutionStackOperations._resolutionStack[n - 1] is ProgramCardTriggerWindowFrame parent && _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is { MouZhangXiuOlQiulie: not null } native && IsMouZhangXiuOlSlash(native) && _resolutionStackOperations._resolutionStack[n] is ProgramSkillFrame f && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count)
        {
            var c = parent.Candidates[parent.CandidateIndex];
            return CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, CoreGameEngineCardActionsRuntime.ToSharedCandidate(c)) && f.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(parent, c);
        }

        return false;
    }

    internal bool MouZhangXiuOlObserverEdge(int n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[n - 1], _resolutionStackOperations._resolutionStack[n]) || MouZhangXiuOlNativeEdge(n) || _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && MouZhangXiuOlFirstChild(f, _resolutionStackOperations._resolutionStack[n]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]);
    internal bool MouZhangXiuOlObserverSuffix(int n) => _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => this.MouZhangXiuOlObserverEdge(i), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? MouZhangXiuOlObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.MouZhangXiuOlFirstChild(f, c), (n) => this.MouZhangXiuOlObserverSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 33000)]
    internal bool IsMouZhangXiuOlDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouZhangXiuOlObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 33000)]
    internal bool AdvanceMouZhangXiuOlChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouZhangXiuOlObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 33000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 33000)]
    internal bool ContinuePaidMouZhangXiuOl(ProgramSkillFrame f) => f.MouZhangXiuOl is { } r && ValidMouZhangXiuOl(f) && (r.PaidCardId is not null || r.Operation is SkillProgramEffectOp.ResolveMouZhangXiuZhuijiao or SkillProgramEffectOp.CompleteMouZhangXiuZhuijiao or SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash) || f.MouZhangXiuOl is null && f.InstructionIndex is 0 or 1 && f.WindowContext is { } w && CanRunIssuedMouZhangXiuOl(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), w);
}
