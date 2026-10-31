#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDistanceNativeQueriesRuntime
{
    private readonly ICoreDistanceNativeQueriesCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    internal CoreGameEngineDistanceNativeQueriesRuntime(ICoreDistanceNativeQueriesCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations)
    {
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
    }
}
