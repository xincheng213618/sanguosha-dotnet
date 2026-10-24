namespace CardGame.Core;

internal interface IModeOpeningRule
{
    int GetStartingSeat(IReadOnlyList<ModePlayerLife> players, Func<int, int> nextRandom,
        int? guanduCommanderSeat = null);
}

internal sealed class FixedSeatOpeningRule(int seat) : IModeOpeningRule
{
    public int GetStartingSeat(IReadOnlyList<ModePlayerLife> players, Func<int, int> nextRandom,
        int? guanduCommanderSeat = null) => seat;
}

internal sealed class RandomSeatOpeningRule(int playerCount) : IModeOpeningRule
{
    public int GetStartingSeat(IReadOnlyList<ModePlayerLife> players, Func<int, int> nextRandom,
        int? guanduCommanderSeat = null) => nextRandom(playerCount);
}

internal sealed class IdentityOpeningRule : IModeOpeningRule
{
    public int GetStartingSeat(IReadOnlyList<ModePlayerLife> players, Func<int, int> nextRandom,
        int? guanduCommanderSeat = null) => players.Single(player => player.Role == Role.Lord).Seat;
}

internal sealed class PublicTeamOpeningRule(string modeId, string? firstTeam) : IModeOpeningRule
{
    public int GetStartingSeat(IReadOnlyList<ModePlayerLife> players, Func<int, int> nextRandom,
        int? guanduCommanderSeat = null)
    {
        if (firstTeam is null) throw new InvalidOperationException($"Mode '{modeId}' has no teams.");
        var seat = players.Where(player => player.TeamId == firstTeam).OrderBy(player => player.Seat)
            .Select(player => player.Seat).FirstOrDefault(-1);
        return seat >= 0 ? seat : throw new InvalidOperationException(
            $"Mode '{modeId}' has no seat assigned to team '{firstTeam}'.");
    }
}

internal sealed class GuanduOpeningRule : IModeOpeningRule
{
    public int GetStartingSeat(IReadOnlyList<ModePlayerLife> players, Func<int, int> nextRandom,
        int? guanduCommanderSeat = null) => guanduCommanderSeat ??
            throw new InvalidOperationException("Guandu opening requires its configured Cao commander seat.");
}
