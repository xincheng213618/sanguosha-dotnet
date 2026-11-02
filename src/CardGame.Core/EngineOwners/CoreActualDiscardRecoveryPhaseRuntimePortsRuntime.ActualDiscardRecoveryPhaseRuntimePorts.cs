#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreActualDiscardRecoveryPhaseRuntimePortsRuntime
{
    IActualDiscardPhaseQueryCacheState? IActualPhaseQueryCache.Current => _coreGameEngineActionQueriesActionQueryAccess._actionQuery;

    Winner IActualDiscardRecoveryPhaseFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;
}
