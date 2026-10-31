#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDuanhunRuntime
{
    private readonly ICoreDuanhunCharacterHuCheErMouOperations _characterHuCheErMouOperations;
    private readonly ICoreDuanhunCardZonesOperations _cardZonesOperations;
    private readonly ICoreDuanhunCoreEquipmentSkillsOperations _coreEquipmentSkillsOperations;
    private readonly ICoreDuanhunCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreDuanhunCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreDuanhunCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDuanhunRuntime(ICoreDuanhunCharacterHuCheErMouOperations characterHuCheErMouOperations, ICoreDuanhunCardZonesOperations cardZonesOperations, ICoreDuanhunCoreEquipmentSkillsOperations coreEquipmentSkillsOperations, ICoreDuanhunCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreDuanhunCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreDuanhunCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
