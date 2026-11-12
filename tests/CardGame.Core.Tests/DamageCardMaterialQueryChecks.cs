using CardGame.Core;

internal static class DamageCardMaterialQueryChecks
{
    // The shared read must retain physical identity after a claim moved the
    // cards, while cardless damage cannot inherit a suspended card's material.
    internal static void ReadOriginalOwnersAcrossMaterialMovement(IProgramRuntimeFrames frames)
    {
        var zones = new CardZoneStore(2);
        zones.LoadInitialDeck([new Card(1, CardKind.Slash, Suit.Spade, 7), new Card(2, CardKind.Dodge, Suit.Heart, 8)]);
        var runtime = new ProgramDamageCardClaimRuntime(frames, zones, null!, null!, null!, null!);
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
        CheckNativeUseClaimReceipts(frames);
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
                !ReferenceEquals(group, claim.ForeignGroup) &&
                ((CardUseFrame)frames.Find(claim.Attack.Id)!).MaterialClaims.Select(c => c.CardId).SequenceEqual([1, 2]),
                "Generic claims still move raw material before applied, validate every card against the foreign live group, preserve its ledger and record actual material on its original use before publishing the claim.");
        }
        using (var claim = new ClaimPorts(frames, hasForeignGroup: true))
        {
            var group = claim.ForeignGroup!;
            frames.Replace(group.Id, group with { Continuations = group.Continuations with
                { GroupCard = group.Continuations.GroupCard! with { PhysicalCardIds = [999] } } });
            Reject(claim.Claim);
            Need(claim.Zones.GetLocation(1) == CardLocation.Hand(1) && claim.Facts.Count == 0 && claim.AttackReads == 2 &&
                ((CardUseFrame)frames.Find(claim.Attack.Id)!).MaterialClaims.Count == 0,
                "A foreign group's invalid physical material is validated after movement and before checking the captured damage window or issuing a claim event.");
        }
        foreach (var hasGroup in new[] { false, true })
        {
            using var claim = new ClaimPorts(frames, hasGroup);
            claim.AfterMove = () => claim.CurrentDamageWindow = claim.Window with { Id = claim.Window.Id + 1000 };
            if (hasGroup) Reject(claim.Claim); else claim.Claim();
            Need(claim.AttackReads == (hasGroup ? 3 : 2) && claim.Zones.GetLocation(1) == CardLocation.Hand(1) &&
                claim.Facts.Count == (hasGroup ? 0 : 1) &&
                ((CardUseFrame)frames.Find(claim.Attack.Id)!).MaterialClaims.Count == (hasGroup ? 0 : 2),
                "A live foreign group still validates its own damage window; an independent claim retains the native owner selected before movement instead of substituting a later active window or rebuilding a producer proof.");
        }
    }

    private static void CheckNativeUseClaimReceipts(IProgramRuntimeFrames frames)
    {
        var zones = new CardZoneStore(2);
        var cards = new[] { new Card(1, CardKind.Slash, Suit.Spade, 7), new Card(2, CardKind.Slash, Suit.Club, 8),
            new Card(3, CardKind.Dodge, Suit.Heart, 9) };
        zones.LoadInitialDeck(cards);
        var movements = new List<CardMovementRecord>();
        var claims = new CardUseMaterialClaimRuntime(frames, new CardMovementHistoryQueries(movements), zones);
        var reason = new CardMoveReason("fixture.native-material-claim");
        void Move(Card card, CardLocation from, CardLocation to, CardMoveReason moveReason)
        {
            zones.Move(card.Id, from, to);
            movements.Add(new(movements.Count + 1, 0, card.Id, card.Kind, from, to, moveReason));
        }
        Move(cards[0], CardLocation.DrawPile, CardLocation.Processing, CardMoveReasons.Use);
        Move(cards[1], CardLocation.DrawPile, CardLocation.Processing, CardMoveReasons.Use);
        var use = new CardUseFrame(frames.NextId(), 0, 1, CardKind.Slash, [1], PhysicalCardIds: [1, 2]);
        frames.Push(use);
        try
        {
            var before = movements.Count;
            Reject(() => claims.Capture(use.Id, 1, [cards[0]], before, reason));
            Move(cards[0], CardLocation.Processing, CardLocation.Hand(1), reason);
            Reject(() => claims.Capture(use.Id, 1, [cards[0], cards[1]], before, reason));
            Need(((CardUseFrame)frames.Find(use.Id)!).MaterialClaims.Count == 0,
                "A batch with one undelivered material must not partially write its first claim.");
            claims.Capture(use.Id, 1, [cards[0]], before, reason);
            var first = (CardUseFrame)frames.Find(use.Id)!;
            Need(first.Action is null && first.MaterialClaims is [{ CardId: 1, RecipientSeat: 1 } receipt] &&
                receipt.MovementSequence == movements.Last().Sequence && claims.WasClaimed(use.Id, 1) &&
                claims.CanFinishClaimedMaterial(use.Id, 1) && !claims.WasClaimed(use.Id, 2),
                "A real Processing-to-Hand movement records a native use's material without an Action, producer opcode, character hash or private event proof.");
            claims.Capture(use.Id, 1, [cards[0]], before, reason);
            Need(ReferenceEquals(first, frames.Find(use.Id)) && first.MaterialClaims.Count == 1,
                "Delivering the same material receipt twice is idempotent and keeps the original immutable frame.");
            Reject(() => claims.Capture(use.Id, 1, [cards[0], cards[0]], before, reason));

            var episode = new NativeCardEffectEpisode(901, new(902, 0, 0,
                new("fixture:repeat", "repeat", 0, "fixture:instance"), "fixture-hash", new(0)), 1, 1);
            var native = new NativeCardEffectEpisodesState(new(null)) { Episodes = [episode], StartedOrdinal = 1 };
            frames.Replace(use.Id, first with { EffectEpisodes = native, TargetIndex = 0,
                Continuations = new() { GroupCard = new() { PhysicalCardIds = [1, 2], DamageClaimedPhysicalCardIds = [] } } });
            var round = (CardUseFrame)frames.Find(use.Id)!;
            frames.Replace(use.Id, round with { EffectEpisodes = native with { Episodes = [episode,
                new(903, new(904, 0, 0, new("fixture:incoming", "repeat", 1, "fixture:instance"), "fixture-hash", new(50)), 1)],
                StartedOrdinal = 2, Current = new(903, 1, 2, 0, [1]) },
                Continuations = new() { GroupCard = new() { PhysicalCardIds = [1, 2], DamageClaimedPhysicalCardIds = [] } } });
            Need(claims.ClaimedCardIds(use.Id).SequenceEqual([1]) && claims.WasClaimed(use.Id, 1) &&
                ((CardUseFrame)frames.Find(use.Id)!).MaterialClaims.SequenceEqual(first.MaterialClaims) &&
                first.MaterialClaims is System.Collections.IList { IsReadOnly: true } &&
                claims.ClaimedCardIds(use.Id) is System.Collections.IList { IsReadOnly: true },
                "Switching extra-effect owners and rebuilding their group continuation retains the original use's frozen claim without copying an episode ledger.");

            foreach (var location in new[] { CardLocation.Equipment(1), CardLocation.Processing,
                         CardLocation.DrawPile, CardLocation.DiscardPile, CardLocation.Hand(0) })
            {
                Move(cards[0], zones.GetLocation(1), location, new("fixture.claim-followup"));
                var current = frames.Find(use.Id);
                var count = movements.Count;
                Need(claims.WasClaimed(use.Id, 1) && claims.ClaimedCardIds(use.Id).SequenceEqual([1]) &&
                    claims.CanFinishClaimedMaterial(use.Id, 1) == (location.Zone is CardZoneKind.Hand or CardZoneKind.DrawPile or CardZoneKind.DiscardPile) &&
                    ReferenceEquals(current, frames.Find(use.Id)) && movements.Count == count,
                    "The claim remains a fact across later locations; only Hand, DrawPile and DiscardPile permit retirement, and reads do not mutate frames or append journal facts.");
            }
            Reject(() => claims.Capture(use.Id, 0, [cards[0]], before, reason));
            Move(cards[2], CardLocation.DrawPile, CardLocation.Processing, CardMoveReasons.Use);
            Move(cards[2], CardLocation.Processing, CardLocation.Hand(1), reason);
            Reject(() => claims.Capture(use.Id, 1, [cards[2]], before, reason));
            Reject(() => claims.Capture(use.Id, 1, [cards[1]], before, new("fixture.wrong-reason")));
            Reject(() => claims.Capture(use.Id, 1, [cards[0]], movements.Count, reason));
            Move(cards[0], CardLocation.Hand(0), CardLocation.Processing, CardMoveReasons.Use);
            var repeatedBefore = movements.Count;
            Move(cards[0], CardLocation.Processing, CardLocation.Hand(0), reason);
            Reject(() => claims.Capture(use.Id, 0, [cards[0]], repeatedBefore, reason));
            Move(cards[1], CardLocation.Processing, CardLocation.Hand(1), reason);
            Move(cards[1], CardLocation.Hand(1), CardLocation.Processing, CardMoveReasons.Use);
            Move(cards[1], CardLocation.Processing, CardLocation.Hand(1), reason);
            Reject(() => claims.Capture(use.Id, 1, [cards[1]], before, reason));
            Need(((CardUseFrame)frames.Find(use.Id)!).MaterialClaims.SequenceEqual(first.MaterialClaims) &&
                !claims.WasClaimed(long.MaxValue, 1) && !claims.CanFinishClaimedMaterial(long.MaxValue, 1) &&
                claims.ClaimedCardIds(long.MaxValue).Count == 0,
                "Unissued material, wrong recipients, reasons, cutoffs and ambiguous redelivery cannot create or replace an original claim; absent native uses have no claim facts.");
        }
        finally { frames.Pop(use.Id, use.Kind); }
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
        private readonly List<CardMovementRecord> _movements = [];
        internal int AttackReads { get; private set; }
        internal Action? AfterMove { get; set; }
        internal ProgramDamageCardOwner? SelectedOwner { get; set; }
        public DamageTriggerWindowFrame? CurrentDamageWindow { get; set; }
        public long? ActiveGroupOwnerId => ForeignGroup?.Id;
        public long Sequence => _movements.Count;

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
            Attack = new(frames.NextId(), 0, 1, CardKind.Slash, [1], PhysicalCardIds: [1, 2])
                { CardAttack = new() { Active = true, PhysicalCardIds = [1, 2] } };
            frames.Push(Attack);
            Window = new(frames.NextId(), 0, 0, 1, 1, CardKind.Slash, [], SkillProgramTriggerWindow.AfterDamageApplied);
            frames.Push(Window);
            CurrentDamageWindow = Window;
            Program = new(frames.NextId(), 1, "fixture:raw-material", "claim", "hash", 0, [], [])
                { TriggerId = "claim", WindowContext = new(SkillProgramTriggerWindow.AfterDamageApplied, Window.Id, 1, 0, 1, 0) };
            frames.Push(Program);
            Runtime = new(frames, Zones, this, this, this,
                new CardUseMaterialClaimRuntime(frames, new CardMovementHistoryQueries(_movements), Zones));
        }

        internal void Claim() => Runtime.Claim(Program.Id, 1, new("fixture.raw-material-claim"));
        public ProgramDamageCardOwner SelectAttackOwner(DamageTriggerWindowFrame window)
        {
            AttackReads++;
            if (CurrentDamageWindow?.Id != window.Id) throw new InvalidOperationException("The original damage window was replaced.");
            return SelectedOwner ?? new ProgramDamageCardOwner(Attack.Id, IsNativeCard: true);
        }
        public void AdvanceFact(IGameEvent fact) => Facts.Add(fact);
        public void MoveCards(IReadOnlyList<Card> cards, CardLocation source, CardLocation destination, CardMoveReason reason,
            ProgramBoundCardGive? give = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified)
        {
            Zones.MoveMany(cards.Select(card => card.Id), source, destination);
            _movements.AddRange(cards.Select((card, index) =>
                new CardMovementRecord(_movements.Count + index + 1, 0, card.Id, card.Kind, source, destination, reason)).ToArray());
            AfterMove?.Invoke();
        }
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
        throw new InvalidOperationException("An invalid material claim or native group boundary was accepted.");
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
