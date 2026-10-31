namespace CardGame.Core;

// 断发 settlement evidence: the recycled card ids are public once discarded,
// and the drawn count prices the phase ledger.
public sealed record ProgramDuanfaRecycledEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int TurnNumber, IReadOnlyList<int> DiscardedCardIds, int DrawnCount) : IGameEvent;

// 诱敌 settlement evidence: the discarded bait card is public; the taken
// counterpart card stays opaque (only whether a hand card was taken) and the
// draw count is public.
public sealed record ProgramYoudiBaitEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int ChooserSeat, int DiscardedCardId, bool TookHandCard, int DrawnCount) : IGameEvent;
