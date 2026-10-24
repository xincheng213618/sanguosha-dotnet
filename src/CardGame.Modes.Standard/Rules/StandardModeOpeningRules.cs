using CardGame.Core;

namespace CardGame.Modes.Standard;

internal sealed class FixedSeatOpeningRule(int seat) : IModeOpeningRule
{
    public int GetStartingSeat(IReadOnlyList<ModePlayerLife> players, Func<int, int> nextRandom,
        int? initialActorSeat = null) => seat;
}

internal sealed class RandomSeatOpeningRule(int playerCount) : IModeOpeningRule
{
    public int GetStartingSeat(IReadOnlyList<ModePlayerLife> players, Func<int, int> nextRandom,
        int? initialActorSeat = null) => nextRandom(playerCount);
}

internal sealed class IdentityOpeningRule : IModeOpeningRule
{
    public int GetStartingSeat(IReadOnlyList<ModePlayerLife> players, Func<int, int> nextRandom,
        int? initialActorSeat = null) => players.Single(player => player.Role == Role.Lord).Seat;
}

internal sealed class PublicTeamOpeningRule(string modeId, string? firstTeam) : IModeOpeningRule
{
    public int GetStartingSeat(IReadOnlyList<ModePlayerLife> players, Func<int, int> nextRandom,
        int? initialActorSeat = null)
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
        int? initialActorSeat = null) => initialActorSeat ??
            throw new InvalidOperationException("Guandu opening requires its configured Cao commander seat.");
}
