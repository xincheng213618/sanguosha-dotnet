#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheSourceCaoSongTrioRuntime
{
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineTeamActivitiesEntries _coreTeamActivitiesOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly RuntimeEntries.CoreTeamActivityProgramsEntries _coreTeamActivityProgramsOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheSourceCaoSongTrioRuntime(IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, RuntimeEntries.CoreGameEngineTeamActivitiesEntries coreTeamActivitiesOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, RuntimeEntries.CoreTeamActivityProgramsEntries coreTeamActivityProgramsOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreTeamActivitiesOperations = coreTeamActivitiesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
