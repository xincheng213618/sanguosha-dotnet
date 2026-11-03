using System.Text.Json;
using CardGame.Core;
using Scene = BoundaryLiaoHuaScenario;

internal static class ProgramExpressionChecks
{
    private const string Draw = """{"op":"draw","target":"owner","amount":1,"condition":{"kind":"expression","expr":EXPR}}""";

    public static void LoaderAndEvaluation()
    {
        var short1 = Load(Draw.Replace("EXPR", """["<",["handCount","owner"],["maxHp","owner"]]"""));
        var short2 = Load(Draw.Replace("EXPR", """["<", ["handCount", "owner"], ["maxHp", "owner"]]"""));
        var other = Load(Draw.Replace("EXPR", """["<=",["handCount","owner"],["maxHp","owner"]]"""));
        Need(short1.GameplayHash == short2.GameplayHash && short1.GameplayHash != other.GameplayHash,
            "Expression content joins the canonical gameplay hash without whitespace sensitivity.");
        Need(short1.Activations[0].Effects[0].Condition is { Kind: SkillProgramConditionKind.Expression, Expression.Op: "<" },
            "The loader freezes the typed expression on the instruction condition.");

        foreach (var (expr, fragment) in new[]
        {
            ("""["sum",1,2]""", "unknown operator"),
            ("""["<","owner",1]""", "expected an integer"),
            ("""["alive","it"]""", "only available inside a participant-set"),
            ("""["hp","owner"]""", "expected a boolean"),
            ("""["and",true]""", "requires 2 to 8 arguments"),
            ("""["<",["hp","owner"],100]""", "integer from -99 to 99"),
            ("""[">",["publicPileCount","owner","invalid"],0]""", "requires a skill identifier"),
            ("""[">",["publicPileCount","owner",1],0]""", "requires a skill identifier"),
            ("""["ruleWounded","it"]""", "only available inside a participant-set"),
            ("""["sameUse",["last","living"],"currentUse"]""", "expected a card-use fact set"),
            ("""["any",["takeLast",["cardUses","window","accepted"],true],true]""", "expected an integer"),
            ("""["usePhaseIs","currentUse","play","latest"]""", "stage must be accepted"),
            ("""["any",["intersect",["cardUses","window","accepted"],"living"],true]""", "expected a participant set"),
        })
            Rejects(Draw.Replace("EXPR", expr), fragment);
        Rejects("""{"op":"draw","target":"owner","amount":1,"condition":{"kind":"expression","value":1,"expr":true}}""", "accepts only kind and expr");
        Rejects("""{"op":"draw","target":"owner","amount":1,"condition":{"kind":"expression"}}""", "expression requires expr");
        Rejects("""{"op":"draw","target":"owner","amount":1}""", "requires a composition instruction condition",
            activationCondition: """{"kind":"expression","expr":true}""");
        var excessive = "true";
        for (var depth = 0; depth <= ProgramExpressions.MaximumDepth; depth++) excessive = "[\"not\"," + excessive + "]";
        Rejects(Draw.Replace("EXPR", excessive), "expression nesting exceeds");

        var world = new SkillContextExpressionWorld(
            new PlayerSkillContext(0, 4, 4, 2, TurnPhase.Play),
            new PlayerSkillContext(1, 0, 3, 1, TurnPhase.Play) { HpUndefined = true });
        Need(Eval("""["<",["handCount","selectedTarget"],["maxHp","selectedTarget"]]""", world) == true &&
             Eval("""[">",["hp","selectedTarget"],0]""", world) is null &&
             Eval("""["not",[">",["hp","selectedTarget"],0]]""", world) is null &&
             Eval("""["or",[">",["hp","selectedTarget"],0],true]""", world) == true &&
             Eval("""["and",[">",["hp","selectedTarget"],0],false]""", world) == false &&
             Eval("""["all","livingOthers",true]""", world) is null &&
             Eval("""["==",["-",["max",["hp","owner"],1],["lostHp","owner"]],4]""", world) == true,
            "Undefined HP and unknown sets stay unknown through not and short-circuit only on a decided and/or operand.");
        ConditionalGrantFactsUseSharedReadsAndOrderedSuitRewrites();
        CardUseFactsComposeThroughExistingExpressions();
        ActualUseSlicesKeepTheirMomentKindAndAiBoundary();
        HandGainCardFactsComposeWithoutRefreshingTheirSnapshots();
        PublicRevealCardsKeepScopesAndUnknownFaces();
    }

    private static void PublicRevealCardsKeepScopesAndUnknownFaces()
    {
        var phase = new ActualDiscardRecoveryPhaseKey(101, 2, 0, 0, ActualDiscardRecoveryPhaseKind.Play);
        var other = new ActualDiscardRecoveryPhaseKey(102, 2, 0, 1, ActualDiscardRecoveryPhaseKind.Play);
        PublicCardRevealHistoryFact[] facts =
        [
            new(1, 1, 1, null, 5, 21, 0, 1, PublicCardRevealPurpose.Explicit),
            new(2, 2, 0, phase, 10, 11, 0, 10, PublicCardRevealPurpose.Explicit),
            new(3, 2, 0, phase, 11, 11, 1, 11, PublicCardRevealPurpose.MaterialDisclosure),
            new(4, 2, 0, phase, 12, 12, 0, 12, PublicCardRevealPurpose.Explicit),
            new(5, 2, 0, other, 13, 13, 1, 13, PublicCardRevealPurpose.Explicit)
        ];
        var world = new PublicRevealFactsWorld(facts, phase);
        Need(world.PublicRevealCards("all", "all") is { Count: 4 } all &&
            all.Single(card => card.CardId == 11) is { CapturedLocation.OwnerSeat: 1, SourceEntrySequence: 11 } &&
            world.PublicRevealCards("actualTurn", "all")!.Select(card => card.CardId).ToHashSet().SetEquals([11, 12, 13]) &&
            world.PublicRevealCards("actualPhase", "explicit") is { Count: 2 } explicitPhase &&
            explicitPhase.Single(card => card.CardId == 11) is { CapturedLocation.OwnerSeat: 0, SourceEntrySequence: 10 },
            "Scopes retain actual turn/phase identity; purpose is filtered before selecting the latest public residency for each physical entity.");
        Need(Eval("""["==",["mod",7,2],1]""", world) == true &&
            Eval("""["==",["mod",-7,2],-1]""", world) == true &&
            Eval("""["==",["mod",7,0],0]""", world) is null &&
            Eval("""["any",["publicRevealCards","all"],["cardKindIs","card",["slash"]]]""", world) is null &&
            Eval("""["not",["any",["publicRevealCards","all"],["cardSuitIs","card",["heart"]]]]""", world) is null &&
            Eval("""["all",["publicRevealCards","all"],["cardActualGain","card"]]""", world) is null &&
            Eval("""["==",["count",["publicRevealCards","actualPhase"],true],0]""", new PublicRevealFactsWorld(facts, null)) is null,
            "Public identities do not invent later private faces or gain status; absent phases and zero divisors remain unknown through expressions.");
        var owner = new PlayerSkillContext(0, 4, 4, 1, TurnPhase.Play);
        var ai = new SkillContextExpressionWorld(owner, null, ownerWindowFacts: world, bindingCards: _ => null);
        Need(ai.CardAt(11, 1, CardZoneKind.Hand) is null && ai.CardAt(12, 0, CardZoneKind.Hand) == true &&
            ai.CardStillAtSource(new(11, CapturedLocation: CardLocation.Hand(1), SourceEntrySequence: 11)) is null &&
            ai.CardStillAtSource(new(12, CapturedLocation: CardLocation.Hand(0), SourceEntrySequence: 12)) == true &&
            ai.CardStillAtSource(new(90, CapturedLocation: CardLocation.Equipment(1), SourceEntrySequence: 1)) == true &&
            ai.BoundCards("unknown-simulated-result") is null,
            "An AI's unknown simulated result does not fall back to live bindings, and public identities cannot query a foreign hidden Hand residency.");
        var estimate = new ProgramAiEstimateContext(owner, false, ownerWindowFacts: new PublicRevealFactsWorld([facts[1]], phase));
        SkillProgramEffect Capture(string result, string expression)
        {
            using var document = JsonDocument.Parse(expression);
            return new(SkillProgramEffectOp.CaptureCards, SkillProgramEffectTarget.Owner, 0,
                new(SkillProgramConditionKind.Always, 0, []), resultBind: result)
                { CardSetExpression = ProgramExpressions.ParseCardSet(document.RootElement, "cards") };
        }
        bool Still(string binding) => estimate.EvaluateExpressionCondition(new(SkillProgramConditionKind.Expression, 0, [],
            expression: Bool("[\"any\",[\"boundCards\",\"" + binding + "\"],[\"cardStillAtSource\",\"card\"]]")));
        estimate.CaptureExpressionCards(Capture("origin", """["publicRevealCards","all"]"""));
        estimate.Move(new(SkillProgramEffectOp.MoveBoundCards, SkillProgramEffectTarget.Owner, 0,
            new(SkillProgramConditionKind.Always, 0, []), sourceBind: "origin", resultBind: "same", destination: SkillProgramCardDestination.OwnerHand));
        Need(Still("origin") && Still("same"), "A simulated same-zone no-op preserves both original and derived physical residency.");
        estimate.Move(new(SkillProgramEffectOp.MoveBoundCards, SkillProgramEffectTarget.Owner, 0,
            new(SkillProgramConditionKind.Always, 0, []), sourceBind: "same", resultBind: "moved", destination: SkillProgramCardDestination.DiscardPile));
        Need(!Still("origin"), "A real simulated departure invalidates the old source even if the live source world still knows it there.");
        estimate.CaptureExpressionCards(Capture("unknown-origin", """["filter",["boundCards","moved"],["cardStillAtSource","card"]]"""));
        Need(estimate.PathCancelled, "A simulated new result without an actual entry token stays unknown rather than adopting a live token.");
        foreach (var expression in new[] { """["publicRevealCards","round"]""", """["publicRevealCards","all","private"]""", """["boundCards",0]""" })
        {
            using var document = JsonDocument.Parse(expression);
            try { ProgramExpressions.ParseCardSet(document.RootElement, "cards"); }
            catch (InvalidOperationException) { continue; }
            throw new InvalidOperationException("Invalid public-card collection was accepted: " + expression);
        }
    }

    private sealed class PublicRevealFactsWorld(IReadOnlyList<PublicCardRevealHistoryFact> facts,
        ActualDiscardRecoveryPhaseKey? phase) : IProgramExpressionWorld
    {
        public int? Participant(string name) => name == "owner" ? 0 : null;
        public IReadOnlyList<int>? ParticipantSet(string name) => null;
        public int? Stat(string name, int seat) => null;
        public bool? Flag(string name, int seat) => null;
        public IReadOnlyList<ProgramExpressionCardFact>? PublicRevealCards(string scope, string purpose) =>
            ProgramPublicRevealCards.Read(facts, scope, purpose, 2, 0, phase);
        public bool? CardAt(int cardId, int seat, CardZoneKind zone) => true;
        public bool? CardStillAtSource(ProgramExpressionCardFact card) => true;
        public IReadOnlyList<ProgramExpressionCardFact>? BoundCards(string name) => PublicRevealCards("all", "all");
    }

    private static void HandGainCardFactsComposeWithoutRefreshingTheirSnapshots()
    {
        var incoming = new List<HandGainCardSnapshot>
        {
            new(11, CardKind.Dodge, CardKind.FireSlash, Suit.Heart),
            new(12, CardKind.Slash, CardKind.Dodge, Suit.Spade),
            new(13, CardKind.Slash, CardKind.Slash, Suit.Heart, IsActualGain: false)
        };
        var previous = new List<HandGainCardSnapshot>
        {
            new(21, CardKind.Slash, CardKind.FireSlash, Suit.Heart),
            new(22, CardKind.Slash, CardKind.Dodge, Suit.Spade)
        };
        var gain = new HandGainOccurrence(10, 0, 2, new(2, TurnPhase.Draw, 1), false, true)
        { Incoming = incoming, PreviousHand = previous, InitiallyAlive = true };
        var world = new HandGainFactsWorld(gain);
        IReadOnlyList<ProgramExpressionCardFact>? Cards(string expression, IProgramExpressionWorld facts)
        {
            using var document = JsonDocument.Parse(expression);
            var parsed = ProgramExpressions.ParseCardSet(document.RootElement, "cards");
            Need(parsed.Type == ProgramExpressionType.CardSet, "A gain collection has its own card-set type.");
            return ProgramExpressions.EvaluateCardSet(parsed, facts);
        }
        const string allIncoming = """["gainCards","owner","incoming"]""";
        const string actualIncoming = """["filter",["gainCards","owner","incoming"],["cardActualGain","card"]]""";
        const string matchingWitness = """
            ["filter",["gainCards","owner","incoming"],"incoming",
              ["any",["gainCards","owner","previousHand"],"old",
                ["and",["sameCardKind","incoming","old"],["sameCardSuit","incoming","old"]]]]
            """;
        Need(Cards(allIncoming, world)!.Select(card => card.CardId).SequenceEqual([11, 12, 13]) &&
            Cards(actualIncoming, world)!.Select(card => card.CardId).SequenceEqual([11, 12]) &&
            Eval("""["==",["count",["gainCards","owner","incoming"],["cardKindIs","card",["slash","fireSlash","thunderSlash"]]],2]""", world) == true &&
            Eval("""["all",["gainCards","owner","previousHand"],["cardActualGain","card"]]""", world) == true,
            "Physical arrivals include an original-material return; actual-gain filtering and the shared Slash kind matcher remain separate reads.");
        Need(Cards(matchingWitness, world)!.Select(card => card.CardId).SequenceEqual([11, 12]) &&
            Eval("""["==",["count",["gainCards","owner","incoming"],["any",["gainCards","owner","previousHand"],"old",["sameCardSuit","card","old"]]],3]""", world) == true &&
            Eval("""["all","livingOthers",["any",["gainCards","owner","incoming"],"candidate",["and",["cardActualGain","candidate"],["not",["same","it","owner"]]]]]""", world) == true,
            "Named card iterators retain both outer card and participant scopes; matching uses each frozen effective kind and suit, rather than its printed face.");
        Need(Eval("""["and",["gainInitiallyAlive","owner"],["gainFirstOutsideDraw","owner"],["gainPhaseIs","owner","draw","selectedTarget"],["not",["gainPhaseIs","owner","draw","owner"]]]""", world) == true,
            "Initial admission and the sampled phase actor remain independent of completion-time life and the actual turn owner.");

        Need(Eval("""["and",["==",["gainActualTurn","owner"],2],["not",["gainRecipientAlive","owner"]],["gainHasSource","owner","initial"],["not",["gainHasSource","owner","completion"]]]""", world) == true &&
            Cards("""["filter",["gainCards","owner","incoming"],["and",["cardAt","card","owner","hand"],["cardCanDiscard","owner","card"],["cardInBinding","card","accepted"]]]""", world)!.Select(card => card.CardId).SequenceEqual([11]),
            "Source-time admission, current card location, discard legality and accepted membership remain independent shared reads.");
        Need(Eval("""["all",["filter",["gainCards","owner","incoming"],["cardKindIs","card",["slash","fireSlash","thunderSlash"]]],"newCard",["any",["gainCards","owner","previousHand"],"oldCard",["sameCardName","newCard","oldCard"]]]""", world) == true,
            "Basic card-name identity unifies elemental Slashes while exact effective-kind comparison remains available separately.");
        world.Displaced = true;
        Need(Eval("""["any",["gainCards","owner","incoming"],["and",["cardAt","card","owner","hand"],["cardCanDiscard","owner","card"]]]""", world) == false &&
            Cards(allIncoming, world)!.Select(card => card.CardId).SequenceEqual([11, 12, 13]),
            "Live eligibility may change without refreshing the frozen acquisition's entities or faces.");
        world.Displaced = false;
        incoming[0] = incoming[0] with { EffectiveKind = CardKind.Peach, EffectiveSuit = Suit.Diamond };
        incoming.Clear(); previous.Clear();
        Need(Cards(matchingWitness, world)!.Select(card => card.CardId).SequenceEqual([11, 12]) &&
            Eval("""["any",["gainCards","owner","incoming"],["and",["cardKindIs","card",["slash","fireSlash","thunderSlash"]],["cardSuitIs","card",["heart"]]]]""", world) == true,
            "Later source-list changes cannot refresh either acquisition or old-hand card identities, kinds or suits.");

        var unknown = new HandGainFactsWorld(null);
        var empty = new HandGainFactsWorld(gain with { Incoming = [], PreviousHand = [], Timing = null });
        Need(Cards(allIncoming, unknown) is null && Cards(actualIncoming, unknown) is null &&
            Eval("""["not",["any",["gainCards","owner","incoming"],true]]""", unknown) is null &&
            Eval("""["not",["gainInitiallyAlive","owner"]]""", unknown) is null &&
            Eval("""["gainHasSource","owner","completion"]""", unknown) is null &&
            Eval("""["any",["gainCards","owner","incoming"],["cardInBinding","card","missing"]]""", world) is null &&
            Eval("""["gainFirstOutsideDraw","selectedTarget"]""", world) is null &&
            Eval("""["gainPhaseIs","owner","draw","owner"]""", empty) is null,
            "Absent acquisitions, recipients and sampled timing remain unknown, including through negation and filtering.");
        Need(Cards(actualIncoming, empty) is { Count: 0 } &&
            Eval("""["and",["==",["count",["gainCards","owner","incoming"],true],0],["all",["gainCards","owner","incoming"],false],["not",["any",["gainCards","owner","incoming"],true]]]""", empty) == true,
            "A real empty acquisition remains a known empty card set with ordinary count and quantifier semantics.");

        foreach (var invalid in new[]
        {
            """["gainInitiallyAlive",1]""",
            """["gainHasSource","owner","now"]""",
            """["cardAt","owner","owner","hand"]""",
            """["any",["gainCards","owner","incoming"],["cardAt","card","owner","missingZone"]]""",
            """["any",["gainCards","owner","incoming"],["cardAt","card","owner","discardPile"]]""",
            """["any",["gainCards","owner","incoming"],["cardCanDiscard","card","owner"]]""",
            """["any",["gainCards","owner","incoming"],["cardInBinding","card",0]]""",

            """["cardActualGain","card"]""",
            """["any",["gainCards","owner","incoming"],["alive","card"]]""",
            """["any","living",["cardActualGain","it"]]""",
            """["any",["gainCards","owner","missing"],true]""",
            """["any",["gainCards","owner","incoming"],["cardKindIs","card",["notACardKind"]]]""",
            """["any",["gainCards","owner","incoming"],["cardSuitIs","card",["notASuit"]]]""",
            """["any",["gainCards","owner","incoming"],"owner",true]""",
            """["any",["gainCards","owner","incoming"],"entry",["any",["gainCards","owner","previousHand"],"entry",true]]""",
            """["and",["any",["gainCards","owner","incoming"],"entry",true],["cardActualGain","entry"]]"""
        })
        {
            try { Bool(invalid); }
            catch (InvalidOperationException) { continue; }
            throw new InvalidOperationException("Invalid typed or scoped card expression was accepted: " + invalid);
        }
        using var scalar = JsonDocument.Parse("true");
        try { ProgramExpressions.ParseCardSet(scalar.RootElement, "cards"); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("A boolean was accepted as a card-set expression.");
    }

    private sealed class HandGainFactsWorld(HandGainOccurrence? gain) : IProgramExpressionWorld
    {
        public int? Participant(string name) => name switch { "owner" => 0, "selectedTarget" => 1, "turnOwner" => 2, _ => null };
        public IReadOnlyList<int>? ParticipantSet(string name) => name == "livingOthers" ? [1, 2] : null;
        public int? Stat(string stat, int seat) => null;
        public bool? Flag(string flag, int seat) => null;
        public HandGainOccurrence? HandGain(int recipient) => gain?.RecipientSeat == recipient ? gain : null;
        public bool Displaced { get; set; }
        public bool? GainHasSource(int recipient, string moment) => HandGain(recipient) is null ? null : moment == "initial";
        public bool? CardAt(int cardId, int seat, CardZoneKind zone) => gain is null ? null : !Displaced && seat == 0 && zone == CardZoneKind.Hand && cardId is 11 or 21;
        public bool? CardCanDiscard(int seat, int cardId) => gain is null ? null : !Displaced && seat == 0 && cardId == 11;
        public bool? CardInBinding(int cardId, string name) => name == "accepted" ? cardId == 11 : null;

    }

    private static void ConditionalGrantFactsUseSharedReadsAndOrderedSuitRewrites()
    {
        SkillProgramCardPolicy Rewrite(string id, Suit from, Suit to) => new(id,
            SkillProgramCardPolicyKind.RewriteSuit, [], [], 0, from, to, new(SkillProgramConditionKind.Always, 0, []));
        var policies = new[] { Rewrite("first", Suit.Spade, Suit.Heart), Rewrite("second", Suit.Heart, Suit.Diamond) };
        Card[] cards = [new(1, CardKind.Slash, Suit.Spade, 1), new(2, CardKind.Slash, Suit.Heart, 2),
            new(3, CardKind.Slash, Suit.Diamond, 3), new(4, CardKind.Slash, Suit.None, 4)];
        Need(CardSuitRewrites.Apply(Suit.Spade, policies) == Suit.Diamond &&
            CardSuitRewrites.CountEquipmentSuits(cards, policies) == 1 &&
            CardSuitRewrites.CountEquipmentSuits(cards, policies.Reverse().ToArray()) == 2,
            "Equipment suit facts apply the admitted root policies in order, collapse rewritten suits and ignore None.");
        foreach (var contextual in new[] { policies[0] with { OwnerRole = Role.Lord },
                     policies[0] with { Condition = new(SkillProgramConditionKind.Expression, 0, [], expression: Bool("true")) } })
        {
            var rejected = false;
            try { CardSuitRewrites.CountEquipmentSuits([], [contextual]); }
            catch (InvalidOperationException) { rejected = true; }
            Need(rejected, "Context-dependent suit rewrites are rejected even when the equipment zone is empty.");
        }
        var owner = new CharacterState { Seat = 0, Name = "Facts", IsHuman = true, Role = Role.Rebel, RoleRevealed = true,
            General = new("fixture:facts", "Facts", "", []), GeneralSelected = true, GeneralRevealed = true, Hp = 4, MaxHp = 4 };
        var world = new SkillStateConditionWorld(new(0, 0), owner, wounded: new WoundedFacts(),
            equipmentSuitCount: _ => CardSuitRewrites.CountEquipmentSuits(cards, policies));
        const string facts = """["and",[">=",["count","living",["ruleWounded","it"]],1],["==",["equipmentSuitCount","owner"],1]]""";
        Load(Draw.Replace("EXPR", facts));
        Need(Eval(facts, world) == true && owner.Hp == owner.MaxHp && ConditionalSkillGrantParser.IsCondition(Bool(facts)) &&
            !SkillStateDescriptor.IsPersistentCondition(Bool(facts)),
            "Dependent-grant conditions use rule-wounded and nonrecursive suit facts; raw recipient restrictions cannot depend on skill effectiveness.");
        Rejects($$"""{"op":"skillState","target":"owner","action":"suppress","match":{},"while":{{facts}}}""", "persistent");
        Need(Eval("""["not",["ruleWounded","owner"]]""", new SkillStateConditionWorld(new(0, 0), owner)) is null &&
            Eval("""[">",["equipmentSuitCount","selectedTarget"],0]""", new SkillContextExpressionWorld(new(0, 4, 4, 0, TurnPhase.Play), null)) is null,
            "Unavailable wounded facts and absent participants remain unknown rather than false or zero.");
        var piles = new PileFacts();
        const string pileCondition = """["and",["==",["publicPileCount","owner","fixture:pile"],3],["==",["publicPileCount","owner","fixture:empty"],0]]""";
        Load(Draw.Replace("EXPR", pileCondition));
        Need(Eval(pileCondition, piles) == true && SkillStateDescriptor.IsPersistentCondition(Bool(pileCondition)) &&
            Eval("""["not",[">",["publicPileCount","selectedTarget","fixture:pile"],0]]""", piles) is null &&
            SkillStateMatcher.Matches(new(filter: Bool(pileCondition)), new("grant", "fixture:skill", "instance", "acquired:fixture"),
                owner, new("fixture:skill", "Skill", ""), bindings: piles),
            "Named pile reads preserve known empty versus unknown, and skill metadata matching forwards the same raw fact.");
    }

    private static void CardUseFactsComposeThroughExistingExpressions()
    {
        const long order = (long)int.MaxValue + 100;
        var current = new CardUseHistoryFact(10, order, 0, 0, CardActionType.Use, false, [0, 2], true, order);
        CardUseHistoryFact[] latest = [new(null, 1, 0, 0, CardActionType.Use, false, [2], false, order - 1),
            current, new(20, order + 1, 0, 0, CardActionType.Response, true, [3], false, order + 1),
            new(30, null, 2, 2, CardActionType.Use, false, [1], false, order + 2),
            new(40, order + 3, 1, 1, CardActionType.Response, false, [1], false, order + 3)];
        CardUseHistoryFact[] observations = [latest[0], current with { Targets = [1] }, .. latest.Skip(1)];
        var world = new CardUseFactsWorld(latest, observations, current);
        const string targets = """["filter",["targets",["filter",["cardUses","window","observations"],["and",["useEstablished","use"],["same",["useActor","use"],"owner"]]]],["not",["same","it","owner"]]]""";
        IReadOnlyList<int>? Seats(string expression)
        {
            using var document = JsonDocument.Parse(expression);
            return ProgramExpressions.EvaluateParticipantSet(ProgramExpressions.ParseParticipantSet(document.RootElement, "targets"), world);
        }
        Need(Seats(targets)!.SequenceEqual([1, 2]) &&
            Seats(targets.Replace("observations", "latest", StringComparison.Ordinal))!.SequenceEqual([2]) &&
            Seats("""["filter","livingOthers",["any",["cardUses","window","observations"],["and",["useEstablished","use"],["contains",["useTargets","use"],"it"]]]]""")!.SequenceEqual([1, 2]) &&
            Eval("""["==",["count",["cardUses","window","latest"],["any",["useTargets","use"],["same","it",["useActor","use"]]]],2]""", world) == true,
            "Both set types compose: observations retain prior targets, latest does not, and nested use/it find both actor 0 and actor 1 targeting themselves.");
        Need(Eval("""["and",["==",["count",["cardUses","window","latest"],["useDeclared","use"]],4],["==",["count",["cardUses","window","latest"],["useIsResponse","use"]],2],["==",["count",["cardUses","window","latest"],["useIsSelfNativeResponse","use"]],1]]""", world) == true &&
            Eval("""["==",["count",["cardUses","window","latest"],["atOrBefore","use","currentUse","accepted"]],2]""", world) == true &&
            Eval("""["same","turnOwner","owner"]""", world) == true,
            "Declaration, response roles and accepted ordering remain independent facts; ordering compares original long IDs or the legacy declaration cursor.");
        var acceptedOrder = new CardUseFactsWorld(
            [latest[0] with { ActionId = order + 10, AcceptedCursor = 1 }, current with { AcceptedCursor = 2 }],
            [], current with { AcceptedCursor = 2 });
        Need(Eval("""["==",["count",["cardUses","window","latest"],["atOrBefore","use","currentUse","accepted"]],2]""", acceptedOrder) == true,
            "When actual acceptance cursors are available, action identity allocation cannot reverse the accepted order.");
        var unknown = new CardUseFactsWorld(null, null, null);
        const string missing = """["not",["any",["cardUses","window","latest"],true]]""";
        Need(Eval(missing, unknown) is null && Eval("""["not",["useDeclared","currentUse"]]""", unknown) is null &&
            Eval("""["any",["cardUses","now","latest"],true]""", world) is null &&
            Eval("""["sameUse",["last",["takeLast",["cardUses","window","latest"],2]],"currentUse"]""", unknown) is null &&
            Eval("""["sameTeam","owner","owner"]""", world) is null,
            "Unknown histories, missing current use and unavailable moments remain unknown through not.");
        Need(Eval("""["all",["takeLast",["cardUses","window","latest"],0],false]""", world) == true &&
            Eval("""["sameUse",["last",["takeLast",["cardUses","window","latest"],0]],"currentUse"]""", world) is null &&
            Seats("""["intersect",["useTargets","currentUse"],"livingOthers"]""")!.SequenceEqual([2]),
            "Empty tails retain quantifier semantics and no last identity; intersection preserves the left target order.");

        var selection = Load($$"""{"op":"selectTarget","target":"owner","targetKind":"otherLiving","targetSetExpr":{{targets}}},{"op":"draw","target":"selectedTarget","amount":1}""");
        var otherSelection = Load($$"""{"op":"selectTarget","target":"owner","targetKind":"otherLiving","targetSetExpr":{{targets.Replace("observations", "latest", StringComparison.Ordinal)}}},{"op":"draw","target":"selectedTarget","amount":1}""");
        var trigger = LoadTrigger("""{"kind":"expression","expr":["useDeclared","currentUse"]}""");
        var changedTrigger = LoadTrigger("""{"kind":"expression","expr":["useEstablished","currentUse"]}""");
        var unknownTrigger = LoadTrigger($$"""{"kind":"not","children":[{"kind":"expression","expr":{{missing}}}]}""").Triggers.Single();
        Need(!unknownTrigger.Condition.Evaluate(new(0, 4, true), evaluateExpression: expression => ProgramExpressions.EvaluateBool(expression, unknown)) &&
            !unknownTrigger.Condition.Evaluate(new(0, 4, true)), "Unknown trigger expressions cannot gain qualification through an outer condition not or an absent evaluator.");
        ContentSkillDefinition Definition(SkillProgram program) => new(program.Id, "Expression", "Expression") { Program = program };
        Need(selection.Activations[0].Effects[0].TargetSetExpression?.Type == ProgramExpressionType.ParticipantSet &&
            new SkillProgramDependencies([Definition(selection)]).RequiresFact(ProgramTrackedFact.ActualCardUseHistory) &&
            new SkillProgramDependencies([Definition(trigger)]).RequiresFact(ProgramTrackedFact.ActualCardUseHistory) &&
            !new SkillProgramDependencies([Definition(Load("""{"op":"draw","target":"owner","amount":1}"""))]).RequiresFact(ProgramTrackedFact.ActualCardUseHistory),
            "Only expression consumers request history; collection inputs are typed and do not require a character descriptor demand.");
        Need(ProgramInstructionResolver.Default.Features(LoadTrigger("""{"kind":"expression","expr":["<",["handCount","owner"],["handLimit","owner"]]}""").Triggers[0]).RequiresHandLimitContext &&
            ProgramInstructionResolver.Default.Features(Load("""{"op":"selectTarget","target":"owner","targetKind":"otherLiving","targetSetExpr":["filter","livingOthers",["<",["handCount","it"],["handLimit","it"]]]},{"op":"draw","target":"selectedTarget","amount":1}""").Activations[0]).RequiresHandLimitContext,
            "Trigger and participant-set expressions request the same hand-limit context as instruction conditions.");
        Need(selection.GameplayHash != otherSelection.GameplayHash && trigger.GameplayHash != changedTrigger.GameplayHash &&
            JsonSerializer.Serialize(selection.Activations[0].Effects[0]).Contains("\"TargetSetExpression\"", StringComparison.Ordinal) &&
            JsonSerializer.Serialize(trigger.Triggers[0].Condition).Contains("\"Expression\"", StringComparison.Ordinal),
            "Target-set and trigger ASTs are serialized, and both raw definition fields join the canonical gameplay fingerprint.");
        foreach (var binding in new[] { "number", "choiceNumber", "boundCount" })
        {
            try { LoadTrigger($$"""{"kind":"expression","expr":["==",["{{binding}}","unissued"],0]}"""); }
            catch (InvalidOperationException error) when (error.Message.Contains("instruction-local bindings", StringComparison.Ordinal)) { continue; }
            throw new InvalidOperationException("Trigger expressions accepted an instruction-local binding.");
        }
        Rejects("""{"op":"selectTarget","target":"owner","targetKind":"otherLiving","targetSetExpr":["filter","livingOthers",[">",["boundCount","unissued"],0]]},{"op":"draw","target":"selectedTarget","amount":1}""", "unknown card binding");
    }

    private sealed class CardUseFactsWorld(IReadOnlyList<CardUseHistoryFact>? latest,
        IReadOnlyList<CardUseHistoryFact>? observations, CardUseHistoryFact? current) : IProgramExpressionWorld
    {
        public int? Participant(string name) => name is "owner" or "turnOwner" ? 0 : null;
        public IReadOnlyList<int>? ParticipantSet(string name) => name == "livingOthers" ? [1, 2, 3] : null;
        public int? Stat(string stat, int seat) => null;
        public bool? Flag(string flag, int seat) => null;
        public IReadOnlyList<CardUseHistoryFact>? CardUses(string moment, string view) =>
            moment != "window" ? null : view == "latest" ? latest : observations;
        public CardUseHistoryFact? CurrentUse() => current;
    }

    private static void ActualUseSlicesKeepTheirMomentKindAndAiBoundary()
    {
        CardUseHistoryFact[] facts =
        [
            new(10, 1, 0, 0, CardActionType.Use, false, [1], false, 1)
                { DeclaredKind = CardKind.Slash, EffectiveKind = CardKind.FireSlash },
            new(null, 2, 1, 1, CardActionType.Response, true, [], false, 2)
                { EffectiveKind = CardKind.Dodge },
            new(30, null, 2, 2, CardActionType.Use, false, [0], false, 3)
                { DeclaredKind = CardKind.Peach, EffectiveKind = CardKind.Peach }
        ];
        var world = new ActualUseFactsWorld(facts);
        const string firstTwo = """["takeFirst",["cardUses","turnEnd","actualUses"],2]""";
        Need(Eval($$"""["==",["count",{{firstTwo}},true],2]""", world) == true &&
            Eval($$"""["same",["useActor",["first",{{firstTwo}}]],"owner"]""", world) == true &&
            Eval($$"""["useIsResponse",["last",{{firstTwo}}]]""", world) == true &&
            world.LastRead == ("turnEnd", "actualUses"),
            "The typed prefix reads the requested actual-use view at the actual turn-end moment and preserves declaration order.");
        Need(Eval("""["==",["count",["takeFirst",["cardUses","turnEnd","actualUses"],-1],true],0]""", world) == true &&
            Eval("""["==",["count",["takeFirst",["cardUses","turnEnd","actualUses"],["handCount","eventTarget"]],true],0]""", world) is null &&
            Eval($$"""["not",["any",{{firstTwo}},true]]""", new ActualUseFactsWorld(null)) is null,
            "A negative prefix is empty; an unknown length or unavailable turn-end point stays unknown.");
        const string firstUse = """["first",["cardUses","turnEnd","actualUses"]]""";
        var declared = $$"""["useKindIs",{{firstUse}},["slash"]]""";
        var effective = $$"""["useKindIs",{{firstUse}},["slash"],"effective"]""";
        Need(Eval(declared, world) == true && Eval(effective, world) == false &&
            Eval($$"""["useKindIs",{{firstUse}},["fireSlash"],"effective"]""", world) == true &&
            Eval($$"""["useKindIs",{{firstUse}},["fireSlash"]]""", world) == false &&
            Load(Draw.Replace("EXPR", declared)).GameplayHash != Load(Draw.Replace("EXPR", effective)).GameplayHash,
            "The default original declared kind and the explicit effective kind remain separate reads and definition fingerprints.");
        var ai = new SkillContextExpressionWorld(new(0, 4, 4, 1, TurnPhase.Play), null, ownerWindowFacts: world);
        var reads = world.Reads;
        Need(Eval($$"""["==",["count",{{firstTwo}},true],2]""", ai) is null &&
            Eval($$"""["contains",["useTargets",["first",{{firstTwo}}]],"owner"]""", ai) is null &&
            Eval("""["any",["cardUses","turnEnd","accepted"],true]""", ai) is null &&
            Eval("""["any",["cardUses","turnEnd","latest"],true]""", ai) is null && world.Reads == reads &&
            Eval($$"""["any",{{firstTwo}},true]""", new SkillContextExpressionWorld(new(0, 4, 4, 1, TurnPhase.Play), null)) is null,
            "AI does not read raw journal history or held native targets without a viewer-safe projection, even when another authorized window world is present.");
        Rejects(Draw.Replace("EXPR", """["any",["takeFirst",["cardUses","turnEnd","actualUses"],true],true]"""), "expected an integer");
        Rejects(Draw.Replace("EXPR", $$"""["useKindIs",{{firstUse}},["slash"],"completed"]"""), "stage must be effective");
        Rejects(Draw.Replace("EXPR", """["any",["cardUses","turnEnd","initial"],true]"""), "cardUses requires");
        Rejects(Draw.Replace("EXPR", """["any",["cardUses","afterTurn","actualUses"],true]"""), "cardUses requires");
    }

    private sealed class ActualUseFactsWorld(IReadOnlyList<CardUseHistoryFact>? facts) : IProgramExpressionWorld
    {
        public (string Moment, string View)? LastRead { get; private set; }
        public int Reads { get; private set; }
        public int? Participant(string name) => name == "owner" ? 0 : null;
        public IReadOnlyList<int>? ParticipantSet(string name) => null;
        public int? Stat(string stat, int seat) => null;
        public bool? Flag(string flag, int seat) => null;
        public IReadOnlyList<CardUseHistoryFact>? CardUses(string moment, string view)
        {
            LastRead = (moment, view);
            Reads++;
            return facts;
        }
    }

    private sealed class WoundedFacts : ISelectionWoundedQueries
    {
        public bool IdentityConcealed(int seat) => false;
        public bool IsRuleWounded(int seat) => true;
    }

    private sealed class PileFacts : IProgramExpressionWorld
    {
        public int? Participant(string name) => name == "owner" ? 0 : null;
        public IReadOnlyList<int>? ParticipantSet(string name) => null;
        public int? Stat(string stat, int seat) => null;
        public bool? Flag(string flag, int seat) => null;
        public int? PublicPileCount(int seat, string skillId) => skillId == "fixture:pile" ? 3 : 0;
    }

    private static ProgramExpression Bool(string json)
    { using var document = JsonDocument.Parse(json); return ProgramExpressions.Parse(document.RootElement, "facts"); }

    public static void FuliFlipsOnlyAsUniqueHighestHp()
    {
        foreach (var (weiHp, flips) in new[] { (3, true), (4, false) })
        {
            var (_, game) = Scene.CreateScene(discardSlash: false, weiHp: weiHp);
            Need(!game.CreateSnapshot(0).Players[0].IsFaceDown, "Liao Hua starts face up.");
            Scene.FatalDamage(game);
            Scene.Reach(game, prompt => prompt is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 });
            Scene.Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "program-trigger" &&
                choice.Parameters.GetValueOrDefault("skill-id") == Scene.Fuli);
            Scene.Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
            var hero = game.CreateSnapshot(0).Players[0];
            Need(hero.IsAlive && hero.Hp == 4, "Fuli first recovers to the four living factions.");
            Need(hero.IsFaceDown == flips, flips
                ? "A recovered owner with strictly the highest HP turns over through the generic instruction."
                : "A peer with equal HP blocks the expression-gated turnover.");
        }
    }

    private static SkillProgram Load(string effect, string? activationCondition = null)
    {
        var condition = activationCondition is null ? "" : $"\"condition\":{activationCondition},";
        var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"fixture:expression","revision":1,"activations":[
              {"id":"go","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,{{condition}}"effects":[{{effect}}]}]}]}
            """;
        return ReadProgram(rules);
    }

    private static SkillProgram LoadTrigger(string condition) => ReadProgram($$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"fixture:expression","revision":1,"triggers":[
          {"id":"go","window":"playPhaseStarting","subject":"owner","optional":true,"condition":{{condition}},"effects":[{"op":"draw","target":"owner","amount":1}]}]}]}
        """);

    private static SkillProgram ReadProgram(string rules)
    {
        var presentation = JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { ["fixture:expression"] = new { name = "表达式", description = "表达式条件" } }
        });
        return SkillProgramCatalog.Load(rules, presentation).Programs["fixture:expression"];
    }

    private static void Rejects(string effect, string fragment, string? activationCondition = null)
    {
        try { Load(effect, activationCondition); }
        catch (InvalidOperationException error) when (error.Message.Contains(fragment, StringComparison.Ordinal)) { return; }
        throw new InvalidOperationException($"Expected rejection containing '{fragment}'.");
    }

    private static bool? Eval(string json, IProgramExpressionWorld world)
    {
        using var document = JsonDocument.Parse(json);
        return ProgramExpressions.EvaluateBool(ProgramExpressions.Parse(document.RootElement, "expr"), world);
    }

    private static void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }
}
