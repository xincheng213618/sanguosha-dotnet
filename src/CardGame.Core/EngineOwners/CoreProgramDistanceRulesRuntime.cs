#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramDistanceRulesRuntime
{
    private readonly ICoreProgramDistanceRulesDistanceRulesOperations _distanceRulesOperations;
    private readonly ICoreProgramDistanceRulesKernelCoreProgramRuntimeModulesOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramDistanceRulesRuntime(ICoreProgramDistanceRulesDistanceRulesOperations distanceRulesOperations, ICoreProgramDistanceRulesKernelCoreProgramRuntimeModulesOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._distanceRulesOperations = distanceRulesOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
