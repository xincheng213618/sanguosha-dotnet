#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreNextActualUseTargetAdjustmentsCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNextActualUseTargetAdjustmentsCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTrustedMovementDiagnosticsRuntime))]
internal sealed class CoreNextActualUseTargetAdjustmentsCoreTrustedMovementDiagnosticsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTrustedMovementDiagnosticsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNextActualUseTargetAdjustmentsCoreTrustedMovementDiagnosticsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTrustedMovementDiagnosticsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> CardMovements { get => this._targetFactory().CardMovements; }
}
