#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineLiushiActivationCardsRuntime
{
    bool ILiushiTopdeckSelectionQuery.CanTopdeck(int ownerSeat, int cardId) => CanTopdeckLiushiCard(ownerSeat, cardId);
    internal bool CanTopdeckLiushiCard(int ownerSeat, int cardId)
    {
        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(ownerSeat)).SingleOrDefault(c => c.Id == cardId);
        if (card is null)
            return false;
        var from = _cardZonesOperations._cardZones.GetLocation(cardId);
        return !(card.IsGeneralWeapon && from.Zone == CardZoneKind.Equipment) && !CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, from, CardLocation.DrawPile) && _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, CardLocation.DrawPile) == CardLocation.DrawPile;
    }
}
