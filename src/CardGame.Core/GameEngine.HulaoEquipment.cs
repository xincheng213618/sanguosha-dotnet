namespace CardGame.Core;

public sealed record HulaoInitialEquipmentPlacedEvent(int BossSeat) : IGameEvent;
