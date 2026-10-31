#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreExactTopReorderProgramsRuntime : global::CardGame.Core.IExactTopReorderProgramEffectHost
{
    private readonly ICoreExactTopReorderProgramsCardZonesOperations _cardZonesOperations;
    private readonly ICoreExactTopReorderProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreExactTopReorderProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreExactTopReorderProgramsCoreProgramTopReorderOperations _coreProgramTopReorderOperations;
    private readonly ICoreExactTopReorderProgramsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreExactTopReorderProgramsCoreSnapshotValueProjectionOperations _coreSnapshotValueProjectionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    internal CoreExactTopReorderProgramsRuntime(ICoreExactTopReorderProgramsCardZonesOperations cardZonesOperations, ICoreExactTopReorderProgramsContentRegistryOperations contentRegistryOperations, ICoreExactTopReorderProgramsResolutionStackOperations resolutionStackOperations, ICoreExactTopReorderProgramsCoreProgramTopReorderOperations coreProgramTopReorderOperations, ICoreExactTopReorderProgramsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreExactTopReorderProgramsCoreSnapshotValueProjectionOperations coreSnapshotValueProjectionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramTopReorderOperations = coreProgramTopReorderOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
    }
}
