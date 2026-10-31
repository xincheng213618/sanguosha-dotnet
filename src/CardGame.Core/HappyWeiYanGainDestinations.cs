using System.Text.Json.Serialization;

namespace CardGame.Core;

// Trusted physical-movement evidence. No private material is added to public events.
public sealed record HappyWeiYanGainDestinationPlan(
    int CardId, CardKind PrintedKind, CardLocation From, CardLocation RequestedTo,
    CardLocation ActualTo, CardMoveReason Reason, EquipmentSlot Slot,
    bool RecipientEquipmentAreaAbolished, int RecipientSlotCapacity,
    long IssueFrameId, CardConversionSource Source, string GameplayHash,
    int IssueEventIndex, int EventCountBefore, long SequenceBefore);

// A request can be replaced by obtaining the issuer's own hand card. This is a
// producer receipt, not a physical movement and not an actual hand gain.
public sealed record HappyWeiYanRetainedGainSettlement(
    long ProducerFrameId, ResolutionFrameKind ProducerKind, int? InstructionIndex,
    string? ProducerSkillId, string? ProducerBindingId, string? ProducerSkillInstanceId, string? ProducerGameplayHash,
    int Ordinal, HappyWeiYanGainDestinationPlan Plan);

public abstract partial record ResolutionFrame
{
    private readonly IReadOnlyList<HappyWeiYanRetainedGainSettlement>? _happyWeiYanRetainedGains;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HappyWeiYanRetainedGainSettlement>? HappyWeiYanRetainedGains
    {
        get => _happyWeiYanRetainedGains;
        init => _happyWeiYanRetainedGains = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
}

/// <summary>The actual outcome of one original gain request, including retained entities.</summary>
public sealed record HappyWeiYanGainRequestSettlement(long ProducerFrameId, long? MovementBatchId,
    long SequenceBefore, CardMoveReason Reason)
{
    private readonly IReadOnlyList<CardMovementRecord> _movements = Array.AsReadOnly(Array.Empty<CardMovementRecord>());
    private readonly IReadOnlyList<HappyWeiYanRetainedGainSettlement> _retainedGains =
        Array.AsReadOnly(Array.Empty<HappyWeiYanRetainedGainSettlement>());
    public IReadOnlyList<CardMovementRecord> Movements
    { get => _movements; init => _movements = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<HappyWeiYanRetainedGainSettlement> RetainedGains
    { get => _retainedGains; init => _retainedGains = Array.AsReadOnly(value.ToArray()); }
}

public sealed partial record CardMovementRecord
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HappyWeiYanGainDestinationPlan? HappyWeiYanGainDestination { get; init; }
}
