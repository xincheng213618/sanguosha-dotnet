#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime))]
internal sealed class CoreNativeActionDispatchCoreProgramActionModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreProgramActionModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramActionRouter ProgramActions { get => this._targetFactory().ProgramActions; }
}
