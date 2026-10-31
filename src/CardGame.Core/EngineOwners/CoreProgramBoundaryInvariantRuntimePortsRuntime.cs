#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramBoundaryInvariantRuntimePortsRuntime : global::CardGame.Core.IProgramBoundaryInvariantFacts
{
    private readonly ICoreProgramBoundaryInvariantRuntimePortsCoreHellActivitiesOperations _coreHellActivitiesOperations;
    private readonly ICoreProgramBoundaryInvariantRuntimePortsCorePhaseHandSeizureOperations _corePhaseHandSeizureOperations;
    private readonly ICoreProgramBoundaryInvariantRuntimePortsCoreShanheMarketOpeningOperations _coreShanheMarketOpeningOperations;
    private readonly ICoreProgramBoundaryInvariantRuntimePortsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess _coreActualTurnProgressTurnProgressionAccess;
    internal CoreProgramBoundaryInvariantRuntimePortsRuntime(ICoreProgramBoundaryInvariantRuntimePortsCoreHellActivitiesOperations coreHellActivitiesOperations, ICoreProgramBoundaryInvariantRuntimePortsCorePhaseHandSeizureOperations corePhaseHandSeizureOperations, ICoreProgramBoundaryInvariantRuntimePortsCoreShanheMarketOpeningOperations coreShanheMarketOpeningOperations, ICoreProgramBoundaryInvariantRuntimePortsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess coreActualTurnProgressTurnProgressionAccess)
    {
        this._coreHellActivitiesOperations = coreHellActivitiesOperations;
        this._corePhaseHandSeizureOperations = corePhaseHandSeizureOperations;
        this._coreShanheMarketOpeningOperations = coreShanheMarketOpeningOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressTurnProgressionAccess = coreActualTurnProgressTurnProgressionAccess;
    }
}
