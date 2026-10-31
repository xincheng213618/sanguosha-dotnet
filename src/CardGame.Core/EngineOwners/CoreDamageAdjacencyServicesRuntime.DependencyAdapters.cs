#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreProgramRuntimeHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesCoreProgramRuntimeHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.DamageAdjacencyRuntime))]
internal sealed class CoreDamageAdjacencyServicesDamageAdjacencyAdapter(global::System.Func<global::CardGame.Core.DamageAdjacencyRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesDamageAdjacencyOperations
{
    private readonly global::System.Func<global::CardGame.Core.DamageAdjacencyRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageAdjacencyRuntime Get() => this._targetFactory();
}
