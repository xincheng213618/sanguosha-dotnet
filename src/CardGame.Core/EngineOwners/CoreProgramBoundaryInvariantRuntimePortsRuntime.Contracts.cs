#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramBoundaryInvariantRuntimePortsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreProgramBoundaryInvariantRuntimePortsCorePhaseHandSeizureOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTerminalPhaseHandDebtBoundary(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame parent);
}

internal interface ICoreProgramBoundaryInvariantRuntimePortsCoreHellActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactHellAppearanceWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreProgramBoundaryInvariantRuntimePortsCoreShanheMarketOpeningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactShanheMarketOpeningWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}
