using System.Text.Json.Serialization;

namespace CardGame.Core;

/// <summary>Visibility of this departure, explicitly supplied by its native producer.</summary>
public enum CardMovementDisclosure { Unspecified, FaceUp, FaceDown }

public sealed partial record CardMovementRecord
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CardMovementDisclosure Disclosure { get; init; }
}

public sealed record WenhePoisonDeparture(int RoundNumber, int MovementSequence, int OwnerSeat, int CardId);
public enum WenhePoisonLossStage { Ready, HealthChildren, Dying, Completed }
public sealed record WenhePoisonLossReceipt(int Cursor, WenhePoisonLossStage Stage,
    int HpBefore = 0, int HpAfter = 0, long? DyingFrameId = null);

public sealed partial record CardMovementBatchContext
{
    private readonly IReadOnlyList<WenhePoisonDeparture>? _wenhePoisonDepartures;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<WenhePoisonDeparture>? WenhePoisonDepartures
    {
        get => _wenhePoisonDepartures;
        init => _wenhePoisonDepartures = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
}
public sealed partial record CardsMovedTriggerWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WenhePoisonLossReceipt? WenhePoisonLoss { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WenhePoisonFireAttackReturn? WenhePoisonFireAttackReturn { get; init; }
}

public sealed record WenhePoisonFireAttackPayment(int CostCardId, int TargetSeat, int RevealedCardId, Suit RevealedSuit,
    int SequenceBefore, int SequenceAfter, IReadOnlyList<long> BatchIds, int BatchCursor = 0, long? ActiveChildId = null)
{
    private readonly IReadOnlyList<long> _batchIds = Array.AsReadOnly(BatchIds.ToArray());
    public IReadOnlyList<long> BatchIds { get => _batchIds; init => _batchIds = Array.AsReadOnly(value.ToArray()); }
}
public sealed record WenhePoisonFireAttackReturn(long ParentFrameId, int CostCardId, long BatchId);
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WenhePoisonFireAttackPayment? WenhePoisonFireAttackPayment { get; init; }
}
public sealed record WenhePoisonFireAttackPaidEvent(long CardUseFrameId, int CostCardId, int TargetSeat,
    int RevealedCardId, Suit RevealedSuit, int SequenceBefore, int SequenceAfter) : IGameEvent;

public sealed record WenhePoisonLossAppliedEvent(long WindowFrameId, long BatchId, int RoundNumber,
    int MovementSequence, int OwnerSeat, int CardId, int HpBefore, int HpAfter) : IGameEvent;
public sealed record WenhePoisonDepartureSettledEvent(long WindowFrameId, long BatchId,
    int MovementSequence, int OwnerSeat, int CardId) : IGameEvent;
