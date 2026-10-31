#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramCardUseRolesRuntime
{
    private readonly RuntimeEntries.CharacterHuaManOlEntries _characterHuaManOlOperations;
    private readonly RuntimeEntries.CharacterLiuPanOlEntries _characterLiuPanOlOperations;
    private readonly RuntimeEntries.CharacterQuHuangOlEntries _characterQuHuangOlOperations;
    private readonly RuntimeEntries.CharacterRuanYuOlEntries _characterRuanYuOlOperations;
    private readonly RuntimeEntries.CharacterSunLuBanBoundaryMobileEntries _characterSunLuBanBoundaryMobileOperations;
    private readonly RuntimeEntries.CharacterWuJingOlEntries _characterWuJingOlOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreDirectedTurnCardPoliciesEntries _coreDirectedTurnCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries _coreAdjacentRedTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineComparedBlackSlashPoliciesEntries _coreComparedBlackSlashPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries _coreDamageTargetBudgetOperations;
    private readonly RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries _coreDeckEquipmentProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries _coreDirectedCardPolicyQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries _coreDyingWindowSkillPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries _coreIssuedPlayUsePoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineOwnPublicPileConversionsEntries _coreOwnPublicPileConversionsOperations;
    private readonly RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCompoundCardTargetsEntries _coreProgramCompoundCardTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineProvenanceCardPoliciesEntries _coreProvenanceCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries _coreSoleTargetProtectionOperations;
    private readonly RuntimeEntries.CoreNativeBorrowedSwordEntries _coreNativeBorrowedSwordOperations;
    private readonly RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    private readonly ICoreProgramCardUseRolesCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly RuntimeEntries.CoreNativeTargetLegalityEntries _coreNativeTargetLegalityOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly RuntimeEntries.CoreProgramFactionActionRewardEntries _coreProgramFactionActionRewardOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramCardUseRolesRuntime(RuntimeEntries.CharacterHuaManOlEntries characterHuaManOlOperations, RuntimeEntries.CharacterLiuPanOlEntries characterLiuPanOlOperations, RuntimeEntries.CharacterQuHuangOlEntries characterQuHuangOlOperations, RuntimeEntries.CharacterRuanYuOlEntries characterRuanYuOlOperations, RuntimeEntries.CharacterSunLuBanBoundaryMobileEntries characterSunLuBanBoundaryMobileOperations, RuntimeEntries.CharacterWuJingOlEntries characterWuJingOlOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreDirectedTurnCardPoliciesEntries coreDirectedTurnCardPoliciesOperations, RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries coreAdjacentRedTargetsOperations, RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, RuntimeEntries.CoreGameEngineComparedBlackSlashPoliciesEntries coreComparedBlackSlashPoliciesOperations, RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries coreDamageTargetBudgetOperations, RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries coreDeckEquipmentProgramsOperations, RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries coreDirectedCardPolicyQueriesOperations, RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries coreDyingWindowSkillPoliciesOperations, RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries coreIssuedPlayUsePoliciesOperations, RuntimeEntries.CoreGameEngineOwnPublicPileConversionsEntries coreOwnPublicPileConversionsOperations, RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, RuntimeEntries.CoreGameEngineProgramCompoundCardTargetsEntries coreProgramCompoundCardTargetsOperations, RuntimeEntries.CoreGameEngineProvenanceCardPoliciesEntries coreProvenanceCardPoliciesOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries coreSoleTargetProtectionOperations, RuntimeEntries.CoreNativeBorrowedSwordEntries coreNativeBorrowedSwordOperations, RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations, ICoreProgramCardUseRolesCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, RuntimeEntries.CoreNativeTargetLegalityEntries coreNativeTargetLegalityOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, RuntimeEntries.CoreProgramFactionActionRewardEntries coreProgramFactionActionRewardOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuaManOlOperations = characterHuaManOlOperations;
        this._characterLiuPanOlOperations = characterLiuPanOlOperations;
        this._characterQuHuangOlOperations = characterQuHuangOlOperations;
        this._characterRuanYuOlOperations = characterRuanYuOlOperations;
        this._characterSunLuBanBoundaryMobileOperations = characterSunLuBanBoundaryMobileOperations;
        this._characterWuJingOlOperations = characterWuJingOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreAdjacentRedTargetsOperations = coreAdjacentRedTargetsOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreComparedBlackSlashPoliciesOperations = coreComparedBlackSlashPoliciesOperations;
        this._coreDamageTargetBudgetOperations = coreDamageTargetBudgetOperations;
        this._coreDeckEquipmentProgramsOperations = coreDeckEquipmentProgramsOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreDyingWindowSkillPoliciesOperations = coreDyingWindowSkillPoliciesOperations;
        this._coreIssuedPlayUsePoliciesOperations = coreIssuedPlayUsePoliciesOperations;
        this._coreOwnPublicPileConversionsOperations = coreOwnPublicPileConversionsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramCompoundCardTargetsOperations = coreProgramCompoundCardTargetsOperations;
        this._coreProvenanceCardPoliciesOperations = coreProvenanceCardPoliciesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSoleTargetProtectionOperations = coreSoleTargetProtectionOperations;
        this._coreNativeBorrowedSwordOperations = coreNativeBorrowedSwordOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramFactionActionRewardOperations = coreProgramFactionActionRewardOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
