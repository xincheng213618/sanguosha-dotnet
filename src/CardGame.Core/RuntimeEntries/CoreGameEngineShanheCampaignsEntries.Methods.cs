#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheCampaignsEntries
{
    internal void ExcludeShanheDeckEquipment(CardLocation zone) => _resolve().ExcludeShanheDeckEquipment(zone);
    internal void PlaceShanhePhysicalCard(Campaigns.ShanheStartingCard item, int seat, bool equipment) => _resolve().PlaceShanhePhysicalCard(item, seat, equipment);
}
