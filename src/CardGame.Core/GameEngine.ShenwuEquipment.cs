namespace CardGame.Core;

public sealed record ShenwuEquipmentInitializedEvent(int SelectedGodCount) : IGameEvent;
