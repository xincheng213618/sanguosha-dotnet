#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramOwnedCardsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries _coreForeignEquipmentDiscardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries _coreDeferredHandAlignmentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentPairPaymentsEntries _coreEquipmentPairPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries _coreForcedPhaseAlcoholOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtProgramsEntries _coreHalfHandPhaseDebtProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedExactCountAndNamedTargetsEntries _coreOwnedExactCountAndNamedTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianHandReturnsEntries _corePindianHandReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries _coreProgramCardPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries _coreProgramCategoryTargetLedgerOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileCashOutEntries _corePublicPileCashOutOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries _coreRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSharedSlashOffersEntries _coreSharedSlashOffersOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTriggeredCardConversionsEntries _coreTriggeredCardConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnDrawDebtsEntries _coreTurnDrawDebtsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramOwnedCardsRuntime(global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations,global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries coreForeignEquipmentDiscardPoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries coreDeferredHandAlignmentsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentPairPaymentsEntries coreEquipmentPairPaymentsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries coreForcedPhaseAlcoholOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtProgramsEntries coreHalfHandPhaseDebtProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedExactCountAndNamedTargetsEntries coreOwnedExactCountAndNamedTargetsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianHandReturnsEntries corePindianHandReturnsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries coreProgramCardPaymentsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries coreProgramCategoryTargetLedgerOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileCashOutEntries corePublicPileCashOutOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries coreRuleQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSharedSlashOffersEntries coreSharedSlashOffersOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTriggeredCardConversionsEntries coreTriggeredCardConversionsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnDrawDebtsEntries coreTurnDrawDebtsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations,global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreForeignEquipmentDiscardPoliciesOperations = coreForeignEquipmentDiscardPoliciesOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreDeferredHandAlignmentsOperations = coreDeferredHandAlignmentsOperations;
        this._coreEquipmentPairPaymentsOperations = coreEquipmentPairPaymentsOperations;
        this._coreForcedPhaseAlcoholOperations = coreForcedPhaseAlcoholOperations;
        this._coreHalfHandPhaseDebtProgramsOperations = coreHalfHandPhaseDebtProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreOwnedExactCountAndNamedTargetsOperations = coreOwnedExactCountAndNamedTargetsOperations;
        this._corePindianHandReturnsOperations = corePindianHandReturnsOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramCategoryTargetLedgerOperations = coreProgramCategoryTargetLedgerOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._corePublicPileCashOutOperations = corePublicPileCashOutOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSharedSlashOffersOperations = coreSharedSlashOffersOperations;
        this._coreTriggeredCardConversionsOperations = coreTriggeredCardConversionsOperations;
        this._coreTurnDrawDebtsOperations = coreTurnDrawDebtsOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
