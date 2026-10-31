#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CorePlayerViewCardPaymentsRuntime
{
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    internal CorePlayerViewCardPaymentsRuntime(IKernelFrameStoreAccess resolutionStackOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
    }
}
