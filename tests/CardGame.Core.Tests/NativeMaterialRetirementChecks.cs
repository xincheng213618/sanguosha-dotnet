using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class NativeMaterialRetirementChecks
{
    public static void PaymentsAndDestructionKeepOriginalRetirementRights()
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var originalTop = frames.Top;
        CheckDirectPayments(frames);
        CheckProcessingDestruction(frames);
        CheckAlreadyProcessingMaterial(frames);
        CheckAutomaticNativeOwner(frames);
        Need(ReferenceEquals(originalTop, frames.Top), "Retirement checks restore the original native stack.");
    }

    private static void CheckDirectPayments(IProgramRuntimeFrames frames)
    {
        var fixture = new Materials(frames, 5);
        var use = fixture.Use([1, 2, 3, 4]);
        var foreign = fixture.Use([5]);
        frames.Push(use);
        frames.Push(foreign);
        try
        {
            var outside = fixture.Move(1, CardLocation.OutsideGame);
            var redirected = fixture.Move(2, CardLocation.DrawPile);
            var publicPile = new CardLocation(CardZoneKind.PublicPersistentPile, 0, "fixture:paid-materials");
            var stored = fixture.Move(3, publicPile);
            var processing = fixture.Move(4, CardLocation.Processing);
            var runtime = fixture.Runtime;
            var unchanged = frames.Find(use.Id);
            runtime.CaptureRetirement(use.Id, []);
            Reject(() => runtime.CaptureRetirement(use.Id, [outside, redirected with { To = CardLocation.Hand(1) }]));
            Reject(() => runtime.CaptureRetirement(use.Id, [outside, outside]));
            Reject(() => runtime.CaptureRetirement(use.Id, [outside with { Sequence = int.MaxValue }]));
            Reject(() => runtime.CaptureRetirement(use.Id, [fixture.Journal[0]]));
            Reject(() => runtime.CaptureRetirement(use.Id, [processing]));
            Reject(() => runtime.CaptureRetirement(foreign.Id, [outside]));
            Reject(() => runtime.CaptureRetirement(long.MaxValue, [outside]));
            Need(ReferenceEquals(unchanged, frames.Find(use.Id)),
                "A forged, stale, foreign or still-Processing payment rejects its whole retirement batch before publishing any material.");

            runtime.CaptureRetirement(use.Id, [outside, redirected, stored]);
            var paid = (CardUseFrame)frames.Find(use.Id)!;
            runtime.CaptureRetirement(use.Id, [stored, redirected, outside]);
            Need(ReferenceEquals(paid, frames.Find(use.Id)) && paid.MaterialClaims.Count == 3 &&
                paid.MaterialClaims.All(claim => claim.Kind == NativeMaterialClaimKind.Retirement &&
                    claim.RecipientSeat is null && claim.OriginalEntrySequence == claim.MovementSequence) &&
                paid.MaterialClaims is System.Collections.IList { IsReadOnly: true },
                "Real Outside, DrawPile and public-pile payments publish frozen idempotent retirement receipts without fabricating a recipient.");
            foreach (var id in new[] { 1, 2, 3 })
                Need(runtime.HasRelinquishedMaterial(use.Id, id) && runtime.PreservesMaterialAtCompletion(use.Id, id) &&
                    runtime.CanReadMaterial(use.Id, id) && !runtime.WasClaimed(use.Id, id),
                    "A paid material is readable at its exact destination and no longer belongs to the original native retirement, but was not acquired.");
            Need(runtime.ClaimedCardIds(use.Id).Count == 0 && !runtime.HasRelinquishedMaterial(use.Id, 4),
                "Retirement does not enter actual acquisition results, and ordinary Processing payment retains its native retirement owner.");

            frames.Replace(use.Id, JsonSerializer.Deserialize<CardUseFrame>(JsonSerializer.Serialize(paid with { TargetIndex = 1 }))!);
            Need(runtime.CanReadMaterial(use.Id, 3) && runtime.HasRelinquishedMaterial(use.Id, 1),
                "The original use keeps payment identities, destination reads and retirement rights through a continuation and JSON recovery.");
            fixture.Move(1, CardLocation.Hand(1));
            Need(!runtime.CanReadMaterial(use.Id, 1) && runtime.PreservesMaterialAtCompletion(use.Id, 1),
                "A later unrelated destination loses the payment's exact representation permission without giving its original owner retirement authority again.");
            fixture.Move(1, CardLocation.Processing);
            Need(runtime.HasRelinquishedMaterial(use.Id, 1) && runtime.PreservesMaterialAtCompletion(use.Id, 1) &&
                !runtime.CanReadMaterial(use.Id, 1),
                "Reentry into Processing cannot make the outer use retire an already-paid material a second time or widen its departure read.");
            var later = fixture.Move(1, CardLocation.OutsideGame);
            var before = frames.Find(use.Id);
            Reject(() => runtime.CaptureRetirement(use.Id, [later]));
            Need(ReferenceEquals(before, frames.Find(use.Id)),
                "An original paid material cannot replace its receipt with a later owner's physical departure.");
            var unrelated = fixture.Move(5, CardLocation.OutsideGame);
            Reject(() => runtime.CaptureRetirement(use.Id, [unrelated]));
            var judgment = new JudgmentFrame(frames.NextId(), 0, 1, "fixture.retirement-owner", 1,
                CardKind.Slash, Suit.Heart, true) { DelayedCardId = 5 };
            frames.Push(judgment);
            try
            {
                Reject(() => runtime.CaptureRetirement(judgment.Id, [outside]));
                Need(!runtime.HasRelinquishedMaterial(judgment.Id, 1),
                    "A real delayed judgment cannot borrow the surrounding card use's material or its separate revealed judgment entity.");
            }
            finally { frames.Pop(judgment.Id, judgment.Kind); }
        }
        finally
        {
            frames.Pop(foreign.Id, foreign.Kind);
            frames.Pop(use.Id, use.Kind);
        }
    }

    private static void CheckProcessingDestruction(IProgramRuntimeFrames frames)
    {
        var fixture = new Materials(frames, 4);
        var use = fixture.Use([1, 2, 3, 4]);
        frames.Push(use);
        try
        {
            var entries = Enumerable.Range(1, 4).Select(id => fixture.Move(id, CardLocation.Processing)).ToArray();
            var destroyed = fixture.Move(1, CardLocation.OutsideGame);
            fixture.Move(2, CardLocation.DiscardPile);
            var afterDiscard = fixture.Move(2, CardLocation.OutsideGame);
            fixture.Move(3, CardLocation.Hand(1));
            fixture.Move(3, CardLocation.Processing);
            var reentered = fixture.Move(3, CardLocation.OutsideGame);
            fixture.Move(4, CardLocation.DiscardPile);
            fixture.Move(4, CardLocation.Hand(1));
            var laterOwner = fixture.Move(4, CardLocation.OutsideGame);
            var runtime = fixture.Runtime;
            var unchanged = frames.Find(use.Id);
            Reject(() => runtime.CaptureRetirement(use.Id, [destroyed, reentered]));
            Need(ReferenceEquals(unchanged, frames.Find(use.Id)),
                "A material's second Processing residence cannot partially authorize the retirement of another original material.");
            Reject(() => runtime.CaptureRetirement(use.Id, [laterOwner]));
            runtime.CaptureRetirement(use.Id, [destroyed, afterDiscard]);
            var retired = (CardUseFrame)frames.Find(use.Id)!;
            Need(retired.MaterialClaims.Count == 2 && retired.MaterialClaims.All(claim =>
                    claim.Kind == NativeMaterialClaimKind.Retirement && claim.Destination == CardLocation.OutsideGame &&
                    claim.OriginalEntrySequence == entries[claim.CardId - 1].Sequence) &&
                !runtime.WasClaimed(use.Id, 1) && !runtime.WasClaimed(use.Id, 2),
                "Direct Processing destruction and its immediate discard-to-destruction continuation both retain the actual original Processing entry, without becoming claimed cards.");
            Need(!runtime.HasRelinquishedMaterial(use.Id, 3) && !runtime.HasRelinquishedMaterial(use.Id, 4),
                "Material ownership that already passed through another hand cannot be reconstructed from a later destruction event.");
        }
        finally { frames.Pop(use.Id, use.Kind); }
    }

    private static void CheckAlreadyProcessingMaterial(IProgramRuntimeFrames frames)
    {
        var fixture = new Materials(frames, 1);
        var originalEntry = fixture.Move(1, CardLocation.Processing);
        var use = fixture.Use([1]);
        frames.Push(use);
        try
        {
            var destroyed = fixture.Move(1, CardLocation.OutsideGame);
            fixture.Runtime.CaptureRetirement(use.Id, [destroyed]);
            var current = (CardUseFrame)frames.Find(use.Id)!;
            Need(current.Action!.PhysicalCards.Single().From == CardLocation.Processing &&
                current.MaterialClaims is [{ Kind: NativeMaterialClaimKind.Retirement, OriginalEntrySequence: var entry,
                    MovementSequence: var departure }] && entry == originalEntry.Sequence && departure == destroyed.Sequence &&
                entry <= use.NativeRepresentationIssuedAfterMovementSequence &&
                fixture.Runtime.HasRelinquishedMaterial(use.Id, 1) && !fixture.Runtime.WasClaimed(use.Id, 1),
                "A native child issued with an already-Processing material preserves its actual earlier entry rather than fabricating an entry at the later departure.");
        }
        finally { frames.Pop(use.Id, use.Kind); }
    }

    private static void CheckAutomaticNativeOwner(IProgramRuntimeFrames frames)
    {
        var fixture = new Materials(frames, 3);
        var parent = fixture.Use([1, 2]);
        frames.Push(parent);
        try
        {
            var entry = fixture.Move(1, CardLocation.Processing);
            var child = fixture.Use([1]);
            frames.Push(child);
            try
            {
                var parentBefore = frames.Find(parent.Id);
                var destroyed = fixture.Move(1, CardLocation.OutsideGame);
                fixture.Runtime.CaptureRetirement(destroyed);
                var retiredChild = (CardUseFrame)frames.Find(child.Id)!;
                fixture.Runtime.CaptureRetirement(destroyed);
                Need(ReferenceEquals(retiredChild, frames.Find(child.Id)) && retiredChild.MaterialClaims is
                        [{ Kind: NativeMaterialClaimKind.Retirement, OriginalEntrySequence: var originalEntry,
                           MovementSequence: var departure }] && originalEntry == entry.Sequence && departure == destroyed.Sequence &&
                    ReferenceEquals(parentBefore, frames.Find(parent.Id)) &&
                    ((CardUseFrame)frames.Find(parent.Id)!).MaterialClaims.Count == 0,
                    "A physical lifetime resolves the nearest native child's actual inherited Processing entry, and a repeated delivery remains idempotent without falling through to its older ancestor.");
            }
            finally { frames.Pop(child.Id, child.Kind); }

            fixture.Move(2, CardLocation.Processing);
            var delivered = fixture.Move(2, CardLocation.Hand(1));
            fixture.Runtime.CaptureDelivery(parent.Id, [delivered]);
            var claimed = frames.Find(parent.Id);
            fixture.Move(2, CardLocation.DiscardPile);
            fixture.Runtime.CaptureRetirement(fixture.Move(2, CardLocation.OutsideGame));
            Need(ReferenceEquals(claimed, frames.Find(parent.Id)) &&
                ((CardUseFrame)frames.Find(parent.Id)!).MaterialClaims is
                    [{ Kind: NativeMaterialClaimKind.Delivery, CardId: 2, MovementSequence: var delivery }] && delivery == delivered.Sequence &&
                fixture.Runtime.ClaimedCardIds(parent.Id).SequenceEqual([2]),
                "Destruction after an actual acquisition leaves the earlier owner's delivery intact and adds no retirement or second acquisition from a later holder's discard.");

            fixture.Move(3, CardLocation.DiscardPile);
            fixture.Runtime.CaptureRetirement(fixture.Move(3, CardLocation.OutsideGame));
            Need(ReferenceEquals(claimed, frames.Find(parent.Id)),
                "An ordinary physical destruction with no native material owner creates no unrelated native retirement receipt.");
        }
        finally { frames.Pop(parent.Id, parent.Kind); }
    }

    private sealed class Materials
    {
        private readonly IProgramRuntimeFrames _frames;
        private readonly Card[] _cards;
        private readonly CardZoneStore _zones = new(2);
        internal List<CardMovementRecord> Journal { get; } = [];
        internal CardUseMaterialClaimRuntime Runtime { get; }

        internal Materials(IProgramRuntimeFrames frames, int count)
        {
            _frames = frames;
            _cards = Enumerable.Range(1, count).Select(id => new Card(id, CardKind.Slash, Suit.Heart, id)).ToArray();
            _zones.LoadInitialDeck(_cards);
            foreach (var card in _cards) Move(card.Id, CardLocation.Hand(0));
            Runtime = new(frames, new CardMovementHistoryQueries(Journal), _zones);
        }

        internal CardUseFrame Use(IReadOnlyList<int> ids)
        {
            var id = _frames.NextId();
            var action = new CardActionContext(id * 100, null, CardActionType.Use, 0, 0, null, null, null,
                CardKind.Slash, [1], ids.Select(cardId => new CardActionCost(cardId, CardKind.Slash, _zones.GetLocation(cardId))).ToArray(), []);
            return new(id, 0, ids[0], CardKind.Slash, [1], PhysicalCardIds: ids)
            { Action = action, NativeRepresentation = _cards[ids[0] - 1], NativeRepresentationIssuedAfterMovementSequence = Journal.Count };
        }

        internal CardMovementRecord Move(int id, CardLocation destination)
        {
            if (destination.Zone == CardZoneKind.PublicPersistentPile)
                _zones.EnsurePublicPersistentPile(destination);
            var from = _zones.GetLocation(id);
            _zones.Move(id, from, destination);
            var movement = new CardMovementRecord(Journal.Count + 1, 0, id, _cards[id - 1].Kind, from,
                destination, CardMoveReasons.Use);
            Journal.Add(movement);
            return movement;
        }
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("A native payment or destruction escaped its original owner, physical movement or material entry.");
    }
}
