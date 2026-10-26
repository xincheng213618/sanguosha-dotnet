namespace CardGame.Core;

public sealed record WushuangShopEquipmentCreatedEvent(int Seat, string OfferId, int SourceOfferId, int CardId) : IGameEvent;
public sealed record WushuangShopEquipmentDestroyedEvent(int CardId, int MovementSequence, long ParentFrameId) : IGameEvent;

public sealed partial class GameEngine
{
    private void PlaceWushuangShopCard(int seat, string offerId, WushuangCard definition)
    {
        if (definition.Kind is not { } kind) throw new InvalidOperationException("The shop card has no executable binding.");
        var card = new Card(CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty().Max() + 1,
            kind, definition.Suit, definition.Rank);
        _cardZones.AddGeneratedCard(card);
        _generatedPhysicalCardIds.Add(card.Id);
        if (definition.IsEquipment)
        {
            AdvanceEventRulesAndQueueFact(new WushuangShopEquipmentCreatedEvent(seat, offerId, definition.Id, card.Id));
            PlaceAdvancedEquipment(card, seat);
        }
        else MoveCard(card, CardLocation.OutsideGame, CardLocation.Hand(seat), new("wushuang.shop-card"));
    }

    private bool IsWushuangShopEquipment(int cardId) => IsWushuangMode && _generatedPhysicalCardIds.Contains(cardId) &&
        ProgramEventHistory<WushuangShopEquipmentCreatedEvent>().Where(e => e.CardId == cardId).ToArray() is [var created] &&
        WushuangCatalogue.Cards.Single(c => c.Id == created.SourceOfferId) is { IsEquipment: true, Kind: { } kind } source &&
        GetAdvancedCard(cardId) is { } card && card.Kind == kind && card.Suit == source.Suit && card.Rank == source.Rank;

    private CardLocation NormalizeWushuangShopEquipmentDestination(Card card, CardLocation from, CardLocation to)
    {
        if (!IsWushuangShopEquipment(card.Id) || from == to) return to;
        var created = ProgramEventHistory<WushuangShopEquipmentCreatedEvent>().Single(e => e.CardId == card.Id);
        return EquipmentRules.NormalizeDestination(new(card, from, to,
            new(card.Id, BuiltInEquipmentContent.WushuangShopSource, created.Seat),
            HasPriorMovement: _cardMovements.Any(m => m.CardId == card.Id)));
    }

    private void RecordWushuangShopEquipmentDestruction(CardMovementRecord movement)
    {
        if (movement.From.Zone != CardZoneKind.Equipment || movement.To != CardLocation.OutsideGame ||
            !IsWushuangShopEquipment(movement.CardId)) return;
        if (ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Any(e => e.CardId == movement.CardId))
            throw new InvalidOperationException("A purchased equipment card cannot leave the game twice.");
        AdvanceEventRulesAndQueueFact(new WushuangShopEquipmentDestroyedEvent(movement.CardId, movement.Sequence,
            _resolutionStack.LastOrDefault()?.Id ?? 0));
    }

    private bool IsWushuangShopDestroyedMaterial(CardMovementRecord movement) =>
        movement.From.Zone == CardZoneKind.Equipment && movement.To == CardLocation.OutsideGame &&
        IsWushuangShopEquipment(movement.CardId) && _cardMovements.Contains(movement) &&
        ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Where(e => e.CardId == movement.CardId).ToArray() is [var fact] &&
        fact.MovementSequence == movement.Sequence;

    private bool IsWushuangShopDestroyedNullificationPayment(long windowId, int provider,
        CardActionCost material, int afterSequence = -1) =>
        material.From == CardLocation.Equipment(provider) &&
        _cardZones.GetLocation(material.CardId) == CardLocation.OutsideGame &&
        ProgramEventHistory<WushuangShopEquipmentCreatedEvent>().SingleOrDefault(e => e.CardId == material.CardId)?.Seat == provider &&
        ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().SingleOrDefault(e => e.CardId == material.CardId) is { } destroyed &&
        destroyed.ParentFrameId == windowId && destroyed.MovementSequence > afterSequence &&
        _cardMovements.SingleOrDefault(m => m.Sequence == destroyed.MovementSequence) is { } movement &&
        movement.CardId == material.CardId && movement.CardKind == material.CardKind && movement.From == material.From &&
        movement.Reason == CardMoveReasons.Nullification && IsWushuangShopDestroyedMaterial(movement);

    private bool IsWushuangShopDestroyedNullificationPayment(CardActionContext action, long windowId, CardActionCost material) =>
        action is { Type: CardActionType.Response, EffectiveKind: CardKind.Nullification } &&
        action.ActorSeat == action.ProviderSeat && action.PhysicalCards.Contains(material) &&
        _resolutionStack.OfType<NullificationWindowFrame>().SingleOrDefault(f => f.Id == windowId) is { } window &&
        action.ParentActionId == NullificationParentActionId(window) &&
        IsWushuangShopDestroyedNullificationPayment(windowId, action.ProviderSeat, material);

    private CardMovementRecord? DestroyedEquipmentResponsePayment(CardActionContext action, long windowId,
        CardActionCost material, ProgramCardContinuation continuation) =>
        continuation == ProgramCardContinuation.NullificationResponse &&
        IsWushuangShopDestroyedNullificationPayment(action, windowId, material)
            ? _cardMovements.Single(m => m.Sequence == ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>()
                .Single(e => e.CardId == material.CardId).MovementSequence) : null;
}
