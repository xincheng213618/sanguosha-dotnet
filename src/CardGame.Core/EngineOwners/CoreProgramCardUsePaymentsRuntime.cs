#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramCardUsePaymentsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCardUsePaymentsAccess _cardUsePaymentsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramCardUsePaymentsRuntime(global::CardGame.Core.EngineOwners.IKernelCardUsePaymentsAccess cardUsePaymentsOperations, global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._cardUsePaymentsOperations = cardUsePaymentsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
