#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramCardUsePaymentsRuntime
{
    private readonly IKernelCardUsePaymentsAccess _cardUsePaymentsOperations;
    private readonly IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramCardUsePaymentsRuntime(IKernelCardUsePaymentsAccess cardUsePaymentsOperations, IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._cardUsePaymentsOperations = cardUsePaymentsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
