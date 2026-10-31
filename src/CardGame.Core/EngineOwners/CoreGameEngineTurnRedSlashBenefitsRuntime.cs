#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEngineTurnRedSlashBenefitsRuntime : ITurnRedSlashProgramHost
{
    private readonly RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly RuntimeEntries.CharacterHanDangMouMobileEntries _characterHanDangMouMobileOperations;
    private readonly RuntimeEntries.CharacterLiuPanOlEntries _characterLiuPanOlOperations;
    private readonly RuntimeEntries.CharacterLiuYanOlEntries _characterLiuYanOlOperations;
    private readonly RuntimeEntries.CharacterLiuZanMobileEntries _characterLiuZanMobileOperations;
    private readonly RuntimeEntries.CharacterLuKangWumiaoEntries _characterLuKangWumiaoOperations;
    private readonly RuntimeEntries.CharacterLuZhiMouMobileEntries _characterLuZhiMouMobileOperations;
    private readonly RuntimeEntries.CharacterMaMiDiOlEntries _characterMaMiDiOlOperations;
    private readonly RuntimeEntries.CharacterQuHuangOlEntries _characterQuHuangOlOperations;
    private readonly RuntimeEntries.CharacterRuanYuOlEntries _characterRuanYuOlOperations;
    private readonly RuntimeEntries.CharacterWenChouMouOlEntries _characterWenChouMouOlOperations;
    private readonly RuntimeEntries.CharacterWuJingOlEntries _characterWuJingOlOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelTurnCardUseEffectsAccess _turnCardUseEffectsOperations;
    private readonly RuntimeEntries.CoreGameEngineActionDiscardColorsEntries _coreActionDiscardColorsOperations;
    private readonly RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries _coreAdjacentRedTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries _coreAlternatingSuitAndCompletedTopOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly RuntimeEntries.CoreGameEngineComparedBlackSlashPoliciesEntries _coreComparedBlackSlashPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineCurrentTurnShownCardBenefitsEntries _coreCurrentTurnShownCardBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries _coreDamageTargetBudgetOperations;
    private readonly RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries _coreDirectedCardPolicyQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries _coreDyingWindowSkillPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineFinalTargetSlashEntries _coreFinalTargetSlashOperations;
    private readonly RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries _coreSoleTargetProtectionOperations;
    private readonly RuntimeEntries.CoreGameEngineSourceCursesEntries _coreSourceCursesOperations;
    private readonly RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries _coreTargetedOrdinaryTrickOperations;
    private readonly RuntimeEntries.CoreNativeBorrowedSwordEntries _coreNativeBorrowedSwordOperations;
    private readonly RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly RuntimeEntries.CoreNativeQinglongEntries _coreNativeQinglongOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineTurnRedSlashBenefitsRuntime(RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, RuntimeEntries.CharacterHanDangMouMobileEntries characterHanDangMouMobileOperations, RuntimeEntries.CharacterLiuPanOlEntries characterLiuPanOlOperations, RuntimeEntries.CharacterLiuYanOlEntries characterLiuYanOlOperations, RuntimeEntries.CharacterLiuZanMobileEntries characterLiuZanMobileOperations, RuntimeEntries.CharacterLuKangWumiaoEntries characterLuKangWumiaoOperations, RuntimeEntries.CharacterLuZhiMouMobileEntries characterLuZhiMouMobileOperations, RuntimeEntries.CharacterMaMiDiOlEntries characterMaMiDiOlOperations, RuntimeEntries.CharacterQuHuangOlEntries characterQuHuangOlOperations, RuntimeEntries.CharacterRuanYuOlEntries characterRuanYuOlOperations, RuntimeEntries.CharacterWenChouMouOlEntries characterWenChouMouOlOperations, RuntimeEntries.CharacterWuJingOlEntries characterWuJingOlOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelTurnCardUseEffectsAccess turnCardUseEffectsOperations, RuntimeEntries.CoreGameEngineActionDiscardColorsEntries coreActionDiscardColorsOperations, RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries coreAdjacentRedTargetsOperations, RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries coreAlternatingSuitAndCompletedTopOperations, RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, RuntimeEntries.CoreGameEngineComparedBlackSlashPoliciesEntries coreComparedBlackSlashPoliciesOperations, RuntimeEntries.CoreGameEngineCurrentTurnShownCardBenefitsEntries coreCurrentTurnShownCardBenefitsOperations, RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries coreDamageTargetBudgetOperations, RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries coreDirectedCardPolicyQueriesOperations, RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries coreDyingWindowSkillPoliciesOperations, RuntimeEntries.CoreGameEngineFinalTargetSlashEntries coreFinalTargetSlashOperations, RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries coreSoleTargetProtectionOperations, RuntimeEntries.CoreGameEngineSourceCursesEntries coreSourceCursesOperations, RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries coreTargetedOrdinaryTrickOperations, RuntimeEntries.CoreNativeBorrowedSwordEntries coreNativeBorrowedSwordOperations, RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, RuntimeEntries.CoreNativeQinglongEntries coreNativeQinglongOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
