#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeEquipmentAndFactionQueriesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries _characterHuCheErMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangfuSongWumiaoEntries _characterHuangfuSongWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuBuHappyGodEntries _characterLuBuHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly ICoreNativeEquipmentAndFactionQueriesCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExtendedViewAsEntries _coreExtendedViewAsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFieldEquipmentEntries _coreFieldEquipmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries _coreJudgedRankSlashPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries _coreProgramCardPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramFactionPoliciesEntries _coreProgramFactionPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries _coreRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries _coreMatchSetupDefinitionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramCardLegalityEntries _coreProgramCardLegalityOperations;
    private readonly ICoreNativeEquipmentAndFactionQueriesCoreProgramDistanceRulesOperations _coreProgramDistanceRulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramPipelineHooksEntries _coreProgramPipelineHooksOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeEquipmentAndFactionQueriesRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries characterHuCheErMouOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangfuSongWumiaoEntries characterHuangfuSongWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterLuBuHappyGodEntries characterLuBuHappyGodOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, ICoreNativeEquipmentAndFactionQueriesCoreEquipmentContentOperations coreEquipmentContentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineExtendedViewAsEntries coreExtendedViewAsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFieldEquipmentEntries coreFieldEquipmentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries coreJudgedRankSlashPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries coreProgramCardPaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramFactionPoliciesEntries coreProgramFactionPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries coreRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries coreMatchSetupDefinitionsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreProgramCardLegalityEntries coreProgramCardLegalityOperations, ICoreNativeEquipmentAndFactionQueriesCoreProgramDistanceRulesOperations coreProgramDistanceRulesOperations, global::CardGame.Core.RuntimeEntries.CoreProgramPipelineHooksEntries coreProgramPipelineHooksOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._characterHuangfuSongWumiaoOperations = characterHuangfuSongWumiaoOperations;
        this._characterLuBuHappyGodOperations = characterLuBuHappyGodOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreExtendedViewAsOperations = coreExtendedViewAsOperations;
        this._coreFieldEquipmentOperations = coreFieldEquipmentOperations;
        this._coreJudgedRankSlashPoliciesOperations = coreJudgedRankSlashPoliciesOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramFactionPoliciesOperations = coreProgramFactionPoliciesOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramCardLegalityOperations = coreProgramCardLegalityOperations;
        this._coreProgramDistanceRulesOperations = coreProgramDistanceRulesOperations;
        this._coreProgramPipelineHooksOperations = coreProgramPipelineHooksOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
