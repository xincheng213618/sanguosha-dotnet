namespace CardGame.Core;

public sealed partial class GameEngine
{
    private ModePresentationContext CaptureModePresentationContext()
    {
        var firstTeam = _modeDefinition.TeamCounts?.Keys.Order(StringComparer.Ordinal).FirstOrDefault();
        return new(_playerCount, _currentSeat, _players[_currentSeat].Name,
            firstTeam is null ? string.Empty : GetRoleName(GetTeamRole(firstTeam)),
            string.Join("、", Enum.GetValues<Role>().Where(role => _modeDefinition.RoleCounts.GetValueOrDefault(role.ToString()) > 0)
                .Select(role => $"{_modeDefinition.RoleCounts[role.ToString()]} {GetRoleName(role)}")),
            _modeDefinition.TeamCounts is null ? string.Empty : FormatTeamSummary(_modeDefinition),
            _modeDefinition.FactionCounts is null ? string.Empty : FormatFactionSummary(_modeDefinition));
    }

    private ModeWinnerPresentationContext CaptureModeWinnerPresentationContext() => new(_winner, GetWinnerName(_winner), _winnerTeamId,
        _winnerTeamId is { } team ? GetTeamName(team) : null,
        _winnerFactionId is { } faction ? GetFactionName(faction) : null,
        _winnerSeat is { } seat && IsValidPlayerSeat(seat) ? _players[seat].Name : null,
        new[] { "wei", "shu", "ambitious", "wu", "qun", "jin" }.Select(id => new ModeFactionDisplayName(id, GetFactionName(id))));
}
