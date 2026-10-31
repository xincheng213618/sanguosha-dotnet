#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHellEncounterHostRuntime
{
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly IKernelCreateHellEncounterHostOperations _kernelCoreGameEngineHellEncounterHostOperations;
    internal CoreGameEngineHellEncounterHostRuntime(IKernelMatchModeRuntimeAccess modeRuntimeOperations, IKernelCreateHellEncounterHostOperations kernelCoreGameEngineHellEncounterHostOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._kernelCoreGameEngineHellEncounterHostOperations = kernelCoreGameEngineHellEncounterHostOperations;
    }
}
