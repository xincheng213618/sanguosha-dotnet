using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum NativeCardEffectDrawStage { Requesting, DrawingChildren }
public sealed record NativeCardEffectDrawReceipt(long NullificationFrameId, int EffectCardId,
    CardKind EffectCardKind, int RecipientSeat, int TargetIndex, NativeCardEffectDrawStage Stage)
{
    public int RequestedCount { get; init; } = 2;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? AwaitingProgramFrameId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeDrawOutcome? Outcome { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? BypassedActionId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? BypassedEffectOrdinal { get; init; }
}

public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeCardEffectDrawReceipt? NativeCardEffectDraw { get; init; }
}

public sealed partial record CardsMovedTriggerWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? ResumeNativeCardEffectDrawFrameId { get; init; }
}

public sealed record NativeCardEffectDrawStartedEvent(long UseFrameId, long NullificationFrameId,
    int EffectCardId, CardKind EffectCardKind, int RecipientSeat, int TargetIndex,
    long? AwaitingProgramFrameId = null) : IGameEvent
{
    public int RequestedCount { get; init; } = 2;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? BypassedActionId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? BypassedEffectOrdinal { get; init; }
}
public sealed record NativeCardEffectDrawAppliedEvent(long UseFrameId, long NullificationFrameId,
    int RecipientSeat, int ActualDrawCount, long SequenceBefore, long SequenceAfter) : IGameEvent
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? BypassedEffectOrdinal { get; init; }
}
