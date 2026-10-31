using CardGame.Content.Standard;
using CardGame.Core;
using System.Reflection;

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
            _ = SkillProgramCatalog.Load(rules, Presentation);
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

    private static ContentRegistry Registry(string rules)
    {
        var program = SkillProgramCatalog.Load(rules, Presentation).Programs[FixturePackage.SkillId];
        return ContentRegistry.Build(new StandardContentPackage(), new FixturePackage(program));
    }

    private static string Rules(
        string effects,
        string window = "turnStartBeforeNormalFlow",
        bool optional = false)
    {
        var occurrence = window == "afterDamageApplied" ? "\"damageOccurrence\":\"perDamagePoint\"," : "";
        return $$"""
        {"schemaVersion":62,"skills":[{"id":"{{FixturePackage.SkillId}}","revision":1,
        "minimumRulesVersion": 171,"modifiers":[],"viewAs":[],"activations":[],
        "triggers":[{"id":"binding","window":"{{window}}","subject":"owner","optional":{{optional.ToString().ToLowerInvariant()}},
        {{occurrence}}"priority":0,"effects":[{{effects}}]}],"contributions":[],"cardIdentities":[]}]}
        """;
    }

    private const string Presentation =
        "{\"schemaVersion\":3,\"skills\":{\"fixture:lifecycle\":{\"name\":\"Lifecycle\",\"description\":\"Fixture\"}}}";

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class FixturePackage(SkillProgram program) : IGameContentPackage
    {
        public const string SkillId = "fixture:lifecycle";
        public const string OwnerGeneralId = "fixture:lifecycle-owner";
        public const string ModeId = "identity:lifecycle-program-test-4";
        private const string DeckId = "fixture:lifecycle-deck";

        public PackageManifest Manifest { get; } = new("lifecycle-program-test", new Version(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddSkill(new ContentSkillDefinition(SkillId, "Lifecycle", "Fixture")
            {
                Program = program,
                ExecutionForms = SkillExecutionForm.Trigger
            });
            var generalIds = Enumerable.Range(0, 4).Select(index => index == 0
                ? OwnerGeneralId
                : $"fixture:lifecycle-owner-{index}").ToArray();
            foreach (var id in generalIds)
                builder.AddGeneral(new ContentGeneralDefinition(
                    id, "Lifecycle Owner", "supporter", SkillId, "wei", BaseHp: 4));
            builder.AddDeck(new ContentDeckRecipe(
                DeckId,
                "Lifecycle Deck",
                InitialHandSize: 4,
                DrawPerTurn: 2,
                Cards: [])
            {
                PhysicalCards = Enumerable.Range(0, 96)
                    .Select(index => new ContentDeckPhysicalCard(
                        "standard:slash", (Suit)(index % 4), index % 13 + 1))
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
