#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreTableSeatQueriesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreTableSeatQueriesRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
