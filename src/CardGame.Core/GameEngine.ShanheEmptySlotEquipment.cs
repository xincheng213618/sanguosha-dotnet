namespace CardGame.Core;

public sealed record ShanheRandomEquipmentCreatedEvent(long ProgramFrameId, int OwnerSeat, string SkillId,
    int CardId, CardKind CardKind, Suit Suit, int Rank) : IGameEvent;
