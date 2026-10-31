#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheS5XuchuAiRuntime
{
    private readonly ICoreShanheS5XuchuAiCoreActualTurnsOperations _coreActualTurnsOperations;
    private readonly ICoreShanheS5XuchuAiCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheS5XuchuAiCoreShanheS5XuchuOperations _coreShanheS5XuchuOperations;
    private readonly ICoreShanheS5XuchuAiCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheS5XuchuAiRuntime(ICoreShanheS5XuchuAiCoreActualTurnsOperations coreActualTurnsOperations, ICoreShanheS5XuchuAiCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheS5XuchuAiCoreShanheS5XuchuOperations coreShanheS5XuchuOperations, ICoreShanheS5XuchuAiCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreShanheS5XuchuOperations = coreShanheS5XuchuOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
