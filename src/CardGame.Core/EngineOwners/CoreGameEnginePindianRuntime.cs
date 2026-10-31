#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePindianRuntime
{
    private readonly RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly RuntimeEntries.CharacterJianYongBoundaryMobileEntries _characterJianYongBoundaryMobileOperations;
    private readonly RuntimeEntries.CharacterLuKangWumiaoEntries _characterLuKangWumiaoOperations;
    private readonly RuntimeEntries.CharacterPangJiOlEntries _characterPangJiOlOperations;
    private readonly RuntimeEntries.CharacterZhangLiaoMobileGodEntries _characterZhangLiaoMobileGodOperations;
    private readonly RuntimeEntries.CharacterZhuRongBoundaryMobileEntries _characterZhuRongBoundaryMobileOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries _coreAlcoholIdentityPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineEquipmentSlotGroupProgramsEntries _coreEquipmentSlotGroupProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineForeignTurnContestsEntries _coreForeignTurnContestsOperations;
    private readonly RuntimeEntries.CoreGameEnginePindianClaimsEntries _corePindianClaimsOperations;
    private readonly RuntimeEntries.CoreGameEnginePindianHandReturnsEntries _corePindianHandReturnsOperations;
    private readonly RuntimeEntries.CoreGameEnginePindianMaximumSlashClaimsEntries _corePindianMaximumSlashClaimsOperations;
    private readonly RuntimeEntries.CoreGameEnginePindianPoliciesEntries _corePindianPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEnginePindianRandomSelectionsEntries _corePindianRandomSelectionsOperations;
    private readonly RuntimeEntries.CoreGameEnginePindianRevealWindowsEntries _corePindianRevealWindowsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly RuntimeEntries.CorePindianOwnedPileSourcesEntries _corePindianOwnedPileSourcesOperations;
    private readonly RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly IKernelRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePindianRuntime(RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, RuntimeEntries.CharacterJianYongBoundaryMobileEntries characterJianYongBoundaryMobileOperations, RuntimeEntries.CharacterLuKangWumiaoEntries characterLuKangWumiaoOperations, RuntimeEntries.CharacterPangJiOlEntries characterPangJiOlOperations, RuntimeEntries.CharacterZhangLiaoMobileGodEntries characterZhangLiaoMobileGodOperations, RuntimeEntries.CharacterZhuRongBoundaryMobileEntries characterZhuRongBoundaryMobileOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries coreAlcoholIdentityPoliciesOperations, RuntimeEntries.CoreGameEngineEquipmentSlotGroupProgramsEntries coreEquipmentSlotGroupProgramsOperations, RuntimeEntries.CoreGameEngineForeignTurnContestsEntries coreForeignTurnContestsOperations, RuntimeEntries.CoreGameEnginePindianClaimsEntries corePindianClaimsOperations, RuntimeEntries.CoreGameEnginePindianHandReturnsEntries corePindianHandReturnsOperations, RuntimeEntries.CoreGameEnginePindianMaximumSlashClaimsEntries corePindianMaximumSlashClaimsOperations, RuntimeEntries.CoreGameEnginePindianPoliciesEntries corePindianPoliciesOperations, RuntimeEntries.CoreGameEnginePindianRandomSelectionsEntries corePindianRandomSelectionsOperations, RuntimeEntries.CoreGameEnginePindianRevealWindowsEntries corePindianRevealWindowsOperations, RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, RuntimeEntries.CorePindianOwnedPileSourcesEntries corePindianOwnedPileSourcesOperations, RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, IKernelRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._characterJianYongBoundaryMobileOperations = characterJianYongBoundaryMobileOperations;
        this._characterLuKangWumiaoOperations = characterLuKangWumiaoOperations;
        this._characterPangJiOlOperations = characterPangJiOlOperations;
        this._characterZhangLiaoMobileGodOperations = characterZhangLiaoMobileGodOperations;
        this._characterZhuRongBoundaryMobileOperations = characterZhuRongBoundaryMobileOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlcoholIdentityPoliciesOperations = coreAlcoholIdentityPoliciesOperations;
        this._coreEquipmentSlotGroupProgramsOperations = coreEquipmentSlotGroupProgramsOperations;
        this._coreForeignTurnContestsOperations = coreForeignTurnContestsOperations;
        this._corePindianClaimsOperations = corePindianClaimsOperations;
        this._corePindianHandReturnsOperations = corePindianHandReturnsOperations;
        this._corePindianMaximumSlashClaimsOperations = corePindianMaximumSlashClaimsOperations;
        this._corePindianPoliciesOperations = corePindianPoliciesOperations;
        this._corePindianRandomSelectionsOperations = corePindianRandomSelectionsOperations;
        this._corePindianRevealWindowsOperations = corePindianRevealWindowsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._corePindianOwnedPileSourcesOperations = corePindianOwnedPileSourcesOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
