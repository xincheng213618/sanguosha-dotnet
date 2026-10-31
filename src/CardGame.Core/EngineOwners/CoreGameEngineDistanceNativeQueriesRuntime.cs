#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDistanceNativeQueriesRuntime
{
    private readonly ICoreDistanceNativeQueriesCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    internal CoreGameEngineDistanceNativeQueriesRuntime(ICoreDistanceNativeQueriesCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations)
    {
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
    }
}
