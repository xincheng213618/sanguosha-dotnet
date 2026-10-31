namespace CardGame.Core;

public sealed record WushuangShopEquipmentCreatedEvent(int Seat, string OfferId, int SourceOfferId, int CardId) : IGameEvent;
public sealed record WushuangShopEquipmentDestroyedEvent(int CardId, int MovementSequence, long ParentFrameId) : IGameEvent;
