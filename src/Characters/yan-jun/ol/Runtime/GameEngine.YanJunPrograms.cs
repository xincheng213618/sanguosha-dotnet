namespace CardGame.Core;

// 观潮 evidence: the committed phase pattern is public, and the phase-start
// event index pins the choice to exactly one play-phase instance.
public sealed record ProgramGuanchaoPatternChosenEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int PhaseEventIndex, bool Ascending) : IGameEvent;
public sealed record ProgramGuanchaoRankDrawEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int TurnNumber, int Rank, bool Drew) : IGameEvent
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public long CardActionId { get; init; }
}

public sealed record ProgramXunxianGiftEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int RecipientSeat, int CardId, int DiscardMovementSequence) : IGameEvent;
