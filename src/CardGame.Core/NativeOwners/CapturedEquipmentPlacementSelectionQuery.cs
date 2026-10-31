#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CapturedEquipmentPlacementSelectionQuery(Func<RuntimeEntries.CoreGameEngineCapturedEquipmentDrawsEntries> getCoreGameEngineCapturedEquipmentDrawsRuntime) : ICapturedEquipmentPlacementSelectionQuery
{
    public bool CanPlace(int ownerSeat, int targetSeat, int cardId, string skillId) => getCoreGameEngineCapturedEquipmentDrawsRuntime().CanPlaceCapturedEquipmentActivation(ownerSeat, targetSeat, cardId, skillId);
}
