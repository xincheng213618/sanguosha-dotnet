#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineModePresentationRuntime
{
    internal ModePresentationContext CaptureModePresentationContext()
    {
        var firstTeam = _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.TeamCounts?.Keys.Order(StringComparer.Ordinal).FirstOrDefault();
        return new(_coreMatchDefinitionModeDefinitionPlayerCountAccess._playerCount, _coreActualTurnProgressCurrentSeatAccess._currentSeat, _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatAccess._currentSeat].Name, firstTeam is null ? string.Empty : _coreMatchPresentationOperations.GetRoleName(_coreMatchParticipantSetupOperations.GetTeamRole(firstTeam)), string.Join("、", Enum.GetValues<Role>().Where(role => _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.RoleCounts.GetValueOrDefault(role.ToString()) > 0).Select(role => $"{(_coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.RoleCounts[role.ToString()])} {(_coreMatchPresentationOperations.GetRoleName(role))}")), _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.TeamCounts is null ? string.Empty : global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.FormatTeamSummary(_coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition), _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.FactionCounts is null ? string.Empty : global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.FormatFactionSummary(_coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition));
    }

    internal ModeWinnerPresentationContext CaptureModeWinnerPresentationContext() => new(_coreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winner, global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.GetWinnerName(_coreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winner), _coreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerTeamId, _coreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerTeamId is { } team ? global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.GetTeamName(team) : null, _coreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerFactionId is { } faction ? global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.GetFactionName(faction) : null, _coreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerSeat is { } seat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) ? _coreMatchParticipantsPlayersAccess._players[seat].Name : null, new[] { "wei", "shu", "ambitious", "wu", "qun", "jin" }.Select(id => new ModeFactionDisplayName(id, global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.GetFactionName(id))));
}
