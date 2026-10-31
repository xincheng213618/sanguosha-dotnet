#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheMarketOpeningRuntime
{
    private readonly ICoreShanheMarketOpeningResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheMarketOpeningCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheMarketOpeningCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheMarketOpeningCoreShanheMarketOperations _coreShanheMarketOperations;
    private readonly ICoreShanheMarketOpeningCoreShanheMarketApplicationOperations _coreShanheMarketApplicationOperations;
    private readonly ICoreShanheMarketOpeningCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheMarketOpeningRuntime(ICoreShanheMarketOpeningResolutionStackOperations resolutionStackOperations, ICoreShanheMarketOpeningCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheMarketOpeningCoreRuntimeOperations coreRuntimeOperations, ICoreShanheMarketOpeningCoreShanheMarketOperations coreShanheMarketOperations, ICoreShanheMarketOpeningCoreShanheMarketApplicationOperations coreShanheMarketApplicationOperations, ICoreShanheMarketOpeningCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreShanheMarketOperations = coreShanheMarketOperations;
        this._coreShanheMarketApplicationOperations = coreShanheMarketApplicationOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
