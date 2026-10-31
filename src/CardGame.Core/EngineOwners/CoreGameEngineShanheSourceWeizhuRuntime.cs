#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheSourceWeizhuRuntime
{
    private readonly ICoreShanheSourceWeizhuResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheSourceWeizhuCoreTurnHandCategoryRestrictionsOperations _coreTurnHandCategoryRestrictionsOperations;
    private readonly ICoreShanheSourceWeizhuCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShanheSourceWeizhuCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheSourceWeizhuRuntime(ICoreShanheSourceWeizhuResolutionStackOperations resolutionStackOperations, ICoreShanheSourceWeizhuCoreTurnHandCategoryRestrictionsOperations coreTurnHandCategoryRestrictionsOperations, ICoreShanheSourceWeizhuCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShanheSourceWeizhuCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
