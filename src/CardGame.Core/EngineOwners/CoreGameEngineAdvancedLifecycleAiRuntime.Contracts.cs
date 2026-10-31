#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreAdvancedLifecycleAiCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreAdvancedLifecycleAiCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
