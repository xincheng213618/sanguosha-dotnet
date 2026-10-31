#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramCardUseRolesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuaManOlEntries _characterHuaManOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuPanOlEntries _characterLiuPanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQuHuangOlEntries _characterQuHuangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterRuanYuOlEntries _characterRuanYuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunLuBanBoundaryMobileEntries _characterSunLuBanBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWuJingOlEntries _characterWuJingOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries _coreDirectedTurnCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries _coreAdjacentRedTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineComparedBlackSlashPoliciesEntries _coreComparedBlackSlashPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries _coreDamageTargetBudgetOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries _coreDeckEquipmentProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries _coreDirectedCardPolicyQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries _coreDyingWindowSkillPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries _coreIssuedPlayUsePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnPublicPileConversionsEntries _coreOwnPublicPileConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCompoundCardTargetsEntries _coreProgramCompoundCardTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProvenanceCardPoliciesEntries _coreProvenanceCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries _coreSoleTargetProtectionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeBorrowedSwordEntries _coreNativeBorrowedSwordOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    private readonly ICoreProgramCardUseRolesCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeTargetLegalityEntries _coreNativeTargetLegalityOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramFactionActionRewardEntries _coreProgramFactionActionRewardOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramCardUseRolesRuntime(global::CardGame.Core.RuntimeEntries.CharacterHuaManOlEntries characterHuaManOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuPanOlEntries characterLiuPanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterQuHuangOlEntries characterQuHuangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterRuanYuOlEntries characterRuanYuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterSunLuBanBoundaryMobileEntries characterSunLuBanBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterWuJingOlEntries characterWuJingOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries coreDirectedTurnCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries coreAdjacentRedTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineComparedBlackSlashPoliciesEntries coreComparedBlackSlashPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries coreDamageTargetBudgetOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries coreDeckEquipmentProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries coreDirectedCardPolicyQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries coreDyingWindowSkillPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries coreIssuedPlayUsePoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnPublicPileConversionsEntries coreOwnPublicPileConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCompoundCardTargetsEntries coreProgramCompoundCardTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProvenanceCardPoliciesEntries coreProvenanceCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries coreSoleTargetProtectionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeBorrowedSwordEntries coreNativeBorrowedSwordOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations, ICoreProgramCardUseRolesCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeTargetLegalityEntries coreNativeTargetLegalityOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreProgramFactionActionRewardEntries coreProgramFactionActionRewardOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
