#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangShopCardsRuntime
{
    internal void PlaceWushuangShopCard(int seat, string offerId, WushuangCard definition)
    {
        if (definition.Kind is not { } kind)
            throw new InvalidOperationException("The shop card has no executable binding.");
        var card = new Card(_coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty().Max() + 1, kind, definition.Suit, definition.Rank);
        _cardZonesOperations._cardZones.AddGeneratedCard(card);
        _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(card.Id);
        if (definition.IsEquipment)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangShopEquipmentCreatedEvent(seat, offerId, definition.Id, card.Id));
            _coreAdvancedWeaponsOperations.PlaceAdvancedEquipment(card, seat);
        }
        else
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.OutsideGame, CardLocation.Hand(seat), new("wushuang.shop-card"));
    }

    internal bool IsWushuangShopEquipment(int cardId) => _coreWushuangModeProjectionOperations.IsWushuangMode && _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Contains(cardId) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangShopEquipmentCreatedEvent>().Where(e => e.CardId == cardId).ToArray()is [var created] && WushuangCatalogue.Cards.Single(c => c.Id == created.SourceOfferId)is { IsEquipment: true, Kind: { } kind } source && _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId)is { } card && card.Kind == kind && card.Suit == source.Suit && card.Rank == source.Rank;
    internal CardLocation NormalizeWushuangShopEquipmentDestination(Card card, CardLocation from, CardLocation to)
    {
        if (!IsWushuangShopEquipment(card.Id) || from == to)
            return to;
        var created = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangShopEquipmentCreatedEvent>().Single(e => e.CardId == card.Id);
        return _coreEquipmentContentOperations.EquipmentRules.NormalizeDestination(new(card, from, to, new(card.Id, BuiltInEquipmentContent.WushuangShopSource, created.Seat), HasPriorMovement: _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.CardId == card.Id)));
    }

    internal void RecordWushuangShopEquipmentDestruction(CardMovementRecord movement)
    {
        if (movement.From.Zone != CardZoneKind.Equipment || movement.To != CardLocation.OutsideGame || !IsWushuangShopEquipment(movement.CardId))
            return;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Any(e => e.CardId == movement.CardId))
            throw new InvalidOperationException("A purchased equipment card cannot leave the game twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangShopEquipmentDestroyedEvent(movement.CardId, movement.Sequence, _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id ?? 0));
    }

    internal bool IsWushuangShopDestroyedMaterial(CardMovementRecord movement) => movement.From.Zone == CardZoneKind.Equipment && movement.To == CardLocation.OutsideGame && IsWushuangShopEquipment(movement.CardId) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(movement) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Where(e => e.CardId == movement.CardId).ToArray()is [var fact] && fact.MovementSequence == movement.Sequence;
    internal bool IsWushuangShopDestroyedNullificationPayment(long windowId, int provider, CardActionCost material, int afterSequence = -1) => material.From == CardLocation.Equipment(provider) && _cardZonesOperations._cardZones.GetLocation(material.CardId) == CardLocation.OutsideGame && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangShopEquipmentCreatedEvent>().SingleOrDefault(e => e.CardId == material.CardId)?.Seat == provider && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().SingleOrDefault(e => e.CardId == material.CardId)is { } destroyed && destroyed.ParentFrameId == windowId && destroyed.MovementSequence > afterSequence && _coreCardMovementJournalCardMovementsAccess._cardMovements.SingleOrDefault(m => m.Sequence == destroyed.MovementSequence)is { } movement && movement.CardId == material.CardId && movement.CardKind == material.CardKind && movement.From == material.From && movement.Reason == CardMoveReasons.Nullification && IsWushuangShopDestroyedMaterial(movement);
    internal bool IsWushuangShopDestroyedNullificationPayment(CardActionContext action, long windowId, CardActionCost material) => action is { Type: CardActionType.Response, EffectiveKind: CardKind.Nullification } && action.ActorSeat == action.ProviderSeat && action.PhysicalCards.Contains(material) && _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().SingleOrDefault(f => f.Id == windowId)is { } window && action.ParentActionId == _coreDelayedNullificationOperations.Value.NullificationParentActionId(window) && IsWushuangShopDestroyedNullificationPayment(windowId, action.ProviderSeat, material);
    internal CardMovementRecord? DestroyedEquipmentResponsePayment(CardActionContext action, long windowId, CardActionCost material, ProgramCardContinuation continuation) => continuation == ProgramCardContinuation.NullificationResponse && IsWushuangShopDestroyedNullificationPayment(action, windowId, material) ? _coreCardMovementJournalCardMovementsAccess._cardMovements.Single(m => m.Sequence == _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Single(e => e.CardId == material.CardId).MovementSequence) : null;
}
