#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeEncountersRuntime
{
    private readonly ICoreModeEncountersKernelCoreGameEngineModeEncountersOperations _kernelCoreGameEngineModeEncountersOperations;
    internal CoreGameEngineModeEncountersRuntime(ICoreModeEncountersKernelCoreGameEngineModeEncountersOperations kernelCoreGameEngineModeEncountersOperations)
    {
        this._kernelCoreGameEngineModeEncountersOperations = kernelCoreGameEngineModeEncountersOperations;
    }
}
