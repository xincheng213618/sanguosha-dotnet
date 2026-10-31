#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreShanheEmptySlotEquipmentCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheEmptySlotEquipmentCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTrustedCardZoneDiagnosticsRuntime))]
internal sealed class CoreShanheEmptySlotEquipmentCoreTrustedCardZoneDiagnosticsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTrustedCardZoneDiagnosticsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheEmptySlotEquipmentCoreTrustedCardZoneDiagnosticsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTrustedCardZoneDiagnosticsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneDiagnostic> CreateCardZoneDiagnostics() => this._targetFactory().CreateCardZoneDiagnostics();
}
