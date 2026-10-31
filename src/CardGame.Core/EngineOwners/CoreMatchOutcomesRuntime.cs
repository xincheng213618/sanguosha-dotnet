#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreMatchOutcomesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelPendingCardsMovedBatchesAccess _pendingCardsMovedBatchesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries _coreMatchSetupDefinitionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNationalGeneralRevealEntries _coreNationalGeneralRevealOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess _coreActualTurnProgressPhaseAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess _coreMatchDefinitionModeDefinitionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess _coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    internal CoreMatchOutcomesRuntime(global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelPendingCardsMovedBatchesAccess pendingCardsMovedBatchesOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries coreMatchSetupDefinitionsOperations, global::CardGame.Core.RuntimeEntries.CoreNationalGeneralRevealEntries coreNationalGeneralRevealOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess coreActualTurnProgressPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess coreMatchDefinitionModeDefinitionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess coreGameEngineHpChangeProgramsPendingHpChangesAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._coreNationalGeneralRevealOperations = coreNationalGeneralRevealOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressPhaseAccess = coreActualTurnProgressPhaseAccess;
        this._coreMatchDefinitionModeDefinitionAccess = coreMatchDefinitionModeDefinitionAccess;
        this._coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess = coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineHpChangeProgramsPendingHpChangesAccess = coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    }
}
