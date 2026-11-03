using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineOwners;

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
        HandGainOccurrencesKeepWholeAcquisitionsAndFrozenSources();
    }

    private static void HandGainOccurrencesKeepWholeAcquisitionsAndFrozenSources()
    {
        var catalogs = new[] { "ol-wang-can", "ol-tang-tang", "le-zhou-fei", "classic-li-dian" }
            .Select(CharacterFixtureContent.EmbeddedProgramBundle).ToArray();
        var registry = ContentRegistry.Build(new StandardContentPackage(), new GainProjectionPackage(catalogs));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, registry);
        var sanwen = catalogs[0].Programs["ol:sanwen"];
        foreach (var hasOldWitness in new[] { false, true })
        {
            var f = new HandGainDrawPorts();
            f.Zones.Move(11, CardLocation.DrawPile, hasOldWitness ? CardLocation.Hand(0) : CardLocation.OutsideGame);
            f.AfterMove = count => { if (count == 1) f.Owner.SkillGrants.SetEnabled("gain-source", false); };
            var drawn = f.Draw.DrawCards(f.Owner, 2, log: false);
            var receipt = f.Batches.PendingBatches[^1].HandGains!.Single();
            Require(drawn.SequenceEqual([13, 12]) && f.Batches.PendingBatchCount == 2 &&
                    f.Batches.PendingBatchAt(0).HandGains is null &&
                    receipt.Incoming.Select(c => c.CardId).SequenceEqual(drawn) &&
                    receipt.NativeDrawInvocationId == 901 && receipt.NativeDrawAttemptId is null &&
                    receipt.PreviousHand.All(c => !drawn.Contains(c.CardId)) &&
                    receipt.PreviousHand.Select(c => c.CardId).SequenceEqual(hasOldWitness ? [11] : []) &&
                    receipt.InitialSources is [{ SkillInstanceId: "gain-instance" }] && receipt.Sources.Count == 0 &&
                    receipt.FirstOutsideDraw,
                "A real Draw2 retains both physical batches, coalesces only its gain occurrence on the last, and separates the initial source from completion-time eligibility.");
            var completed = f.Batches.PendingBatches[^1];
            var tang = CharacterTangTangOlRuntime.ProjectTangTangGains(completed);
            Require(tang.Select(gain => gain.CardId).SequenceEqual(hasOldWitness ? drawn : []) &&
                    ProgramExpressions.EvaluateBool(sanwen.Triggers.Single().Condition.Expression!, new GainRuleWorld(f, sanwen, completed.HandGains!.Single())) == false,
                "TangTang projects exactly the two acquisitions with an old same-color witness; Sanwen rejects the completion-time lost source.");
            var active = new HandGainDrawPorts(catalogs[0].Programs["ol:sanwen"]);
            active.Zones.Move(11, CardLocation.DrawPile, hasOldWitness ? CardLocation.Hand(0) : CardLocation.OutsideGame);
            active.Draw.DrawCards(active.Owner, 2, log: false);
            var ruleWorld = new GainRuleWorld(active, sanwen, active.Batches.PendingBatches[^1].HandGains!.Single());
            var candidates = ProgramExpressions.EvaluateCardSet(sanwen.Triggers.Single().Effects[0].CardSetExpression!, ruleWorld)!;
            ruleWorld.Accepted = candidates.Select(card => card.CardId).ToArray();
            var witnesses = ProgramExpressions.EvaluateCardSet(sanwen.Triggers.Single().Effects[2].CardSetExpression!, ruleWorld)!;
            Require(ProgramExpressions.EvaluateBool(sanwen.Triggers.Single().Condition.Expression!, ruleWorld) == hasOldWitness &&
                    (hasOldWitness ? candidates.Select(card => card.CardId).SequenceEqual(drawn) && witnesses.Select(card => card.CardId).SequenceEqual([11]) : candidates.Count == 0),
                "The real Sanwen projection normalizes effective Slash names and requires a pre-acquisition witness, not another new card.");
            if (hasOldWitness) f.Zones.Move(11, CardLocation.Hand(0), CardLocation.DiscardPile);
            f.Face = CardKind.Dodge;
            Require(receipt.PreviousHand.Count == (hasOldWitness ? 1 : 0) &&
                    receipt.PreviousHand.All(c => c.EffectiveKind == CardKind.Slash) &&
                    receipt.Incoming.All(c => c.EffectiveKind != CardKind.Dodge),
                "Later hand movement or effective-face changes do not refresh the completed occurrence.");
            var incoming = receipt.Incoming.ToList();
            var previous = receipt.PreviousHand.ToList();
            var sources = receipt.InitialSources.ToList();
            var rows = new List<HandGainOccurrence> { receipt with
                { Incoming = incoming, PreviousHand = previous, Sources = sources, InitialSources = sources } };
            var batch = f.Batches.PendingBatches[^1] with { HandGains = rows };
            incoming.Clear(); previous.Clear(); sources.Clear(); rows.Clear();
            var frozen = batch.HandGains!.Single();
            Require(frozen.Incoming.Count == 2 && frozen.PreviousHand.Count == (hasOldWitness ? 1 : 0) &&
                    frozen.Sources.Count == 1 && frozen.InitialSources.Count == 1,
                "Every exposed gain collection freezes its input, including both source snapshots.");
            CannotAppend(batch.HandGains, frozen);
            CannotAppend(frozen.Incoming, frozen.Incoming[0]);
            CannotAppend(frozen.PreviousHand, frozen.Incoming[0]);
            CannotAppend(frozen.Sources, frozen.Sources[0]);
            CannotAppend(frozen.InitialSources, frozen.InitialSources[0]);
        }

        var marking = new HandGainDrawPorts(catalogs[2].Programs["le:lingkong"]);
        marking.AfterMove = count => { if (count == 1) marking.Owner.SkillGrants.SetEnabled("gain-source", false); };
        marking.Draw.DrawCards(marking.Owner, 2, log: false);
        var markedBatch = marking.Batches.PendingBatches[^1];
        var movementWindows = game.GetRuntimeModule<CardsMovedWindowRuntime>();
        var firstWindow = movementWindows.CreateWindow(marking.Batches.PendingBatches[0]);
        var issuedWindow = movementWindows.CreateWindow(markedBatch)!;
        var candidate = issuedWindow.Candidates.Single(c => c.SkillId == "le:lingkong");
        issuedWindow = issuedWindow with { CandidateIndex = issuedWindow.Candidates.ToList().IndexOf(candidate) };
        var context = issuedWindow.Contexts![issuedWindow.CandidateIndex];
        var authorization = ProgramInvocations.Find(issuedWindow, candidate, context);
        Require(firstWindow?.Candidates.All(c => c.SkillId != "le:lingkong") != false &&
                markedBatch.HandGains is [{ Sources.Count: 0, InitialSources: [var initial], Incoming.Count: 2 }] &&
                candidate == initial && context.Window == SkillProgramTriggerWindow.CardsGained &&
                ReferenceEquals(context.MovementBatch, markedBatch) && authorization is { RetainOwnerDeath: true } &&
                markedBatch.HandGains[0].Incoming.Select(c => c.CardId).SequenceEqual([13, 12]),
            "The shared movement window freezes the initial source, canonical gain context and one declared invocation authorization after source loss.");
        var invocation = new ProgramSkillFrame(904, candidate.OwnerSeat, candidate.SkillId, candidate.BindingId,
            candidate.GameplayHash, 0, [], [])
        {
            TriggerId = candidate.BindingId, SkillInstanceId = candidate.SkillInstanceId, WindowContext = context,
            InvocationAuthorization = authorization! with { ProgramFrameId = 904 }
        };
        var ownerGain = markedBatch.HandGains!.Single();
        var foreignGain = ownerGain with { RecipientSeat = candidate.OwnerSeat + 1 };
        // HandGain reads only the supplied frame and context; unrelated world ports are deliberately absent.
        var gainWorld = new CardGame.Core.NativeOwners.ProgramExpressionWorld(null!, null!, null!, null!, null!, null!, null!,
            invocation with { WindowContext = context with { MovementBatch = markedBatch with { HandGains = [ownerGain, foreignGain] } } });
        Require(ReferenceEquals(gainWorld.HandGain(candidate.OwnerSeat), ownerGain) && gainWorld.HandGain(foreignGain.RecipientSeat) is null,
            "A gain expression reads its owner's frozen acquisition without exposing another recipient's private cards from the same batch.");
        Require(ProgramInvocations.Allows(invocation, issuedWindow) &&
                !new ProgramInvocationAuthorization(1, 2, SkillProgramTriggerWindow.CardsMoved, candidate).RetainOwnerDeath,
            "The original admitted gain invocation is authorized while equipment-departure authorizations retain the default stop-on-death policy.");
        foreach (var (name, changedFrame, changedWindow) in new[]
        {
            ("no ticket", invocation with { InvocationAuthorization = null }, issuedWindow),
            ("other program frame", invocation with { Id = invocation.Id + 1 }, issuedWindow),
            ("other binding", invocation with { TriggerId = "other" }, issuedWindow),
            ("other activation", invocation with { ActivationId = "other" }, issuedWindow),
            ("other instance", invocation with { SkillInstanceId = "other" }, issuedWindow),
            ("other content", invocation with { GameplayHash = "other" }, issuedWindow),
            ("other occurrence", invocation with { WindowContext = context with { OccurrenceIndex = context.OccurrenceIndex + 1 } }, issuedWindow),
            ("other context", invocation with { WindowContext = context with { Amount = context.Amount + 1 } }, issuedWindow),
            ("copied window identity", invocation, issuedWindow with { Id = issuedWindow.Id + 1 }),
            ("window advanced", invocation, issuedWindow with { CandidateIndex = issuedWindow.Candidates.Count }),
            ("window ticket removed", invocation, issuedWindow with { InvocationAuthorizations = [] })
        })
            Require(!ProgramInvocations.Allows(changedFrame, changedWindow), "A gain authorization cannot be borrowed by " + name + ".");

        var timing = new HandGainDrawPorts { Timing = new(1, TurnPhase.Draw, 0) };
        timing.Move(timing.Card(13), CardLocation.DrawPile, CardLocation.Hand(0), CardMoveReasons.Draw);
        Require(!timing.Batches.PendingBatches[^1].HandGains!.Single().FirstOutsideDraw,
            "The recipient's own Draw does not count as an outside-Draw gain.");
        timing.Timing = new(1, TurnPhase.Play, 1);
        timing.Move(timing.Card(12), CardLocation.DrawPile, CardLocation.Hand(0), CardMoveReasons.Draw);
        Require(timing.Batches.PendingBatches[^1].HandGains!.Single().FirstOutsideDraw,
            "An earlier owned Draw does not consume the first outside-Draw occurrence.");
        timing.Move(timing.Card(11), CardLocation.DrawPile, CardLocation.Hand(0), CardMoveReasons.Draw);
        Require(!timing.Batches.PendingBatches[^1].HandGains!.Single().FirstOutsideDraw,
            "The physical journal, rather than pending-window execution, identifies a later outside-Draw gain.");
        var transit = new HandGainDrawPorts();
        transit.Zones.Move(11, CardLocation.DrawPile, CardLocation.Hand(0));
        transit.Zones.Move(12, CardLocation.DrawPile, CardLocation.Hand(0));
        transit.Move(transit.Card(11), CardLocation.Hand(0), CardLocation.Processing, CardMoveReasons.Use);
        transit.Move(transit.Card(11), CardLocation.Processing, CardLocation.Hand(0), CardMoveReasons.Draw);
        var returned = transit.Batches.PendingBatches[^1];
        Require(!returned.HandGains!.Single().Incoming.Single().IsActualGain &&
                CharacterTangTangOlRuntime.ProjectTangTangGains(returned) is [{ CardId: 11, OtherHandCardId: 12 }],
            "A processing return is not a new acquisition, but TangTang retains its original physical-arrival benefit with another same-color hand witness.");
        transit.Move(transit.Card(13), CardLocation.DrawPile, CardLocation.Hand(0), CardMoveReasons.Draw);
        Require(transit.Batches.PendingBatches[^1].HandGains!.Single().FirstOutsideDraw,
            "Returning one's own processing material does not consume an outside-Draw acquisition.");

        foreach (var kind in new[] { NativeDrawReplacementSourceKind.OwnedHandOrEquipment,
                     NativeDrawReplacementSourceKind.DrawPileSlash })
        {
            var f = new HandGainDrawPorts();
            f.Zones.Move(11, CardLocation.DrawPile, CardLocation.Hand(0));
            var from = kind == NativeDrawReplacementSourceKind.OwnedHandOrEquipment ? CardLocation.Hand(1) : CardLocation.DrawPile;
            if (from != CardLocation.DrawPile)
                foreach (var id in new[] { 13, 12 }) f.Zones.Move(id, CardLocation.DrawPile, from);
            var reason = new CardMoveReason("fixture.typed-gain");
            foreach (var id in new[] { 13, 12 }) f.Move(f.Card(id), from, CardLocation.Hand(0), reason);
            var parent = new ProgramSkillFrame(100, 0, "fixture:replacement", "run", "hash", 1, [], []);
            var attempt = new NativeDrawAttemptFrame(902, new(902, parent.Id, 0, 2, CardMoveReasons.Draw,
                3, 1, null, 0), new(NativeDrawAttemptReturnKind.ProgramInstruction, 1, 0), 903);
            f.Gains.Complete(parent, attempt, new(0, parent.SkillId, "replacement-instance", "run", 1, reason, 0, 2)
                { SourceKind = kind, GainedIds = [13, 12] });
            Require(f.Batches.PendingBatchCount == 2 && f.Batches.PendingBatchAt(0).HandGains is null &&
                    f.Batches.PendingBatchAt(1).HandGains is [var gain] &&
                    gain.Incoming.Select(c => c.CardId).SequenceEqual([13, 12]) &&
                    gain.PreviousHand.Select(c => c.CardId).SequenceEqual([11]) &&
                    gain.NativeDrawAttemptId == attempt.Id && gain.NativeDrawInvocationId is null,
                "Both typed replacement protocols coalesce their exact gain materials without relabeling a replacement as a draw invocation.");
        }
        ReadOnlyCardSnapshotsNeverOwnProcessingCleanup(game, registry);
        BoundMovementResultsFreezeActualPhysicalMoves(game, registry);
    }

    private static void ReadOnlyCardSnapshotsNeverOwnProcessingCleanup(GameEngine game, ContentRegistry registry)
    {
        var match = (MatchRuntime)typeof(GameEngine)
            .GetProperty("MatchRuntime", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = match.Capabilities.Get<IProgramRuntimeFrames>();
        var zones = (CardZoneStore)typeof(GameEngine)
            .GetField("_cardZones", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var materials = zones.CardsAt(CardLocation.DrawPile).Take(3).ToArray();
        Require(materials.Length == 3, "The existing match deck supplies three real entities for the cleanup boundary.");
        var observed = materials.Take(2).Select(card => card.Id).ToArray();
        var materialId = materials[2].Id;
        foreach (var card in materials)
            zones.Move(card.Id, CardLocation.DrawPile, CardLocation.Processing);
        var production = game.GetRuntimeModule<ProgramBoundCardRuntime>();
        T Port<T>() => (T)typeof(ProgramBoundCardRuntime).GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(field => field.FieldType == typeof(T)).GetValue(production)!;
        var runtime = new ProgramBoundCardRuntime(Port<IProgramBoundCardFacts>(), Port<IProgramBoundInstructionFacts>(),
            Port<IProgramBoundCardMovementOperations>(), Port<IProgramBoundCardPaymentOperations>(),
            new SubsetReturnFrames(frames), Port<IProgramRuntimePrompts>(), Port<IProgramRuntimeEvents>(),
            Port<IProgramCardSnapshotQueries>(), Port<INativeLogicalDiscardOperations>(), Port<ICardMovementHistoryQueries>(), Port<ICardZoneEntryQueries>(),
            Port<CardGame.Core.RuntimeAccess.IClosedOwnedCardMoveQueriesSource>());
        var pindian = game.GetRuntimeModule<ProgramPindianRuntime>();
        var hasPending = typeof(ProgramPindianRuntime).GetMethod("HasPendingBoundCards", BindingFlags.Instance | BindingFlags.NonPublic)!;
        bool Pending() => (bool)hasPending.Invoke(pindian, null)!;
        var program = registry.GetSkill("classic:xunxun").Program!;
        var trigger = program.Triggers.Single();
        var originalTop = frames.Top;
        var frame = new ProgramSkillFrame(frames.NextId(), 0, program.Id, trigger.Id, program.GameplayHash, 2, [], [])
            { TriggerId = trigger.Id };
        frames.Push(frame);
        try
        {
            runtime.SetProgramCardSet(frame.Id, "snapshot", observed, SkillProgramCardSetVisibility.Private,
                isReadOnlySnapshot: true);
            runtime.FilterProgramBoundCards(frame.Id, "snapshot", "viewed", [], null, [], [], [], null);
            Require(!Pending() && frames.ActiveProgram(frame.Id).CardSetBindings.All(binding => binding.IsReadOnlySnapshot),
                "The actual Pindian pending-material query ignores read-only Processing snapshots and filtered bindings.");
            Require(runtime.SelectProgramCardSubset(frame.Id, 0, "viewed", "gained", 2, 2, 208,
                    SkillProgramSubsetAiOrder.MostCardsThenRankSum, true, false) == SkillProgramStepOutcome.AwaitChoice,
                "The existing subset selection consumes a read-only snapshot through its original frame binding.");
            var prompt = match.Capabilities.Get<IProgramRuntimePrompts>().Current!;
            runtime.ResolveProgramCardSubsetAnswer(prompt.Choices.Single(choice => choice.Cards.SequenceEqual(observed)));
            Require(frames.ActiveProgram(frame.Id).CardSetBindings.Single(binding => binding.Name == "gained").IsReadOnlySnapshot && !Pending(),
                "An accepted subset preserves read-only provenance without acquiring another resolution's Processing materials.");
            runtime.CleanupProgramBoundCards(frames.ActiveProgram(frame.Id), completed: false);
            Require(observed.All(id => zones.GetLocation(id) == CardLocation.Processing),
                "Cancellation cleanup preserves original, filtered and selected read-only snapshot entities.");
            runtime.SetProgramCardSet(frame.Id, "material", [materialId], SkillProgramCardSetVisibility.Public);
            Require(Pending(), "A real material binding still blocks another Pindian while its entity remains in Processing.");
            runtime.CleanupProgramBoundCards(frames.ActiveProgram(frame.Id), completed: true);
            Require(zones.GetLocation(materialId) == CardLocation.DiscardPile && !Pending() &&
                    observed.All(id => zones.GetLocation(id) == CardLocation.Processing),
                "Completion cleanup retires real bound materials while preserving read-only observations in the same frame.");
        }
        finally { frames.Pop(frame.Id, frame.Kind); }
        Require(ReferenceEquals(frames.Top, originalTop), "The snapshot cleanup check restores the original resolution stack.");
    }

    private static void BoundMovementResultsFreezeActualPhysicalMoves(GameEngine game, ContentRegistry registry)
    {
        var match = (MatchRuntime)typeof(GameEngine).GetProperty("MatchRuntime", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = match.Capabilities.Get<IProgramRuntimeFrames>();
        var zones = (CardZoneStore)typeof(GameEngine).GetField("_cardZones", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var production = game.GetRuntimeModule<ProgramBoundCardRuntime>();
        T Port<T>() => (T)typeof(ProgramBoundCardRuntime).GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(field => field.FieldType == typeof(T)).GetValue(production)!;
        var material = zones.CardsAt(CardLocation.DrawPile).Take(2).ToArray();
        Require(material.Length == 2, "The existing deck supplies real movable and blocked hand materials.");
        foreach (var card in material) zones.Move(card.Id, CardLocation.DrawPile, CardLocation.Hand(0));
        var weapon = new Card(990001, CardKind.Crossbow, Suit.Spade, 1) { IsGeneralWeapon = true };
        zones.AddGeneratedCard(weapon);
        zones.Move(weapon.Id, CardLocation.OutsideGame, CardLocation.Equipment(0));
        var history = Port<ICardMovementHistoryQueries>();
        var runtime = new ProgramBoundCardRuntime(new BlockedDiscardFacts(Port<IProgramBoundCardFacts>(), material[1].Id),
            Port<IProgramBoundInstructionFacts>(), new NestedMovementAfterFirst(Port<IProgramBoundCardMovementOperations>(), zones, material[0].Id),
            Port<IProgramBoundCardPaymentOperations>(), frames, Port<IProgramRuntimePrompts>(), Port<IProgramRuntimeEvents>(),
            Port<IProgramCardSnapshotQueries>(), Port<INativeLogicalDiscardOperations>(), history, Port<ICardZoneEntryQueries>(),
            Port<CardGame.Core.RuntimeAccess.IClosedOwnedCardMoveQueriesSource>());
        var program = registry.GetSkill("classic:xunxun").Program!;
        var trigger = program.Triggers.Single();
        var originalTop = frames.Top;
        var frame = new ProgramSkillFrame(frames.NextId(), 0, program.Id, trigger.Id, program.GameplayHash, 2, [], []) { TriggerId = trigger.Id };
        frames.Push(frame);
        try
        {
            ProgramSkillCardSetBinding Result(string name) => frames.ActiveProgram(frame.Id).CardSetBindings.Single(binding => binding.Name == name);
            void Move(string source, string result) => ((IProgramBoundCardMovementResults)runtime).Execute(frames.ActiveProgram(frame.Id),
                new(SkillProgramEffectOp.MoveBoundCards, SkillProgramEffectTarget.Owner, 0, new(SkillProgramConditionKind.Always, 0, []),
                    sourceBind: source, resultBind: result, destination: SkillProgramCardDestination.DiscardPile), new("fixture.bound-result.move"));
            var before = history.Read().Count;
            runtime.SetProgramCardSet(frame.Id, "empty-source", [], SkillProgramCardSetVisibility.Private);
            Move("empty-source", "empty-result");
            runtime.SetProgramCardSet(frame.Id, "blocked-source", [material[1].Id], SkillProgramCardSetVisibility.Private);
            Move("blocked-source", "blocked-result");
            Require(Result("empty-result").CardIds.Count == 0 && Result("blocked-result").CardIds.Count == 0 &&
                Result("empty-result").IsReadOnlySnapshot && Result("blocked-result").IsReadOnlySnapshot && history.Read().Count == before &&
                zones.GetLocation(material[1].Id) == CardLocation.Hand(0),
                "Empty and fully prohibited moves produce real empty observation results without inventing a physical payment.");
            runtime.SetProgramCardSet(frame.Id, "materials", [material[0].Id, material[1].Id, weapon.Id], SkillProgramCardSetVisibility.Private);
            Move("materials", "actual-result");
            var actual = Result("actual-result");
            Require(actual.CardIds.SequenceEqual([material[0].Id, weapon.Id]) &&
                actual.SourceLocations.SequenceEqual([CardLocation.DiscardPile, CardLocation.OutsideGame]) &&
                actual.Visibility == SkillProgramCardSetVisibility.Private && actual.IsReadOnlySnapshot &&
                zones.GetLocation(material[0].Id) == CardLocation.Processing && zones.GetLocation(weapon.Id) == CardLocation.OutsideGame &&
                zones.GetLocation(material[1].Id) == CardLocation.Hand(0),
                "The result retains first physical destinations, including real generated-weapon destruction, excludes the blocked material, and does not refresh from a later move of the same entity.");
        }
        finally { frames.Pop(frame.Id, frame.Kind); }
        Require(ReferenceEquals(frames.Top, originalTop), "The movement-result check restores the original resolution stack.");
    }

    private sealed class BlockedDiscardFacts(IProgramBoundCardFacts facts, int blockedCard) : IProgramBoundCardFacts
    {
        public bool IsValidSeat(int seat) => facts.IsValidSeat(seat);
        public bool IsHuman(int seat) => facts.IsHuman(seat);
        public string Name(int seat) => facts.Name(seat);
        public bool HasHpValue(int seat) => facts.HasHpValue(seat);
        public int Hp(int seat) => facts.Hp(seat);
        public int MaxHp(int seat) => facts.MaxHp(seat);
        public IReadOnlyList<Card> Hand(int seat) => facts.Hand(seat);
        public IReadOnlyList<Card> Equipment(int seat) => facts.Equipment(seat);
        public IReadOnlyList<Card> CardsAt(CardLocation location) => facts.CardsAt(location);
        public CardLocation Location(int cardId) => facts.Location(cardId);
        public CardMovementRecord? LastMovement(int cardId) => facts.LastMovement(cardId);
        public EquipmentSlot FieldEquipmentSlot(Card card) => facts.FieldEquipmentSlot(card);
        public Suit EffectiveSuit(int seat, Card card) => facts.EffectiveSuit(seat, card);
        public bool ForeignDiscardPrevented(int chooser, Card card, CardLocation source, OwnedCardMoveIntent intent) =>
            card.Id == blockedCard || facts.ForeignDiscardPrevented(chooser, card, source, intent);
    }

    private sealed class NestedMovementAfterFirst(IProgramBoundCardMovementOperations movement, CardZoneStore zones, int observedCard)
        : IProgramBoundCardMovementOperations
    {
        private bool _moved;
        private void AfterPrimaryMove()
        {
            if (_moved || zones.GetLocation(observedCard) != CardLocation.DiscardPile) return;
            _moved = true;
            var card = zones.CardsAt(CardLocation.DiscardPile).Single(item => item.Id == observedCard);
            movement.Move(card, CardLocation.DiscardPile, CardLocation.Processing, new("fixture.bound-result.nested"));
        }
        public long Sequence => movement.Sequence;
        public Card? DrawToProcessing(CardMoveReason reason) => movement.DrawToProcessing(reason);
        public void Move(Card card, CardLocation source, CardLocation destination, CardMoveReason reason,
            ProgramBoundCardGive? give = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified)
        { movement.Move(card, source, destination, reason, give, disclosure); AfterPrimaryMove(); }
        public void MoveCards(IReadOnlyList<Card> cards, CardLocation source, CardLocation destination, CardMoveReason reason,
            ProgramBoundCardGive? give = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified)
        { movement.MoveCards(cards, source, destination, reason, give, disclosure); AfterPrimaryMove(); }
        public void MoveMultipleSources(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason,
            CardMovementDisclosure disclosure, Action<long, IReadOnlyList<CardMovementRecord>>? afterPhysicalRecords = null)
        { movement.MoveMultipleSources(ids, destination, reason, disclosure, afterPhysicalRecords); AfterPrimaryMove(); }
        public void PlaceAtBottom(IReadOnlyList<int> ids) => movement.PlaceAtBottom(ids);
        public void PlaceAtTop(IReadOnlyList<int> ids) => movement.PlaceAtTop(ids);
    }

    // Observe the subset's typed binding return before the real scheduler begins the next recipe instruction.
    private sealed class SubsetReturnFrames(IProgramRuntimeFrames frames) : IProgramRuntimeFrames
    {
        public ResolutionFrame? Top => frames.Top;
        public ResolutionFrame? Find(long id) => frames.Find(id);
        public ResolutionFrame? ParentOf(long id) => frames.ParentOf(id);
        public ResolutionFrame? ChildOf(long id) => frames.ChildOf(id);
        public IReadOnlyList<T> OfType<T>() where T : ResolutionFrame => frames.OfType<T>();
        public ProgramSkillFrame ActiveProgram(long id) => frames.ActiveProgram(id);
        public long NextId() => frames.NextId();
        public void ReplaceTop(ResolutionFrame frame) => frames.ReplaceTop(frame);
        public void Replace(long id, ResolutionFrame frame) => frames.Replace(id, frame);
        public void Push(ResolutionFrame frame) => frames.Push(frame);
        public void Pop(long id, ResolutionFrameKind kind) => frames.Pop(id, kind);
        public void Advance(long id) => frames.Advance(id);
        public void AdvanceTop<T>() where T : ResolutionFrame => frames.AdvanceTop<T>();
        public void AdvanceProgram(long id) { }
    }

    private sealed class GainProjectionPackage(IReadOnlyList<SkillProgramCatalog> catalogs) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-hand-gain-projections", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var catalog in catalogs)
                foreach (var (id, program) in catalog.Programs)
                    builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
        }
    }

    private static void CannotAppend<T>(IReadOnlyList<T> items, T item)
    {
        try { ((IList<T>)items).Add(item); throw new InvalidOperationException("A gain collection remained writable."); }
        catch (NotSupportedException) { }
    }

    private sealed class GainRuleWorld(HandGainDrawPorts ports, SkillProgram program, HandGainOccurrence gain) : IProgramExpressionWorld
    {
        public IReadOnlyList<int> Accepted { get; set; } = [];
        public int? Participant(string name) => name == "owner" ? gain.RecipientSeat : null;
        public IReadOnlyList<int>? ParticipantSet(string name) => null;
        public int? Stat(string stat, int seat) => null;
        public bool? Flag(string flag, int seat) => null;
        public HandGainOccurrence? HandGain(int recipient) => recipient == gain.RecipientSeat ? gain : null;
        public bool? GainHasSource(int recipient, string moment) => HandGain(recipient) is null ? null :
            (moment == "initial" ? gain.InitialSources : gain.Sources).Any(source => source.SkillId == program.Id &&
                source.BindingId == program.Triggers.Single().Id && source.SkillInstanceId == "gain-instance" && source.GameplayHash == program.GameplayHash);
        public bool? CardAt(int id, int seat, CardZoneKind zone) => ports.Zones.GetLocation(id) == new CardLocation(zone, seat);
        public bool? CardCanDiscard(int seat, int id) => ports.Zones.GetLocation(id) == CardLocation.Hand(seat);
        public bool? CardInBinding(int id, string name) => name == "sanwen-accepted" ? Accepted.Contains(id) : null;
    }

    private sealed class HandGainDrawPorts : IPhysicalCardDrawPolicies, IPhysicalCardDrawMovement,
        IPhysicalCardDrawLogs, INativeDrawInvocationInvoices, IProgramRuntimeEvents,
        IProgramWindowCandidateQueries, IHandGainCardSnapshotQueries
    {
        private readonly MatchSkillBindingIndex _bindings;
        internal CharacterState Owner { get; } = new()
        {
            Seat = 0, Name = "Gain", IsHuman = true, Role = Role.Rebel, RoleRevealed = true,
            General = new("fixture:gain-owner", "Gain", "", []), GeneralSelected = true,
            GeneralRevealed = true, MaxHp = 4, Hp = 4
        };
        internal CardZoneStore Zones { get; } = new(2);
        internal CardMovementBatchStore Batches { get; } = new();
        private readonly List<CardMovementRecord> _movements = [];
        internal HandGainOccurrenceRuntime Gains { get; }
        internal PhysicalCardDrawRuntime Draw { get; }
        internal CardMovementTiming Timing { get; set; } = new(1, TurnPhase.Play, 1);
        internal CardKind? Face { get; set; }
        internal Action<int>? AfterMove { get; set; }

        internal HandGainDrawPorts(SkillProgram? program = null)
        {
            program ??= CharacterFixtureContent.EmbeddedProgramBundle("ol-wang-can").Programs["ol:sanwen"];
            _bindings = new(id => new ContentSkillDefinition(id, "Gain", "") { Program = program }, false);
            Owner.SkillGrants.Grant(new("gain-source", program.Id, "gain-instance", "acquired:fixture"));
            Zones.LoadInitialDeck([new(11, CardKind.Slash, Suit.Spade, 1), new(12, CardKind.FireSlash, Suit.Spade, 2),
                new(13, CardKind.Slash, Suit.Spade, 3)]);
            var history = new CardMovementHistoryQueries(_movements);
            Gains = new(new MatchNativeFactDemand(ProgramTrackedFact.HandGainOccurrence), Zones, history,
                new MatchParticipantStateAccess([Owner]), this, this, Batches, Batches);
            Draw = new(Zones, history, Zones, new DeterministicRandom(1), this, this, this, this, this);
        }
        internal Card Card(int id) => Zones.CardsAt(Zones.GetLocation(id)).Single(c => c.Id == id);
        public HandGainCardSnapshot Capture(int recipient, int id, int sequence, CardLocation from, bool actualGain) =>
            new(id, Card(id).Kind, Face ?? Card(id).Kind, Card(id).Suit, sequence, from, actualGain);
        public SkillBindingShard Shard(CharacterState owner) => _bindings.GetShard(owner);
        public SkillProgramTriggerFacts Capture(CharacterState owner) => throw new NotSupportedException();
        public IReadOnlyList<ProgramTriggerCandidate> Eligible(CharacterState owner, SkillProgramTriggerWindow window,
            SkillProgramTriggerFacts facts) => throw new NotSupportedException();
        public IReadOnlyList<ProgramTriggerCandidate> IncludeIssuedReturns(CharacterState owner, SkillProgramTriggerWindow window,
            IReadOnlyList<ProgramTriggerCandidate> ordinary) => throw new NotSupportedException();
        public bool DrawFromBottom(CharacterState player) => false;
        public Card MinimumDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> stock, Card ordinary) => ordinary;
        public bool TryChooseDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> stock, out Card? material)
        { material = null; return false; }
        public void ExcludeDeckEquipment(CardLocation location) { }
        public bool AllowsDiscardRecycling => false;
        public void HandleEmptyDrawPile() { }
        public void NotifyDeckRecycled() { }
        public void Move(Card card, CardLocation from, CardLocation to, CardMoveReason reason)
        {
            Zones.Move(card.Id, from, to);
            var move = new CardMovementRecord(_movements.Count + 1, 3, card.Id, card.Kind, from, to, reason)
                { MovementTiming = Timing };
            _movements.Add(move);
            Batches.AppendCompletedBatch(Gains.Capture(new(200 + _movements.Count, 100, null, 3, [move], [])
                { MovementTiming = Timing }));
            AfterMove?.Invoke(_movements.Count);
        }
        public void MoveAll(CardLocation from, CardLocation to, CardMoveReason reason) => throw new NotSupportedException();
        public NativeDrawInvocationDraft? Capture(int owner, int requested, CardMoveReason reason, CardConversionSource? producer) =>
            new(901, owner, requested, 3, 1, 1, null, 100, producer, reason, _movements.Count);
        public void Complete(NativeDrawInvocationDraft? draft, IReadOnlyList<int> actualIds)
        {
            if (draft is null || actualIds.Count == 0) return;
            Gains.Complete(new NativeDrawInvocationProof
            {
                InvocationId = draft.Id, ParentFrameId = draft.Parent, OwnerSeat = draft.Owner,
                ActualTurnNumber = draft.Turn, ActualTurnOwnerSeat = draft.TurnOwner, RequestedCount = draft.Requested,
                Reason = draft.Reason, SequenceBefore = draft.Before, SequenceAfter = _movements.Count,
                LastBatchId = Batches.PendingBatches[^1].Id,
                Materials = _movements.Where(m => actualIds.Contains(m.CardId))
                    .Select(m => new NativeDrawMaterial(m.CardId, m.CardKind, m.Sequence)).ToArray()
            });
        }
        public void AdvanceFact(IGameEvent fact) { }
        public void Add(string type, string message, int? actor = null) { }
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
