#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineUnexpectedAssaultActionsRuntime
{
    private readonly ICoreUnexpectedAssaultActionsCoreActionQueriesOperations _coreActionQueriesOperations;
    private readonly ICoreUnexpectedAssaultActionsCoreCardConversionsOperations _coreCardConversionsOperations;
    private readonly ICoreUnexpectedAssaultActionsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineUnexpectedAssaultActionsRuntime(ICoreUnexpectedAssaultActionsCoreActionQueriesOperations coreActionQueriesOperations, ICoreUnexpectedAssaultActionsCoreCardConversionsOperations coreCardConversionsOperations, ICoreUnexpectedAssaultActionsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
