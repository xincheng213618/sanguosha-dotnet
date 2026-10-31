#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramTargetWindowProtocolsRuntime
{
    private readonly IKernelTargetWindowProtocolsAccess _targetWindowProtocolsOperations;
    private readonly IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramTargetWindowProtocolsRuntime(IKernelTargetWindowProtocolsAccess targetWindowProtocolsOperations, IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._targetWindowProtocolsOperations = targetWindowProtocolsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
