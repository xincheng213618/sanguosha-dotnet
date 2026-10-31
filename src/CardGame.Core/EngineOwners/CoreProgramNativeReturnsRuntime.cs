#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramNativeReturnsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelNativeReturnsAccess _nativeReturnsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramNativeReturnsRuntime(global::CardGame.Core.EngineOwners.IKernelNativeReturnsAccess nativeReturnsOperations, global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._nativeReturnsOperations = nativeReturnsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
