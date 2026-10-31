#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class EquipmentPlacementSelectionQueries(Func<RuntimeEntries.CoreGameEngineEquipmentPlacementAndHpPairEntries> getCoreGameEngineEquipmentPlacementAndHpPairRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime>> getCoreGameEngineCapturedEquipmentDrawsRuntime) : IEquipmentPlacementSelectionQueries
{
    public bool CanPlace(int ownerSeat, int targetSeat, int cardId) => getCoreGameEngineEquipmentPlacementAndHpPairRuntime().CanPlaceActivationEquipment(ownerSeat, targetSeat, cardId);
    public bool CanPlaceCaptured(int ownerSeat, int targetSeat, int cardId, string skillId) => getCoreGameEngineCapturedEquipmentDrawsRuntime().Value.CanPlaceCapturedEquipmentActivation(ownerSeat, targetSeat, cardId, skillId);
}
