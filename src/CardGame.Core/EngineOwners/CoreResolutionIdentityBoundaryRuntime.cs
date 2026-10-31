#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreResolutionIdentityBoundaryRuntime
{
    private readonly IKernelResolutionSchedulerAccess _resolutionSchedulerOperations;
    internal CoreResolutionIdentityBoundaryRuntime(IKernelResolutionSchedulerAccess resolutionSchedulerOperations)
    {
        this._resolutionSchedulerOperations = resolutionSchedulerOperations;
    }
}
