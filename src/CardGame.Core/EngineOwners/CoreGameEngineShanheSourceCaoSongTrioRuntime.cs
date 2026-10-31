#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheSourceCaoSongTrioRuntime
{
    private readonly ICoreShanheSourceCaoSongTrioResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheSourceCaoSongTrioCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreShanheSourceCaoSongTrioCoreTeamActivitiesOperations _coreTeamActivitiesOperations;
    private readonly ICoreShanheSourceCaoSongTrioCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShanheSourceCaoSongTrioCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheSourceCaoSongTrioRuntime(ICoreShanheSourceCaoSongTrioResolutionStackOperations resolutionStackOperations, ICoreShanheSourceCaoSongTrioCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreShanheSourceCaoSongTrioCoreTeamActivitiesOperations coreTeamActivitiesOperations, ICoreShanheSourceCaoSongTrioCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShanheSourceCaoSongTrioCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreTeamActivitiesOperations = coreTeamActivitiesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
