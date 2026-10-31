namespace CardGame.Core;

public sealed record GuanduAllCardBanIssuedEvent(long ProgramFrameId, int OwnerSeat, int TargetSeat, int ActualTurn,
    CardUseEffectSource Source) : IGameEvent;
