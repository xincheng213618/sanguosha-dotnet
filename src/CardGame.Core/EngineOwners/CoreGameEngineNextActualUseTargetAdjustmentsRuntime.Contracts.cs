#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNextActualUseTargetAdjustmentsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreTrustedMovementDiagnosticsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<CardMovementRecord> CardMovements { get; }
}
