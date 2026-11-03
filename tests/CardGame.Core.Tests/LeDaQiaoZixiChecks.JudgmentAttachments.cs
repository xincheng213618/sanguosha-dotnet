using CardGame.Core;

namespace CardGame.Core.Tests;

internal static partial class LeDaQiaoZixiChecks
{
    private static void JudgmentAttachmentsFollowActualEntityResidency()
    {
        var pending = new List<IGameEvent>();
        var history = new ProgramEventHistoryIndex([], pending);
        var movements = new List<CardMovementRecord>();
        var zones = new CardZoneStore(3);
        var originals = new[]
        {
            new Card(7101, CardKind.Slash, Suit.Spade, 7),
            new Card(7102, CardKind.Dodge, Suit.Heart, 8),
            new Card(7103, CardKind.Crossbow, Suit.Club, 9),
            new Card(7104, CardKind.Peach, Suit.Diamond, 10)
        };
        zones.LoadInitialDeck(originals);
        var runtime = new JudgmentAttachmentRuntime(history, new CardMovementHistoryQueries(movements),
            zones, new AttachmentJournalEvents(history));
        var source = new CardConversionSource("fixture:judgment-attachment", "place", 0, "attachment-source");
        var slots = new[] { CardKind.Indulgence, CardKind.SupplyShortage, CardKind.Lightning };
        Need(runtime.Slot(0) is null && runtime.Slot(-1) is null && runtime.Slot(99999) is null && runtime.Slot(originals[0].Id) is null,
            "An unattached host-only or physical entity has no invented occupied judgment slot.");

        for (var i = 0; i < slots.Length; i++)
        {
            var arrival = Move(originals[i].Id, CardLocation.Judgment(1));
            runtime.Attach(8100 + i, source, slots[i], arrival);
            runtime.Attach(8100 + i, source, slots[i], arrival);
            var attached = history.Read<JudgmentCardAttachedEvent>().Single(e => e.CardId == originals[i].Id);
            Need(runtime.Slot(originals[i].Id) == slots[i] &&
                    zones.CardsAt(CardLocation.Judgment(1)).Single(c => c.Id == originals[i].Id) == originals[i] &&
                    attached == new JudgmentCardAttachedEvent(8100 + i, source, originals[i].Id, slots[i], arrival.Sequence),
                "Each of the three slots attaches to its actual arrival once without changing that entity's printed face.");
        }

        var first = originals[0].Id;
        var transfer = Move(first, CardLocation.Judgment(2));
        Need(runtime.Slot(first) == CardKind.Indulgence,
            "A direct Judgment-to-Judgment transfer carries the original attached slot to the actual new holder.");
        Reject(() => runtime.Attach(8200, source, CardKind.Lightning, transfer));
        Move(first, CardLocation.Hand(2));
        Need(runtime.Slot(first) is null, "Leaving Judgment ends the attachment even while the entity remains in the game.");
        var reentry = Move(first, CardLocation.Judgment(1));
        Need(runtime.Slot(first) is null,
            "Returning the same entity from Hand does not revive a terminated attachment.");
        runtime.Attach(8201, source, CardKind.Lightning, reentry);
        Need(runtime.Slot(first) == CardKind.Lightning &&
                zones.CardsAt(CardLocation.Judgment(1)).Single(c => c.Id == first).Kind == CardKind.Slash &&
                history.Read<JudgmentCardAttachedEvent>().Count(e => e.CardId == first) == 2,
            "A later explicit attachment can assign another slot to the new real residency without replacing the physical Slash.");

        var unplaced = Move(originals[3].Id, CardLocation.Hand(0));
        Reject(() => runtime.Attach(8300, source, CardKind.Indulgence, unplaced));
        var actual = Move(originals[3].Id, CardLocation.Judgment(1));
        Reject(() => runtime.Attach(8300, source, CardKind.Slash, actual));
        Reject(() => runtime.Attach(0, source, CardKind.Indulgence, actual));
        Reject(() => runtime.Attach(8300, source, CardKind.Indulgence, actual with { Sequence = actual.Sequence + 100 }));
        Reject(() => runtime.Attach(8300, source, CardKind.Indulgence, actual with { From = actual.To }));
        Reject(() => runtime.Attach(8300, source, CardKind.Indulgence, actual with { To = CardLocation.Judgment(2) }));
        runtime.Attach(8300, source, CardKind.Indulgence, actual);
        Reject(() => runtime.Attach(8301, source, CardKind.Indulgence, actual));
        Reject(() => runtime.Attach(8300, source with { SkillInstanceId = "another-instance" }, CardKind.Indulgence, actual));
        Reject(() => runtime.Attach(8300, source, CardKind.SupplyShortage, actual));
        Move(originals[3].Id, CardLocation.Hand(0));
        Reject(() => runtime.Attach(8300, source, CardKind.Indulgence, actual));
        Need(history.Read<JudgmentCardAttachedEvent>().Count == 5 && runtime.Slot(originals[3].Id) is null,
            "Rejected non-Judgment, fabricated, stale and changed-source writes cannot issue another attachment or revive one.");

        // This reconstructs the shared facts and physical state, not a command replay.
        var rebuilt = new JudgmentAttachmentRuntime(new ProgramEventHistoryIndex([], pending.ToList()),
            new CardMovementHistoryQueries(movements.ToArray()), zones, new AttachmentJournalEvents(history));
        Need(originals.All(c => rebuilt.Slot(c.Id) == runtime.Slot(c.Id)),
            "The common answer rebuilds from the event journal and actual physical movements without an auxiliary slot table.");

        CardMovementRecord Move(int id, CardLocation destination)
        {
            var from = zones.GetLocation(id);
            var card = zones.Move(id, from, destination);
            var movement = new CardMovementRecord(movements.Count + 1, 1, id, card.Kind, from, destination,
                new CardMoveReason("fixture.judgment-attachment"));
            movements.Add(movement);
            return movement;
        }

        void Reject(Action write)
        {
            var count = history.Read<JudgmentCardAttachedEvent>().Count;
            var rejected = false;
            try { write(); }
            catch (InvalidOperationException) { rejected = true; }
            Need(rejected && history.Read<JudgmentCardAttachedEvent>().Count == count,
                "The shared write boundary rejects an invalid attachment without publishing a fact.");
        }

        static void Need(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }
    }

    private sealed class AttachmentJournalEvents(ProgramEventHistoryIndex history) : IProgramRuntimeEvents
    {
        public void AdvanceFact(IGameEvent fact) => history.Append(fact);
    }
}
