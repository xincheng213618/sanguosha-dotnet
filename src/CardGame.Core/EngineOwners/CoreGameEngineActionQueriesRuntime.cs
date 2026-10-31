#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActionQueriesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries _coreRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeLegalActionsEntries _coreNativeLegalActionsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateActionQueryScopeOperations _kernelCoreGameEngineActionQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess _coreGameEngineActionQueriesActionQueryAccess;
    internal CoreGameEngineActionQueriesRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries coreRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeLegalActionsEntries coreNativeLegalActionsOperations, global::CardGame.Core.EngineOwners.IKernelCreateActionQueryScopeOperations kernelCoreGameEngineActionQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess coreGameEngineActionQueriesActionQueryAccess)
    {
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreNativeLegalActionsOperations = coreNativeLegalActionsOperations;
        this._kernelCoreGameEngineActionQueriesOperations = kernelCoreGameEngineActionQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineActionQueriesActionQueryAccess = coreGameEngineActionQueriesActionQueryAccess;
    }
}
