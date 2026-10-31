#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhangHeMouMobileCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterZhangHeMouMobileMouQiaobianOperations : IProgramRuntimeOperations
{
    MouQiaobianRuntime Get();
}

internal interface ICharacterZhangHeMouMobileCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
