namespace CardGame.Core;

public sealed record GuanduChariotCreatedEvent(long ProgramFrameId, int OwnerSeat, int CardId) : IGameEvent;
public sealed record GuanduChariotDestroyedEvent(int CardId, int MovementSequence, int FormerOwnerSeat) : IGameEvent;
