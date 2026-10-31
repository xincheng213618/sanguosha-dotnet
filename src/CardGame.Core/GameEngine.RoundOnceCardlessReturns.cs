namespace CardGame.Core;

public sealed record RoundOnceCardlessRequestedOwnerDamagedEvent(
    long ProgramFrameId, long NativeUseId, long NativeActionId, int ActualSourceSeat, int TargetSeat, int Amount) : IGameEvent;
