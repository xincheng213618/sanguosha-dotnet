using CardGame.Content.Standard;
using CardGame.Core;
using System.Reflection;
using System.Text.Json;

internal static class ProgramLifecycleChecks
{
    private const int HumanSeat = 0;

    public static void RejectsCardBindingsAcrossDetachedPhaseBoundary()
    {
        var rules = Rules("""
            {"op":"revealTopCards","target":"owner","amount":1,"resultBind":"held","visibility":"public"},
            {"op":"insertPhase","target":"owner","phase":"play","phaseContinuation":"beforeNormalPreparation"},
            {"op":"moveBoundCards","target":"owner","sourceBind":"held","destination":"discardPile"}
            """);
        try
        {
            _ = Registry(rules);
        }
        catch (InvalidOperationException exception) when (
            exception.Message.Contains("card bindings cannot cross", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
        throw new InvalidOperationException("A phase insertion with live card bindings must fail at content load time.");
    }

    public static void MissingConditionalBindingsCancelWithoutLeakingCards()
    {
        var rules = Rules("""
            {"op":"revealTopCards","target":"owner","amount":1,"resultBind":"held","visibility":"public","condition":{"kind":"wounded"}},
            {"op":"selectCardSubset","target":"owner","sourceBind":"held","resultBind":"selected","minimumCards":0,"maximumCards":1,"maximumRankSum":13,"aiOrder":"mostCardsThenRankSum"}
            """);
        try { _ = Registry(rules); }
        catch (InvalidOperationException exception) when (
            exception.Message.Contains("resource operations must be always", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
        throw new InvalidOperationException("A conditional binding producer must be rejected before it can leak cards.");
    }

    public static void CancelledDamageProgramsDoNotCleanupParentAttackCards()
    {
        var registry = Registry(Rules("""
            {"op":"revealTopCards","target":"owner","amount":1,"resultBind":"held","visibility":"public"},
            {"op":"moveBoundCards","target":"owner","sourceBind":"held","destination":"discardPile"}
            """, "afterDamageApplied"));
        var game = CreateAndSelect(registry);
        ReachHumanPlay(game);
        var prompt = RequirePrompt(game, DecisionKind.PlayCard);
        var slash = game.GetHumanLegalActions().First(action =>
            action.Kind == LegalActionKind.Slash && action.TargetSeats.Count == 1);
        var sourceCardId = slash.CardId!.Value;
        var played = game.Submit(new PlayCardCommand(
            HumanSeat,
            sourceCardId,
            slash.TargetSeats,
            game.Revision,
            prompt.PromptId,
            slash.PlayedCardKind,
            slash.TargetCardId)
        {
            ConversionSource = slash.ConversionSource
        });
        Require(played.Accepted, played.Error?.Message ?? "The lifecycle fixture Slash was rejected.");
        AdvanceUntilProgramResolved(game);

        Require(game.Events.Select(item => item.Payload).OfType<ProgramBindingResolvedEvent>()
                    .Any(item => item.SkillId == FixturePackage.SkillId && item.Completed) &&
                game.CardMovements.Where(move => move.CardId == sourceCardId)
                    .All(move => !move.Reason.Value.Contains("skill-program", StringComparison.Ordinal)),
            "Program disposal must move only its own bound cards, never the parent attack card.");
    }

    public static void ConditionalTargetSetsSkipPromptsAndResume()
    {
        foreach (var expression in new[] { "false", """["==",["hp","selectedTarget"],1]""", "true" })
        {
            var registry = Registry(Rules($$"""
                {"op":"selectTargets","target":"owner","targetKind":"anyLiving","minimumTargets":0,"maximumTargets":1,"numberExpression":"currentHandCount","targetAiOrder":"supportDraw","condition":{"kind":"expression","expr":{{expression}} } },
                {"op":"draw","target":"selectedTargets","amount":1,"condition":{"kind":"expression","expr":{{expression}} } },
                {"op":"draw","target":"owner","amount":1}
                """));
            var game = CreateAndSelect(registry);
            if (expression == "true")
            {
                for (var step = 0; game.PendingDecision?.Choices.Any(choice =>
                    choice.Parameters.GetValueOrDefault("program-action") == "select-targets") != true; step++)
                {
                    Require(step < 80 && game.PendingDecision is null,
                        "An enabled conditional target set must reach its original selection prompt.");
                    Require(game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted,
                        "The conditional target fixture could not reach selection.");
                }
                var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single();
                Require(frame.InstructionIndex == 1,
                    "The selector commits its cursor before publishing the existing target prompt.");
                var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(
                    GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
                foreach (var (branch, targets) in new[] { (game, new[] { 1 }), (restored, Array.Empty<int>()) })
                {
                    var prompt = RequirePrompt(branch, DecisionKind.ProgramTrigger);
                    var choice = prompt.Choices.Single(item => item.Targets.SequenceEqual(targets));
                    Require(branch.Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, branch.Revision)).Accepted,
                        "A resumed conditional target set must accept its published recipient.");
                    ReachHumanPlay(branch);
                    CheckDraws(branch, 1 + targets.Length, targets.Length);
                }
            }
            else
            {
                ReachHumanPlay(game);
                CheckDraws(game, 1, 0);
            }
        }

        const string selector = """{"op":"selectTargets","target":"owner","targetKind":"anyLiving","minimumTargets":0,"maximumTargets":1,"numberExpression":"currentHandCount","targetAiOrder":"supportDraw","condition":CONDITION}""";
        const string draw = """{"op":"draw","target":"selectedTargets","amount":1,"condition":CONDITION}""";
        static string Pair(string selected, string drawn) =>
            selector.Replace("CONDITION", selected) + "," + draw.Replace("CONDITION", drawn);
        RejectPair(Pair("""{"kind":"expression","expr":false}""", """{"kind":"expression","expr":true}"""),
            "immediately preceding committed optional recipient selection");
        RejectPair("""
            {"op":"selectOwnedCards","target":"owner","numberExpression":"allOwnedZoneCards","zones":["hand"],"resultBind":"first"},
            {"op":"selectOwnedCards","target":"owner","numberExpression":"allOwnedZoneCards","zones":["hand"],"resultBind":"second"},
            """ + Pair("""{"kind":"boundCardsSameColor","sourceBind":"first"}""", """{"kind":"boundCardsSameColor","sourceBind":"second"}"""),
            "immediately preceding committed optional recipient selection");
        RejectPair(Pair("""{"kind":"boundCardsSameColor","sourceBind":"missing"}""", """{"kind":"boundCardsSameColor","sourceBind":"missing"}"""),
            "unknown card binding");

        static void RejectPair(string effects, string error)
        {
            try { _ = Registry(Rules(effects)); }
            catch (InvalidOperationException failure) when (failure.Message.Contains(error, StringComparison.Ordinal)) { return; }
            throw new InvalidOperationException("A conditional recipient draw accepted an unmatched or missing binding.");
        }

        static void CheckDraws(GameEngine game, int total, int recipient)
        {
            var draws = game.CardMovements.Where(move =>
                move.Reason.Value == "skill-program.fixture:lifecycle.Draw").ToArray();
            Require(draws.Length == total && draws.Count(move => move.To == CardLocation.Hand(0)) == 1 &&
                draws.Count(move => move.To == CardLocation.Hand(1)) == recipient &&
                game.Events.Select(item => item.Payload).OfType<ProgramBindingResolvedEvent>()
                    .Count(item => item.SkillId == FixturePackage.SkillId && item.Completed) == 1,
                "False or unknown conditions skip selection; an enabled selection resumes once before the following owner draw.");
        }
    }

    public static void ComposedChoicesReadExpressionsAndResumePaidDraws()
    {
        foreach (var gate in new[] { "false", """[">",["hp","selectedTarget"],0]""", "true" })
        {
            var registry = Registry(Rules("""
                {"op":"captureNumber","target":"owner","resultBind":"allowed","valueExpr":1},
                {"op":"chooseOption","target":"owner","resultBind":"reward","condition":{"kind":"expression","expr":GATE},"options":[
                    {"id":"take","condition":{"kind":"expression","expr":["==",["number","allowed"],1]}},
                    {"id":"hidden","condition":{"kind":"expression","expr":false}},
                    {"id":"unknown","condition":{"kind":"expression","expr":[">",["hp","selectedTarget"],0]}},
                    {"id":"decline"}]},
                {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true,"condition":{"kind":"choiceIs","sourceBind":"reward","optionId":"take"}},
                {"op":"captureNumber","target":"owner","resultBind":"continued","valueExpr":1}
                """.Replace("GATE", gate)), observeDraw: true);
            var game = CreateAndSelect(registry);
            if (gate == "true")
            {
                ReachDecision(game, prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("result-bind") == "reward"));
                Require(game.PendingDecision!.Choices.Select(choice => choice.Parameters["option-id"]).SequenceEqual(["take", "decline"]),
                    "Option expressions read the owning frame's captured number and omit false or unknown choices.");
                var id = game.ResolutionStack.OfType<ProgramSkillFrame>().Single().Id;
                game = Cold(game, registry);
                Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "take");
                ReachDecision(game, prompt => prompt.SkillPrompt?.SkillId == FixturePackage.Watch);
                var producer = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == id);
                var payloads = game.Events.Select(item => item.Payload).ToArray();
                Require(producer.NativeDraw is { InstructionIndex: 3, RecipientApplied: true } &&
                    payloads.OfType<ProgramDrawCompletedEvent>().Single(draw => draw.FrameId == id) is
                        { InstructionIndex: 3, RecipientSeat: 0, RequestedCount: 1, ActualDrawCount: 1 } &&
                    Array.FindIndex(payloads, item => item is ProgramDrawCompletedEvent draw && draw.FrameId == id) <
                    Array.FindIndex(payloads, item => item is ProgramBindingStartedEvent child && child.SkillId == FixturePackage.Watch),
                    "The paid Draw keeps its original cursor and publishes one completion fact before its movement observer child.");
                game = Cold(game, registry);
                Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
                ReachHumanPlay(game);
                Require(game.Events.Select(item => item.Payload).OfType<ProgramDrawCompletedEvent>().Count(draw => draw.FrameId == id) == 1 &&
                    !game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == id),
                    "Cold continuation drains the paid movement child without drawing again or leaving its producer pending.");
            }
            else
            {
                ReachHumanPlay(game);
                Require(!game.Events.Select(item => item.Payload).OfType<ProgramDrawCompletedEvent>().Any(),
                    "False or unknown choice conditions skip the prompt and its bound reward.");
            }
            Require(game.Events.Select(item => item.Payload).OfType<ProgramExpressionNumberCapturedEvent>()
                .Count(capture => capture.Name == "continued") == 1,
                "Every branch continues the original instruction sequence once.");
        }
    }

    public static void HappyFirstDrawKeepsAcceptedPaymentAndDecline()
    {
        const string jilue = "happy:jilue", first = "first-jilue-reward";
        foreach (var loseSkill in new[] { false, true })
        {
            var registry = Registry(Rules("""
                {"op":"changeAttributedMarker","target":"owner","targetRef":{"kind":"owner"},"marker":"ren","amount":1}
                """, "gameStarting"), happySima: true);
            var game = CreateAndSelect(registry);
            ReachHumanPlay(game);
            StartJilue(game);
            ReachDecision(game, prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("result-bind") == first));
            var accepted = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == jilue);
            var runtime = game.GetRuntimeModule<CardGame.Core.EngineOwners.CharacterSimaYiHappyGodRuntime>();
            Require(accepted.HappySimaYi is { } receipt && runtime.HappySimaPaidRen(accepted)?.ResolutionId == receipt.Payment &&
                runtime.RetainHappySimaFirst(accepted) && runtime.ChooseHappySimaFirst(game.PendingDecision!).Parameters["option-id"] == "draw",
                "The real first-use choice retains its accepted Ren payment and original draw-first AI policy.");
            game = Cold(game, registry);
            if (loseSkill)
            {
                var owner = ((IReadOnlyList<CharacterState>)FixtureRuntimeAccess.Engine.GetField("_players",
                    BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!)[0];
                foreach (var grant in owner.SkillGrants.Grants.Where(grant => grant.SkillId == jilue).ToArray())
                    owner.SkillGrants.SetEnabled(grant.GrantId, false);
            }
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == (loseSkill ? "draw" : "decline"));
            ReachHumanPlay(game);
            var draws = game.Events.Select(item => item.Payload).OfType<ProgramDrawCompletedEvent>().Where(draw => draw.FrameId == accepted.Id).ToArray();
            Require(draws is [{ RequestedCount: 1, ActualDrawCount: 1, InstructionIndex: var instruction }] &&
                instruction == (loseSkill ? 3 : 4),
                "Decline skips only the first reward; skill loss still honors that accepted reward and then stops the later Jizhi draw.");
            if (!loseSkill)
            {
                StartJilue(game);
                ReachHumanPlay(game);
                Require(game.Events.Select(item => item.Payload).OfType<ProgramExpressionNumberCapturedEvent>()
                    .Where(capture => capture.Name == "first-jilue-use").Select(capture => capture.Value).SequenceEqual([1, 0]),
                    "Declining consumes the actual turn's first accepted use, so a second paid Jilue never asks again.");
            }
        }

        static void StartJilue(GameEngine game)
        {
            var prompt = RequirePrompt(game, DecisionKind.PlayCard);
            var action = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.DrawTwo);
            Require(game.Submit(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, game.Revision,
                prompt.PromptId, action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource }).Accepted,
                "The real Jilue fixture could not use its ordinary trick.");
            ReachDecision(game, decision => decision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("skill-id") == jilue));
            Answer(game, choice => choice.Parameters.GetValueOrDefault("skill-id") == jilue);
        }
    }

    private static void ReachDecision(GameEngine game, Func<PendingDecision, bool> done)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.PendingDecision is { } prompt)
            {
                Require(done(prompt), $"Unexpected composition fixture prompt: {prompt.Kind}, {prompt.SkillPrompt?.SkillId}.");
                return;
            }
            Require(game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted, "The composition fixture could not advance.");
        }
        throw new InvalidOperationException("The composition fixture did not reach its bounded decision.");
    }

    private static void Answer(GameEngine game, Func<PromptChoice, bool> choose)
    {
        var prompt = RequirePrompt(game, DecisionKind.ProgramTrigger);
        Require(game.Submit(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First(choose).Id, game.Revision)).Accepted,
            "The composition fixture could not answer its current choice.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry registry) =>
        GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);

    private static void AdvanceUntilProgramResolved(GameEngine game)
    {
        for (var step = 0; step < 512; step++)
        {
            if (game.Events.Select(item => item.Payload).OfType<ProgramBindingResolvedEvent>()
                .Any(item => item.SkillId == FixturePackage.SkillId)) return;
            if (game.PendingDecision is { PlayerSeat: HumanSeat } prompt)
            {
                var decline = prompt.Choices.FirstOrDefault(choice => choice.Cards.Count == 0);
                if (decline is null) throw new InvalidOperationException($"Unexpected prompt {prompt.Kind}.");
                var answered = game.Submit(new AnswerPromptCommand(
                    HumanSeat, prompt.PromptId, decline.Id, game.Revision));
                Require(answered.Accepted, answered.Error?.Message ?? "The lifecycle fixture prompt was rejected.");
                continue;
            }
            var advanced = game.Submit(new AdvanceOneStepCommand(game.Revision));
            Require(advanced.Accepted, advanced.Error?.Message ?? "The lifecycle fixture could not advance.");
        }
        throw new InvalidOperationException("The lifecycle fixture did not resolve its program in bounded steps.");
    }

    private static GameEngine CreateAndSelect(ContentRegistry registry)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 7,
            PlayerCount = 4,
            ModeId = FixturePackage.ModeId,
            HumanSeat = HumanSeat,
            HumanRole = Role.Lord,
            UseInteractiveSetup = true,
            UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false,
            AiPolicyVersion = 2,
            MaxTurns = 20
        }, registry);
        Require(game.Submit(new StartGameCommand()).Accepted, "The lifecycle fixture failed to start.");
        var selection = RequirePrompt(game, DecisionKind.SelectGeneral);
        var selected = game.Submit(new SelectGeneralCommand(
            HumanSeat,
            FixturePackage.OwnerGeneralId,
            game.Revision,
            selection.PromptId));
        Require(selected.Accepted, selected.Error?.Message ?? "The lifecycle fixture could not select its general.");
        return game;
    }

    private static void ReachHumanPlay(GameEngine game)
    {
        for (var step = 0; step < 1024; step++)
        {
            if (game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: HumanSeat }) return;
            Require(game.PendingDecision?.PlayerSeat != HumanSeat,
                $"Unexpected human prompt {game.PendingDecision?.Kind} while reaching Play.");
            var advanced = game.Submit(new AdvanceOneStepCommand(game.Revision));
            Require(advanced.Accepted, advanced.Error?.Message ?? "The lifecycle fixture could not advance.");
        }
        throw new InvalidOperationException("The lifecycle fixture did not reach human Play in bounded steps.");
    }

    private static PendingDecision RequirePrompt(GameEngine game, DecisionKind kind) =>
        game.PendingDecision is { PlayerSeat: HumanSeat } prompt && prompt.Kind == kind
            ? prompt
            : throw new InvalidOperationException(
                $"Expected {kind}, found {game.PendingDecision?.Kind.ToString() ?? "no prompt"}.");

    private static ContentRegistry Registry(string rules, bool observeDraw = false, bool happySima = false)
    {
        using var document = JsonDocument.Parse(rules);
        var labels = document.RootElement.GetProperty("skills")[0].EnumerateObject()
            .Where(property => property.Name is "triggers" or "activations").SelectMany(property => property.Value.EnumerateArray())
            .SelectMany(entry => entry.GetProperty("effects").EnumerateArray()).Where(effect => effect.TryGetProperty("options", out _))
            .SelectMany(effect => effect.GetProperty("options").EnumerateArray()).Select(option => option.GetProperty("id").GetString()!)
            .Distinct().ToDictionary(id => id, id => id);
        var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [FixturePackage.SkillId] = new { name = "Lifecycle", description = "Fixture", optionLabels = labels } } });
        var program = SkillProgramCatalog.Load(rules, presentation).Programs[FixturePackage.SkillId];
        return ContentRegistry.Build(new StandardContentPackage(), new FixturePackage(program, observeDraw, happySima));
    }

    private static string Rules(
        string effects,
        string window = "turnStartBeforeNormalFlow",
        bool optional = false)
    {
        var occurrence = window == "afterDamageApplied" ? "\"damageOccurrence\":\"perDamagePoint\"," : "";
        return $$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{FixturePackage.SkillId}}","revision":1,
        "minimumRulesVersion": 171,"modifiers":[],"viewAs":[],"activations":[],
        "triggers":[{"id":"binding","window":"{{window}}","subject":"owner","optional":{{optional.ToString().ToLowerInvariant()}},
        {{occurrence}}"priority":0,"effects":[{{effects}}]}],"contributions":[],"cardIdentities":[]}]}
        """;
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class FixturePackage(SkillProgram program, bool observeDraw = false, bool happySima = false) : IGameContentPackage
    {
        public const string SkillId = "fixture:lifecycle";
        public const string OwnerGeneralId = "fixture:lifecycle-owner";
        public const string ModeId = "identity:lifecycle-program-test-4";
        private const string DeckId = "fixture:lifecycle-deck";
        internal const string Watch = "fixture:composition-draw-observer";

        public PackageManifest Manifest { get; } = new("lifecycle-program-test", new Version(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            if (happySima) CharacterFixtureContent.Register(builder, "happy:shen-sima-yi");
            builder.AddSkill(new ContentSkillDefinition(SkillId, "Lifecycle", "Fixture")
            {
                Program = program,
                ExecutionForms = SkillExecutionForm.Trigger
            });
            if (observeDraw)
            {
                var observer = SkillProgramCatalog.Load("""
                    {"schemaVersion":SCHEMA,"skills":[{"id":"WATCH","revision":1,"triggers":[
                      {"id":"paid-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch",
                       "movementReasons":["skill-program.fixture:lifecycle.Draw"],"optional":false,"effects":[
                         {"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                    """.Replace("SCHEMA", SkillProgramCatalog.RulesSchemaVersion.ToString(System.Globalization.CultureInfo.InvariantCulture))
                        .Replace("WATCH", Watch), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                        skills = new Dictionary<string, object> { [Watch] = new { name = "Paid child", description = "Paid draw observer",
                            optionLabels = new Dictionary<string, string> { ["continue"] = "Continue" } } } }));
                builder.AddSkill(new(Watch, "Paid child", "Paid draw observer") { Program = observer.Programs[Watch], ProgramPresentation = observer.Presentations[Watch] });
            }
            var generalIds = Enumerable.Range(0, 4).Select(index => index == 0
                ? OwnerGeneralId
                : $"fixture:lifecycle-owner-{index}").ToArray();
            foreach (var id in generalIds)
                builder.AddGeneral(new ContentGeneralDefinition(
                    id, "Lifecycle Owner", "supporter", happySima && id == OwnerGeneralId ? "happy:renjie" : SkillId, "wei", BaseHp: 4,
                    AdditionalSkillIds: happySima && id == OwnerGeneralId ? [SkillId, "happy:jilue"] : observeDraw ? [Watch] : null));
            builder.AddDeck(new ContentDeckRecipe(
                DeckId,
                "Lifecycle Deck",
                InitialHandSize: 4,
                DrawPerTurn: 2,
                Cards: [])
            {
                PhysicalCards = Enumerable.Range(0, 96)
                    .Select(index => new ContentDeckPhysicalCard(
                        happySima ? "standard:draw_two" : "standard:slash", (Suit)(index % 4), index % 13 + 1))
                    .ToArray()
            });
            builder.AddMode(new ContentModeDefinition(
                ModeId,
                "Lifecycle Program Test",
                4,
                4,
                new Dictionary<string, int>
                {
                    [nameof(Role.Lord)] = 1,
                    [nameof(Role.Loyalist)] = 1,
                    [nameof(Role.Rebel)] = 1,
                    [nameof(Role.Renegade)] = 1
                },
                DeckId,
                GeneralCandidateCount: 4,
                GeneralPoolIds: generalIds));
        }
    }
}
