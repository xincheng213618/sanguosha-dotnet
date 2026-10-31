#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramMovementSourceCountsRuntime
{
    private readonly RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries _coreProgramDiscardOriginsOperations;
    internal CoreGameEngineProgramMovementSourceCountsRuntime(RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries coreProgramDiscardOriginsOperations)
    {
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
    }
}
