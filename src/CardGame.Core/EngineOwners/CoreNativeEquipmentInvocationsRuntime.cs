#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeEquipmentInvocationsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries _characterHuCheErMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries _characterHuaTuoAnniversaryGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchRandomAccess _randomOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries _coreDirectedTurnCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries _coreForeignEquipmentDiscardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFactionRequestPaymentsEntries _coreFactionRequestPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries _coreNativeLogicalDiscardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineQinglongInsertionEntries _coreQinglongInsertionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDodgeEntries _coreNativeDodgeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeFactionDefenseEntries _coreNativeFactionDefenseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeIceSwordEntries _coreNativeIceSwordOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeJudgmentEntries _coreNativeJudgmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeQilinBowEntries _coreNativeQilinBowOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeQinglongEntries _coreNativeQinglongOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseWindowsEntries _coreNativeResponseWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentInvocationsEntries _coreNativeEquipmentInvocationsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardAttackAndFactionRequestHandlesLookup _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess;
    internal CoreNativeEquipmentInvocationsRuntime(global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries characterHuCheErMouOperations,global::CardGame.Core.RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries characterHuaTuoAnniversaryGodOperations,global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations,global::CardGame.Core.EngineOwners.IKernelMatchRandomAccess randomOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries coreDirectedTurnCardPoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries coreForeignEquipmentDiscardPoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineFactionRequestPaymentsEntries coreFactionRequestPaymentsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries coreNativeLogicalDiscardsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineQinglongInsertionEntries coreQinglongInsertionOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations,global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations,global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations,global::CardGame.Core.RuntimeEntries.CoreNativeDodgeEntries coreNativeDodgeOperations,global::CardGame.Core.RuntimeEntries.CoreNativeFactionDefenseEntries coreNativeFactionDefenseOperations,global::CardGame.Core.RuntimeEntries.CoreNativeIceSwordEntries coreNativeIceSwordOperations,global::CardGame.Core.RuntimeEntries.CoreNativeJudgmentEntries coreNativeJudgmentOperations,global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations,global::CardGame.Core.RuntimeEntries.CoreNativeQilinBowEntries coreNativeQilinBowOperations,global::CardGame.Core.RuntimeEntries.CoreNativeQinglongEntries coreNativeQinglongOperations,global::CardGame.Core.RuntimeEntries.CoreNativeResponseWindowsEntries coreNativeResponseWindowsOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentInvocationsEntries coreNativeEquipmentInvocationsOperations,global::CardGame.Core.EngineOwners.IKernelCardAttackAndFactionRequestHandlesLookup kernelCoreMainOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess,global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess)
    {
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._characterHuaTuoAnniversaryGodOperations = characterHuaTuoAnniversaryGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreForeignEquipmentDiscardPoliciesOperations = coreForeignEquipmentDiscardPoliciesOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreFactionRequestPaymentsOperations = coreFactionRequestPaymentsOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreQinglongInsertionOperations = coreQinglongInsertionOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeDamageApplicationOperations = coreNativeDamageApplicationOperations;
        this._coreNativeDodgeOperations = coreNativeDodgeOperations;
        this._coreNativeFactionDefenseOperations = coreNativeFactionDefenseOperations;
        this._coreNativeIceSwordOperations = coreNativeIceSwordOperations;
        this._coreNativeJudgmentOperations = coreNativeJudgmentOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeQilinBowOperations = coreNativeQilinBowOperations;
        this._coreNativeQinglongOperations = coreNativeQinglongOperations;
        this._coreNativeResponseWindowsOperations = coreNativeResponseWindowsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreNativeEquipmentInvocationsOperations = coreNativeEquipmentInvocationsOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess = coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess;
    }
}
