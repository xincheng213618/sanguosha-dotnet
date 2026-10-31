#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramTargetRulesRuntime
{
    private readonly ICoreProgramTargetRulesTargetRulesOperations _targetRulesOperations;
    private readonly ICoreProgramTargetRulesKernelCoreProgramRuntimeModulesOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramTargetRulesRuntime(ICoreProgramTargetRulesTargetRulesOperations targetRulesOperations, ICoreProgramTargetRulesKernelCoreProgramRuntimeModulesOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._targetRulesOperations = targetRulesOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
