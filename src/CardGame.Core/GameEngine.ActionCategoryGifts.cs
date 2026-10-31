namespace CardGame.Core;

public sealed record ProgramActionCategoryGiftResolvedEvent(long FrameId, string SkillId, int OwnerSeat,
    int ProviderSeat, CardKind UsedKind, int? GiftCardId) : IGameEvent;
