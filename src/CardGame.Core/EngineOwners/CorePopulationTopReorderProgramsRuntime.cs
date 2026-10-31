#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CorePopulationTopReorderProgramsRuntime : global::CardGame.Core.IPopulationTopReorderProgramEffectHost
{
    private readonly ICorePopulationTopReorderProgramsCardZonesOperations _cardZonesOperations;
    private readonly ICorePopulationTopReorderProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePopulationTopReorderProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICorePopulationTopReorderProgramsCoreProgramTopReorderOperations _coreProgramTopReorderOperations;
    private readonly ICorePopulationTopReorderProgramsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICorePopulationTopReorderProgramsCoreSnapshotValueProjectionOperations _coreSnapshotValueProjectionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    internal CorePopulationTopReorderProgramsRuntime(ICorePopulationTopReorderProgramsCardZonesOperations cardZonesOperations, ICorePopulationTopReorderProgramsContentRegistryOperations contentRegistryOperations, ICorePopulationTopReorderProgramsResolutionStackOperations resolutionStackOperations, ICorePopulationTopReorderProgramsCoreProgramTopReorderOperations coreProgramTopReorderOperations, ICorePopulationTopReorderProgramsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICorePopulationTopReorderProgramsCoreSnapshotValueProjectionOperations coreSnapshotValueProjectionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramTopReorderOperations = coreProgramTopReorderOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
    }
}
