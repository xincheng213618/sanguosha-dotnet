namespace CardGame.Core;

public sealed record NativeCardRecoveryReceipt(long RecoveryFrameId, int SourceSeat, int TargetSeat,
    CardKind CardKind, int RequestedAmount, int HpBefore, int HpAfter);
public sealed record NativeCardRecoveryAppliedEvent(long CardUseFrameId, NativeCardRecoveryReceipt Receipt) : IGameEvent;
