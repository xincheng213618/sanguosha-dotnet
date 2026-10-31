#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreAvailableBoundSubsetProgramsRuntime : global::CardGame.Core.IAvailableBoundSubsetProgramEffectHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    internal CoreAvailableBoundSubsetProgramsRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations)
    {
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
    }
}
