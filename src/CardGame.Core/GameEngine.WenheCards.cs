using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum WenheCardEffectStage { AwaitingChaos, Children }
public sealed record WenheRedistributionMaterial(int CardId, CardKind PrintedKind, CardLocation From, CardLocation To);
public sealed record WenheCardEffectReceipt(long ActionId, long NullificationFrameId, CardKind Kind,
    int TargetSeat, int TargetIndex, WenheCardEffectStage Stage, int MovementBefore)
{
    private readonly IReadOnlyList<WenheRedistributionMaterial> _materials = Array.AsReadOnly(Array.Empty<WenheRedistributionMaterial>());
    public IReadOnlyList<WenheRedistributionMaterial> Materials
    { get => _materials; init => _materials = Array.AsReadOnly(value.ToArray()); }
    public int? SourceHpBefore { get; init; }
    public int? TargetHpBefore { get; init; }
    public int? SourceHpAfter { get; init; }
    public int? TargetHpAfter { get; init; }
    public int? SourceHandCount { get; init; }
    public bool Applied { get; init; }
    public long? MovementBatchId { get; init; }
    public int MovementAfter { get; init; }
}
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WenheCardEffectReceipt? WenheCardEffect { get; init; }
}
public sealed partial record CardsMovedTriggerWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? ResumeWenheCardEffectFrameId { get; init; }
}

// Only public quantities enter history. Hidden hand identities and the complete
// random destination plan stay on the trusted owning card frame.
public sealed record WenheCardPlanIssuedEvent(long CardUseFrameId, long ActionId, long NullificationFrameId,
    CardKind Kind, int SourceSeat, int TargetSeat, int MaterialCount, int? SourceHandCount,
    int? SourceHpBefore, int? TargetHpBefore, int? SourceHpAfter, int? TargetHpAfter) : IGameEvent;
public sealed record WenheCardEffectAppliedEvent(long CardUseFrameId, long ActionId, long NullificationFrameId,
    CardKind Kind, int MovedCards, int MovementBefore, int MovementAfter) : IGameEvent;
public sealed record WenheCardChaosReturnedEvent(long CardUseFrameId, long ActionId, long NullificationFrameId) : IGameEvent;
