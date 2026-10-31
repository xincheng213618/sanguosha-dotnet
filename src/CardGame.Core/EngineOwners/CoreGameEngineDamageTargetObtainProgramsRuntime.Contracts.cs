#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDamageTargetObtainProgramsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreDamageTargetObtainProgramsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
