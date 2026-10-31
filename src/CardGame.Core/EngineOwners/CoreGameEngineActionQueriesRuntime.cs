#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActionQueriesRuntime
{
    private readonly ICoreActionQueriesCoreCardConversionsOperations _coreCardConversionsOperations;
    private readonly ICoreActionQueriesCoreRuleQueriesOperations _coreRuleQueriesOperations;
    private readonly ICoreActionQueriesCoreNativeLegalActionsOperations _coreNativeLegalActionsOperations;
    private readonly ICoreActionQueriesKernelCoreGameEngineActionQueriesOperations _kernelCoreGameEngineActionQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess _coreGameEngineActionQueriesActionQueryAccess;
    internal CoreGameEngineActionQueriesRuntime(ICoreActionQueriesCoreCardConversionsOperations coreCardConversionsOperations, ICoreActionQueriesCoreRuleQueriesOperations coreRuleQueriesOperations, ICoreActionQueriesCoreNativeLegalActionsOperations coreNativeLegalActionsOperations, ICoreActionQueriesKernelCoreGameEngineActionQueriesOperations kernelCoreGameEngineActionQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess coreGameEngineActionQueriesActionQueryAccess)
    {
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreNativeLegalActionsOperations = coreNativeLegalActionsOperations;
        this._kernelCoreGameEngineActionQueriesOperations = kernelCoreGameEngineActionQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineActionQueriesActionQueryAccess = coreGameEngineActionQueriesActionQueryAccess;
    }
}
