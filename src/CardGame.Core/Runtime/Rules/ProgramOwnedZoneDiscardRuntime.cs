namespace CardGame.Core;

// Synchronous whole-zone discard keeps the original frame and native invoices.
// Selection, actual physical results and completion order live in this module.
[ProgramRuntimeModule]
internal sealed class ProgramOwnedZoneDiscardRuntime(IProgramRuntimeFrames frames, ICardZoneQueries cards,
    IMatchParticipantStateAccess participants, IOwnedCardMoveQueries legality,
    IProgramBoundCardMovementOperations movement, IProgramRuntimeEvents events,
    IProgramOwnedZoneDiscardFacts facts, IProgramOwnedZoneDiscardReceipts receipts,
    INativeLogicalDiscardOperations logicalDiscards)
{
    internal void Discard(
        ProgramSkillFrame frame,
        IReadOnlyList<CardZoneKind> zones,
        CardMoveReason reason)
    {
        var active = frames.ActiveProgram(frame.Id);
        if (active.OwnerSeat != frame.OwnerSeat || active.SkillId != frame.SkillId ||
            active.ActivationId != frame.ActivationId)
        {
            throw new InvalidOperationException("Owned-zone discard requires the active program binding.");
        }

        var owner = participants.At(frame.OwnerSeat);
        var nativeDiscard = receipts.Capture(active, zones, reason);
        var logicalDiscard = logicalDiscards.Capture(frame.OwnerSeat, frame.OwnerSeat,
            zones.SelectMany(zone => cards.CardsAt(new(zone, owner.Seat))).Select(card => card.Id).ToArray(),
            reason, frame.Id);
        List<int>? actualDiscardIds = nativeDiscard is null ? null : [];
        var discarded = 0;
        if (facts.HasAtomicDiscardReplacement)
        {
            var ids = zones.SelectMany(zone => cards.CardsAt(new(zone, owner.Seat))
                .Where(card => !legality.IsSelfHandCategoryDiscardForbidden(owner.Seat, card, new(zone, owner.Seat), OwnedCardMoveIntent.Discard)))
                .Select(card => card.Id).ToArray();
            movement.MoveMultipleSources(ids, CardLocation.DiscardPile, reason, CardMovementDisclosure.FaceUp, (_, records) =>
            {
                var actual = records.Where(move => move.To == CardLocation.DiscardPile ||
                    move.CardKind == CardKind.ThunderChariot && move.To == CardLocation.OutsideGame &&
                    facts.DiscardSource(move) == move.From).Select(move => move.CardId).ToArray();
                actualDiscardIds?.AddRange(actual);
                discarded = actual.Length;
            });
        }
        else foreach (var zone in zones)
        {
            var (zoneCards, location) = zone switch
            {
                CardZoneKind.Hand => (cards.CardsAt(CardLocation.Hand(owner.Seat)).ToArray(), CardLocation.Hand(owner.Seat)),
                CardZoneKind.Equipment => (cards.CardsAt(CardLocation.Equipment(owner.Seat)).ToArray(), CardLocation.Equipment(owner.Seat)),
                CardZoneKind.Judgment => (cards.CardsAt(CardLocation.Judgment(owner.Seat)).ToArray(), CardLocation.Judgment(owner.Seat)),
                _ => throw new InvalidOperationException($"Unsupported owned discard zone '{zone}'.")
            };
            foreach (var card in zoneCards)
            {
                if (legality.IsSelfHandCategoryDiscardForbidden(owner.Seat, card, location, OwnedCardMoveIntent.Discard)) continue;
                movement.Move(card, location, CardLocation.DiscardPile, reason, disclosure: CardMovementDisclosure.FaceUp);
                actualDiscardIds?.Add(card.Id);
                discarded++;
            }
        }

        receipts.Complete(nativeDiscard, actualDiscardIds ?? []);
        logicalDiscards.Complete(logicalDiscard);
        events.AdvanceFact(new ProgramOwnedZoneCardsDiscardedEvent(
            frame.Id,
            frame.SkillId,
            RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame),
            frame.OwnerSeat,
            Array.AsReadOnly(zones.ToArray()),
            discarded));
    }

}
