#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAttributedMarkerQualificationsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries _coreNativeSkillContextQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    internal CoreGameEngineAttributedMarkerQualificationsRuntime(global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries coreNativeSkillContextQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations)
    {
        this._coreNativeSkillContextQueriesOperations = coreNativeSkillContextQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
    }
}
