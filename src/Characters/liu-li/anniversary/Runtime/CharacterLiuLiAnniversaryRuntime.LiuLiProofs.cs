#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuLiAnniversaryRuntime
{
    internal bool LiuLiOriginal(ProgramSkillFrame f)
    {
        if (f.SkillId == AnniversaryLiuLiComposition.Fuli)
            return f.TriggerId is null && f.ActivationId == "reveal-discard-draw" && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) == 1;
        return f.SkillId == AnniversaryLiuLiComposition.Dehua && f.ActivationId == f.TriggerId && f.TriggerId == "round-damage-name" && f.WindowContext is { Window: SkillProgramTriggerWindow.RoundStarting } c && c.OwnerSeat == f.OwnerSeat && _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == c.ParentFrameId)is ProgramLifecycleTriggerWindowFrame w && w.Window == c.Window && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.SkillInstanceId == f.SkillInstanceId && e.BindingId == f.TriggerId && e.Window == c.Window) == 1;
    }

    internal bool LiuLiDiscardProof(ProgramSkillFrame f)
    {
        var r = f.AnniversaryLiuLi!;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryLiuLiDiscardPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid.Category != r.Category || paid.Count != r.Cards.Count || paid.NameSum != r.NameSum || paid.DamageDiscarded != r.DamageDiscarded || paid.Before != paid.After || paid.Before < 0 || paid.Before > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Cards.Count == 0)
            return false;
        var materials = r.Cards.Select(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > paid.Before && m.CardId == id && m.From == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == LiuLiDiscardReason && (m.To == CardLocation.DiscardPile || _coreProgramAtomicCardMovesOperations.IsRedirectedPaidDiscardMovement(m, CardLocation.Hand(f.OwnerSeat), new CardMoveReason(LiuLiDiscardReason)))).ToArray()).ToArray();
        // The payment is frozen before its first move fact. No child may use
        // the not-yet-moved state as a completed payment invoice.
        if (r.Stage == AnniversaryLiuLiStage.CostChildren && r.After == r.Before && materials.All(rows => rows.Length == 0))
            return r.Cards.All(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.OwnerSeat));
        return materials.All(rows => rows.Length == 1) && materials.Sum(rows => LiuLiNameLength(_coreAdvancedLifecycleOperations.GetAdvancedCard(rows[0].CardId))) == r.NameSum && materials.Any(rows => CoreGameEngineMouLuMengRuntime.IsDamageCardKind(rows[0].CardKind)) == r.DamageDiscarded && materials.All(rows => (int)CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(rows[0].CardKind) == r.Category) && (r.Stage != AnniversaryLiuLiStage.CostChildren || materials.All(rows => rows[0].Sequence <= r.After));
    }

    internal bool ValidLiuLi(ProgramSkillFrame f)
    {
        if (f.AnniversaryLiuLi is not { } r || r.Source != LiuLiSource(f) || r.Hash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || r.ParentId != (f.WindowContext?.ParentFrameId ?? 0) || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.Round != _coreActualRoundProgressRoundNumberAccess._roundNumber || !LiuLiOriginal(f) || !Enum.IsDefined(r.Stage) || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Cards.Distinct().Count() != r.Cards.Count || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryLiuLiStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start != new AnniversaryLiuLiStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, r.Round))
            return false;
        if (f.SkillId == AnniversaryLiuLiComposition.Dehua)
        {
            if (r.Cards.Count != 0 || r.Draw is not null || r.Stage is not (AnniversaryLiuLiStage.UseChoice or AnniversaryLiuLiStage.IssuingUse or AnniversaryLiuLiStage.NativeUse or AnniversaryLiuLiStage.ReturnedUse or AnniversaryLiuLiStage.SkillChildren or AnniversaryLiuLiStage.Completed))
                return false;
            if (r.Stage == AnniversaryLiuLiStage.UseChoice)
                return r.Plan is null && r.UseId == 0 && r.ActionId == 0;
            if (r.Stage == AnniversaryLiuLiStage.Completed && r.Plan is null)
                return r.UseId == 0;
            if (r.Plan is not { } p || !CoreGameEngineMouLuMengRuntime.IsDamageCardKind(p.Kind) || p.Targets.Any(t => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(t)) || p.Targets.Distinct().Count() != p.Targets.Count)
                return false;
            if (r.Stage == AnniversaryLiuLiStage.IssuingUse)
                return r.UseId == 0 && r.ActionId == 0;
            if (r.UseId <= f.Id || r.ActionId <= 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryLiuLiNativeIssuedEvent>().Count(e => e == new AnniversaryLiuLiNativeIssuedEvent(f.Id, r.UseId, r.ActionId, p.Kind)) != 1)
                return false;
            return r.Stage == AnniversaryLiuLiStage.NativeUse || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == r.UseId && e.CardId == 0 && e.CardKind == p.Kind) == 1;
        }

        if (r.Stage is AnniversaryLiuLiStage.Revealing or AnniversaryLiuLiStage.Category)
            return r.Cards.Count == 0 && r.Category == -1 && r.Draw is null && r.Plan is null;
        if (r.Category is < 0 or > 2 || r.NameSum < 1 || !LiuLiDiscardProof(f) || r.Plan is not null)
            return false;
        if (r.Stage == AnniversaryLiuLiStage.CostChildren)
            return r.Draw is null;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryLiuLiDrawIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var issued] || issued.NameSum != r.NameSum || issued.Count != r.DrawCount || issued.Count != Math.Min(issued.NameSum, issued.MaximumHandCount) || issued.MaximumHandCount < 0)
            return false;
        if (r.Stage == AnniversaryLiuLiStage.Drawing)
            return r.DrawCount > 0 && r.Draw is null;
        if (r.Stage is not (AnniversaryLiuLiStage.DrawChildren or AnniversaryLiuLiStage.RangeTarget or AnniversaryLiuLiStage.Completed))
            return false;
        return r.DrawCount == 0 ? r.Draw is null : r.Draw is { } draw && draw.RecipientSeat == f.OwnerSeat && draw.RequestedCount == r.DrawCount && draw.SequenceBefore == r.Before && draw.SequenceAfter == r.After && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryLiuLiDrawReturnedEvent>().Count(e => e == new AnniversaryLiuLiDrawReturnedEvent(f.Id, r.DrawCount, draw.ActualDrawIds.Count, r.Before, r.After)) == 1;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 43300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 43300)]
    internal bool KeepPaidLiuLi(ProgramSkillFrame f) => f.AnniversaryLiuLi is { Stage: AnniversaryLiuLiStage.CostChildren or AnniversaryLiuLiStage.Drawing or AnniversaryLiuLiStage.DrawChildren or AnniversaryLiuLiStage.RangeTarget or AnniversaryLiuLiStage.NativeUse or AnniversaryLiuLiStage.ReturnedUse or AnniversaryLiuLiStage.SkillChildren } && ValidLiuLi(f);
    internal bool LiuLiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryLiuLi is not { } r || !ValidLiuLi(f))
            return false;
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryLiuLiStage.Drawing && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == f.OwnerSeat && draw.Context.RequestedCount == r.DrawCount;
        if (child is CardsMovedTriggerWindowFrame m)
            return r.Stage is AnniversaryLiuLiStage.CostChildren or AnniversaryLiuLiStage.DrawChildren && m.ResumeProgramFrameId == f.Id && m.Batch.ParentFrameId == f.Id && m.Batch.OriginOwnerSeat == f.OwnerSeat && m.Batch.OriginSkillId == f.SkillId && m.Batch.OriginSkillInstanceId == f.SkillInstanceId && m.Batch.Movements.Count > 0 && m.Batch.Movements.All(move => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) && move.Sequence > r.Before && move.Sequence <= r.After && (move.Reason == CardMoveReasons.Reshuffle || r.Stage == AnniversaryLiuLiStage.CostChildren && move.Reason.Value == LiuLiDiscardReason || r.Stage == AnniversaryLiuLiStage.DrawChildren && move.Reason.Value == LiuLiDrawReason));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && r.Stage == AnniversaryLiuLiStage.SkillChildren && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? LiuLiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.LiuLiFirstChild(f, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 43300)]
    internal bool AdvanceLiuLiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiuLiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
