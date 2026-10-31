using System.Text.Json.Serialization;

namespace CardGame.Core;

// A scalar fact about one actual use. It holds no hand entities or mutable
// collections; turn history is the ledger, not a second pending use object.
public sealed record CurrentTurnCardUseKindsRecordedEvent(
    int TurnNumber, int TurnOwnerSeat, int ActorSeat, long OriginFrameId,
    long? CardActionId, int HandSuitMask, int CardCategoryMask,
    bool IsNullificationUse = false,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    int? ActualTurnActorUseOrdinal = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    CardActionType? RoundActionType = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    int? RoundProviderSeat = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    int? WholeActualTurnUseSequence = null) : IGameEvent
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ProgramInstructionIndex { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CardKind? EffectiveCardKind { get; init; }
}
