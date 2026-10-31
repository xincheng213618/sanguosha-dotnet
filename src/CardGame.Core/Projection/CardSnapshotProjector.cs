namespace CardGame.Core;

internal interface ICardSnapshotFacts : IProgramSelectionQuery
{
    EquipmentSlot? BoneSlot(Card card);
    int? BoneOwnerSeat(int cardId);
    bool IsCarrier(int cardId);
    string? VisibleSkillMark(int cardId);
    CardKind? QinJudgmentSlot(int cardId);
    bool IsQinJudgmentCard(int cardId);
    CardKind JudgmentEffectiveKind(Card card);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class CardSnapshotProjector(ICardSnapshotFacts facts, ICardSnapshotCache cache)
    : IProgramCardSnapshotQueries
{
    public CardSnapshot Snapshot(Card card)
    {
        var bone = facts.BoneSlot(card);
        var kind = bone is not null ? CardSnapshot.UndisclosedKind : card.Kind;
        var displayName = bone is not null ? "骨" : card.DisplayName;
        var equipmentSlot = bone;
        var rankText = facts.IsCarrier(card.Id) ? "—" : card.RankText;
        var skillMarkLabel = facts.VisibleSkillMark(card.Id);
        var qinJudgmentSlot = facts.QinJudgmentSlot(card.Id);
        if (card.Id > 0 && cache.TryGet(card.Id, out var existing) &&
            existing.Id == card.Id && existing.Kind == kind && existing.Suit == card.Suit &&
            existing.Rank == card.Rank && existing.DisplayName == displayName && existing.RankText == rankText && existing.SkillMarkLabel == skillMarkLabel && existing.QinJudgmentSlot == qinJudgmentSlot && existing.OccupiedEquipmentSlot == equipmentSlot && existing.IsBone == (bone is not null))
            return existing;

        var snapshot = new CardSnapshot(card.Id, kind, card.Suit, card.Rank, displayName, rankText)
        { SkillMarkLabel = skillMarkLabel, QinJudgmentSlot = qinJudgmentSlot, OccupiedEquipmentSlot = equipmentSlot, IsBone = bone is not null };
        if (card.Id > 0) cache.Store(card.Id, snapshot);
        return snapshot;
    }

    public CardSnapshot EquipmentSnapshot(Card card, int viewerSeat, bool revealAll)
    {
        var snapshot = Snapshot(card);
        return snapshot.IsBone && (revealAll || facts.BoneOwnerSeat(card.Id) == viewerSeat)
            ? snapshot with { Kind = card.Kind, DisplayName = card.DisplayName, SkillMarkLabel = "骨" }
            : snapshot;
    }

    public CardSnapshot JudgmentSnapshot(Card card)
    {
        if (facts.IsQinJudgmentCard(card.Id)) return Snapshot(card);
        var effectiveKind = facts.JudgmentEffectiveKind(card);
        return new CardSnapshot(
            card.Id,
            effectiveKind,
            card.Suit,
            card.Rank,
            CardCatalog.Get(effectiveKind).DisplayName,
            facts.IsCarrier(card.Id) ? "—" : card.RankText) { SkillMarkLabel = facts.VisibleSkillMark(card.Id) };
    }
}
