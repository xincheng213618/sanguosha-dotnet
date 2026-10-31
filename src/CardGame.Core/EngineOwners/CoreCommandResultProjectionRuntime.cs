#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreCommandResultProjectionRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreCommandResultProjectionCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelRevisionAccess _kernelCoreGameEngineCommandSessionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess;
    internal CoreCommandResultProjectionRuntime(global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, ICoreCommandResultProjectionCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, global::CardGame.Core.EngineOwners.IKernelRevisionAccess kernelCoreGameEngineCommandSessionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess)
    {
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._kernelCoreGameEngineCommandSessionOperations = kernelCoreGameEngineCommandSessionOperations;
        this._coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess = coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess;
    }
}
