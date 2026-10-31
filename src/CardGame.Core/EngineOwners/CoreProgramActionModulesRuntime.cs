#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramActionModulesRuntime
{
    private readonly ICoreProgramActionModulesProgramActionsOperations _programActionsOperations;
    private readonly ICoreProgramActionModulesKernelCoreProgramRuntimeModulesOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramActionModulesRuntime(ICoreProgramActionModulesProgramActionsOperations programActionsOperations, ICoreProgramActionModulesKernelCoreProgramRuntimeModulesOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._programActionsOperations = programActionsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
