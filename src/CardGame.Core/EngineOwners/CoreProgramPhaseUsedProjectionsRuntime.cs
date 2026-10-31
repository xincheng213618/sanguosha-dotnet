#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramPhaseUsedProjectionsRuntime
{
    private readonly IKernelPhaseUsedProjectionsAccess _phaseUsedProjectionsOperations;
    private readonly IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramPhaseUsedProjectionsRuntime(IKernelPhaseUsedProjectionsAccess phaseUsedProjectionsOperations, IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._phaseUsedProjectionsOperations = phaseUsedProjectionsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
