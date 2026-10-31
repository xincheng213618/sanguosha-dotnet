#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterPanZhangMaZhongBoundaryMobileCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterPanZhangMaZhongBoundaryMobileCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
