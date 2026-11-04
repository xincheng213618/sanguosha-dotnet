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

    public static void AcceptedClaimsAndNativeHandoffsKeepOriginalOwnership()
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var prior = frames.Top;
        CheckAcceptedClaims(frames);
        CheckNativeHandoffs(frames);
        Need(ReferenceEquals(prior, frames.Top), "Accepted dispositions and native handoffs restore their original frame stack.");
    }

    private static void CheckAcceptedClaims(IProgramRuntimeFrames frames)
    {
        var cards = Enumerable.Range(1, 4).Select(id => new Card(id, CardKind.Slash, Suit.Heart, id)).ToArray();
        var zones = new CardZoneStore(2);
        zones.LoadInitialDeck(cards);
        var journal = new List<CardMovementRecord>();
        for (var index = 0; index < cards.Length; index++) Move(cards[index], CardLocation.Processing);
        var runtime = new CardUseMaterialClaimRuntime(frames, new CardMovementHistoryQueries(journal), zones);
        var use = new CardUseFrame(frames.NextId(), 0, cards[0].Id, CardKind.Slash, [1], PhysicalCardIds: [1, 2, 3]);
        var producer = new ProgramSkillFrame(frames.NextId(), 0, "fixture:material-acceptance", "run", "hash", 1, [], []);
        frames.Push(use);
        frames.Push(producer);
        try
        {
            runtime.DeclareRetainedMaterial(use.Id, 3);
            Need(!runtime.CanReadMaterial(use.Id, 3) && !runtime.HasRelinquishedMaterial(use.Id, 3) &&
                !runtime.PreservesMaterialAtCompletion(use.Id, 3),
                "An issued retention permits only outside-Processing reads and leaves native Processing retirement intact.");
            Need(runtime.AcceptClaim(use.Id, producer.Id, 0, [1, 2]) &&
                !runtime.AcceptClaim(use.Id, producer.Id, 0, [2, 1]) &&
                !runtime.AcceptClaim(use.Id, producer.Id, 1, []),
                "One original producer instruction accepts its complete material set once; repeated order and empty results create no second commitment.");
            var accepted = (CardUseFrame)frames.Find(use.Id)!;
            Need(runtime.HasRelinquishedMaterial(use.Id, 1) && runtime.CanReadMaterial(use.Id, 1) &&
                !runtime.WasClaimed(use.Id, 1) && runtime.ClaimedCardIds(use.Id).Count == 0 &&
                accepted.MaterialClaims.Where(claim => claim.Kind == NativeMaterialClaimKind.AcceptedClaim).All(claim => claim.MovementSequence == 0),
                "Accepted material ownership precedes physical movement without inventing an actual delivery.");
            Reject(() => runtime.AcceptClaim(use.Id, producer.Id, 0, [1]));
            Reject(() => runtime.AcceptClaim(use.Id, producer.Id, 1, [1]));
            Reject(() => runtime.AcceptClaim(use.Id, producer.Id, 1, [4]));
            Reject(() => runtime.AcceptClaim(use.Id, long.MaxValue, 1, [3]));
            Need(ReferenceEquals(accepted, frames.Find(use.Id)), "Invalid or conflicting acceptance does not publish partial ownership.");
            var delivery = Move(cards[0], CardLocation.Hand(1));
            runtime.CaptureDelivery(use.Id, [delivery]);
            Need(runtime.WasClaimed(use.Id, 1) && runtime.ClaimedCardIds(use.Id).SequenceEqual([1]) &&
                ((CardUseFrame)frames.Find(use.Id)!).MaterialClaims.Single(claim => claim.CardId == 1) is
                    { Kind: NativeMaterialClaimKind.Delivery, ProducerFrameId: var issuer, InstructionIndex: 0 } && issuer == producer.Id &&
                !runtime.AcceptClaim(use.Id, producer.Id, 0, [1, 2]),
                "A real delivery upgrades its original accepted commitment, keeps its issuer key and remains idempotent for the complete issued set.");

            var before = journal.Count;
            var reason = new CardMoveReason("fixture.retained-damage-material");
            Move(cards[2], CardLocation.Hand(1), reason);
            runtime.Capture(use.Id, 1, [cards[2]], before, reason);
            Move(cards[2], CardLocation.Equipment(1));
            Need(runtime.CanReadMaterial(use.Id, 3) && runtime.HasRelinquishedMaterial(use.Id, 3) && runtime.WasClaimed(use.Id, 3) &&
                !runtime.CanFinishClaimedMaterial(use.Id, 3) &&
                ((CardUseFrame)frames.Find(use.Id)!).MaterialClaims.Count(claim => claim.CardId == 3) == 2,
                "Independent retained read permission survives a later narrow damage delivery; one table keeps both facts without widening the old damage finish contract.");
            frames.Replace(use.Id, JsonSerializer.Deserialize<CardUseFrame>(JsonSerializer.Serialize((CardUseFrame)frames.Find(use.Id)!))!);
            Need(runtime.CanReadMaterial(use.Id, 3) && runtime.HasRelinquishedMaterial(use.Id, 2) &&
                !runtime.WasClaimed(use.Id, 2) && runtime.ClaimedCardIds(use.Id).SequenceEqual([1, 3]) &&
                !runtime.AcceptClaim(use.Id, producer.Id, 0, [1, 2]),
                "Ordinary recovery retains independent issued and delivered facts without promoting pending commitments to physical claims.");
        }
        finally
        {
            frames.Pop(producer.Id, producer.Kind);
            frames.Pop(use.Id, use.Kind);
        }
        CardMovementRecord Move(Card card, CardLocation to, CardMoveReason? reason = null)
        {
            var from = zones.GetLocation(card.Id);
            zones.Move(card.Id, from, to);
            var move = new CardMovementRecord(journal.Count + 1, 0, card.Id, card.Kind, from, to,
                reason ?? new("fixture.accepted-material"));
            journal.Add(move);
            return move;
        }
    }

    private static void CheckNativeHandoffs(IProgramRuntimeFrames frames)
    {
        var cards = Enumerable.Range(1, 5).Select(id => new Card(id, CardKind.Slash, Suit.Club, id)).ToArray();
        var zones = new CardZoneStore(2);
        zones.LoadInitialDeck(cards);
        var journal = new List<CardMovementRecord>();
        foreach (var card in cards) Move(card, CardLocation.Processing);
        var runtime = new CardUseMaterialClaimRuntime(frames, new CardMovementHistoryQueries(journal), zones);
        var older = Child([1]);
        frames.Push(older);
        var use = new CardUseFrame(frames.NextId(), 0, cards[0].Id, CardKind.Slash, [1], PhysicalCardIds: [1, 2, 3, 4, 5]);
        var producer = new ProgramSkillFrame(frames.NextId(), 0, "fixture:material-handoff", "run", "hash", 1, [], []);
        frames.Push(use);
        frames.Push(producer);
        try
        {
            runtime.CompleteNativeChild(older);
            Need(ReferenceEquals(use, frames.Find(use.Id)),
                "Unrelated native returns with no pending handoff add no global completion prerequisite.");
            Reject(() => runtime.TransferToNativeUse(use.Id, older.Id, [1], NativeMaterialHandoffMode.Issued));
            var wrongSource = Child([1], CardLocation.Hand(0));
            frames.Push(wrongSource);
            try { Reject(() => runtime.TransferToNativeUse(use.Id, wrongSource.Id, [1], NativeMaterialHandoffMode.Issued)); }
            finally { frames.Pop(wrongSource.Id, wrongSource.Kind); }

            var issued = Child([1, 2]);
            frames.Push(issued);
            try
            {
                runtime.TransferToNativeUse(use.Id, issued.Id, [1, 2], NativeMaterialHandoffMode.Issued);
                var promised = (CardUseFrame)frames.Find(use.Id)!;
                runtime.TransferToNativeUse(use.Id, issued.Id, [2, 1], NativeMaterialHandoffMode.Issued);
                Need(ReferenceEquals(promised, frames.Find(use.Id)) && runtime.HasRelinquishedMaterial(use.Id, 1) &&
                    runtime.CanReadMaterial(use.Id, 1) && !runtime.WasClaimed(use.Id, 1) &&
                    promised.MaterialClaims.All(claim => claim.MovementSequence == 0 && claim.OriginalEntrySequence > 0),
                    "A real native Use transfers its committed Processing ownership at issuance without a fake Processing-to-Processing movement or Hand acquisition.");
                Reject(() => runtime.TransferToNativeUse(use.Id, issued.Id, [1], NativeMaterialHandoffMode.Issued));
                issued = issued with { Step = ResolutionFrameStep.Completed };
                frames.Replace(issued.Id, issued);
                runtime.CompleteNativeChild(issued);
            }
            finally { frames.Pop(issued.Id, issued.Kind); }
            Need(runtime.HasRelinquishedMaterial(use.Id, 1) && runtime.CanReadMaterial(use.Id, 1) && !runtime.WasClaimed(use.Id, 1),
                "Issued native ownership remains transferred after the child returns, independently of a physical delivery or continuing source.");

            var pending = Child([3]);
            frames.Push(pending);
            try
            {
                Reject(() => runtime.TransferToNativeUse(use.Id, pending.Id, [3], (NativeMaterialHandoffMode)999));
                frames.Replace(pending.Id, pending with { NativeRepresentationIssuedAfterMovementSequence = 0 });
                Reject(() => runtime.TransferToNativeUse(use.Id, pending.Id, [3], NativeMaterialHandoffMode.UntilDelivery));
                frames.Replace(pending.Id, pending);
                runtime.TransferToNativeUse(use.Id, pending.Id, [3], NativeMaterialHandoffMode.UntilDelivery);
                Need(runtime.HasRelinquishedMaterial(use.Id, 3) && runtime.CanReadMaterial(use.Id, 3) && !runtime.WasClaimed(use.Id, 3),
                    "An actual pending child temporarily owns its original Processing material without becoming an actual delivery.");
                frames.Replace(pending.Id, pending with { PhysicalCardIds = [4] });
                Need(!runtime.HasRelinquishedMaterial(use.Id, 3) && !runtime.CanReadMaterial(use.Id, 3),
                    "A reservation reads the real child's committed material rather than a source or historical proof ledger.");
                pending = pending with { Step = ResolutionFrameStep.Completed };
                frames.Replace(pending.Id, JsonSerializer.Deserialize<CardUseFrame>(JsonSerializer.Serialize(pending))!);
                frames.Replace(use.Id, JsonSerializer.Deserialize<CardUseFrame>(JsonSerializer.Serialize((CardUseFrame)frames.Find(use.Id)!))!);
                Need(runtime.HasRelinquishedMaterial(use.Id, 3), "The issued native child relation survives ordinary recovery.");
                runtime.CompleteNativeChild(pending);
                Need(!runtime.HasRelinquishedMaterial(use.Id, 3) && !runtime.CanReadMaterial(use.Id, 3) && !runtime.WasClaimed(use.Id, 3),
                    "A returned child with no actual departure releases its temporary reservation and restores original native retirement ownership.");
            }
            finally { frames.Pop(pending.Id, pending.Kind); }

            var intermediate = new CardUseFrame(frames.NextId(), 0, 4, CardKind.Slash, [1], PhysicalCardIds: [4, 5]);
            frames.Push(intermediate);
            var delivered = Child([4, 5]);
            frames.Push(delivered);
            try
            {
                runtime.TransferToNativeUse(use.Id, delivered.Id, [4, 5], NativeMaterialHandoffMode.UntilDelivery);
                runtime.TransferToNativeUse(intermediate.Id, delivered.Id, [4, 5], NativeMaterialHandoffMode.UntilDelivery);
                var entry = Move(cards[3], CardLocation.Equipment(1));
                var terminal = Move(cards[4], CardLocation.DiscardPile);
                var before = frames.Find(use.Id);
                frames.Replace(delivered.Id, delivered with { Step = ResolutionFrameStep.Completed, PhysicalCardIds = [4] });
                Reject(() => runtime.CompleteNativeChild(delivered with { Step = ResolutionFrameStep.Completed }));
                Need(ReferenceEquals(before, frames.Find(use.Id)) && !runtime.WasClaimed(use.Id, 4),
                    "A malformed second child material rejects the whole settlement before a valid first material is published.");
                delivered = delivered with { Step = ResolutionFrameStep.Completed };
                frames.Replace(delivered.Id, delivered);
                var intermediatePending = (CardUseFrame)frames.Find(intermediate.Id)!;
                var invalidPolicy = intermediatePending with { MaterialClaims = intermediatePending.MaterialClaims.Select(claim =>
                    claim.CardId == 5 ? claim with { Residency = NativeMaterialClaimResidency.HandDrawDiscard } : claim).ToArray() };
                frames.Replace(intermediate.Id, invalidPolicy);
                before = frames.Find(use.Id);
                Reject(() => runtime.CompleteNativeChild(delivered));
                Need(ReferenceEquals(before, frames.Find(use.Id)) && ReferenceEquals(invalidPolicy, frames.Find(intermediate.Id)),
                    "A later ancestor's conflicting delivery policy rejects all prepared ancestor settlements without partially committing an earlier owner.");
                frames.Replace(intermediate.Id, intermediatePending);
                runtime.CompleteNativeChild(delivered);
                Need(runtime.WasClaimed(use.Id, 4) && runtime.WasClaimed(use.Id, 5) &&
                    runtime.WasClaimed(intermediate.Id, 4) && runtime.WasClaimed(intermediate.Id, 5) &&
                    ((CardUseFrame)frames.Find(use.Id)!).MaterialClaims.Where(claim => claim.ChildUseId == delivered.Id)
                        .Select(claim => claim.MovementSequence).SequenceEqual([entry.Sequence, terminal.Sequence]),
                    "The real first native Equipment entry or terminal discard settles the original pending handoff as physical delivery.");
            }
            finally
            {
                frames.Pop(delivered.Id, delivered.Kind);
                frames.Pop(intermediate.Id, intermediate.Kind);
            }
            Move(cards[3], CardLocation.Processing);
            Need(runtime.HasRelinquishedMaterial(use.Id, 4) && runtime.CanReadMaterial(use.Id, 4),
                "A settled handoff remains relinquished after the child disappears and the entity later reenters Processing.");
        }
        finally
        {
            frames.Pop(producer.Id, producer.Kind);
            frames.Pop(use.Id, use.Kind);
            frames.Pop(older.Id, older.Kind);
        }

        CardUseFrame Child(IReadOnlyList<int> ids, CardLocation? from = null)
        {
            var id = frames.NextId();
            var action = new CardActionContext(id * 100, null, CardActionType.Use, 0, 0, null, null, null,
                CardKind.Slash, [1], ids.Select(cardId => new CardActionCost(cardId, cards[cardId - 1].Kind,
                    from ?? CardLocation.Processing)).ToArray(), []);
            return new(id, 0, ids[0], CardKind.Slash, [1], PhysicalCardIds: ids)
                { Action = action, NativeRepresentationIssuedAfterMovementSequence = journal.Count };
        }
        CardMovementRecord Move(Card card, CardLocation to)
        {
            var from = zones.GetLocation(card.Id);
            zones.Move(card.Id, from, to);
            var move = new CardMovementRecord(journal.Count + 1, 0, card.Id, card.Kind, from, to,
                new("fixture.native-material-handoff"));
            journal.Add(move);
            return move;
        }
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
                    runtime.HasRelinquishedMaterial(use.Id, card.Id) && runtime.WasClaimed(use.Id, card.Id) &&
                    runtime.PreservesMaterialAtCompletion(use.Id, card.Id) == (allowed || to == CardLocation.Processing),
                    "A damage claim preserves Hand/DrawPile/DiscardPile at completion and prevents reclaiming later Processing ownership, while Equipment/Judgment/Outside remain unsupported and actual delivery remains a permanent fact.");
            }
            var legacy = JsonSerializer.Deserialize<CardUseFrame>(
                "{\"Id\":123,\"SourceSeat\":0,\"CardId\":1,\"CardKind\":0,\"TargetSeats\":[1],\"MaterialClaims\":[{\"CardId\":1,\"RecipientSeat\":1,\"MovementSequence\":1}]}")!;
            Need(legacy.MaterialClaims is [{ Residency: NativeMaterialClaimResidency.HandDrawDiscard, Kind: NativeMaterialClaimKind.Delivery }],
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
