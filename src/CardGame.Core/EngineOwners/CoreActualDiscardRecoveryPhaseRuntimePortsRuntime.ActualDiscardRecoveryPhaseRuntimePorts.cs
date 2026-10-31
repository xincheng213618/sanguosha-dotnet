#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreActualDiscardRecoveryPhaseRuntimePortsRuntime
{
    IActualDiscardPhaseQueryCacheState? IActualPhaseQueryCache.Current => _coreGameEngineActionQueriesActionQueryAccess._actionQuery;

    Winner IActualDiscardRecoveryPhaseFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool IActualDiscardRecoveryPhaseFacts.ModeTracksActualDiscardRecoveryPhases => _modeRulesOperations._modeRules.RuntimeRequirements.TracksActualDiscardRecoveryPhases;

    bool IActualDiscardRecoveryPhaseFacts.HasLishiCapability => _characterZhongHuiAnniversaryGodOperations.HasLishiCapability;
}
