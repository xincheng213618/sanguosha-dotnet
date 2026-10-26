namespace CardGame.Core;

public sealed record WushuangJinzhiPaidEvent(long FrameId, int Owner, int RoundNumber, int Ordinal,
    int ActualTurn, int MovementBefore, CardActionContext Action) : IGameEvent
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public long? MaterialPaymentFrameId { get; init; }
}

public sealed record WushuangJinzhiMaterialSelection(int RequiredCount, IReadOnlyList<int> CardIds);

public sealed partial class GameEngine
{
    private const string WushuangJinzhiSkill = "wushuang:skill-3252";
    private const string WushuangJinzhiDiscardReason = "skill-program.wushuang.jinzhi.discard";
    private bool IsWushuangJinzhiSource(CardConversionSource source) => source.SkillId == WushuangJinzhiSkill &&
        ViewAsRule(source) is not null;
    private bool IsWushuangJinzhiSelection(ProgramMultiCardViewAsSelection selection) => IsWushuangJinzhiSource(selection.Source);
    private CardActionContext CaptureWushuangJinzhiAppearance(CardActionContext action) =>
        !action.ConversionChain.Any(IsWushuangJinzhiSource) ? action :
        new(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat,
            action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind,
            action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats,
            Suit.None, null, null, action.FactionOrigin);
    private int WushuangJinzhiNextCount(int owner) => checked(ProgramEventHistory<WushuangJinzhiPaidEvent>()
        .Count(e => e.Owner == owner && e.RoundNumber == _roundNumber) + 1);

    // An unpaid UI draft remains available even when no complete same-color set
    // exists. Only the ordinary legal-action query can authorize its final use.
    private WushuangJinzhiMaterialSelection? HumanWushuangJinzhiMaterials()
    {
        if (_options.HumanSeat < 0 || _pendingDecision is not { Kind: DecisionKind.PlayCard } prompt ||
            prompt.PlayerSeat != _options.HumanSeat || _winner != Winner.None) return null;
        var owner = _players[_options.HumanSeat];
        if (!owner.IsAlive || !GetSkillBindingShard(owner).ProgramInstances.Any(p => p.SkillId == WushuangJinzhiSkill)) return null;
        return new(WushuangJinzhiNextCount(owner.Seat), Array.AsReadOnly(WushuangJinzhiDiscardableCards(owner)
            .Select(card => card.Id).ToArray()));
    }

    private IEnumerable<Card> WushuangJinzhiDiscardableCards(CharacterState owner) =>
        GuanduOwnedCards(owner.Seat).Where(c => !c.IsGeneralWeapon &&
            !IsSelfHandCategoryDiscardForbidden(owner.Seat, c, _cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard));

    private IReadOnlyList<ProgramMultiCardViewAsSelection> WushuangJinzhiSelections(CharacterState owner, CardKind kind, bool response)
    {
        var instances = GetSkillBindingShard(owner).ProgramInstances.Where(p => p.SkillId == WushuangJinzhiSkill).ToArray();
        if (instances.Length == 0 || IsCardUseForbidden(owner.Seat, kind,
            response ? CardActionType.Response : CardActionType.Use)) return [];
        var count = WushuangJinzhiNextCount(owner.Seat);
        // These entities pay a discard. They are not the virtual basic card's
        // hand materials, so use/response-only hand bans do not forbid payment.
        var cards = WushuangJinzhiDiscardableCards(owner)
            .GroupBy(c => IsRedSuit(EffectiveSuit(owner, c))).ToArray();
        var selections = new List<ProgramMultiCardViewAsSelection>();
        foreach (var instance in instances)
        foreach (var rule in instance.Program.ViewAs.Where(v => v.OutputKind == kind && (response ? v.ForResponse : v.ForPlay)))
        foreach (var group in cards.Where(g => g.Count() >= count))
        foreach (var selected in EnumerateCardCombinations(group.OrderBy(c => c.Id).ToArray(), count))
            selections.Add(new(selected, new(instance.SkillId, rule.Id, owner.Seat, instance.SkillInstanceId), kind));
        return selections.Where(s => IsProgramViewAsAllowed(owner, s.OutputKind,
            s.Cards.Select(c => c.Id).ToArray(), s.Source, response)).ToArray();
    }
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 61010)]
    private bool WushuangJinzhiConversionForbidden(ProgramViewAsLegalityQuery query)
    {
        if (!IsWushuangJinzhiSource(query.Source)) return false;
        var cards = query.CardIds.Select(GetAdvancedCard).ToArray();
        return cards.Length != WushuangJinzhiNextCount(query.Owner.Seat) || cards.Select(c => IsRedSuit(EffectiveSuit(query.Owner, c))).Distinct().Count() != 1 ||
            cards.Any(c => c.IsGeneralWeapon || _cardZones.GetLocation(c.Id).OwnerSeat != query.Owner.Seat ||
                _cardZones.GetLocation(c.Id).Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) ||
                IsSelfHandCategoryDiscardForbidden(query.Owner.Seat, c, _cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard));
    }
    private void ContinueWushuangJinzhi(ProgramSkillFrame f)
    {
        var r = f.WushuangOriginalSkill!;
        if (r.Stage == "jinzhi-draw") { DrawWushuangOriginal(f, f.OwnerSeat, 1, "finish"); return; }
        if (r.Stage != "begin" || f.WindowContext is not { CardUse: { } c } context ||
            _resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(w => w.Id == context.ParentFrameId) is not { } window ||
            window.Action is not { } action || action.ActionId != c.CardActionId || action.ProviderSeat != f.OwnerSeat ||
            action.ConversionChain.Count(IsWushuangJinzhiSource) != 1 || action.PhysicalCards.Count != WushuangJinzhiNextCount(f.OwnerSeat) ||
            action.PhysicalCards.Any(cost => cost.From.OwnerSeat != f.OwnerSeat || cost.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) ||
            action.PhysicalCards.Select(cost => cost.EffectiveIsRed).Distinct().Count() != 1 ||
            action.PhysicalCards.Any(cost => _cardZones.GetLocation(cost.CardId) != CardLocation.Processing &&
                WushuangJinzhiDestroyedPayment(action, cost, c.ParentCardUseFrameId, _movementSequence) is null))
            throw new InvalidOperationException("Jinzhi requires its exact accepted once-paid same-color material action.");
        if (ProgramEventHistory<WushuangJinzhiPaidEvent>().Any(e => e.Action.ActionId == action.ActionId))
            throw new InvalidOperationException("One Jinzhi action cannot pay its dynamic ordinal twice.");
        var ordinal = WushuangJinzhiNextCount(f.OwnerSeat);
        var prepaid = action.PhysicalCards.Select(cost =>
            WushuangJinzhiDestroyedPayment(action, cost, c.ParentCardUseFrameId, _movementSequence))
            .OfType<CardMovementRecord>().ToArray();
        AdvanceEventRulesAndQueueFact(new WushuangJinzhiPaidEvent(f.Id, f.OwnerSeat, _roundNumber, ordinal,
            _turnNumber, _movementSequence, action)
            { MaterialPaymentFrameId = prepaid.Length > 0 ? c.ParentCardUseFrameId : null });
        var reason = new CardMoveReason(WushuangJinzhiDiscardReason);
        var invoice = TracksNativeLogicalDiscards ? new NativeLogicalDiscardInvocationDraft
        {
            InvocationId = ++_resolutionSequence, TurnNumber = _turnNumber, ActualTurnOwnerSeat = _currentSeat,
            InitiatorSeat = f.OwnerSeat, ExecutorSeat = f.OwnerSeat, ParentFrameId = f.Id, Reason = reason,
            SequenceBefore = prepaid.Length == 0 ? _movementSequence : prepaid.Min(m => m.Sequence) - 1,
            Materials = action.PhysicalCards.Select(cost => new NativeLogicalDiscardMaterial(cost.CardId, cost.From)).ToArray()
        } : null;
        f = WushuangOriginalStage(f, "jinzhi-draw");
        // Shop equipment has already paid by its irreversible native departure.
        // Credit that exact movement; never move the destroyed entity again.
        var processing = action.PhysicalCards.Where(cost => _cardZones.GetLocation(cost.CardId) == CardLocation.Processing)
            .Select(cost => cost.CardId).ToArray();
        if (processing.Length > 0)
            MoveProgramCardsFromMultipleSources(processing, CardLocation.DiscardPile, reason, disclosure: CardMovementDisclosure.FaceUp);
        CompleteNativeLogicalDiscardInvocation(invoice, prepaid);
        RunWushuangOriginalSkill(f.Id);
    }
    private CardMovementRecord? WushuangJinzhiDestroyedPayment(CardActionContext action, CardActionCost cost,
        long paymentFrameId, int movementThrough)
    {
        if (cost.From != CardLocation.Equipment(action.ProviderSeat) ||
            action.ConversionChain.Count(IsWushuangJinzhiSource) != 1 ||
            _cardZones.GetLocation(cost.CardId) != CardLocation.OutsideGame ||
            !HasWushuangJinzhiPaymentAction(action, paymentFrameId) ||
            ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Where(e => e.CardId == cost.CardId).ToArray() is not [var destroyed] ||
            destroyed.ParentFrameId != paymentFrameId || destroyed.MovementSequence > movementThrough)
            return null;
        var movement = _cardMovements.LastOrDefault(m => m.CardId == cost.CardId && m.Sequence == destroyed.MovementSequence);
        return movement is not null && movement.CardKind == cost.CardKind && movement.From == cost.From &&
            movement.Reason == (action.Type == CardActionType.Use ? CardMoveReasons.Use : CardMoveReasons.Respond) &&
            IsWushuangShopDestroyedMaterial(movement) ? movement : null;
    }
    private bool HasWushuangJinzhiPaymentAction(CardActionContext action, long paymentFrameId)
    {
        bool Matches(CardActionContext issued) => issued.ActionId == action.ActionId &&
            issued.Type == action.Type && issued.ActorSeat == action.ActorSeat && issued.ProviderSeat == action.ProviderSeat &&
            issued.RequesterSeat == action.RequesterSeat && issued.ResponderSeat == action.ResponderSeat &&
            issued.OpponentSeat == action.OpponentSeat && issued.EffectiveKind == action.EffectiveKind &&
            issued.PhysicalCards.SequenceEqual(action.PhysicalCards) && issued.ConversionChain.SequenceEqual(action.ConversionChain);
        var accepted = ProgramEventHistory<CardActionAcceptedEvent>().Where(e => e.Action.ActionId == action.ActionId).ToArray();
        if (accepted.Length > 0) return accepted.All(e => Matches(e.Action));
        // Simple-card commitment runs before its optional finalized-target
        // acceptance event. Its issued native owner is already authoritative.
        if (action.Type == CardActionType.Use && LifecycleCardUse(paymentFrameId) is
            { ProgramUseCommitted: true, Action: { } committed } use &&
            use.SourceSeat == action.ActorSeat && use.CardKind == action.EffectiveKind && Matches(committed)) return true;
        // The paid receipt retains that checked owner after its native frame
        // retires, including catalogs with no finalized-target subscribers.
        return ProgramEventHistory<WushuangJinzhiPaidEvent>().Where(e => e.Action.ActionId == action.ActionId).ToArray() is [var paid] &&
            paid.MaterialPaymentFrameId == paymentFrameId && Matches(paid.Action);
    }
    private CardLocation? WushuangJinzhiDiscardSource(CardMovementRecord movement)
    {
        if (movement.To == CardLocation.OutsideGame)
        {
            var prepaid = ProgramEventHistory<WushuangJinzhiPaidEvent>().LastOrDefault(e =>
                e.MaterialPaymentFrameId is { } parent && e.Action.PhysicalCards.Any(cost =>
                    cost.CardId == movement.CardId && WushuangJinzhiDestroyedPayment(e.Action, cost, parent, e.MovementBefore) == movement));
            return prepaid?.Action.PhysicalCards.Single(cost => cost.CardId == movement.CardId).From;
        }
        if (movement.From != CardLocation.Processing || movement.To != CardLocation.DiscardPile ||
            movement.Reason.Value != WushuangJinzhiDiscardReason) return null;
        var previous = _cardMovements.LastOrDefault(m => m.CardId == movement.CardId && m.Sequence < movement.Sequence);
        if (previous is null || previous.To != CardLocation.Processing) return null;
        var paid = ProgramEventHistory<WushuangJinzhiPaidEvent>().LastOrDefault(e => e.MovementBefore >= previous.Sequence && e.MovementBefore < movement.Sequence &&
            e.Action.PhysicalCards.Any(cost => cost.CardId == movement.CardId) &&
            e.Action.PhysicalCards.Any(cost => cost.CardId == movement.CardId && cost.From == previous.From));
        return paid?.Action.PhysicalCards.Single(cost => cost.CardId == movement.CardId).From;
    }
    private bool IsWushuangJinzhiPaidMaterial(CardActionContext? action, int card) => action is not null &&
        ProgramEventHistory<WushuangJinzhiPaidEvent>().Any(e => e.Action.ActionId == action.ActionId &&
            e.Action.PhysicalCards.Any(cost => cost.CardId == card));
    private bool TryGetWushuangJinzhiResponseMovements(CardActionContext action, int card, out IReadOnlyList<CardMovementRecord> movements)
    {
        movements = [];
        if (!IsWushuangJinzhiPaidMaterial(action, card)) return false;
        var paid = ProgramEventHistory<WushuangJinzhiPaidEvent>().Single(e => e.Action.ActionId == action.ActionId);
        var cost = paid.Action.PhysicalCards.Single(c => c.CardId == card);
        if (paid.MaterialPaymentFrameId is { } parent &&
            WushuangJinzhiDestroyedPayment(paid.Action, cost, parent, paid.MovementBefore) is { } destroyed)
        { movements = Array.AsReadOnly(new[] { destroyed }); return true; }
        var entry = _cardMovements.LastOrDefault(m => m.CardId == card && m.Sequence <= paid.MovementBefore && m.From == cost.From && m.To == CardLocation.Processing);
        var finish = _cardMovements.FirstOrDefault(m => m.CardId == card && m.Sequence > paid.MovementBefore && m.Reason.Value == WushuangJinzhiDiscardReason && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile);
        if (entry is null || finish is null) throw new InvalidOperationException("A Jinzhi response lost its exact entered and discarded materials.");
        movements = Array.AsReadOnly(new[] { entry, finish }); return true;
    }
    private bool TryResolveWushuangJinzhiDyingAlcohol(CharacterState owner, ProgramMultiCardViewAsSelection selection)
    {
        if (selection.OutputKind != CardKind.Alcohol || !IsWushuangJinzhiSelection(selection)) return false;
        if (ActiveDying is not { } dying || dying.VictimSeat != owner.Seat || dying.ResponderSeat != owner.Seat)
            throw new InvalidOperationException("Jinzhi Alcohol requires the actual owner's self-rescue.");
        ClearPendingDecision();
        ResolveRecoveryCard(owner, owner, ApplyProgramUseAppearance(owner, selection.Cards[0], selection.Source), "酒", CardKind.Alcohol, conversionSource: selection.Source,
            dyingResponse: new(dying.FrameId, owner.Seat, false, null, true, selection.Cards[0].Id), physicalCards: selection.Cards);
        return true;
    }
}
