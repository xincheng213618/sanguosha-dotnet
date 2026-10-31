#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramTargetRulesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelTargetRulesAccess _targetRulesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramTargetRulesRuntime(global::CardGame.Core.EngineOwners.IKernelTargetRulesAccess targetRulesOperations, global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._targetRulesOperations = targetRulesOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
