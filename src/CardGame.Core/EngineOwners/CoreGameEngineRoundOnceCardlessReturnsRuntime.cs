#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRoundOnceCardlessReturnsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeResponsesEntries _coreRoundOnceCardlessNativeResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeUsesEntries _coreRoundOnceCardlessNativeUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessProgramRequestsEntries _coreRoundOnceCardlessProgramRequestsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAiDecisionsEntries _coreNativeAiDecisionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreRoundOnceCardlessReturnsCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess _coreAiDiagnosticsThoughtSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineRoundOnceCardlessReturnsRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeResponsesEntries coreRoundOnceCardlessNativeResponsesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeUsesEntries coreRoundOnceCardlessNativeUsesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessProgramRequestsEntries coreRoundOnceCardlessProgramRequestsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeAiDecisionsEntries coreNativeAiDecisionsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,ICoreRoundOnceCardlessReturnsCorePlayerViewProjectionOperations corePlayerViewProjectionOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess,global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess coreAiDiagnosticsThoughtSequenceAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreRoundOnceCardlessNativeResponsesOperations = coreRoundOnceCardlessNativeResponsesOperations;
        this._coreRoundOnceCardlessNativeUsesOperations = coreRoundOnceCardlessNativeUsesOperations;
        this._coreRoundOnceCardlessProgramRequestsOperations = coreRoundOnceCardlessProgramRequestsOperations;
        this._coreNativeAiDecisionsOperations = coreNativeAiDecisionsOperations;
        this._coreNativeCardUseCompletionOperations = coreNativeCardUseCompletionOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreAiDiagnosticsThoughtSequenceAccess = coreAiDiagnosticsThoughtSequenceAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
