using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class NativeMaterialClaimChecks
{
    public static void DeliveriesKeepOriginalOwnersAndRetirementBoundaries()
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        Need(ReferenceEquals(capabilities.Get<INativeMaterialClaims>(), capabilities.Get<ICardUseMaterialClaims>()),
            "Actual native delivery and the existing damage contract resolve to one owner and one frame store.");
        var prior = frames.Top;
        CheckDeliveryOwners(frames);
        CheckDamageResidency(frames);
        Need(ReferenceEquals(prior, frames.Top), "Material checks restore the original native stack.");
    }

    private static void CheckDeliveryOwners(IProgramRuntimeFrames frames)
    {
        var cards = Enumerable.Range(1, 7).Select(id => new Card(id, CardKind.Slash, Suit.Spade, id)).ToArray();
        var zones = new CardZoneStore(3);
        zones.LoadInitialDeck(cards);
        var journal = new List<CardMovementRecord>();
        var runtime = new CardUseMaterialClaimRuntime(frames, new CardMovementHistoryQueries(journal), zones);
        for (var index = 0; index < cards.Length; index++)
            Move(cards[index], index == 4 ? CardLocation.Judgment(1) : CardLocation.Processing);
        var use = new CardUseFrame(frames.NextId(), 0, cards[0].Id, CardKind.Slash, [1],
            PhysicalCardIds: [cards[0].Id, cards[1].Id, cards[2].Id, cards[6].Id])
            { NativeRepresentationIssuedAfterMovementSequence = journal.Count };
        var foreign = new CardUseFrame(frames.NextId(), 1, cards[3].Id, CardKind.Slash, [0], PhysicalCardIds: [cards[3].Id]);
        var judgment = new JudgmentFrame(frames.NextId(), 0, 1, "fixture.native-material", cards[5].Id,
            cards[5].Kind, cards[5].Suit, true) { DelayedCardId = cards[4].Id };
        frames.Push(use);
        frames.Push(foreign);
        frames.Push(judgment);
        try
        {
            runtime.CaptureDelivery(use.Id, []);
            Reject(() => runtime.CaptureDelivery(use.Id, [journal[0]]));
            Need(ReferenceEquals(use, frames.Find(use.Id)) && !runtime.HasRelinquishedMaterial(use.Id, cards[0].Id),
                "An empty or legally retained acquisition creates no false delivery or relinquished cleanup right.");
            var hand = Move(cards[0], CardLocation.Hand(1));
            var redirected = Move(cards[1], CardLocation.DrawPile);
            var outside = Move(cards[2], CardLocation.OutsideGame);
            runtime.CaptureDelivery(use.Id, [hand, redirected, outside]);
            var delivered = (CardUseFrame)frames.Find(use.Id)!;
            Need(delivered.Action is null && delivered.MaterialClaims.Count == 3 &&
                delivered.MaterialClaims.Single(claim => claim.CardId == hand.CardId).RecipientSeat == 1 &&
                delivered.MaterialClaims.Where(claim => claim.CardId != hand.CardId).All(claim => claim.RecipientSeat is null) &&
                new[] { hand, redirected, outside }.All(move => runtime.HasRelinquishedMaterial(use.Id, move.CardId) &&
                    runtime.CanReadMaterial(use.Id, move.CardId)),
                "Actual Hand, redirected DrawPile and OutsideGame deliveries retain original entities without inventing Hand recipients or Action identities.");
            runtime.CaptureDelivery(use.Id, [hand, redirected, outside]);
            Need(ReferenceEquals(delivered, frames.Find(use.Id)) && delivered.MaterialClaims is System.Collections.IList { IsReadOnly: true },
                "Repeating an actual delivery is idempotent and the one original-owner receipt list is immutable.");
            Reject(() => runtime.CaptureDelivery(foreign.Id, [hand]));
            Reject(() => runtime.CaptureDelivery(use.Id, [hand, hand]));
            Reject(() => runtime.CaptureDelivery(use.Id, [hand with { To = CardLocation.Hand(2) }]));
            Reject(() => runtime.CaptureDelivery(use.Id, [hand with { Sequence = int.MaxValue }]));
            Reject(() => runtime.CaptureDelivery(use.Id, [hand], (NativeMaterialClaimResidency)999));
            Reject(() => runtime.CaptureDelivery(use.Id, [hand], NativeMaterialClaimResidency.HandDrawDiscard));
            var unrelated = Move(cards[5], CardLocation.Hand(2));
            Reject(() => runtime.CaptureDelivery(use.Id, [unrelated]));
            Reject(() => runtime.CaptureDelivery(judgment.Id, [unrelated]));
            var noOp = new CardMovementRecord(journal.Count + 1, 0, cards[3].Id, cards[3].Kind,
                CardLocation.Processing, CardLocation.Processing, CardMoveReasons.Use);
            journal.Add(noOp);
            Reject(() => runtime.CaptureDelivery(foreign.Id, [noOp]));

            var additional = Move(cards[6], CardLocation.Hand(1));
            var conflicting = Move(cards[1], CardLocation.Hand(2));
            Reject(() => runtime.CaptureDelivery(use.Id, [additional, conflicting]));
            Need(ReferenceEquals(delivered, frames.Find(use.Id)) && !runtime.HasRelinquishedMaterial(use.Id, cards[6].Id),
                "A later conflicting delivery cannot replace an original receipt or partially publish another material in its batch.");
            Move(cards[0], CardLocation.Processing);
            Need(runtime.HasRelinquishedMaterial(use.Id, cards[0].Id) && runtime.CanReadMaterial(use.Id, cards[0].Id),
                "A genuinely relinquished original material never regains native retirement ownership when a later owner moves it back to Processing.");
            Move(cards[0], CardLocation.Equipment(2));
            Need(runtime.CanReadMaterial(use.Id, cards[0].Id) && !runtime.CanFinishClaimedMaterial(use.Id, cards[0].Id),
                "Any-residency delivery supports later equipment placement while the legacy damage view retains its narrower residency contract.");

            var judgmentDelivery = Move(cards[4], CardLocation.Hand(2));
            runtime.CaptureDelivery(judgment.Id, [judgmentDelivery]);
            var claimedJudgment = (JudgmentFrame)frames.Find(judgment.Id)!;
            Need(runtime.HasRelinquishedMaterial(judgment.Id, cards[4].Id) && runtime.CanReadMaterial(judgment.Id, cards[4].Id) &&
                claimedJudgment.MaterialClaims is [{ CardId: 5, RecipientSeat: 2 }],
                "Delayed judgment ownership follows DelayedCardId rather than the judgment's separate revealed card or a fabricated native Use.");
            frames.Replace(use.Id, JsonSerializer.Deserialize<CardUseFrame>(JsonSerializer.Serialize(delivered with { TargetIndex = 1 }))!);
            frames.Replace(judgment.Id, JsonSerializer.Deserialize<JudgmentFrame>(JsonSerializer.Serialize(claimedJudgment))!);
            Need(runtime.HasRelinquishedMaterial(use.Id, cards[0].Id) && runtime.CanReadMaterial(use.Id, cards[0].Id) &&
                runtime.HasRelinquishedMaterial(judgment.Id, cards[4].Id) && runtime.CanReadMaterial(judgment.Id, cards[4].Id),
                "Use continuations and ordinary JSON recovery preserve the same delivered facts on both actual native owners.");
        }
        finally
        {
            frames.Pop(judgment.Id, judgment.Kind);
            frames.Pop(foreign.Id, foreign.Kind);
            frames.Pop(use.Id, use.Kind);
        }

        CardMovementRecord Move(Card card, CardLocation to)
        {
            var from = zones.GetLocation(card.Id);
            zones.Move(card.Id, from, to);
            var move = new CardMovementRecord(journal.Count + 1, 0, card.Id, card.Kind, from, to,
                new("fixture.native-material-delivery"));
            journal.Add(move);
            return move;
        }
    }

    private static void CheckDamageResidency(IProgramRuntimeFrames frames)
    {
        var card = new Card(1, CardKind.Slash, Suit.Heart, 7);
        var zones = new CardZoneStore(2);
        zones.LoadInitialDeck([card]);
        zones.Move(card.Id, CardLocation.DrawPile, CardLocation.Processing);
        var journal = new List<CardMovementRecord>();
        var runtime = new CardUseMaterialClaimRuntime(frames, new CardMovementHistoryQueries(journal), zones);
        var use = new CardUseFrame(frames.NextId(), 0, card.Id, card.Kind, [1], PhysicalCardIds: [card.Id]);
        frames.Push(use);
        try
        {
            var reason = new CardMoveReason("fixture.native-damage-claim");
            Reject(() => runtime.Capture(use.Id, 1, [card], 0, reason));
            Move(CardLocation.Hand(1), reason);
            runtime.Capture(use.Id, 1, [card], 0, reason);
            Need(runtime.WasClaimed(use.Id, card.Id) && runtime.CanReadMaterial(use.Id, card.Id) &&
                runtime.CanFinishClaimedMaterial(use.Id, card.Id), "Damage capture retains its original exact Processing-to-Hand contract.");
            foreach (var to in new[] { CardLocation.DrawPile, CardLocation.DiscardPile, CardLocation.Equipment(1),
                         CardLocation.Judgment(1), CardLocation.OutsideGame, CardLocation.Processing })
            {
                Move(to, reason);
                var allowed = to.Zone is CardZoneKind.Hand or CardZoneKind.DrawPile or CardZoneKind.DiscardPile;
                Need(runtime.CanReadMaterial(use.Id, card.Id) == allowed && runtime.CanFinishClaimedMaterial(use.Id, card.Id) == allowed &&
                    runtime.HasRelinquishedMaterial(use.Id, card.Id),
                    "A damage claim's read/finish residency stays Hand/DrawPile/DiscardPile while its actual relinquished ownership remains a permanent fact.");
            }
            var legacy = JsonSerializer.Deserialize<CardUseFrame>(
                "{\"Id\":123,\"SourceSeat\":0,\"CardId\":1,\"CardKind\":0,\"TargetSeats\":[1],\"MaterialClaims\":[{\"CardId\":1,\"RecipientSeat\":1,\"MovementSequence\":1}]}")!;
            Need(legacy.MaterialClaims is [{ Residency: NativeMaterialClaimResidency.HandDrawDiscard }],
                "Existing claim JSON without a residency field keeps the original damage residency rather than gaining unrestricted material access.");
        }
        finally { frames.Pop(use.Id, use.Kind); }

        void Move(CardLocation to, CardMoveReason reason)
        {
            var from = zones.GetLocation(card.Id);
            zones.Move(card.Id, from, to);
            journal.Add(new(journal.Count + 1, 0, card.Id, card.Kind, from, to, reason));
        }
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("An invalid native material delivery was accepted.");
    }
}
