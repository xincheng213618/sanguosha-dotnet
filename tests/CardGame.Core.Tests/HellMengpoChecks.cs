using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class HellMengpoChecks
{
    private const string Mode = "fixture:hell-mengpo-mode", Owner = "fixture:hell-mengpo-owner";
    private const string Driver = "fixture:hell-mengpo-driver", Quiet = "fixture:hell-mengpo-quiet";
    private const string Limited = "fixture:hell-mengpo-limited", Watch = "fixture:hell-mengpo-watch";
    private const string Curse = "fixture:hell-mengpo-curse", Mengpo = "fixture:hell-mengpo";

    public static void WanghunRestoresOnlyItsOwnSkillsAndDestroysSoulWithColdChildren()
    {
        var (g, registry) = Start("soul");
        var limitedBefore = Enumerable.Range(0, 3).Select(seat => Facts<SkillUsageConsumedEvent>(g)
            .Count(e => e.SkillOwnerSeat == seat && e.SkillId == Limited)).ToArray();
        Need(limitedBefore.All(count => count == 1), "Each original limited skill has consumed its real game usage before Wanghun.");
        Use(g, "kill", [], [3]); Play(g);
        var lost = Facts<HellWanghunGrantLostEvent>(g).ToArray();
        Need(lost.Length == 2 && lost.Select(e => e.TargetSeat).Distinct().Count() == 2 &&
            lost.All(e => e.Grant.SkillId == Limited && !g.GetHumanLegalActions().Any(a =>
                a.ProgramSkillId == "hell:return-soul-resolution")) &&
            Facts<HellReturnSoulInjectedEvent>(g).Select(e => e.CardId).Distinct().Count() == 2 &&
            Facts<HellReturnSoulInjectedEvent>(g).All(e => g.CardMovements.Count(m => m.CardId == e.CardId &&
                m.From == CardLocation.OutsideGame && m.To == CardLocation.DrawPile &&
                m.Reason.Value == "activity.hell.return-soul-injected") == 1),
            "A real Mengpo death selects two distinct enemies, excludes awakening-tagged control skills and injects exactly two additional registered physical soul cards.");
        Cold(g, registry);
        Use(g, "kill", [], [3]); Play(g);
        Need(!g.GetHumanLegalActions().Any(a => a.ProgramSkillId == Driver),
            "A separate real Duanchang death has independently disabled the source's remaining original skills.");
        Cold(g, registry);
        var first = ResolveSoul(g, registry, counterFirst: true);
        Need(Facts<HellSoulTargetResolvedEvent>(g).Count(e => e.CardUseFrameId == first) == 1 &&
            Facts<HellSoulGrantRestoredEvent>(g).Count(e => e.CardUseFrameId == first && e.Outcome == "restored") == 1,
            "One actual paid nullification excludes only its current recipient while the later soul target restores through its native child.");
        var second = ResolveSoul(g, registry, counterFirst: false);
        Need(Facts<HellSoulGrantRestoredEvent>(g).Count(e => e.Outcome == "restored") == 2 &&
            Facts<HellSoulGrantRestoredEvent>(g).Where(e => e.Outcome == "restored").All(e =>
                lost.Any(l => l.FrameId == e.LossFrameId && l.TargetSeat == e.TargetSeat && l.Grant.GrantId == e.GrantId)) &&
            !g.GetHumanLegalActions().Any(a => a.ProgramSkillId == Driver) &&
            Enumerable.Range(0, 3).All(seat => Facts<SkillUsageConsumedEvent>(g).Count(e =>
                e.SkillOwnerSeat == seat && e.SkillId == Limited) == limitedBefore[seat]) &&
            Facts<HellReturnSoulDestroyedEvent>(g).Count(e => g.Events.Select(x => x.Payload).OfType<CardUseFinishedEvent>()
                .Any(use => (use.ResolutionId == first || use.ResolutionId == second) && use.CardId == e.CardId)) == 2,
            "Soul restores only the exact Wanghun original grants, retains every limited-use ledger and other Duanchang loss, and destroys each real used soul once.");
        Cold(g, registry);
    }

    private static long ResolveSoul(GameEngine g, ContentRegistry registry, bool counterFirst)
    {
        var action = g.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.ReturnSoul);
        Accept(g, new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, g.Revision, P(g)!.PromptId));
        Until(g, () => P(g) is { Kind: DecisionKind.Nullification, PlayerSeat: 0 });
        var use = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardKind == CardKind.ReturnSoul);
        var targetCount = use.TargetSeats.Count;
        Need(targetCount == (counterFirst ? 2 : 1) &&
            g.CreateCardZoneDiagnostics().Single(c => c.CardId == action.CardId).Location == CardLocation.Processing,
            "The real physical soul remains in Processing while the native sequential target cursor owns each nullification.");
        Cold(g, registry);
        var countered = false; var skillChildren = 0;
        for (var i = 0; i < 160 && !IsPlay(g); i++)
        {
            var prompt = P(g);
            if (prompt is { Kind: DecisionKind.Nullification, PlayerSeat: 0 })
            {
                var pay = counterFirst && !countered;
                Answer(g, c => c.Parameters.GetValueOrDefault("response") == (pay ? "nullification" : "pass"));
                countered |= pay;
            }
            else if (prompt is { SkillPrompt.SkillId: Watch } &&
                g.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.HellSoulReturn?.CardUseFrameId == use.Id))
            {
                skillChildren++;
                Cold(g, registry);
                if (prompt.PlayerSeat == 0) Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
                else Accept(g, new AdvanceOneStepCommand(g.Revision));
            }
            else Step(g);
        }
        var expectsSkillChild = use.TargetSeats.Skip(counterFirst ? 1 : 0).Any(seat => seat != 0);
        Need(IsPlay(g) && countered == counterFirst && (skillChildren > 0) == expectsSkillChild &&
            Facts<CardUseFinishedEvent>(g).Count(e => e.ResolutionId == use.Id) == 1 &&
            Facts<HellReturnSoulDestroyedEvent>(g).Count(e => e.CardId == action.CardId) == 1 &&
            g.CreateCardZoneDiagnostics().Single(c => c.CardId == action.CardId).Location == CardLocation.OutsideGame &&
            !g.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.HellSoulReturn?.CardUseFrameId == use.Id),
            "The native restoration drains its actual SkillsChanged child, returns once, retires its private internal grant and destroys the used entity before it can be recycled.");
        Cold(g, registry); return use.Id;
    }
    private static (GameEngine, ContentRegistry) Start(string scene)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(scene));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = "team:allies", ModeId = Mode, UseInteractiveSetup = true,
            UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 16 }, registry);
        Accept(game, new StartGameCommand()); Until(game, () => P(game)?.Kind == DecisionKind.SelectGeneral);
        Accept(game, new SelectGeneralCommand(0, Owner, game.Revision, P(game)!.PromptId)); Play(game);
        return (game, registry);
    }
    private sealed class Fixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-hell-mengpo", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var assembly = typeof(StandardContentPackage).Assembly;
            using var rules = new StreamReader(assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.hell-mengpo.rules.json")!);
            using var presentation = new StreamReader(assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.hell-mengpo.presentation.json")!);
            var actual = SkillProgramCatalog.Load(rules.ReadToEnd(), presentation.ReadToEnd());
            foreach (var (id, program) in actual.Programs) b.AddSkill(new(id, actual.Presentations[id].Name, actual.Presentations[id].Description)
            { Program = program, ProgramPresentation = actual.Presentations[id], Tags = id is "hell:wanghun" or "hell:wangshi" ? SkillTag.Locked : SkillTag.None });
            b.AddCard(new("fixture:return-soul", "回魂", "锦囊牌", "Real fixture soul entity.", CardKind.ReturnSoul));
            var blockedSoul = scene == "soul-blocked";
            if (blockedSoul) b.AddCard(new("fixture:robe", "国风玉袍", "装备牌", "Native ordinary-trick target prohibition.", CardKind.NationalStyleRobe));
            var definitions = new List<Dictionary<string, object>>();
            void Add(string id, string property, object value) => definitions.Add(new() { ["id"] = id, ["revision"] = 1, [property] = value });
            object Activation(string id, object[] effects, int cards = 0, int targets = 0) => new
            { id, minCards = cards, maxCards = cards, minTargets = targets, maxTargets = targets,
                targetKind = "anyLiving", sourceZones = new[] { "hand" }, usesPerTurn = (int?)null, effects };
            Add(Driver, "activations", new[]
            {
                Activation("kill", [new { op = "damage", target = "selectedTarget", amount = 20 }], targets: 1),
                Activation("discard-one", [new { op = "discardSelected", target = "owner", amount = 1 }], cards: 1)
            });
            definitions[0]["modifiers"] = scene.StartsWith("shiyou", StringComparison.Ordinal) ? Array.Empty<object>() : new object[]
            { new { id = "all-original-test-entities", query = "initialHandSize", operation = "add", value = 48, priority = 0 },
                new { id = "retain-materials", query = "handLimit", operation = "add", value = 100, priority = 0 } };
            if (scene is "shiyou-program" or "shiyou-mixed" or "shiyou-all") definitions[0]["triggers"] = new object[]
            {
                new { id = "real-discard-phase-payment", window = "discardPhaseStarting", subject = "owner", optional = false,
                    effects = scene == "shiyou-all" ? new object[] {
                        new { op = "discardOwnedZoneCards", target = "owner", zones = new[] { "hand" } } } : new object[] {
                        new { op = "selectOwnedCards", target = "owner", minimumCards = 2, maximumCards = 2,
                            zones = scene == "shiyou-mixed" ? new[] { "hand", "equipment" } : new[] { "hand" }, resultBind = "paid" },
                        new { op = "moveBoundCards", target = "owner", sourceBind = "paid", destination = "discardPile", awaitMovementTriggers = true } } }
            };
            Add(Quiet, "modifiers", new[] { new { id = "quiet-retention", query = "handLimit", operation = "add", value = 100, priority = 0 } });
            if (blockedSoul)
            {
                definitions[0]["activations"] = ((object[])definitions[0]["activations"]).Append(new
                { id = "all-hand", minCards = 1, maxCards = 64, minTargets = 0, maxTargets = 0,
                    targetKind = "anyLiving", sourceZones = new[] { "hand" }, usesPerTurn = (int?)null,
                    effects = new[] { new { op = "useAllHandCardsAsOrdinaryTrick", target = "owner", viewAsId = "all-hand" } } }).ToArray();
                definitions[^1]["triggers"] = new[] { new { id = "real-deck-robe", window = "gameStarting", subject = "owner", optional = false,
                    effects = new[] { new { op = "useRandomDeckEquipment", target = "owner", targetRef = new { kind = "owner" }, resultBind = "robe" } } } };
            }
            Add(Limited, "triggers", new[] { new { id = "initial-limited-payment", window = "gameStarting", subject = "owner",
                optional = false, usageScope = "game", usageLimit = 1,
                effects = new[] { new { op = "setBooleanState", target = "owner", stateId = "paid", value = true } } } });
            definitions[^1]["states"] = new[] { new { id = "paid", initialValue = false, visibility = "private", resetScope = "game", reacquirePolicy = "preserveUntilGameEnd" } };
            Add(Watch, "triggers", new[] { new { id = "actual-skills-changed", window = "skillsChanged", subject = "owner", optional = false,
                effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "continued", options = new[] { new { id = "continue" } } } } } });
            Add(Curse, "triggers", new[] { new { id = "separate-real-duanchang", window = "ownerDied", subject = "owner", optional = false,
                effects = new[] { new { op = "skillState", target = "owner", targetRef = new { kind = "eventSource" }, action = "lose", scope = "all", duration = "permanent", characterOnly = true } } } });
            foreach (var id in new[] { "hell:player-tier", "hell:boss-tier" })
                Add(id, "modifiers", new[] { new { id = "internal-tier-host", query = "handLimit", operation = "add", value = 1, priority = 0 } });
            var fixture = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = definitions.ToDictionary(d => (string)d["id"], d => new { name = (string)d["id"], description = "Real native Mengpo fixture.",
                        optionLabels = (string)d["id"] == Watch ? new Dictionary<string, string> { ["continue"] = "Continue" } : [] }) }));
            foreach (var (id, program) in fixture.Programs) b.AddSkill(new(id, id, "Native Mengpo control fixture.")
            { Program = program, ProgramPresentation = fixture.Presentations[id],
                Tags = id == Limited ? SkillTag.Limited : id is Driver or Quiet or Watch ? SkillTag.Awakening : SkillTag.None });
            var shiyou = scene.StartsWith("shiyou", StringComparison.Ordinal);
            b.AddGeneral(new(Owner, "Original human", "", Driver, "wei", shiyou ? 2 : 20,
                scene == "soul" ? [Limited, Watch, "hell:player-tier", "hell:boss-tier", "hell:return-soul-resolution"] : []));
            string[] peers = ["fixture:hell-mengpo-peer-1", "fixture:hell-mengpo-peer-2"];
            foreach (var peer in peers) b.AddGeneral(new(peer, peer, "", Quiet, "wei", 20,
                scene == "soul" ? [Limited, Watch, "hell:player-tier", "hell:boss-tier", "hell:return-soul-resolution"] :
                    blockedSoul && peer == peers[0] ? ["hell:wanghun"] : []));
            b.AddGeneral(new(Mengpo, "孟婆", "", blockedSoul ? Quiet : scene == "soul" ? "hell:wanghun" : scene == "wangshi" ? "hell:wangshi" : "hell:shiyou", "qun", 3,
                blockedSoul ? [Limited] : []));
            const string next = "fixture:hell-mengpo-next", last = "fixture:hell-mengpo-last";
            b.AddGeneral(new(next, "Next real enemy", "", scene == "soul" ? Curse : Quiet, "qun", scene == "soul" ? 3 : 20));
            b.AddGeneral(new(last, "Last quiet enemy", "", Quiet, "qun", 20));
            var deckCards = shiyou ? Enumerable.Range(0, 96).Select(i => new ContentDeckPhysicalCard(
                    scene == "shiyou-mixed" ? "standard:crossbow" : "standard:dodge",
                    scene is "shiyou-mixed" or "shiyou-all" ? Suit.Heart : (Suit)(i % 4), 7)).ToArray() :
                Enumerable.Range(0, blockedSoul ? 128 : 48).Select(i => new ContentDeckPhysicalCard(blockedSoul ?
                    i < 48 ? "fixture:return-soul" : "fixture:robe" : scene == "soul" ?
                    i % 2 == 0 ? "fixture:return-soul" : "standard:nullification" :
                    new[] { "standard:slash", "standard:dodge", "standard:nullification", "standard:crossbow", "standard:draw_two", "standard:indulgence" }[i % 6],
                    (Suit)(i % 4), 7)).ToArray();
            b.AddDeck(new("fixture:hell-mengpo-deck", "Real native materials", shiyou ? 6 : 0, 0, []) { PhysicalCards = deckCards });
            BossActivityStage[] stages = scene == "soul" ? [new("Mengpo", [Mengpo], 0), new("Independent curse", [next], 0), new("Quiet continuation", [last], 0)] :
                [new("Mengpo", [Mengpo], 0), new("Quiet continuation", [next], 0)];
            b.AddMode(new(Mode, "Isolated real Mengpo mechanics", 4, 4, new Dictionary<string, int>(), "fixture:hell-mengpo-deck", 3,
                [Owner, .. peers, .. stages.SelectMany(stage => stage.GeneralPoolIds)], ContentModeKind.Team,
                new Dictionary<string, int> { ["team:allies"] = 3, ["team:hell"] = 1 })
            { BossActivity = new([Owner, .. peers], stages, 1, [], EnemyTeamId: "team:hell") });
        }
    }
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 4).Select(seat => g.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
    private static IEnumerable<T> Facts<T>(GameEngine g) => g.Events.Select(e => e.Payload).OfType<T>();
    private static bool IsPlay(GameEngine g) => P(g) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };
    private static void Play(GameEngine g) => Until(g, () => IsPlay(g));
    private static void Use(GameEngine g, string id, IReadOnlyList<int> cards, IReadOnlyList<int> targets) =>
        Accept(g, new UseProgramSkillCommand(0, Driver, id, cards, targets, g.Revision, P(g)!.PromptId));
    private static void Answer(GameEngine g, Func<PromptChoice, bool> choose)
    { var p = P(g)!; Accept(g, new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(choose).Id, g.Revision)); }
    private static void Step(GameEngine g)
    {
        if (P(g) is { PlayerSeat: 0 } prompt)
        {
            if (prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "pass"))
            { Answer(g, c => c.Parameters.GetValueOrDefault("response") == "pass"); return; }
            if (prompt.SkillPrompt?.SkillId == Watch)
            { Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue"); return; }
        }
        Accept(g, new AdvanceOneStepCommand(g.Revision));
    }
    private static void Until(GameEngine g, Func<bool> ready)
    {
        for (var i = 0; i < 160; i++) { if (ready()) return; Step(g); }
        throw new InvalidOperationException($"Mengpo boundary missing: {g.State.Status}/{g.State.Phase}; prompt={P(g)?.Kind}/{P(g)?.SkillPrompt?.SkillId}; " +
            string.Join(',', g.ResolutionStack.Select(f => f.GetType().Name + ':' + f.Id)));
    }
    private static void Accept(GameEngine g, GameCommand command)
    { var result = g.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Native Mengpo command rejected."); }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static string State(GameEngine g) => JsonSerializer.Serialize(new
    { Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(g.CreateSnapshot(seat))),
        Frames = g.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), g.CardMovements,
        Facts = g.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
    private static void Cold(GameEngine g, ContentRegistry registry) => Need(State(g) == State(GameReplay.Restore(
        GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry)),
        "Cold replay preserves exact interception, original lost grants, soul materials and native child returns.");
}
