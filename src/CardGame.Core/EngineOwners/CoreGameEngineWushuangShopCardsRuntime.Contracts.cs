#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreWushuangShopCardsCoreWushuangModeProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangMode { get; }
}

internal interface ICoreWushuangShopCardsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreWushuangShopCardsCoreTrustedCardZoneDiagnosticsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneDiagnostic> CreateCardZoneDiagnostics();
}
