#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CorePhysicalMovementFacadesRuntime
{
    private readonly RuntimeEntries.CoreGameEngineAtomicCardMovementEntries _coreAtomicCardMovementOperations;
    private readonly ICorePhysicalMovementFacadesMovementEquipmentEffectsOperations _movementEquipmentEffectsOperations;
    private readonly ICorePhysicalMovementFacadesMovementRecordOperations _movementRecordOperations;
    internal CorePhysicalMovementFacadesRuntime(RuntimeEntries.CoreGameEngineAtomicCardMovementEntries coreAtomicCardMovementOperations, ICorePhysicalMovementFacadesMovementEquipmentEffectsOperations movementEquipmentEffectsOperations, ICorePhysicalMovementFacadesMovementRecordOperations movementRecordOperations)
    {
        this._coreAtomicCardMovementOperations = coreAtomicCardMovementOperations;
        this._movementEquipmentEffectsOperations = movementEquipmentEffectsOperations;
        this._movementRecordOperations = movementRecordOperations;
    }
}
