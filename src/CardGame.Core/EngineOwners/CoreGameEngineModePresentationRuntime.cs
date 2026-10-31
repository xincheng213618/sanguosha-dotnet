#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModePresentationRuntime
{
    private readonly ICoreModePresentationCoreMatchParticipantSetupOperations _coreMatchParticipantSetupOperations;
    private readonly ICoreModePresentationCoreMatchPresentationOperations _coreMatchPresentationOperations;
    private readonly ICoreModePresentationCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess _coreMatchDefinitionModeDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess _coreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineModePresentationRuntime(ICoreModePresentationCoreMatchParticipantSetupOperations coreMatchParticipantSetupOperations, ICoreModePresentationCoreMatchPresentationOperations coreMatchPresentationOperations, ICoreModePresentationCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess coreMatchDefinitionModeDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess coreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
