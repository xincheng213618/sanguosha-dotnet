#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreEndingHandCountProgramsRuntime : global::CardGame.Core.IEndingHandCountProgramHost
{
    private readonly ICoreEndingHandCountProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreEndingHandCountProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreEndingHandCountProgramsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreEndingHandCountProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreEndingHandCountProgramsEndingHandCountHistoriesAccess _coreEndingHandCountProgramsEndingHandCountHistoriesAccess;
    internal CoreEndingHandCountProgramsRuntime(ICoreEndingHandCountProgramsResolutionStackOperations resolutionStackOperations, ICoreEndingHandCountProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreEndingHandCountProgramsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreEndingHandCountProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreEndingHandCountProgramsEndingHandCountHistoriesAccess coreEndingHandCountProgramsEndingHandCountHistoriesAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreEndingHandCountProgramsEndingHandCountHistoriesAccess = coreEndingHandCountProgramsEndingHandCountHistoriesAccess;
    }
}
