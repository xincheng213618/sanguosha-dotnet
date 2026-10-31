#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICorePhysicalMovementFacadesMovementRecordOperations : IProgramRuntimeOperations
{
    MovementRecordRuntime Get();
}

internal interface ICorePhysicalMovementFacadesMovementEquipmentEffectsOperations : IProgramRuntimeOperations
{
    MovementEquipmentEffectsRuntime Get();
}
