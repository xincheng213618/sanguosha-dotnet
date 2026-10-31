#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHellEncounterHostRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateHellEncounterHostOperations _kernelCoreGameEngineHellEncounterHostOperations;
    internal CoreGameEngineHellEncounterHostRuntime(global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess modeRuntimeOperations, global::CardGame.Core.EngineOwners.IKernelCreateHellEncounterHostOperations kernelCoreGameEngineHellEncounterHostOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._kernelCoreGameEngineHellEncounterHostOperations = kernelCoreGameEngineHellEncounterHostOperations;
    }
}
