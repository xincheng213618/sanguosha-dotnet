using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record RecoveryAmountBonusSource(CardConversionSource Source, string GameplayHash,
    int Amount, int? MaximumRecoveryOrdinal);

public sealed record RecoveryAmountBonusReceipt(int SourceSeat, int TargetSeat,
    int BaseRequestedAmount, int ModifiedRequestedAmount, int HpBefore, int HpAfter,
    int ActualTurnNumber, int ActualTurnOwnerSeat, int? ActualTurnRecoveryOrdinal)
{
    public int MaxHpAtApplication { get; init; }
    public long AppliedRecoveryFrameId { get; init; }
    private readonly IReadOnlyList<RecoveryAmountBonusSource> _sources = Array.AsReadOnly(Array.Empty<RecoveryAmountBonusSource>());
    public IReadOnlyList<RecoveryAmountBonusSource> Sources
    {
        get => _sources;
        init => _sources = Array.AsReadOnly(value.ToArray());
    }
}

public sealed record RecoveryAmountBonusAppliedEvent(long RecoveryFrameId, RecoveryAmountBonusReceipt Receipt) : IGameEvent;
public sealed record ActualTurnRecoveryRecordedEvent(long RecoveryFrameId, long ParentFrameId,
    int SourceSeat, int TargetSeat, int TurnNumber, int TurnOwnerSeat, int Ordinal,
    int RequestedAmount, int ActualAmount, int HpBefore, int HpAfter) : IGameEvent;

public sealed partial record RecoveryReplacementFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RecoveryAmountBonusReceipt? AmountBonus { get; init; }
}
