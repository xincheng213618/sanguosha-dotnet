#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed class CoreDamageObserverCursorQueriesCoreDamageCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageObserverCursorQueriesCoreDamageCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get => this._targetFactory().ActiveDamageTrigger; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageAdjacencyServicesRuntime))]
internal sealed class CoreDamageObserverCursorQueriesCoreDamageAdjacencyServicesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageAdjacencyServicesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageObserverCursorQueriesCoreDamageAdjacencyServicesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageAdjacencyServicesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageAdjacencyRuntime DamageAdjacency { get => this._targetFactory().DamageAdjacency; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreDamageObserverCursorQueriesCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageObserverCursorQueriesCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}
