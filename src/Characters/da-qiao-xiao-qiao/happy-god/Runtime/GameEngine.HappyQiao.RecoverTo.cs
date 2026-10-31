using System.Text.Json.Serialization;

namespace CardGame.Core;

// This is a real recovery intent. The numerical HP query receipts deliberately
// cannot satisfy it, and the eventual native recovery owns a fresh payment.
public sealed record HappyQiaoRecoverToOrigin(long ProgramFrameId, int InstructionIndex,
    string GameplayHash, int SourceSeat, int TargetSeat, int TargetHp,
    long HealthRequestId, HappyQiaoPool Pool, HappyQiaoDualHealth Before, int Amount);

public sealed record HappyQiaoRecoverToIntent(long ProgramFrameId, int InstructionIndex,
    string GameplayHash, int SourceSeat, int TargetSeat, int TargetHp,
    long HealthRequestId, HappyQiaoDualHealth Before, long? RecoveryAttemptId = null, bool FinishedWithoutRecovery = false);

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HappyQiaoRecoverToIntent? HappyQiaoRecoverTo { get; init; }
}

public sealed partial record RecoveryAttempt
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HappyQiaoRecoverToOrigin? HappyQiaoRecoverToOrigin { get; init; }
}

public sealed record HappyQiaoRecoverToQueuedEvent(long ProgramFrameId, long HealthRequestId,
    long RecoveryAttemptId, int SourceSeat, int TargetSeat, int TargetHp, HappyQiaoPool Pool, int RequestedAmount) : IGameEvent;
public sealed record HappyQiaoRecoverToNoRecoveryEvent(long ProgramFrameId, long HealthRequestId,
    int TargetSeat, int TargetHp, HappyQiaoPool Pool) : IGameEvent;
