namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    private static IEnumerable<CardSnapshot> VisibleOriginalPublicPileCards(PlayerSnapshot self) =>
        (self.PublicPersistentPiles?.SelectMany(pile => pile.Cards) ?? [])
            .Concat(self.PublicPersistentPileCards ?? []).DistinctBy(card => card.Id);

    private static IEnumerable<CardSnapshot> VisibleBingPileCards(PlayerSnapshot self, CardConversionSource source) =>
        self.PublicPersistentPiles is { } piles
            ? piles.Where(pile => pile.OwnerSeat == self.Seat && pile.SourceSkillId == source.SkillId &&
                pile.SourceSkillInstanceId == source.SkillInstanceId).SelectMany(pile => pile.Cards)
            // The scalar view is published only for one unambiguous public
            // source. It has no instance field; the issued legal action retains
            // that identity and the native payment validates it on submission.
            : self.PublicPersistentPileSkillId == source.SkillId ? self.PublicPersistentPileCards ?? [] : [];

    private CardSnapshot AiActionPhysicalCard(GameSnapshot view, PlayerSnapshot self, LegalAction action) =>
        self.Hand.Concat(self.WoodenOxGrain ?? []).Concat(self.Equipment).Concat(self.AuthorityCards ?? [])
            .Concat(_includeJudgmentMaterials ? self.Judgment : [])
            .Concat(action.NativePublicPileSource is { } source && source.OwnerSeat == self.Seat
                ? VisibleOriginalPublicPileCards(self) : [])
            // Jibing publishes a conversion of an existing entity in its own
            // exact source-instance pile, rather than an original Hand use.
            .Concat(action.Kind == LegalActionKind.Slash && action.CardId is > 0 &&
                action.ConversionSource is { SkillId: "ol:jibing", BindingId: "bing-slash" } bingSource &&
                bingSource.OwnerSeat == self.Seat
                    ? VisibleBingPileCards(self, bingSource)
                    : [])
            // Shouli publishes one real field-mount entity as its exact cost.
            // Its source permits only this public material pool, never a hand.
            .Concat(action.Kind == LegalActionKind.Slash && action.CardId is > 0 &&
                action.ConversionSource is
                    { SkillId: ShenMaChaoComposition.Shouli, BindingId: ShenMaChaoComposition.Slash } fieldSource &&
                fieldSource.OwnerSeat == self.Seat
                    ? view.Players.Where(player => player.IsAlive)
                        .SelectMany(player => player.Equipment.Concat(player.Judgment))
                        .Where(card => EquipmentCatalog.IsEquipment(card.Kind) &&
                            EquipmentCatalog.Get(card.Kind).Slot == EquipmentSlot.OffensiveHorse)
                    : [])
            .DistinctBy(card => card.Id).Single(card => card.Id == action.CardId);
}
