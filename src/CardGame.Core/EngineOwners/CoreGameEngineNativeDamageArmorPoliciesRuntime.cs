#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeDamageArmorPoliciesRuntime
{
    private readonly ICoreNativeDamageArmorPoliciesCoreDuanhunOperations _coreDuanhunOperations;
    private readonly ICoreNativeDamageArmorPoliciesCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreNativeDamageArmorPoliciesCoreWushuangCombatTacticsOperations _coreWushuangCombatTacticsOperations;
    private readonly ICoreNativeDamageArmorPoliciesCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineNativeDamageArmorPoliciesRuntime(ICoreNativeDamageArmorPoliciesCoreDuanhunOperations coreDuanhunOperations, ICoreNativeDamageArmorPoliciesCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreNativeDamageArmorPoliciesCoreWushuangCombatTacticsOperations coreWushuangCombatTacticsOperations, ICoreNativeDamageArmorPoliciesCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreDuanhunOperations = coreDuanhunOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreWushuangCombatTacticsOperations = coreWushuangCombatTacticsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
