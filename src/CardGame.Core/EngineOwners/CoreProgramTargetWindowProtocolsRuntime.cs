#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramTargetWindowProtocolsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelTargetWindowProtocolsAccess _targetWindowProtocolsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramTargetWindowProtocolsRuntime(global::CardGame.Core.EngineOwners.IKernelTargetWindowProtocolsAccess targetWindowProtocolsOperations, global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._targetWindowProtocolsOperations = targetWindowProtocolsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
