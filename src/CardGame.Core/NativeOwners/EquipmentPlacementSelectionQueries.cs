#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class EquipmentPlacementSelectionQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentPlacementAndHpPairEntries> getCoreGameEngineEquipmentPlacementAndHpPairRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCapturedEquipmentDrawsEntries> getCoreGameEngineCapturedEquipmentDrawsRuntime) : IEquipmentPlacementSelectionQueries
{
    public bool CanPlace(int ownerSeat, int targetSeat, int cardId) => getCoreGameEngineEquipmentPlacementAndHpPairRuntime().CanPlaceActivationEquipment(ownerSeat, targetSeat, cardId);
    public bool CanPlaceCaptured(int ownerSeat, int targetSeat, int cardId, string skillId) => getCoreGameEngineCapturedEquipmentDrawsRuntime().CanPlaceCapturedEquipmentActivation(ownerSeat, targetSeat, cardId, skillId);
}
