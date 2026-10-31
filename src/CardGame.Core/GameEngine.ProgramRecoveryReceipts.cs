namespace CardGame.Core;

public sealed record ProgramRecoveryApplicationReceipt(long HpChangeId, long RecoveryFrameId,
    int InstructionIndex, string GameplayHash, int SourceSeat, int TargetSeat,
    int RequestedAmount, int HpBefore, int HpAfter);
public sealed record ProgramRecoveryAppliedEvent(long ProgramFrameId, ProgramRecoveryApplicationReceipt Receipt) : IGameEvent;
public sealed record ProgramRecoveryRequestedEvent(long ProgramFrameId, int InstructionIndex, string GameplayHash,
    long RecoveryFrameId, int SourceSeat, int TargetSeat, int RequestedAmount, int? HpBefore) : IGameEvent
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public HappyQiaoDualHealth? HappyQiaoBefore { get; init; }
}
