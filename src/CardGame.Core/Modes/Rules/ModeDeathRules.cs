namespace CardGame.Core;

public enum ModeDeathOutcomeKind { Preserve, SurvivalRank, Identity, Team, Faction }

/// <summary>A rule decision, applied by the command session before the existing death continuation returns.</summary>
public readonly record struct ModeDeathOutcome(ModeDeathOutcomeKind Kind,
    Winner Winner = Winner.None, string? WinningGroupId = null);

public interface IModeDeathRule
{
    bool UsesIdentityRewards { get; }
    bool NotifyAiOnDeath { get; }
    bool EvaluatesIdentityVictory { get; }
    ModeDeathOutcome Evaluate(IReadOnlyList<ModePlayerLife> players, bool zaowangRebelDeathWins = false);
}
