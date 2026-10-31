#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreProgramBoundaryInvariantRuntimePortsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundaryInvariantRuntimePortsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime))]
internal sealed class CoreProgramBoundaryInvariantRuntimePortsCorePhaseHandSeizureAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundaryInvariantRuntimePortsCorePhaseHandSeizureOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> _targetFactory = targetFactory;
    public bool IsTerminalPhaseHandDebtBoundary(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame parent) => this._targetFactory().IsTerminalPhaseHandDebtBoundary(parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime))]
internal sealed class CoreProgramBoundaryInvariantRuntimePortsCoreHellActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundaryInvariantRuntimePortsCoreHellActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> _targetFactory = targetFactory;
    public bool IsExactHellAppearanceWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().IsExactHellAppearanceWindow(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketOpeningRuntime))]
internal sealed class CoreProgramBoundaryInvariantRuntimePortsCoreShanheMarketOpeningAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketOpeningRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBoundaryInvariantRuntimePortsCoreShanheMarketOpeningOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketOpeningRuntime> _targetFactory = targetFactory;
    public bool IsExactShanheMarketOpeningWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().IsExactShanheMarketOpeningWindow(window);
}
