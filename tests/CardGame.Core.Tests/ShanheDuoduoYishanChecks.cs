using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDuoduoYishanChecks
{
    private const string First = "shanhe:current-duoduo-yishan-2082", Second = "shanhe:current-duoduo-yishan-2083", Third = "shanhe:current-duoduo-yishan-2084";
    private const string Capacity = StandardShanheCampaignCatalog.CurrentJinnangJiSkillId, Zaiqi = StandardShanheCampaignCatalog.S5ZaiqiSkillId;
    private const string Owner = "fixture:ddys-owner", Deck = "fixture:ddys-deck", Driver = "fixture:ddys-driver", ThirdDraw = "fixture:ddys-third-draw";
    private const string Gain = "fixture:ddys-gain", Mutation = "fixture:ddys-mutation", Gate = "fixture:ddys-hp-source";
    private const string Extra = "fixture:ddys-phase-draw", OffTurn = "fixture:ddys-off-turn", Quiet = "fixture:ddys-quiet";
    private static string Peer(int seat) => "fixture:ddys-peer-" + seat;
    private static readonly string[] Families = [First, Second, Third];
    private static readonly Lazy<ContentRegistry> Production = new(() => StandardShanheCampaignCatalog.CreateBattleRegistry(
        new(83, 1, "fixture:ddys-production", "classic:guan-yu", 4, 0, 0, 0, 0, [Zaiqi], [], [],
            [new(StandardShanheS5BossPackage.ZhangliaoHuangkongId)], [], new string('b', 64))
        { TacticSkillIds = [First, Second, Third, Capacity] }));

    public static void CapturedDrawEligibilityRejectsLateSourcesAndPreservesPaidChildrenCold()
    {
        foreach (var scene in new[] { "late-before", "late-child", "loss-before", "paid-loss", "paid-death" })
        {
            var s = Scene.Start(scene); s.Play();
            s.Begin("one"); s.Play(); s.Begin("one"); s.Play();
            Need(s.Draws().Select(draw => draw.ActualTurnDrawOrdinal).SequenceEqual([1, 2]) && s.Paid(Second).Length == 0,
                "Two positive original Draw calls establish the real prefix without consuming II's third-call opportunity.");
            if (scene == "late-before") { s.Begin("lose"); s.Play(); Need(s.Has(Second), "A real HP payment obtains II before the third native invocation begins."); }
            s.BeginThird();
            if (scene is "late-child" or "loss-before")
            {
                s.Until(() => s.AtMutation("mutated"));
                Need((scene == "late-child" ? s.Has(Second) : !s.Has(Second)) && s.Paid(Second).Length == 0 &&
                    s.Draws().Single(draw => draw.ActualTurnDrawOrdinal == 3).ActualCount == 2,
                    "A genuine early gain child changes the live HP-derived source before the final batch can offer a reward. " + s.Diagnostic());
                s.Cold(); s.Continue(); s.Until(() => s.AtOrdinal(3));
                var captured = s.Proof!.OrdinalEntitlements?.Any(entry => entry.Source.SkillId == Second) == true;
                Need(captured == (scene == "loss-before") && s.Paid(Second).Length == 0,
                    "Eligibility records the source at native Draw start, rather than borrowing a grant from an earlier batch of that same Draw.");
                s.Cold(); s.Play();
                Need(s.Paid(Second).Length == 0 && s.Draws().Length == 3,
                    "Late acquisition cannot backfill an unqualified third Draw; withdrawal before reward payment also cancels it.");
                if (scene == "loss-before") { s.Begin("lose"); s.Play(); }
                s.Begin("one"); s.Play();
                Need(s.Has(Second) && s.Draws().Last().ActualTurnDrawOrdinal == 4 && s.Paid(Second).Length == 0,
                    "A later active source cannot turn the fourth native Draw into the already passed third-call opportunity.");
                s.Cold(); continue;
            }
            if (scene is "paid-loss" or "paid-death")
            {
                s.Until(() => s.AtMutation("paid"));
                var paid = s.Paid(Second).Single();
                var paidProgram = paid.ParentFrameId ?? throw new InvalidOperationException("The real reward must retain its producing program frame.");
                Need(paid.ActualTurnDrawOrdinal == 4 && paid.ActualCount == 1 && s.V().HandCount == 5,
                    "The genuine reward has already paid its one entity before its own gained-card child changes the source.");
                s.Privacy(); s.Cold(); s.Continue();
                if (scene == "paid-loss")
                {
                    s.Until(() => s.AtMutation("mutated"));
                    Need(!s.Has(Second) && s.V().Hp == 6, "Real recovery withdraws the HP-conditioned source inside the paid reward's child.");
                    s.Cold(); s.Play();
                }
                else
                {
                    s.Until(() => !s.V().IsAlive && s.G.ResolutionStack.All(frame => frame.Id != paidProgram) &&
                        s.E<ProgramBindingResolvedEvent>().Any(fact => fact.FrameId == paidProgram && fact.SkillId == Second));
                    Need(s.G.CreateSnapshot(0).Winner == Winner.None && s.G.CreateSnapshot(0).Players[1].IsAlive &&
                        s.E<PlayerDiedEvent>().Any(fact => fact.VictimSeat == 0),
                        "The real LoseHp/Dying/LetDie child returns the paid reward program while a living ally keeps the match running; game-over cleanup cannot conceal a broken return.");
                }
                Need(s.Paid(Second).Length == 1 && s.Draws().Count(draw => draw.InvocationId == paid.InvocationId) == 1 &&
                    s.G.CardMovements.Count(move => move.Reason == paid.Reason && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) == 1 &&
                    s.E<ProgramBindingResolvedEvent>().Count(fact => fact.FrameId == paidProgram && fact.SkillId == Second) == 1 &&
                    s.G.ResolutionStack.All(frame => frame.Id != paidProgram),
                    "Source loss or death after payment cannot retract, repeat or orphan the one real reward entity.");
                s.Cold(); continue;
            }
            s.Play();
            Need(s.Draws().Select(draw => draw.ActualTurnDrawOrdinal).SequenceEqual([1, 2, 3, 4]) && s.Paid(Second).Length == 1,
                "An actually acquired source before the third invocation qualifies against the genuine earlier unowned draw prefix.");
            s.Cold();
        }
    }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static string State(GameEngine game) => JsonSerializer.Serialize(new
    {
        Views = Enumerable.Range(0, game.PlayerCount).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))),
        Frames = game.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType())),
        Facts = game.Events.Select(fact => JsonSerializer.Serialize(fact.Payload, fact.Payload.GetType())),
        game.Revision, game.CardMovements, Zones = game.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(game.AcceptedCommands)
    });

    private sealed class Scene(GameEngine game, ContentRegistry registry, string scene)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public int Turn => G.CreateSnapshot(0).TurnNumber;
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public T[] E<T>() => G.Events.Select(fact => fact.Payload).OfType<T>().ToArray();
        public CardMovementBatchContext? Batch => G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(frame => frame.WindowContext?.MovementBatch is not null)?.WindowContext?.MovementBatch;
        public NativeDrawInvocationProof? Proof => Batch?.NativeDrawInvocation;
        public NativeDrawInvocationRecordedEvent[] Draws() => E<NativeDrawInvocationRecordedEvent>().Where(fact => fact.OwnerSeat == 0 && fact.TurnNumber == Turn && fact.ActualTurnDrawOrdinal is not null).ToArray();
        public NativeDrawInvocationRecordedEvent[] Paid(string id) => E<NativeDrawInvocationRecordedEvent>().Where(fact => fact.OwnerSeat == 0 && fact.Reason == new CardMoveReason("skill-program." + id + ".Draw")).ToArray();
        public int DrawPhaseTotal() => Draws().Where(draw => draw.Phase is { Kind: ActualDiscardRecoveryPhaseKind.Draw, ActorSeat: 0 }).Sum(draw => draw.ActualCount);
        public bool Has(string id) => V().Skills?.Any(skill => skill.ContentId == id) == true;
        public bool AtGain() => P?.SkillPrompt?.SkillId == Gain;
        public bool AtOrdinal(int ordinal) => AtGain() && Proof is { } proof && proof.ActualTurnDrawOrdinal == ordinal && proof.LastBatchId == Batch!.Id;
        public bool AtMutation(string option) => P is { SkillPrompt.SkillId: Mutation } prompt && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == option);
        public bool FromSkill(string id) => Proof is { ParentFrameId: { } parent } proof && proof.Reason == new CardMoveReason("skill-program." + id + ".Draw") &&
            G.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == parent && frame.SkillId == id && frame.OwnerSeat == 0) &&
            E<ProgramBindingStartedEvent>().Any(fact => fact.FrameId == parent && fact.SkillId == id && fact.OwnerSeat == 0);
        public void Accept(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, (result.Error?.Message ?? "Actual draw command rejected.") + " " + Diagnostic());
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P ?? throw new InvalidOperationException("Missing real draw decision."); Accept(new AnswerPromptCommand(0, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Continue() => Answer(choice => choice.Parameters.GetValueOrDefault("option-id") is "continue" or "mutated" or "paid");
        public void Until(Func<bool> stop)
        {
            for (var step = 0; step < 320; step++)
            {
                if (stop()) return;
                if (P is { PlayerSeat: 0 })
                {
                    if (AtGain() || AtMutation("mutated") || AtMutation("paid")) { Continue(); continue; }
                    if (P.Kind == DecisionKind.RescueDying) { Answer(choice => choice.Parameters.GetValueOrDefault("response") == "let-die"); continue; }
                    throw new InvalidOperationException("Unexpected native draw boundary: " + Diagnostic());
                }
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Bounded real draw boundary not reached: " + Diagnostic());
        }
        public void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Begin(string activation) => Accept(new UseProgramSkillCommand(0, Driver, activation, [], [], G.Revision, P!.PromptId));
        public void BeginThird() => Accept(new UseProgramSkillCommand(0, ThirdDraw, "third", [], [], G.Revision, P!.PromptId));
        public void EndPlay() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Cold()
        { var before = State(G); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == State(G), "Cold replay retains exact native ordinals, entitlements, real children and once-paid entities."); }
        public void Privacy()
        {
            for (var viewer = 0; viewer < G.PlayerCount; viewer++)
            {
                var snapshot = G.CreateSnapshot(viewer);
                Need(snapshot.Players.Where(player => player.Seat != viewer).All(player => player.Hand.Count == 0), "Native draw proof cannot reveal another player's private hand identities.");
                if (viewer != 0) Need(snapshot.PendingDecision is null, "Only the real owner receives the private gain-child prompt.");
            }
        }
        public void AssertPureQueries()
        { var before = State(G); for (var repeat = 0; repeat < 3; repeat++) for (var viewer = 0; viewer < G.PlayerCount; viewer++) _ = G.CreateSnapshot(viewer); Need(before == State(G), "Ordinal and hand-limit projection is pure and issues no second reward."); }
        public string Diagnostic() => JsonSerializer.Serialize(new
        {
            scene, Turn, V().Hp, V().HandCount, V().HandLimit, Alive = V().IsAlive,
            Prompt = P is null ? null : new { P.Kind, Skill = P.SkillPrompt?.SkillId, P.Choices },
            Draws = E<NativeDrawInvocationRecordedEvent>().Select(draw => new { draw.InvocationId, draw.OwnerSeat, draw.TurnNumber, draw.ActualTurnOwnerSeat,
                draw.ActualTurnDrawOrdinal, draw.Requested, draw.ActualCount, draw.PhaseActorSeat, PhaseKind = draw.Phase?.Kind,
                Producer = draw.DirectProducer?.SkillId, draw.LastBatchId }),
            Frames = G.ResolutionStack.Select(frame => new { frame.Id, frame.Kind })
        });
        public static Scene Start(string scene)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(scene));
            var health = scene == "zaiqi" ? 4 : scene is "loss-before" or "paid-loss" or "paid-death" ? 5 : 6;
            var allied = scene is "offturn" or "paid-death";
            var allies = allied ? new ShanheCombatantDefinition[] { new(Peer(1)) } : [];
            var enemies = (allied ? new[] { 2, 3 } : new[] { 1, 2, 3 }).Select(seat => new ShanheCombatantDefinition(Peer(seat))).ToArray();
            var setup = new ShanheBattleSetup(83, 1, "fixture:ddys-" + scene, Owner, health, 0, 0, 0, 0, [], [], [], enemies, allies, new string('b', 64));
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var result = new Scene(game, registry, scene); result.Accept(new StartGameCommand()); return result;
        }
    }

    private sealed class Fixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ddys", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            foreach (var id in Families.Append(Capacity)) b.AddSkill(Production.Value.GetSkill(id));
            if (scene == "zaiqi") b.AddSkill(Production.Value.GetSkill(Zaiqi));
            if (scene == "tuxi") b.AddSkill(Production.Value.GetSkill("classic:tuxi"));
            var definitions = new List<Dictionary<string, object>>();
            var labels = new Dictionary<string, object>();
            object Pause(string option) => new { op = "chooseOption", target = "owner", resultBind = option, options = new[] { new { id = option } } };
            object Effect(string op, int amount) => new { op, target = "owner", amount };
            void Add(string id, string property, object value, params string[] options)
            {
                definitions.Add(new() { ["id"] = id, ["revision"] = 1, [property] = value });
                labels[id] = new { name = id, description = "真实原生摸牌、来源与子流程，不修改运行时快照或注入事实", optionLabels = options.ToDictionary(option => option, option => option) };
            }
            object Activation(string id, string op, int amount) => new
            { id, minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { Effect(op, amount) } };
            Add(Driver, "activations", new[] { Activation("one", "draw", 1), Activation("lose", "loseHp", 1) });
            Add(ThirdDraw, "activations", new[] { Activation("third", "draw", 2) });
            Add(Gain, "triggers", new[]
            {
                new { id = "actual-gain", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                    movementOccurrence = "perBatch", priority = 100, optional = false,
                    movementReasons = new[] { CardMoveReasons.Draw.Value, "skill-program." + Driver + ".Draw", "skill-program." + ThirdDraw + ".Draw",
                        "skill-program." + Extra + ".Draw", "skill-program." + OffTurn + ".Draw", "skill-program." + First + ".Draw",
                        "skill-program." + Second + ".Draw", "skill-program." + Third + ".Draw", "skill-program." + Zaiqi + ".MoveBoundCards",
                        "skill-program.classic:tuxi.TakeRandomHandCardFromSelectedTargets" }, effects = new[] { Pause("continue") } }
            }, "continue");
            if (scene is "late-before" or "late-child" or "loss-before" or "paid-loss" or "paid-death")
                Add(Gate, "hpSkillGrants", new[] { new { id = "real-hp-source", skillId = Second, maximumHp = 5 } });
            if (scene is "late-child" or "loss-before" or "paid-loss" or "paid-death")
            {
                var paid = scene.StartsWith("paid-", StringComparison.Ordinal);
                var effects = new List<object>(); if (paid) effects.Add(Pause("paid"));
                effects.Add(Effect(scene == "late-child" || scene == "paid-death" ? "loseHp" : "recover", scene == "paid-death" ? 20 : 1));
                if (scene != "paid-death") effects.Add(Pause("mutated"));
                Add(Mutation, "triggers", new[]
                {
                    new { id = "real-source-child", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch",
                        movementReasons = new[] { "skill-program." + (paid ? Second : ThirdDraw) + ".Draw" }, priority = 200, optional = false,
                        usageScope = "game", usageLimit = 1, effects = effects.ToArray() }
                }, scene == "paid-death" ? ["paid"] : paid ? ["paid", "mutated"] : ["mutated"]);
            }
            if (scene is "phase" or "cross") Add(Extra, "triggers", new[]
            {
                new { id = "actual-phase-draws", window = "drawPhaseEnded", subject = "owner", optional = false,
                    effects = scene == "phase" ? new[] { Effect("draw", 2), Effect("draw", 2) } : new[] { Effect("draw", 2) } }
            });
            if (scene == "offturn") Add(OffTurn, "triggers", new[]
            {
                new { id = "ally-actual-turn", window = "turnStartBeforeNormalFlow", subject = "owner", optional = false,
                    effects = new object[] { new { op = "selectAllPublicTeamTargets", target = "owner", targetKind = "otherPublicTeamAllies", allowEmpty = false },
                        new { op = "draw", target = "selectedTargets", amount = 1 }, new { op = "draw", target = "selectedTargets", amount = 1 },
                        new { op = "draw", target = "selectedTargets", amount = 1 } } }
            });
            Add(Quiet, "triggers", new[]
            {
                new { id = "quiet-dodge-holder", window = "afterNormalDraw", subject = "owner", optional = false,
                    effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } }
            });
            var programs = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in programs.Programs) b.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description)
            { Program = program, ProgramPresentation = programs.Presentations[id] });
            var own = new List<string> { ThirdDraw, Gain };
            if (scene == "stack") own.AddRange(Families);
            else if (scene is "phase" or "cross" or "offturn" or "zaiqi" or "tuxi") own.AddRange([Second, Capacity]);
            else own.Add(Gate);
            if (scene is "late-child" or "loss-before" or "paid-loss" or "paid-death") own.Add(Mutation);
            if (scene is "phase" or "cross") own.Add(Extra);
            if (scene == "zaiqi") own.Add(Zaiqi);
            if (scene == "tuxi") own.Add("classic:tuxi");
            b.AddGeneral(new(Owner, "多多益善真实摸牌者", "", Driver, "qun", 6, own));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "只持真实闪的角色", "", Quiet, "wei", 6,
                scene == "offturn" && seat == 1 ? [OffTurn] : []));
            b.AddDeck(new(Deck, "固定黑色闪", scene == "stack" ? 3 : scene == "tuxi" ? 2 : 0, scene is "phase" or "cross" ? 3 : scene is "zaiqi" or "tuxi" ? 2 : 0, [])
            { PhysicalCards = Enumerable.Range(0, 112).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "多多益善原生调用", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = scene is "offturn" or "paid-death" ? 2 : 1, [ShanheBattleSetup.EnemyTeamId] = scene is "offturn" or "paid-death" ? 2 : 3 }));
        }
    }
}
