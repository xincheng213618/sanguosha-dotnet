#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeDuelRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries _characterCaoZhiLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries _characterGuoTuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuQiOlEntries _characterLiuQiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuMengMouEntries _characterLuMengMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries _characterMaChaoAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaYiMobileGodEntries _characterSimaYiMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoMobileGodEntries _characterZhangLiaoMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionDiscardColorsEntries _coreActionDiscardColorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries _coreCardDeclarationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFriendDuelResponsesEntries _coreFriendDuelResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries _coreIssuedPlayUsePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries _coreNativePublicPileOriginalCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries _coreRequestedDeckBasicsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionZhangbaEntries _coreResponseCompletionZhangbaOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseUseCompletionEntries _coreResponseUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries _coreNativeCardUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeGroupResponsesEntries _coreNativeGroupResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeLegalActionsEntries _coreNativeLegalActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeOwnedCardQueriesEntries _coreNativeOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries _coreNativeResponseMaterialQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseWindowsEntries _coreNativeResponseWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeDuelRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries characterCaoZhiLeOperations, global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries characterGuoTuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuQiOlEntries characterLiuQiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLuMengMouEntries characterLuMengMouOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries characterMaChaoAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterSimaYiMobileGodEntries characterSimaYiMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoMobileGodEntries characterZhangLiaoMobileGodOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionDiscardColorsEntries coreActionDiscardColorsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries coreCardDeclarationsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFriendDuelResponsesEntries coreFriendDuelResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries coreIssuedPlayUsePoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries coreNativePublicPileOriginalCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries coreRequestedDeckBasicsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionZhangbaEntries coreResponseCompletionZhangbaOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseUseCompletionEntries coreResponseUseCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries coreNativeCardUseOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeGroupResponsesEntries coreNativeGroupResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeLegalActionsEntries coreNativeLegalActionsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreNativeOwnedCardQueriesEntries coreNativeOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries coreNativeResponseMaterialQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeResponseWindowsEntries coreNativeResponseWindowsOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoZhiLeOperations = characterCaoZhiLeOperations;
        this._characterGuoTuOlOperations = characterGuoTuOlOperations;
        this._characterLiuQiOlOperations = characterLiuQiOlOperations;
        this._characterLuMengMouOperations = characterLuMengMouOperations;
        this._characterMaChaoAnniversaryGodOperations = characterMaChaoAnniversaryGodOperations;
        this._characterSimaYiMobileGodOperations = characterSimaYiMobileGodOperations;
        this._characterZhangLiaoMobileGodOperations = characterZhangLiaoMobileGodOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreActionDiscardColorsOperations = coreActionDiscardColorsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreCardDeclarationsOperations = coreCardDeclarationsOperations;
        this._coreFriendDuelResponsesOperations = coreFriendDuelResponsesOperations;
        this._coreIssuedPlayUsePoliciesOperations = coreIssuedPlayUsePoliciesOperations;
        this._coreNativePublicPileOriginalCardsOperations = coreNativePublicPileOriginalCardsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreRequestedDeckBasicsOperations = coreRequestedDeckBasicsOperations;
        this._coreResponseCompletionZhangbaOperations = coreResponseCompletionZhangbaOperations;
        this._coreResponseUseCompletionOperations = coreResponseUseCompletionOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseOperations = coreNativeCardUseOperations;
        this._coreNativeDamageApplicationOperations = coreNativeDamageApplicationOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreNativeGroupResponsesOperations = coreNativeGroupResponsesOperations;
        this._coreNativeLegalActionsOperations = coreNativeLegalActionsOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeResponseMaterialQueriesOperations = coreNativeResponseMaterialQueriesOperations;
        this._coreNativeResponseWindowsOperations = coreNativeResponseWindowsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
