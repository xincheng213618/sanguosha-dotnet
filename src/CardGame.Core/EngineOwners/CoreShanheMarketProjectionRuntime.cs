#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreShanheMarketProjectionRuntime
{
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly IKernelShanheMarketAccess _shanheMarketOperations;
    private readonly IKernelEnsureSessionHealthyOperations _kernelCoreGameEngineFaultBoundaryOperations;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreShanheMarketProjectionRuntime(IKernelFrameStoreAccess resolutionStackOperations, IKernelShanheMarketAccess shanheMarketOperations, IKernelEnsureSessionHealthyOperations kernelCoreGameEngineFaultBoundaryOperations, EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._shanheMarketOperations = shanheMarketOperations;
        this._kernelCoreGameEngineFaultBoundaryOperations = kernelCoreGameEngineFaultBoundaryOperations;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
