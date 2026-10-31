#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramNativeReturnsRuntime
{
    private readonly ICoreProgramNativeReturnsNativeReturnsOperations _nativeReturnsOperations;
    private readonly ICoreProgramNativeReturnsKernelCoreProgramRuntimeModulesOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramNativeReturnsRuntime(ICoreProgramNativeReturnsNativeReturnsOperations nativeReturnsOperations, ICoreProgramNativeReturnsKernelCoreProgramRuntimeModulesOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._nativeReturnsOperations = nativeReturnsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
