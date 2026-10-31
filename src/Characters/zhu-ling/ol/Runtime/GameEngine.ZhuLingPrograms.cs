namespace CardGame.Core;

// 战意 evidence: the discarded category and the discarded entity cards are
// public; the buff split is reconstructible from the chosen category.
public sealed record ProgramZhanyiCategoryChosenEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, string Category, IReadOnlyList<int> DiscardedCardIds) : IGameEvent;
public sealed record ProgramZhanyiEquipmentPunishEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int TargetSeat, int CardId, bool FromHand) : IGameEvent;
