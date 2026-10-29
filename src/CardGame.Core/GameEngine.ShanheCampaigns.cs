using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private static GameOptions FreezeShanheBattleOptions(GameOptions options) =>
        global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ShanheCampaigns.FreezeShanheBattleOptions(options);


    private void PlaceShanhePhysicalCard(ShanheStartingCard item, int seat, bool equipment)
    {
        var card = new Card(CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty(0).Max() + 1,
            item.Kind, item.Suit, item.Rank);
        _cardZones.AddGeneratedCard(card);
        _generatedPhysicalCardIds.Add(card.Id);
        if (equipment) PlaceAdvancedEquipment(card, seat);
        else MoveCard(card, CardLocation.OutsideGame, CardLocation.Hand(seat), CardMoveReasons.InitialDeal);
    }

    private void ExcludeShanheDeckEquipment(CardLocation zone)
    {
        // The equipment-free deck belongs to the opted-in resource contract.
        // Legacy recipes retain their authored deck through command replay.
        if (_options.ShanheBattle is not { StartingResourcesExpireOnUse: true }) return;
        var equipment = _cardZones.CardsAt(zone).Where(card => EquipmentRules.IsEquipment(card.Kind)).ToArray();
        if (equipment.Length > 0)
            MoveCards(equipment, zone, CardLocation.OutsideGame, new CardMoveReason("shanhe.deck.exclude-equipment"));
    }
}
