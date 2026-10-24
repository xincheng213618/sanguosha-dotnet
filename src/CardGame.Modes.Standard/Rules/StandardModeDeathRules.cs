using CardGame.Core;

namespace CardGame.Modes.Standard;

internal abstract class ModeDeathRule : IModeDeathRule
{
    public virtual bool UsesIdentityRewards => false;
    public virtual bool NotifyAiOnDeath => true;
    public virtual bool EvaluatesIdentityVictory => false;
    public abstract ModeDeathOutcome Evaluate(IReadOnlyList<ModePlayerLife> players, bool zaowangRebelDeathWins = false);
}

internal sealed class IdentityDeathRule : ModeDeathRule
{
    public override bool UsesIdentityRewards => true;
    public override bool EvaluatesIdentityVictory => true;
    public override ModeDeathOutcome Evaluate(IReadOnlyList<ModePlayerLife> players, bool zaowangRebelDeathWins = false)
    {
        var winner = GameRules.EvaluateWinner(players.Select(player => new PlayerLifeState(player.Role, player.IsAlive)));
        return new(ModeDeathOutcomeKind.Identity, zaowangRebelDeathWins ? Winner.LordAndLoyalists : winner);
    }
}

internal sealed class NationalWarDeathRule : ModeDeathRule
{
    // Preserve the existing gate exactly. National-war compatibility roles do not match identity rewards.
    public override bool UsesIdentityRewards => true;
    public override bool NotifyAiOnDeath => false;
    public override ModeDeathOutcome Evaluate(IReadOnlyList<ModePlayerLife> players, bool zaowangRebelDeathWins = false) =>
        new(ModeDeathOutcomeKind.Faction, WinningGroupId: GameRules.EvaluateWinningFaction(players.Select(player =>
            new FactionLifeState(player.NationalFactionId!, player.IsAlive))));
}

internal sealed class PublicTeamDeathRule : ModeDeathRule
{
    public override ModeDeathOutcome Evaluate(IReadOnlyList<ModePlayerLife> players, bool zaowangRebelDeathWins = false) =>
        new(ModeDeathOutcomeKind.Team, WinningGroupId: GameRules.EvaluateWinningTeam(players.Select(player =>
            new TeamLifeState(player.TeamId!, player.IsAlive))));
}

internal sealed class SurvivalDeathRule : ModeDeathRule
{
    public override ModeDeathOutcome Evaluate(IReadOnlyList<ModePlayerLife> players, bool zaowangRebelDeathWins = false) =>
        new(ModeDeathOutcomeKind.SurvivalRank);
}

internal sealed class DeferredDeathRule : ModeDeathRule
{
    // Dragon Boat and Guandu decide the winner at their own turn/round boundary, not on each death.
    public override ModeDeathOutcome Evaluate(IReadOnlyList<ModePlayerLife> players, bool zaowangRebelDeathWins = false) =>
        new(ModeDeathOutcomeKind.Preserve);
}

internal sealed class BossEncounterDeathRule(string playerTeamId, string enemyTeamId) : ModeDeathRule
{
    // A dead boss is not yet a cleared encounter. Killing-action and death children must return first.
    public override ModeDeathOutcome Evaluate(IReadOnlyList<ModePlayerLife> players, bool zaowangRebelDeathWins = false) =>
        new(ModeDeathOutcomeKind.Team,
            WinningGroupId: players.Any(player => player.TeamId == playerTeamId && player.IsAlive) ? null : enemyTeamId);
}

internal sealed class ShenwuDeathRule : ModeDeathRule
{
    private readonly HashSet<int> _playerSeats;
    private readonly string _enemyTeamId;

    internal ShenwuDeathRule(int playerCount, int humanSeat, IReadOnlyList<int> playerSeatOffsets, string enemyTeamId)
    {
        // Slots, not an arbitrary allied TeamId, qualify the three actual participants.
        _playerSeats = playerSeatOffsets.Select(offset => (Math.Max(0, humanSeat) + offset) % playerCount).ToHashSet();
        _enemyTeamId = enemyTeamId;
    }

    public override ModeDeathOutcome Evaluate(IReadOnlyList<ModePlayerLife> players, bool zaowangRebelDeathWins = false) =>
        new(ModeDeathOutcomeKind.Team,
            WinningGroupId: players.Any(player => _playerSeats.Contains(player.Seat) && player.IsAlive) ? null : _enemyTeamId);
}

internal sealed class JiangeDeathRule(int playerCount, int humanSeat, string playerTeamId, string enemyTeamId) : ModeDeathRule
{
    public override ModeDeathOutcome Evaluate(IReadOnlyList<ModePlayerLife> players, bool zaowangRebelDeathWins = false)
    {
        var spiritsAlive = players.Any(player => player.IsAlive &&
            (player.Seat - Math.Max(0, humanSeat) + playerCount) % playerCount is 2 or 3);
        // Preserve objective-before-elimination precedence and the original public-team fallback.
        var winningTeam = !spiritsAlive ? playerTeamId :
            !players.Any(player => player.TeamId == playerTeamId && player.IsAlive) ? enemyTeamId :
            GameRules.EvaluateWinningTeam(players.Select(player => new TeamLifeState(player.TeamId!, player.IsAlive)));
        return new(ModeDeathOutcomeKind.Team, WinningGroupId: winningTeam);
    }
}
