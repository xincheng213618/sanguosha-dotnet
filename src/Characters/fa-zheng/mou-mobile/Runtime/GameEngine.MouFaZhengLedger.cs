namespace CardGame.Core;

public sealed record MouFaZhengActualGainEvent(long MarkId, long BatchId, long Sequence, int SourceSeat, int RecipientSeat) : IGameEvent;
