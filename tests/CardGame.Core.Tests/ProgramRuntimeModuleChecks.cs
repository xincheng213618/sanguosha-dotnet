using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ProgramRuntimeModuleChecks
{
    public static void IndependentFactoriesShareMatchInstancesAndRejectEngineDependencies()
    {
        var types = new[] { typeof(FixtureModule), typeof(StatelessModule) };
        var catalog = new ProgramRuntimeModuleCatalog(types);
        var reversed = new ProgramRuntimeModuleCatalog(types.Reverse());
        Require(catalog.Methods.SequenceEqual(reversed.Methods), "Module discovery must be deterministic.");
        var firstFacts = new Facts(3);
        var secondFacts = new Facts(7);
        var resolutions = 0;
        var first = new ProgramRuntimeModuleRegistry(catalog, type =>
        {
            resolutions++;
            return type == typeof(IFacts) ? firstFacts : throw new Exception();
        });
        var second = new ProgramRuntimeModuleRegistry(catalog, _ => secondFacts);
        var gate = typeof(FixtureModule).GetMethod(nameof(FixtureModule.Gate))!;
        var effect = typeof(FixtureModule).GetMethod(nameof(FixtureModule.Effect))!;
        var boundGate = (Func<int>)first.Bind(gate, typeof(Func<int>), new object());
        var boundEffect = (Action)first.Bind(effect, typeof(Action), new object());
        Require(resolutions == 0, "Binding runtime handlers must not construct modules or resolve their match dependencies.");
        Require(ReferenceEquals(boundGate, first.Bind(gate, typeof(Func<int>), new object())),
            "Different entry points cache delegates on the same match registry.");
        Require(boundGate() == 3 && resolutions == 1, "The first invocation resolves the owning match's constructor facts once.");
        boundEffect();
        var instance = (FixtureModule)first.Get(typeof(FixtureModule));
        Require(boundGate() == 4 && instance.Gate() == 4 && resolutions == 1 &&
                ReferenceEquals(instance, first.Get(typeof(FixtureModule))),
            "All hooks and explicit module resolution must share exactly one module instance.");
        Require(((Func<int>)second.Bind(gate, typeof(Func<int>), new object()))() == 7 &&
                !ReferenceEquals(instance, second.Get(typeof(FixtureModule))),
            "Match instances and module-local counters cannot leak between games.");
        var staticMethod = typeof(StatelessModule).GetMethod(nameof(StatelessModule.Double))!;
        Require(((Func<int, int>)first.Bind(staticMethod, typeof(Func<int, int>), new object()))(5) == 10,
            "A declared stateless static module binds without manufacturing an instance.");
        Require(new ProgramCapabilityCatalog([typeof(FixtureModule)]).Registrations.Single().Implementation == typeof(FixtureModule),
            "An independent module may expose dedicated capability contracts.");
        Reject(() => new ProgramRuntimeModuleCatalog([typeof(EngineDependency)]));
        Reject(() => new ProgramRuntimeModuleCatalog([typeof(EngineCallbackDependency)]));
        Reject(() => new ProgramRuntimeModuleCatalog([typeof(EngineReturningContractDependency)]));
        Reject(() => new ProgramRuntimeModuleCatalog([typeof(UndeclaredDependency)]));
        Reject(() => new ProgramRuntimeModuleCatalog([typeof(MutableStaticModule)]));
        var definition = new CardGame.Core.EngineStates.CoreMatchDefinitionState();
        var originalOptions = new GameOptions { Seed = 101 };
        Require(ReferenceEquals(definition.InitializeOptions(originalOptions), originalOptions),
            "Definition initialization retains the original immutable options reference.");
        Reject(() => definition.InitializeOptions(new GameOptions { Seed = 102 }));
        Require(ReferenceEquals(typeof(CardGame.Core.EngineStates.CoreMatchDefinitionState)
                .GetField("_options", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(definition), originalOptions),
            "Repeated initialization must be rejected before changing the original readonly definition.");
        Reject(() => first.Get(typeof(StatelessModule)));
        Reject(() => new ProgramRuntimeModuleRegistry(catalog, _ => new object()).Get(typeof(FixtureModule)));
        CheckMechanismBindingsAndFramePrimitives();
    }

    public static void MatchCompositionDefersBindingsAndRetriesFailedDependencies()
    {
        var modules = new ProgramRuntimeModuleCatalog([typeof(FixtureModule), typeof(StatelessModule)]);
        var capabilities = new ProgramCapabilityCatalog([typeof(FixtureModule), typeof(FixtureFactsAdapter)]);
        var attempts = 0;
        var failure = new InvalidOperationException("fixture dependency failure");
        var facts = new FixtureFactsAdapter(null!) { Value = 5 };
        var adapters = new FixtureAdapterFactory(registration =>
        {
            Require(registration.Implementation == typeof(FixtureFactsAdapter), "Independent modules must use the match's module registry.");
            if (++attempts == 1) throw failure;
            return facts;
        });
        var match = new MatchRuntime(modules, capabilities, adapters);
        var gate = typeof(FixtureModule).GetMethod(nameof(FixtureModule.Gate))!;
        var effect = typeof(FixtureModule).GetMethod(nameof(FixtureModule.Effect))!;
        var tryRead = typeof(FixtureModule).GetMethod(nameof(FixtureModule.TryRead))!;
        var handler = (Func<int>)match.BindHandler(gate, typeof(Func<int>));
        var effectHandler = (Action)match.BindHandler(effect, typeof(Action));
        var readHandler = (FixtureTryRead)match.BindHandler(tryRead, typeof(FixtureTryRead));
        Require(attempts == 0 && ReferenceEquals(handler, match.BindHandler(gate, typeof(Func<int>))),
            "Composing and binding a match only records factories and shares each typed callback.");
        try { match.BindHandler(gate, typeof(Action)); throw new Exception("A mismatched handler signature was accepted."); }
        catch (ArgumentException) { }
        try { match.BindHandler(gate, typeof(Func<int, int>)); throw new Exception("A mismatched handler parameter was accepted."); }
        catch (ArgumentException) { }
        Require(attempts == 0, "Invalid signatures must fail at binding, before dependency construction.");
        try { handler(); throw new Exception("A dependency failure was swallowed."); }
        catch (InvalidOperationException error)
        { Require(ReferenceEquals(error, failure), "Deferred construction preserves the original dependency exception."); }
        Require(attempts == 1 && handler() == 5 && attempts == 2,
            "A failed construction must clear dependency guards and leave the handler available for a valid retry.");
        effectHandler();
        Require(readHandler(false, out var read) == false && read == 6,
            "Deferred custom delegates preserve their return value and output parameters.");
        Require(handler() == 6 && ReferenceEquals(match.GetModule(typeof(FixtureModule)), match.Capabilities.Get<IFixtureCapability>()) &&
                ReferenceEquals(facts, match.Capabilities.Get<IFacts>()) && attempts == 2,
            "Explicit resolution, capability contracts and deferred entry points share the same match-owned objects.");
        var other = new MatchRuntime(modules, capabilities,
            new FixtureAdapterFactory(_ => new FixtureFactsAdapter(null!) { Value = 9 }));
        Require(((Func<int>)other.BindHandler(gate, typeof(Func<int>)))() == 9 && handler() == 6,
            "Match composition never shares adapters, bound handlers or rule instances across matches.");
        var doubleMethod = typeof(StatelessModule).GetMethod(nameof(StatelessModule.Double))!;
        Require(((Func<int, int>)match.BindHandler(doubleMethod, typeof(Func<int, int>)))(4) == 8 && attempts == 2,
            "Stateless handlers remain direct bindings without resolving runtime dependencies.");

        var cyclic = new MatchRuntime(new ProgramRuntimeModuleCatalog([typeof(CyclicModule)]),
            new ProgramCapabilityCatalog([typeof(CyclicModule)]), new FixtureAdapterFactory(_ => throw new Exception()));
        Reject(() => cyclic.GetModule(typeof(CyclicModule)));
        Reject(() => cyclic.GetModule(typeof(CyclicModule)));
        Reject(() => cyclic.Capabilities.Get<ICyclicFacts>());
        Reject(() => cyclic.Capabilities.Get<ICyclicFacts>());

        CheckNativeOwnerComposition();
        CheckNativeFrameOwnerComposition();
    }

    private static void CheckNativeOwnerComposition()
    {
        var modules = new ProgramRuntimeModuleCatalog([typeof(NativeZoneConsumer)]);
        var capabilities = new ProgramCapabilityCatalog([typeof(FixtureFactsAdapter)]);
        var factoryCalls = 0;
        var firstStore = new CardZoneStore(2);
        var secondStore = new CardZoneStore(2);
        firstStore.LoadInitialDeck([new Card(1, CardKind.Slash, Suit.Spade, 7)]);
        secondStore.LoadInitialDeck([new Card(1, CardKind.Dodge, Suit.Heart, 2)]);
        var first = new MatchRuntime(modules, capabilities,
            new FixtureAdapterFactory(_ => { factoryCalls++; throw new Exception("A native owner cannot use an engine adapter."); }),
            new MatchCoreCapabilityProvider(firstStore).Bindings);
        var second = new MatchRuntime(modules, capabilities,
            new FixtureAdapterFactory(_ => { factoryCalls++; throw new Exception("A native owner cannot use an engine adapter."); }),
            new MatchCoreCapabilityProvider(secondStore).Bindings);
        var method = typeof(NativeZoneConsumer).GetMethod(nameof(NativeZoneConsumer.DrawCount))!;
        var firstHandler = (Func<int>)first.BindHandler(method, typeof(Func<int>));
        var secondHandler = (Func<int>)second.BindHandler(method, typeof(Func<int>));
        Require(factoryCalls == 0 && ReferenceEquals(firstHandler, first.BindHandler(method, typeof(Func<int>))),
            "Binding a native-dependent module keeps deferred handler identity and never requests an engine adapter.");
        var firstModule = (NativeZoneConsumer)first.GetModule(typeof(NativeZoneConsumer));
        var secondModule = (NativeZoneConsumer)second.GetModule(typeof(NativeZoneConsumer));
        Require(ReferenceEquals(firstModule.Zones, firstStore) && ReferenceEquals(secondModule.Zones, secondStore) &&
                ReferenceEquals(first.Capabilities.Get<IAtomicCardMovementZones>(), firstStore) &&
                !ReferenceEquals(firstModule, secondModule) && factoryCalls == 0,
            "Rule dependencies and capability lookup must share each actual state owner, with independent modules per match.");
        firstModule.Zones.Move(1, CardLocation.DrawPile, CardLocation.Hand(0));
        Require(firstHandler() == 0 && secondHandler() == 1 && secondStore.GetLocation(1) == CardLocation.DrawPile,
            "A module's direct zone mutations affect only its original match and remain visible through the bound handler.");

        var production = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var owner = typeof(GameEngine).GetField("_cardZones", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(production);
        var atomic = production.GetRuntimeModule<AtomicCardMovementRuntime>();
        var zonePort = typeof(AtomicCardMovementRuntime).GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(field => field.FieldType == typeof(IAtomicCardMovementZones)).GetValue(atomic);
        Require(ReferenceEquals(zonePort, owner),
            "The production atomic runtime receives CardZoneStore itself instead of a nested GameEngine adapter.");

        var productionCapabilities = MatchCapabilities(production);
        var otherProduction = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var otherCapabilities = MatchCapabilities(otherProduction);
        Require(ReferenceEquals(productionCapabilities.Get<ICardZoneQueries>(), owner) &&
                ReferenceEquals(productionCapabilities.Get<IAtomicCardMovementZones>(), owner) &&
                !ReferenceEquals(productionCapabilities.Get<ICardZoneQueries>(), otherCapabilities.Get<ICardZoneQueries>()),
            "Card reads and atomic mutations use the same actual owner and keep different matches isolated.");
        var participantState = productionCapabilities.Get<IMatchParticipantStateAccess>();
        var participantOwner = typeof(GameEngine).GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(field => field.FieldType == typeof(CardGame.Core.EngineStates.CoreMatchParticipantsState)).GetValue(production)!;
        var actualPlayers = (IReadOnlyList<CharacterState>)typeof(CardGame.Core.EngineStates.CoreMatchParticipantsState)
            .GetField("_players", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(participantOwner)!;
        Require(ReferenceEquals(participantState.Players, actualPlayers) &&
                ReferenceEquals(participantState.At(0), actualPlayers[0]) &&
                !ReferenceEquals(participantState.At(0), otherCapabilities.Get<IMatchParticipantStateAccess>().At(0)),
            "Core participant primitives receive the original character objects and keep match state isolated.");
        foreach (var module in new object[] { production.GetRuntimeModule<ProgramHealthStateRuntime>(),
                     production.GetRuntimeModule<ProgramOwnedZoneDiscardRuntime>() })
            Require(ReferenceEquals(module.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(field => field.FieldType == typeof(IMatchParticipantStateAccess)).GetValue(module), participantState),
                "Health and owned-zone discard share the actual participant access service without another engine adapter.");
        Require(ReferenceEquals(productionCapabilities.Get<IProgramCardSetBindings>(), production.GetRuntimeModule<ProgramBoundCardRuntime>()),
            "Bound-card capabilities resolve the same rule runtime as registered handlers and compatibility entry points.");
        var scheduler = (ResolutionScheduler)typeof(GameEngine).GetField("_resolutionScheduler", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(production)!;
        var productionFrames = productionCapabilities.Get<IProgramRuntimeFrames>();
        var windowCandidates = productionCapabilities.Get<IProgramWindowCandidateQueries>();
        foreach (var module in new object[] { production.GetRuntimeModule<ProgramTurnWindowEntryRuntime>(),
                     production.GetRuntimeModule<ProgramDrawDiscardWindowEntryRuntime>(),
                     production.GetRuntimeModule<ProgramPhaseBoundaryRuntime>() })
            Require(ReferenceEquals(module.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(field => field.FieldType == typeof(IProgramWindowCandidateQueries)).GetValue(module), windowCandidates),
                "Window entry and progression share the same match-local candidate and fact query service.");
        Require(!ReferenceEquals(windowCandidates, otherCapabilities.Get<IProgramWindowCandidateQueries>()),
            "Window queries must remain isolated between matches.");
        var historyBoundaries = productionCapabilities.Get<IProgramEventBoundaryQueries>();
        var factProjector = production.GetRuntimeModule<ProgramTriggerFactProjector>();
        Require(ReferenceEquals(historyBoundaries, productionCapabilities.Get<IProgramEventHistoryQueries>()) &&
                ReferenceEquals(factProjector.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(field => field.FieldType == typeof(IProgramEventBoundaryQueries)).GetValue(factProjector), historyBoundaries),
            "Trigger fact capture and typed event queries read the same actual journal owner.");
        var resolutionId = scheduler.Identities.Value + 1;
        Require(ReferenceEquals(productionCapabilities.Get<INativeFrameRoutingFacts>(), scheduler) &&
                productionFrames.NextId() == resolutionId && scheduler.Identities.Value == resolutionId &&
                (long)global::FixtureRuntimeAccess.Engine.GetProperty("_resolutionSequence", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(production)! == resolutionId,
            "Production native callers and runtime frames share the real scheduler's one resolution identity sequence.");
        CheckNativeMovementBatchOwnership(production, otherProduction, productionCapabilities, otherCapabilities);
        CheckNativeGiveQueryOwnership(production, productionCapabilities, otherCapabilities);
        CheckCardSnapshotOwnership(production, otherProduction, productionCapabilities, otherCapabilities);
        ProgramTurnModifierRuntimeChecks.CheckNativeOwnershipAndGrantBoundaries(production, productionCapabilities, otherCapabilities);
        DamageCardMaterialQueryChecks.ReadOriginalOwnersAcrossMaterialMovement(productionFrames);
    }

    private static void CheckCardSnapshotOwnership(GameEngine game, GameEngine otherGame,
        ProgramCapabilityRegistry capabilities, ProgramCapabilityRegistry otherCapabilities)
    {
        CardSkillMarkProjectionChecks.OrderedCompositionPreservesEmptyLabelsAndCapturedTail();
        object Cache(GameEngine match) => typeof(GameEngine)
            .GetField("_cardSnapshotCache", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(match)!;
        var snapshots = capabilities.Get<IProgramCardSnapshotQueries>();
        var otherSnapshots = otherCapabilities.Get<IProgramCardSnapshotQueries>();
        var cache = capabilities.Get<ICardSnapshotCache>();
        Require(ReferenceEquals(snapshots, game.GetRuntimeModule<CardSnapshotProjector>()) &&
                ReferenceEquals(cache, Cache(game)) &&
                ReferenceEquals(otherCapabilities.Get<ICardSnapshotCache>(), Cache(otherGame)) &&
                !ReferenceEquals(cache, otherCapabilities.Get<ICardSnapshotCache>()) &&
                !ReferenceEquals(snapshots, otherSnapshots),
            "Card projections share their match's actual cache and one runtime across capability and native entry points.");
        foreach (var module in new object[] { game.GetRuntimeModule<ProgramBoundCardRuntime>(), game.GetRuntimeModule<ProgramPaidCardRuntime>() })
            Require(ReferenceEquals(module.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(field => field.FieldType == typeof(IProgramCardSnapshotQueries)).GetValue(module), snapshots),
                "Bound and paid card views use the same dedicated projector instead of a facts adapter's projection method.");

        var hands = capabilities.Get<IHandCardViewQueries>();
        Require(ReferenceEquals(hands, game.GetRuntimeModule<HandCardProjector>()) &&
                !ReferenceEquals(hands, otherCapabilities.Get<IHandCardViewQueries>()),
            "Hand projection resolves one runtime per match across its direct capability and registered handlers.");
        var playerView = game.GetRuntimeModule<PlayerViewProjector>();
        foreach (var contract in new[] { typeof(ICardZoneQueries), typeof(IProgramContentQueries),
                     typeof(IProgramCardSnapshotQueries), typeof(IHandCardViewQueries), typeof(IActivePlayerEffectProjection) })
            Require(ReferenceEquals(playerView.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(field => field.FieldType == contract).GetValue(playerView), capabilities.Get(contract)),
                "Player view assembly receives actual card, content and projection services from its match's capability registry.");
        var turnEffects = typeof(GameEngine).GetField("_turnCardUseEffects", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var activeEffects = game.GetRuntimeModule<ActivePlayerEffectProjector>();
        Require(ReferenceEquals(capabilities.Get<IActivePlayerEffectProjection>(), activeEffects) &&
                ReferenceEquals(capabilities.Get<IActivePlayerTurnEffectQueries>(), turnEffects) &&
                ReferenceEquals(activeEffects.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(field => field.FieldType == typeof(IActivePlayerTurnEffectQueries)).GetValue(activeEffects), turnEffects) &&
                !ReferenceEquals(turnEffects, otherCapabilities.Get<IActivePlayerTurnEffectQueries>()),
            "Active effect projection reads the actual isolated turn store instead of copied policy lists or an engine adapter.");

        var facts = new SnapshotFacts();
        var projector = new CardSnapshotProjector(facts, new CardSnapshotCache());
        var other = new CardSnapshotProjector(facts, new CardSnapshotCache());
        var card = new Card(991, CardKind.Slash, Suit.Spade, 7);
        var plain = projector.Snapshot(card);
        Require(ReferenceEquals(plain, projector.Snapshot(card)) && !ReferenceEquals(plain, other.Snapshot(card)),
            "Unchanged visible facts reuse one derived projection while another match retains its own cache identity.");
        facts.Bone = EquipmentSlot.Armor;
        facts.Mark = "fixture-mark";
        facts.Carrier = true;
        var hidden = projector.Snapshot(card);
        Require(hidden.IsBone && hidden.Kind == CardSnapshot.UndisclosedKind && hidden.DisplayName == "骨" &&
                hidden.OccupiedEquipmentSlot == EquipmentSlot.Armor && hidden.SkillMarkLabel == facts.Mark &&
                hidden.RankText == "—" && !ReferenceEquals(plain, hidden) && plain.Kind == CardKind.Slash,
            "Current concealment and decoration facts replace a cached projection without rewriting an exposed snapshot.");
        facts.BoneOwner = 0;
        var foreignEquipment = projector.EquipmentSnapshot(card, 1, revealAll: false);
        var ownerEquipment = projector.EquipmentSnapshot(card, 0, revealAll: false);
        var ownerReads = facts.BoneOwnerReads;
        var diagnosticEquipment = projector.EquipmentSnapshot(card, 1, revealAll: true);
        Require(ReferenceEquals(foreignEquipment, hidden) && ownerEquipment.Kind == card.Kind &&
                ownerEquipment.DisplayName == card.DisplayName && ownerEquipment.SkillMarkLabel == "骨" &&
                diagnosticEquipment == ownerEquipment && facts.BoneOwnerReads == ownerReads &&
                hidden.Kind == CardSnapshot.UndisclosedKind,
            "Equipment projection reveals a bone's printed face only to its source or explicit diagnostics, without changing the cached concealed face.");
        facts.QinSlot = CardKind.Lightning;
        Require(projector.JudgmentSnapshot(card).IsBone && projector.JudgmentSnapshot(card).QinJudgmentSlot == facts.QinSlot,
            "A Qin judgment keeps the same concealed entity projection and occupied slot.");
        facts.QinSlot = null;
        var judgment = projector.JudgmentSnapshot(card);
        Require(judgment.Kind == CardKind.Indulgence && judgment.SkillMarkLabel == facts.Mark && judgment.RankText == "—",
            "An ordinary judgment reads its current effective identity instead of a cached physical-card kind.");
    }

    private sealed class SnapshotFacts : ICardSnapshotFacts
    {
        internal EquipmentSlot? Bone;
        internal int? BoneOwner;
        internal int BoneOwnerReads;
        internal bool Carrier;
        internal string? Mark;
        internal CardKind? QinSlot;
        public EquipmentSlot? BoneSlot(Card card) => Bone;
        public int? BoneOwnerSeat(int cardId) { BoneOwnerReads++; return BoneOwner; }
        public bool IsCarrier(int cardId) => Carrier;
        public string? VisibleSkillMark(int cardId) => Mark;
        public CardKind? QinJudgmentSlot(int cardId) => QinSlot;
        public bool IsQinJudgmentCard(int cardId) => QinSlot is not null;
        public CardKind JudgmentEffectiveKind(Card card) => CardKind.Indulgence;
    }

    private static void CheckNativeGiveQueryOwnership(GameEngine game,
        ProgramCapabilityRegistry capabilities, ProgramCapabilityRegistry otherCapabilities)
    {
        object Field(string name) => global::FixtureRuntimeAccess.Engine.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var content = (ContentRegistry)Field("_contentRegistry");
        Require(ReferenceEquals(capabilities.Get<IProgramContentQueries>(), content) &&
                capabilities.Get<IProgramContentQueries>().HasSkill(content.Skills.Keys.First()),
            "Runtime content reads use the actual immutable registry instead of another engine adapter.");
        var queries = capabilities.Get<INativeGivenCardQueries>();
        var otherQueries = otherCapabilities.Get<INativeGivenCardQueries>();
        Require(ReferenceEquals(queries, capabilities.Get<INativeGivenCardQueries>()) && !ReferenceEquals(queries, otherQueries),
            "Each match shares one Give query over its own receipt and movement owners.");
        var players = (IReadOnlyList<CharacterState>)Field("_players");
        var movements = (List<CardMovementRecord>)Field("_cardMovements");
        var pending = (List<IGameEvent>)Field("_pendingEvents");
        var committed = (List<EventEnvelope>)Field("_events");
        var history = (ProgramEventHistoryIndex)capabilities.Get<IProgramEventHistoryQueries>();
        var pendingCount = pending.Count;
        var committedCount = committed.Count;
        var movement = new CardMovementRecord(901, 3, 1, CardKind.Slash,
            CardLocation.Hand(0), CardLocation.Hand(1), new("fixture.native-give-query"));
        var gift = new MouHqGift(902, 903, 0, 1, 3, 0, movement.Sequence) { Movements = [movement] };
        var batch = new CardMovementBatchContext(904, null, null, 3, [movement], []);
        var fact = new NativeCardsGivenEvent(gift.Id, gift.ProducerId, 0, 1, 1, 3, 0, movement.Sequence);
        players[0].MouHqActualGifts = [gift];
        Require(queries.GiftsForBatch(batch, 0).Count == 0, "A newly issued receipt still needs its original movement proof.");
        movements.Add(movement);
        Require(queries.GiftsForBatch(batch, 0).Count == 0, "An original movement alone cannot forge the issued Give fact.");
        history.Append(fact);
        var issued = queries.GiftsForBatch(batch, 0);
        Require(issued.Count == 1 && ReferenceEquals(issued[0], gift) &&
                issued is System.Collections.IList { IsReadOnly: true } && otherQueries.GiftsForBatch(batch, 0).Count == 0,
            "Reads observe the existing owners' new pending proof, preserve receipt identity, freeze results and stay match-local.");
        history.Append(fact);
        Require(queries.GiftsForBatch(batch, 0).Count == 0, "A duplicated Give fact cannot authorize the same receipt twice.");
        pending.RemoveRange(pendingCount, pending.Count - pendingCount);
        committed.Add(new(new EventId(committedCount + 1), null, committedCount + 1, 1, "fixture.native-give-query", fact));
        Require(ReferenceEquals(queries.GiftsForBatch(batch, 0).Single(), gift),
            "Moving the proof from pending to committed keeps the same exact receipt visible once.");
        players[0].MouHqActualGifts = [gift with { Turn = 4 }];
        Require(queries.GiftsForBatch(batch, 0).Count == 0 && ReferenceEquals(issued[0], gift),
            "A replaced turn receipt is read live without rewriting an already exposed result.");
        players[0].MouHqActualGifts = null;
        Require(queries.GiftsForBatch(batch, 0).Count == 0, "Owner expiry removes the receipt from subsequent Give queries.");
    }

    private static void CheckNativeMovementBatchOwnership(GameEngine game, GameEngine otherGame,
        ProgramCapabilityRegistry capabilities, ProgramCapabilityRegistry otherCapabilities)
    {
        object Field(GameEngine match, string name) => typeof(GameEngine)
            .GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(match)!;
        object Port(object runtime, Type contract) => runtime.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(field => field.FieldType == contract).GetValue(runtime)!;
        var owner = (CardMovementBatchStore)Field(game, "_cardMovementBatches");
        var queue = capabilities.Get<ICardMovementBatchQueue>();
        var suppression = capabilities.Get<ICardMovementEventSuppression>();
        var otherSuppression = otherCapabilities.Get<ICardMovementEventSuppression>();
        Require(ReferenceEquals(queue, owner) && ReferenceEquals(suppression, owner) &&
                !ReferenceEquals(queue, otherCapabilities.Get<ICardMovementBatchQueue>()) &&
                !ReferenceEquals(suppression, otherSuppression) &&
                ReferenceEquals(Field(game, "_pendingCardsMovedBatches"), owner.PendingBatches) &&
                ReferenceEquals(Field(game, "_activeCardMovementBatchIds"), owner.ActiveBatchIds) &&
                ReferenceEquals(Field(game, "_suppressedCardMovedEventBatchIds"), owner.SuppressedEventBatchIds),
            "Native batch ports and interim engine aliases share each original collection with no cross-match state.");
        Require(ReferenceEquals(Port(game.GetRuntimeModule<CardsMovedWindowRuntime>(), typeof(ICardMovementBatchQueue)), owner) &&
                ReferenceEquals(Port(game.GetRuntimeModule<AtomicCardMovementRuntime>(), typeof(ICardMovementEventSuppression)), owner) &&
                ReferenceEquals(Port(game.GetRuntimeModule<MovementRecordRuntime>(), typeof(ICardMovementEventSuppression)), owner),
            "Movement windows, atomic movement and movement facts receive the real batch owner instead of engine forwarding adapters.");
        var first = new CardMovementBatchContext(401, null, null, 1, [], []);
        var second = first with { Id = 402 };
        var count = queue.PendingBatchCount;
        owner.PendingBatches.Add(first);
        owner.PendingBatches.Add(second);
        Require(ReferenceEquals(queue.PendingBatchAt(count), first) && ReferenceEquals(queue.PendingBatchAt(count + 1), second),
            "Native queue reads preserve insertion order and the exact completed batch references.");
        queue.RemovePendingBatch(first);
        Require(queue.PendingBatchCount == count + 1 && ReferenceEquals(owner.PendingBatches[count], second) &&
                otherCapabilities.Get<ICardMovementBatchQueue>().PendingBatchCount == 0,
            "Removing a native pending batch mutates the original list and preserves the next batch's position.");
        queue.RemovePendingBatch(second);
        owner.ActiveBatchIds.Push(first.Id);
        owner.ActiveBatchIds.Push(second.Id);
        var activeAlias = (Stack<long>)Field(game, "_activeCardMovementBatchIds");
        Require(activeAlias.Pop() == second.Id && owner.ActiveBatchIds.Pop() == first.Id &&
                ((CardMovementBatchStore)Field(otherGame, "_cardMovementBatches")).ActiveBatchIds.Count == 0,
            "Interim native batch nesting uses the owner's original last-in-first-out stack.");
        var suppressionAlias = (Stack<long>)Field(game, "_suppressedCardMovedEventBatchIds");
        suppressionAlias.Push(first.Id);
        suppression.Suppress(second.Id);
        Require(suppression.HasSuppressedCardMovedEvents && suppressionAlias.Peek() == second.Id &&
                suppression.TryPopSuppression(out var nested) && nested == second.Id &&
                suppressionAlias.Pop() == first.Id && !suppression.HasSuppressedCardMovedEvents &&
                !suppression.TryPopSuppression(out _) && !otherSuppression.HasSuppressedCardMovedEvents,
            "Nested event suppression is shared by producers and record readers, unwinds in original order and stays match-local.");
    }

    private static void CheckNativeFrameOwnerComposition()
    {
        var modules = new ProgramRuntimeModuleCatalog([typeof(NativeFrameRoutingRuntime)]);
        var capabilities = new ProgramCapabilityCatalog([typeof(FrameProgramSpy)]);
        var scheduler = new ResolutionScheduler(new FrameLifecycle());
        var otherScheduler = new ResolutionScheduler(new FrameLifecycle());
        var routes = new RouteSpy();
        var programs = new FrameProgramSpy(null!);
        var attempts = 0;
        var failure = new InvalidOperationException("frame router dependency failure");
        var match = new MatchRuntime(modules, capabilities, new FixtureAdapterFactory(_ =>
        {
            if (++attempts == 1) throw failure;
            return programs;
        }), [new(typeof(INativeFrameRoutes), routes)], scheduler);
        var other = new MatchRuntime(modules, capabilities, new FixtureAdapterFactory(_ => new FrameProgramSpy(null!)),
            [new(typeof(INativeFrameRoutes), new RouteSpy())], otherScheduler);
        var continuation = typeof(NativeFrameRoutingRuntime).GetMethod("ContinueProgram", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var handler = (Action<ProgramSkillFrame>)match.BindHandler(continuation, typeof(Action<ProgramSkillFrame>));
        routes.OnAdvance = frame => handler((ProgramSkillFrame)frame);
        var frames = match.Capabilities.Get<IProgramRuntimeFrames>();
        var otherFrames = other.Capabilities.Get<IProgramRuntimeFrames>();
        Require(frames is ProgramRuntimeFrameServices && ReferenceEquals(frames, match.Capabilities.Get<IProgramRuntimeFrames>()) &&
                !ReferenceEquals(frames, otherFrames) && ReferenceEquals(match.Capabilities.Get<INativeFrameRoutingFacts>(), scheduler),
            "Frame services are match-owned independent objects and routing facts are the actual scheduler.");
        var dependencies = typeof(ProgramRuntimeFrameServices).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        Require(ReferenceEquals(dependencies.Single(field => field.FieldType == typeof(ResolutionScheduler)).GetValue(frames), scheduler) &&
                ReferenceEquals(dependencies.Single(field => field.FieldType == typeof(ResolutionIdentitySequence)).GetValue(frames), scheduler.Identities),
            "Frame operations retain their original scheduler and its single identity owner.");
        scheduler.Identities.Value = 40;
        Require(frames.NextId() == 41 && scheduler.Identities.Value == 41 && otherFrames.NextId() == 1 &&
                scheduler.Identities.Value == 41 && attempts == 0,
            "Native and program frame IDs share one sequence, while composing and querying frames does not construct routing modules.");
        var parent = new ProgramSkillFrame(41, 0, "fixture:owned-frames", "run", "hash", 0, [], []);
        frames.Push(parent);
        var snapshot = frames.OfType<ProgramSkillFrame>();
        Require(ReferenceEquals(frames.Top, scheduler.Frames.LastOrDefault()) && ReferenceEquals(frames.Find(parent.Id), parent) &&
                ReferenceEquals(frames.ActiveProgram(parent.Id), parent) && frames.ParentOf(parent.Id) is null &&
                frames.ChildOf(parent.Id) is null && otherFrames.Top is null && attempts == 0,
            "The independent frame port exposes original frame references without copying the store or resolving a router.");
        try { frames.Advance(parent.Id); throw new Exception("Frame service swallowed failed router construction."); }
        catch (InvalidOperationException error)
        { Require(ReferenceEquals(error, failure), "Frame routing preserves its original dependency failure."); }
        Require(attempts == 1 && routes.Calls == 0 && ReferenceEquals(frames.Top, parent),
            "A failed deferred router cannot change the selected frame or cache a successful construction.");
        frames.Advance(parent.Id);
        var routing = match.GetModule(typeof(NativeFrameRoutingRuntime));
        Require(attempts == 2 && routes.Calls == 1 && programs.Resumes == 1 && programs.Executions == 1 &&
                ReferenceEquals(routing, match.HandlerTarget(continuation)) &&
                !ReferenceEquals(routing, other.GetModule(typeof(NativeFrameRoutingRuntime))) &&
                ReferenceEquals(handler, match.BindHandler(continuation, typeof(Action<ProgramSkillFrame>))),
            "Frame services and deferred handlers share one successfully retried router per match.");
        var child = new ResponseWindowFrame(frames.NextId(), parent.Id, 0, 1, CardKind.Slash, RequiredCardKind: CardKind.Dodge);
        frames.Push(child);
        Require(ReferenceEquals(frames.ParentOf(child.Id), parent) && ReferenceEquals(frames.ChildOf(parent.Id), child) &&
                frames.ParentOf(999) is null && frames.ChildOf(999) is null && snapshot.Count == 1 && ReferenceEquals(snapshot[0], parent),
            "Parent and child queries follow the scheduler stack while typed frame snapshots remain detached.");
        frames.Advance(parent.Id);
        frames.AdvanceProgram(parent.Id);
        frames.AdvanceTop<DeferredTurnEndFrame>();
        Reject(() => frames.ActiveProgram(parent.Id), $"Program frame {parent.Id} is not active.");
        Reject(() => frames.Advance(999), "Runtime continuation 999 has no owner.");
        Reject(() => frames.ReplaceTop(parent));
        Reject(() => frames.Pop(parent.Id, parent.Kind));
        Require(routes.Calls == 1 && ReferenceEquals(frames.Top, child),
            "Suspended parent dispatch and rejected writes preserve the actual child and original query errors.");
        var replacement = parent with { Step = ResolutionFrameStep.ResolvingEffect };
        frames.Replace(parent.Id, replacement);
        Require(ReferenceEquals(frames.Top, child) && ReferenceEquals(frames.ParentOf(child.Id), replacement),
            "Replacing a suspended parent updates only its original scheduler entry.");
        frames.Pop(child.Id, child.Kind);
        Require(ReferenceEquals(frames.ActiveProgram(parent.Id), replacement), "Completing a child exposes the same current parent.");
        frames.Pop(parent.Id, parent.Kind);
        Require(scheduler.Frames.Count == 0 && otherFrames.Top is null, "Frame completion stays within the owning match.");
    }

    private static void CheckMechanismBindingsAndFramePrimitives()
    {
        var content = StandardContentRegistry.Create();
        var first = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, content);
        var second = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, content);
        var metadata = ProgramRuntimeModuleCatalog.Default;
        foreach (var registration in metadata.Registrations.Where(item => !item.Type.IsAbstract))
        {
            var instance = first.GetRuntimeModuleInstance(registration.Type);
            Require(ReferenceEquals(instance, first.GetRuntimeModuleInstance(registration.Type)) &&
                    !ReferenceEquals(instance, second.GetRuntimeModuleInstance(registration.Type)),
                "Stateful mechanism runtimes must bind once per match, including modules used only by compatibility bridges.");
            Require(registration.Type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .All(field => field.FieldType.IsInterface &&
                        (typeof(IProgramSelectionQuery).IsAssignableFrom(field.FieldType) ||
                         typeof(IProgramRuntimeOperations).IsAssignableFrom(field.FieldType))),
                $"Production runtime '{registration.Type.FullName}' must retain only narrow ports; paid cursors and mutable stores remain outside its instance.");
        }
        var capabilities = new ProgramCapabilityCatalog(typeof(GameEngine).Assembly.GetTypes());
        var adapters = MatchCapabilities(first);
        Require(ReferenceEquals(adapters.Get<IProgramWindowCursors>(), first.GetRuntimeModule<ProgramWindowCursorRuntime>()),
            "Native returns and ordinary window dispatch must share the same cursor runtime instance.");
        Require(ReferenceEquals(adapters.Get<IProgramOperationDraws>(), first.GetRuntimeModule<NativeDrawProducerRuntime>()),
            "Independent skill runtimes and legacy draw bridges must share the actual native producer.");
        var returnMethod = typeof(ProgramWindowReturnRuntime).GetMethod("ReturnDrawPhaseEndedProgramWindow",
            BindingFlags.Instance | BindingFlags.NonPublic)!;
        var returnHandler = first.BindRuntimeHandler(returnMethod, typeof(Action<ProgramSkillFrame, bool>));
        Require(ReferenceEquals(returnHandler, first.BindRuntimeHandler(returnMethod, typeof(Action<ProgramSkillFrame, bool>))),
            "Registered window returns cache the same deferred binding used by compatibility entry points.");
        var coreMethod = typeof(GameEngine).GetMethod("GetRuntimeModuleInstance", BindingFlags.Instance | BindingFlags.NonPublic,
            null, [typeof(Type)], null)!;
        var coreHandler = (Func<Type, object>)first.BindRuntimeHandler(coreMethod, typeof(Func<Type, object>));
        Require(ReferenceEquals(coreHandler.Target, first) &&
                ReferenceEquals(coreHandler(typeof(ProgramWindowCursorRuntime)), first.GetRuntimeModule<ProgramWindowCursorRuntime>()),
            "The kernel composition handler binds directly to its original engine and resolves that match's actual runtime instance.");

        var frames = adapters.Get<IProgramRuntimeFrames>();
        var original = frames.Top;
        var frame = new ProgramSkillFrame(41, 0, "fixture:runtime", "run", "hash", 0, [], [])
        {
            NativeDraw = new NativeDrawProducerReceipt(1, 1, null, SkillProgramCardSetVisibility.Private,
                new CardMoveReason("fixture.runtime")) { Recipients = [0] }
        };
        frames.Push(frame);
        Reject(() => frames.ReplaceTop(frame with { Id = 42 }));
        Reject(() => frames.Pop(frame.Id, frame.Kind));
        Require(ReferenceEquals(frames.Top, frame), "A rejected transition cannot retire or change an unconsumed paid producer.");
        frames.ReplaceTop(frame with { NativeDraw = null });
        frames.Pop(frame.Id, frame.Kind);
        Require(ReferenceEquals(frames.Top, original), "Consuming the original receipt restores the original parent exactly once.");
        CheckNativeFrameRouting(first, adapters, frames);
    }

    private static void CheckNativeFrameRouting(GameEngine engine, ProgramCapabilityRegistry adapters,
        IProgramRuntimeFrames frames)
    {
        var metadata = NativeFrameRouteCatalog.Default;
        Require(metadata.Registrations.Any(route => route.Method.DeclaringType == typeof(NativeFrameRoutingRuntime)) &&
                metadata.Registrations.Any(route => route.Method.DeclaringType == typeof(NativeDrawAttemptRuntime)),
            "Native routes bind actual independent implementations alongside compatibility handlers.");
        var methods = typeof(RoutingFixture).GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
        var fixture = new NativeFrameRouteCatalog(methods.Reverse());
        var parent = new ProgramSkillFrame(41, 0, "fixture:route", "run", "hash", 0, [], []);
        Require(fixture.TryAdvance(engine, parent), "A declined route must allow the next matching native route.");
        var selected = false;
        try { fixture.TryAdvance(engine, parent with { OwnerSeat = 1 }); }
        catch (RouteSelected) { selected = true; }
        Require(selected, "The first matching native route must win.");
        Require(!fixture.TryAdvance(engine, new DeferredTurnEndFrame(43, 0, 1, false, [])),
            "Native routing cannot dispatch another frame kind as its parent.");
        Reject(() => new NativeFrameRouteCatalog(methods.Concat(methods)));
        var automatic = new NativeFrameAutomaticStepCatalog(fixture, methods);
        Require(automatic.Registrations[NativeFrameAutomaticPhase.AfterOpening].Single().Matches(engine, parent) &&
                !automatic.Registrations[NativeFrameAutomaticPhase.AfterOpening].Single().Matches(engine,
                    new DeferredTurnEndFrame(43, 0, 1, false, [])),
            "Automatic admission reuses the local typed route without executing its handler.");
        Reject(() => new NativeFrameAutomaticStepCatalog(fixture, methods.Concat(methods)));
        Require(NativeFrameAutomaticStepCatalog.Default.Registrations.Values.SelectMany(routes => routes)
                .All(route => metadata.Registrations.Contains(route.Route)),
            "Automatic steps reference registered implementations rather than maintaining another handler list.");

        var spy = new RouteSpy();
        var routing = new NativeFrameRoutingRuntime(frames, adapters.Get<INativeFrameRoutingFacts>(), spy,
            adapters.Get<INativeProgramExecution>());
        frames.Push(parent);
        frames.Push(parent with { Id = 42 });
        routing.AdvanceFrame(41);
        routing.AdvanceProgram(41);
        routing.AdvanceTop<DeferredTurnEndFrame>();
        Reject(() => routing.AdvanceFrame(999));
        Require(spy.Calls == 0, "Retired and suspended owners never dispatch a child as their continuation.");
        routing.AdvanceFrame(42);
        Require(spy.Calls == 1 && spy.FrameId == 42, "Only the selected active frame is dispatched.");
        frames.Pop(42, parent.Kind);
        frames.Pop(41, parent.Kind);
    }

    private sealed class RouteSpy : INativeFrameRoutes
    {
        internal int Calls;
        internal long FrameId;
        internal Action<ResolutionFrame>? OnAdvance;
        public bool TryAdvance(ResolutionFrame frame) { Calls++; FrameId = frame.Id; OnAdvance?.Invoke(frame); return true; }
    }

    private sealed class FrameLifecycle : IResolutionLifecycle
    {
        public void CardUsePushed(CardUseFrame frame, CardActionContext action) { }
        public void BeforeDamageCompleted(DamageFrame frame) { }
        public void RecoveryCompleted(RecoveryFrame frame) { }
    }

    [ProgramCapability]
    private sealed class FrameProgramSpy : INativeProgramExecution
    {
        internal FrameProgramSpy(GameEngine engine) { }
        internal int Resumes;
        internal int Executions;
        public bool TryResume(long frameId) { Resumes++; return false; }
        public void Execute(long frameId) => Executions++;
    }

    private sealed class RouteSelected : Exception { }
    [ProgramRuntimeModule]
    private static class RoutingFixture
    {
        private static bool First(ProgramSkillFrame frame) => frame.OwnerSeat == 1;
        [NativeFrameRoute(typeof(ProgramSkillFrame), 1, typeof(RoutingFixture), nameof(First))]
        private static void Select(long id) => throw new RouteSelected();
        [NativeFrameRoute(typeof(ProgramSkillFrame), 2)]
        [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 1)]
        private static void Fallback(ProgramSkillFrame frame) { }
    }

    [ProgramRuntimeModule]
    private sealed class NativeZoneConsumer(IAtomicCardMovementZones zones)
    {
        internal IAtomicCardMovementZones Zones { get; } = zones;
        public int DrawCount() => Zones.CardsAt(CardLocation.DrawPile).Count;
    }

    private interface IFacts : IProgramSelectionQuery { int Value { get; } }
    private interface IFixtureCapability { int Gate(); }
    private delegate bool FixtureTryRead(bool accepted, out int value);
    private sealed record Facts(int Value) : IFacts;

    [ProgramCapability]
    private sealed class FixtureFactsAdapter : IFacts
    {
        internal FixtureFactsAdapter(GameEngine engine) { }
        public int Value { get; init; }
    }

    private sealed class FixtureAdapterFactory(Func<ProgramCapabilityCatalog.Registration, object> create) : IMatchRuntimeAdapterFactory
    {
        public object CreateAdapter(ProgramCapabilityCatalog.Registration registration) => create(registration);
        public Delegate BindCoreHandler(MethodInfo method, Type delegateType) => throw new Exception("Unexpected core handler binding.");
        public object CoreHandlerTarget(MethodInfo method) => throw new Exception("Unexpected core handler target.");
    }

    private interface ICyclicFacts : IProgramSelectionQuery { }
    [ProgramCapability, ProgramRuntimeModule]
    private sealed class CyclicModule(ICyclicFacts facts) : ICyclicFacts
    {
        private readonly ICyclicFacts _facts = facts;
    }

    [ProgramCapability, ProgramRuntimeModule]
    private sealed class FixtureModule(IFacts facts) : IFixtureCapability
    {
        private int _calls;
        public int Gate() => facts.Value + _calls;
        public void Effect() => _calls++;
        public bool TryRead(bool accepted, out int value) { value = Gate(); return accepted; }
    }

    [ProgramRuntimeModule]
    private static class StatelessModule
    {
        private const int Factor = 2;
        public static int Double(int value) => value * Factor;
    }

    [ProgramRuntimeModule]
    private sealed class EngineDependency(GameEngine engine) { private readonly object _host = engine; }

    [ProgramRuntimeModule]
    private sealed class EngineCallbackDependency
    {
        internal bool Observe(Func<GameEngine, bool> callback) => false;
    }

    private interface IEngineReturningFacts : IProgramSelectionQuery { GameEngine Engine { get; } }

    [ProgramRuntimeModule]
    private sealed class EngineReturningContractDependency(IEngineReturningFacts facts)
    {
        internal bool HasEngine() => facts.Engine is not null;
    }
    [ProgramRuntimeModule]
    private sealed class UndeclaredDependency(IDisposable value) { private readonly object _value = value; }
    [ProgramRuntimeModule]
    private static class MutableStaticModule { internal static int Counter = 0; }

    private static void Require(bool condition, string message)
    { if (!condition) throw new Exception(message); }
    private static ProgramCapabilityRegistry MatchCapabilities(GameEngine game) =>
        (ProgramCapabilityRegistry)typeof(GameEngine).GetField("_programCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
    private static void Reject(Action action, string? message = null)
    {
        try { action(); }
        catch (InvalidOperationException error)
        {
            if (message is not null) Require(error.Message == message, "A rejected frame transition changed its diagnostic.");
            return;
        }
        throw new Exception("An invalid runtime module was accepted.");
    }
}
