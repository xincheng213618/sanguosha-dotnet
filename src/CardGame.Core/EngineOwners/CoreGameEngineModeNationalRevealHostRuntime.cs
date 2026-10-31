#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeNationalRevealHostRuntime
{
    private readonly IKernelCreateModeNationalRevealHostOperations _kernelCoreGameEngineModeNationalRevealHostOperations;
    internal CoreGameEngineModeNationalRevealHostRuntime(IKernelCreateModeNationalRevealHostOperations kernelCoreGameEngineModeNationalRevealHostOperations)
    {
        this._kernelCoreGameEngineModeNationalRevealHostOperations = kernelCoreGameEngineModeNationalRevealHostOperations;
    }
}
