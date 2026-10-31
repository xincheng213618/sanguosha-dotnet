#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MovementRecordRuntime))]
internal sealed class CorePhysicalMovementFacadesMovementRecordAdapter(global::System.Func<global::CardGame.Core.MovementRecordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhysicalMovementFacadesMovementRecordOperations
{
    private readonly global::System.Func<global::CardGame.Core.MovementRecordRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MovementRecordRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MovementEquipmentEffectsRuntime))]
internal sealed class CorePhysicalMovementFacadesMovementEquipmentEffectsAdapter(global::System.Func<global::CardGame.Core.MovementEquipmentEffectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhysicalMovementFacadesMovementEquipmentEffectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.MovementEquipmentEffectsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MovementEquipmentEffectsRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAtomicCardMovementRuntime))]
internal sealed class CorePhysicalMovementFacadesCoreAtomicCardMovementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAtomicCardMovementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhysicalMovementFacadesCoreAtomicCardMovementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAtomicCardMovementRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.AtomicCardMovementRuntime AtomicCardMovements { get => this._targetFactory().AtomicCardMovements; }
}
