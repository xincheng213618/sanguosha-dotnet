#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramTargetRulesRuntime
{
    private readonly IKernelTargetRulesAccess _targetRulesOperations;
    private readonly IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramTargetRulesRuntime(IKernelTargetRulesAccess targetRulesOperations, IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._targetRulesOperations = targetRulesOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
