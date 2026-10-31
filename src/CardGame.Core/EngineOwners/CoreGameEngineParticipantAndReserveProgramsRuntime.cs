#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineParticipantAndReserveProgramsRuntime : global::CardGame.Core.IParticipantReserveProgramHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBianFuRenOlEntries _characterBianFuRenOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaiWenJiLeEntries _characterCaiWenJiLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaiYongLeEntries _characterCaiYongLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries _characterCaoZhiLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuBuOlMagicEntries _characterLuBuOlMagicOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries _characterMiHengLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoLeEntries _characterXiaoQiaoLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouFeiLeEntries _characterZhouFeiLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouYuLeEntries _characterZhouYuLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeGuoLeEntries _characterZhugeGuoLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZouShiLeEntries _characterZouShiLeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries _coreForeignEquipmentDiscardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineIdentityConcealmentEntries _coreIdentityConcealmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNearestLegalSlashRequestsEntries _coreNearestLegalSlashRequestsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramMarkerPaymentsEntries _coreProgramMarkerPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramSlashRequestEntries _coreProgramSlashRequestOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWenheChaosEntries _coreWenheChaosOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries _coreMatchSetupDefinitionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateProgramSkillHostOperations _kernelCoreGameEngineAdvancedLifecycleHostOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess _coreActualTurnProgressCurrentSeatStartedAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupGameStartingProgramsResolvedAccess _coreMatchSetupGameStartingProgramsResolvedAccess;
    internal CoreGameEngineParticipantAndReserveProgramsRuntime(global::CardGame.Core.RuntimeEntries.CharacterBianFuRenOlEntries characterBianFuRenOlOperations,global::CardGame.Core.RuntimeEntries.CharacterCaiWenJiLeEntries characterCaiWenJiLeOperations,global::CardGame.Core.RuntimeEntries.CharacterCaiYongLeEntries characterCaiYongLeOperations,global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries characterCaoZhiLeOperations,global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations,global::CardGame.Core.RuntimeEntries.CharacterLuBuOlMagicEntries characterLuBuOlMagicOperations,global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries characterMiHengLeOperations,global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoLeEntries characterXiaoQiaoLeOperations,global::CardGame.Core.RuntimeEntries.CharacterZhouFeiLeEntries characterZhouFeiLeOperations,global::CardGame.Core.RuntimeEntries.CharacterZhouYuLeEntries characterZhouYuLeOperations,global::CardGame.Core.RuntimeEntries.CharacterZhugeGuoLeEntries characterZhugeGuoLeOperations,global::CardGame.Core.RuntimeEntries.CharacterZouShiLeEntries characterZouShiLeOperations,global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries coreForeignEquipmentDiscardPoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineIdentityConcealmentEntries coreIdentityConcealmentOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNearestLegalSlashRequestsEntries coreNearestLegalSlashRequestsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramMarkerPaymentsEntries coreProgramMarkerPaymentsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramSlashRequestEntries coreProgramSlashRequestOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineWenheChaosEntries coreWenheChaosOperations,global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries coreMatchSetupDefinitionsOperations,global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations,global::CardGame.Core.EngineOwners.IKernelCreateProgramSkillHostOperations kernelCoreGameEngineAdvancedLifecycleHostOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess coreActualTurnProgressCurrentSeatStartedAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupGameStartingProgramsResolvedAccess coreMatchSetupGameStartingProgramsResolvedAccess)
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
