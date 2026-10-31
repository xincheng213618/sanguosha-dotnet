#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreNextActualUseTargetAdjustmentsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreTrustedMovementDiagnosticsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> CardMovements { get; }
}
