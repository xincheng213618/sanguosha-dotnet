#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeDamagePoliciesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCheZhouAnniversaryEntries _characterCheZhouAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDingYuanOlEntries _characterDingYuanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuMengMouEntries _characterLuMengMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries _characterMaChaoAnniversaryGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess _turnCardUseEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectSkillDamageEntries _coreDirectSkillDamageOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries _coreDyingWindowSkillPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExtendedViewAsEntries _coreExtendedViewAsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalTargetSlashEntries _coreFinalTargetSlashOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHeartSlashDamageEntries _coreHeartSlashDamageOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveDamageCapsEntries _corePassiveDamageCapsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveRulePoliciesEntries _corePassiveRulePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries _coreProgramDodgeResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineQiaoquHaoduoEntries _coreQiaoquHaoduoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheBadaoEntries _coreShanheBadaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5QuyiEntries _coreShanheS5QuyiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolLateEntries _coreShanheSchoolLateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolsEntries _coreShanheSchoolsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShenwuEquipmentEntries _coreShenwuEquipmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashUseOrdinalsEntries _coreSlashUseOrdinalsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries _coreWholeActualTurnUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangCombatTacticsEntries _coreWushuangCombatTacticsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries _coreNativeCardClassificationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly ICoreNativeDamagePoliciesCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreNativeDamagePoliciesCoreProgramDamageHooksOperations _coreProgramDamageHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTeamActivityProgramsEntries _coreTeamActivityProgramsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeDamagePoliciesRuntime(global::CardGame.Core.RuntimeEntries.CharacterCheZhouAnniversaryEntries characterCheZhouAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterDingYuanOlEntries characterDingYuanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLuMengMouEntries characterLuMengMouOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries characterMaChaoAnniversaryGodOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess modeRuntimeOperations, global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess turnCardUseEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectSkillDamageEntries coreDirectSkillDamageOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries coreDyingWindowSkillPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineExtendedViewAsEntries coreExtendedViewAsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalTargetSlashEntries coreFinalTargetSlashOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHeartSlashDamageEntries coreHeartSlashDamageOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveDamageCapsEntries corePassiveDamageCapsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveRulePoliciesEntries corePassiveRulePoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries coreProgramDodgeResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineQiaoquHaoduoEntries coreQiaoquHaoduoOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheBadaoEntries coreShanheBadaoOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5QuyiEntries coreShanheS5QuyiOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolLateEntries coreShanheSchoolLateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolsEntries coreShanheSchoolsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShenwuEquipmentEntries coreShenwuEquipmentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashUseOrdinalsEntries coreSlashUseOrdinalsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries coreWholeActualTurnUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangCombatTacticsEntries coreWushuangCombatTacticsOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries coreNativeCardClassificationsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, ICoreNativeDamagePoliciesCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, ICoreNativeDamagePoliciesCoreProgramDamageHooksOperations coreProgramDamageHooksOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreTeamActivityProgramsEntries coreTeamActivityProgramsOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCheZhouAnniversaryOperations = characterCheZhouAnniversaryOperations;
        this._characterDingYuanOlOperations = characterDingYuanOlOperations;
        this._characterLuMengMouOperations = characterLuMengMouOperations;
        this._characterMaChaoAnniversaryGodOperations = characterMaChaoAnniversaryGodOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreDirectSkillDamageOperations = coreDirectSkillDamageOperations;
        this._coreDyingWindowSkillPoliciesOperations = coreDyingWindowSkillPoliciesOperations;
        this._coreExtendedViewAsOperations = coreExtendedViewAsOperations;
        this._coreFinalTargetSlashOperations = coreFinalTargetSlashOperations;
        this._coreHeartSlashDamageOperations = coreHeartSlashDamageOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._corePassiveDamageCapsOperations = corePassiveDamageCapsOperations;
        this._corePassiveRulePoliciesOperations = corePassiveRulePoliciesOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreQiaoquHaoduoOperations = coreQiaoquHaoduoOperations;
        this._coreShanheBadaoOperations = coreShanheBadaoOperations;
        this._coreShanheS5QuyiOperations = coreShanheS5QuyiOperations;
        this._coreShanheSchoolLateOperations = coreShanheSchoolLateOperations;
        this._coreShanheSchoolsOperations = coreShanheSchoolsOperations;
        this._coreShenwuEquipmentOperations = coreShenwuEquipmentOperations;
        this._coreSlashUseOrdinalsOperations = coreSlashUseOrdinalsOperations;
        this._coreWholeActualTurnUsesOperations = coreWholeActualTurnUsesOperations;
        this._coreWushuangCombatTacticsOperations = coreWushuangCombatTacticsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramDamageHooksOperations = coreProgramDamageHooksOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
