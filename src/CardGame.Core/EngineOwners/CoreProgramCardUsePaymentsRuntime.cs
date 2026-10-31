#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramCardUsePaymentsRuntime
{
    private readonly ICoreProgramCardUsePaymentsCardUsePaymentsOperations _cardUsePaymentsOperations;
    private readonly ICoreProgramCardUsePaymentsKernelCoreProgramRuntimeModulesOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramCardUsePaymentsRuntime(ICoreProgramCardUsePaymentsCardUsePaymentsOperations cardUsePaymentsOperations, ICoreProgramCardUsePaymentsKernelCoreProgramRuntimeModulesOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._cardUsePaymentsOperations = cardUsePaymentsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
