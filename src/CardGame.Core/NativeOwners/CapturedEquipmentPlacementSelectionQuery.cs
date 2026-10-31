#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CapturedEquipmentPlacementSelectionQuery(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime>> getCoreGameEngineCapturedEquipmentDrawsRuntime) : ICapturedEquipmentPlacementSelectionQuery
{
    public bool CanPlace(int ownerSeat, int targetSeat, int cardId, string skillId) => getCoreGameEngineCapturedEquipmentDrawsRuntime().Value.CanPlaceCapturedEquipmentActivation(ownerSeat, targetSeat, cardId, skillId);
}
