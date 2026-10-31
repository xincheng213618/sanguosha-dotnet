#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativePromptBoundaryRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExtendedViewAsEntries _coreExtendedViewAsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineMultiplayerEntries _coreMultiplayerOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries _coreNativePublicPileOriginalCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnPublicPileConversionsEntries _coreOwnPublicPileConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries _coreProgramDodgeResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramFactionPoliciesEntries _coreProgramFactionPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashSuitDiscardEntries _coreSlashSuitDiscardOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeLegalActionsEntries _coreNativeLegalActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries _coreNativeResponseMaterialQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPromptsEntries _coreProgramParticipantPromptsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess _coreActualTurnProgressCurrentSeatStartedAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICorePlayerDecisionNextPromptIdAccess _corePlayerDecisionNextPromptIdAccess;
    internal CoreNativePromptBoundaryRuntime(global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineExtendedViewAsEntries coreExtendedViewAsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineMultiplayerEntries coreMultiplayerOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries coreNativePublicPileOriginalCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnPublicPileConversionsEntries coreOwnPublicPileConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries coreProgramDodgeResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramFactionPoliciesEntries coreProgramFactionPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashSuitDiscardEntries coreSlashSuitDiscardOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeLegalActionsEntries coreNativeLegalActionsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries coreNativeResponseMaterialQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPromptsEntries coreProgramParticipantPromptsOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess coreActualTurnProgressCurrentSeatStartedAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICorePlayerDecisionNextPromptIdAccess corePlayerDecisionNextPromptIdAccess)
    {
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreExtendedViewAsOperations = coreExtendedViewAsOperations;
        this._coreMultiplayerOperations = coreMultiplayerOperations;
        this._coreNativePublicPileOriginalCardsOperations = coreNativePublicPileOriginalCardsOperations;
        this._coreOwnPublicPileConversionsOperations = coreOwnPublicPileConversionsOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreProgramFactionPoliciesOperations = coreProgramFactionPoliciesOperations;
        this._coreSlashSuitDiscardOperations = coreSlashSuitDiscardOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeLegalActionsOperations = coreNativeLegalActionsOperations;
        this._coreNativeResponseMaterialQueriesOperations = coreNativeResponseMaterialQueriesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreProgramParticipantPromptsOperations = coreProgramParticipantPromptsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatStartedAccess = coreActualTurnProgressCurrentSeatStartedAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._corePlayerDecisionNextPromptIdAccess = corePlayerDecisionNextPromptIdAccess;
    }
}
