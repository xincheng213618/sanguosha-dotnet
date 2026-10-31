#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHellEncounterHostRuntime
{
    private readonly ICoreHellEncounterHostModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICoreHellEncounterHostKernelCoreGameEngineHellEncounterHostOperations _kernelCoreGameEngineHellEncounterHostOperations;
    internal CoreGameEngineHellEncounterHostRuntime(ICoreHellEncounterHostModeRuntimeOperations modeRuntimeOperations, ICoreHellEncounterHostKernelCoreGameEngineHellEncounterHostOperations kernelCoreGameEngineHellEncounterHostOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._kernelCoreGameEngineHellEncounterHostOperations = kernelCoreGameEngineHellEncounterHostOperations;
    }
}
