#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterDianWeiMobileRuntime : global::CardGame.Core.IMobileOrdinaryQiangxiQueries
{
    private readonly ICharacterDianWeiMobileCharacterSunYuanOlOperations _characterSunYuanOlOperations;
    private readonly ICharacterDianWeiMobileCharacterTaishiCiMobileGodOperations _characterTaishiCiMobileGodOperations;
    private readonly ICharacterDianWeiMobileCardZonesOperations _cardZonesOperations;
    private readonly ICharacterDianWeiMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterDianWeiMobileCoreActionQueriesOperations _coreActionQueriesOperations;
    private readonly ICharacterDianWeiMobileCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICharacterDianWeiMobileCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly ICharacterDianWeiMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterDianWeiMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterDianWeiMobileCoreProgramPersistentCombatPoliciesOperations _coreProgramPersistentCombatPoliciesOperations;
    private readonly ICharacterDianWeiMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterDianWeiMobileCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    internal CharacterDianWeiMobileRuntime(ICharacterDianWeiMobileCharacterSunYuanOlOperations characterSunYuanOlOperations, ICharacterDianWeiMobileCharacterTaishiCiMobileGodOperations characterTaishiCiMobileGodOperations, ICharacterDianWeiMobileCardZonesOperations cardZonesOperations, ICharacterDianWeiMobileResolutionStackOperations resolutionStackOperations, ICharacterDianWeiMobileCoreActionQueriesOperations coreActionQueriesOperations, ICharacterDianWeiMobileCoreCardAttackStateOperations coreCardAttackStateOperations, ICharacterDianWeiMobileCoreEquipmentContentOperations coreEquipmentContentOperations, ICharacterDianWeiMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterDianWeiMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterDianWeiMobileCoreProgramPersistentCombatPoliciesOperations coreProgramPersistentCombatPoliciesOperations, ICharacterDianWeiMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterDianWeiMobileCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess)
    {
        this._characterSunYuanOlOperations = characterSunYuanOlOperations;
        this._characterTaishiCiMobileGodOperations = characterTaishiCiMobileGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramPersistentCombatPoliciesOperations = coreProgramPersistentCombatPoliciesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    }
}
