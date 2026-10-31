#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CorePhysicalMovementFacadesRuntime
{
    private readonly ICorePhysicalMovementFacadesCoreAtomicCardMovementOperations _coreAtomicCardMovementOperations;
    private readonly ICorePhysicalMovementFacadesMovementEquipmentEffectsOperations _movementEquipmentEffectsOperations;
    private readonly ICorePhysicalMovementFacadesMovementRecordOperations _movementRecordOperations;
    internal CorePhysicalMovementFacadesRuntime(ICorePhysicalMovementFacadesCoreAtomicCardMovementOperations coreAtomicCardMovementOperations, ICorePhysicalMovementFacadesMovementEquipmentEffectsOperations movementEquipmentEffectsOperations, ICorePhysicalMovementFacadesMovementRecordOperations movementRecordOperations)
    {
        this._coreAtomicCardMovementOperations = coreAtomicCardMovementOperations;
        this._movementEquipmentEffectsOperations = movementEquipmentEffectsOperations;
        this._movementRecordOperations = movementRecordOperations;
    }
}
