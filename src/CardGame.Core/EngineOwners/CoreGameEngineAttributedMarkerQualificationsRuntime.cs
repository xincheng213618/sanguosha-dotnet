#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAttributedMarkerQualificationsRuntime
{
    private readonly ICoreAttributedMarkerQualificationsCoreNativeSkillContextQueriesOperations _coreNativeSkillContextQueriesOperations;
    private readonly ICoreAttributedMarkerQualificationsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    internal CoreGameEngineAttributedMarkerQualificationsRuntime(ICoreAttributedMarkerQualificationsCoreNativeSkillContextQueriesOperations coreNativeSkillContextQueriesOperations, ICoreAttributedMarkerQualificationsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations)
    {
        this._coreNativeSkillContextQueriesOperations = coreNativeSkillContextQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
    }
}
