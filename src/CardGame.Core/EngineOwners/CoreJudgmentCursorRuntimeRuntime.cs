#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreJudgmentCursorRuntimeRuntime
{
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    internal CoreJudgmentCursorRuntimeRuntime(IKernelFrameStoreAccess resolutionStackOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
    }
}
