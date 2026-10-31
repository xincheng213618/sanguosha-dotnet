#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramCardUseRulesRuntime
{
    private readonly ICoreProgramCardUseRulesCharacterGuanYuWumiaoOperations _characterGuanYuWumiaoOperations;
    private readonly ICoreProgramCardUseRulesCardZonesOperations _cardZonesOperations;
    private readonly ICoreProgramCardUseRulesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramCardUseRulesTurnCardUseEffectsOperations _turnCardUseEffectsOperations;
    private readonly ICoreProgramCardUseRulesCoreAdvancedWeaponsOperations _coreAdvancedWeaponsOperations;
    private readonly ICoreProgramCardUseRulesCoreCardConversionsOperations _coreCardConversionsOperations;
    private readonly ICoreProgramCardUseRulesCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreProgramCardUseRulesCorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICoreProgramCardUseRulesCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreProgramCardUseRulesCoreRuleQueriesOperations _coreRuleQueriesOperations;
    private readonly ICoreProgramCardUseRulesCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreProgramCardUseRulesCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreProgramCardUseRulesCoreNativeSkillContextQueriesOperations _coreNativeSkillContextQueriesOperations;
    private readonly ICoreProgramCardUseRulesCoreNativeTargetLegalityOperations _coreNativeTargetLegalityOperations;
    private readonly ICoreProgramCardUseRulesCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreProgramCardUseRulesCoreProgramTargetRulesOperations _coreProgramTargetRulesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramCardUseRulesRuntime(ICoreProgramCardUseRulesCharacterGuanYuWumiaoOperations characterGuanYuWumiaoOperations, ICoreProgramCardUseRulesCardZonesOperations cardZonesOperations, ICoreProgramCardUseRulesContentRegistryOperations contentRegistryOperations, ICoreProgramCardUseRulesTurnCardUseEffectsOperations turnCardUseEffectsOperations, ICoreProgramCardUseRulesCoreAdvancedWeaponsOperations coreAdvancedWeaponsOperations, ICoreProgramCardUseRulesCoreCardConversionsOperations coreCardConversionsOperations, ICoreProgramCardUseRulesCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreProgramCardUseRulesCorePlayCardHistoryOperations corePlayCardHistoryOperations, ICoreProgramCardUseRulesCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreProgramCardUseRulesCoreRuleQueriesOperations coreRuleQueriesOperations, ICoreProgramCardUseRulesCoreTableOrderOperations coreTableOrderOperations, ICoreProgramCardUseRulesCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreProgramCardUseRulesCoreNativeSkillContextQueriesOperations coreNativeSkillContextQueriesOperations, ICoreProgramCardUseRulesCoreNativeTargetLegalityOperations coreNativeTargetLegalityOperations, ICoreProgramCardUseRulesCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreProgramCardUseRulesCoreProgramTargetRulesOperations coreProgramTargetRulesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeSkillContextQueriesOperations = coreNativeSkillContextQueriesOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramTargetRulesOperations = coreProgramTargetRulesOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
