#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class EquipmentPlacementSelectionQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentPlacementAndHpPairEntries> getCoreGameEngineEquipmentPlacementAndHpPairRuntime) : IEquipmentPlacementSelectionQuery
{
    public bool CanPlace(int ownerSeat, int targetSeat, int cardId) => getCoreGameEngineEquipmentPlacementAndHpPairRuntime().CanPlaceActivationEquipment(ownerSeat, targetSeat, cardId);
}
