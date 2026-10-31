#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramDistanceRulesRuntime
{
    private readonly IKernelDistanceRulesAccess _distanceRulesOperations;
    private readonly IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramDistanceRulesRuntime(IKernelDistanceRulesAccess distanceRulesOperations, IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._distanceRulesOperations = distanceRulesOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
