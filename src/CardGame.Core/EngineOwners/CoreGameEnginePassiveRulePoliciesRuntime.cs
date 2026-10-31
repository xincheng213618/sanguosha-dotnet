#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePassiveRulePoliciesRuntime
{
    private readonly ICorePassiveRulePoliciesContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePassiveRulePoliciesCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICorePassiveRulePoliciesCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICorePassiveRulePoliciesCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICorePassiveRulePoliciesCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePassiveRulePoliciesRuntime(ICorePassiveRulePoliciesContentRegistryOperations contentRegistryOperations, ICorePassiveRulePoliciesCoreNationalSkillsOperations coreNationalSkillsOperations, ICorePassiveRulePoliciesCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICorePassiveRulePoliciesCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICorePassiveRulePoliciesCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
