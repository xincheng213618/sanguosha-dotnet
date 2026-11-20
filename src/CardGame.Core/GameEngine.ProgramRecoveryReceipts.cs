namespace CardGame.Core;

public sealed record ProgramOperationRecovery(ProgramRecoveryOrigin Origin, int SourceSeat, int TargetSeat, int RequestedAmount)
{
    public long? NativeFrameId { get; init; }
    public ProgramOperationRecoveryResult? Result { get; init; }

    internal bool BelongsTo(ProgramSkillFrame owner) =>
        Origin == new ProgramRecoveryOrigin(owner.Id, owner.InstructionIndex, owner.GameplayHash);
}

public sealed record ProgramOperationRecoveryResult(int OriginalRecovered, int? RedirectedSeat = null, int RedirectedRecovered = 0);

public sealed partial record ProgramSkillFrame
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public ProgramOperationRecovery? OperationRecovery { get; init; }
}

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
