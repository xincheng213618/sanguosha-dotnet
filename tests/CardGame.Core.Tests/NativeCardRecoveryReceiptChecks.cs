using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineStates;

internal static class NativeCardRecoveryReceiptChecks
{
    public static void ActualApplicationsOwnNativeReturnsAndRescueIdentity()
    {
        using var h = new Harness();
        var use = h.Use(0, CardKind.Peach, [1, 2], [900001, 900002]);
        h.Frames.Push(use);
        try
        {
            var first = h.Apply(use, 1, 2, 3, 2, recovery =>
            {
                var owner = h.Frames.Find(use.Id);
                var count = h.AppliedCount;
                h.Runtime.RecordNativeCardRecovery(use.Id, recovery.Id, 0, 1, CardKind.Peach, 2, 2, 3);
                Need(ReferenceEquals(owner, h.Frames.Find(use.Id)) && h.AppliedCount == count,
                    "One actual RecoveryFrame keeps one idempotent application before its HP return.");
                Reject(() => h.Runtime.RecordNativeCardRecovery(use.Id, recovery.Id, 0, 1, CardKind.Peach, 2, 2, 4));
                Reject(() => h.Runtime.RecordNativeCardRecovery(long.MaxValue, recovery.Id, 0, 1, CardKind.Peach, 2, 2, 3));
                Reject(() => h.Runtime.RecordNativeCardRecovery(use.Id, long.MaxValue, 0, 1, CardKind.Peach, 2, 2, 3));
                Reject(() => h.Runtime.RecordNativeCardRecovery(use.Id, recovery.Id, 1, 1, CardKind.Peach, 2, 2, 3));
                Reject(() => h.Runtime.RecordNativeCardRecovery(use.Id, recovery.Id, 0, 2, CardKind.Peach, 2, 2, 3));
                Reject(() => h.Runtime.RecordNativeCardRecovery(use.Id, recovery.Id, 0, 1, CardKind.Alcohol, 2, 2, 3));
                Reject(() => h.Runtime.RecordNativeCardRecovery(use.Id, recovery.Id, 0, 1, CardKind.Peach, 1, 2, 3));
                Reject(() => h.Runtime.RecordNativeCardRecovery(use.Id, recovery.Id, 0, 1, CardKind.Peach, 2, 1, 3));
                h.Frames.Replace(recovery.Id, recovery with { ParentFrameId = long.MaxValue });
                try { Reject(() => h.Runtime.RecordNativeCardRecovery(use.Id, recovery.Id, 0, 1, CardKind.Peach, 2, 2, 3)); }
                finally { h.Frames.Replace(recovery.Id, recovery); }
                Need(ReferenceEquals(owner, h.Frames.Find(use.Id)) && h.AppliedCount == count,
                    "A conflicting invoice or foreign child and parent cannot replace the original application or emit another fact.");
            });
            var hp = first.Hp!;
            var historyReads = 0;
            IReadOnlyList<IGameEvent> ReadHistory() { historyReads++; return h.Journal; }
            Need(!h.Runtime.ExactPaidNativeRecoveryUseEdge(first.Use, hp, ReadHistory) &&
                !h.Runtime.ExactPaidNativeRecoveryReplacementEdge(first.Use, hp, ReadHistory) && historyReads == 0,
                "A real native use and its positive HP child do not demand unrelated program-issuance or replacement history.");
            Need(h.Runtime.NativeCardRecoveryReturnMatches(first.Use, hp),
                "A real popped recovery produces the exact positive HP child of its owning native use.");
            h.Journal.RemoveAll(fact => fact is NativeCardRecoveryAppliedEvent);
            Need(h.Runtime.NativeCardRecoveryReturnMatches(first.Use, hp),
                "A native application's original receipt remains sufficient when its output notification is absent.");
            var notification = new NativeCardRecoveryAppliedEvent(use.Id, first.Use.RecoveryApplication!);
            h.Journal.Add(notification); h.Journal.Add(notification);
            Need(h.Runtime.NativeCardRecoveryReturnMatches(first.Use, hp) &&
                !h.Runtime.NativeCardRecoveryReturnMatches(first.Use with { RecoveryApplication = null }, hp),
                "Duplicate historical notifications neither invalidate the real receipt nor supply a missing one.");
            CheckApplicationShape(h.Runtime, first.Use, hp);

            h.Frames.Replace(use.Id, first.Use with { TargetIndex = 1 });
            var second = h.Apply((CardUseFrame)h.Frames.Find(use.Id)!, 2, 1, 2);
            var repeated = h.Apply(second.Use, 2, 2, 3);
            Need(second.Use.RecoveryApplication!.RecoveryFrameId != first.Use.RecoveryApplication!.RecoveryFrameId &&
                repeated.Use.RecoveryApplication!.RecoveryFrameId != second.Use.RecoveryApplication.RecoveryFrameId &&
                h.Runtime.NativeCardRecoveryReturnMatches(second.Use, second.Hp!) &&
                h.Runtime.NativeCardRecoveryReturnMatches(repeated.Use, repeated.Hp!) &&
                !h.Runtime.NativeCardRecoveryReturnMatches(second.Use, hp) &&
                !h.Runtime.NativeCardRecoveryReturnMatches(repeated.Use, second.Hp!),
                "Another target and a repeated effect on the same use replace only the current actual application; old HP invoices cannot borrow the new receipt.");
            var pending = h.Health.PendingHpChangeCount;
            var zero = h.Apply(repeated.Use, 2, 3, 3);
            Need(zero.Hp is null && h.Health.PendingHpChangeCount == pending &&
                zero.Use.RecoveryApplication is { HpBefore: 3, HpAfter: 3 } zeroReceipt &&
                h.Journal.OfType<NativeCardRecoveryAppliedEvent>().Count(fact => fact.Receipt == zeroReceipt) == 1 &&
                !h.Runtime.NativeCardRecoveryReturnMatches(zero.Use, repeated.Hp!),
                "A real zero-delta application still issues its notification but creates no positive HP child and cannot reuse an earlier one.");
        }
        finally { h.Frames.Pop(use.Id, use.Kind); }

        CheckRescue(h, CardKind.Peach, source: 1, target: 0, expected: true);
        CheckRescue(h, CardKind.Alcohol, source: 0, target: 0, expected: true);
        CheckRescue(h, CardKind.Alcohol, source: 1, target: 0, expected: false);
        Need(h.BeforeFactChecks >= 7, "The actual writer exposes every new application on its original use before emitting its canonical fact.");
    }

    private static void CheckApplicationShape(NativeHealthRecoveryRuntime runtime, CardUseFrame use,
        HpChangedTriggerWindowFrame hp)
    {
        var ids = use.PhysicalCardIds!;
        foreach (var invalid in new[]
        {
            use with { PhysicalCardIds = [ids[0]] },
            use with { PhysicalCardIds = [ids[1], ids[0]] },
            use with { CardKind = CardKind.Slash },
            use with { TargetSeats = [0] },
            use with { Action = Action(use, actor: 1) },
            use with { Action = Action(use, provider: 1) },
            use with { Action = Action(use, kind: CardKind.Alcohol) },
            use with { Action = Action(use, type: CardActionType.Response) },
            use with { PhysicalCardIds = [ids[0], ids[0]], Action = Action(use,
                materials: [new(ids[0], CardKind.Dodge, CardLocation.Hand(0)), new(ids[0], CardKind.Dodge, CardLocation.Hand(0))]) }
        })
            Need(!runtime.NativeCardRecoveryReturnMatches(invalid, hp),
                "Native HP returns keep the complete ordered distinct physical group, its provider and actor, effective kind and actual targets.");
        var supplied = use with { Action = Action(use, provider: 2,
            materials: ids.Select(id => new CardActionCost(id, CardKind.Dodge, CardLocation.Hand(2))).ToArray()) };
        Need(runtime.NativeCardRecoveryReturnMatches(supplied, hp),
            "A material provider remains distinct from the native card actor when the full accepted group belongs to that provider.");
        foreach (var invalid in new[]
        {
            hp with { Id = long.MaxValue }, hp with { ResumeFrameId = long.MaxValue },
            hp with { Continuation = PostEventContinuation.Program }, hp with { CardKind = CardKind.Alcohol },
            hp with { Change = hp.Change with { ParentFrameId = long.MaxValue } },
            hp with { Change = hp.Change with { SourceSeat = 1 } },
            hp with { Change = hp.Change with { TargetSeat = 2 } },
            hp with { Change = hp.Change with { HpBefore = 1 } }
        })
            Need(!runtime.NativeCardRecoveryReturnMatches(use, invalid),
                "A native invoice retains its actual HP change, participant pair and return owner.");
    }

    private static void CheckRescue(Harness h, CardKind kind, int source, int target, bool expected)
    {
        var damage = new DamageFrame(h.Frames.NextId(), 0, 2, target, 1);
        var original = new DyingFrame(h.Frames.NextId(), damage.Id, target, 2, [source, 2], 0, DyingContinuationKind.Damage);
        var use = h.Use(source, kind, [target], [900003]) with
        {
            DyingResponse = kind == CardKind.Peach ? new(original.Id, source, true, 900003) :
                new(original.Id, source, false, null, true, 900003)
        };
        h.Frames.Push(damage); h.Frames.Push(original); h.Frames.Push(use);
        try
        {
            var applied = h.Apply(use, target, 0, 1);
            use = applied.Use;
            var hp = applied.Hp!;
            h.Frames.Push(hp);
            try
            {
                var index = h.Facts.FrameIndex(hp.Id);
                Need(h.Runtime.NativeCardRecoveryReturnMatches(use, hp) &&
                    h.Runtime.TryGetNativeRescueRecovery(index, out var returned) == expected &&
                    (!expected || returned?.Id == original.Id),
                    "Peach may rescue another participant; Alcohol may rescue only its own user through the exact original dying token.");
                if (!expected) return;
                foreach (var invalid in new[]
                {
                    use with { DyingResponse = use.DyingResponse! with { ResolutionId = long.MaxValue } },
                    use with { DyingResponse = use.DyingResponse! with { ResponderSeat = 2 } },
                    use with { DyingResponse = use.DyingResponse! with { UsedPeach = true, UsedAlcohol = true } }
                })
                {
                    h.Frames.Replace(use.Id, invalid);
                    Need(!h.Runtime.TryGetNativeRescueRecovery(index, out _),
                        "The current responder and declared rescue card cannot borrow another dying occurrence or both response identities.");
                }
                h.Frames.Replace(use.Id, use);
                foreach (var invalid in new[]
                {
                    original with { ResponderIndex = 1 },
                    original with { VictimSeat = 2 },
                    original with { ParentFrameId = long.MaxValue },
                    original with { Continuation = DyingContinuationKind.WenhePoison }
                })
                {
                    h.Frames.Replace(original.Id, invalid);
                    Need(!h.Runtime.TryGetNativeRescueRecovery(index, out _),
                        "A rescue keeps its original victim, live responder and supported actual continuation parent.");
                }
                foreach (var continuation in new[] { DyingContinuationKind.ProgramSkill, DyingContinuationKind.AttackHpLoss })
                {
                    h.Frames.Replace(original.Id, original with { Continuation = continuation });
                    Need(h.Runtime.TryGetNativeRescueRecovery(index, out var kept) && kept?.Id == original.Id,
                        "Native rescue retains the existing program-loss and attack-loss dying return protocols.");
                }
                h.Frames.Replace(original.Id, original);
                var inner = new DyingFrame(h.Frames.NextId(), hp.Id, 2, source, [2], 0, DyingContinuationKind.ProgramSkill);
                var innerChange = new HpChangeContext(h.Frames.NextId(), inner.Id, source, 2, HpChangeKind.Recovery, 1, 0, 1);
                var innerHp = new HpChangedTriggerWindowFrame(innerChange.Id, innerChange, [], [], PostEventContinuation.CardUse,
                    use.Id, use.CardId, use.CardKind);
                h.Frames.Push(inner); h.Frames.Push(innerHp);
                try
                {
                    Need(h.Runtime.TryGetNativeRescueRecovery(index, out var kept) && kept?.Id == original.Id &&
                        !h.Runtime.TryGetNativeRescueRecovery(h.Facts.FrameIndex(innerHp.Id), out _) &&
                        !h.Runtime.IsOriginalDyingSuspendedByRecoveryDamage(inner),
                        "A nested victim retains its own dying flow and cannot borrow the outer rescue's receipt or original token.");
                }
                finally { h.Frames.Pop(innerHp.Id, innerHp.Kind); h.Frames.Pop(inner.Id, inner.Kind); }
            }
            finally { h.Frames.Replace(original.Id, original); h.Frames.Pop(hp.Id, hp.Kind); }
        }
        finally { h.Frames.Pop(use.Id, use.Kind); h.Frames.Pop(original.Id, original.Kind); h.Frames.Pop(damage.Id, damage.Kind); }
    }

    private static CardActionContext Action(CardUseFrame use, int? actor = null, int? provider = null,
        CardKind? kind = null, CardActionType type = CardActionType.Use, IReadOnlyList<CardActionCost>? materials = null) =>
        new(use.Id, null, type, actor ?? use.SourceSeat, provider ?? use.SourceSeat, null, null, null,
            kind ?? use.CardKind, use.TargetSeats, materials ?? use.PhysicalCardIds!.Select(id =>
                new CardActionCost(id, CardKind.Dodge, CardLocation.Hand(use.SourceSeat))).ToArray(), []);

    private sealed class Harness : IDisposable
    {
        private readonly IMatchParticipantStateAccess _players;
        private readonly int _priorCount;
        internal IProgramRuntimeFrames Frames { get; }
        internal INativeHealthRecoveryFacts Facts { get; }
        internal IHealthContinuationFacts Health { get; }
        internal NativeHealthRecoveryRuntime Runtime { get; }
        internal List<IGameEvent> Journal { get; }
        internal int BeforeFactChecks { get; private set; }
        internal int AppliedCount => Journal.OfType<NativeCardRecoveryAppliedEvent>().Count();

        internal Harness()
        {
            var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, HumanSeat = 0, HumanRole = Role.Lord,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, StandardContentRegistry.Create());
            Need(game.Submit(new StartGameCommand()).Accepted, "The native receipt fixture starts its real health journal.");
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", flags)!.GetValue(game)!;
            Frames = capabilities.Get<IProgramRuntimeFrames>();
            Facts = capabilities.Get<INativeHealthRecoveryFacts>();
            Health = capabilities.Get<IHealthContinuationFacts>();
            _players = capabilities.Get<IMatchParticipantStateAccess>();
            Journal = ((CoreCommandJournalStorageState)typeof(GameEngine).GetField("_commandJournalStorage", flags)!.GetValue(game)!)
                .ReferenceToPendingEvents;
            _priorCount = Frames.OfType<ResolutionFrame>().Count;
            Runtime = new(Facts, capabilities.Get<INativeHealthRecoveryProtocols>(), Frames,
                new ObservingEvents(capabilities.Get<IProgramRuntimeEvents>(), fact =>
                {
                    if (fact is not NativeCardRecoveryAppliedEvent applied) return;
                    Need(Frames.Find(applied.CardUseFrameId) is CardUseFrame { RecoveryApplication: { } receipt } &&
                        receipt == applied.Receipt && Frames.Top is RecoveryFrame child && child.Id == receipt.RecoveryFrameId,
                        "The actual native receipt must be on its owner while the real recovery child is still top, before any fact observer runs.");
                    BeforeFactChecks++;
                }));
        }

        internal CardUseFrame Use(int source, CardKind kind, int[] targets, int[] materials)
        {
            var use = new CardUseFrame(Frames.NextId(), source, materials[0], kind, targets, PhysicalCardIds: materials);
            return use with { Action = Action(use) };
        }

        internal (CardUseFrame Use, HpChangedTriggerWindowFrame? Hp) Apply(CardUseFrame use, int target,
            int before, int after, int requested = 1, Action<RecoveryFrame>? whilePending = null)
        {
            var recovery = new RecoveryFrame(Frames.NextId(), use.Id, use.SourceSeat, target, requested, HpBefore: before);
            _players.At(target).Hp = after;
            var pending = Health.PendingHpChangeCount;
            Frames.Push(recovery);
            try
            {
                Runtime.RecordNativeCardRecovery(use.Id, recovery.Id, use.SourceSeat, target, use.CardKind, requested, before, after);
                whilePending?.Invoke(recovery);
            }
            finally { Frames.Pop(recovery.Id, recovery.Kind); }
            var owner = (CardUseFrame)Frames.Find(use.Id)!;
            if (before == after) return (owner, null);
            var change = Enumerable.Range(pending, Health.PendingHpChangeCount - pending).Select(Health.PendingHpChangeAt)
                .Single(item => item.ParentFrameId == use.Id && item.TargetSeat == target && item.HpBefore == before && item.HpAfter == after);
            return (owner, new(change.Id, change, [], [], PostEventContinuation.CardUse, use.Id, use.CardId, use.CardKind));
        }

        public void Dispose()
        {
            while (Frames.OfType<ResolutionFrame>().Count > _priorCount)
            { var top = Frames.Top!; Frames.Pop(top.Id, top.Kind); }
        }
    }

    private sealed class ObservingEvents(IProgramRuntimeEvents inner, Action<IGameEvent> before) : IProgramRuntimeEvents
    {
        public void AdvanceFact(IGameEvent fact) { before(fact); inner.AdvanceFact(fact); }
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("A native recovery replaced its actual original child or application invoice.");
    }
}
