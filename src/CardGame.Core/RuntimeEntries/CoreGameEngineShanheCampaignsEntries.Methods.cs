#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheCampaignsEntries
{
    internal void ExcludeShanheDeckEquipment(global::CardGame.Core.CardLocation zone) => _resolve().ExcludeShanheDeckEquipment(zone);
    internal void PlaceShanhePhysicalCard(global::CardGame.Core.Campaigns.ShanheStartingCard item, int seat, bool equipment) => _resolve().PlaceShanhePhysicalCard(item, seat, equipment);
}
