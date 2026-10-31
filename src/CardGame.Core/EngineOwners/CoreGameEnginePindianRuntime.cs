#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePindianRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterJianYongBoundaryMobileEntries _characterJianYongBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries _characterLuKangWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterPangJiOlEntries _characterPangJiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoMobileGodEntries _characterZhangLiaoMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhuRongBoundaryMobileEntries _characterZhuRongBoundaryMobileOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries _coreAlcoholIdentityPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentSlotGroupProgramsEntries _coreEquipmentSlotGroupProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignTurnContestsEntries _coreForeignTurnContestsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianClaimsEntries _corePindianClaimsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianHandReturnsEntries _corePindianHandReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianMaximumSlashClaimsEntries _corePindianMaximumSlashClaimsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianPoliciesEntries _corePindianPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianRandomSelectionsEntries _corePindianRandomSelectionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianRevealWindowsEntries _corePindianRevealWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePindianOwnedPileSourcesEntries _corePindianOwnedPileSourcesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePindianRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterJianYongBoundaryMobileEntries characterJianYongBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries characterLuKangWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterPangJiOlEntries characterPangJiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoMobileGodEntries characterZhangLiaoMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterZhuRongBoundaryMobileEntries characterZhuRongBoundaryMobileOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries coreAlcoholIdentityPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentSlotGroupProgramsEntries coreEquipmentSlotGroupProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignTurnContestsEntries coreForeignTurnContestsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianClaimsEntries corePindianClaimsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianHandReturnsEntries corePindianHandReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianMaximumSlashClaimsEntries corePindianMaximumSlashClaimsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianPoliciesEntries corePindianPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianRandomSelectionsEntries corePindianRandomSelectionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianRevealWindowsEntries corePindianRevealWindowsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePindianOwnedPileSourcesEntries corePindianOwnedPileSourcesOperations, global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
