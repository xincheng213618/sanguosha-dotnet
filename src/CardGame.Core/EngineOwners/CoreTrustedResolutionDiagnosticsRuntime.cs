#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreTrustedResolutionDiagnosticsRuntime
{
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    internal CoreTrustedResolutionDiagnosticsRuntime(IKernelFrameStoreAccess resolutionStackOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
    }
}
