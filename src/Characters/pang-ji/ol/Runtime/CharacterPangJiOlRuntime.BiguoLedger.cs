#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangJiOlRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedBeforeActivity, 10)]
    internal bool BiguoFinalizedBeforeActivityTargetWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveBiguoTargetedPindianAndCategoryBan);
    internal bool ValidBiguoArm(BiguoArmedEvent arm)
    {
        if (arm.Source.SkillId != "ol:biguo" || arm.Source.BindingId != "draw-end-category-gain" || _contentRegistryOperations._contentRegistry.GetSkill(arm.Source.SkillId).Program?.GameplayHash != arm.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<PangJiStartedEvent>().Count(e => e.FrameId == arm.FrameId && e.Source == arm.Source && e.GameplayHash == arm.GameplayHash && e.Op == SkillProgramEffectOp.ResolveBiguoDrawAndArmTargetPindian && e.ActualTurn == arm.ActualTurn && e.TurnOwner == arm.TurnOwner) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoArmedEvent>().Count(e => e.FrameId == arm.FrameId) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoCostCommittedEvent>().Where(e => e.FrameId == arm.FrameId).ToArray()is not [var cost] || _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoCostPaidEvent>().Where(e => e.FrameId == arm.FrameId).ToArray()is not [var paid] || paid.CardId != cost.CardId || paid.PrintedKind != cost.PrintedKind || paid.From != cost.From || paid.Sequence <= cost.Before || cost.From.OwnerSeat != arm.Source.OwnerSeat || cost.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == paid.Sequence && m.CardId == paid.CardId && m.CardKind == paid.PrintedKind && m.From == paid.From && m.To == paid.To && m.Reason == paid.Reason))
            return false;
        var issued = _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoObtainEntityIssuedEvent>().Where(e => e.FrameId == arm.FrameId).OrderBy(e => e.Index).ToArray();
        return issued.Select(e => CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(e.PrintedKind)).Distinct().Count() == issued.Length && issued.Where((e, i) => e.Index != i).Any() == false && issued.All(e => _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoObtainEntityPaidEvent>().Where(p => p.FrameId == e.FrameId && p.Index == e.Index).ToArray()is [var p] && p.CardId == e.CardId && p.PrintedKind == e.PrintedKind && p.From == CardLocation.DrawPile && p.To == CardLocation.Hand(arm.Source.OwnerSeat) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == p.Sequence && m.CardId == p.CardId && m.CardKind == p.PrintedKind && m.From == p.From && m.To == p.To && m.Reason.Value == BiguoObtainReason));
    }

    internal IReadOnlyList<int> BiguoDesignatedTargets(CardActionContext action) => action.EffectiveKind == CardKind.BorrowedSword ? action.TargetSeats.Where((_, i) => i % 2 == 0).ToArray() : _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(action.EffectiveKind, action.ActorSeat, action.EffectiveDesignatedTargetSeats);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 21)]
    internal void CollectIssuedBiguoCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> candidates)
    {
        if (window != SkillProgramTriggerWindow.CardUseTargetsFinalized)
            return;
        candidates.RemoveAll(c => c.SkillId == "ol:biguo" && c.TriggerId == "armed-target-pindian");
        if (action.Type != CardActionType.Use || !BiguoTurnOpen(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == action.ActionId)is not { } use)
            return;
        var targets = BiguoDesignatedTargets(action);
        foreach (var arm in _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoArmedEvent>().Where(a => a.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && a.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && ValidBiguoArm(a)).OrderBy(a => a.FrameId).DistinctBy(a => a.Source.OwnerSeat))
        {
            var owner = arm.Source.OwnerSeat;
            if (!targets.Contains(owner) || !CanBiguoPindian(owner) || _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoTargetResolvedEvent>().Any(e => e.UseId == use.Id && e.OwnerSeat == owner) || _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoTargetAcceptedEvent>().Any(e => e.Origin.UseId == use.Id && e.Origin.Source.OwnerSeat == owner))
                continue;
            var t = _contentRegistryOperations._contentRegistry.GetSkill(arm.Source.SkillId).Program!.Triggers.Single(t => t.Id == "armed-target-pindian");
            var origin = new BiguoTargetOrigin(arm.FrameId, arm.Source, arm.GameplayHash, use.Id, action.ActionId, arm.ActualTurn, arm.TurnOwner);
            var c = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, owner, owner, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[owner], action))with
            {
                BiguoTarget = origin
            };
            candidates.Add(new(owner, owner, arm.Source.SkillId, t.Id, arm.GameplayHash, arm.Source.SkillInstanceId, t.Priority, c));
        }
    }

    internal ProgramSkillWindowContext CreateBiguoTargetContext(ProgramCardTriggerWindowFrame window, ProgramCardTriggerCandidate candidate, ProgramSkillWindowContext ordinary)
    {
        if (candidate.SkillId != "ol:biguo" || candidate.TriggerId != "armed-target-pindian")
            return ordinary;
        if (candidate.FrozenContext?.BiguoTarget is not { } origin || origin.UseId != window.ParentFrameId || origin.ActionId != window.Action.ActionId)
            throw new InvalidOperationException("Biguo lost his frozen target-use entitlement.");
        return ordinary with
        {
            BiguoTarget = origin,
            SourceSeat = window.Action.ActorSeat,
            TargetSeat = candidate.OwnerSeat
        };
    }

    internal bool BiguoTargetRoot(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c, bool requireUnpaid)
    {
        if (candidate.SkillId != "ol:biguo" || candidate.BindingId != "armed-target-pindian" || c.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || c.OwnerSeat != candidate.OwnerSeat || c.TargetSeat != candidate.OwnerSeat || c.BiguoTarget is not { } origin || origin.Source.OwnerSeat != candidate.OwnerSeat || origin.Source.SkillId != candidate.SkillId || origin.Source.SkillInstanceId != candidate.SkillInstanceId || origin.GameplayHash != candidate.GameplayHash || c.CardUse?.CardActionId != origin.ActionId || c.CardUse.ParentCardUseFrameId != origin.UseId || _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoArmedEvent>().SingleOrDefault(a => a.FrameId == origin.ArmFrameId)is not { } arm || arm.Source != origin.Source || arm.GameplayHash != origin.GameplayHash || arm.ActualTurn != origin.ActualTurn || arm.TurnOwner != origin.TurnOwner || !ValidBiguoArm(arm) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.ParentFrameId != origin.UseId || window.Action.ActionId != origin.ActionId || CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != c.Window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex]) != candidate || window.Candidates[window.CandidateIndex].FrozenContext?.BiguoTarget != origin || _coreCardUseLifecycleOperations.LifecycleCardUse(origin.UseId)is not { Action.Type: CardActionType.Use } use || use.Action!.ActionId != origin.ActionId || !BiguoDesignatedTargets(window.Action).Contains(candidate.OwnerSeat))
            return false;
        return !requireUnpaid || BiguoTurnOpen(origin.ActualTurn, origin.TurnOwner) && CanBiguoPindian(candidate.OwnerSeat) && !_coreProgramEventHistoryOperations.ProgramEventHistory<BiguoTargetAcceptedEvent>().Any(e => e.Origin.UseId == origin.UseId && e.Origin.Source.OwnerSeat == candidate.OwnerSeat);
    }

    [ProgramIssuedTrigger(260)]
    internal bool IsIssuedBiguoCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => BiguoTargetRoot(c, context, true);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 28130)]
    internal void CaptureBiguoNativeFacts(IGameEvent payload)
    {
        if (payload is ProgramCardTriggerResolvedEvent { SkillId: "ol:biguo", TriggerId: "armed-target-pindian" } resolved && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramCardTriggerWindowFrame w && w.Id == resolved.FrameId && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && w.Candidates[w.CandidateIndex] is { FrozenContext.BiguoTarget: { } o } candidate && candidate.OwnerSeat == resolved.OwnerSeat)
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<BiguoTargetResolvedEvent>().Any(e => e.UseId == o.UseId && e.OwnerSeat == resolved.OwnerSeat))
                throw new InvalidOperationException("Biguo cannot finish the same target occurrence twice.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiguoTargetResolvedEvent(o.UseId, o.ActionId, resolved.OwnerSeat, resolved.Activated));
            return;
        }

        if (payload is not PindianResultDeterminedEvent e || _resolutionStackOperations._resolutionStack.LastOrDefault()is not PindianFrame p || p.Id != e.FrameId || p.SkillId != "ol:biguo" || !IsBiguoPindianFrame(p) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == p.ParentFrameId)is not ProgramSkillFrame { PangJi: { } r } f)
            return;
        var result = e.Result;
        if (result.SourceSeat != p.SourceSeat || result.OpponentSeat != p.OpponentSeat || (result.SourceCardId != p.SourceCardId && !IsDouyuPairAssignment(p, result) && !_characterLuKangWumiaoOperations.IsWuLuKangAssignedPair(p, result)) || result.SourceCardId == result.OpponentCardId || _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoPindianCardCategoryEvent>().Any(x => x.PindianId == p.Id))
            throw new InvalidOperationException("Biguo cannot replace or classify a native Pindian pair twice.");
        var cards = new[]
        {
            (result.SourceSeat, result.SourceCardId),
            (result.OpponentSeat, result.OpponentCardId)
        };
        var categories = new List<SkillProgramCardCategory>();
        foreach (var(seat, id)in cards)
        {
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
            var category = CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind);
            categories.Add(category);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiguoPindianCardCategoryEvent(f.Id, p.Id, seat, id, card.Kind, category));
        }

        foreach (var seat in new[]
        {
            result.SourceSeat,
            result.OpponentSeat
        }.Where(seat => !result.WonBy(seat)))
            foreach (var category in categories.Distinct())
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiguoCategoryBanEvent(f.Id, p.Id, seat, category, r.ActualTurn, r.TurnOwner));
    }

    internal bool IsBiguoCategoryForbidden(int seat, CardKind effectiveKind)
    {
        if (!BiguoTurnOpen(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat))
            return false;
        var category = CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(effectiveKind);
        return _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoCategoryBanEvent>().Any(e => e.Seat == seat && e.Category == category && e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && ValidBiguoBan(e));
    }

    internal bool ValidBiguoBan(BiguoCategoryBanEvent ban)
    {
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<PangJiStartedEvent>().SingleOrDefault(e => e.FrameId == ban.FrameId)is not { Op: SkillProgramEffectOp.ResolveBiguoTargetedPindianAndCategoryBan } start || start.ActualTurn != ban.ActualTurn || start.TurnOwner != ban.TurnOwner || _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoPindianIssuedEvent>().Count(e => e.FrameId == ban.FrameId && e.PindianId == ban.PindianId && e.OwnerSeat == start.Source.OwnerSeat) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().Where(e => e.FrameId == ban.PindianId && e.SkillId == start.Source.SkillId).ToArray()is not [var determined] || determined.Result.WonBy(ban.Seat) || ban.Seat != determined.Result.SourceSeat && ban.Seat != determined.Result.OpponentSeat)
            return false;
        var result = determined.Result;
        var cards = _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoPindianCardCategoryEvent>().Where(e => e.FrameId == ban.FrameId && e.PindianId == ban.PindianId).ToArray();
        return cards.Length == 2 && cards.Any(e => e.Seat == result.SourceSeat && e.CardId == result.SourceCardId) && cards.Any(e => e.Seat == result.OpponentSeat && e.CardId == result.OpponentCardId) && cards.All(e => _coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).Kind == e.PrintedKind && CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(e.PrintedKind) == e.Category) && cards.Any(e => e.Category == ban.Category) && _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoCategoryBanEvent>().Count(e => e == ban) == 1;
    }

    internal bool IsBiguoPindianFrame(PindianFrame child) => _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == child.ParentFrameId)is ProgramSkillFrame { PangJi: { Paid: true, Stage: PangJiStage.Pindian } r } f && r.Op == SkillProgramEffectOp.ResolveBiguoTargetedPindianAndCategoryBan && r.PindianId == child.Id && child.SkillId == f.SkillId && child.SourceSeat == f.OwnerSeat && child.ProgramResultBind == BiguoResultBind && child.ProgramResultVisibility == SkillProgramCardSetVisibility.Public && _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoPindianIssuedEvent>().Count(e => e.FrameId == f.Id && e.PindianId == child.Id && e.ArmFrameId == r.Target!.ArmFrameId && e.UseId == r.Target.UseId && e.ActionId == r.Target.ActionId && e.OwnerSeat == f.OwnerSeat && e.Before == r.Before) == 1;

    internal bool IsAcceptedBiguoUse(int actorSeat, CardKind kind, CardActionType actionType, long? useId) => actionType == CardActionType.Use && useId is { } id && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { Action: { Type: CardActionType.Use } action } use && use.SourceSeat == actorSeat && use.CardKind == kind && action.ActorSeat == actorSeat && action.EffectiveKind == kind && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Count(e => e.ResolutionId == id && e.SourceSeat == actorSeat && e.CardKind == kind && e.CardId == use.CardId) == 1;
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeIntrinsicUseBan, 10)]
    internal bool BiguoCardUseForbidden(ProgramCardUseQuery query) => IsBiguoCategoryForbidden(query.ActorSeat, query.Kind) && !IsAcceptedBiguoUse(query.ActorSeat, query.Kind, query.ActionType, query.ExistingUseFrameId);
}
