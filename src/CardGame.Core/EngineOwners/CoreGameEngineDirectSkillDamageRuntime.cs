#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDirectSkillDamageRuntime
{
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    internal CoreGameEngineDirectSkillDamageRuntime(IKernelFrameStoreAccess resolutionStackOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
    }
}
