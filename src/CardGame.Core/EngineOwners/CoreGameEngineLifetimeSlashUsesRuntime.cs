#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineLifetimeSlashUsesRuntime
{
    private readonly ICoreLifetimeSlashUsesCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreLifetimeSlashUsesCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    internal CoreGameEngineLifetimeSlashUsesRuntime(ICoreLifetimeSlashUsesCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreLifetimeSlashUsesCoreTableSeatQueriesOperations coreTableSeatQueriesOperations)
    {
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
    }
}
