using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;

internal static partial class PhaseStartCardControlChecks
{
    private const string DirectedSource = "fixture:directed-policy-source", DirectedPeer = "fixture:directed-policy-peer";

    private static void DirectedPoliciesKeepAppliedRightsAndFreezeNativeUse()
    {
        var (g, registry) = StartDirected("lifetime");
        DirectedGrant(g, "source", 2); Play(g); DirectedGrant(g, "applied", 3); Play(g);
        Need(DirectedHas(g, 2, DirectedTurnCardPolicyEffect.IgnoreDistance) && DirectedHas(g, 3, DirectedTurnCardPolicyEffect.ForbidTarget),
            "Both real grants apply before their original source is lost.");
        Use(g, "remove-source"); Play(g); g = Cold(g, registry);
        Need(!DirectedHas(g, 2, DirectedTurnCardPolicyEffect.IgnoreDistance) && DirectedHas(g, 3, DirectedTurnCardPolicyEffect.ForbidTarget) &&
            !g.GetRuntimeModule<CoreDirectedTurnCardPoliciesRuntime>().HasDirectedTurnCardPolicy(3, 0, CardKind.Slash, DirectedTurnCardPolicyEffect.ForbidTarget),
            "A source-bound permission ends on real skill loss; an applied directed restriction and its direction survive cold recovery.");
        foreach (var boundary in new[] { "turnStarted", "turnEnded" })
        {
            (g, registry) = StartDirected("lifetime"); DirectedGrant(g, boundary, 2); Play(g);
            var turn = g.State.TurnNumber; var issued = E<DirectedTurnCardPolicyGrantedEvent>(g).Single().Policy;
            Need(issued.Until.Count == 1 && DirectedHas(g, 2, DirectedTurnCardPolicyEffect.IgnoreDistance), "A boundary grant applies after issuance.");
            g = Cold(g, registry); End(g);
            for (var step = 0; step < 120 && !ReachedBoundary(); step++) Accept(g, new AdvanceOneStepCommand(g.Revision));
            Need(ReachedBoundary() && !DirectedHas(g, 2, DirectedTurnCardPolicyEffect.IgnoreDistance), "The original native turn boundary expires the issued policy.");
            g = Cold(g, registry);
            Need(!DirectedHas(g, 2, DirectedTurnCardPolicyEffect.IgnoreDistance) && E<DirectedTurnCardPolicyGrantedEvent>(g).Count() == 1,
                "Boundary recovery does not issue or retain a second policy.");
            bool ReachedBoundary() => boundary == "turnStarted"
                ? E<TurnStartedEvent>(g).Any(e => e.ActorSeat == 0 && e.TurnNumber > turn)
                : E<TurnEndedEvent>(g).Any(e => e.ActorSeat == 0 && e.TurnNumber == turn);
        }
        DirectedPolicyIssuedAfterTurnEndSkipsThatOccurrence();
        DirectedDeclarationFreezesAnEmptySnapshot();
        foreach (var count in new[] { 2, 3 }) DirectedWeiwanUsesActualObtainedCount(count);
        TargetQuotaAllowsSelfAndUsesOriginalIssuanceSlots();
        ActualCardUseHistoryKeepsNativeOrderAndPrincipalTargets();
        HuaManUsesFrozenDirectedHistory();
        QietingUsesLatestDeclaredNativeHistory();
    }

    private static void DirectedPolicyIssuedAfterTurnEndSkipsThatOccurrence()
    {
        var (g, registry) = StartDirected("late"); DirectedGrant(g, "turnEnded", 2); Play(g); End(g);
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
        Need(E<TurnEndedEvent>(g).Any() && !DirectedHas(g, 2, DirectedTurnCardPolicyEffect.IgnoreDistance),
            "The real afterTurnEnded window sees the committed End fact after the earlier ticket has already expired.");
        Answer(g, c => c.Targets.SequenceEqual([2]));
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        var ended = E<TurnEndedEvent>(g).Last(); var policy = E<DirectedTurnCardPolicyGrantedEvent>(g).Last().Policy;
        Need(policy.Until.Single().IssuedOccurrence == ended.TurnNumber && DirectedHas(g, 2, DirectedTurnCardPolicyEffect.IgnoreDistance),
            "A policy issued by the real afterTurnEnded window anchors the already committed End fact and survives that same occurrence.");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        for (var step = 0; step < 120 && !E<TurnEndedEvent>(g).Any(e => e.TurnNumber > ended.TurnNumber); step++)
            Accept(g, new AdvanceOneStepCommand(g.Revision));
        Need(E<TurnEndedEvent>(g).Any(e => e.TurnNumber > ended.TurnNumber) && !DirectedHas(g, 2, DirectedTurnCardPolicyEffect.IgnoreDistance),
            "The next actual End expires the late-issued any-seat policy.");
        g = Cold(g, registry);
    }

    private static void DirectedDeclarationFreezesAnEmptySnapshot()
    {
        var (g, registry) = StartDirected("freeze");
        var id = V(g, 0).Hand.Single(c => c.Suit == Suit.Club).Id;
        var duel = V(g, 0).Hand.Single(c => c.Kind == CardKind.Duel).Id;
        var action = g.GetHumanLegalActions().First(a => a.CardId == id && a.ConversionSource?.SkillId == "classic:guhuo" &&
            a.PlayedCardKind == CardKind.Slash && a.TargetSeats.SequenceEqual([1]));
        DirectedPlay(g, action);
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
        Answer(g, c => c.Targets.SequenceEqual([1]));
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        var declaration = g.ResolutionStack.OfType<CardDeclarationFrame>().Single();
        Need(declaration.Payment.DirectedPolicies is { Count: 0 } &&
            g.GetRuntimeModule<CoreDirectedTurnCardPoliciesRuntime>().CaptureDirectedPolicies(0).Any(p => p.TargetSeat == 1),
            "A real declaration captures empty before its payment child issues a later directed restriction.");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(g, _ => g.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == Watch && f.TriggerId == "hold-use"));
        var use = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == id);
        Need(use.DirectedPolicies is { Count: 0 } && use.AcceptedDeclarationPayment?.DirectedPolicies is { Count: 0 } &&
            !DirectedHas(g, 1, DirectedTurnCardPolicyEffect.ForbidTarget, use.Id),
            "The accepted payment and native Use retain captured absence rather than falling back to the later live restriction.");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue"); Play(g);
        Need(E<CardUseFinishedEvent>(g).Count(e => e.ResolutionId == use.Id && e.CardId == id) == 1 &&
            DirectedHas(g, 1, DirectedTurnCardPolicyEffect.ForbidTarget) &&
            !g.GetHumanLegalActions().Any(a => a.CardId == duel && a.ConversionSource is null && a.TargetSeats.Contains(1)) &&
            g.GetHumanLegalActions().Any(a => a.CardId == duel && a.ConversionSource is null && a.TargetSeats.Contains(3)),
            "The original Use completes once; the next real card sees the newly applied restriction only against its named target.");
    }

    private static void DirectedWeiwanUsesActualObtainedCount(int count)
    {
        var (g, registry) = StartDirected("weiwan", count);
        var cost = V(g, 0).Hand.Single(c => c.Suit == Suit.Club).Id;
        var duel = V(g, 0).Hand.Single(c => c.Kind == CardKind.Duel).Id;
        Need(g.GetCombatDistance(0, 2) > g.GetAttackRange(0) &&
            g.GetHumanLegalActions().Any(a => a.CardId == duel && a.ConversionSource is null && a.TargetSeats.Contains(2)), "The selected target is truly distant but an ordinary Duel is initially legal.");
        Accept(g, new UseProgramSkillCommand(0, "le:weiwan", "discard-qin-obtain-suits", [cost], [2], g.Revision, P(g)!.PromptId)); Play(g);
        Need(E<WeiwanObtainPaidEvent>(g).Single().ActualCount == count &&
            g.CardMovements.Count(m => m.From.OwnerSeat == 2 && m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program.le-xiao-qiao.weiwan.obtain") == count,
            "Real Qin payment and one actual card per eligible suit determine the obtained count without seeded sampling searches.");
        Use(g, "remove-source"); Play(g); g = Cold(g, registry);
        var effect = count == 2 ? DirectedTurnCardPolicyEffect.IgnoreDistance : DirectedTurnCardPolicyEffect.ForbidTarget;
        Need(DirectedHas(g, 2, effect) && E<DirectedTurnCardPolicyGrantedEvent>(g).Count(e => e.Policy.Source.SkillId == "le:weiwan") == 1,
            "The real Weiwan outcome issues one independent policy which survives source loss and cold recovery.");
        if (count == 3)
            Need(!g.GetHumanLegalActions().Any(a => a.CardId == duel && a.ConversionSource is null && a.TargetSeats.Contains(2)), "Obtaining three prevents the next actual Duel against that target.");
        else
        {
            Need(g.GetRuntimeModule<CoreDirectedTurnCardPoliciesRuntime>().HasDirectedTurnCardPolicy(0, 2, CardKind.Duel, DirectedTurnCardPolicyEffect.BypassUseLimit),
                "The obtained-two outcome grants the general use-limit right for non-Slash cards as well.");
            var slashes = V(g, 0).Hand.Where(c => c.Kind == CardKind.Slash).Select(c => c.Id).Take(2).ToArray();
            foreach (var slash in slashes) { DirectedPlay(g, g.GetHumanLegalActions().First(a => a.CardId == slash && a.ConversionSource is null && a.TargetSeats.SequenceEqual([2]))); Play(g); }
            Need(slashes.Length == 2 && slashes.All(slash => E<CardUseFinishedEvent>(g).Count(e => e.CardId == slash && e.CardKind == CardKind.Slash) == 1),
                "Obtaining two permits two real distant Slashes across the ordinary quota, each paying and completing once.");
        }
        g = Cold(g, registry);
    }

    private static bool DirectedHas(GameEngine g, int target, DirectedTurnCardPolicyEffect effect, long? use = null) =>
        g.GetRuntimeModule<CoreDirectedTurnCardPoliciesRuntime>().HasDirectedTurnCardPolicy(0, target, CardKind.Slash, effect, use);
    private static void DirectedGrant(GameEngine g, string id, int target) =>
        Accept(g, new UseProgramSkillCommand(0, DirectedSource, id, [], [target], g.Revision, P(g)!.PromptId));
    private static void DirectedPlay(GameEngine g, LegalAction a) => Accept(g,
        new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, g.Revision, P(g)!.PromptId, a.PlayedCardKind, a.TargetCardId)
        { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });

    private static (GameEngine, ContentRegistry) StartDirected(string scenario, int obtained = 0)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new DirectedFixture(scenario));
        var qieting = scenario.StartsWith("qieting", StringComparison.Ordinal);
        var target = new ShanheCombatantDefinition(DirectedPeer) { StartingHand = new[] { Suit.Heart, Suit.Spade, Suit.Diamond }.Take(obtained).Select(s => new ShanheStartingCard(CardKind.Dodge, s, 8)).ToArray() };
        ShanheStartingCard[] hand = scenario == "quota-self"
            ? [new(CardKind.Alcohol, Suit.Club, 7), new(CardKind.Alcohol, Suit.Heart, 8), new(CardKind.Slash, Suit.Spade, 9), new(CardKind.Duel, Suit.Diamond, 10)]
            : [new(CardKind.Slash, Suit.Club, 7), new(CardKind.Slash, Suit.Heart, 8), new(CardKind.Slash, Suit.Spade, 9), new(CardKind.Duel, Suit.Diamond, 10)];
        if (scenario.StartsWith("history", StringComparison.Ordinal) || scenario.StartsWith("souying", StringComparison.Ordinal)) hand = HistoryStartingHand();
        if (qieting) hand = QietingStartingHand();
        var setup = new ShanheBattleSetup(7, 1, "directed-rights", Owner, qieting && scenario != "qieting-legacy" ? 4 : 12, 0, 0, 0, 0, [],
            hand, [],
            [target, new(DirectedPeer) { StartingHand = [] }], [new(qieting ? QietingObserver : DirectedPeer) { StartingHand = [],
                StartingEquipment = scenario.StartsWith("history", StringComparison.Ordinal) ? [new(CardKind.Crossbow, Suit.Club, 3)] : null }], new string('a', 64));
        var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, registry);
        Accept(g, new StartGameCommand());
        if (scenario == "quota-bao") Reach(g, p => Optional(p, "ol:zhennan"));
        else if (scenario == "qieting-legacy") Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
        else Play(g);
        return (g, registry);
    }

    private sealed class DirectedFixture(string scenario) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-directed-rights", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var skills = new List<string> { Driver, DirectedSource, "fixture:directed-no-draw" };
            var nativeQuotaSkill = scenario switch { "quota-fire" => "ol:huxiao", "quota-bao" => "ol:zhennan", "quota-guosi" => "ol:tanbei", _ => (string?)null };
            string[] bundles = scenario switch { "freeze" => ["classic-yu-ji"], "weiwan" => ["le-xiao-qiao"],
                "quota-fire" => ["ordinary-guan-yin-ping"], "quota-bao" => ["ol-bao-san-niang"], "quota-guosi" => ["ol-guo-si"], _ => [] };
            foreach (var bundle in bundles)
            {
                var real = CharacterFixtureContent.EmbeddedProgramBundle(bundle);
                foreach (var (id, program) in real.Programs.Where(p => nativeQuotaSkill is null || p.Key == nativeQuotaSkill)) b.AddSkill(new(id, real.Presentations[id].Name, real.Presentations[id].Description) { Program = program, ProgramPresentation = real.Presentations[id] });
            }
            if (scenario == "freeze") skills.Add("classic:guhuo");
            if (scenario == "weiwan") skills.AddRange(["le:qiqin", "le:weiwan"]);
            if (nativeQuotaSkill is not null) skills.Add(nativeQuotaSkill);
            if (scenario.StartsWith("souying", StringComparison.Ordinal)) RegisterSouyingFixture(b, skills);
            if (scenario.StartsWith("qieting", StringComparison.Ordinal)) RegisterQietingFixture(b);
            if (scenario.StartsWith("history", StringComparison.Ordinal))
            {
                foreach (var bundle in new[] { "ol-su-fei", "ol-guo-si" })
                {
                    var actual = CharacterFixtureContent.EmbeddedProgramBundle(bundle);
                    foreach (var (id, program) in actual.Programs) b.AddSkill(new(id, actual.Presentations[id].Name, actual.Presentations[id].Description)
                        { Program = program, ProgramPresentation = actual.Presentations[id] });
                }
            }
            if (scenario.StartsWith("history", StringComparison.Ordinal) || scenario.StartsWith("souying", StringComparison.Ordinal))
                b.AddCard(new("fixture:history-borrowed", "借刀杀人", "锦囊牌", "真实原生复合目标", CardKind.BorrowedSword));
            var rules = JsonNode.Parse(SkillProgramTestJson.CurrentSchema("""
            {"schemaVersion":__RULES_SCHEMA__,"skills":[
              {"id":"fixture:directed-policy-source","revision":1,"activations":[
                {"id":"source","minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"grantDirectedTurnCardPolicy","target":"owner","actorRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"effects":["ignoreDistance"],"until":[]}]},
                {"id":"applied","minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"grantDirectedTurnCardPolicy","target":"owner","actorRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"effects":["forbidTarget"],"requiresEffectiveSource":false,"captureOnUse":true,"until":[]}]},
                {"id":"turnStarted","minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"grantDirectedTurnCardPolicy","target":"owner","actorRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"effects":["ignoreDistance"],"requiresEffectiveSource":false,"captureOnUse":true,"until":[{"event":"turnStarted","participant":{"kind":"owner"}}]}]},
                {"id":"turnEnded","minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"grantDirectedTurnCardPolicy","target":"owner","actorRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"effects":["ignoreDistance"],"requiresEffectiveSource":false,"captureOnUse":true,"until":[{"event":"turnEnded","participant":{"kind":"any"}}]}]}]},
              {"id":"fixture:phase-control-driver","revision":1,"activations":[{"id":"remove-source","usesPerTurn":null,"effects":[{"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["fixture:directed-policy-source"]},"until":[]}]}]},
              {"id":"fixture:directed-no-draw","revision":1,"triggers":[{"id":"quiet-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]}
            ]}
            """))!;
            ConfigureQuotaFixture(rules, scenario);
            ConfigureHistoryFixture(rules, scenario, skills);
            ConfigureHuaManHistoryFixture(rules, scenario, skills);
            ConfigureQietingHistoryFixture(rules, scenario, skills);
            foreach (var skill in rules["skills"]!.AsArray())
            foreach (var activation in skill!["activations"]?.AsArray() ?? new JsonArray())
            {
                activation!["minCards"] = 0; activation["maxCards"] = 0;
                activation["minTargets"] ??= 0; activation["maxTargets"] ??= 0; activation["targetKind"] ??= "otherLiving";
            }
            if (scenario == "weiwan")
                rules["skills"]![1]!["activations"]![0]!["effects"]![0]!["match"]!["skillIds"] = JsonNode.Parse("""["le:weiwan"]""");
            if (scenario == "late")
                rules["skills"]![1]!["triggers"] = JsonNode.Parse("""
                [{"id":"late-policy","window":"afterTurnEnded","subject":"owner","turnOwnerScope":"own","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"grantDirectedTurnCardPolicy","target":"owner","actorRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"effects":["ignoreDistance"],"requiresEffectiveSource":false,"captureOnUse":true,"until":[{"event":"turnEnded","participant":{"kind":"any"},"skipIssued":true}]},{"op":"chooseOption","target":"owner","resultBind":"late","options":[{"id":"continue"}]}]}]
                """);
            if (scenario == "freeze")
            {
                skills.Add(Watch);
                rules["skills"]!.AsArray().Add(JsonNode.Parse("""
                {"id":"fixture:phase-control-watch","revision":1,"triggers":[
                  {"id":"declaration-policy","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementReasons":["conversion.declaration.pay"],"movementOccurrence":"perBatch","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"grantDirectedTurnCardPolicy","target":"owner","actorRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"effects":["forbidTarget"],"requiresEffectiveSource":false,"captureOnUse":true,"until":[]},{"op":"chooseOption","target":"owner","resultBind":"issued","options":[{"id":"continue"}]}]},
                  {"id":"hold-use","window":"cardUseCommitted","ownerRelation":"actor","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"accepted","options":[{"id":"continue"}]}]}]}
                """));
            }
            var ids = rules["skills"]!.AsArray().Select(n => n!["id"]!.GetValue<string>()).ToArray();
            var options = rules["skills"]!.AsArray().Where(n => n!.ToJsonString().Contains("\"chooseOption\"", StringComparison.Ordinal)).Select(n => n!["id"]!.GetValue<string>()).ToHashSet(StringComparer.Ordinal);
            var labels = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = ids.ToDictionary(id => id, id => new { name = id, description = "真实定向权利与原生付款", optionLabels = options.Contains(id) ? new Dictionary<string, string> { ["continue"] = "继续" } : new Dictionary<string, string>() }) });
            var catalog = SkillProgramCatalog.Load(rules.ToJsonString(), labels);
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Owner, "定向权利拥有者", "", "standard:none", "wei", 12, skills));
            b.AddGeneral(new(DirectedPeer, "固定其他角色", "", "fixture:directed-no-draw", "qun", 20,
                scenario == "quota-fire" ? [Watch] : scenario == "quota-bao" ? ["fixture:quota-prevent"] :
                scenario == "history-dodge" ? ["fixture:history-retaliate"] : []));
            b.AddDeck(new("fixture:directed-rights-deck", "固定实体", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "定向权利真实原生边界", 4, 4, new Dictionary<string, int>(), "fixture:directed-rights-deck", 2,
                scenario.StartsWith("qieting", StringComparison.Ordinal) ? [Owner, DirectedPeer, QietingObserver] : [Owner, DirectedPeer], ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
