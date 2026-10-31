#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramSlashRequestRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChengJiChengCuiOlEntries _characterChengJiChengCuiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries _characterGuoTuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuCheErAnniversaryEntries _characterHuCheErAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuFengBoundaryEntries _characterLiuFengBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuZhangOlEntries _characterLiuZhangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaXiuMaTieOlEntries _characterMaXiuMaTieOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterPangJiOlEntries _characterPangJiOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries _coreChainedStateBasicConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicOffersEntries _coreDrawFundedDistinctBasicOffersOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries _coreNativePublicPileOriginalCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries _coreRequestedDeckBasicsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundDistinctBasicRequestsEntries _coreRoundDistinctBasicRequestsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessProgramRequestsEntries _coreRoundOnceCardlessProgramRequestsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly ICoreProgramSlashRequestCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSlashUseEntries _coreNativeSlashUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramCardLegalityEntries _coreProgramCardLegalityOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateProgramSkillHostOperations _kernelCoreGameEngineAdvancedLifecycleHostOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramSlashRequestRuntime(global::CardGame.Core.RuntimeEntries.CharacterChengJiChengCuiOlEntries characterChengJiChengCuiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries characterGuoTuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterHuCheErAnniversaryEntries characterHuCheErAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuFengBoundaryEntries characterLiuFengBoundaryOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuZhangOlEntries characterLiuZhangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaXiuMaTieOlEntries characterMaXiuMaTieOlOperations, global::CardGame.Core.RuntimeEntries.CharacterPangJiOlEntries characterPangJiOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries coreChainedStateBasicConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicOffersEntries coreDrawFundedDistinctBasicOffersOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries coreNativePublicPileOriginalCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries coreRequestedDeckBasicsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundDistinctBasicRequestsEntries coreRoundDistinctBasicRequestsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessProgramRequestsEntries coreRoundOnceCardlessProgramRequestsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations, global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, ICoreProgramSlashRequestCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSlashUseEntries coreNativeSlashUseOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreProgramCardLegalityEntries coreProgramCardLegalityOperations, global::CardGame.Core.EngineOwners.IKernelCreateProgramSkillHostOperations kernelCoreGameEngineAdvancedLifecycleHostOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterChengJiChengCuiOlOperations = characterChengJiChengCuiOlOperations;
        this._characterGuoTuOlOperations = characterGuoTuOlOperations;
        this._characterHuCheErAnniversaryOperations = characterHuCheErAnniversaryOperations;
        this._characterHuanShuJunOlOperations = characterHuanShuJunOlOperations;
        this._characterLiuFengBoundaryOperations = characterLiuFengBoundaryOperations;
        this._characterLiuZhangOlOperations = characterLiuZhangOlOperations;
        this._characterMaXiuMaTieOlOperations = characterMaXiuMaTieOlOperations;
        this._characterPangJiOlOperations = characterPangJiOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreChainedStateBasicConversionsOperations = coreChainedStateBasicConversionsOperations;
        this._coreDrawFundedDistinctBasicOffersOperations = coreDrawFundedDistinctBasicOffersOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativePublicPileOriginalCardsOperations = coreNativePublicPileOriginalCardsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRequestedDeckBasicsOperations = coreRequestedDeckBasicsOperations;
        this._coreRoundDistinctBasicRequestsOperations = coreRoundDistinctBasicRequestsOperations;
        this._coreRoundOnceCardlessProgramRequestsOperations = coreRoundOnceCardlessProgramRequestsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._coreNativeSlashUseOperations = coreNativeSlashUseOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramCardLegalityOperations = coreProgramCardLegalityOperations;
        this._kernelCoreGameEngineAdvancedLifecycleHostOperations = kernelCoreGameEngineAdvancedLifecycleHostOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
