#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangJinzhiRuntime
{
    internal const string WushuangJinzhiSkill = "wushuang:skill-3252";
    internal const string WushuangJinzhiDiscardReason = "skill-program.wushuang.jinzhi.discard";
    internal bool IsWushuangJinzhiSource(CardConversionSource source) => source.SkillId == WushuangJinzhiSkill && _corePlayCardHistoryOperations.ViewAsRule(source)is not null;
    internal bool IsWushuangJinzhiSelection(ProgramMultiCardViewAsSelection selection) => IsWushuangJinzhiSource(selection.Source);
    internal CardActionContext CaptureWushuangJinzhiAppearance(CardActionContext action) => !action.ConversionChain.Any(IsWushuangJinzhiSource) ? action : new(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, Suit.None, null, null, action.FactionOrigin);
    internal int WushuangJinzhiNextCount(int owner) => checked(_coreProgramEventHistoryOperations.ProgramEventHistory<WushuangJinzhiPaidEvent>().Count(e => e.Owner == owner && e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber) + 1);
    internal WushuangJinzhiMaterialSelection? HumanWushuangJinzhiMaterials()
    {
        if (_coreMatchDefinitionOptionsAccess._options.HumanSeat < 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.PlayCard } prompt || prompt.PlayerSeat != _coreMatchDefinitionOptionsAccess._options.HumanSeat || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return null;
        var owner = _coreMatchParticipantsPlayersAccess._players[_coreMatchDefinitionOptionsAccess._options.HumanSeat];
        if (!owner.IsAlive || !_coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Any(p => p.SkillId == WushuangJinzhiSkill))
            return null;
        return new(WushuangJinzhiNextCount(owner.Seat), Array.AsReadOnly(WushuangJinzhiDiscardableCards(owner).Select(card => card.Id).ToArray()));
    }

    internal IEnumerable<Card> WushuangJinzhiDiscardableCards(CharacterState owner) => _coreGuanduGeneralsOperations.GuanduOwnedCards(owner.Seat).Where(c => !c.IsGeneralWeapon && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner.Seat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard));
    internal IReadOnlyList<ProgramMultiCardViewAsSelection> WushuangJinzhiSelections(CharacterState owner, CardKind kind, bool response)
    {
        var instances = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(p => p.SkillId == WushuangJinzhiSkill).ToArray();
        if (instances.Length == 0 || _coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, kind, response ? CardActionType.Response : CardActionType.Use))
            return[];
        var count = WushuangJinzhiNextCount(owner.Seat);
        // These entities pay a discard. They are not the virtual basic card's
        // hand materials, so use/response-only hand bans do not forbid payment.
        var cards = WushuangJinzhiDiscardableCards(owner).GroupBy(c => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, c))).ToArray();
        var selections = new List<ProgramMultiCardViewAsSelection>();
        foreach (var instance in instances)
            foreach (var rule in instance.Program.ViewAs.Where(v => v.OutputKind == kind && (response ? v.ForResponse : v.ForPlay)))
                foreach (var group in cards.Where(g => g.Count() >= count))
                    foreach (var selected in global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.EnumerateCardCombinations(group.OrderBy(c => c.Id).ToArray(), count))
                        selections.Add(new(selected, new(instance.SkillId, rule.Id, owner.Seat, instance.SkillInstanceId), kind));
        return selections.Where(s => _coreProgramCardLegalityOperations.IsProgramViewAsAllowed(owner, s.OutputKind, s.Cards.Select(c => c.Id).ToArray(), s.Source, response)).ToArray();
    }

    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 61010)]
    internal bool WushuangJinzhiConversionForbidden(ProgramViewAsLegalityQuery query)
    {
        if (!IsWushuangJinzhiSource(query.Source))
            return false;
        var cards = query.CardIds.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray();
        return cards.Length != WushuangJinzhiNextCount(query.Owner.Seat) || cards.Select(c => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(query.Owner, c))).Distinct().Count() != 1 || cards.Any(c => c.IsGeneralWeapon || _cardZonesOperations._cardZones.GetLocation(c.Id).OwnerSeat != query.Owner.Seat || _cardZonesOperations._cardZones.GetLocation(c.Id).Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(query.Owner.Seat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard));
    }

    internal void ContinueWushuangJinzhi(ProgramSkillFrame f)
    {
        var r = f.WushuangOriginalSkill!;
        if (r.Stage == "jinzhi-draw")
        {
            _coreWushuangOriginalSkillsOperations.DrawWushuangOriginal(f, f.OwnerSeat, 1, "finish");
            return;
        }

        if (r.Stage != "begin" || f.WindowContext is not { CardUse: { } c } context || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.Action is not { } action || action.ActionId != c.CardActionId || action.ProviderSeat != f.OwnerSeat || action.ConversionChain.Count(IsWushuangJinzhiSource) != 1 || action.PhysicalCards.Count != WushuangJinzhiNextCount(f.OwnerSeat) || action.PhysicalCards.Any(cost => cost.From.OwnerSeat != f.OwnerSeat || cost.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) || action.PhysicalCards.Select(cost => cost.EffectiveIsRed).Distinct().Count() != 1 || action.PhysicalCards.Any(cost => _cardZonesOperations._cardZones.GetLocation(cost.CardId) != CardLocation.Processing && WushuangJinzhiDestroyedPayment(action, cost, c.ParentCardUseFrameId, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)is null))
            throw new InvalidOperationException("Jinzhi requires its exact accepted once-paid same-color material action.");
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<WushuangJinzhiPaidEvent>().Any(e => e.Action.ActionId == action.ActionId))
            throw new InvalidOperationException("One Jinzhi action cannot pay its dynamic ordinal twice.");
        var ordinal = WushuangJinzhiNextCount(f.OwnerSeat);
        var prepaid = action.PhysicalCards.Select(cost => WushuangJinzhiDestroyedPayment(action, cost, c.ParentCardUseFrameId, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)).OfType<CardMovementRecord>().ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangJinzhiPaidEvent(f.Id, f.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber, ordinal, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, action) { MaterialPaymentFrameId = prepaid.Length > 0 ? c.ParentCardUseFrameId : null });
        var reason = new CardMoveReason(WushuangJinzhiDiscardReason);
        var invoice = _coreNativeLogicalDiscardsOperations.TracksNativeLogicalDiscards ? new NativeLogicalDiscardInvocationDraft
        {
            InvocationId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence,
            TurnNumber = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
            ActualTurnOwnerSeat = _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat,
            InitiatorSeat = f.OwnerSeat,
            ExecutorSeat = f.OwnerSeat,
            ParentFrameId = f.Id,
            Reason = reason,
            SequenceBefore = prepaid.Length == 0 ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence : prepaid.Min(m => m.Sequence) - 1,
            Materials = action.PhysicalCards.Select(cost => new NativeLogicalDiscardMaterial(cost.CardId, cost.From)).ToArray()
        }

        : null;
        f = _coreWushuangOriginalSkillsOperations.WushuangOriginalStage(f, "jinzhi-draw");
        // Shop equipment has already paid by its irreversible native departure.
        // Credit that exact movement; never move the destroyed entity again.
        var processing = action.PhysicalCards.Where(cost => _cardZonesOperations._cardZones.GetLocation(cost.CardId) == CardLocation.Processing).Select(cost => cost.CardId).ToArray();
        if (processing.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(processing, CardLocation.DiscardPile, reason, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(invoice, prepaid);
        _coreWushuangOriginalSkillsOperations.RunWushuangOriginalSkill(f.Id);
    }

    internal CardMovementRecord? WushuangJinzhiDestroyedPayment(CardActionContext action, CardActionCost cost, long paymentFrameId, int movementThrough)
    {
        if (cost.From != CardLocation.Equipment(action.ProviderSeat) || action.ConversionChain.Count(IsWushuangJinzhiSource) != 1 || _cardZonesOperations._cardZones.GetLocation(cost.CardId) != CardLocation.OutsideGame || !HasWushuangJinzhiPaymentAction(action, paymentFrameId) || _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Where(e => e.CardId == cost.CardId).ToArray()is not [var destroyed] || destroyed.ParentFrameId != paymentFrameId || destroyed.MovementSequence > movementThrough)
            return null;
        var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cost.CardId && m.Sequence == destroyed.MovementSequence);
        return movement is not null && movement.CardKind == cost.CardKind && movement.From == cost.From && movement.Reason == (action.Type == CardActionType.Use ? CardMoveReasons.Use : CardMoveReasons.Respond) && _coreWushuangShopCardsOperations.IsWushuangShopDestroyedMaterial(movement) ? movement : null;
    }

    internal bool HasWushuangJinzhiPaymentAction(CardActionContext action, long paymentFrameId)
    {
        bool Matches(CardActionContext issued) => issued.ActionId == action.ActionId && issued.Type == action.Type && issued.ActorSeat == action.ActorSeat && issued.ProviderSeat == action.ProviderSeat && issued.RequesterSeat == action.RequesterSeat && issued.ResponderSeat == action.ResponderSeat && issued.OpponentSeat == action.OpponentSeat && issued.EffectiveKind == action.EffectiveKind && issued.PhysicalCards.SequenceEqual(action.PhysicalCards) && issued.ConversionChain.SequenceEqual(action.ConversionChain);
        var accepted = _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Where(e => e.Action.ActionId == action.ActionId).ToArray();
        if (accepted.Length > 0)
            return accepted.All(e => Matches(e.Action));
        // Simple-card commitment runs before its optional finalized-target
        // acceptance event. Its issued native owner is already authoritative.
        if (action.Type == CardActionType.Use && _coreCardUseLifecycleOperations.LifecycleCardUse(paymentFrameId)is { ProgramUseCommitted: true, Action: { } committed } use && use.SourceSeat == action.ActorSeat && use.CardKind == action.EffectiveKind && Matches(committed))
            return true;
        // The paid receipt retains that checked owner after its native frame
        // retires, including catalogs with no finalized-target subscribers.
        return _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangJinzhiPaidEvent>().Where(e => e.Action.ActionId == action.ActionId).ToArray()is [var paid] && paid.MaterialPaymentFrameId == paymentFrameId && Matches(paid.Action);
    }

    internal CardLocation? WushuangJinzhiDiscardSource(CardMovementRecord movement)
    {
        if (movement.To == CardLocation.OutsideGame)
        {
            var prepaid = _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangJinzhiPaidEvent>().LastOrDefault(e => e.MaterialPaymentFrameId is { } parent && e.Action.PhysicalCards.Any(cost => cost.CardId == movement.CardId && WushuangJinzhiDestroyedPayment(e.Action, cost, parent, e.MovementBefore) == movement));
            return prepaid?.Action.PhysicalCards.Single(cost => cost.CardId == movement.CardId).From;
        }

        if (movement.From != CardLocation.Processing || movement.To != CardLocation.DiscardPile || movement.Reason.Value != WushuangJinzhiDiscardReason)
            return null;
        var previous = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == movement.CardId && m.Sequence < movement.Sequence);
        if (previous is null || previous.To != CardLocation.Processing)
            return null;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangJinzhiPaidEvent>().LastOrDefault(e => e.MovementBefore >= previous.Sequence && e.MovementBefore < movement.Sequence && e.Action.PhysicalCards.Any(cost => cost.CardId == movement.CardId) && e.Action.PhysicalCards.Any(cost => cost.CardId == movement.CardId && cost.From == previous.From));
        return paid?.Action.PhysicalCards.Single(cost => cost.CardId == movement.CardId).From;
    }

    internal bool IsWushuangJinzhiPaidMaterial(CardActionContext? action, int card) => action is not null && _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangJinzhiPaidEvent>().Any(e => e.Action.ActionId == action.ActionId && e.Action.PhysicalCards.Any(cost => cost.CardId == card));
    internal bool TryGetWushuangJinzhiResponseMovements(CardActionContext action, int card, out IReadOnlyList<CardMovementRecord> movements)
    {
        movements = [];
        if (!IsWushuangJinzhiPaidMaterial(action, card))
            return false;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangJinzhiPaidEvent>().Single(e => e.Action.ActionId == action.ActionId);
        var cost = paid.Action.PhysicalCards.Single(c => c.CardId == card);
        if (paid.MaterialPaymentFrameId is { } parent && WushuangJinzhiDestroyedPayment(paid.Action, cost, parent, paid.MovementBefore)is { } destroyed)
        {
            movements = Array.AsReadOnly(new[] { destroyed });
            return true;
        }

        var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == card && m.Sequence <= paid.MovementBefore && m.From == cost.From && m.To == CardLocation.Processing);
        var finish = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.FirstOrDefault(m => m.CardId == card && m.Sequence > paid.MovementBefore && m.Reason.Value == WushuangJinzhiDiscardReason && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile);
        movements = Array.AsReadOnly(new[] { entry, finish });
        return true;
    }

    internal bool TryResolveWushuangJinzhiDyingAlcohol(CharacterState owner, ProgramMultiCardViewAsSelection selection)
    {
        if (selection.OutputKind != CardKind.Alcohol || !IsWushuangJinzhiSelection(selection))
            return false;
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || dying.VictimSeat != owner.Seat || dying.ResponderSeat != owner.Seat)
            throw new InvalidOperationException("Jinzhi Alcohol requires the actual owner's self-rescue.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(owner, owner, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(owner, selection.Cards[0], selection.Source), "酒", CardKind.Alcohol, conversionSource: selection.Source, dyingResponse: new(dying.FrameId, owner.Seat, false, null, true, selection.Cards[0].Id), physicalCards: selection.Cards);
        return true;
    }
}
