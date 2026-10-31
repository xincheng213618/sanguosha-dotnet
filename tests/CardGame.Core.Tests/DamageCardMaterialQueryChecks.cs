using CardGame.Core;

internal static class DamageCardMaterialQueryChecks
{
    // The shared read must retain physical identity after a claim moved the
    // cards, while cardless damage cannot inherit a suspended card's material.
    internal static void ReadOriginalOwnersAcrossMaterialMovement(IProgramRuntimeFrames frames)
    {
        var zones = new CardZoneStore(2);
        zones.LoadInitialDeck([new Card(1, CardKind.Slash, Suit.Spade, 7), new Card(2, CardKind.Dodge, Suit.Heart, 8)]);
        var runtime = new ProgramDamageCardClaimRuntime(frames, zones, null!, null!, null!);
        var attack = new CardAttackState { Active = true, DamageWasApplied = true, PhysicalCardIds = [1, 2] };
        var owner = new CardUseFrame(frames.NextId(), 0, 1, CardKind.Slash, [1]) { CardAttack = attack };
        frames.Push(owner);
        var original = runtime.Read(owner.Id)!;
        zones.MoveAll(CardLocation.DrawPile, CardLocation.Hand(1));
        var child = new ProgramSkillFrame(frames.NextId(), 1, "fixture:cardless-material", "run", "hash", 0, [], [])
        {
            AttackAttempt = new(1, 0, 1, DamageNature.Normal, false, DamageWasApplied: true),
            CardAttack = attack
        };
        frames.Push(child);
        try
        {
            Need(original.Applied && original.CardIds.SequenceEqual([1, 2]) &&
                runtime.Read(owner.Id) is { Applied: true } material && material.CardIds.SequenceEqual(original.CardIds) &&
                runtime.Read(child.Id) is { Applied: true, CardIds.Count: 0 },
                "Material reads follow each original native attack and retain claimed physical cards across zone movement; cardless damage stays empty.");
            frames.Replace(owner.Id, owner with { CardAttack = attack with { Active = false } });
            Need(runtime.Read(owner.Id) is null && runtime.Read(long.MaxValue) is null && original.CardIds.SequenceEqual([1, 2]) &&
                original.CardIds is System.Collections.IList { IsReadOnly: true },
                "Retiring an attack removes its live material without rewriting the detached immutable result or finding another owner's material.");
        }
        finally
        {
            frames.Pop(child.Id, child.Kind);
            frames.Pop(owner.Id, owner.Kind);
        }
        CheckSharedClaimBoundaries(frames);
    }

    private static void CheckSharedClaimBoundaries(IProgramRuntimeFrames frames)
    {
        using (var claim = new ClaimPorts(frames, hasForeignGroup: false))
        {
            var dual = new ProgramSkillFrame(frames.NextId(), 0, "fixture:dual-damage", "run", "hash", 0, [], [])
            {
                AttackAttempt = new(0, 1, 1, DamageNature.Normal, false, DamageWasApplied: true),
                CardAttack = claim.Attack.CardAttack
            };
            frames.Push(dual);
            try
            {
                claim.SelectedOwner = new ProgramDamageCardOwner(dual.Id, IsNativeCard: true);
                Need(claim.Runtime.GetClaimable(claim.Program).Select(card => card.Id).SequenceEqual([1, 2]),
                    "A native-card selection on a dual-state program owner reads that exact card material instead of inferring cardless damage from AttackAttempt.");
                claim.SelectedOwner = new ProgramDamageCardOwner(dual.Id, IsNativeCard: false);
                Need(claim.Runtime.GetClaimable(claim.Program).Length == 0 &&
                    claim.Runtime.Read(dual.Id) is { Applied: true, CardIds.Count: 0 },
                    "A program-damage selection stays cardless on the same dual-state owner; the separate paid-material facts retain their original AttackAttempt priority.");
            }
            finally { frames.Pop(dual.Id, dual.Kind); }
        }
        using (var claim = new ClaimPorts(frames, hasForeignGroup: true))
        {
            Need(claim.Runtime.Read(claim.Attack.Id) is { Applied: false, CardIds.Count: 0 },
                "Paid material facts retain their applied qualification.");
            claim.Claim();
            var group = (CardUseFrame)frames.Find(claim.ForeignGroup!.Id)!;
            Need(claim.Zones.GetLocation(1) == CardLocation.Hand(1) && claim.Zones.GetLocation(2) == CardLocation.Hand(1) &&
                claim.Facts is [ProgramDamageCardsClaimedEvent { CardIds.Count: 2 }] && claim.AttackReads == 4 &&
                group.Continuations.GroupCard!.DamageClaimedPhysicalCardIds.SequenceEqual([3]) &&
                !ReferenceEquals(group, claim.ForeignGroup) && ReferenceEquals(claim.CapturedWindow, claim.Window),
                "Generic claims still move raw material before applied, validate every card against the foreign live group, preserve its ledger, write its owner and capture the original window after the event.");
        }
        using (var claim = new ClaimPorts(frames, hasForeignGroup: true))
        {
            var group = claim.ForeignGroup!;
            frames.Replace(group.Id, group with { Continuations = group.Continuations with
                { GroupCard = group.Continuations.GroupCard! with { PhysicalCardIds = [999] } } });
            Reject(claim.Claim);
            Need(claim.Zones.GetLocation(1) == CardLocation.Hand(1) && claim.Facts.Count == 0 && claim.AttackReads == 1,
                "A foreign group's invalid physical material is validated after movement and before checking the captured damage window or issuing a claim event.");
        }
        foreach (var hasGroup in new[] { false, true })
        {
            using var claim = new ClaimPorts(frames, hasGroup);
            claim.AfterMove = () => claim.CurrentDamageWindow = claim.Window with { Id = claim.Window.Id + 1000 };
            Reject(claim.Claim);
            Need(claim.AttackReads == 2 && claim.Zones.GetLocation(1) == CardLocation.Hand(1) &&
                claim.Facts.Count == (hasGroup ? 0 : 1) &&
                (hasGroup ? claim.CapturedWindow is null : ReferenceEquals(claim.CapturedWindow, claim.Window)),
                "Replacing the active damage window cannot substitute a new receipt: group claims reject before their event, while claims without a group reject at the original post-event capture boundary.");
        }
    }

    private sealed class ClaimPorts : IProgramDamageCardClaimNativeRules, IProgramBoundCardMovementOperations,
        IProgramRuntimeEvents, IDisposable
    {
        private readonly IProgramRuntimeFrames _frames;
        internal CardZoneStore Zones { get; } = new(2);
        internal CardUseFrame? ForeignGroup { get; }
        internal CardUseFrame Attack { get; }
        internal DamageTriggerWindowFrame Window { get; }
        internal ProgramSkillFrame Program { get; }
        internal ProgramDamageCardClaimRuntime Runtime { get; }
        internal List<IGameEvent> Facts { get; } = [];
        internal DamageTriggerWindowFrame? CapturedWindow { get; private set; }
        internal int AttackReads { get; private set; }
        internal Action? AfterMove { get; set; }
        internal ProgramDamageCardOwner? SelectedOwner { get; set; }
        public DamageTriggerWindowFrame? CurrentDamageWindow { get; set; }
        public long? ActiveGroupOwnerId => ForeignGroup?.Id;
        public long Sequence => 0;

        internal ClaimPorts(IProgramRuntimeFrames frames, bool hasForeignGroup)
        {
            _frames = frames;
            Zones.LoadInitialDeck([new Card(1, CardKind.Slash, Suit.Spade, 7), new Card(2, CardKind.Slash, Suit.Club, 8),
                new Card(3, CardKind.Dodge, Suit.Heart, 9)]);
            Zones.MoveMany([1, 2], CardLocation.DrawPile, CardLocation.Processing);
            if (hasForeignGroup)
            {
                ForeignGroup = new CardUseFrame(frames.NextId(), 0, 3, CardKind.ArrowBarrage, [1])
                {
                    Continuations = new() { GroupCard = new() { PhysicalCardIds = [3], DamageClaimedPhysicalCardIds = [3] } }
                };
                frames.Push(ForeignGroup);
            }
            Attack = new(frames.NextId(), 0, 1, CardKind.Slash, [1])
                { CardAttack = new() { Active = true, PhysicalCardIds = [1, 2] } };
            frames.Push(Attack);
            Window = new(frames.NextId(), 0, 0, 1, 1, CardKind.Slash, [], SkillProgramTriggerWindow.AfterDamageApplied);
            frames.Push(Window);
            CurrentDamageWindow = Window;
            Program = new(frames.NextId(), 1, "fixture:raw-material", "claim", "hash", 0, [], [])
                { TriggerId = "claim", WindowContext = new(SkillProgramTriggerWindow.AfterDamageApplied, Window.Id, 1, 0, 1, 0) };
            frames.Push(Program);
            Runtime = new(frames, Zones, this, this, this);
        }

        internal void Claim() => Runtime.Claim(Program.Id, 1, new("fixture.raw-material-claim"));
        public ProgramDamageCardOwner SelectAttackOwner(DamageTriggerWindowFrame window)
        {
            AttackReads++;
            if (CurrentDamageWindow?.Id != window.Id) throw new InvalidOperationException("The original damage window was replaced.");
            return SelectedOwner ?? new ProgramDamageCardOwner(Attack.Id, IsNativeCard: true);
        }
        public void CaptureExtraEffectClaims(ProgramSkillFrame frame, DamageTriggerWindowFrame window, IReadOnlyList<Card> cards,
            long sequenceBefore, CardMoveReason reason)
        { CapturedWindow = window; _ = SelectAttackOwner(window); }
        public void AdvanceFact(IGameEvent fact) => Facts.Add(fact);
        public void MoveCards(IReadOnlyList<Card> cards, CardLocation source, CardLocation destination, CardMoveReason reason,
            ProgramBoundCardGive? give = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified)
        { Zones.MoveMany(cards.Select(card => card.Id), source, destination); AfterMove?.Invoke(); }
        public void Dispose()
        {
            _frames.Pop(Program.Id, Program.Kind);
            _frames.Pop(Window.Id, Window.Kind);
            _frames.Pop(Attack.Id, Attack.Kind);
            if (ForeignGroup is { } group) _frames.Pop(group.Id, group.Kind);
        }
        public Card? DrawToProcessing(CardMoveReason reason) => throw new NotSupportedException();
        public void Move(Card card, CardLocation source, CardLocation destination, CardMoveReason reason,
            ProgramBoundCardGive? give = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified) => throw new NotSupportedException();
        public void MoveMultipleSources(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason,
            CardMovementDisclosure disclosure, Action<long, IReadOnlyList<CardMovementRecord>>? afterPhysicalRecords = null) => throw new NotSupportedException();
        public void PlaceAtBottom(IReadOnlyList<int> ids) => throw new NotSupportedException();
        public void PlaceAtTop(IReadOnlyList<int> ids) => throw new NotSupportedException();
        public NativeLogicalDiscardInvocationDraft? CaptureLogicalDiscard(int initiator, int executor, IReadOnlyList<int> ids,
            CardMoveReason reason, long? parentFrameId = null) => throw new NotSupportedException();
        public void CompleteLogicalDiscard(NativeLogicalDiscardInvocationDraft? invocation) => throw new NotSupportedException();
    }

    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("A claim accepted a replaced window or invalid foreign group material.");
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
