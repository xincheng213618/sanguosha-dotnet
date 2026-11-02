namespace CardGame.Core;

/// <summary>Mode-owned admission to shared tracking mechanisms; content and live skill dependencies still apply independently.</summary>
public sealed record ModeRuntimeRequirements(
    ProgramTrackedFact RequiredFacts = ProgramTrackedFact.None,
    bool TracksRoundEnding = false,
    bool CapturesRecipientDamageBase = false,
    bool TracksFinalizedCardTargets = false);
