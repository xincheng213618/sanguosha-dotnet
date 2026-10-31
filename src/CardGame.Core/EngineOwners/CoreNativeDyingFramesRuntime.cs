#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeDyingFramesRuntime
{
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    internal CoreNativeDyingFramesRuntime(IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
    }
}
