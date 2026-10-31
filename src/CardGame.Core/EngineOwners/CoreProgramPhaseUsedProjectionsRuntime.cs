#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramPhaseUsedProjectionsRuntime
{
    private readonly ICoreProgramPhaseUsedProjectionsPhaseUsedProjectionsOperations _phaseUsedProjectionsOperations;
    private readonly ICoreProgramPhaseUsedProjectionsKernelCoreProgramRuntimeModulesOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramPhaseUsedProjectionsRuntime(ICoreProgramPhaseUsedProjectionsPhaseUsedProjectionsOperations phaseUsedProjectionsOperations, ICoreProgramPhaseUsedProjectionsKernelCoreProgramRuntimeModulesOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._phaseUsedProjectionsOperations = phaseUsedProjectionsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
