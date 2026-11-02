using System.Text.Json.Nodes;
using System.Text.Json;
using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineOwners;

internal static class ProgramTriggerResourceChecks
{
    public static void TriggerWindowAndFrozenSuitContracts()
    {
        var sidi = Read("classic-cao-zhen");
        var han = Read("classic-han-hao-shi-huan");
        SkillProgramCatalog.Load(sidi.Rules, sidi.Presentation);
        SkillProgramCatalog.Load(han.Rules, han.Presentation);

        var wrongDiscard = JsonNode.Parse(han.Rules)!;
        var discardTrigger = wrongDiscard["skills"]![0]!["triggers"]![0]!;
        discardTrigger["window"] = "cardsGained";
        discardTrigger.AsObject().Remove("discardOwnerScope");
        discardTrigger.AsObject().Remove("movementDiscardOnly");
        discardTrigger.AsObject().Remove("cardCategories");
        discardTrigger.AsObject().Remove("suits");
        discardTrigger["destinationZones"] = new JsonArray("hand");
        Reject(wrongDiscard, han.Presentation, "requires trigger window DiscardPileReceived");

        var wrongPhase = JsonNode.Parse(sidi.Rules)!;
        wrongPhase["skills"]![0]!["triggers"]![0]!["window"] = "judgmentPhaseStarting";
        Reject(wrongPhase, sidi.Presentation, "requires trigger window PlayPhaseStarting");

        var unfrozen = JsonNode.Parse(sidi.Rules)!;
        unfrozen["skills"]![0]!["triggers"]![0]!["effects"]![0] = JsonNode.Parse(
            """{"op":"selectOwnedCards","target":"owner","zones":["hand"],"amount":1,"resultBind":"cost"}""");
        Reject(unfrozen, sidi.Presentation, "previously frozen single-card metadata binding");
        CardActorDamageRequiresCardAction();
        OptionalFactsPreserveLegacyJson();
    }

    private static void CardActorDamageRequiresCardAction()
    {
        const string id = "fixture:card-actor-damage";
        var rules = $$"""{"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{id}}","revision":1,"triggers":[{"id":"after-use","window":"cardUseCompleted","ownerRelation":"observer","optional":false,"effects":[{"op":"damage","target":"owner","targetRef":{"kind":"actor"},"amount":1}]}]}]}""";
        var presentation = JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [id] = new { name = "Card actor", description = "Damage the actual card user." } }
        });
        SkillProgramCatalog.Load(rules, presentation);
        var wrongContext = JsonNode.Parse(rules)!;
        var trigger = wrongContext["skills"]![0]!["triggers"]![0]!;
        trigger["window"] = "judgmentFinalized";
        trigger.AsObject().Remove("ownerRelation");
        trigger["subject"] = "owner";
        trigger["suits"] = new JsonArray("spade");
        trigger["minimumRank"] = 1;
        trigger["maximumRank"] = 13;
        trigger["excludedReasons"] = new JsonArray();
        Reject(wrongContext, presentation, "requires context CardAction");
    }

    private static void OptionalFactsPreserveLegacyJson()
    {
        const string id = "fixture:optional-facts";
        var rules = $$"""{"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{id}}","revision":1,"triggers":[{"id":"before","window":"beforeDamageApplied","subject":"damageTarget","optional":true,"effects":[{"op":"draw","target":"owner","amount":1}]}]}]}""";
        const string presentation = """{"schemaVersion":3,"skills":{"fixture:optional-facts":{"name":"Facts","description":"Facts"}}}""";
        var program = SkillProgramCatalog.Load(rules, presentation).Programs[id];
        var registry = ContentRegistry.Build(new StandardContentPackage(), new FactsPackage(program));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 5, ModeId = "identity:standard-5", HumanSeat = 0,
            HumanRole = Role.Lord, UseInteractiveSetup = false, AdvanceAfterHumanCommands = false }, registry);
        if (!game.Submit(new StartGameCommand()).Accepted) throw new InvalidOperationException("Facts fixture failed to start.");
        NativeAppearanceFactDemand(program, game);
        ActualPhaseFactDemand(game, registry);
        NativeDrawFactDemand(registry);
        MovementTimingFactDemand(game, registry);
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var players = (IReadOnlyList<CharacterState>)global::FixtureRuntimeAccess.Engine.GetField("_players", flags)!.GetValue(game)!;
        var action = new CardActionContext(1, null, CardActionType.Use, 0, 0, null, null, 1,
            CardKind.Slash, [1], [], [], effectiveSuit: Suit.Spade);
        var capture = global::FixtureRuntimeAccess.Engine.GetMethods(flags).Single(method => global::FixtureRuntimeAccess.OriginalName(method) == "CaptureProgramTriggerFacts" && method.GetParameters().Length == 2);
        var facts = (SkillProgramTriggerFacts)capture.Invoke(game, [players[0], action])!;
        AssertOmitted(facts, "CardActionCardIsBlack");
        players[0].SkillGrants.Grant(new SkillGrant("fixture:facts", id, "fixture:facts-instance", "fixture:facts"));
        var stack = (FrameStore)typeof(GameEngine).GetField("_resolutionStack", flags)!.GetValue(game)!;
        stack.Push(new DamageFrame(1000, 999, 1, 0, 1));
        var before = global::FixtureRuntimeAccess.Engine.GetMethod("TryBeginBeforeDamageProgramWindow", flags)!;
        if (!(bool)before.Invoke(game, [1, 0, 1, DamageNature.Normal, BeforeDamageProgramContinuation.Attack, Type.Missing])!)
            throw new InvalidOperationException("Legacy before-damage fixture failed to pause.");
        var window = stack.OfType<BeforeDamageProgramWindowFrame>().Last();
        foreach (var candidate in window.Candidates) AssertOmitted(candidate.Facts, "DamageSourceGender");

        var invalid = JsonNode.Parse(rules)!;
        invalid["skills"]![0]!["triggers"]![0]!["window"] = "playPhaseStarting";
        invalid["skills"]![0]!["triggers"]![0]!["subject"] = "owner";
        invalid["skills"]![0]!["triggers"]![0]!["condition"] = JsonNode.Parse("""{"kind":"damageSourceGenderIs","gender":"male"}""");
        Reject(invalid, presentation, "requires a beforeDamageApplied trigger");
    }

    private static void NativeAppearanceFactDemand(SkillProgram template, GameEngine withoutDemand)
    {
        var triggered = ProgramOperationCatalog.Default.Resolve(SkillProgramEffectOp.DrawHengwuFieldSuitEquipment).FactRequirements;
        var activated = ProgramOperationCatalog.Default.Resolve(SkillProgramEffectOp.ResolveWuyouContributionAndNamedReturn).FactRequirements;
        if ((triggered.Triggered & ProgramTrackedFact.NativeActionAppearance) == 0 ||
            (triggered.Activated & ProgramTrackedFact.NativeActionAppearance) != 0 ||
            (activated.Activated & ProgramTrackedFact.NativeActionAppearance) == 0 ||
            (activated.Triggered & ProgramTrackedFact.NativeActionAppearance) != 0)
            throw new InvalidOperationException("Native appearance demand must preserve the operation's trigger or activation scope.");

        var rank = WithViewAs(CharacterFixtureContent.EmbeddedProgramBundle("wumiao-guan-yu")
            .Programs["wumiao:juewu"].ViewAs.Where(rule => rule.RankTwoDamageUse).ToArray());
        var name = WithViewAs(CharacterFixtureContent.EmbeddedProgramBundle("le-mi-heng")
            .Programs["le:sirui"].ViewAs.Where(rule => rule.EqualNameLengthUse).ToArray());
        foreach (var program in new[] { rank, name })
            if (!new SkillProgramDependencies([Definition(program)]).RequiresFact(ProgramTrackedFact.NativeActionAppearance))
                throw new InvalidOperationException("A view-as declaration must request native appearance without a character capability callback.");
        if (new SkillProgramDependencies([Definition(WithViewAs([]))]).RequiresFact(ProgramTrackedFact.NativeActionAppearance))
            throw new InvalidOperationException("Programs without an appearance consumer must retain omitted legacy appearance facts.");

        var omitted = Capture(withoutDemand);
        if (omitted.Action.EffectiveSuit is not null || omitted.Action.EffectiveRank is not null)
            throw new InvalidOperationException("Native use appearance must remain omitted without catalog demand.");
        var registry = ContentRegistry.Build(new StandardContentPackage(), new FactsPackage(rank));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 5, ModeId = "identity:standard-5", HumanSeat = 0,
            HumanRole = Role.Lord, UseInteractiveSetup = false, AdvanceAfterHumanCommands = false }, registry);
        if (!game.Submit(new StartGameCommand()).Accepted) throw new InvalidOperationException("Appearance fixture failed to start.");
        var captured = Capture(game);
        if (captured.Action.EffectiveSuit != captured.Card.Suit || captured.Action.EffectiveRank != captured.Card.Rank ||
            captured.Action.EffectiveIsRed != (captured.Card.Suit is Suit.Heart or Suit.Diamond))
            throw new InvalidOperationException("An unowned catalog declaration must freeze the actual physical use's suit, rank and color.");

        SkillProgram WithViewAs(IReadOnlyList<SkillProgramViewAs> viewAs) => new(template.Id, template.Revision,
            template.GameplayHash, template.RuntimeVersion, template.MinimumRulesVersion, [], viewAs, [], [], [], []);
        static ContentSkillDefinition Definition(SkillProgram program) => new(program.Id, "Facts", "Facts") { Program = program };
        static (CardActionContext Action, Card Card) Capture(GameEngine game)
        {
            var zones = (CardZoneStore)typeof(GameEngine).GetField("_cardZones", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
            var card = zones.CardsAt(CardLocation.Hand(0)).First();
            var action = game.GetRuntimeModule<CoreGameEngineCardActionsRuntime>()
                .CaptureCardUseAction(card, 0, [], card.Kind, [card.Id])!;
            return (action, card);
        }
    }

    private static void ActualPhaseFactDemand(GameEngine game, ContentRegistry withoutPhaseDemand)
    {
        const ProgramTrackedFact phase = ProgramTrackedFact.ActualDiscardRecoveryPhase, end = ProgramTrackedFact.ActualPhaseEnd;
        var trigger = ProgramOperationCatalog.Default.Resolve(SkillProgramEffectOp.ChooseLishiPunishments).FactRequirements;
        var activation = ProgramOperationCatalog.Default.Resolve(SkillProgramEffectOp.ResolveMouHuCheErKongwu).FactRequirements;
        Check((trigger.Triggered & end) != 0 && (trigger.Activated & end) == 0 &&
            (activation.Activated & end) != 0 && (activation.Triggered & end) == 0,
            "Phase-end operation demand must preserve trigger versus activation admission.");
        Check((ProgramOperationCatalog.Default.Resolve(SkillProgramEffectOp.InitializeMouSimaShiSanshi).FactRequirements.Triggered & end) == 0 &&
            (ProgramOperationCatalog.Default.Resolve(SkillProgramEffectOp.ResolveMouSimaShiChenlve).FactRequirements.Activated & end) != 0 &&
            (ProgramOperationCatalog.Default.Resolve(SkillProgramEffectOp.RemoveMouSimaShiChenlve).FactRequirements.Triggered & end) != 0,
            "Only Sima Shi's real issued phase-end consumers require ends; initialization is not an end consumer.");

        var phaseEnd = Program("""{"triggers":[{"id":"end","window":"actualPhaseEnded","subject":"owner","optional":false,"effects":[{"op":"draw","target":"owner","amount":1}]}]}""");
        Check(Requires(phaseEnd, end) && Requires(phaseEnd, phase),
            "A pure phase-end window declares end facts and closes over phase identity, without a character operation.");
        var phaseStart = Program("""{"activations":[{"id":"state","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"skillState","target":"owner","action":"suppress","match":{"skillIds":["fixture:optional-facts"]},"until":[{"event":"phaseStarted","participant":{"kind":"owner"},"phase":"play"}]}]}]}""");
        Check(Requires(phaseStart, phase) && !Requires(phaseStart, end), "A phase-start lifetime needs identity without manufacturing end demand.");
        var declaration = JsonNode.Parse("""{"skillStates":[{"target":"owner","action":"suppress","on":{"event":"turnStarted","participant":{"kind":"owner"},"timing":"before"},"match":{"skillIds":["fixture:optional-facts"]},"until":[{"event":"turnEnded","participant":{"kind":"owner"},"timing":"after"}]}]}""")!;
        var turnOnly = Program(declaration.ToJsonString());
        Check(!Requires(turnOnly, phase) && !Requires(turnOnly, end), "A turn declaration does not request unrelated actual phases.");
        declaration["skillStates"]![0]!["until"]!.AsArray().Add(JsonNode.Parse("""{"event":"phaseEnded","participant":{"kind":"owner"},"phase":"play"}"""));
        Check(Requires(Program(declaration.ToJsonString()), end), "A legal event declaration's additional phase-end boundary participates in catalog demand.");
        var passive = new SkillProgramDependencies([new ContentSkillDefinition("ol:liangzhu", "Passive", "Passive")]);
        Check(passive.RequiresFact(phase) && !passive.RequiresFact(end), "A passive definition requests phase identity without an instruction or an end consumer.");

        var zones = (CardZoneStore)typeof(GameEngine).GetField("_cardZones", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var catalog = ContentRegistry.Build(new StandardContentPackage(), new FactsPackage(phaseEnd));
        var plainMode = CardGame.Modes.Standard.StandardModeRuleProfiles.Create(withoutPhaseDemand.Modes["identity:standard-5"], new GameOptions());
        var guanduMode = CardGame.Modes.Standard.StandardModeRuleProfiles.Create(new ContentModeDefinition("fixture:phase-guandu", "Guandu", 2, 8,
            new Dictionary<string, int>(), ModeKind: ContentModeKind.Team)
        { GuanduActivity = new(new Dictionary<string, IReadOnlyList<string>>(), new Dictionary<string, string>(), [], []) }, new GameOptions());
        var plain = Demand(withoutPhaseDemand, plainMode.RuntimeRequirements.RequiredFacts);
        var modeOnly = Demand(withoutPhaseDemand, guanduMode.RuntimeRequirements.RequiredFacts);
        var catalogOnly = Demand(catalog, ProgramTrackedFact.None);
        Check(!plain.RequiresFact(phase) && !plain.RequiresFact(end) && modeOnly.RequiresFact(phase) && modeOnly.RequiresFact(end) &&
            catalogOnly.RequiresFact(phase) && catalogOnly.RequiresFact(end) && !withoutPhaseDemand.ProgramDependencies.RequiresFact(end),
            "Selected-mode and catalog demand compose per match; sharing a catalog must not leak a mode's phase requirements.");
        Check(!Demand(withoutPhaseDemand, ProgramTrackedFact.None).RequiresFact(end), "Constructing an end-demanding match does not alter later no-demand matches.");
        var closed = new MatchNativeFactDemand(end);
        Check(closed.RequiresFact(phase) && closed.RequiresFact(end) && !closed.RequiresFact(ProgramTrackedFact.NativeActionAppearance),
            "End-demand closure adds only its real identity dependency.");
        try { _ = new MatchNativeFactDemand((ProgramTrackedFact)int.MinValue); throw new InvalidOperationException("Unknown native fact bits were accepted."); }
        catch (ArgumentOutOfRangeException) { }
        try
        {
            _ = new ModeRuleSet(plainMode.Opening, plainMode.Death, plainMode.Cards, false, false, false, false,
                runtimeRequirements: new(RequiredFacts: (ProgramTrackedFact)int.MinValue));
            throw new InvalidOperationException("A mode profile accepted unknown native fact bits.");
        }
        catch (ArgumentException) { }

        INativeFactDemand Demand(ContentRegistry content, ProgramTrackedFact mode) => (INativeFactDemand)new MatchCoreCapabilityProvider(zones,
            content: content, modeRequiredFacts: mode).Bindings.Single(binding => binding.Contract == typeof(INativeFactDemand)).Instance;
        static bool Requires(SkillProgram program, ProgramTrackedFact fact) =>
            new SkillProgramDependencies([new ContentSkillDefinition(program.Id, "Facts", "Facts") { Program = program }]).RequiresFact(fact);
        static SkillProgram Program(string fields)
        {
            var skill = JsonNode.Parse(fields)!.AsObject();
            skill["id"] = "fixture:optional-facts"; skill["revision"] = 1;
            var rules = new JsonObject { ["schemaVersion"] = SkillProgramCatalog.RulesSchemaVersion, ["skills"] = new JsonArray(skill) };
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { ["fixture:optional-facts"] = new { name = "Facts", description = "Facts" } } });
            return SkillProgramCatalog.Load(rules.ToJsonString(), presentation).Programs["fixture:optional-facts"];
        }
        static void Check(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    }

    private static void NativeDrawFactDemand(ContentRegistry withoutDemand)
    {
        const ProgramTrackedFact draw = ProgramTrackedFact.NativeDrawInvocation, phase = ProgramTrackedFact.ActualDiscardRecoveryPhase;
        var triggered = ProgramOperationCatalog.Default.Resolve(SkillProgramEffectOp.DiscardHellOutsideDrawGain).FactRequirements;
        var activated = ProgramOperationCatalog.Default.Resolve(SkillProgramEffectOp.ResolveWenrenSequentialDraw).FactRequirements;
        Check((triggered.Triggered & draw) != 0 && (triggered.Activated & draw) == 0 &&
            (triggered.Triggered & phase) == 0 && (activated.Activated & draw) != 0 && (activated.Triggered & draw) == 0,
            "Native draw descriptors retain trigger/activation demand; pure invoice demand does not invent phase identity.");
        var passive = new SkillProgramDependencies([new ContentSkillDefinition("mou-ol:yangwei", "Passive", "Passive")]);
        Check(passive.RequiresFact(draw) && passive.RequiresFact(phase), "Yangwei retains its independent passive draw and phase requirements.");
        var trait = CharacterFixtureContent.EmbeddedProgramBundle("shanhe-current-jinnangji").Programs.Values.Single();
        Check(Dependencies(trait).RequiresFact(draw) && Dependencies(trait).RequiresFact(phase),
            "The actual draw-phase hand-limit trait requests both its native invoices and existing phase identity.");
        var ordinal = CharacterFixtureContent.EmbeddedProgramBundle("shanhe-current-duoduo-yishan").Programs.Values.First();
        Check(Dependencies(ordinal).RequiresFact(draw) && !Dependencies(ordinal).RequiresFact(phase),
            "A native draw ordinal condition requests invoices without requiring actual phase identity.");
        var acquisition = new MatchNativeFactDemand(ProgramTrackedFact.HandGainOccurrence);
        Check(acquisition.RequiresFact(draw) && !acquisition.RequiresFact(phase) &&
            !acquisition.RequiresFact(ProgramTrackedFact.MovementTiming),
            "Whole acquisitions require real draw invoices for grouping, without manufacturing phase identity or movement timing.");
        var pure = new MatchNativeFactDemand(draw);
        Check(pure.RequiresFact(draw) && !pure.RequiresFact(phase) && !pure.RequiresFact(ProgramTrackedFact.ActualPhaseEnd),
            "Native draw demand has no phase-end or phase-identity closure.");

        var omitted = Draw(Start(withoutDemand));
        Check(omitted.Cards.Count == 1 && omitted.Invoices.Length == 0 && omitted.Batch.NativeDrawInvocation is null,
            "A real physical draw still moves its card but omits invoice facts without catalog or selected-mode demand.");
        var zhiwang = CharacterFixtureContent.EmbeddedProgramBundle("hell-kings").Programs["hell:zhiwang"];
        var registry = ContentRegistry.Build(new StandardContentPackage(), new FactsPackage(zhiwang));
        var game = Start(registry);
        var actual = Draw(game);
        var capabilities = Capabilities(game);
        var hell = game.GetRuntimeModule<CoreHellKingProgramsRuntime>();
        Check(!capabilities.Get<INativeFactDemand>().RequiresFact(phase) && actual.Cards.Count == 1 &&
            actual.Invoices is [var invoice] && invoice.OwnerSeat == 0 && invoice.ActualCount == 1 &&
            actual.Batch.NativeDrawInvocation is { } proof && proof.InvocationId == invoice.InvocationId &&
            proof.Materials.Single().CardId == actual.Cards[0],
            "An unowned real Hell descriptor records the original draw invoice and material without enabling phase identity.");
        Check(hell.IsHellZhiwangGain(actual.Batch, 1, 0) && !hell.CollectHellZhiwangCandidates(actual.Batch).Any(),
            "The genuine outside-Draw gain is classifiable, but catalog demand alone cannot grant any character a Zhiwang candidate.");

        static SkillProgramDependencies Dependencies(SkillProgram program) =>
            new([new ContentSkillDefinition(program.Id, "Facts", "Facts") { Program = program }]);
        static ProgramCapabilityRegistry Capabilities(GameEngine game) => (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        static GameEngine Start(ContentRegistry content)
        {
            var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 5, ModeId = "identity:standard-5", HumanSeat = 0,
                HumanRole = Role.Lord, UseInteractiveSetup = false, AdvanceAfterHumanCommands = false }, content);
            Check(game.Submit(new StartGameCommand()).Accepted, "The native draw-demand fixture must start normally.");
            return game;
        }
        static (IReadOnlyList<int> Cards, NativeDrawInvocationRecordedEvent[] Invoices, CardMovementBatchContext Batch) Draw(GameEngine game)
        {
            for (var i = 0; i < 60 && (game.State.TurnNumber <= 0 || game.State.Phase != TurnPhase.Play); i++)
                Check(game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted, "The native draw fixture must reach its real turn.");
            Check(game.State.TurnNumber > 0 && game.State.Phase == TurnPhase.Play, "A native invoice test must run after real setup and outside Draw.");
            var capabilities = Capabilities(game);
            var history = capabilities.Get<IProgramEventHistoryQueries>();
            var before = history.Read<NativeDrawInvocationRecordedEvent>().Count;
            var cards = game.GetRuntimeModule<PhysicalCardDrawRuntime>().DrawCards(capabilities.Get<IMatchParticipantStateAccess>().Players[0], 1, false);
            var batches = capabilities.Get<ICardMovementBatchQueue>();
            return (cards, history.Read<NativeDrawInvocationRecordedEvent>().Skip(before).ToArray(), batches.PendingBatchAt(batches.PendingBatchCount - 1));
        }
        static void Check(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    }

    private static void MovementTimingFactDemand(GameEngine withoutDemand, ContentRegistry sharedCatalog)
    {
        const ProgramTrackedFact timing = ProgramTrackedFact.MovementTiming;
        var activation = ProgramOperationCatalog.Default.Resolve(SkillProgramEffectOp.ResolveDiciGiftRecoveryAndNeighborChain).FactRequirements;
        var trigger = ProgramOperationCatalog.Default.Resolve(SkillProgramEffectOp.ResolveBiriFirstNonDrawGain).FactRequirements;
        Check((activation.Activated & timing) != 0 && (activation.Triggered & timing) == 0 &&
            (trigger.Triggered & timing) != 0 && (trigger.Activated & timing) == 0,
            "Movement timing descriptors preserve the real activation and trigger entry points.");
        foreach (var id in new[] { "happy:renjie", "ol:renjie-2016", "mobile:mingzhe" })
            Check(new SkillProgramDependencies([new ContentSkillDefinition(id, "Passive", "Passive")]).RequiresFact(timing),
                "An unowned loaded passive must request its original movement timing: " + id);
        var pure = new MatchNativeFactDemand(timing);
        Check(pure.RequiresFact(timing) && !pure.RequiresFact(ProgramTrackedFact.ActualDiscardRecoveryPhase) &&
            !pure.RequiresFact(ProgramTrackedFact.ActualPhaseEnd) && !pure.RequiresFact(ProgramTrackedFact.NativeDrawInvocation),
            "Movement timing alone does not manufacture phase identity, phase ends or draw invoices.");

        var rules = JsonNode.Parse("""{"skills":[{"id":"fixture:optional-facts","revision":1,"triggers":[{"id":"outside-draw","window":"cardsGained","subject":"owner","optional":false,"destinationZones":["hand"],"movementOccurrence":"perBatch","gainPhaseQualification":"outsideOwnerDraw","effects":[{"op":"draw","target":"owner","amount":1}]}]}]}""")!;
        rules["schemaVersion"] = SkillProgramCatalog.RulesSchemaVersion;
        var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { ["fixture:optional-facts"] = new { name = "Timing", description = "Timing" } } });
        var program = SkillProgramCatalog.Load(rules.ToJsonString(), presentation).Programs["fixture:optional-facts"];
        var content = ContentRegistry.Build(new StandardContentPackage(), new FactsPackage(program));
        Check(content.ProgramDependencies.RequiresFact(timing) &&
            !content.ProgramDependencies.RequiresFact(ProgramTrackedFact.NativeDrawInvocation) &&
            !content.ProgramDependencies.RequiresFact(ProgramTrackedFact.ActualDiscardRecoveryPhase),
            "A generic gain-phase qualifier requests only movement timing, without a character operation or holder.");
        Check(withoutDemand.GetRuntimeModule<CoreGameEngineCardMovementProgramsRuntime>().CaptureMovementTiming() is null,
            "The ordinary no-demand match preserves omitted movement timing.");
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 5, ModeId = "identity:standard-5", HumanSeat = 0,
            HumanRole = Role.Lord, UseInteractiveSetup = false, AdvanceAfterHumanCommands = false }, content);
        Check(game.Submit(new StartGameCommand()).Accepted, "The unowned timing fixture starts normally.");
        for (var i = 0; i < 60 && (game.State.TurnNumber <= 0 || game.State.Phase != TurnPhase.Play); i++)
            Check(game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted, "The timing fixture advances its real native turn.");
        Check(game.State.TurnNumber > 0 && game.State.Phase == TurnPhase.Play,
            "The unowned timing fixture reaches a genuine native Play phase.");
        var captured = game.GetRuntimeModule<CoreGameEngineCardMovementProgramsRuntime>().CaptureMovementTiming();
        Check(captured == new CardMovementTiming(game.State.CurrentSeat, TurnPhase.Play, game.State.CurrentSeat),
            "Catalog gain qualification captures the actual turn owner, native phase and phase actor even without a holder.");
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var cards = game.GetRuntimeModule<PhysicalCardDrawRuntime>().DrawCards(capabilities.Get<IMatchParticipantStateAccess>().Players[0], 1, false);
        var batches = capabilities.Get<ICardMovementBatchQueue>();
        var batch = batches.PendingBatchAt(batches.PendingBatchCount - 1);
        Check(cards.Count == 1 && batch.MovementTiming == captured && batch.DiscardRecoveryPhase is null && batch.NativeDrawInvocation is null,
            "The real physical batch carries the captured timing without synthetic phase identity or a draw invoice.");

        var zones = (CardZoneStore)typeof(GameEngine).GetField("_cardZones", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var original = sharedCatalog.ProgramDependencies.RequiredFacts;
        var modeDemand = Demand(timing);
        Check(modeDemand.RequiresFact(timing) && !modeDemand.RequiresFact(ProgramTrackedFact.NativeDrawInvocation) &&
            !modeDemand.RequiresFact(ProgramTrackedFact.ActualDiscardRecoveryPhase) && !Demand(ProgramTrackedFact.None).RequiresFact(timing) &&
            sharedCatalog.ProgramDependencies.RequiredFacts == original,
            "Selected-mode timing demand composes per match without changing a shared catalog or another match's mask.");
        INativeFactDemand Demand(ProgramTrackedFact mode) => (INativeFactDemand)new MatchCoreCapabilityProvider(zones,
            content: sharedCatalog, modeRequiredFacts: mode).Bindings.Single(binding => binding.Contract == typeof(INativeFactDemand)).Instance;
        static void Check(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    }

    private static void AssertOmitted(SkillProgramTriggerFacts facts, string property)
    {
        if (JsonSerializer.Serialize(facts).Contains(property, StringComparison.Ordinal))
            throw new InvalidOperationException($"Legacy facts unexpectedly serialized {property} without an opt-in condition.");
    }

    private sealed class FactsPackage(SkillProgram program) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("optional-facts-test", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder) => builder.AddSkill(new ContentSkillDefinition(
            program.Id, "Facts", "Facts") { Program = program, ExecutionForms = SkillExecutionForm.Trigger });
    }

    private static void Reject(JsonNode rules, string presentation, string expected)
    {
        try { SkillProgramCatalog.Load(rules.ToJsonString(), presentation); }
        catch (InvalidOperationException error) when (error.Message.Contains(expected, StringComparison.Ordinal)) { return; }
        throw new InvalidOperationException($"Invalid composition was accepted instead of rejecting: {expected}");
    }

    private static (string Rules, string Presentation) Read(string bundle)
    {
        var assembly = typeof(StandardContentPackage).Assembly;
        string Text(string suffix)
        {
            using var stream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames()
                .Single(name => name.EndsWith(bundle + suffix, StringComparison.Ordinal)))!;
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        return (SkillProgramTestJson.VersionedEmbeddedRules(Text(".rules.json")), Text(".presentation.json"));
    }
}
