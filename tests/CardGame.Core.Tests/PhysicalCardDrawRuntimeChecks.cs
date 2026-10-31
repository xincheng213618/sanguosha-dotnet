using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class PhysicalCardDrawRuntimeChecks
{
    public static void SharedOwnersPreserveSelectionActualGainsAndRecyclingOrder()
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 8 },
            StandardContentRegistry.Create());
        var match = (MatchRuntime)typeof(GameEngine)
            .GetProperty("MatchRuntime", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var capabilities = match.Capabilities;
        var matchRandom = (DeterministicRandom)typeof(GameEngine)
            .GetField("_random", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var randomState = matchRandom.State;
        var production = game.GetRuntimeModule<PhysicalCardDrawRuntime>();
        object? Dependency(Type type) => typeof(PhysicalCardDrawRuntime)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Single(field => field.FieldType == type)
            .GetValue(production);
        Require(ReferenceEquals(production, capabilities.Get<IPhysicalCardDrawOperations>()) &&
                ReferenceEquals(Dependency(typeof(ICardZoneQueries)), capabilities.Get<ICardZoneQueries>()) &&
                ReferenceEquals(Dependency(typeof(ICardMovementHistoryQueries)), capabilities.Get<ICardMovementHistoryQueries>()) &&
                ReferenceEquals(Dependency(typeof(ICardDrawPileOrder)), capabilities.Get<ICardDrawPileOrder>()) &&
                ReferenceEquals(capabilities.Get<ICardZoneQueries>(), capabilities.Get<ICardDrawPileOrder>()) &&
                ReferenceEquals(Dependency(typeof(IMatchRandomSource)), matchRandom) &&
                ReferenceEquals(Dependency(typeof(IPhysicalCardDrawMovement)), game.GetRuntimeModule<AtomicCardMovementRuntime>()) &&
                ReferenceEquals(Dependency(typeof(INativeDrawInvocationInvoices)), game.GetRuntimeModule<NativeDrawInvoiceRuntime>()) &&
                matchRandom.State == randomState,
            "Physical draws resolve once per match, retain the actual zone/history/random owners and consume no random step during construction.");
        var invoiceRuntime = game.GetRuntimeModule<NativeDrawInvoiceRuntime>();
        object? InvoiceDependency(Type type) => typeof(NativeDrawInvoiceRuntime)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Single(field => field.FieldType == type)
            .GetValue(invoiceRuntime);
        Require(ReferenceEquals(InvoiceDependency(typeof(ICardMovementBatchQueue)), capabilities.Get<ICardMovementBatchQueue>()) &&
                ReferenceEquals(InvoiceDependency(typeof(ICardMovementBatchNativeDrawInvoices)), capabilities.Get<ICardMovementBatchQueue>()) &&
                ReferenceEquals(InvoiceDependency(typeof(ICardMovementHistoryQueries)), capabilities.Get<ICardMovementHistoryQueries>()) &&
                ReferenceEquals(InvoiceDependency(typeof(IProgramEventHistoryQueries)), capabilities.Get<IProgramEventHistoryQueries>()),
            "Draw invoices assign the original pending batch and read the same actual physical journal and fact history.");
        Require(ReferenceEquals(InvoiceDependency(typeof(INativeDrawOrdinalInvoices)), game.GetRuntimeModule<NativeDrawOrdinalRuntime>()) &&
                ReferenceEquals(InvoiceDependency(typeof(INativeDrawThresholdInvoices)), game.GetRuntimeModule<NativeDrawThresholdInvoiceRuntime>()) &&
                ReferenceEquals(InvoiceDependency(typeof(IActualDiscardRecoveryPhaseQueries)), game.GetRuntimeModule<ActualDiscardRecoveryPhaseRuntime>()),
            "Invoices compose the actual ordinal, threshold and phase rule owners without an engine return bridge or construction cycle.");
        var startup = game.GetRuntimeModule<GameStartingDrawOriginRuntime>();
        Require(ReferenceEquals(InvoiceDependency(typeof(IGameStartingDrawOrigins)), startup) &&
                ReferenceEquals(InvoiceDependency(typeof(IZhanjiDrawEntitlements)), game.GetRuntimeModule<ZhanjiDrawEntitlementRuntime>()) &&
                ReferenceEquals(capabilities.Get<IGameStartingDrawOriginQueries>(), startup) &&
                ReferenceEquals(capabilities.Get<IGameStartingMovementBatchOrigins>(), capabilities.Get<ICardMovementBatchQueue>()),
            "Startup and Zhanji invoice sources resolve independent shared modules; startup metadata writes reach the original pending-batch owner.");
        var logicalDiscards = game.GetRuntimeModule<NativeLogicalDiscardRuntime>();
        Require(ReferenceEquals(capabilities.Get<INativeLogicalDiscardMovement>(), game.GetRuntimeModule<AtomicCardMovementRuntime>()) &&
                ReferenceEquals(capabilities.Get<INativeLogicalDiscardOperations>(), logicalDiscards),
            "Logical discard composes the existing physical movement runtime and has one rule owner per match.");
        foreach (var consumer in new object[] { game.GetRuntimeModule<ManualDiscardRuntime>(),
                     game.GetRuntimeModule<ProgramBoundCardRuntime>(), game.GetRuntimeModule<ProgramOwnedZoneDiscardRuntime>(),
                     game.GetRuntimeModule<ProgramPaidCardRuntime>() })
            Require(ReferenceEquals(consumer.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(field => field.FieldType == typeof(INativeLogicalDiscardOperations)).GetValue(consumer), logicalDiscards),
                "Human, bound-card, whole-zone and paid-card discard reuse the same logical invoice service directly.");
        var journal = capabilities.Get<IProgramEventJournalQueries>();
        foreach (var source in new object[] { game.GetRuntimeModule<NativeDrawOrdinalRuntime>(),
                     game.GetRuntimeModule<NativeDrawThresholdInvoiceRuntime>(), game.GetRuntimeModule<ActualDiscardRecoveryPhaseRuntime>() })
            Require(ReferenceEquals(source.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(field => field.FieldType == typeof(IProgramEventJournalQueries)).GetValue(source), journal),
                "Phase and draw-source proofs use the same original journal owner and retain their own snapshot boundaries.");
        var phaseRuntime = game.GetRuntimeModule<OwnedDrawPhaseRuntime>();
        object? PhaseDependency(Type type) => typeof(OwnedDrawPhaseRuntime)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Single(field => field.FieldType == type)
            .GetValue(phaseRuntime);
        Require(ReferenceEquals(PhaseDependency(typeof(IPhysicalCardDrawOperations)), production) &&
                ReferenceEquals(PhaseDependency(typeof(IGuanduDrawWindowCandidates)), game.GetRuntimeModule<ProgramDrawDiscardWindowEntryRuntime>()),
            "Actual Draw phases compose the same physical draw and foreign observer services directly.");
        var phaseRules = game.GetRuntimeModule<ActualDiscardRecoveryPhaseRuntime>();
        var phaseEnds = game.GetRuntimeModule<ActualPhaseEndRuntime>();
        Require(ReferenceEquals(PhaseDependency(typeof(IActualPhaseEndQueries)), phaseEnds) &&
                ReferenceEquals(PhaseDependency(typeof(IActualPhaseEndOperations)), phaseEnds),
            "Owned draw phase endings query and enter their actual phase-end runtime directly.");
        Require(ReferenceEquals(PhaseDependency(typeof(IActualDiscardRecoveryPhaseQueries)), phaseRules) &&
                ReferenceEquals(PhaseDependency(typeof(IActualDiscardRecoveryPhaseOperations)), phaseRules) &&
                ReferenceEquals(capabilities.Get<IProgramEventHistoryQueries>(), journal) &&
                ReferenceEquals(capabilities.Get<INativeDrawThresholdHost>(), game.GetRuntimeModule<NativeDrawThresholdRuntime>()),
            "Actual Draw phases share the same phase query/mutation owner and threshold effects resolve their independent rule module.");

        var player = capabilities.Get<IMatchParticipantStateAccess>().At(0);
        var originalChained = player.IsChained;
        var zones = new CardZoneStore(8);
        zones.LoadInitialDeck([new(1, CardKind.Slash, Suit.Spade, 1), new(2, CardKind.Dodge, Suit.Heart, 2),
            new(3, CardKind.Peach, Suit.Heart, 3), new(4, CardKind.Slash, Suit.Club, 4)]);
        var ports = new DrawPorts(zones, player) { SelectMaterials = true, OnFirstMove = () => player.IsChained = !originalChained };
        var runtime = Create(zones, ports, new DeterministicRandom(1));
        try
        {
            var drawn = runtime.DrawCards(player, 3, log: true);
            Require(drawn.SequenceEqual([4]) && ports.CompletedIds.SequenceEqual(drawn) &&
                    zones.GetLocation(4) == CardLocation.DiscardPile && zones.GetLocation(3) == CardLocation.OutsideGame &&
                    ports.MinimumOrdinaryIds.SequenceEqual([1, 1, 1]) &&
                    ports.Trace.Where(item => item is "bottom" or "minimum" or "choice").SequenceEqual(
                        ["bottom", "minimum", "choice", "bottom", "minimum", "choice", "bottom", "minimum", "choice"]) &&
                    ports.SeenChained.SequenceEqual([originalChained, !originalChained, !originalChained]),
                "Bottom selection precedes the minimum-material policy, then the optional choice; later synchronous draws see the same changed character. Only an original hand destination counts, even when its gain immediately leaves hand.");
            Require(ports.Facts is [CardsDrawnEvent fact] && fact.Count == 1 && fact.PlayerSeat == player.Seat &&
                    ports.Trace.TakeLast(3).SequenceEqual(["invoice", "fact", "log:CardsDrawn"]) &&
                    ports.Logs is [("CardsDrawn", var message, var actor)] &&
                    message == $"{player.Name} 摸了 1 张牌。" && actor == player.Seat,
                "The completed actual-gain invoice precedes the CardsDrawn fact and its unchanged log; a declined optional material ends the remaining draw attempts.");

            ports.SelectMaterials = false;
            ports.Trace.Clear();
            ports.Facts.Clear();
            Require(runtime.DrawCards(player, 1, log: false, CardMoveReasons.InitialDeal).SequenceEqual([2]) &&
                    !ports.Trace.Contains("bottom") && ports.Facts.Count == 0,
                "Initial dealing keeps the top card, bypasses bottom policy and emits no draw presentation fact.");
            ports.Trace.Clear();
            Require(runtime.DrawOneToProcessing(CardMoveReasons.Reveal)?.Id == 1 &&
                    ports.Trace.SequenceEqual(["exclude:DrawPile", "move:1:Processing"]),
                "A processing reveal takes the physical top without hand-draw material policies or an invoice.");
        }
        finally { player.IsChained = originalChained; }

        var recycledZones = new CardZoneStore(8);
        recycledZones.LoadInitialDeck([new(10, CardKind.Slash, Suit.Spade, 1), new(11, CardKind.Dodge, Suit.Heart, 2),
            new(12, CardKind.Peach, Suit.Heart, 3), new(13, CardKind.Slash, Suit.Club, 4)]);
        recycledZones.MoveAll(CardLocation.DrawPile, CardLocation.DiscardPile);
        var recycledPorts = new DrawPorts(recycledZones, player);
        var random = new DeterministicRandom(1);
        var recycling = Create(recycledZones, recycledPorts, random);
        Require(recycling.EnsureDrawPile() && random.State == 2647435461u &&
                recycledZones.CardsAt(CardLocation.DrawPile).Select(card => card.Id).SequenceEqual([10, 12, 13, 11]) &&
                recycledPorts.Trace.SequenceEqual(["exclude:DrawPile", "exclude:DiscardPile", "move-all", "log:DeckReshuffled", "recycled"]),
            "Recycling moves the original discard zone, uses the match generator's existing shuffle and logs before the mode callback.");
        recycledZones.MoveAll(CardLocation.DrawPile, CardLocation.DiscardPile);
        recycledPorts.AllowsDiscardRecycling = false;
        recycledPorts.Trace.Clear();
        var beforeEmpty = random.State;
        Require(!recycling.EnsureDrawPile() && random.State == beforeEmpty &&
                recycledZones.Count(CardLocation.DiscardPile) == 4 &&
                recycledPorts.Trace.SequenceEqual(["exclude:DrawPile", "empty"]),
            "A mode that disallows recycling receives its empty-pile callback before any discard filtering, movement or random consumption.");
    }

    private static PhysicalCardDrawRuntime Create(CardZoneStore zones, DrawPorts ports, DeterministicRandom random) =>
        new(zones, new CardMovementHistoryQueries(ports.Movements), zones, random, ports, ports, ports, ports, ports);

    private sealed class DrawPorts(CardZoneStore zones, CharacterState player) : IPhysicalCardDrawPolicies,
        IPhysicalCardDrawMovement, IPhysicalCardDrawLogs, INativeDrawInvocationInvoices, IProgramRuntimeEvents
    {
        public bool AllowsDiscardRecycling { get; set; } = true;
        internal bool SelectMaterials { get; set; }
        internal Action? OnFirstMove { get; init; }
        internal List<CardMovementRecord> Movements { get; } = [];
        internal List<string> Trace { get; } = [];
        internal List<IGameEvent> Facts { get; } = [];
        internal List<(string Type, string Message, int? Actor)> Logs { get; } = [];
        internal List<int> MinimumOrdinaryIds { get; } = [];
        internal List<bool> SeenChained { get; } = [];
        internal IReadOnlyList<int> CompletedIds { get; private set; } = [];
        private int _choices;

        public bool DrawFromBottom(CharacterState supplied)
        {
            Require(ReferenceEquals(supplied, player), "The bottom policy receives the original character.");
            Trace.Add("bottom");
            return SelectMaterials;
        }
        public Card MinimumDrawMaterial(CharacterState supplied, CardMoveReason reason, IReadOnlyList<Card> stock, Card ordinary)
        {
            Require(ReferenceEquals(supplied, player) && ReferenceEquals(stock, zones.CardsAt(CardLocation.DrawPile)),
                "Material policies receive the original character and live physical zone.");
            MinimumOrdinaryIds.Add(ordinary.Id);
            SeenChained.Add(supplied.IsChained);
            Trace.Add("minimum");
            return SelectMaterials ? stock.Single(card => card.Id == 2) : ordinary;
        }
        public bool TryChooseDrawMaterial(CharacterState supplied, CardMoveReason reason, IReadOnlyList<Card> stock, out Card? material)
        {
            Require(ReferenceEquals(supplied, player), "The last material policy receives the same original character.");
            Trace.Add("choice");
            material = SelectMaterials && _choices < 2 ? stock.Single(card => card.Id == 4 - _choices) : null;
            if (SelectMaterials) _choices++;
            return SelectMaterials;
        }
        public void ExcludeDeckEquipment(CardLocation location) => Trace.Add($"exclude:{location.Zone}");
        public void HandleEmptyDrawPile() => Trace.Add("empty");
        public void NotifyDeckRecycled() => Trace.Add("recycled");
        public void Move(Card card, CardLocation from, CardLocation to, CardMoveReason reason)
        {
            var actualTo = card.Id == 3 ? CardLocation.OutsideGame : to;
            zones.Move(card.Id, from, actualTo);
            Record(card, from, actualTo, reason);
            Trace.Add($"move:{card.Id}:{actualTo.Zone}");
            if (card.Id != 4) return;
            OnFirstMove?.Invoke();
            zones.Move(card.Id, actualTo, CardLocation.DiscardPile);
            Record(card, actualTo, CardLocation.DiscardPile, new("fixture.after-gain"));
        }
        public void MoveAll(CardLocation from, CardLocation to, CardMoveReason reason)
        {
            foreach (var card in zones.MoveAll(from, to)) Record(card, from, to, reason);
            Trace.Add("move-all");
        }
        private void Record(Card card, CardLocation from, CardLocation to, CardMoveReason reason) =>
            Movements.Add(new(Movements.Count + 1, 1, card.Id, card.Kind, from, to, reason));
        public NativeDrawInvocationDraft? Capture(int owner, int requested, CardMoveReason reason, CardConversionSource? directProducer)
        { Trace.Add("capture"); return null; }
        public void Complete(NativeDrawInvocationDraft? draft, IReadOnlyList<int> actualIds)
        { CompletedIds = actualIds.ToArray(); Trace.Add("invoice"); }
        public void AdvanceFact(IGameEvent fact) { Facts.Add(fact); Trace.Add("fact"); }
        public void Add(string type, string message, int? actor = null)
        { Logs.Add((type, message, actor)); Trace.Add($"log:{type}"); }
    }

    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
