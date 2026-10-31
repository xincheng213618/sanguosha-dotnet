#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePublicDamagePredictionRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChengPuBoundaryMobileEntries _characterChengPuBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangZhongBoundaryMobileEntries _characterHuangZhongBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterPanZhangMaZhongBoundaryMobileEntries _characterPanZhangMaZhongBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQuYiClassicEntries _characterQuYiClassicOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFeiBoundaryMobileEntries _characterZhangFeiBoundaryMobileOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess _turnCardUseEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectSkillDamageEntries _coreDirectSkillDamageOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDuanhunEntries _coreDuanhunOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHeartSlashDamageEntries _coreHeartSlashDamageOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries _coreLordSkillProjectionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDamageArmorPoliciesEntries _coreNativeDamageArmorPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveDamageCapsEntries _corePassiveDamageCapsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries _coreProgramDodgeResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheBadaoEntries _coreShanheBadaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheLeizhenEntries _coreShanheLeizhenOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5QuyiEntries _coreShanheS5QuyiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5ShuangxiongEntries _coreShanheS5ShuangxiongOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolLateEntries _coreShanheSchoolLateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolsEntries _coreShanheSchoolsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceShennuEntries _coreShanheSourceShennuOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheWoxinEntries _coreShanheWoxinOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashUseOrdinalsEntries _coreSlashUseOrdinalsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangAuditCombatEntries _coreWushuangAuditCombatOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangPublicPredictionEntries _coreWushuangPublicPredictionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries _coreNativeCardClassificationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly ICorePublicDamagePredictionCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries _coreNativeSkillContextQueriesOperations;
    private readonly ICorePublicDamagePredictionCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreShenwuNpcProgramsEntries _coreShenwuNpcProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly ICorePublicDamagePredictionCoreWushuangModeProjectionOperations _coreWushuangModeProjectionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePublicDamagePredictionRuntime(global::CardGame.Core.RuntimeEntries.CharacterChengPuBoundaryMobileEntries characterChengPuBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangZhongBoundaryMobileEntries characterHuangZhongBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterPanZhangMaZhongBoundaryMobileEntries characterPanZhangMaZhongBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterQuYiClassicEntries characterQuYiClassicOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangFeiBoundaryMobileEntries characterZhangFeiBoundaryMobileOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess turnCardUseEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectSkillDamageEntries coreDirectSkillDamageOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDuanhunEntries coreDuanhunOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHeartSlashDamageEntries coreHeartSlashDamageOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries coreLordSkillProjectionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDamageArmorPoliciesEntries coreNativeDamageArmorPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveDamageCapsEntries corePassiveDamageCapsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries coreProgramDodgeResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheBadaoEntries coreShanheBadaoOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheLeizhenEntries coreShanheLeizhenOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5QuyiEntries coreShanheS5QuyiOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5ShuangxiongEntries coreShanheS5ShuangxiongOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolLateEntries coreShanheSchoolLateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolsEntries coreShanheSchoolsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceShennuEntries coreShanheSourceShennuOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheWoxinEntries coreShanheWoxinOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashUseOrdinalsEntries coreSlashUseOrdinalsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangAuditCombatEntries coreWushuangAuditCombatOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangPublicPredictionEntries coreWushuangPublicPredictionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries coreNativeCardClassificationsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, ICorePublicDamagePredictionCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries coreNativeSkillContextQueriesOperations, ICorePublicDamagePredictionCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, global::CardGame.Core.RuntimeEntries.CoreShenwuNpcProgramsEntries coreShenwuNpcProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, ICorePublicDamagePredictionCoreWushuangModeProjectionOperations coreWushuangModeProjectionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterChengPuBoundaryMobileOperations = characterChengPuBoundaryMobileOperations;
        this._characterHuangZhongBoundaryMobileOperations = characterHuangZhongBoundaryMobileOperations;
        this._characterPanZhangMaZhongBoundaryMobileOperations = characterPanZhangMaZhongBoundaryMobileOperations;
        this._characterQuYiClassicOperations = characterQuYiClassicOperations;
        this._characterZhangFeiBoundaryMobileOperations = characterZhangFeiBoundaryMobileOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreDirectSkillDamageOperations = coreDirectSkillDamageOperations;
        this._coreDuanhunOperations = coreDuanhunOperations;
        this._coreHeartSlashDamageOperations = coreHeartSlashDamageOperations;
        this._coreLordSkillProjectionsOperations = coreLordSkillProjectionsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativeDamageArmorPoliciesOperations = coreNativeDamageArmorPoliciesOperations;
        this._corePassiveDamageCapsOperations = corePassiveDamageCapsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreShanheBadaoOperations = coreShanheBadaoOperations;
        this._coreShanheLeizhenOperations = coreShanheLeizhenOperations;
        this._coreShanheS5QuyiOperations = coreShanheS5QuyiOperations;
        this._coreShanheS5ShuangxiongOperations = coreShanheS5ShuangxiongOperations;
        this._coreShanheSchoolLateOperations = coreShanheSchoolLateOperations;
        this._coreShanheSchoolsOperations = coreShanheSchoolsOperations;
        this._coreShanheSourceShennuOperations = coreShanheSourceShennuOperations;
        this._coreShanheWoxinOperations = coreShanheWoxinOperations;
        this._coreSlashUseOrdinalsOperations = coreSlashUseOrdinalsOperations;
        this._coreWushuangAuditCombatOperations = coreWushuangAuditCombatOperations;
        this._coreWushuangPublicPredictionOperations = coreWushuangPublicPredictionOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._coreNativeSkillContextQueriesOperations = coreNativeSkillContextQueriesOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreShenwuNpcProgramsOperations = coreShenwuNpcProgramsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreWushuangModeProjectionOperations = coreWushuangModeProjectionOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
