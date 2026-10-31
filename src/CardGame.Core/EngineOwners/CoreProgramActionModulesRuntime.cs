#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramActionModulesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramActionsAccess _programActionsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramActionModulesRuntime(global::CardGame.Core.EngineOwners.IKernelProgramActionsAccess programActionsOperations, global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._programActionsOperations = programActionsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
