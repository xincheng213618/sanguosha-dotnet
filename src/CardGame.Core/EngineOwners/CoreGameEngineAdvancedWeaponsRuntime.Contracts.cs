#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreAdvancedWeaponsCoreTrustedCardZoneDiagnosticsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics();
}

internal interface ICoreAdvancedWeaponsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
