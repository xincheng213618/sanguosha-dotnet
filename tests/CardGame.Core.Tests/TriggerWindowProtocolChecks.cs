using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class TriggerWindowProtocolChecks
{
    private const string Skill = "fixture:window-protocol";
    private const string PaymentReason = "skill-program." + Skill + ".MoveBoundCards";

    public static void OrderedSkipAndTypedReturnsPreserveBoundaries()
    {
        CheckWindow(playStarting: false);
        CheckWindow(playStarting: true);
    }

    private static void CheckWindow(bool playStarting)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(),
            new Fixture(playStarting ? "playPhaseStarting" : "turnStartBeforeNormalFlow"));
        foreach (var skipGroup in new[] { true, false })
        {
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord,
                ModeId = "identity:window-protocol", UseInteractiveSetup = true,
                AdvanceAfterHumanCommands = false, MaxTurns = 2
            }, registry);
            Accept(game, new StartGameCommand());
            Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral);
            Accept(game, new SelectGeneralCommand(0, "fixture:window-owner", game.Revision, Pending(game)!.PromptId));
            Reach(game, prompt => IsActivation(prompt, "early"));
            var initialHand = game.CreateSnapshot(0).Players[0].HandCount;
            var initialHp = game.CreateSnapshot(0).Players[0].Hp;
            var early = Pending(game)!;
            Answer(game, choice => Action(choice) == "skip");
            Reach(game, prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("choice-group") == "opening"));
            var parent = game.ResolutionStack.Single(frame => playStarting
                ? frame is PlayPhaseStartingBoundaryFrame
                : frame is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow });
            IEnumerable<ProgramTriggerCandidate> candidates = parent switch
            {
                PlayPhaseStartingBoundaryFrame starting => starting.Items.Select(item => item.Candidate!),
                ProgramLifecycleTriggerWindowFrame lifecycle => lifecycle.Candidates,
                _ => throw new InvalidOperationException("The protocol fixture lost its typed boundary.")
            };
            Require(Cursor(game, parent.Id) == 1 && candidates.Select(candidate => candidate.BindingId)
                    .SequenceEqual(["early", "group-a", "group-paid", "late"]),
                "Skipping the first candidate must advance exactly once and preserve the frozen priority/group order.");
            var beforeStale = State(game);
            Require(!game.Submit(new AnswerPromptCommand(0, early.PromptId, early.Choices[0].Id, game.Revision)).Accepted &&
                    State(game) == beforeStale,
                "A retired optional prompt cannot answer the next group or change its cursor.");
            VerifyPause();

            if (skipGroup)
            {
                Answer(game, choice => Action(choice) == "skip");
                Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
                Require(Resolved(game).Select(item => item.BindingId)
                        .SequenceEqual(["early", "group-a", "group-paid", "late"]) &&
                        Resolved(game).Take(3).All(item => !item.Activated && !item.Completed) &&
                        Resolved(game).Last() is { Activated: true, Completed: true } &&
                        Started(game).Select(item => item.BindingId).SequenceEqual(["late"]) &&
                        game.CreateSnapshot(0).Players[0].HandCount == initialHand + 1 &&
                        !game.CardMovements.Any(move => move.Reason.Value == PaymentReason),
                    "Group skip must resolve both original branches once, pay nothing and continue after the entire group.");
                VerifyPause();
                continue;
            }

            Answer(game, choice => Action(choice) == "activate" &&
                choice.Parameters.GetValueOrDefault("binding-id") == "group-paid");
            Reach(game, prompt => prompt.Choices.Any(choice => Action(choice) == "select-owned-cards"));
            var selected = Pending(game)!.Choices.First(choice => Action(choice) == "select-owned-cards");
            var paidCard = selected.Cards.Single();
            var paidFrame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.TriggerId == "group-paid");
            Require(paidFrame.WindowContext is { ResumeCandidateIndex: 3 } context && context.ParentFrameId == parent.Id &&
                    context.Window == (playStarting ? SkillProgramTriggerWindow.PlayPhaseStarting : SkillProgramTriggerWindow.TurnStartBeforeNormalFlow),
                "The selected branch must retain the exact group return cursor and owning typed boundary.");
            VerifyPause();
            Answer(game, choice => choice.Id == selected.Id);
            Reach(game, prompt => IsOption(prompt, "movement-continue"));
            Require(Payments(game, paidCard) == 1 && game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any() &&
                    game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == paidFrame.Id) &&
                    Cursor(game, parent.Id) == 1 &&
                    !Resolved(game).Any(item => item.BindingId is "group-paid" or "late"),
                "The paid movement child must suspend the original branch and group cursor without completing either parent.");
            VerifyPause();
            var frozenPayment = State(game);
            var movementPrompt = Pending(game)!;
            Require(!game.Submit(new AnswerPromptCommand(0, movementPrompt.PromptId, new ChoiceId("unpublished"), game.Revision)).Accepted &&
                    State(game) == frozenPayment && Payments(game, paidCard) == 1,
                "A rejected child choice must not replay the already committed physical payment.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "movement-continue");
            Reach(game, prompt => IsOption(prompt, "hp-continue"));
            Require(Payments(game, paidCard) == 1 && game.CreateSnapshot(0).Players[0].Hp == initialHp - 1 &&
                    game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Any() &&
                    game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == paidFrame.Id) &&
                    Cursor(game, parent.Id) == 1 &&
                    !Resolved(game).Any(item => item.BindingId is "group-paid" or "late"),
                "Returning from movement must resume at the next instruction and suspend again on exactly one real HP-change child.");
            VerifyPause();
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "hp-continue");
            Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
            // Turn-start retains its historical one-position return. Play-start
            // uses the selected child's resume cursor to retire the entire group.
            string[] expectedResolved = playStarting
                ? ["early", "group-a", "payment-seen", "hp-seen", "group-paid", "late"]
                : ["early", "group-a", "payment-seen", "hp-seen", "group-paid", "group-paid", "late"];
            string[] expectedSkipped = playStarting ? ["early", "group-a"] : ["early", "group-a", "group-paid"];
            Require(Resolved(game).Select(item => item.BindingId)
                    .SequenceEqual(expectedResolved) &&
                    Resolved(game).Where(item => item.Activated).All(item => item.Completed) &&
                    Resolved(game).Where(item => !item.Activated).Select(item => item.BindingId)
                        .SequenceEqual(expectedSkipped) &&
                    Resolved(game).Where(item => !item.Activated).All(item => !item.Completed) &&
                    Started(game).Select(item => item.BindingId)
                        .SequenceEqual(["group-paid", "payment-seen", "hp-seen", "late"]) &&
                    Resolved(game).Single(item => item.BindingId == "group-paid" && item.Activated).FrameId == paidFrame.Id &&
                    (playStarting
                        ? !Resolved(game).Any(item => item.BindingId == "group-paid" && !item.Activated)
                        : Resolved(game).Single(item => item.BindingId == "group-paid" && !item.Activated).FrameId == parent.Id) &&
                    Payments(game, paidCard) == 1 && game.CreateSnapshot(0).Players[0].HandCount == initialHand + 1 &&
                    game.CreateSnapshot(0).Players[0].Hp == initialHp - 1 &&
                    !game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == Skill) &&
                    !game.ResolutionStack.Any(frame => frame.Id == parent.Id),
                "Nested returns must complete each activated child once, preserve the typed boundary's group return cursor and continue without repeating cost or HP loss. " +
                JsonSerializer.Serialize(new
                {
                    InitialHand = initialHand, Hand = game.CreateSnapshot(0).Players[0].HandCount,
                    InitialHp = initialHp, Hp = game.CreateSnapshot(0).Players[0].Hp,
                    PaymentCount = Payments(game, paidCard), PaidCard = paidCard, ParentId = parent.Id, PaidFrameId = paidFrame.Id,
                    Resolved = Resolved(game), Started = Started(game),
                    Frames = game.ResolutionStack.Select(frame => new { frame.Id, frame.Kind,
                        Binding = (frame as ProgramSkillFrame)?.TriggerId })
                }));
            VerifyPause();

            void VerifyPause()
            {
                var restored = Replay(game, registry);
                if (playStarting) game = restored;
            }
        }
    }

    private static int Cursor(GameEngine game, long parentId) =>
        game.ResolutionStack.Single(frame => frame.Id == parentId) switch
        {
            PlayPhaseStartingBoundaryFrame starting => starting.ItemIndex,
            ProgramLifecycleTriggerWindowFrame lifecycle => lifecycle.CandidateIndex,
            _ => throw new InvalidOperationException("The protocol cursor belongs to another frame kind.")
        };

    private static IReadOnlyList<ProgramBindingResolvedEvent> Resolved(GameEngine game) => game.Events
        .Select(item => item.Payload).OfType<ProgramBindingResolvedEvent>().Where(item => item.SkillId == Skill).ToArray();
    private static IReadOnlyList<ProgramBindingStartedEvent> Started(GameEngine game) => game.Events
        .Select(item => item.Payload).OfType<ProgramBindingStartedEvent>().Where(item => item.SkillId == Skill).ToArray();
    private static int Payments(GameEngine game, int card) => game.CardMovements.Count(move => move.CardId == card &&
        move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile && move.Reason.Value == PaymentReason);
    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
    private static bool IsActivation(PendingDecision prompt, string binding) => prompt.Choices.Any(choice =>
        Action(choice) == "activate" && choice.Parameters.GetValueOrDefault("binding-id") == binding);
    private static bool IsOption(PendingDecision prompt, string option) => prompt.Choices.Any(choice =>
        choice.Parameters.GetValueOrDefault("option-id") == option);
    private static PendingDecision? Pending(GameEngine game) => game.PendingDecision ?? game.CreateSnapshot(0).PendingDecision;
    private static void Reach(GameEngine game, Func<PendingDecision, bool> predicate)
    {
        for (var step = 0; step < 80; step++)
        {
            if (Pending(game) is { } prompt)
            {
                if (predicate(prompt)) return;
                throw new InvalidOperationException("Unexpected protocol prompt: " + prompt.Kind + " / " + prompt.Prompt);
            }
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The protocol fixture did not reach its next command boundary.");
    }
    private static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    {
        var prompt = Pending(game)!;
        Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision));
    }
    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "The protocol fixture rejected its command.");
    }
    private static string State(GameEngine game) => JsonSerializer.Serialize(new
    {
        Views = Enumerable.Range(0, 4).Append(-1).Select(viewer => SnapshotJson.Serialize(game.CreateSnapshot(viewer))).ToArray(),
        Frames = JsonSerializer.Serialize(game.ResolutionStack),
        Events = game.Events.Select(item => JsonSerializer.Serialize(item.Payload, item.Payload.GetType())).ToArray(),
        game.CardMovements, Commands = CommandJson.Serialize(game.AcceptedCommands), Zones = game.CreateCardZoneDiagnostics()
    });
    private static GameEngine Replay(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(
            GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(State(game) == State(restored),
            "Every actual player and spectator view, exact frames, paid movements and committed prefix must survive a cold restore.");
        return restored;
    }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture(string boundaryWindow) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("window-protocol-fixture", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Skill}}","revision":1,"triggers":[
                {"id":"early","window":"{{boundaryWindow}}","subject":"owner","optional":true,"priority":30,"effects":[{"op":"draw","target":"owner","amount":1}]},
                {"id":"group-a","window":"{{boundaryWindow}}","subject":"owner","optional":true,"priority":20,"choiceGroup":"opening","usageScope":"game","usageLimit":1,"effects":[{"op":"draw","target":"owner","amount":3}]},
                {"id":"group-paid","window":"{{boundaryWindow}}","subject":"owner","optional":true,"priority":20,"choiceGroup":"opening","usageScope":"game","usageLimit":1,"effects":[
                {"op":"selectOwnedCards","target":"owner","amount":1,"zones":["hand"],"resultBind":"payment"},
                {"op":"moveBoundCards","target":"owner","sourceBind":"payment","destination":"discardPile","awaitMovementTriggers":true},
                {"op":"loseHp","target":"owner","amount":1},{"op":"draw","target":"owner","amount":1}]},
                {"id":"late","window":"{{boundaryWindow}}","subject":"owner","optional":false,"priority":10,"effects":[{"op":"draw","target":"owner","amount":1}]},
                {"id":"payment-seen","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{PaymentReason}}"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"movement-return","options":[{"id":"movement-continue"}]}]},
                {"id":"hp-seen","window":"afterHpLost","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"hp-return","options":[{"id":"hp-continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Skill] = new { name = "Window protocol", description = "Exact command boundaries",
                        triggerChoices = new Dictionary<string, string> { ["group-a"] = "Draw branch", ["group-paid"] = "Paid branch" },
                        optionLabels = new Dictionary<string, string> { ["movement-continue"] = "Return movement", ["hp-continue"] = "Return HP change" } } } }));
            builder.AddSkill(new(Skill, "Window protocol", "Exact command boundaries") { Program = catalog.Programs[Skill] });
            var generals = new[] { "fixture:window-owner", "fixture:window-peer-1", "fixture:window-peer-2", "fixture:window-peer-3" };
            foreach (var id in generals) builder.AddGeneral(new(id, id, "supporter", id == generals[0] ? Skill : "standard:none", "wei"));
            builder.AddDeck(new("fixture:window-deck", "Window deck", 4, 0, [])
            { PhysicalCards = Enumerable.Range(0, 48).Select(index => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, index % 13 + 1)).ToArray() });
            builder.AddMode(new("identity:window-protocol", "Window protocol", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                "fixture:window-deck", GeneralCandidateCount: 4, GeneralPoolIds: generals));
        }
    }
}
