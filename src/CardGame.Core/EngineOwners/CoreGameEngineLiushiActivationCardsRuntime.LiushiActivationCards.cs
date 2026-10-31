#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineLiushiActivationCardsRuntime
{
    bool global::CardGame.Core.ILiushiTopdeckSelectionQuery.CanTopdeck(int ownerSeat, int cardId) => CanTopdeckLiushiCard(ownerSeat, cardId);
    internal bool CanTopdeckLiushiCard(int ownerSeat, int cardId)
    {
        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(ownerSeat)).SingleOrDefault(c => c.Id == cardId);
        if (card is null)
            return false;
        var from = _cardZonesOperations._cardZones.GetLocation(cardId);
        return !(card.IsGeneralWeapon && from.Zone == CardZoneKind.Equipment) && !global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, from, CardLocation.DrawPile) && _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, CardLocation.DrawPile) == CardLocation.DrawPile;
    }
}
