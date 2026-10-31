using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record BoundDyingAlcoholUseReceipt(long ProgramFrameId, int InstructionIndex,
    CardConversionSource Source, string GameplayHash, long DyingFrameId, int VictimSeat,
    long CardUseFrameId, long ActionId, int CardId, CardLocation From, CardMoveReason Reason, long SequenceBefore);
public sealed record BoundDyingAlcoholPaymentReceipt(long? RecoveryAttemptId, int Recovered, int RemainingHp);
public sealed record ProgramBoundDyingAlcoholIssuedEvent(BoundDyingAlcoholUseReceipt Receipt) : IGameEvent;
public sealed record ProgramBoundDyingAlcoholPaidEvent(long CardUseFrameId, long ProgramFrameId,
    BoundDyingAlcoholPaymentReceipt Payment) : IGameEvent;

public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoundDyingAlcoholUseReceipt? BoundDyingAlcoholUse { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoundDyingAlcoholPaymentReceipt? BoundDyingAlcoholPayment { get; init; }
}
