#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreShanheEmptySlotEquipmentCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreShanheEmptySlotEquipmentCoreTrustedCardZoneDiagnosticsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics();
}
