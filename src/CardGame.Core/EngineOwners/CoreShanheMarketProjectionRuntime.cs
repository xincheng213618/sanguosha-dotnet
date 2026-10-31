#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreShanheMarketProjectionRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelShanheMarketAccess _shanheMarketOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelEnsureSessionHealthyOperations _kernelCoreGameEngineFaultBoundaryOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreShanheMarketProjectionRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.EngineOwners.IKernelShanheMarketAccess shanheMarketOperations, global::CardGame.Core.EngineOwners.IKernelEnsureSessionHealthyOperations kernelCoreGameEngineFaultBoundaryOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._shanheMarketOperations = shanheMarketOperations;
        this._kernelCoreGameEngineFaultBoundaryOperations = kernelCoreGameEngineFaultBoundaryOperations;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
