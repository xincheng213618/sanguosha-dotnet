#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeEquipmentAndFactionQueriesRuntime
{
    private readonly RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly RuntimeEntries.CharacterHuCheErMouEntries _characterHuCheErMouOperations;
    private readonly RuntimeEntries.CharacterHuangfuSongWumiaoEntries _characterHuangfuSongWumiaoOperations;
    private readonly RuntimeEntries.CharacterLuBuHappyGodEntries _characterLuBuHappyGodOperations;
    private readonly RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly ICoreNativeEquipmentAndFactionQueriesCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly RuntimeEntries.CoreGameEngineExtendedViewAsEntries _coreExtendedViewAsOperations;
    private readonly RuntimeEntries.CoreGameEngineFieldEquipmentEntries _coreFieldEquipmentOperations;
    private readonly RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries _coreJudgedRankSlashPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries _coreProgramCardPaymentsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramFactionPoliciesEntries _coreProgramFactionPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineRuleQueriesEntries _coreRuleQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly RuntimeEntries.CoreMatchSetupDefinitionsEntries _coreMatchSetupDefinitionsOperations;
    private readonly RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly RuntimeEntries.CoreProgramCardLegalityEntries _coreProgramCardLegalityOperations;
    private readonly ICoreNativeEquipmentAndFactionQueriesCoreProgramDistanceRulesOperations _coreProgramDistanceRulesOperations;
    private readonly RuntimeEntries.CoreProgramPipelineHooksEntries _coreProgramPipelineHooksOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeEquipmentAndFactionQueriesRuntime(RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, RuntimeEntries.CharacterHuCheErMouEntries characterHuCheErMouOperations, RuntimeEntries.CharacterHuangfuSongWumiaoEntries characterHuangfuSongWumiaoOperations, RuntimeEntries.CharacterLuBuHappyGodEntries characterLuBuHappyGodOperations, RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, ICoreNativeEquipmentAndFactionQueriesCoreEquipmentContentOperations coreEquipmentContentOperations, RuntimeEntries.CoreGameEngineExtendedViewAsEntries coreExtendedViewAsOperations, RuntimeEntries.CoreGameEngineFieldEquipmentEntries coreFieldEquipmentOperations, RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries coreJudgedRankSlashPoliciesOperations, RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries coreProgramCardPaymentsOperations, RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, RuntimeEntries.CoreGameEngineProgramFactionPoliciesEntries coreProgramFactionPoliciesOperations, RuntimeEntries.CoreGameEngineRuleQueriesEntries coreRuleQueriesOperations, RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, RuntimeEntries.CoreMatchSetupDefinitionsEntries coreMatchSetupDefinitionsOperations, RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, RuntimeEntries.CoreProgramCardLegalityEntries coreProgramCardLegalityOperations, ICoreNativeEquipmentAndFactionQueriesCoreProgramDistanceRulesOperations coreProgramDistanceRulesOperations, RuntimeEntries.CoreProgramPipelineHooksEntries coreProgramPipelineHooksOperations, EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
