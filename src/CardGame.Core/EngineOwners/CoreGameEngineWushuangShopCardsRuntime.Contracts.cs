#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreWushuangShopCardsCoreWushuangModeProjectionOperations : IProgramRuntimeOperations
{
    bool IsWushuangMode { get; }
}

internal interface ICoreWushuangShopCardsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreWushuangShopCardsCoreTrustedCardZoneDiagnosticsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics();
}
