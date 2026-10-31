#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePrepDiscardPaymentsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICorePrepDiscardPaymentsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
