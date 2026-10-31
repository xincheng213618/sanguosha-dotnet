#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheS5ShajueRuntime
{
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    internal CoreGameEngineShanheS5ShajueRuntime(IKernelFrameStoreAccess resolutionStackOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
    }
}
