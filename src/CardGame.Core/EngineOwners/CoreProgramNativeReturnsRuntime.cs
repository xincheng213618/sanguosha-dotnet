#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramNativeReturnsRuntime
{
    private readonly IKernelNativeReturnsAccess _nativeReturnsOperations;
    private readonly IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramNativeReturnsRuntime(IKernelNativeReturnsAccess nativeReturnsOperations, IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._nativeReturnsOperations = nativeReturnsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
