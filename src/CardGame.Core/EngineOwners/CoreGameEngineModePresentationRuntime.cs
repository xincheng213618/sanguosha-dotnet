#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModePresentationRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchParticipantSetupEntries _coreMatchParticipantSetupOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchPresentationEntries _coreMatchPresentationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess _coreMatchDefinitionModeDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess _coreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineModePresentationRuntime(global::CardGame.Core.RuntimeEntries.CoreMatchParticipantSetupEntries coreMatchParticipantSetupOperations, global::CardGame.Core.RuntimeEntries.CoreMatchPresentationEntries coreMatchPresentationOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess coreMatchDefinitionModeDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess coreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreMatchParticipantSetupOperations = coreMatchParticipantSetupOperations;
        this._coreMatchPresentationOperations = coreMatchPresentationOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreMatchDefinitionModeDefinitionPlayerCountAccess = coreMatchDefinitionModeDefinitionPlayerCountAccess;
        this._coreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess = coreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
