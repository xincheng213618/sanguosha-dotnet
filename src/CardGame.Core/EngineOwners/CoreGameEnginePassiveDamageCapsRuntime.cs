#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePassiveDamageCapsRuntime
{
    private readonly ICorePassiveDamageCapsCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICorePassiveDamageCapsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICorePassiveDamageCapsCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICorePassiveDamageCapsCoreTeamActivitiesOperations _coreTeamActivitiesOperations;
    private readonly ICorePassiveDamageCapsCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly ICorePassiveDamageCapsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePassiveDamageCapsRuntime(ICorePassiveDamageCapsCoreCardAttackStateOperations coreCardAttackStateOperations, ICorePassiveDamageCapsCoreNationalSkillsOperations coreNationalSkillsOperations, ICorePassiveDamageCapsCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICorePassiveDamageCapsCoreTeamActivitiesOperations coreTeamActivitiesOperations, ICorePassiveDamageCapsCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, ICorePassiveDamageCapsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreTeamActivitiesOperations = coreTeamActivitiesOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
