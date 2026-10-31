namespace CardGame.Core;

// 援资 evidence: one scalar record per accepted invocation. The recipient and
// the turn number are public; the same-turn damage payoff reads this committed
// fact, so cold recovery rebuilds the benefit window from history alone.
public sealed record ProgramYuanziInvokedEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int RecipientSeat, int TurnNumber) : IGameEvent;

// 援资 payoff evidence: the drawn card ids are public; one record per payoff.
public sealed record ProgramYuanziDamageDrawEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int RecipientSeat, int TurnNumber, IReadOnlyList<int> DrawnCardIds) : IGameEvent;

// 烈节 evidence: source-side HE discards become public only when paid; the
// red count behind the quota is rebuilt from the committed first-clause discards.
public sealed record ProgramLiejieSourceDiscardEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int SourceSeat, int DiscardCount, IReadOnlyList<int> DiscardedCardIds) : IGameEvent;

public sealed partial record ProgramSkillFrame
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public YuanziDrawPendingState? YuanziDrawPending { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public LiejieDiscardPendingState? LiejieDiscardPending { get; init; }
}

// The suspension receipt of one 援资 payoff prompt: the armed recipient the
// draw prompt belongs to.
public sealed record YuanziDrawPendingState(int RecipientSeat);



public sealed partial class GameEngine
{
}
