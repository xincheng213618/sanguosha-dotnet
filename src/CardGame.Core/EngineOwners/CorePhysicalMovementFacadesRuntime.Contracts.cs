#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePhysicalMovementFacadesMovementRecordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MovementRecordRuntime Get();
}

internal interface ICorePhysicalMovementFacadesMovementEquipmentEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MovementEquipmentEffectsRuntime Get();
}
