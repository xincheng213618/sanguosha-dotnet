#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramRuntimeHooksRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelRuntimeHooksAccess _runtimeHooksOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramRuntimeHooksRuntime(global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess modeRuntimeOperations, global::CardGame.Core.EngineOwners.IKernelRuntimeHooksAccess runtimeHooksOperations, global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._runtimeHooksOperations = runtimeHooksOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
