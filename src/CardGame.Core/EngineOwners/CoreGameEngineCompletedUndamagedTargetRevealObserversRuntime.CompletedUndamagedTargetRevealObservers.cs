#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime
{
    internal bool ExactCompletedUndamagedParent(ProgramSkillFrame f, ProgramSkillWindowContext context, CardUseFrame use)
    {
        if (context.CardUse is not { } original)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == f.Id);
        if (index < 2 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramCardTriggerWindowFrame window || window.Id != context.ParentFrameId || window.ParentFrameId != use.Id || !window.Activated || window.Continuation is not (ProgramCardContinuation.CompletedCard or ProgramCardContinuation.CompletedSlash) || window.CompletedResponseReturn is not null || CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != SkillProgramTriggerWindow.CardUseCompleted || window.Action.Type != CardActionType.Use || window.Action.ActionId != use.Action?.ActionId || _resolutionStackOperations._resolutionStack[index - 2].Id != use.Id || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return false;
        var c = window.Candidates[window.CandidateIndex];
        return CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, CoreGameEngineCardActionsRuntime.ToSharedCandidate(c)) && _coreCardActionsOperations.CreateCardActionProgramContext2(window, c) == context && original.ParentCardUseFrameId == use.Id && original.CardActionId == window.Action.ActionId && _coreCompletedUndamagedTargetRevealOperations.HasExactAcceptedCompletedUndamagedUse(use, window.Action);
    }

    internal bool ValidCompletedUndamagedTargetRevealReceipt(ProgramSkillFrame f)
    {
        if (f.CompletedUndamagedTargetReveal is not { } r || f.WindowContext is not { } context || f.TriggerId is null || r.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Source != new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || !Enum.IsDefined(r.Stage) || r.WindowFrameId != context.ParentFrameId || _coreCompletedUndamagedTargetRevealOperations.CompletedUndamagedUse(context)is not { Action: { } action } use || !ExactCompletedUndamagedParent(f, context, use) || use.Id != r.CardUseFrameId || action.ActionId != r.CardActionId || use.CardKind != r.EffectiveKind || use.SourceSeat != f.OwnerSeat || !_coreCompletedUndamagedTargetRevealOperations.CompletedUndamagedSourceUseQualifies(f.SkillId, use) || !r.FinalTargets.SequenceEqual(use.TargetSeats) || r.EligibleTargets.Count == 0 || r.EligibleTargets.Distinct().Count() != r.EligibleTargets.Count || r.EligibleTargets.Any(s => !r.FinalTargets.Contains(s)))
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        if (plan.Trigger is not { Window: SkillProgramTriggerWindow.CardUseCompleted, Optional: true, IncludeResponseUses: false, OwnerRelation: SkillProgramCardActionOwnerRelation.Actor } trigger || trigger.Effects is not [{ Op: SkillProgramEffectOp.RevealUndamagedUseTargetHandAndDiscardSameColor, Target: SkillProgramEffectTarget.Owner, Amount: 3, Condition.Kind: SkillProgramConditionKind.Always }] || plan.Instructions.Count != 1 || plan.Instructions[0].Op != SkillProgramEffectOp.RevealUndamagedUseTargetHandAndDiscardSameColor)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.OwnerSeat == f.OwnerSeat && e.Window == SkillProgramTriggerWindow.CardUseCompleted) != 1)
            return false;
        var damage = history.OfType<CompletedUndamagedUseDamageRecordedEvent>().Where(e => e.CardUseFrameId == use.Id).ToArray();
        if (damage.Select(e => e.DamageFrameId).Distinct().Count() != damage.Length || damage.Any(e => e.Amount <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(e.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(e.TargetSeat) || e.ActorSeat != use.SourceSeat || e.EffectiveKind != use.CardKind || e.CardActionId is { } id && id != action.ActionId || history.OfType<DamageRequestedEvent>().Count(d => d.ResolutionId == e.DamageFrameId && d.SourceSeat == e.SourceSeat && d.TargetSeat == e.TargetSeat && d.Amount == e.Amount && d.Nature == e.Nature && d.SourceLess == e.SourceLess && d.SourceCard == e.EffectiveKind) != 1) || r.EligibleTargets.Any(s => damage.Any(e => e.TargetSeat == s)))
            return false;
        foreach (var fact in damage)
        {
            var requested = Array.FindIndex(history, e => e is DamageRequestedEvent d && d.ResolutionId == fact.DamageFrameId);
            if (history.Skip(requested + 1).TakeWhile(e => e is not DamageRequestedEvent).OfType<DamageAppliedEvent>().FirstOrDefault()is not { } applied || applied.SourceSeat != fact.SourceSeat || applied.TargetSeat != fact.TargetSeat || applied.Amount != fact.Amount || applied.Nature != fact.Nature || applied.SourceLess != fact.SourceLess)
                return false;
        }

        if (history.OfType<CompletedUndamagedTargetRevealStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start.Source != r.Source || start.GameplayHash != r.GameplayHash || start.WindowFrameId != r.WindowFrameId || start.CardUseFrameId != r.CardUseFrameId || start.CardActionId != r.CardActionId)
            return false;
        var selections = history.OfType<CompletedUndamagedTargetRevealSelectionIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var movements = history.OfType<CompletedUndamagedTargetRevealMovementIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var completions = history.OfType<CompletedUndamagedTargetRevealCompletedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var reveals = history.OfType<ProgramCardsRevealedEvent>().Where(e => e.FrameId == f.Id && e.Bind == "completed-undamaged-target-reveal").ToArray();
        if (r.Stage == CompletedUndamagedTargetRevealStage.ChoosingTarget)
            return r.TargetSeat is null && r.TargetHand.Count == 0 && r.SelectedSlots.Count == 0 && !r.SelectionIssued && r.RevealedMaterials.Count == 0 && r.PaidMaterials.Count == 0 && !r.SameColor && !r.MovementIssued && r.SequenceBefore == 0 && r.SequenceAfter == 0 && r.BatchId is null && f.PendingMovementContinuation is null && selections.Length == 0 && movements.Length == 0 && completions.Length == 0 && reveals.Length == 0;
        if (r.TargetSeat is not { } target || !r.EligibleTargets.Contains(target) || r.TargetHand.Select(m => m.CardId).Distinct().Count() != r.TargetHand.Count || r.TargetHand.Where((m, slot) => m.Slot != slot || m.CardId <= 0 || !Enum.IsDefined(m.PrintedKind)).Any() || r.SelectedSlots.Distinct().Count() != r.SelectedSlots.Count || r.SelectedSlots.Count > Math.Min(3, r.TargetHand.Count) || r.SelectedSlots.Any(s => s < 0 || s >= r.TargetHand.Count))
            return false;
        if (r.Stage == CompletedUndamagedTargetRevealStage.ChoosingCards)
            return !r.SelectionIssued && r.SelectedSlots.Count < Math.Min(3, r.TargetHand.Count) + (r.TargetHand.Count == 0 ? 1 : 0) && r.RevealedMaterials.Count == 0 && r.PaidMaterials.Count == 0 && !r.SameColor && !r.MovementIssued && r.SequenceBefore == 0 && r.SequenceAfter == 0 && r.BatchId is null && f.PendingMovementContinuation is null && selections.Length == 0 && movements.Length == 0 && completions.Length == 0 && reveals.Length == 0;
        if (!r.SelectionIssued || selections is not [var selection] || selection.TargetSeat != target || selection.SelectedCount != r.SelectedSlots.Count || selection.RevealedCount != r.RevealedMaterials.Count || selection.SameColor != r.SameColor || r.RevealedMaterials.Select(m => m.CardId).Distinct().Count() != r.RevealedMaterials.Count || r.RevealedMaterials.Any(m => !r.SelectedSlots.Contains(m.Slot) || r.TargetHand[m.Slot].CardId != m.CardId || r.TargetHand[m.Slot].PrintedKind != m.PrintedKind || !Enum.IsDefined(m.EffectiveSuit) || m.EffectiveColor != CoreGameEngineCompletedUndamagedTargetRevealRuntime.CompletedUndamagedColor(m.EffectiveSuit)) || r.SameColor != (r.RevealedMaterials.Count > 0 && r.RevealedMaterials.All(m => m.EffectiveColor == r.RevealedMaterials[0].EffectiveColor)) || r.PaidMaterials.Select(m => m.CardId).Distinct().Count() != r.PaidMaterials.Count || r.PaidMaterials.Any(m => !r.RevealedMaterials.Contains(m)) || !r.SameColor && r.PaidMaterials.Count > 0 || r.SequenceBefore < 0 || r.SequenceAfter < r.SequenceBefore || r.SequenceAfter > _coreCompletedUndamagedTargetRevealOperations.CompletedUndamagedMovementSequence)
            return false;
        if (r.RevealedMaterials.Count == 0 ? reveals.Length != 0 : reveals is not [var shown] || shown.SkillId != f.SkillId || shown.BindingId != CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) || shown.OwnerSeat != f.OwnerSeat || !shown.Cards.Select(c => c.Id).SequenceEqual(r.RevealedMaterials.Select(m => m.CardId)) || shown.Cards.Zip(r.RevealedMaterials).Any(p => p.First.Kind != p.Second.PrintedKind || p.First.Suit != p.Second.EffectiveSuit))
            return false;
        if (_characterXiaoQiaoMouOlOperations.IsPublicHandRevealProducerSuspended(f.Id, "completed-target-hand-reveal"))
            return !r.MovementIssued && r.BatchId is null && r.SequenceBefore == r.SequenceAfter && f.PendingMovementContinuation is null && movements.Length == 0 && completions.Length == 0;
        if (r.PaidMaterials.Count == 0)
            return r.Stage == CompletedUndamagedTargetRevealStage.Complete && !r.MovementIssued && r.BatchId is null && r.SequenceAfter == r.SequenceBefore && f.PendingMovementContinuation is null && movements.Length == 0 && completions is [var empty] && empty.SelectedCount == r.SelectedSlots.Count && empty.ActualDiscardCount == 0;
        if (!r.MovementIssued || r.BatchId is not { } batchId || movements is not [var paid] || paid.TargetSeat != target || paid.ActualCount != r.PaidMaterials.Count || paid.SequenceBefore != r.SequenceBefore || paid.SequenceAfter != r.SequenceAfter || paid.BatchId != batchId)
            return false;
        var records = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter).ToArray();
        if (records.Length != r.PaidMaterials.Count || !records.Select(m => m.CardId).SequenceEqual(r.PaidMaterials.Select(m => m.CardId)) || records.Any(m => m.From != CardLocation.Hand(target) || m.To != CardLocation.DiscardPile || m.Reason.Value != CoreGameEngineCompletedUndamagedTargetRevealRuntime.CompletedUndamagedDiscardReason(f) || m.CardKind != r.PaidMaterials.Single(c => c.CardId == m.CardId).PrintedKind))
            return false;
        return r.Stage == CompletedUndamagedTargetRevealStage.Complete ? f.PendingMovementContinuation is null && completions is [var complete] && complete.SelectedCount == r.SelectedSlots.Count && complete.ActualDiscardCount == records.Length : r.Stage == CompletedUndamagedTargetRevealStage.MovementChildren && completions.Length == 0 && f.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } pending && pending.SubjectSeat == target;
    }

    internal bool CompletedUndamagedTargetRevealFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidCompletedUndamagedTargetRevealReceipt(f) || f.CompletedUndamagedTargetReveal is not { Stage: CompletedUndamagedTargetRevealStage.MovementChildren, MovementIssued: true, TargetSeat: { } target, BatchId: { } batch } r || child is not CardsMovedTriggerWindowFrame moved)
            return false;
        return moved.ResumeProgramFrameId is null && moved.Id == batch && moved.Batch.Id == batch && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count == r.PaidMaterials.Count && moved.Batch.Movements.Select(m => m.CardId).SequenceEqual(r.PaidMaterials.Select(m => m.CardId)) && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.From == CardLocation.Hand(target) && m.To == CardLocation.DiscardPile && m.Reason.Value == CoreGameEngineCompletedUndamagedTargetRevealRuntime.CompletedUndamagedDiscardReason(f) && m.CardKind == r.PaidMaterials.Single(c => c.CardId == m.CardId).PrintedKind);
    }

    internal bool CompletedUndamagedTargetRevealStructuralEdge(ResolutionFrame parent, ResolutionFrame child)
    {
        if (parent is ProgramSkillFrame root && CompletedUndamagedTargetRevealFirstChild(root, child))
            return true;
        return parent is ProgramCardTriggerWindowFrame window && child is ProgramSkillFrame { CompletedUndamagedTargetReveal: { } r } observer && r.WindowFrameId == window.Id && ValidCompletedUndamagedTargetRevealReceipt(observer);
    }

    internal ProgramSkillFrame? CompletedUndamagedTargetRevealObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.CompletedUndamagedTargetRevealFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => this.CompletedUndamagedTargetRevealObserverEdge(child), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(child, dying) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(child, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(child, dying))));
    internal bool CompletedUndamagedTargetRevealObserverEdge(int child)
    {
        var parent = _resolutionStackOperations._resolutionStack[child - 1];
        var current = _resolutionStackOperations._resolutionStack[child];
        return CoreGameEngineObserverSubtreesRuntime.IsObserverSkillsChangedEdge(parent, current) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(parent, current) || _coreOriginalHandEntityObserversOperations.OriginalHandEntityStructuralEdge(parent, current) || _coreOutsidePhaseDrawDiscardObserversOperations.OutsidePhaseDrawDiscardStructuralEdge(parent, current) || CompletedUndamagedTargetRevealStructuralEdge(parent, current) || _coreRecipientCategoryMarkObserversOperations.RecipientCategoryMarkStructuralEdge(parent, current) || _coreOffTurnUsedCardObserversOperations.OffTurnUsedCardGiftStructuralEdge(parent, current) || _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandStructuralEdge(parent, current) || _coreResponseCompletionOperations.ResponseCompletionStructuralEdge(parent, current) || _coreCardSupplyCompletionOperations.CardSupplyCompletionStructuralEdge(parent, current) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child);
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 60)]
    internal bool IsCompletedUndamagedTargetRevealDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && CompletedUndamagedTargetRevealObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 20)]
    internal bool TryAdvanceCompletedUndamagedTargetRevealSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || CompletedUndamagedTargetRevealObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}
