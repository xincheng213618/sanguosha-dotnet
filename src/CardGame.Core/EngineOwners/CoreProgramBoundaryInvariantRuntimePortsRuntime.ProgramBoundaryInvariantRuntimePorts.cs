#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramBoundaryInvariantRuntimePortsRuntime
{
    int global::CardGame.Core.IProgramBoundaryInvariantFacts.RoundNumber => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    ActualTurnKind global::CardGame.Core.IProgramBoundaryInvariantFacts.TurnKind => _coreActualTurnProgressTurnProgressionAccess._turnProgression.Kind;

    DecisionKind? global::CardGame.Core.IProgramBoundaryInvariantFacts.PendingDecisionKind => _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind;

    int global::CardGame.Core.IProgramBoundaryInvariantFacts.SkipDrawPhaseMask => (int)global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase;

    bool global::CardGame.Core.IProgramBoundaryInvariantFacts.IsTerminalPhaseHandDebtBoundary(ProgramLifecycleTriggerWindowFrame frame) => _corePhaseHandSeizureOperations.IsTerminalPhaseHandDebtBoundary(frame);
    bool global::CardGame.Core.IProgramBoundaryInvariantFacts.IsExactHellAppearanceWindow(ProgramLifecycleTriggerWindowFrame frame) => _coreHellActivitiesOperations.IsExactHellAppearanceWindow(frame);
    bool global::CardGame.Core.IProgramBoundaryInvariantFacts.IsExactShanheMarketOpeningWindow(ProgramLifecycleTriggerWindowFrame frame) => _coreShanheMarketOpeningOperations.IsExactShanheMarketOpeningWindow(frame);
}
