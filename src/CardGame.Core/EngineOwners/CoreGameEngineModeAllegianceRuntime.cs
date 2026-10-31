#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeAllegianceRuntime
{
    private readonly IKernelAllegiancePolicyAccess _allegiancePolicyOperations;
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    internal CoreGameEngineModeAllegianceRuntime(IKernelAllegiancePolicyAccess allegiancePolicyOperations, IKernelMatchModeRuntimeAccess modeRuntimeOperations)
    {
        this._allegiancePolicyOperations = allegiancePolicyOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
    }
}
