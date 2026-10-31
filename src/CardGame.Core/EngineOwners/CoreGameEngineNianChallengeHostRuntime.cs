#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNianChallengeHostRuntime
{
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly IKernelCreateNianChallengeHostOperations _kernelCoreGameEngineNianChallengeHostOperations;
    internal CoreGameEngineNianChallengeHostRuntime(IKernelMatchModeRuntimeAccess modeRuntimeOperations, IKernelCreateNianChallengeHostOperations kernelCoreGameEngineNianChallengeHostOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._kernelCoreGameEngineNianChallengeHostOperations = kernelCoreGameEngineNianChallengeHostOperations;
    }
}
