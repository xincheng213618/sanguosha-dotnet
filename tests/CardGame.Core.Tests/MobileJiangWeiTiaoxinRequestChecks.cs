using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileJiangWeiTiaoxinRequestChecks
{
    private const string Tiaoxin = "boundary:tiaoxin-mobile";
    private const string Watch = "fixture:mobile-tiaoxin-watch";
    private const string Dodge = "fixture:mobile-tiaoxin-dodge";
    private const string Owner = "fixture:mobile-tiaoxin-owner";
    private const string Mode = "identity:classic-mobile-tiaoxin-fixture";
    private const string DiscardReason = "skill-program.boundary:tiaoxin-mobile.SelectAndMoveOwnedCard";

    public static void FixedOriginalTargetPaysNativeSlashAndReturnsCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 12
        }, registry);
        Submit(new StartGameCommand());
        Reach(p => p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });

        var farHand = Player(2).HandCount;
        Need(game.GetCombatDistance(2, 0) == 2 && game.GetLegalActions(0).Any(a =>
            a.ProgramSkillId == Tiaoxin && a.SelectableTargetSeats.Contains(2)), "The real taunt can choose the distant living seat.");
        Taunt(2);
        Reach(p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card"));
        var distant = Frame();
        Need(distant.SlashRequest is null && Facts<ProgramOptionChosenEvent>().Any(e => e.FrameId == distant.Id &&
            e.ResultBind == "tiaoxin-answer" && e.OptionId == "declined" && e.ChooserSeat == 2),
            "The distant actor's lack of range returns the decline to the original taunt before any Slash request is issued.");
        Answer(_ => true);
        Reach(p => p.SkillPrompt?.SkillId == Watch && p.PlayerSeat == 2);
        Need(Frame().Id == distant.Id && Frame().PendingMovementContinuation is not null &&
            Player(2).HandCount == farHand - 1 && game.CardMovements.Count(m =>
                m.From == CardLocation.Hand(2) && m.To == CardLocation.DiscardPile && m.Reason.Value == DiscardReason) == 1,
            "The unreachable reply moves one real card and keeps the original taunt under its movement window.");
        Cold();
        Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(p => p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        Need(game.GetLegalActions(0).All(a => a.ProgramSkillId != Tiaoxin), "The distant branch consumes this actual Play's taunt.");
        End(0);
        Reach(p => p is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
        End(1);
        Reach(p => p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });

        var hp = Player(0).Hp;
        var nearHand = Player(1).HandCount;
        Taunt(1);
        Reach(p => p.PlayerSeat == 1 && p.Choices.Any(IsPhysicalSlash));
        var original = Frame();
        Need(Prompt() is { IsPrivate: true, PlayerSeat: 1 } && Prompt()!.Choices.Where(IsPhysicalSlash)
            .All(c => c.Targets.SequenceEqual([0])) && Prompt()!.Choices.All(c => c.Parameters.GetValueOrDefault("request-option") != "target"),
            "The selected actor directly receives the private payment choices for the sole fixed recipient.");
        Need(original.SlashRequest is { Identity.ActorSeat: 1, Identity.InstructionIndex: 1, SelectedTargetSeat: 0, Acceptance: null } request &&
            request.Identity.FrameId == original.Id && request.Identity.Spec.CandidateSeats.SequenceEqual([0]) &&
            original.AssistedSlashRequest?.TargetSeat is null,
            $"The native request must be the sole owner of the fixed recipient before payment. Native={original.SlashRequest?.SelectedTargetSeat}, draft={original.AssistedSlashRequest?.TargetSeat}.");
        Cold();
        var material = Prompt()!.Choices.First(IsPhysicalSlash).Cards.Single();
        Answer(c => IsPhysicalSlash(c) && c.Cards.SequenceEqual([material]));
        Reach(p => p is { Kind: DecisionKind.RespondDodge, PlayerSeat: 0 });
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == material);
        Need(use.ProgramSlashRequestExecution is { Acceptance: { Identity.ActorSeat: 1, TargetSeat: 0 } accepted } execution &&
            execution.ProgramFrameId == original.Id && execution.UseFrameId == use.Id &&
            Frame().SlashRequest?.Acceptance == accepted && use.PhysicalCardIds?.SequenceEqual([material]) == true,
            "One native Use pays the selected physical Slash and carries the exact original accepted request.");
        Cold();
        Answer(c => c.Parameters.GetValueOrDefault("response") == "dodge" && c.Cards.Count == 1);
        Reach(p => p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        Need(Player(0).Hp == hp && Player(1).HandCount == nearHand - 1 &&
            Facts<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.SourceSeat == 1 && e.CardId == material) == 1 &&
            Facts<ProgramOptionChosenEvent>().Count(e => e.FrameId == original.Id && e.ResultBind == "tiaoxin-answer" &&
                e.OptionId == "used-slash" && e.ChooserSeat == 1) == 1 &&
            !game.CardMovements.Any(m => m.From == CardLocation.Hand(1) && m.Reason.Value == DiscardReason) &&
            game.GetLegalActions(0).All(a => a.ProgramSkillId != Tiaoxin),
            "The real Dodge leaves the original successful reply paid once and the new actual Play quota spent.");

        static bool IsPhysicalSlash(PromptChoice choice) => choice.Parameters.GetValueOrDefault("request-option") == "use" && choice.Cards.Count == 1;
        PendingDecision? Prompt() => Enumerable.Range(0, 5).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        IEnumerable<T> Facts<T>() => game.Events.Select(e => e.Payload).OfType<T>();
        ProgramSkillFrame Frame() => game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Tiaoxin);
        void Taunt(int seat) => Submit(new UseProgramSkillCommand(0, Tiaoxin, "taunt", [], [seat], game.Revision, Prompt()!.PromptId));
        void End(int seat) => Submit(new EndPlayPhaseCommand(seat, game.Revision, Prompt()!.PromptId));
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The fixture's public command was rejected.");
        }

        void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = Prompt()!;
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision));
        }

        void Reach(Func<PendingDecision, bool> predicate)
        {
            for (var step = 0; step < 320; step++)
            {
                var prompt = Prompt();
                if (prompt is not null && predicate(prompt)) return;
                if (prompt is { Kind: DecisionKind.SelectGeneral })
                    Submit(new SelectGeneralCommand(prompt.PlayerSeat, prompt.PlayerSeat == 0 ? Owner : Peer(prompt.PlayerSeat),
                        game.Revision, prompt.PromptId));
                else if (prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 }) End(prompt.PlayerSeat);
                else if (prompt is null) Submit(new AdvanceOneStepCommand(game.Revision));
                else throw new InvalidOperationException($"Unexpected taunt fixture prompt {prompt.Kind} for seat {prompt.PlayerSeat} ({prompt.SkillPrompt?.SkillId}).");
            }
            throw new InvalidOperationException("The native taunt fixture did not reach its next boundary.");
        }

        void Cold()
        {
            string State() => JsonSerializer.Serialize(new
            {
                Views = Enumerable.Range(0, 5).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))).ToArray(),
                Frames = game.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(),
                game.CardMovements
            });
            var before = State();
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Need(State() == before, "Cold replay preserves the current taunt prompt, original frames and physical movement.");
        }
    }

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static string Peer(int seat) => $"fixture:mobile-tiaoxin-peer-{seat}";

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-tiaoxin-request", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "boundary:jiang-wei-mobile", "boundary:zhuge-liang", "classic:zhuge-liang");
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Dodge}}","revision":1,"viewAs":[{"id":"physical-slash-as-dodge","inputKinds":["slash"],"inputSuits":[],"outputKind":"dodge","forPlay":false,"forResponse":true}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"actual-child","window":"cardsMoved","subject":"owner","optional":false,
                    "sourceZones":["hand","equipment"],"movementReasons":["{{DiscardReason}}"],"movementOccurrence":"perOwnerBatch",
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}
                ]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{
                  "{{Dodge}}":{"name":"固定实体闪","description":"用一张实体杀当闪响应。"},
                  "{{Watch}}":{"name":"弃牌观察","description":"在原实体移动后暂停。","optionLabels":{"continue":"继续"}}
                }}
                """);
            foreach (var (id, program) in catalog.Programs)
            {
                var presentation = catalog.Presentations[id];
                builder.AddSkill(new(id, presentation.Name, presentation.Description) { Program = program, ProgramPresentation = presentation });
            }

            builder.AddGeneral(new(Owner, "挑衅固定角色", "supporter", Tiaoxin, "shu", 4, ["boundary:zhiji-mobile", Dodge]));
            var peers = Enumerable.Range(1, 4).Select(Peer).ToArray();
            foreach (var peer in peers)
                builder.AddGeneral(new(peer, "实战应答角色", "supporter", "standard:none", "wu", 6, [Watch]));
            const string deck = "fixture:mobile-tiaoxin-deck";
            builder.AddDeck(new(deck, "固定实体杀", 4, 2, [])
                { PhysicalCards = Enumerable.Range(0, 160).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "挑衅固定场景", 5, 5, new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1
            }, deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
