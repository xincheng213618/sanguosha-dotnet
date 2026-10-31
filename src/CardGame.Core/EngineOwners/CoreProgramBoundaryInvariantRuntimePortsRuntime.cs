#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramBoundaryInvariantRuntimePortsRuntime : global::CardGame.Core.IProgramBoundaryInvariantFacts
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries _coreHellActivitiesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries _corePhaseHandSeizureOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheMarketOpeningEntries _coreShanheMarketOpeningOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess _coreActualTurnProgressTurnProgressionAccess;
    internal CoreProgramBoundaryInvariantRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries coreHellActivitiesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries corePhaseHandSeizureOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheMarketOpeningEntries coreShanheMarketOpeningOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess coreActualTurnProgressTurnProgressionAccess)
    {
        this._coreHellActivitiesOperations = coreHellActivitiesOperations;
        this._corePhaseHandSeizureOperations = corePhaseHandSeizureOperations;
        this._coreShanheMarketOpeningOperations = coreShanheMarketOpeningOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressTurnProgressionAccess = coreActualTurnProgressTurnProgressionAccess;
    }
}
