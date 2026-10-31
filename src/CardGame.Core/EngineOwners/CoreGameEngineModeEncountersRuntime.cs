#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeEncountersRuntime
{
    private readonly IKernelCreateModeEncounterHostOperations _kernelCoreGameEngineModeEncountersOperations;
    internal CoreGameEngineModeEncountersRuntime(IKernelCreateModeEncounterHostOperations kernelCoreGameEngineModeEncountersOperations)
    {
        this._kernelCoreGameEngineModeEncountersOperations = kernelCoreGameEngineModeEncountersOperations;
    }
}
