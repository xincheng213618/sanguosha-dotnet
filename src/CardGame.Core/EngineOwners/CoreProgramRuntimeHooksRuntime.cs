#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramRuntimeHooksRuntime
{
    private readonly ICoreProgramRuntimeHooksModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICoreProgramRuntimeHooksRuntimeHooksOperations _runtimeHooksOperations;
    private readonly ICoreProgramRuntimeHooksKernelCoreProgramRuntimeModulesOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramRuntimeHooksRuntime(ICoreProgramRuntimeHooksModeRuntimeOperations modeRuntimeOperations, ICoreProgramRuntimeHooksRuntimeHooksOperations runtimeHooksOperations, ICoreProgramRuntimeHooksKernelCoreProgramRuntimeModulesOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._runtimeHooksOperations = runtimeHooksOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
