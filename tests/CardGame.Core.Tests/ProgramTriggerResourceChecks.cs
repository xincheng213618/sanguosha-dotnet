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

    private static void AssertOmitted(SkillProgramTriggerFacts facts, string property)
    {
        if (JsonSerializer.Serialize(facts).Contains(property, StringComparison.Ordinal))
            throw new InvalidOperationException($"Legacy facts unexpectedly serialized {property} without an opt-in condition.");
    }

    private sealed class FactsPackage(SkillProgram program) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("optional-facts-test", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder) => builder.AddSkill(new ContentSkillDefinition(
            "fixture:optional-facts", "Facts", "Facts") { Program = program, ExecutionForms = SkillExecutionForm.Trigger });
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
