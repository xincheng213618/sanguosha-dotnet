#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTableOrderRuntime
{
    private readonly ICoreTableOrderResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreTableOrderCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreTableOrderCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreTableOrderKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess _coreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess;
    internal CoreGameEngineTableOrderRuntime(ICoreTableOrderResolutionStackOperations resolutionStackOperations, ICoreTableOrderCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreTableOrderCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreTableOrderKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess coreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess = coreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess;
    }
}
