using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class CardEntityMarkChecks
{
    private const string Skill = "fixture:physical-card-marks", Owner = "fixture:card-mark-owner", Peer = "fixture:card-mark-peer";
    private const string Deck = "fixture:card-mark-deck";

    public static void OpeningMarksBindingsAndZoneCountsKeepTheirPhysicalIdentityCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Create(registry);
        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        void Submit(GameCommand command)
        { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected mark command."); }
        void Reach(Func<bool> done)
        {
            for (var step = 0; step < 120; step++)
            {
                if (done()) return;
                if (Prompt() is { } prompt && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip"))
                    Submit(new AnswerPromptCommand(0, prompt.PromptId,
                        prompt.Choices.First(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip").Id, game.Revision));
                else if (Prompt() is null) Submit(new AdvanceOneStepCommand(game.Revision));
                else throw new InvalidOperationException("Unexpected card-mark boundary " + JsonSerializer.Serialize(Prompt()));
            }
            throw new InvalidOperationException("Card marks missed their bounded native boundary.");
        }
        void Cold()
        {
            var before = SnapshotJson.Serialize(game.CreateSnapshot(0));
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Need(SnapshotJson.Serialize(game.CreateSnapshot(0)) == before, "Cold recovery retains the original mark prompt and physical Hand.");
        }
        ICardEntityMarkQueries Marks() => game.GetRuntimeModule<CardEntityMarkRuntime>();
        Submit(new StartGameCommand());
        Reach(() => Prompt()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip") == true);
        var opening = game.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>()
            .Single(frame => frame.Window == SkillProgramTriggerWindow.GameStarting).OpeningHandEntities!;
        var initial = opening.Where(entity => entity.OwnerSeat == 0).Select(entity => entity.CardId).ToArray();
        var frozen = false;
        try { ((IList<OpeningHandEntity>)opening).Clear(); } catch (NotSupportedException) { frozen = true; }
        Need(initial.Length == 4 && frozen && Marks().MarkedCardIds("opening").Count == 0,
            "The opening mother freezes the true initial Hand before an earlier opening draw can add another card.");
        Cold();
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard });
        Need(Marks().MarkedCardIds("opening").SequenceEqual(initial) &&
            initial.All(id => Marks().InitialOwnerSeat(id, "opening") == 0) &&
            game.CreateSnapshot(0).Players[0].HandCount == initial.Length + 1,
            "MarkInitialHand uses the frozen original identities, excluding cards gained by an earlier opening child.");
        Need(initial.All(id => game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Id == id).SkillMarkLabel == "开局") &&
            game.CreateSnapshot(1).Players[0].Hand.Count == 0,
            "The actual generic presentation exposes declared names on the owner's visible cards without exposing that hand to another viewer.");
        var fresh = game.CreateSnapshot(0).Players[0].Hand.Single(card => !Marks().Has(card.Id, "opening")).Id;
        var hand = game.CreateSnapshot(0).Players[0].HandCount;
        Submit(new UseProgramSkillCommand(0, Skill, "tag", [], [], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        var selection = Prompt()!;
        Submit(new AnswerPromptCommand(0, selection.PromptId,
            selection.Choices.Single(choice => choice.Cards.SequenceEqual([fresh])).Id, game.Revision));
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard });
        Need(Marks().Has(fresh, "chosen") && Marks().InitialOwnerSeat(fresh, "chosen") is null &&
            Marks().CountMarked(0, "chosen", CardZoneKind.Hand) == 1 && game.CreateSnapshot(0).Players[0].HandCount == hand + 1,
            "MarkCards reuses its accepted binding and cardMarkCount reads the physical marked Hand exactly once.");
        Need(game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Id == fresh).SkillMarkLabel == "所选",
            "A JSON mark declaration automatically labels a newly marked visible card without a character projection hook.");
        Cold();
        Submit(new UseProgramSkillCommand(0, Skill, "drop", [fresh], [], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard });
        Need(Marks().Has(fresh, "chosen") && Marks().CountMarked(0, "chosen", CardZoneKind.Hand) == 0 &&
            Marks().CountMarked(0, "chosen", CardZoneKind.Equipment) == 0,
            "Leaving the Hand changes zone counts without removing the permanent physical mark.");
        Cold();
        Need(Marks().Has(fresh, "chosen") && Marks().MarkedCardIds("opening").SequenceEqual(initial),
            "Replay rebuilds the same permanent journal facts without a second mark ledger.");
        Need(game.GetRuntimeModule<CardSkillMarkProjector>().Label(fresh) == "所选",
            "A public discarded physical entity retains its declared name after cold recovery.");
    }

    public static void IssuedOpeningMarksOutliveSourceLossAndOwnerDeathCold()
    {
        foreach (var terminal in new[] { "lost", "dead" })
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(terminal));
            var game = Create(registry);
            void Submit(GameCommand command)
            { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected issued-opening command."); }
            void Reach(Func<bool> done)
            {
                for (var step = 0; step < 160; step++)
                {
                    if (done()) return;
                    var prompt = game.CreateSnapshot(0).PendingDecision;
                    if (prompt is not null)
                    {
                        var choice = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip") ??
                            (prompt.Kind == DecisionKind.RescueDying ? prompt.Choices.FirstOrDefault(choice => choice.Cards.Count == 0) : null);
                        Need(choice is not null, "Only the original opening draw or native dying response may suspend this fixture. " +
                            "terminal=" + terminal + "; prompt=" + JsonSerializer.Serialize(prompt) +
                            "; frames=" + string.Join(",", game.ResolutionStack.Select(frame => frame.GetType().Name)) +
                            "; bindings=" + JsonSerializer.Serialize(game.Events.Select(envelope => envelope.Payload)
                                .OfType<ProgramBindingResolvedEvent>().Where(binding => binding.SkillId == Skill)));
                        Submit(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, game.Revision));
                    }
                    else Submit(new AdvanceOneStepCommand(game.Revision));
                }
                throw new InvalidOperationException("The issued opening mark did not finish after " + terminal);
            }
            Submit(new StartGameCommand());
            Reach(() => game.CreateSnapshot(0).PendingDecision?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip") == true);
            var initial = game.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>()
                .Single(frame => frame.Window == SkillProgramTriggerWindow.GameStarting).OpeningHandEntities!
                .Where(entity => entity.OwnerSeat == 0).Select(entity => entity.CardId).ToArray();
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Reach(() => game.Events.Any(envelope => envelope.Payload is ProgramBindingResolvedEvent done &&
                done.SkillId == Skill && done.BindingId == "mark-opening" && done.Activated && done.Completed));
            var marks = game.GetRuntimeModule<CardEntityMarkRuntime>();
            Need(marks.MarkedCardIds("opening").SequenceEqual(initial) && initial.All(id => marks.InitialOwnerSeat(id, "opening") == 0),
                "The accepted opening candidate must mark its original entities after an earlier child removes its source or kills its owner.");
            var player = game.CreateSnapshot(0).Players[0];
            Need(terminal == "dead" ? !player.IsAlive : player.Skills?.Any(skill => skill.Id == Skill) != true,
                "The fixture must actually reach its native source-loss or owner-death branch before the retained marker completes.");
        }
        IssuedGainMarksRetainOnlyTheirOriginalWindow();
        GainInvocationDeclarationsRejectInvalidShapes();
    }

    private static void IssuedGainMarksRetainOnlyTheirOriginalWindow()
    {
        foreach (var terminal in new[] { "gain-lost", "gain-dead" })
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(terminal));
            var game = Create(registry);
            PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
            void Submit(GameCommand command)
            { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected gain authorization command."); }
            void Reach(Func<bool> done)
            {
                for (var step = 0; step < 180; step++)
                {
                    if (done()) return;
                    if (Prompt() is { } prompt)
                    {
                        var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                            (prompt.Kind == DecisionKind.RescueDying ? prompt.Choices.FirstOrDefault(c => c.Cards.Count == 0) : null);
                        Need(choice is not null, "Unexpected gain authorization boundary: " + terminal + "; " + JsonSerializer.Serialize(prompt));
                        Submit(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, game.Revision));
                    }
                    else Submit(new AdvanceOneStepCommand(game.Revision));
                }
                throw new InvalidOperationException("The issued gain mark did not finish: " + terminal);
            }
            Submit(new StartGameCommand());
            Reach(() => Prompt() is { Kind: DecisionKind.PlayCard });
            var sequence = game.CardMovements.LastOrDefault()?.Sequence ?? 0;
            Submit(new UseProgramSkillCommand(0, Skill, "gain", [], [], game.Revision, Prompt()!.PromptId));
            Reach(() => Prompt()?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip") == true);
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Reach(() => game.Events.Select(e => e.Payload).OfType<ProgramBindingResolvedEvent>().Any(e =>
                e.SkillId == "le:lingkong" && e.BindingId == "mark-first-outside-draw-gain" && e.Activated && e.Completed));
            var gained = game.CardMovements.Where(m => m.Sequence > sequence && m.From == CardLocation.DrawPile &&
                m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program." + Skill + ".Draw").Select(m => m.CardId).ToArray();
            var marks = game.Events.Select(e => e.Payload).OfType<CardEntityMarkedEvent>()
                .Where(e => e.Mark == "lingkong" && gained.Contains(e.CardId)).ToArray();
            Need(gained.Length == 2 && marks.Select(e => e.CardId).Order().SequenceEqual(gained.Order()) &&
                    marks.Select(e => e.FrameId).Distinct().Count() == 1 && marks.All(e => e.InitialOwnerSeat is null) &&
                    gained.All(id => game.GetRuntimeModule<CardEntityMarkRuntime>().Has(id, "lingkong")) &&
                    game.Events.Select(e => e.Payload).OfType<ProgramBindingResolvedEvent>().Count(e =>
                        e.SkillId == "le:lingkong" && e.BindingId == "mark-first-outside-draw-gain" && e.Completed) == 1,
                "One shared initial-gain authorization finishes the original two-card mark after an earlier movement child, without duplicate payment or binding.");
            Need(terminal == "gain-dead" ? !game.CreateSnapshot(0).Players[0].IsAlive :
                    game.CreateSnapshot(0).Players[0].Skills?.Any(skill => skill.Id == "le:lingkong") != true,
                "The actual native child must remove the source or kill its owner before the retained gain mark.");
            if (terminal == "gain-dead")
            {
                Reach(() => game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().All(window =>
                    window.CandidateIndex >= window.Candidates.Count ||
                    window.Candidates.Skip(window.CandidateIndex).All(candidate => candidate.BindingId != "gain-stop")));
                Need(!game.Events.Select(e => e.Payload).OfType<CardEntityMarkedEvent>().Any(e =>
                        e.Mark == "stop-gain" && gained.Contains(e.CardId)),
                    "A captured source-loss authorization without ownerDeath retain must not execute captureCards or markCards after owner death.");
            }
        }
    }

    private static void GainInvocationDeclarationsRejectInvalidShapes()
    {
        var valid = JsonNode.Parse(SkillProgramTestJson.CurrentSchema("""
        {"schemaVersion":__RULES_SCHEMA__,"skills":[{"id":"fixture:gain-invocation","revision":1,"triggers":[{
          "id":"issued","window":"cardsGained","subject":"owner","movementOccurrence":"perBatch",
          "destinationZones":["hand"],"optional":false,"invocation":{"source":"initialHandGain","ownerDeath":"retain"},
          "effects":[{"op":"setChainedState","target":"owner","chained":true}]}]}]}
        """))!;
        var presentation = JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { ["fixture:gain-invocation"] = new { name = "Gain invocation", description = "Shared window authorization" } }
        });
        SkillProgramCatalog.Load(valid.ToJsonString(), presentation);
        var ordinary = valid.DeepClone();
        ordinary["skills"]![0]!["triggers"]![0]!["invocation"]!.AsObject().Remove("ownerDeath");
        Need(SkillProgramCatalog.Load(ordinary.ToJsonString(), presentation).Programs["fixture:gain-invocation"].Triggers[0].Invocation?.OwnerDeath ==
                SkillProgramInvocationOwnerDeath.Stop, "Omitted death behavior retains the explicit default stop policy.");
        var paidOrdinary = ordinary.DeepClone();
        paidOrdinary["skills"]![0]!["triggers"]![0]!["markerCost"] = JsonNode.Parse("""{"marker":"ren","amount":1}""");
        SkillProgramCatalog.Load(paidOrdinary.ToJsonString(), presentation);
        foreach (var invalid in new[] { "window", "optional", "markerCost", "movementReasons", "excludedMovementReasons", "ignoreOwnSkillMovements" })
        {
            var copy = valid.DeepClone();
            var trigger = copy["skills"]![0]!["triggers"]![0]!;
            if (invalid == "window")
            {
                trigger["window"] = "cardsMoved";
                trigger.AsObject().Remove("destinationZones");
                trigger["sourceZones"] = new JsonArray("hand");
            }
            else if (invalid == "optional") trigger["optional"] = true;
            else if (invalid == "markerCost") trigger["markerCost"] = JsonNode.Parse("""{"marker":"ren","amount":1}""");
            else if (invalid == "ignoreOwnSkillMovements") trigger[invalid] = true;
            else trigger[invalid] = new JsonArray("rule.draw");
            var expected = invalid switch
            {
                "window" => "initialHandGain requires an owner cardsGained perBatch hand trigger",
                "optional" or "markerCost" => "retain requires a mandatory trigger without a marker cost",
                _ => "initialHandGain does not support movement reason or own-skill filters"
            };
            try { SkillProgramCatalog.Load(copy.ToJsonString(), presentation); }
            catch (InvalidOperationException error) when (error.Message.Contains(expected, StringComparison.Ordinal)) { continue; }
            throw new InvalidOperationException("Invalid gain invocation was accepted: " + invalid);
        }
    }

    private static GameEngine Create(ContentRegistry registry)
    {
        var setup = new ShanheBattleSetup(7, 1, "card-mark-core", Owner, 4, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Club, 2), new(CardKind.Slash, Suit.Heart, 3),
                new(CardKind.Slash, Suit.Spade, 4), new(CardKind.Slash, Suit.Diamond, 5)], [],
            [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
            [new(Peer) { StartingHand = [] }], new string('a', 64));
        return GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry);
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture(string? terminal = null) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-card-entity-marks", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Skill}}","revision":1,
              "triggers":[
                {"id":"grow-opening","window":"gameStarting","subject":"owner","priority":20,"optional":false,"effects":[{"op":"draw","target":"owner","amount":1}]},
                {"id":"mark-opening","window":"gameStarting","subject":"owner","priority":10,"optional":false,"effects":[{"op":"markInitialHand","target":"owner","mark":"opening"}]},
                {"id":"hold-draw","window":"nativeDrawStarting","subject":"owner","optional":true,"effects":[{"op":"setChainedState","target":"owner","chained":true}]},
                {"id":"no-normal-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}],
              "activations":[
                {"id":"tag","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                  "effects":[{"op":"selectOwnedCards","target":"owner","amount":1,"zones":["hand"],"resultBind":"material"},
                    {"op":"revealBoundCards","target":"owner","sourceBind":"material"},{"op":"markCards","target":"owner","mark":"chosen","bind":"material"},
                    {"op":"draw","target":"owner","amountExpr":["cardMarkCount","owner","chosen","hand"]}]},
                {"id":"drop","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                  "effects":[{"op":"discardSelected","target":"owner","amount":1}]}]}]}
            """;
            if (terminal is "lost" or "dead")
            {
                var definition = System.Text.Json.Nodes.JsonNode.Parse(rules)!;
                definition["skills"]![0]!["triggers"]![0]!["effects"]!.AsArray().Add(System.Text.Json.Nodes.JsonNode.Parse(
                    terminal == "lost"
                        ? $$$"""{"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{{Skill}}}"]}}"""
                        : """{"op":"loseHp","target":"owner","amount":10}"""));
                rules = definition.ToJsonString();
            }
            var gainScenario = terminal is "gain-lost" or "gain-dead";
            if (gainScenario)
            {
                var definition = JsonNode.Parse(rules)!;
                var skill = definition["skills"]![0]!;
                skill["states"] = JsonNode.Parse("""[{"id":"gain-ready","initialValue":false,"visibility":"public","resetScope":"game","reacquirePolicy":"preserveUntilGameEnd"}]""");
                var operation = terminal == "gain-lost" ?
                    """{"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["le:lingkong"]}}""" :
                    """{"op":"loseHp","target":"owner","amount":10}""";
                skill["triggers"]!.AsArray().Add(JsonNode.Parse("""
                {"id":"gain-terminal","window":"cardsGained","subject":"owner","priority":1000,"optional":false,
                  "movementOccurrence":"perBatch","destinationZones":["hand"],"usageScope":"game","usageLimit":1,
                  "condition":{"kind":"booleanState","stateId":"gain-ready","expectedValue":true},"effects":[
                """ + operation + "]}"));
                skill["triggers"]!.AsArray().Add(JsonNode.Parse("""
                {"id":"gain-stop","window":"cardsGained","subject":"owner","priority":-10,"optional":false,
                  "movementOccurrence":"perBatch","destinationZones":["hand"],"invocation":{"source":"initialHandGain"},
                  "condition":{"kind":"booleanState","stateId":"gain-ready","expectedValue":true},"effects":[
                    {"op":"captureCards","target":"owner","resultBind":"stop-gained","cardsExpr":["gainCards","owner","incoming"]},
                    {"op":"markCards","target":"owner","bind":"stop-gained","mark":"stop-gain"}]}
                """));
                skill["activations"]!.AsArray().Add(JsonNode.Parse("""
                {"id":"gain","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                  "effects":[{"op":"setBooleanState","target":"owner","stateId":"gain-ready","value":true},
                    {"op":"draw","target":"owner","amount":2,"awaitMovementTriggers":true}]}
                """));
                rules = definition.ToJsonString();
                var lingkong = CharacterFixtureContent.EmbeddedProgramBundle("le-zhou-fei");
                foreach (var (id, program) in lingkong.Programs)
                    builder.AddSkill(new(id, lingkong.Presentations[id].Name, lingkong.Presentations[id].Description)
                    { Program = program, ProgramPresentation = lingkong.Presentations[id] });
            }
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Skill] = new { name = "Physical marks", description = "Shared physical mark fixture",
                    cardMarks = new Dictionary<string, string> { ["opening"] = "开局", ["chosen"] = "所选", ["stop-gain"] = "普通获得" } } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Skill, "Physical marks", "Shared physical mark fixture") { Program = catalog.Programs[Skill], ProgramPresentation = catalog.Presentations[Skill] });
            builder.AddSkill(new("fixture:card-mark-noop", "Peer", "No active rules"));
            builder.AddGeneral(new(Owner, "Mark owner", "", Skill, "wei", 4,
                AdditionalSkillIds: gainScenario ? ["le:lingkong"] : null));
            builder.AddGeneral(new(Peer, "Mark peer", "", "fixture:card-mark-noop", "wei", 8));
            builder.AddDeck(new(Deck, "Fixed physical cards", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 60)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Shared card marks", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
