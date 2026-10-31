#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreActualDiscardRecoveryPhaseRuntimePortsRuntime
{
    IActualDiscardPhaseQueryCacheState? global::CardGame.Core.IActualPhaseQueryCache.Current => _coreGameEngineActionQueriesActionQueryAccess._actionQuery;

    Winner global::CardGame.Core.IActualDiscardRecoveryPhaseFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool global::CardGame.Core.IActualDiscardRecoveryPhaseFacts.ModeTracksActualDiscardRecoveryPhases => _modeRulesOperations._modeRules.RuntimeRequirements.TracksActualDiscardRecoveryPhases;

    bool global::CardGame.Core.IActualDiscardRecoveryPhaseFacts.HasLishiCapability => _characterZhongHuiAnniversaryGodOperations.HasLishiCapability;
}
