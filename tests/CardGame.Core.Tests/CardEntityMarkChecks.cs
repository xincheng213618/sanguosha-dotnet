using System.Text.Json;
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
            if (terminal is not null)
            {
                var definition = System.Text.Json.Nodes.JsonNode.Parse(rules)!;
                definition["skills"]![0]!["triggers"]![0]!["effects"]!.AsArray().Add(System.Text.Json.Nodes.JsonNode.Parse(
                    terminal == "lost"
                        ? $$$"""{"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{{Skill}}}"]}}"""
                        : """{"op":"loseHp","target":"owner","amount":10}"""));
                rules = definition.ToJsonString();
            }
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Skill] = new { name = "Physical marks", description = "Shared physical mark fixture",
                    cardMarks = new Dictionary<string, string> { ["opening"] = "开局", ["chosen"] = "所选" } } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Skill, "Physical marks", "Shared physical mark fixture") { Program = catalog.Programs[Skill], ProgramPresentation = catalog.Presentations[Skill] });
            builder.AddSkill(new("fixture:card-mark-noop", "Peer", "No active rules"));
            builder.AddGeneral(new(Owner, "Mark owner", "", Skill, "wei", 4));
            builder.AddGeneral(new(Peer, "Mark peer", "", "fixture:card-mark-noop", "wei", 8));
            builder.AddDeck(new(Deck, "Fixed physical cards", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 60)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Shared card marks", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
