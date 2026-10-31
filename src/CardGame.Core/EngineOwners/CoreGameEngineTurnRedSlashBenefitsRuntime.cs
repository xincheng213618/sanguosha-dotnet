#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineTurnRedSlashBenefitsRuntime : global::CardGame.Core.ITurnRedSlashProgramHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHanDangMouMobileEntries _characterHanDangMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuPanOlEntries _characterLiuPanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuYanOlEntries _characterLiuYanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuZanMobileEntries _characterLiuZanMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries _characterLuKangWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuZhiMouMobileEntries _characterLuZhiMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaMiDiOlEntries _characterMaMiDiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQuHuangOlEntries _characterQuHuangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterRuanYuOlEntries _characterRuanYuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWenChouMouOlEntries _characterWenChouMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWuJingOlEntries _characterWuJingOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess _turnCardUseEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionDiscardColorsEntries _coreActionDiscardColorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries _coreAdjacentRedTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries _coreAlternatingSuitAndCompletedTopOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineComparedBlackSlashPoliciesEntries _coreComparedBlackSlashPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnShownCardBenefitsEntries _coreCurrentTurnShownCardBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries _coreDamageTargetBudgetOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries _coreDirectedCardPolicyQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries _coreDyingWindowSkillPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalTargetSlashEntries _coreFinalTargetSlashOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries _coreSoleTargetProtectionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceCursesEntries _coreSourceCursesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries _coreTargetedOrdinaryTrickOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeBorrowedSwordEntries _coreNativeBorrowedSwordOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeQinglongEntries _coreNativeQinglongOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineTurnRedSlashBenefitsRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterHanDangMouMobileEntries characterHanDangMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuPanOlEntries characterLiuPanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuYanOlEntries characterLiuYanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuZanMobileEntries characterLiuZanMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries characterLuKangWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterLuZhiMouMobileEntries characterLuZhiMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterMaMiDiOlEntries characterMaMiDiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterQuHuangOlEntries characterQuHuangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterRuanYuOlEntries characterRuanYuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterWenChouMouOlEntries characterWenChouMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterWuJingOlEntries characterWuJingOlOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess turnCardUseEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionDiscardColorsEntries coreActionDiscardColorsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries coreAdjacentRedTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries coreAlternatingSuitAndCompletedTopOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineComparedBlackSlashPoliciesEntries coreComparedBlackSlashPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnShownCardBenefitsEntries coreCurrentTurnShownCardBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries coreDamageTargetBudgetOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries coreDirectedCardPolicyQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries coreDyingWindowSkillPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalTargetSlashEntries coreFinalTargetSlashOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries coreSoleTargetProtectionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceCursesEntries coreSourceCursesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries coreTargetedOrdinaryTrickOperations, global::CardGame.Core.RuntimeEntries.CoreNativeBorrowedSwordEntries coreNativeBorrowedSwordOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeQinglongEntries coreNativeQinglongOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._characterHanDangMouMobileOperations = characterHanDangMouMobileOperations;
        this._characterLiuPanOlOperations = characterLiuPanOlOperations;
        this._characterLiuYanOlOperations = characterLiuYanOlOperations;
        this._characterLiuZanMobileOperations = characterLiuZanMobileOperations;
        this._characterLuKangWumiaoOperations = characterLuKangWumiaoOperations;
        this._characterLuZhiMouMobileOperations = characterLuZhiMouMobileOperations;
        this._characterMaMiDiOlOperations = characterMaMiDiOlOperations;
        this._characterQuHuangOlOperations = characterQuHuangOlOperations;
        this._characterRuanYuOlOperations = characterRuanYuOlOperations;
        this._characterWenChouMouOlOperations = characterWenChouMouOlOperations;
        this._characterWuJingOlOperations = characterWuJingOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreActionDiscardColorsOperations = coreActionDiscardColorsOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreAdjacentRedTargetsOperations = coreAdjacentRedTargetsOperations;
        this._coreAlternatingSuitAndCompletedTopOperations = coreAlternatingSuitAndCompletedTopOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreComparedBlackSlashPoliciesOperations = coreComparedBlackSlashPoliciesOperations;
        this._coreCurrentTurnShownCardBenefitsOperations = coreCurrentTurnShownCardBenefitsOperations;
        this._coreDamageTargetBudgetOperations = coreDamageTargetBudgetOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreDyingWindowSkillPoliciesOperations = coreDyingWindowSkillPoliciesOperations;
        this._coreFinalTargetSlashOperations = coreFinalTargetSlashOperations;
        this._coreSoleTargetProtectionOperations = coreSoleTargetProtectionOperations;
        this._coreSourceCursesOperations = coreSourceCursesOperations;
        this._coreTargetedOrdinaryTrickOperations = coreTargetedOrdinaryTrickOperations;
        this._coreNativeBorrowedSwordOperations = coreNativeBorrowedSwordOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreNativeQinglongOperations = coreNativeQinglongOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
