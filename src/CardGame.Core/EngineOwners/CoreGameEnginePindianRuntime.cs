#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePindianRuntime
{
    private readonly ICorePindianCharacterGuanYuWumiaoOperations _characterGuanYuWumiaoOperations;
    private readonly ICorePindianCharacterJianYongBoundaryMobileOperations _characterJianYongBoundaryMobileOperations;
    private readonly ICorePindianCharacterLuKangWumiaoOperations _characterLuKangWumiaoOperations;
    private readonly ICorePindianCharacterPangJiOlOperations _characterPangJiOlOperations;
    private readonly ICorePindianCharacterZhangLiaoMobileGodOperations _characterZhangLiaoMobileGodOperations;
    private readonly ICorePindianCharacterZhuRongBoundaryMobileOperations _characterZhuRongBoundaryMobileOperations;
    private readonly ICorePindianCardZonesOperations _cardZonesOperations;
    private readonly ICorePindianResolutionStackOperations _resolutionStackOperations;
    private readonly ICorePindianCoreAlcoholIdentityPoliciesOperations _coreAlcoholIdentityPoliciesOperations;
    private readonly ICorePindianCoreEquipmentSlotGroupProgramsOperations _coreEquipmentSlotGroupProgramsOperations;
    private readonly ICorePindianCoreForeignTurnContestsOperations _coreForeignTurnContestsOperations;
    private readonly ICorePindianCorePindianClaimsOperations _corePindianClaimsOperations;
    private readonly ICorePindianCorePindianHandReturnsOperations _corePindianHandReturnsOperations;
    private readonly ICorePindianCorePindianMaximumSlashClaimsOperations _corePindianMaximumSlashClaimsOperations;
    private readonly ICorePindianCorePindianPoliciesOperations _corePindianPoliciesOperations;
    private readonly ICorePindianCorePindianRandomSelectionsOperations _corePindianRandomSelectionsOperations;
    private readonly ICorePindianCorePindianRevealWindowsOperations _corePindianRevealWindowsOperations;
    private readonly ICorePindianCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICorePindianCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICorePindianCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICorePindianCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICorePindianCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICorePindianCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICorePindianCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICorePindianCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICorePindianCorePindianOwnedPileSourcesOperations _corePindianOwnedPileSourcesOperations;
    private readonly ICorePindianCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICorePindianKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePindianRuntime(ICorePindianCharacterGuanYuWumiaoOperations characterGuanYuWumiaoOperations, ICorePindianCharacterJianYongBoundaryMobileOperations characterJianYongBoundaryMobileOperations, ICorePindianCharacterLuKangWumiaoOperations characterLuKangWumiaoOperations, ICorePindianCharacterPangJiOlOperations characterPangJiOlOperations, ICorePindianCharacterZhangLiaoMobileGodOperations characterZhangLiaoMobileGodOperations, ICorePindianCharacterZhuRongBoundaryMobileOperations characterZhuRongBoundaryMobileOperations, ICorePindianCardZonesOperations cardZonesOperations, ICorePindianResolutionStackOperations resolutionStackOperations, ICorePindianCoreAlcoholIdentityPoliciesOperations coreAlcoholIdentityPoliciesOperations, ICorePindianCoreEquipmentSlotGroupProgramsOperations coreEquipmentSlotGroupProgramsOperations, ICorePindianCoreForeignTurnContestsOperations coreForeignTurnContestsOperations, ICorePindianCorePindianClaimsOperations corePindianClaimsOperations, ICorePindianCorePindianHandReturnsOperations corePindianHandReturnsOperations, ICorePindianCorePindianMaximumSlashClaimsOperations corePindianMaximumSlashClaimsOperations, ICorePindianCorePindianPoliciesOperations corePindianPoliciesOperations, ICorePindianCorePindianRandomSelectionsOperations corePindianRandomSelectionsOperations, ICorePindianCorePindianRevealWindowsOperations corePindianRevealWindowsOperations, ICorePindianCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICorePindianCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICorePindianCoreRuntimeOperations coreRuntimeOperations, ICorePindianCoreMatchLogOperations coreMatchLogOperations, ICorePindianCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICorePindianCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICorePindianCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICorePindianCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICorePindianCorePindianOwnedPileSourcesOperations corePindianOwnedPileSourcesOperations, ICorePindianCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICorePindianKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
