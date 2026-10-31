namespace CardGame.Core;

public sealed record ActualDiscardRecoveryPhaseRestoredEvent(long ProducerFrameId, ActualDiscardRecoveryPhaseKey Phase) : IGameEvent;
public sealed record ActualDiscardRecoveryPhaseClearedEvent(int TurnNumber) : IGameEvent;
