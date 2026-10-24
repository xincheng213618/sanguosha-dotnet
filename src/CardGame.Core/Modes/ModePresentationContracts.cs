namespace CardGame.Core;

/// <summary>Already formatted native names and summaries; presentation selects only the mode's established wording.</summary>
public sealed record ModePresentationContext(int PlayerCount, int CurrentSeat, string CurrentPlayerName,
    string FirstTeamName, string RoleSummary, string TeamSummary, string FactionSummary);

public sealed record ModeFactionDisplayName(string Id, string Name);

public sealed class ModeWinnerPresentationContext
{
    public ModeWinnerPresentationContext(Winner winner, string defaultWinnerName, string? winningTeamId,
        string? winningTeamName, string? winningFactionName, string? winningPlayerName,
        IEnumerable<ModeFactionDisplayName>? factionNames = null)
    {
        Winner = winner;
        DefaultWinnerName = defaultWinnerName;
        WinningTeamId = winningTeamId;
        WinningTeamName = winningTeamName;
        WinningFactionName = winningFactionName;
        WinningPlayerName = winningPlayerName;
        FactionNames = Array.AsReadOnly((factionNames ?? []).ToArray());
    }
    public Winner Winner { get; }
    public string DefaultWinnerName { get; }
    public string? WinningTeamId { get; }
    public string? WinningTeamName { get; }
    public string? WinningFactionName { get; }
    public string? WinningPlayerName { get; }
    public IReadOnlyList<ModeFactionDisplayName> FactionNames { get; }
}

public interface IMatchModePresentation : IGameModeFeature
{
    string StartLog(ModePresentationContext context);
    string RulesText(ModePresentationContext context);
    string WinnerLog(ModeWinnerPresentationContext context);
}
