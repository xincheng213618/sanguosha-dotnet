#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeRuleQueriesRuntime
{
    private readonly ICoreNativeRuleQueriesCoreActionQueriesOperations _coreActionQueriesOperations;
    private readonly ICoreNativeRuleQueriesCoreRuleQueriesOperations _coreRuleQueriesOperations;
    private readonly ICoreNativeRuleQueriesCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreNativeRuleQueriesCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeRuleQueriesRuntime(ICoreNativeRuleQueriesCoreActionQueriesOperations coreActionQueriesOperations, ICoreNativeRuleQueriesCoreRuleQueriesOperations coreRuleQueriesOperations, ICoreNativeRuleQueriesCoreTableOrderOperations coreTableOrderOperations, ICoreNativeRuleQueriesCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
