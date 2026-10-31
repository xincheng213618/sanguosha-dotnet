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

// 逊贤 evidence: one scalar per used/played card that finished into the discard
// pile, attributed while the acting use frame is still live; the gift itself is
// public once it moves.
public sealed record SettledActionCardDiscardEvent(long BatchId, int MovementSequence, int CardId,
    int ActorSeat, int TurnNumber) : IGameEvent;
public sealed record ProgramXunxianGiftEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int RecipientSeat, int CardId, int DiscardMovementSequence) : IGameEvent;

public sealed partial class GameEngine
{
}
