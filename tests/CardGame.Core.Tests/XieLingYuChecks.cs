using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class XieLingYuChecks
{
    private const string Owner = "fixture:xly-owner", Mode = "identity:classic-xie-ling-yu",
        Driver = "fixture:xly-driver", Quiet = "fixture:xly-quiet", Watch = "fixture:xly-watch",
        Conditional = "fixture:xly-conditional", Yuandi = "ol:yuandi", Xinyou = "ol:xinyou",
        Zenrun = "ol:zenrun", Obtained = "fixture:xly-obtained", Assault = "fixture:xly-assault";
    private const string YDraw = "skill-program.xie-ling-yu.yuandi.draw",
        XDraw = "skill-program.xie-ling-yu.xinyou.draw", XCost = "skill-program.xie-ling-yu.xinyou.discard";
    private static string Scene = "";
    private static GameEngine? DiagnosticContext;

    public static void XinyouPaidChildrenReplacementSourceLossDeathAndInputPrivacyCold()
    {
        foreach (var scene in new[] { "paid-child", "paid-loss", "paid-death" })
        {
            var (g, r) = Start(scene); Stock(g, "stock-six"); var cost = Hand(g, CardKind.Dodge);
            BeginXinyou(g); Cost(g, cost); Pay(g); Reach(g, () => Option(g, Watch, "paid"));
            var paid = E<XinyouPaidEvent>(g).Single(); var frame = Frame(g, Xinyou);
            Need(frame.XieLingYu is { Paid: true, Stage: XieLingYuStage.CostChildren, Requests: 0 } receipt &&
                receipt.Materials is System.Collections.IList { IsReadOnly: true } && receipt.Materials.Single().CardId == cost &&
                CostMoves(g, paid.FrameId).Length == 1 && E<XieLingYuDrawReturnedEvent>(g).All(e => e.FrameId != paid.FrameId),
                "The original atomic Basic discard owns its frozen receipt before a real movement child and before any Draw1 request.");
            AssertPrivate(g, 0); g = Cold(g, r); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "paid");
            if (scene == "paid-child")
            {
                for (var request = 1; request <= 5; request++)
                {
                    Reach(g, () => Option(g, Watch, "gained"));
                    var returned = E<XieLingYuDrawReturnedEvent>(g).Where(e => e.FrameId == paid.FrameId).ToArray();
                    Need(returned.Length == request && returned.Last().Request == request &&
                        E<XinyouGainedEntityEvent>(g).Count(e => e.FrameId == paid.FrameId) == request &&
                        E<XieLingYuCompletedEvent>(g).All(e => e.FrameId != paid.FrameId) && CostMoves(g, paid.FrameId).Length == 1,
                        "Each real gain child finishes before another request or completion; the paid initial cost is never repeated.");
                    AssertPrivate(g, 0); g = Cold(g, r); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "gained");
                }
                Play(g);
            }
            else Reach(g, () => E<XieLingYuCompletedEvent>(g).Any(e => e.FrameId == paid.FrameId));
            var complete = E<XieLingYuCompletedEvent>(g).Single(e => e.FrameId == paid.FrameId);
            Need(complete.Paid && CostMoves(g, paid.FrameId).Length == 1 && !g.ResolutionStack.Any(f => f.Id == paid.FrameId),
                "The exact paid parent drains its native descendants and returns once, even across cold recovery.");
            if (scene == "paid-loss") Need(V(g).IsAlive && !Has(g, Xinyou) && complete.Requests == 5 &&
                E<XinyouGainedEntityEvent>(g).Count(e => e.FrameId == paid.FrameId) == 5,
                "Genuine conditional source loss does not revoke the already-paid frozen loop.");
            if (scene == "paid-death") Need(!V(g).IsAlive && complete.Requests == 0 &&
                E<XinyouGainedEntityEvent>(g).All(e => e.FrameId != paid.FrameId) && Zone(g, cost) == CardLocation.DiscardPile,
                "Real cost-child death keeps its one payment but starts no draw or fresh private choice for the deceased owner.");
            _ = Cold(g, r);
        }
        {
            var (g, r) = Start("replacement"); Stock(g);
            var given = new[] { Hand(g, CardKind.Dodge), Hand(g, CardKind.Peach) }; Use(g, "give", given, [1]); Play(g);
            var cost = Hand(g, CardKind.Slash); BeginXinyou(g); Cost(g, cost); Pay(g);
            Reach(g, () => Optional(g, Zenrun)); var original = Frame(g, Xinyou).Id;
            var attempt = g.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single(a => a.Context.ParentFrameId == original);
            Need(attempt.Context.RequestedCount == 1 && attempt.Context.Reason.Value == XDraw && Moves(g, XDraw).Length == 0,
                "Production Zenrun is offered on Xinyou's genuine first native Draw1, before DrawPile material is moved.");
            g = Cold(g, r); Activate(g); Reach(g, () => Branch(g, "target"));
            Answer(g, c => c.Parameters.GetValueOrDefault("branch") == "target" && c.Targets.SequenceEqual([1]));
            Reach(g, () => Branch(g, "pick"));
            Need(P(g)!.Choices.Where(c => c.Parameters.GetValueOrDefault("branch") == "pick").All(c => c.Cards.Count == 0) &&
                g.CreateSnapshot(0).Players[1].Hand.Count == 0,
                "The replacement offers actual foreign Hand slots without revealing their faces to the requester.");
            Answer(g, c => c.Parameters.GetValueOrDefault("branch") == "pick" && c.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand));
            Answer(g, c => c.Parameters.GetValueOrDefault("branch") == "take");
            Reach(g, () => Option(g, Obtained, "obtained"));
            var replacement = g.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single(a => a.Id == attempt.Id).Replacement!;
            Need(replacement.GainedIds is [var id] && given.Contains(id) && Moves(g, XDraw).Length == 0 &&
                E<XieLingYuDrawReturnedEvent>(g).All(e => e.FrameId != original),
                "The real obtained entity and its child remain separate from the unpaid original DrawPile material result.");
            AssertPrivate(g, 0); g = Cold(g, r); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "obtained");
            Reach(g, () => g.CreateSnapshot(1).PendingDecision?.SkillPrompt?.SkillId == Zenrun);
            AssertPrivate(g, 1); g = Cold(g, r); Accept(g, new AdvanceOneStepCommand(g.Revision)); Play(g);
            var returned = E<XieLingYuDrawReturnedEvent>(g).Single(e => e.FrameId == original);
            var gained = E<XinyouGainedEntityEvent>(g).Single(e => e.FrameId == original);
            Need(returned is { Request: 1, ActualCount: 0, ReplacementCount: 1, Stop: true } && gained.Replacement &&
                replacement.GainedIds.SequenceEqual([gained.CardId]) && given.Contains(gained.CardId) &&
                gained.PrintedKind is CardKind.Dodge or CardKind.Peach && Zone(g, gained.CardId) == CardLocation.Hand(0) &&
                Moves(g, XDraw).Length == 0 && CostMoves(g, original).Select(m => m.CardId).SequenceEqual([cost]) &&
                E<NativeDrawAttemptDecidedEvent>(g).Count(e => e.AttemptFrameId == attempt.Id && e.Replaced) == 1 &&
                E<XieLingYuCompletedEvent>(g).Single(e => e.FrameId == original) is { Paid: true, Requests: 1 },
                "One real replacement Basic stops the Slash-cost loop and receives its actual-gain identity without forging an original draw ID or repeating either payment.");
            End(g); Reach(g, () => P(g) is { PlayerSeat: 0, Kind: DecisionKind.DiscardCards });
            Need(P(g)!.RequiredCardCount == 1 && !P(g)!.ValidCardIds.Contains(gained.CardId),
                "The actual replacement-obtained entity is also excluded from this turn's genuine discard payment.");
            _ = Cold(g, r);
        }
    }

    private static CardKind Name(CardKind kind) => kind is CardKind.FireSlash or CardKind.ThunderSlash ? CardKind.Slash : kind;
    private static int Hand(GameEngine g, CardKind kind) => V(g).Hand.FirstOrDefault(c => Name(c.Kind) == kind)?.Id ??
        throw new InvalidOperationException("Missing actual Hand " + kind + " " + Diagnostic(g));
    private static PlayerSnapshot V(GameEngine g, int seat = 0) => g.CreateSnapshot(seat).Players[seat];
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 4).Select(s => g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static T[] E<T>(GameEngine g) where T : IGameEvent => g.Events.Select(e => e.Payload).OfType<T>().ToArray();
    private static CardLocation Zone(GameEngine g, int id) => g.CreateCardZoneDiagnostics().Single(z => z.CardId == id).Location;
    private static bool Has(GameEngine g, string skill) => V(g).Skills!.Any(s => s.Id == skill);
    private static ProgramSkillFrame Frame(GameEngine g, string skill) => g.ResolutionStack.OfType<ProgramSkillFrame>().Last(f => f.SkillId == skill && f.XieLingYu is not null);
    private static CardMovementRecord[] Moves(GameEngine g, string reason) => g.CardMovements.Where(m => m.Reason.Value == reason && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0)).ToArray();
    private static CardMovementRecord[] CostMoves(GameEngine g, long frame) => g.CardMovements.Where(m => m.Reason.Value == XCost &&
        E<XinyouMaterialDiscardedEvent>(g).Any(e => e.FrameId == frame && e.CardId == m.CardId && e.MovementSequence == m.Sequence)).ToArray();
    private static bool Branch(GameEngine g, string branch) => P(g)?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == branch) == true;
    private static bool Optional(GameEngine g, string skill) => P(g)?.SkillPrompt?.SkillId == skill && P(g)!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
    private static bool Option(GameEngine g, string skill, string option) => P(g) is { } p && p.SkillPrompt?.SkillId == skill && p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == option);
    private static void Activate(GameEngine g) => Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "activate");
    private static void Cost(GameEngine g, int id) => Answer(g, c => c.Parameters.GetValueOrDefault("branch") == "cost" && c.Cards.SequenceEqual([id]));
    private static void Pay(GameEngine g) => Answer(g, c => c.Parameters.GetValueOrDefault("branch") == "pay");
    private static void BeginXinyou(GameEngine g)
    {
        var action = g.GetHumanLegalActions().Single(a => a.SkillId == Xinyou);
        Accept(g, new UseProgramSkillCommand(0, Xinyou, action.ProgramActivationId!, [], [], g.Revision, P(g)!.PromptId));
        Reach(g, () => Branch(g, "cost"));
    }
    private static void Stock(GameEngine g, string id = "stock-nine") { Use(g, id); Play(g); }
    private static void Use(GameEngine g, string id, int[]? cards = null, int[]? targets = null) =>
        Accept(g, new UseProgramSkillCommand(0, Driver, id, cards ?? [], targets ?? [], g.Revision, P(g)!.PromptId));
    private static void End(GameEngine g) => Accept(g, new EndPlayPhaseCommand(0, g.Revision, P(g)!.PromptId));
    private static void Play(GameEngine g) => Reach(g, () => P(g) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
    private static void Answer(GameEngine g, Func<PromptChoice, bool> test)
    {
        var prompt = P(g) ?? throw new InvalidOperationException("No published decision " + Diagnostic(g));
        var choice = prompt.Choices.FirstOrDefault(test);
        Need(prompt.PlayerSeat == 0 && choice is not null, "Only the human's actual published choice can be answered.");
        Accept(g, new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, g.Revision));
    }
    private static void AssertPrivate(GameEngine g, int seat) => Need(g.CreateSnapshot(seat).PendingDecision is { IsPrivate: true } p && p.PlayerSeat == seat &&
        Enumerable.Range(0, 4).Where(s => s != seat).All(s => g.CreateSnapshot(s).PendingDecision is null), "Every pending private selection belongs only to its real chooser.");
    private static void Reach(GameEngine g, Func<bool> done, [CallerMemberName] string caller = "", [CallerLineNumber] int line = 0)
    {
        for (var i = 0; i < 180; i++)
        {
            if (done()) return;
            Need(g.State.Winner == Winner.None, "Ended before " + caller + ":" + line);
            var prompt = P(g);
            if (prompt is null || prompt.PlayerSeat != 0) Accept(g, new AdvanceOneStepCommand(g.Revision));
            else if (prompt.Kind == DecisionKind.PlayCard) End(g);
            else if (prompt.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification)
                Answer(g, c => c.Cards.Count == 0 && c.Parameters.GetValueOrDefault("response") is not ("faction-defense-request" or "faction-slash-request"));
            else if (prompt.Kind == DecisionKind.RescueDying) Answer(g, c => c.Parameters.GetValueOrDefault("response") is "let-die" or "pass");
            else if (prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "skip");
            else throw new InvalidOperationException("Unexpected " + caller + ":" + line + " " + Diagnostic(g));
        }
        throw new InvalidOperationException("Missing bounded " + caller + ":" + line + " " + Diagnostic(g));
    }
    private static void Accept(GameEngine g, GameCommand command)
    {
        try { var result = g.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, "Rejected " + command.GetType().Name + ": " + JsonSerializer.Serialize(result)); }
        catch (Exception error) { throw new InvalidOperationException("scene=" + Scene + " command=" + command.GetType().Name + " cause=" + error + " " + Diagnostic(g), error); }
    }
    private static void Need(bool yes, string text)
    { if (!yes) throw new InvalidOperationException("scene=" + Scene + " " + text + (DiagnosticContext is { } g ? " " + Diagnostic(g) : "")); }
    private static string Diagnostic(GameEngine g) => JsonSerializer.Serialize(new { Scene, g.State.TurnNumber, g.State.CurrentSeat, g.State.Phase,
        Players = Enumerable.Range(0, 4).Select(s => new { Seat = s, V(g, s).Hp, V(g, s).MaxHp, V(g, s).IsAlive, V(g, s).HandCount }),
        Prompt = P(g), Frames = g.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())),
        Paid = E<XinyouPaidEvent>(g), Draws = E<XieLingYuDrawReturnedEvent>(g), Yuan = E<YuandiPaidEvent>(g) });
    private static string State(GameEngine g) => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(s => SnapshotJson.Serialize(g.CreateSnapshot(s))),
        Frames = g.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), g.CardMovements, Zones = g.CreateCardZoneDiagnostics(),
        Events = g.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), Commands = CommandJson.Serialize(g.AcceptedCommands) });
    private static GameEngine Cold(GameEngine g, ContentRegistry r)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), r);
        Need(State(g) == State(restored), "Cold replay preserves all private drafts, printed-name history, paid native draws and exact material cursors.");
        DiagnosticContext = restored; return restored;
    }
    private static (GameEngine, ContentRegistry) Start(string scene)
    {
        Scene = scene; DiagnosticContext = null; ContentRegistry registry;
        try { registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(scene)); }
        catch (Exception error) { throw new InvalidOperationException("scene=" + scene + " fixture cause=" + error, error); }
        var g = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0,
            HumanRole = scene == "paid-death" ? Role.Renegade : Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = scene is "five" or "replacement", AdvanceAfterHumanCommands = false, MaxTurns = 14 }, registry);
        DiagnosticContext = g; Accept(g, new StartGameCommand()); Reach(g, () => P(g) is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral });
        Accept(g, new SelectGeneralCommand(0, Owner, g.Revision, P(g)!.PromptId)); Play(g); return (g, registry);
    }

    private sealed class Fixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-xie-ling-yu", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "ol:xie-ling-yu");
            if (scene == "replacement") RegisterProductionSkill(b, "ol-sun-hong", Zenrun);
            if (scene is "chain-use" or "chain-recast") CharacterFixtureContent.Register(b, "classic:pang-tong");
            static JsonNode N(string json) => JsonNode.Parse(json)!;
            static JsonObject A(string id, string effects, int cards = 0, int targets = 0, int? maximumCards = null)
            {
                var activation = new JsonObject { ["id"] = id, ["minCards"] = cards, ["maxCards"] = maximumCards ?? cards,
                    ["minTargets"] = targets, ["maxTargets"] = targets, ["targetKind"] = "anyLiving", ["usesPerTurn"] = null,
                    ["effects"] = N("[" + effects + "]") };
                if ((maximumCards ?? cards) > 0) activation["sourceZones"] = N("[\"hand\"]");
                return activation;
            }
            var definitions = new JsonArray();
            var duel = A("duel", """{"op":"useSelectedActorDuel","target":"owner"}""", targets: 1);
            duel["targetKind"] = "otherLiving"; duel["usesPerPhase"] = 2;
            var driver = new JsonObject { ["id"] = Driver, ["revision"] = 1, ["activations"] = new JsonArray(
                A("stock-nine", """{"op":"draw","target":"owner","amount":9}"""), A("stock-rest", """{"op":"draw","target":"owner","amount":3}"""),
                A("stock-six", """{"op":"draw","target":"owner","amount":6}"""), A("stock-thirteen", """{"op":"draw","target":"owner","amount":13}"""),
                A("drop-one", """{"op":"discardSelected","target":"owner","amount":1}""", 1), A("drop-two", """{"op":"discardSelected","target":"owner","amount":2}""", 2),
                A("drop-three", """{"op":"discardSelected","target":"owner","amount":3}""", 3),
                A("top", """{"op":"captureSelectedCards","target":"owner","resultBind":"top"},{"op":"moveBoundCards","target":"owner","sourceBind":"top","destination":"drawPileTop","awaitMovementTriggers":true}""", 1),
                A("wound", """{"op":"loseHp","target":"owner","amount":2}"""),
                A("give", """{"op":"giveSelected","target":"selectedTarget"}""", 2, 1),
                A("incoming", """{"op":"offerVirtualSlashOrDraw","target":"selectedTarget"}""", targets: 1), duel) };
            if (scene is not ("five" or "replacement")) driver["modifiers"] = N("""[{"id":"keep","query":"handLimit","operation":"add","value":30,"priority":0}]""");
            if (scene == "converted-null") driver["viewAs"] = N("""[{"id":"native-counterspell","inputKinds":["dodge"],"inputSuits":["club"],"sourceZones":["hand"],"outputKind":"nullification","forPlay":false,"forResponse":true}]""");
            definitions.Add(driver);
            definitions.Add(N("{\"id\":\"" + Quiet + "\",\"revision\":1,\"triggers\":[{\"id\":\"quiet\",\"window\":\"afterNormalDraw\",\"subject\":\"owner\",\"optional\":false,\"effects\":[{\"op\":\"skipTurnPhases\",\"target\":\"owner\",\"phases\":[\"play\"]}]}]}"));
            if (scene == "paid-loss") definitions.Add(N("{\"id\":\"" + Conditional + "\",\"revision\":1,\"hpSkillGrants\":[{\"id\":\"healthy-source\",\"skillId\":\"ol:xinyou\",\"minimumHp\":9}]}"));
            if (scene.StartsWith("paid-", StringComparison.Ordinal))
            {
                var effects = new JsonArray(N("""{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"paid"}]}"""));
                if (scene != "paid-child") effects.Add(new JsonObject { ["op"] = "loseHp", ["target"] = "owner", ["amount"] = scene == "paid-death" ? 20 : 1 });
                var triggers = new JsonArray(new JsonObject { ["id"] = "real-paid-cost", ["window"] = "cardsMoved", ["subject"] = "owner",
                    ["sourceZones"] = N("[\"hand\"]"), ["movementOccurrence"] = "perBatch", ["movementReasons"] = new JsonArray(XCost), ["optional"] = false, ["effects"] = effects });
                if (scene == "paid-child") triggers.Add(new JsonObject { ["id"] = "real-draw-child", ["window"] = "cardsGained", ["subject"] = "owner",
                    ["destinationZones"] = N("[\"hand\"]"), ["movementOccurrence"] = "perBatch", ["movementReasons"] = new JsonArray(XDraw), ["optional"] = false,
                    ["effects"] = N("""[{"op":"chooseOption","target":"owner","resultBind":"gained","options":[{"id":"gained"}]}]""") });
                definitions.Add(new JsonObject { ["id"] = Watch, ["revision"] = 1, ["triggers"] = triggers });
            }
            if (scene == "replacement") definitions.Add(new JsonObject { ["id"] = Obtained, ["revision"] = 1, ["triggers"] = new JsonArray(new JsonObject {
                ["id"] = "real-replacement-gain", ["window"] = "cardsGained", ["subject"] = "owner", ["destinationZones"] = N("[\"hand\"]"),
                ["movementOccurrence"] = "perBatch", ["movementReasons"] = N("[\"skill-program.sun-hong.zenrun.obtain\"]"), ["optional"] = false,
                ["effects"] = N("""[{"op":"chooseOption","target":"owner","resultBind":"obtained","options":[{"id":"obtained"}]}]""") }) });
            if (scene == "arrows") definitions.Add(new JsonObject { ["id"] = Assault, ["revision"] = 1, ["triggers"] = N("""[{"id":"actual-native-arrows","window":"turnStartBeforeNormalFlow","subject":"owner","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"useVirtualOrdinaryTrick","target":"owner","actorRef":{"kind":"owner"},"targetRef":{"kind":"owner"},"outputKind":"arrowBarrage"}]}]""") });
            var labels = definitions.ToDictionary(n => n!["id"]!.GetValue<string>(), n => new { name = n!["id"]!.GetValue<string>(), description = "真实原生入弃/连续Draw1行为夹具",
                optionLabels = new[] { "activations", "triggers" }.SelectMany(k => n[k]?.AsArray() ?? new JsonArray()).SelectMany(t => t!["effects"]!.AsArray())
                    .SelectMany(e => e!["options"]?.AsArray() ?? new JsonArray()).Select(o => o!["id"]!.GetValue<string>()).Distinct().ToDictionary(x => x, x => x) });
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, "真实元嫡/心幽夹具") { Program = p, ProgramPresentation = catalog.Presentations[id] });
            var own = new List<string> { Driver, Yuandi, scene == "paid-loss" ? Conditional : Xinyou };
            if (scene.StartsWith("paid-", StringComparison.Ordinal)) own.Add(Watch);
            if (scene == "replacement") { own.Add(Zenrun); own.Add(Obtained); }
            if (scene is "chain-use" or "chain-recast") own.Add("classic:lianhuan");
            b.AddSkill(new("fixture:xly-own-pick", "本人", "固定公开选将") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(role => role, _ => -100000000d) });
            b.AddGeneral(new(Owner, "真实谢灵毓机制", "ol-xie-ling-yu", "fixture:xly-own-pick", "wu",
                scene == "five" ? 3 : scene == "replacement" ? 4 : scene.StartsWith("paid-", StringComparison.Ordinal) ? 8 : 12, own, GeneralGender.Female));
            var peers = Enumerable.Range(1, 3).Select(i => "fixture:xly-peer-" + i).ToArray();
            for (var i = 0; i < peers.Length; i++)
            {
                var pick = "fixture:xly-peer-pick-" + i;
                b.AddSkill(new(pick, "其他角色", "固定公开选将") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(role => role, _ => 1000000d * (3 - i)) });
                b.AddGeneral(new(peers[i], "真实其他角色", "supporter", pick, "shu", 12, scene == "arrows" ? [Quiet, Assault] : [Quiet], GeneralGender.Male));
            }
            ContentDeckPhysicalCard[] deck = scene == "nonbasic"
                ? Enumerable.Range(0, 24).Select(i => new ContentDeckPhysicalCard(i < 12 ? "standard:dodge" : "standard:crossbow", Suit.Heart, 7)).ToArray()
                : scene == "five" || scene.StartsWith("paid-", StringComparison.Ordinal)
                    ? Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray()
                    : Enumerable.Range(0, 12).Select(i => new ContentDeckPhysicalCard(i < 4 ? "standard:slash" : i < 8 ? "standard:dodge" : "standard:peach",
                        scene is "converted-null" or "chain-use" or "chain-recast" ? Suit.Club : Suit.Heart, 7)).ToArray();
            b.AddDeck(new("fixture:xly-deck", "元嫡/心幽固定小实体牌堆", 0, 0, []) { PhysicalCards = deck });
            var roles = scene == "paid-death" ? new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }
                : new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 };
            b.AddMode(new(Mode, "谢灵毓真实四人边界", 4, 4, roles, "fixture:xly-deck", GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }

        private static void RegisterProductionSkill(IContentRegistryBuilder builder, string bundle, string skillId)
        {
            string Read(string suffix)
            {
                using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream(
                    "CardGame.Content.Standard.SkillPrograms." + bundle + "." + suffix + ".json")
                    ?? throw new InvalidOperationException("Missing production resource " + bundle + "." + suffix);
                using var reader = new StreamReader(stream); return reader.ReadToEnd();
            }
            var catalog = SkillProgramCatalog.Load(Read("rules"), Read("presentation"));
            var presentation = catalog.Presentations[skillId];
            builder.AddSkill(new(skillId, presentation.Name, presentation.Description)
            { Program = catalog.Programs[skillId], ProgramPresentation = presentation });
        }
    }
}
