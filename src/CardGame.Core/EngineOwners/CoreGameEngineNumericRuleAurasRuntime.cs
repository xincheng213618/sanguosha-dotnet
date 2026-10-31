#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNumericRuleAurasRuntime
{
    private readonly ICoreNumericRuleAurasCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreNumericRuleAurasCoreRuleQueriesOperations _coreRuleQueriesOperations;
    private readonly ICoreNumericRuleAurasCoreNativeSkillContextQueriesOperations _coreNativeSkillContextQueriesOperations;
    private readonly ICoreNumericRuleAurasCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineNumericRuleAurasRuntime(ICoreNumericRuleAurasCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreNumericRuleAurasCoreRuleQueriesOperations coreRuleQueriesOperations, ICoreNumericRuleAurasCoreNativeSkillContextQueriesOperations coreNativeSkillContextQueriesOperations, ICoreNumericRuleAurasCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreNativeSkillContextQueriesOperations = coreNativeSkillContextQueriesOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
