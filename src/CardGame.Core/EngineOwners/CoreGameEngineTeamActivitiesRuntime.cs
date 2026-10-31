#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTeamActivitiesRuntime
{
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly ICoreTeamActivitiesCoreModeAllegianceOperations _coreModeAllegianceOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineTeamActivitiesRuntime(IKernelMatchModeRuntimeAccess modeRuntimeOperations, ICoreTeamActivitiesCoreModeAllegianceOperations coreModeAllegianceOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreModeAllegianceOperations = coreModeAllegianceOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
