#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreCommandResultProjectionRuntime
{
    internal EngineRunResult BuildResult()
    {
        var state = _corePlayerViewProjectionOperations.State;
        return new EngineRunResult(_coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._status, _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winner, state, state.PendingDecision, _kernelCoreGameEngineCommandSessionOperations._revision)
        {
            WinnerSeat = _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerSeat,
            WinnerTeamId = _coreNativeDamageHandlesOperations.IsTeamMode ? _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerTeamId : null,
            WinnerFactionId = _coreNativeDamageHandlesOperations.IsNationalWarMode ? _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerFactionId : null
        };
    }

    internal void RefreshPendingDecisionRevision()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is { } pending)
        {
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = pending with
            {
                Revision = _kernelCoreGameEngineCommandSessionOperations._revision
            };
        }
    }
}
