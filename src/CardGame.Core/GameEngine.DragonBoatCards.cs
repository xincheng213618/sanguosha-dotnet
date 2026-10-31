using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum DragonBoatCardEffectStage { Drawing, DrawingChildren, AwaitingDamage, Ranking, ChoosingDiscard, DiscardChildren, FinishingAttack }
public sealed record DragonBoatCardEffectReceipt(long ActionId, long NullificationFrameId, CardKind Kind,
    int TargetSeat, int TargetIndex, DragonBoatCardEffectStage Stage)
{
    private readonly IReadOnlyList<int> _recipients = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Recipients { get => _recipients; init => _recipients = Array.AsReadOnly(value.ToArray()); }
    public int RecipientIndex { get; init; }
    public int RequestedCount { get; init; }
    public NativeDrawOutcome? DrawOutcome { get; init; }
    public long? AwaitingProgramFrameId { get; init; }
    public bool MostTreasures { get; init; }
    public bool LeastTreasures { get; init; }
    public int? DiscardCardId { get; init; }
    public CardLocation? DiscardFrom { get; init; }
    public int DiscardSequence { get; init; }
}
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DragonBoatCardEffectReceipt? DragonBoatCardEffect { get; init; }
}
public sealed partial record CardsMovedTriggerWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? ResumeDragonBoatCardEffectFrameId { get; init; }
}
public sealed record DragonBoatCardEffectStartedEvent(long UseFrameId, long ActionId, long NullificationFrameId,
    CardKind Kind, int TargetSeat, int TargetIndex, int RecipientCount, int RequestedCount) : IGameEvent;
public sealed record DragonBoatCardDrawAppliedEvent(long UseFrameId, long NullificationFrameId, int RecipientIndex,
    int RecipientSeat, int RequestedCount, int ActualDrawCount, long SequenceBefore, long SequenceAfter) : IGameEvent;
public sealed record DragonBoatAgainstCurrentRankingEvent(long UseFrameId, long NullificationFrameId,
    int TeamTreasures, bool Most, bool Least) : IGameEvent;
