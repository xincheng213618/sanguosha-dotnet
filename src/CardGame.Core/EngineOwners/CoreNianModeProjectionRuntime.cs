#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNianModeProjectionRuntime
{
    private readonly RuntimeEntries.CoreGameEngineNianChallengeHostEntries _coreNianChallengeHostOperations;
    internal CoreNianModeProjectionRuntime(RuntimeEntries.CoreGameEngineNianChallengeHostEntries coreNianChallengeHostOperations)
    {
        this._coreNianChallengeHostOperations = coreNianChallengeHostOperations;
    }
}
