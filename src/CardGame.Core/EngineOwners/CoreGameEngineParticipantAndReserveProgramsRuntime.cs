#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEngineParticipantAndReserveProgramsRuntime : IParticipantReserveProgramHost
{
    private readonly RuntimeEntries.CharacterBianFuRenOlEntries _characterBianFuRenOlOperations;
    private readonly RuntimeEntries.CharacterCaiWenJiLeEntries _characterCaiWenJiLeOperations;
    private readonly RuntimeEntries.CharacterCaiYongLeEntries _characterCaiYongLeOperations;
    private readonly RuntimeEntries.CharacterCaoZhiLeEntries _characterCaoZhiLeOperations;
    private readonly RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly RuntimeEntries.CharacterLuBuOlMagicEntries _characterLuBuOlMagicOperations;
    private readonly RuntimeEntries.CharacterMiHengLeEntries _characterMiHengLeOperations;
    private readonly RuntimeEntries.CharacterXiaoQiaoLeEntries _characterXiaoQiaoLeOperations;
    private readonly RuntimeEntries.CharacterZhouFeiLeEntries _characterZhouFeiLeOperations;
    private readonly RuntimeEntries.CharacterZhouYuLeEntries _characterZhouYuLeOperations;
    private readonly RuntimeEntries.CharacterZhugeGuoLeEntries _characterZhugeGuoLeOperations;
    private readonly RuntimeEntries.CharacterZouShiLeEntries _characterZouShiLeOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries _coreForeignEquipmentDiscardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineIdentityConcealmentEntries _coreIdentityConcealmentOperations;
    private readonly RuntimeEntries.CoreGameEngineNearestLegalSlashRequestsEntries _coreNearestLegalSlashRequestsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramMarkerPaymentsEntries _coreProgramMarkerPaymentsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramSlashRequestEntries _coreProgramSlashRequestOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly RuntimeEntries.CoreGameEngineWenheChaosEntries _coreWenheChaosOperations;
    private readonly RuntimeEntries.CoreMatchSetupDefinitionsEntries _coreMatchSetupDefinitionsOperations;
    private readonly RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly IKernelCreateProgramSkillHostOperations _kernelCoreGameEngineAdvancedLifecycleHostOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess _coreActualTurnProgressCurrentSeatStartedAccess;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly EngineStates.Contracts.ICoreMatchSetupGameStartingProgramsResolvedAccess _coreMatchSetupGameStartingProgramsResolvedAccess;
    internal CoreGameEngineParticipantAndReserveProgramsRuntime(RuntimeEntries.CharacterBianFuRenOlEntries characterBianFuRenOlOperations,RuntimeEntries.CharacterCaiWenJiLeEntries characterCaiWenJiLeOperations,RuntimeEntries.CharacterCaiYongLeEntries characterCaiYongLeOperations,RuntimeEntries.CharacterCaoZhiLeEntries characterCaoZhiLeOperations,RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations,RuntimeEntries.CharacterLuBuOlMagicEntries characterLuBuOlMagicOperations,RuntimeEntries.CharacterMiHengLeEntries characterMiHengLeOperations,RuntimeEntries.CharacterXiaoQiaoLeEntries characterXiaoQiaoLeOperations,RuntimeEntries.CharacterZhouFeiLeEntries characterZhouFeiLeOperations,RuntimeEntries.CharacterZhouYuLeEntries characterZhouYuLeOperations,RuntimeEntries.CharacterZhugeGuoLeEntries characterZhugeGuoLeOperations,RuntimeEntries.CharacterZouShiLeEntries characterZouShiLeOperations,IKernelCardZoneStoreAccess cardZonesOperations,IKernelContentRegistryAccess contentRegistryOperations,IKernelFrameStoreAccess resolutionStackOperations,RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries coreForeignEquipmentDiscardPoliciesOperations,RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations,RuntimeEntries.CoreGameEngineIdentityConcealmentEntries coreIdentityConcealmentOperations,RuntimeEntries.CoreGameEngineNearestLegalSlashRequestsEntries coreNearestLegalSlashRequestsOperations,RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,RuntimeEntries.CoreGameEngineProgramMarkerPaymentsEntries coreProgramMarkerPaymentsOperations,RuntimeEntries.CoreGameEngineProgramSlashRequestEntries coreProgramSlashRequestOperations,RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations,RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations,RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations,RuntimeEntries.CoreGameEngineWenheChaosEntries coreWenheChaosOperations,RuntimeEntries.CoreMatchSetupDefinitionsEntries coreMatchSetupDefinitionsOperations,RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations,RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations,RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations,RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations,RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations,IKernelCreateProgramSkillHostOperations kernelCoreGameEngineAdvancedLifecycleHostOperations,IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess coreActualTurnProgressCurrentSeatStartedAccess,EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess,EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess,EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess,EngineStates.Contracts.ICoreMatchSetupGameStartingProgramsResolvedAccess coreMatchSetupGameStartingProgramsResolvedAccess)
    {
        this._characterBianFuRenOlOperations = characterBianFuRenOlOperations;
        this._characterCaiWenJiLeOperations = characterCaiWenJiLeOperations;
        this._characterCaiYongLeOperations = characterCaiYongLeOperations;
        this._characterCaoZhiLeOperations = characterCaoZhiLeOperations;
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterLuBuOlMagicOperations = characterLuBuOlMagicOperations;
        this._characterMiHengLeOperations = characterMiHengLeOperations;
        this._characterXiaoQiaoLeOperations = characterXiaoQiaoLeOperations;
        this._characterZhouFeiLeOperations = characterZhouFeiLeOperations;
        this._characterZhouYuLeOperations = characterZhouYuLeOperations;
        this._characterZhugeGuoLeOperations = characterZhugeGuoLeOperations;
        this._characterZouShiLeOperations = characterZouShiLeOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreForeignEquipmentDiscardPoliciesOperations = coreForeignEquipmentDiscardPoliciesOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreIdentityConcealmentOperations = coreIdentityConcealmentOperations;
        this._coreNearestLegalSlashRequestsOperations = coreNearestLegalSlashRequestsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramMarkerPaymentsOperations = coreProgramMarkerPaymentsOperations;
        this._coreProgramSlashRequestOperations = coreProgramSlashRequestOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreWenheChaosOperations = coreWenheChaosOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineAdvancedLifecycleHostOperations = kernelCoreGameEngineAdvancedLifecycleHostOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatStartedAccess = coreActualTurnProgressCurrentSeatStartedAccess;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreMatchSetupGameStartingProgramsResolvedAccess = coreMatchSetupGameStartingProgramsResolvedAccess;
    }
}
