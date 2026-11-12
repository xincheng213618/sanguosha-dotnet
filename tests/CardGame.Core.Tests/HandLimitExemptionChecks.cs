using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class HandLimitExemptionChecks
{
    public static void ContinuousAndIssuedRelationsShareOneCountingMatcher()
    {
        CheckDefinitionShape();
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var participants = capabilities.Get<IMatchParticipantStateAccess>();
        participants.At(0).IsAlive = true;
        var cards = new Cards();
        var hand = new[] { new Card(101, CardKind.Slash, Suit.Heart, 7), new Card(102, CardKind.FireSlash, Suit.Club, 8),
            new Card(103, CardKind.Alcohol, Suit.Diamond, 9), new Card(104, CardKind.Peach, Suit.None, 8),
            new Card(105, CardKind.Dodge, Suit.None, 3), new Card(106, CardKind.Crossbow, Suit.Spade, 1) };
        foreach (var card in hand) cards.Add(card, CardLocation.Hand(0));
        var turn = new Turn();
        var facts = new Facts { UnknownCardId = 105 };
        var marks = new Marks();
        marks.Values["fixture-mark"] = [101, 102, 103];
        var entries = new Entries(cards);
        var store = new HandLimitExemptionStore(turn, facts, new Sequences());
        var runtime = new HandLimitExemptionRuntime(store, capabilities.Get<IProgramRuntimeFrames>(), turn,
            capabilities.Get<IProgramRuleLifetimes>(), facts, participants, cards, marks, entries);
        var legacySlash = new HandLimitMatch(CardKinds: [CardKind.Slash], Face: HandLimitCardFace.LegacyHandIdentity);
        Need(runtime.Matches(0, hand[2], legacySlash) &&
            runtime.Matches(0, hand[2], new(CardKinds: [CardKind.Alcohol], Face: HandLimitCardFace.LegacyHandIdentity)) &&
            !runtime.Matches(0, hand[2], new(CardKinds: [CardKind.Slash])) &&
            !runtime.Matches(0, hand[1], legacySlash),
            "Legacy hand identity retains both the physical Alcohol and its issued Slash alias without borrowing another Slash-family policy.");
        Need(runtime.Matches(0, hand[1], new(IsRed: false)) && !runtime.Matches(0, hand[3], new(IsRed: false)) &&
            !runtime.Matches(0, hand[4], new(IsRed: false)),
            "Black means Spade or Club; known suitless and unknown appearance do not become black by negating red.");
        var markInput = new List<CardKind> { CardKind.Slash };
        var selector = new HandLimitMatch("fixture-mark", markInput, HandLimitCardFace.LegacyHandIdentity);
        markInput.Add(CardKind.FireSlash);
        var recovered = JsonSerializer.Deserialize<HandLimitMatch>(JsonSerializer.Serialize(selector))!;
        Need(recovered.CardKinds.SequenceEqual([CardKind.Slash]) && recovered.CardKinds is System.Collections.IList { IsReadOnly: true } &&
            HandLimitExemptionRuntime.SameMatch(selector, recovered),
            "Declarative selectors freeze their inputs and retain the same card meaning through raw JSON recovery.");

        var source = new CardConversionSource("fixture:hand-relation", "issued", 0, "fixture:source-instance");
        HandLimitExemptionIssue Issue(int slot, string relation, HandLimitExemptionSpec spec) => new(50, 0, slot,
            relation, source, "fixture:hash", 0, spec);
        var ids = new List<int> { 101 };
        var permanentIssue = Issue(0, "permanent", new(new(), ids));
        var permanent = runtime.Grant(permanentIssue);
        ids.Add(102);
        Need(runtime.Grant(permanentIssue).Sequence == permanent.Sequence && store.Grants.Count == 1 &&
            permanent.Issue.Spec.ExactCardIds is [101],
            "One actual issuer instruction and slot owns one immutable entity relation, including issuers that have already returned.");
        Reject(() => runtime.Grant(permanentIssue with { Spec = new(new(), [102]) }));
        cards.Move(101, CardLocation.Equipment(0));
        Need(runtime.IsMember(permanent.Sequence, 0, 101) && runtime.HasEntityRelation("permanent", 101) &&
            !runtime.IsIssuedExempt(0, hand[0]) && !runtime.IsMember(permanent.Sequence, 1, 101),
            "An issued recipient relation can govern non-Hand business without authorizing another recipient or counting equipment as hand.");
        cards.Move(101, CardLocation.Hand(0));
        Need(runtime.IsIssuedExempt(0, hand[0]), "A permanent recipient entity right resumes when that entity returns to the recipient's hand.");
        foreach (var id in new[] { 1_000_000, 0, -1 })
        {
            var value = hand[0] with { Id = id };
            Need(!runtime.IsMember(permanent.Sequence, 0, id) && !runtime.HasEntityRelation("permanent", id) &&
                runtime.GetMatchingRelations("permanent", 0, id).Count == 0 && !runtime.IsIssuedExempt(0, value) &&
                runtime.GetExemptCardIds(0, [value], [new(CardKinds: [CardKind.Slash])]).Count == 0,
                "A host-only or virtual Card value has no physical relation or hand entitlement, even when its printed kind matches a declaration.");
        }
        var physicalZones = new CardZoneStore(2);
        Need(!physicalZones.TryGetLocation(1_000_000, out _), "The shared optional zone read reports an unknown physical identity.");
        Reject(() => physicalZones.GetLocation(1_000_000));
        var living = runtime.Grant(Issue(1, "living-benefit", new(new(RequireLivingHolder: true), [103])));
        participants.At(0).IsAlive = false;
        Need(runtime.IsMember(living.Sequence, 0, 103) && runtime.HasEntityRelation("living-benefit", 103) &&
            !runtime.IsIssuedExempt(0, hand[2]) && runtime.IsIssuedExempt(0, hand[0]),
            "Source-independent relation membership remains available for its real business while a declared living-holder count condition is evaluated separately.");
        participants.At(0).IsAlive = true;

        var kept = runtime.Grant(Issue(2, "kept-entry", new(new(), [106], RequireCurrentHandEntry: true,
            Until: [new(SkillStateBoundaryEvent.TurnEnded, SkillStateSubject.Target, SkillStateBoundaryTiming.Before, SkipIssued: true)])));
        Need(runtime.IsMember(kept.Sequence, 0, 106), "Keeping a card captures its actual current Hand entry.");
        cards.Move(106, CardLocation.DiscardPile);
        Need(!runtime.IsMember(kept.Sequence, 0, 106), "A kept entry ends as soon as that entity leaves Hand.");
        cards.Move(106, CardLocation.Hand(0)); entries.Next(106);
        Need(!runtime.IsMember(kept.Sequence, 0, 106), "Reentry never revives a consumed keep right.");
        store.ProcessBoundary(new(SkillStateBoundaryEvent.TurnEnded, 0, SkillStateBoundaryTiming.Before, 3));
        Need(runtime.Get(kept.Sequence) is not null, "SkipIssued does not expire a keep at the already-issued turn occurrence.");
        store.ProcessBoundary(new(SkillStateBoundaryEvent.TurnEnded, 0, SkillStateBoundaryTiming.Before, 4));
        Need(runtime.Get(kept.Sequence) is null, "The declared later recipient boundary expires the same relation used by all consumers.");
        Need(runtime.Grant(kept.Issue) is { Expired: true } repeated && repeated.Sequence == kept.Sequence &&
            !runtime.IsMember(kept.Sequence, 0, 106),
            "Reentering an expired issuer key retains its original terminal relation and never starts a fresh lifetime.");
        var nativeEnd = runtime.Grant(Issue(20, "native-end", new(new(), [104],
            NativeExpiration: SkillProgramDamageModifierExpiration.CurrentTurnEnd)));
        store.ProcessBoundary(new(SkillStateBoundaryEvent.TurnCleanup, 0, SkillStateBoundaryTiming.Before, 3));
        Need(runtime.Get(nativeEnd.Sequence) is not null && store.ExpireTurn(3, 1).Count == 0 &&
            store.ExpireTurn(3, 0).SequenceEqual([nativeEnd.Sequence]) && store.ExpireTurn(3, 0).Count == 0,
            "A legacy native lease ends once at its original native cleanup point, without an approximate boundary consuming its notification sequence early.");
        var nextOwner = runtime.Grant(Issue(21, "native-owner", new(new(), [105],
            NativeExpiration: SkillProgramDamageModifierExpiration.NextOwnerTurnStart)) with { RecipientSeat = 1 });
        Need(store.ExpireOwnerTurnStart(1, 4).Count == 0 && store.ExpireOwnerTurnStart(0, 3).Count == 0 &&
            store.ExpireOwnerTurnStart(0, 4).SequenceEqual([nextOwner.Sequence]),
            "The legacy next-owner lease uses its actual source owner and a later turn, independently of the recipient.");
        var death = runtime.Grant(Issue(22, "native-death", new(new(), [105],
            NativeExpiration: SkillProgramDamageModifierExpiration.NextOwnerTurnStart)));
        Need(store.ExpireOwnerDeath(1).Count == 0 && store.ExpireOwnerDeath(0).SequenceEqual([death.Sequence]),
            "Native source death returns only still-active matching legacy lease sequences, without emitting another event or reviving earlier ones.");

        var physicalTurn = HandLimitTurnRequirements.SetupComplete | HandLimitTurnRequirements.PositiveTurn |
            HandLimitTurnRequirements.ProgressionTurnMatches | HandLimitTurnRequirements.NoTurnEnded | HandLimitTurnRequirements.NoTurnReturned;
        var cursorTurn = HandLimitTurnRequirements.ProgressionTurnMatches | HandLimitTurnRequirements.OwnerAtCurrentCursor |
            HandLimitTurnRequirements.NoTurnReturned;
        var finishedSensitive = runtime.Grant(Issue(3, "turn-boundary", new(new(), [104], HandLimitExemptionExpiry.ActualTurnOpen,
            new(3, 0), TurnRequirements: physicalTurn)));
        var cursorSensitive = runtime.Grant(Issue(4, "cursor-boundary", new(new(), [105], HandLimitExemptionExpiry.ActualTurnOpen,
            new(3, 0), AllowBeforeFirstTurn: true, TurnRequirements: cursorTurn)));
        facts.Status = new(true, true, true, true, false);
        Need(!runtime.IsMember(finishedSensitive.Sequence, 0, 104) && runtime.IsMember(cursorSensitive.Sequence, 0, 105),
            "A delivered TurnEnded fact and an unreturned current cursor retain their distinct declared lifetimes.");
        facts.Status = new(true, true, false, false, false);
        Need(runtime.IsMember(finishedSensitive.Sequence, 0, 104) && !runtime.IsMember(cursorSensitive.Sequence, 0, 105),
            "A physical turn relation does not silently acquire a current-seat prerequisite from another rule.");
        facts.Status = new(true, false, true, false, false);
        Need(!runtime.IsMember(cursorSensitive.Sequence, 0, 105),
            "A current cursor at the original coordinates still requires its matching actual-turn progression.");
        facts.Status = new(true, true, true, false, false);
        turn.Number = 4;
        Need(!runtime.IsMember(finishedSensitive.Sequence, 0, 104) && !runtime.IsMember(cursorSensitive.Sequence, 0, 105),
            "Exact frozen turn number and owner coordinates remain part of each issued scope.");
        turn.Number = 0;
        var opening = runtime.Grant(Issue(5, "before-first-turn", new(new(), [104], HandLimitExemptionExpiry.ActualTurnOpen,
            new(0, 0), AllowBeforeFirstTurn: true, TurnRequirements: cursorTurn)));
        facts.Status = new(false, false, false, false, false);
        Need(runtime.IsMember(opening.Sequence, 0, 104), "A declared pre-first-turn exception remains explicit rather than widening other actual-turn relations.");

        turn.Number = 3; facts.Status = new(true, true, true, false, false);
        facts.Reads = 0; marks.HasReads = 0; marks.BatchReads = 0; entries.Reads = 0;
        var batch = runtime.GetExemptCardIds(0, hand, [selector, new(Mark: "fixture-mark", IsRed: false)]);
        Need(batch.SetEquals([101, 102, 103, 104, 105]) && facts.Reads == 1 && marks.HasReads == 0 && marks.BatchReads == 1 && entries.Reads == 0,
            "One hand evaluation captures turn status and each needed mark set once, without per-card history or unrelated entry reads.");
        var onlyActiveSlash = runtime.GetExemptCardIds(1, [], [legacySlash]);
        Need(onlyActiveSlash.Count == 0 && runtime.GetExemptCardIds(0, [hand[1]], [legacySlash]).Count == 0,
            "An inactive FireSlash declaration contributes no exemption through an active ordinary Slash declaration.");
        var coordinateStore = new HandLimitExemptionStore(turn, facts, new Sequences());
        var coordinateRuntime = new HandLimitExemptionRuntime(coordinateStore, capabilities.Get<IProgramRuntimeFrames>(), turn,
            capabilities.Get<IProgramRuleLifetimes>(), facts, participants, cards, marks, entries);
        var coordinates = coordinateRuntime.Grant(Issue(30, "coordinates-only", new(new(), [101],
            HandLimitExemptionExpiry.FrozenTurnCoordinates, new(3, 0))));
        facts.Reads = 0;
        Need(coordinateRuntime.IsMember(coordinates.Sequence, 0, 101) &&
            coordinateRuntime.GetExemptCardIds(0, hand, []).SetEquals([101]) &&
            coordinateRuntime.ActiveRelations("coordinates-only", 0).Count == 1,
            "Frozen coordinate relations read the same original turn and owner in membership, counting and projection.");
        coordinateStore.ProcessBoundary(new(SkillStateBoundaryEvent.TurnEnded, 0, SkillStateBoundaryTiming.Before, 3));
        Need(facts.Reads == 0, "Coordinate-only relation reads and boundary processing never scan actual-turn-ended or returned status.");
        Reject(() => new HandLimitExemptionSpec(new(), [101], HandLimitExemptionExpiry.ActualTurnOpen));
        CheckIssuedJsonFlow();
    }

    private static void CheckDefinitionShape()
    {
        SkillProgramCatalog Load(string policy) => SkillProgramCatalog.LoadCurrent(
            "{\"skills\":[{\"id\":\"fixture:hand-match\",\"revision\":1,\"cardPolicies\":[" + policy + "]}]}",
            JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { ["fixture:hand-match"] = new { name = "Hand", description = "Shared selector" } } }));
        var definition = Load("{\"id\":\"marked\",\"kind\":\"handLimitCardKindExemption\",\"handLimitMatch\":{\"mark\":\"fixture-mark\",\"requireLivingHolder\":true}}");
        Need(definition.Programs["fixture:hand-match"].CardPolicies.Single().HandLimitMatch is { Mark: "fixture-mark", RequireLivingHolder: true },
            "The existing hand-limit policy kind carries its generic immutable selector through the current loader.");
        var legacy = Load("{\"id\":\"legacy\",\"kind\":\"handLimitCardKindExemption\",\"cardKinds\":[\"alcohol\"]}");
        Need(!JsonSerializer.Serialize(legacy.Programs["fixture:hand-match"].CardPolicies.Single()).Contains("HandLimitMatch", StringComparison.Ordinal),
            "Omitting the additive selector preserves existing definition serialization.");
        Reject(() => Load("{\"id\":\"bad\",\"kind\":\"minimumHandLimit\",\"value\":2,\"handLimitMatch\":{}}"));
        Reject(() => Load("{\"id\":\"bad\",\"kind\":\"handLimitCardKindExemption\"}"));
        Reject(() => Load("{\"id\":\"bad\",\"kind\":\"ignoreUseDistance\"}"));
        Reject(() => Load("{\"id\":\"bad\",\"kind\":\"handLimitCardKindExemption\",\"cardKinds\":[\"slash\"],\"handLimitMatch\":{}}"));
        foreach (var missing in new[] { "", ",\"turnRequirements\":[]", ",\"turnRequirements\":[\"none\"]" })
            Reject(() => SkillProgramCatalog.LoadCurrent(
                "{\"skills\":[{\"id\":\"fixture:hand-match\",\"revision\":1,\"activations\":[{\"id\":\"invalid-open\",\"minCards\":0,\"maxCards\":0,\"minTargets\":0,\"maxTargets\":0,\"targetKind\":\"anyLiving\",\"effects\":[{\"op\":\"grantHandLimitExemption\",\"target\":\"owner\",\"relationId\":\"invalid-open\",\"handLimitMatch\":{\"cardKinds\":[\"peach\"]},\"expiry\":\"actualTurnOpen\"" + missing + "}]}]}]}",
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { ["fixture:hand-match"] = new { name = "Hand", description = "Explicit remaining lifetime" } } })));
    }

    private const string JsonSkill = "fixture:hand-exemption-issued", JsonQuiet = "fixture:hand-exemption-quiet";
    private const string JsonOwner = "fixture:hand-exemption-owner", JsonPeer = "fixture:hand-exemption-peer";
    private static void CheckIssuedJsonFlow()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new IssuedFixture());
        var setup = new ShanheBattleSetup(31, 1, "hand-exemption-json", JsonOwner, 8, 0, 0, 0, 0, [],
            [new(CardKind.Peach, Suit.Heart, 3), new(CardKind.Dodge, Suit.Club, 4)], [],
            [new(JsonPeer, Hp: 8) { StartingHand = [] }, new(JsonPeer, Hp: 8) { StartingHand = [] }],
            [new(JsonPeer, Hp: 8) { StartingHand = [] }], new string('c', 64)) { StartingResourcesExpireOnUse = false };
        var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry);
        ProgramCapabilityRegistry Capabilities() => (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        void Submit(GameCommand command)
        { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected shared hand-limit node command."); }
        int[] chosen = [];
        void ReachPlay()
        {
            var remaining = chosen.ToHashSet();
            for (var step = 0; step < 100; step++)
            {
                var prompt = Prompt();
                if (prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                if (prompt is null) Submit(new AdvanceOneStepCommand(game.Revision));
                else if (prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip") is { } skip)
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, skip.Id, game.Revision));
                else if (prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards" &&
                    choice.Cards is [var cardId] && remaining.Contains(cardId)) is { } selected)
                {
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, selected.Id, game.Revision));
                    remaining.ExceptWith(selected.Cards);
                }
                else throw new InvalidOperationException("Unexpected hand-limit node boundary " + JsonSerializer.Serialize(prompt));
            }
            throw new InvalidOperationException("The shared hand-limit node missed its bounded native Play return.");
        }
        Submit(new StartGameCommand()); ReachPlay();
        chosen = game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).ToArray();
        Need(chosen.Length == 2, "The actual issued-node fixture starts with two fixed physical Hand cards.");
        Submit(new UseProgramSkillCommand(0, JsonSkill, "empty", [], [], game.Revision, Prompt()!.PromptId)); ReachPlay();
        Need(Capabilities().Get<IHandLimitExemptions>().ActiveRelations("fixture:empty", 0).Count == 0,
            "A real empty bound capture returns no grant; it never becomes a wildcard over future hand cards.");
        Submit(new UseProgramSkillCommand(0, JsonSkill, "issue", [], [], game.Revision, Prompt()!.PromptId)); ReachPlay();
        void CheckIssued()
        {
            var shared = Capabilities().Get<IHandLimitExemptions>();
            Need(game.CreateSnapshot(0).Players[0].Skills?.All(skill => skill.ContentId != JsonSkill) == true &&
                chosen.All(id => shared.GetMatchingRelations("fixture:permanent", 0, id).Count == 1 &&
                    shared.GetMatchingRelations("fixture:entry", 0, id).Count == 1) &&
                shared.ActiveRelations("fixture:entry", 0).Single().HandEntries.Count == 2,
                "Real capture and both JSON grant handlers retain their original recipient entities and entry facts after the issuing skill is lost.");
        }
        CheckIssued();
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        CheckIssued();
    }

    private sealed class IssuedFixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-hand-exemption-json", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = """
                {"skills":[
                  {"id":"fixture:hand-exemption-quiet","revision":1,"triggers":[{"id":"fixed-draw","window":"drawPhaseStarting","subject":"owner","optional":false,
                    "effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"fixture:hand-exemption-issued","revision":1,"activations":[
                    {"id":"empty","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"captureCards","target":"owner","resultBind":"empty","cardsExpr":["filter",["publicRevealCards","all"],"emptyCandidate",false]},
                       {"op":"grantHandLimitExemption","target":"owner","relationId":"fixture:empty","cardsBind":"empty"}]},
                    {"id":"issue","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"selectOwnedCards","target":"owner","amount":2,"zones":["hand"],"resultBind":"chosen"},
                       {"op":"revealBoundCards","target":"owner","sourceBind":"chosen"},
                       {"op":"grantHandLimitExemption","target":"owner","relationId":"fixture:permanent","cardsBind":"chosen"},
                       {"op":"grantHandLimitExemption","target":"owner","relationId":"fixture:entry","cardsBind":"chosen","requireCurrentHandEntry":true,
                        "until":[{"event":"turnEnded","participant":{"kind":"target"},"timing":"before","skipIssued":true}]},
                       {"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["fixture:hand-exemption-issued"]}}]},
                    {"id":"future-kind","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"grantHandLimitExemption","target":"owner","relationId":"fixture:future-kind","handLimitMatch":{"cardKinds":["peach"]}}]}]}]}
                """;
            var catalog = SkillProgramCatalog.LoadCurrent(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { JsonQuiet, JsonSkill }.ToDictionary(id => id, id => new { name = id, description = "Shared issued hand relation" })
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(JsonOwner, "Issued hand owner", "", JsonSkill, "shu", 8, [JsonQuiet]));
            builder.AddGeneral(new(JsonPeer, "Issued hand peer", "", JsonQuiet, "shu", 8));
            builder.AddDeck(new("fixture:hand-exemption-deck", "Fixed hand exemption cards", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Hand exemption fixture", 4, 4, new Dictionary<string, int>(),
                "fixture:hand-exemption-deck", GeneralPoolIds: [JsonOwner, JsonPeer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }

    private sealed class Turn : IProgramTurnQueries
    {
        internal int Number { get; set; } = 3;
        public int TurnNumber => Number;
        public int CurrentSeat => 0;
        public int TurnOwnerSeat => 0;
        public int TurnOwnerTurn => 1;
    }
    private sealed class Sequences : IHandLimitExemptionSequences
    {
        private long _next;
        public long Next() => ++_next;
    }
    private sealed class Facts : IHandLimitExemptionCardFacts
    {
        internal HandLimitTurnStatus Status { get; set; } = new(true, true, true, false, false);
        internal int Reads { get; set; }
        internal int UnknownCardId { get; set; }
        public HandLimitTurnStatus TurnStatus { get { Reads++; return Status; } }
        public CardKind LegacyHandKind(int holderSeat, Card card) => card.Kind == CardKind.Alcohol ? CardKind.Slash : card.Kind;
        public Suit? EffectiveSuit(int holderSeat, Card card) => card.Id == UnknownCardId ? null : card.Suit;
    }
    private sealed class Cards : ICardZoneQueries
    {
        private readonly Dictionary<int, (Card Card, CardLocation Location)> _cards = [];
        internal void Add(Card card, CardLocation location) => _cards.Add(card.Id, (card, location));
        internal void Move(int id, CardLocation location) => _cards[id] = (_cards[id].Card, location);
        public int Count(CardLocation location) => _cards.Values.Count(item => item.Location == location);
        public IReadOnlyList<Card> CardsAt(CardLocation location) => _cards.Values.Where(item => item.Location == location).Select(item => item.Card).ToArray();
        public bool TryGetLocation(int cardId, out CardLocation location)
        {
            var known = _cards.TryGetValue(cardId, out var actual);
            location = actual.Location;
            return known;
        }
        public CardLocation GetLocation(int cardId) => _cards[cardId].Location;
    }
    private sealed class Entries(Cards cards) : ICurrentHandEntryQueries
    {
        private readonly Dictionary<int, long> _entries = [];
        internal int Reads { get; set; }
        internal void Next(int id) => _entries[id] = _entries.GetValueOrDefault(id, 1) + 1;
        public long? CurrentHandEntry(int owner, int cardId)
        { Reads++; return cards.GetLocation(cardId) == CardLocation.Hand(owner) ? _entries.GetValueOrDefault(cardId, 1) : null; }
    }
    private sealed class Marks : ICardEntityMarkQueries
    {
        internal Dictionary<string, HashSet<int>> Values { get; } = [];
        internal int HasReads { get; set; }
        internal int BatchReads { get; set; }
        public bool Has(int cardId, string mark) { HasReads++; return Values.GetValueOrDefault(mark)?.Contains(cardId) == true; }
        public int? InitialOwnerSeat(int cardId, string mark) => null;
        public IReadOnlyList<int> MarkedCardIds(string mark) { BatchReads++; return Values.GetValueOrDefault(mark)?.ToArray() ?? []; }
        public int CountMarked(IEnumerable<int> cardIds, string mark) => cardIds.Count(id => Has(id, mark));
        public int CountMarked(int seat, string mark, CardZoneKind zone) => throw new NotSupportedException();
        public IReadOnlyList<(string Mark, string Label)> MarkedNames(int cardId) => [];
    }
    private static void Reject(Action action)
    { try { action(); } catch (InvalidOperationException) { return; } throw new InvalidOperationException("An invalid hand-limit declaration was accepted."); }
    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
