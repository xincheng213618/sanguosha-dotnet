#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramPhaseUsedProjectionsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelPhaseUsedProjectionsAccess _phaseUsedProjectionsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramPhaseUsedProjectionsRuntime(global::CardGame.Core.EngineOwners.IKernelPhaseUsedProjectionsAccess phaseUsedProjectionsOperations, global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._phaseUsedProjectionsOperations = phaseUsedProjectionsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
