#nullable enable

using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheCampaignsRuntime
{
    internal static GameOptions FreezeShanheBattleOptions(GameOptions options) => RuntimeSkills.RuntimeRules_Core_ShanheCampaigns.FreezeShanheBattleOptions(options);
    internal void PlaceShanhePhysicalCard(ShanheStartingCard item, int seat, bool equipment)
    {
        var card = new Card(_coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty(0).Max() + 1, item.Kind, item.Suit, item.Rank);
        _cardZonesOperations._cardZones.AddGeneratedCard(card);
        _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(card.Id);
        if (equipment)
            _coreAdvancedWeaponsOperations.PlaceAdvancedEquipment(card, seat);
        else
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.OutsideGame, CardLocation.Hand(seat), CardMoveReasons.InitialDeal);
    }

    internal void ExcludeShanheDeckEquipment(CardLocation zone)
    {
        // The equipment-free deck belongs to the opted-in resource contract.
        // Legacy recipes retain their authored deck through command replay.
        if (_coreMatchDefinitionOptionsAccess._options.ShanheBattle is not { StartingResourcesExpireOnUse: true })
            return;
        var equipment = _cardZonesOperations._cardZones.CardsAt(zone).Where(card => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind)).ToArray();
        if (equipment.Length > 0)
            _corePhysicalMovementFacadesOperations.MoveCards(equipment, zone, CardLocation.OutsideGame, new CardMoveReason("shanhe.deck.exclude-equipment"));
    }
}
