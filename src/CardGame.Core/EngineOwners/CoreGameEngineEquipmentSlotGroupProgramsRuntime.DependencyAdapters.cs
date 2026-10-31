#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTargetSelectionRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsProgramTargetSelectionAdapter(global::System.Func<global::CardGame.Core.ProgramTargetSelectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsProgramTargetSelectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTargetSelectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetSelectionRuntime Get() => this._targetFactory();
}
