#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CorePlayerViewProjectionRuntime
{
    private readonly RuntimeEntries.CoreGameEnginePlayerViewProjectionQueriesEntries _corePlayerViewProjectionQueriesOperations;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    internal CorePlayerViewProjectionRuntime(RuntimeEntries.CoreGameEnginePlayerViewProjectionQueriesEntries corePlayerViewProjectionQueriesOperations, EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess)
    {
        this._corePlayerViewProjectionQueriesOperations = corePlayerViewProjectionQueriesOperations;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
    }
}
