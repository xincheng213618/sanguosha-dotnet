#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramTargetWindowProtocolsRuntime
{
    private readonly ICoreProgramTargetWindowProtocolsTargetWindowProtocolsOperations _targetWindowProtocolsOperations;
    private readonly ICoreProgramTargetWindowProtocolsKernelCoreProgramRuntimeModulesOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramTargetWindowProtocolsRuntime(ICoreProgramTargetWindowProtocolsTargetWindowProtocolsOperations targetWindowProtocolsOperations, ICoreProgramTargetWindowProtocolsKernelCoreProgramRuntimeModulesOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._targetWindowProtocolsOperations = targetWindowProtocolsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
