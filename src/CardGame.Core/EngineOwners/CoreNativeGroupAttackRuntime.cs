#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeGroupAttackRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries _characterGuoTuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuQiOlEntries _characterLiuQiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuMengMouEntries _characterLuMengMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuXunMouMobileEntries _characterLuXunMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaYiMobileGodEntries _characterSimaYiMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangMouEntries _characterZhugeLiangMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries _coreDirectedCardPolicyQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGroupTargetNullificationEntries _coreGroupTargetNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries _coreHellActivitiesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries _coreIssuedPlayUsePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries _coreProgramCardTargetEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries _coreProgramDodgeResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries _coreRequestedDeckBasicsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRequiredGroupDodgesEntries _coreRequiredGroupDodgesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries _coreVirtualOrdinaryTrickUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeGroupCompletionEntries _coreNativeGroupCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeGroupResponsesEntries _coreNativeGroupResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationFramesEntries _coreNativeNullificationFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries _coreNativeResponseMaterialQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseWindowsEntries _coreNativeResponseWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardAttackHandleFactory _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeGroupAttackRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries characterGuoTuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuQiOlEntries characterLiuQiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLuMengMouEntries characterLuMengMouOperations, global::CardGame.Core.RuntimeEntries.CharacterLuXunMouMobileEntries characterLuXunMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterSimaYiMobileGodEntries characterSimaYiMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangMouEntries characterZhugeLiangMouOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries coreDirectedCardPolicyQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGroupTargetNullificationEntries coreGroupTargetNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries coreHellActivitiesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries coreIssuedPlayUsePoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries coreProgramCardTargetEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries coreProgramDodgeResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries coreRequestedDeckBasicsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRequiredGroupDodgesEntries coreRequiredGroupDodgesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries coreVirtualOrdinaryTrickUsesOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeGroupCompletionEntries coreNativeGroupCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeGroupResponsesEntries coreNativeGroupResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeNullificationFramesEntries coreNativeNullificationFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries coreNativeResponseMaterialQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeResponseWindowsEntries coreNativeResponseWindowsOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.EngineOwners.IKernelCardAttackHandleFactory kernelCoreMainOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterGuoTuOlOperations = characterGuoTuOlOperations;
        this._characterLiuQiOlOperations = characterLiuQiOlOperations;
        this._characterLuMengMouOperations = characterLuMengMouOperations;
        this._characterLuXunMouMobileOperations = characterLuXunMouMobileOperations;
        this._characterSimaYiMobileGodOperations = characterSimaYiMobileGodOperations;
        this._characterZhugeLiangMouOperations = characterZhugeLiangMouOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreGroupTargetNullificationOperations = coreGroupTargetNullificationOperations;
        this._coreHellActivitiesOperations = coreHellActivitiesOperations;
        this._coreIssuedPlayUsePoliciesOperations = coreIssuedPlayUsePoliciesOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramCardTargetEffectsOperations = coreProgramCardTargetEffectsOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreRequestedDeckBasicsOperations = coreRequestedDeckBasicsOperations;
        this._coreRequiredGroupDodgesOperations = coreRequiredGroupDodgesOperations;
        this._coreVirtualOrdinaryTrickUsesOperations = coreVirtualOrdinaryTrickUsesOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreNativeGroupCompletionOperations = coreNativeGroupCompletionOperations;
        this._coreNativeGroupResponsesOperations = coreNativeGroupResponsesOperations;
        this._coreNativeNullificationFramesOperations = coreNativeNullificationFramesOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeResponseMaterialQueriesOperations = coreNativeResponseMaterialQueriesOperations;
        this._coreNativeResponseWindowsOperations = coreNativeResponseWindowsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
