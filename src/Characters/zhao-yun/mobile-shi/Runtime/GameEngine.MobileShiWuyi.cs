namespace CardGame.Core;

public sealed record MobileShiWuyiBasicNameRecordedEvent(int OwnerSeat, CardKind Name, long ActionId) : IGameEvent;
