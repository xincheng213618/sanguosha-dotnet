#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeEncountersRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateModeEncounterHostOperations _kernelCoreGameEngineModeEncountersOperations;
    internal CoreGameEngineModeEncountersRuntime(global::CardGame.Core.EngineOwners.IKernelCreateModeEncounterHostOperations kernelCoreGameEngineModeEncountersOperations)
    {
        this._kernelCoreGameEngineModeEncountersOperations = kernelCoreGameEngineModeEncountersOperations;
    }
}
