#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProvenanceClaimObserversRuntime
{
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    internal CoreGameEngineProvenanceClaimObserversRuntime(IKernelFrameStoreAccess resolutionStackOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
    }
}
