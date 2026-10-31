namespace CardGame.Core;

// 巧力 evidence: every committed option, draw, distribution and ending gain is a
// scalar fact; cold recovery rebuilds the turn counts from these facts alone.
public sealed record ProgramQiaoliArmorCommittedEvent(long FrameId, string SkillId, string ActivationId,
    int OwnerSeat, int TurnNumber) : IGameEvent;
public sealed record ProgramQiaoliWeaponDrawEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int Range, int DrawnCount, int TurnNumber) : IGameEvent;
public sealed record ProgramQiaoliDistributionEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int CardId, int? RecipientSeat) : IGameEvent;
public sealed record ProgramQiaoliEndingGainEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int CardId, int TurnNumber) : IGameEvent;
public sealed record ProgramQingliangRevealEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int ActorSeat, int CardCount) : IGameEvent;
public sealed record ProgramQingliangResolutionEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int ActorSeat, string Choice, int DiscardCount, bool Nullified) : IGameEvent;

// One prompt per drawn card; the distribution rides the owning trigger frame.
public sealed record RuiJiDistributionState(int OwnerSeat, IReadOnlyList<int> CardIds, int Cursor);
// 清靓 suspends between the option prompt and the suit prompt on the same frame.
public sealed record RuiJiQingliangState(long ParentUseFrameId, int ActorSeat, string Stage);

public sealed partial record ProgramSkillFrame
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public RuiJiDistributionState? RuiJiDistribution { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public RuiJiQingliangState? RuiJiQingliang { get; init; }
}
