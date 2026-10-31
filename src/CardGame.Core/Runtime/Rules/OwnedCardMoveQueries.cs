namespace CardGame.Core;

internal interface IOwnedCardMoveQueries : IProgramSelectionQuery
{
    bool IsSelfHandCategoryDiscardForbidden(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent);
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent);
}

// These individual rule owners still live on the legacy engine. This temporary
// fact bridge must migrate with those owners; it does not decide the shared
// eligibility order or expose a character-specific selection facade.
internal interface IOwnedCardMoveLegacyFacts : IProgramSelectionQuery
{
    bool UnupgradedCartDiscardForbidden(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent);
    bool ChainedHandDiscardForbidden(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent);
    bool GiftedHandDiscardForbidden(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent);
    bool TurnHandCategoryRestricted(int ownerSeat, Card card);
    bool Alive(int seat);
    bool HasEquipmentDiscardProtection(int ownerSeat);
}

// Intent and actor belong to the actual operation. These live queries preserve
// both self-hand restrictions and the protected foreign armor/treasure policy.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class OwnedCardMoveQueries(ICardZoneQueries cards, IOwnedCardMoveLegacyFacts facts) : IOwnedCardMoveQueries
{
    public bool IsSelfHandCategoryDiscardForbidden(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent) =>
        facts.UnupgradedCartDiscardForbidden(actorSeat, card, from, intent) ||
        facts.ChainedHandDiscardForbidden(actorSeat, card, from, intent) ||
        facts.GiftedHandDiscardForbidden(actorSeat, card, from, intent) ||
        intent == OwnedCardMoveIntent.Discard && from == CardLocation.Hand(actorSeat) &&
        facts.TurnHandCategoryRestricted(actorSeat, card);

    public bool IsForeignEquipmentDiscardPrevented(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent)
    {
        if (IsSelfHandCategoryDiscardForbidden(actorSeat, card, from, intent)) return true;
        if (intent != OwnedCardMoveIntent.Discard || from.Zone != CardZoneKind.Equipment ||
            from.OwnerSeat is not { } ownerSeat || actorSeat == ownerSeat ||
            !facts.Alive(ownerSeat) || cards.GetLocation(card.Id) != from ||
            !EquipmentCatalog.IsEquipment(card.Kind) ||
            EquipmentCatalog.Get(card.Kind).Slot is not (EquipmentSlot.Armor or EquipmentSlot.Treasure)) return false;
        return facts.HasEquipmentDiscardProtection(ownerSeat);
    }
}
