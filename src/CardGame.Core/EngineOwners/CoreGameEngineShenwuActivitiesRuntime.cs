#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShenwuActivitiesRuntime
{
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    internal CoreGameEngineShenwuActivitiesRuntime(IKernelMatchModeRuntimeAccess modeRuntimeOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
    }
}
