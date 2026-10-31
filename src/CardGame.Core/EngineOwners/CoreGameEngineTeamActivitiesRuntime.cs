#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTeamActivitiesRuntime
{
    private readonly ICoreTeamActivitiesModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICoreTeamActivitiesCoreModeAllegianceOperations _coreModeAllegianceOperations;
    private readonly ICoreTeamActivitiesCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineTeamActivitiesRuntime(ICoreTeamActivitiesModeRuntimeOperations modeRuntimeOperations, ICoreTeamActivitiesCoreModeAllegianceOperations coreModeAllegianceOperations, ICoreTeamActivitiesCoreNationalSkillsOperations coreNationalSkillsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreModeAllegianceOperations = coreModeAllegianceOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
