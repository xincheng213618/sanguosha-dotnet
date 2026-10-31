using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheYaoliChecks
{
    private static readonly string[] Skills = ["shanhe:current-yaoli-2319", "shanhe:current-yaoli-2320", "shanhe:current-yaoli-2321",
        "shanhe:current-yaoli-2322", "shanhe:current-yaoli-2191", "shanhe:current-yaoli-2192"];
    private const string Owner = "fixture:yaoli-owner", Driver = "fixture:yaoli-driver", Deck = "fixture:yaoli-deck";
    private const string Health = "fixture:yaoli-health", Gain = "fixture:yaoli-gain", Gate = "fixture:yaoli-gate";
    private const string Mutation = "fixture:yaoli-mutation", Cost = "fixture:yaoli-cost", Quiet = "fixture:yaoli-quiet";
    private const string Kuanggu = "boundary:kuanggu", Blood = "shanhe:xueren", RecoveryDraw = "shanhe:shixue-1";
    private const string Zaiqi = "shanhe:s5-zaiqi-887", Jiuyuan = "boundary:jiuyuan", Yizheng = "ol:yizheng-cao-song";
    private static readonly Lazy<ContentRegistry> Production = new(() => StandardShanheCampaignCatalog.CreateBattleRegistry(
        new ShanheBattleSetup(83, 1, "fixture:yaoli-content", "classic:guan-yu", 4, 0, 0, 0, 0, [Zaiqi], [], [],
            [new("classic:cao-cao")], [], new string('c', 64)) { TacticSkillIds = Skills }));

    public static void LiveSourcesAndPaidChildrenPreserveEligibilityAndZeroRecoveryCold()
    {
        foreach (var scene in new[] { "startup-limited", "startup-unlimited", "ended-limited", "ended-unlimited" })
        {
            var s = Scene.Start(scene);
            if (scene.StartsWith("ended-", StringComparison.Ordinal)) { s.Play(); s.EndPlay(); }
            s.Until(s.AtHealth);
            var unlimited = scene.EndsWith("unlimited", StringComparison.Ordinal);
            Need(s.V().Hp == (unlimited ? 4 : 2) && s.E<ActualTurnRecoveryRecordedEvent>().Length == 0 &&
                s.E<RecoveryAppliedEvent>() is [{ Amount: var actual }] && actual == (unlimited ? 3 : 1),
                "Only unlimited policies modify a real startup or already-ended-turn recovery; neither creates an active-turn ordinal: " + s.Diagnostic());
            if (unlimited)
            {
                var bill = s.E<RecoveryAmountBonusAppliedEvent>().Single().Receipt;
                Need(bill.ActualTurnRecoveryOrdinal is null && bill.BaseRequestedAmount == 1 && bill.ModifiedRequestedAmount == 3 &&
                    bill.Sources.Select(source => source.Source.SkillId).SequenceEqual([Skills[5]]),
                    "The unlimited startup/ended payment freezes its one live source without inventing an active turn.");
            }
            else Need(s.E<RecoveryAmountBonusAppliedEvent>().Length == 0, "A limited policy cannot manufacture a turn-zero/ended bonus.");
            s.Cold();
        }

        var late = Scene.Start("late"); late.Play(); late.Begin("two"); late.Until(late.AtHealth);
        Need(late.E<ActualTurnRecoveryRecordedEvent>().Single() is { Ordinal: 1, ActualAmount: 2 } &&
            late.E<RecoveryAmountBonusAppliedEvent>().Length == 0, "The first real two-HP recovery counts even before the owner gains any medicine.");
        late.Play(); late.Use("grant-second");
        for (var ordinal = 2; ordinal <= 4; ordinal++)
        {
            late.Begin("one"); late.Until(late.AtHealth);
            if (ordinal <= 3) CheckPayment(late, 1, 2, ordinal, 2, [Skills[1]]);
            else Need(late.E<ActualTurnRecoveryRecordedEvent>().Last().Ordinal == 4 &&
                late.E<ActualTurnRecoveryRecordedEvent>().Last().ActualAmount == 1 && late.E<RecoveryAmountBonusAppliedEvent>().Length == 2,
                "Late acquisition uses the recipient's genuine prior recoveries instead of a new per-skill counter.");
            late.Cold(); late.Play();
        }

        var boundary = Scene.Start("source-boundary"); boundary.Play(); boundary.Use("lose-2");
        Need(!boundary.Has(Skills[1]) && boundary.V().Hp == 4, "A real HP-conditioned source is withdrawn before the next payment.");
        boundary.Begin("one"); boundary.Until(boundary.AtHealth);
        Need(boundary.Has(Skills[1]) && boundary.E<RecoveryAmountBonusAppliedEvent>().Length == 0 &&
            boundary.E<ActualTurnRecoveryRecordedEvent>().Single() is { Ordinal: 1, ActualAmount: 1 },
            "Gaining the source in the recovery's HP child cannot retroactively enhance its already-paid amount.");
        boundary.Cold(); boundary.Play(); boundary.Begin("one"); boundary.Until(boundary.AtHealth);
        CheckPayment(boundary, 1, 2, 2, 2, [Skills[1]]); boundary.Cold(); boundary.Play();

        foreach (var scene in new[] { "paid-loss", "paid-death" })
        {
            var s = Scene.Start(scene); s.Play(); s.Begin("one"); s.Until(() => s.AtMutation("paid"));
            CheckPayment(s, 1, 2, 1, 2, [Skills[1]]);
            var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Driver).Id;
            var billId = s.E<RecoveryAmountBonusAppliedEvent>().Single().RecoveryFrameId;
            s.Cold(); s.Continue();
            if (scene == "paid-loss")
            {
                s.Until(() => s.AtMutation("mutated"));
                Need(s.V().Hp == 2 && !s.Has(Skills[1]), "An actual recovery child loses three HP and withdraws its medicine source after payment.");
                s.Cold(); s.Play();
            }
            else
            {
                s.Until(() => !s.V().IsAlive && s.E<ProgramSkillResolvedEvent>().Any(fact => fact.FrameId == root));
                Need(s.G.State.Winner == Winner.None && s.V(1).IsAlive && s.E<PlayerDiedEvent>().Count(fact => fact.VictimSeat == 0) == 1 &&
                    !s.G.ResolutionStack.Any(frame => frame.Id == root),
                    "Actual dying/let-die returns the paid recovery producer with an ally still alive; game-over cleanup cannot conceal its continuation.");
            }
            Need(s.E<RecoveryAppliedEvent>() is [{ Amount: 2 }] && s.E<RecoveryAmountBonusAppliedEvent>() is [var bill] &&
                bill.RecoveryFrameId == billId && bill.Receipt.ModifiedRequestedAmount == 2 &&
                s.E<ActualTurnRecoveryRecordedEvent>() is [{ Ordinal: 1, RequestedAmount: 2, ActualAmount: 2 }] &&
                s.E<ProgramSkillResolvedEvent>().Where(fact => fact.FrameId == root).ToArray() is [var resolved] &&
                resolved is { OwnerSeat: 0, SkillId: Driver, ActivationId: "one" } && resolved.Completed == (scene == "paid-loss") &&
                !s.G.ResolutionStack.Any(frame => frame.Id == root),
                "Losing or dying in a true paid child neither refunds nor repeats the one frozen application: " + s.Diagnostic());
            s.Cold();
        }
    }
    private static string Peer(int seat) => "fixture:yaoli-peer-" + seat;
    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
    private static string? Option(PromptChoice choice) => choice.Parameters.GetValueOrDefault("option-id");
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static void CheckPayment(Scene s, int requested, int modified, int ordinal, int actual, IReadOnlyList<string> sources)
    {
        var fact = s.E<ActualTurnRecoveryRecordedEvent>().Last(entry => entry.TargetSeat == 0);
        var paid = s.E<RecoveryAmountBonusAppliedEvent>().Single(entry => entry.RecoveryFrameId == fact.ParentFrameId);
        var receipt = paid.Receipt;
        Need(fact.Ordinal == ordinal && fact.RequestedAmount == actual && fact.ActualAmount == actual && fact.TurnNumber == s.Turn &&
            fact.TurnOwnerSeat == 0 && receipt.BaseRequestedAmount == requested && receipt.ModifiedRequestedAmount == modified &&
            receipt.ActualTurnRecoveryOrdinal == ordinal && receipt.TargetSeat == 0 && receipt.HpAfter - receipt.HpBefore == actual &&
            receipt.AppliedRecoveryFrameId == fact.RecoveryFrameId && receipt.MaxHpAtApplication >= receipt.HpAfter &&
            receipt.Sources.Select(entry => entry.Source.SkillId).Order().SequenceEqual(sources.Order()) &&
            receipt.Sources.All(entry => entry.GameplayHash.Length == 64 && !string.IsNullOrEmpty(entry.Source.SkillInstanceId)) &&
            s.G.ResolutionStack.OfType<RecoveryReplacementFrame>().Any(frame => frame.Id == paid.RecoveryFrameId &&
                JsonSerializer.Serialize(frame.AmountBonus) == JsonSerializer.Serialize(receipt)),
            "One live-source bonus receipt is frozen on its exact recovery owner and equals the genuine actual-turn payment: " + s.Diagnostic());
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry, string scene)
    {
        public GameEngine G { get; private set; } = game;
        public int ColdPaidChildren { get; private set; }
        public int Turn => G.State.TurnNumber;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(0).Players[seat];
        public T[] E<T>() => G.Events.Select(fact => fact.Payload).OfType<T>().ToArray();
        public bool Has(string id) => V().Skills?.Any(skill => skill.ContentId == id) == true;
        public bool AtHealth() => P?.SkillPrompt?.SkillId == Health;
        public bool AtMutation(string option) => P?.SkillPrompt?.SkillId == Mutation && P.Choices.Any(choice => Option(choice) == option);
        public bool AtDriver(string option) => P?.SkillPrompt?.SkillId == Driver && P.Choices.Any(choice => Option(choice) == option);
        public bool Optional(string id) => P?.SkillPrompt?.SkillId == id && P.Choices.Any(choice => Action(choice) == "activate");
        public void Accept(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, (result.Error?.Message ?? "Actual medicine command rejected.") + " " + Diagnostic());
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = P ?? throw new InvalidOperationException("Missing actual medicine prompt."); Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First(predicate).Id, G.Revision)); }
        public void Continue() => Answer(choice => Option(choice) is "continue" or "tail" or "paid" or "mutated" or "outside" or "support-tail");
        public void Begin(string activation, int[]? targets = null) => Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], G.Revision, P!.PromptId));
        public void Use(string activation) { Begin(activation); Play(); }
        public void EndPlay() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void BeginPeach()
        {
            var card = V().Hand.First(card => card.Kind == CardKind.Peach);
            var action = G.GetHumanLegalActions().First(action => action.CardId == card.Id && action.PlayedCardKind is null or CardKind.Peach);
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Until(Func<bool> stop)
        {
            for (var step = 0; step < 400; step++)
            {
                if (stop()) return;
                if (P is { PlayerSeat: 0 } prompt)
                {
                    if (prompt.SkillPrompt?.SkillId is Health or Gain or Mutation or Cost || AtDriver("tail") || AtDriver("outside"))
                    { if (AtHealth()) { Cold(); ColdPaidChildren++; } Continue(); continue; }
                    if (Optional(Kuanggu)) { Answer(choice => Action(choice) == (V().Hp < V().MaxHp ? "activate" : "skip")); continue; }
                    if (prompt.SkillPrompt?.SkillId == Kuanggu && prompt.Choices.Any(choice => Option(choice) == "recover"))
                    { Answer(choice => Option(choice) == "recover"); continue; }
                    if (prompt.Kind == DecisionKind.RescueDying) { Answer(choice => choice.Parameters.GetValueOrDefault("response") == "let-die"); continue; }
                    throw new InvalidOperationException("Unexpected actual medicine boundary: " + Diagnostic());
                }
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Bounded actual medicine boundary not reached: " + Diagnostic());
        }
        private string State() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, G.PlayerCount).Select(viewer => SnapshotJson.Serialize(G.CreateSnapshot(viewer))),
            Frames = G.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType())),
            Facts = G.Events.Select(fact => JsonSerializer.Serialize(fact.Payload, fact.Payload.GetType())),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands)
        });
        public void Cold()
        { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == State(), "Cold replay preserves frozen medicine sources/amount/ordinal, typed recovery ownership, exact payments, children and private views."); }
        public void PureViews()
        {
            var before = State();
            for (var repeat = 0; repeat < 2; repeat++) for (var viewer = 0; viewer < G.PlayerCount; viewer++)
                Need(G.CreateSnapshot(viewer).Players.Where(player => player.Seat != viewer).All(player => player.Hand.Count == 0), "Medicine facts do not reveal another seat's private card identities.");
            Need(before == State(), "Reading medicine-bearing snapshots cannot advance or repay any recovery.");
        }
        public string Diagnostic() => JsonSerializer.Serialize(new
        {
            scene, Turn, V().Hp, V().MaxHp, V().IsAlive,
            Prompt = P is null ? null : new { P.Kind, Skill = P.SkillPrompt?.SkillId, ChoiceIds = P.Choices.Select(choice => choice.Id.Value) },
            Recovery = E<ActualTurnRecoveryRecordedEvent>(), Bills = E<RecoveryAmountBonusAppliedEvent>().Select(fact => new
                { fact.RecoveryFrameId, fact.Receipt.TargetSeat, fact.Receipt.BaseRequestedAmount, fact.Receipt.ModifiedRequestedAmount,
                    fact.Receipt.ActualTurnRecoveryOrdinal, Sources = fact.Receipt.Sources.Select(source => source.Source.SkillId) }),
            Frames = G.ResolutionStack.Select(frame => new { frame.Id, frame.Kind }),
            DriverResults = E<ProgramSkillResolvedEvent>().Where(fact => fact.SkillId == Driver)
                .Select(fact => new { fact.FrameId, fact.ActivationId, fact.Completed })
        });
        public static Scene Start(string scene)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(scene));
            var count = scene is "reaction" or "zaiqi" or "redirect" ? 2 : 4;
            var allied = scene is "paid-death" or "support";
            var hp = scene switch { "reaction" or "zaiqi" => 4, "source-boundary" => 6, "paid-loss" or "paid-death" => 3, "redirect" => 10, "support" => 20, _ => 1 };
            var allies = allied ? new ShanheCombatantDefinition[] { new(Peer(1)) } : [];
            var enemies = Enumerable.Range(allied ? 2 : 1, allied ? 2 : count - 1).Select(seat => new ShanheCombatantDefinition(Peer(seat))
            { QualifiedPrintedLordSkillIds = scene == "redirect" && seat == 1 ? [Jiuyuan] : null }).ToArray();
            ShanheStartingCard[] cards = scene == "stack" ? [new(CardKind.Peach, Suit.Heart, 3), new(CardKind.Peach, Suit.Heart, 4)] : [];
            var setup = new ShanheBattleSetup(83, 1, "fixture:yaoli-" + scene, Owner, hp, 0, 0, 0, 0, [], cards, [], enemies, allies, new string('b', 64));
            var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = count, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12,
                AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var result = new Scene(game, registry, scene); result.Accept(new StartGameCommand()); return result;
        }
    }

    private sealed class Fixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-yaoli", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            foreach (var id in Skills.Concat(new[] { Kuanggu, Blood, RecoveryDraw, Zaiqi, Jiuyuan, Yizheng })) b.AddSkill(Production.Value.GetSkill(id));
            var definitions = new List<Dictionary<string, object>>(); var labels = new Dictionary<string, object>();
            object Pause(string option) => new { op = "chooseOption", target = "owner", resultBind = option, options = new[] { new { id = option } } };
            object Effect(string op, int amount) => new { op, target = "owner", amount };
            object Activation(string id, params object[] effects) => new { id, minCards = 0, maxCards = 0, minTargets = 0,
                maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, effects };
            void Add(string id, Dictionary<string, object> body, params string[] options)
            {
                body["id"] = id; body["revision"] = 1; definitions.Add(body);
                labels[id] = new { name = id, description = "真实恢复、来源变化与父帧归返，无伪造状态或付款事实", optionLabels = options.ToDictionary(option => option, option => option) };
            }
            var activations = new List<object> { Activation("one", Effect("recover", 1), Pause("tail")), Activation("two", Effect("recover", 2), Pause("tail")),
                Activation("grant-second", new { op = "grantSkills", target = "owner", skillIds = new[] { Skills[1] } }),
                new { id = "damage", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                    effects = new object[] { new { op = "damage", target = "selectedTarget", amount = 1 }, Pause("tail") } } };
            foreach (var amount in new[] { 1, 2, 3, 5, 6, 8, 10 }) activations.Add(Activation("lose-" + amount, Effect("loseHp", amount)));
            var driver = new Dictionary<string, object> { ["activations"] = activations };
            if (scene.StartsWith("startup-", StringComparison.Ordinal) || scene.StartsWith("ended-", StringComparison.Ordinal))
                driver["triggers"] = new[] { new { id = "outside-real-turn", window = scene.StartsWith("startup-", StringComparison.Ordinal) ? "gameStarting" : "afterTurnEnded",
                    subject = "owner", optional = false, usageScope = "game", usageLimit = 1, effects = new[] { Effect("recover", 1) } } };
            if (scene == "support") driver["triggers"] = new[] { new { id = "after-real-yizheng", window = "turnEnding", subject = "owner", turnOwnerScope = "own",
                optional = false, priority = -100, usageScope = "game", usageLimit = 1,
                effects = new object[] { new { op = "selectTarget", target = "owner", targetKind = "otherLiving" }, new { op = "recover", target = "selectedTarget", amount = 1 }, Pause("support-tail") } } };
            Add(Driver, driver, scene == "support" ? ["tail", "support-tail"] : ["tail"]);
            Add(Health, new() { ["triggers"] = new[] { new { id = "real-positive-recovery", window = "afterHpRecovered", subject = "owner", hpChangeOccurrence = "perEvent",
                optional = false, priority = 100, effects = new[] { Pause("continue") } } } }, "continue");
            Add(Gain, new() { ["triggers"] = new[] { new { id = "real-native-draw-child", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                movementOccurrence = "perBatch", movementReasons = new[] { "skill-program.recovery-replacement.reward", "skill-program." + RecoveryDraw + ".Draw", "skill-program." + Kuanggu + ".Draw" },
                optional = false, effects = new[] { Pause("continue") } } } }, "continue");
            Add(Cost, new() { ["triggers"] = new[] { new { id = "real-support-cost-child", window = "afterHealthChanged", subject = "owner", optional = false,
                effects = new[] { Pause("continue") } } } }, "continue");
            if (scene is "source-boundary" or "paid-loss" or "paid-death") Add(Gate, new() { ["hpSkillGrants"] = new[]
                { new { id = "real-hp-dependent-medicine", skillId = Skills[1], minimumHp = scene == "source-boundary" ? 5 : 3 } } });
            if (scene is "paid-loss" or "paid-death")
            {
                var effects = new List<object> { Pause("paid"), Effect("loseHp", scene == "paid-loss" ? 3 : 20) };
                if (scene == "paid-loss") effects.Add(Pause("mutated"));
                Add(Mutation, new() { ["triggers"] = new[] { new { id = "real-paid-child", window = "afterHpRecovered", subject = "owner", hpChangeOccurrence = "perEvent",
                    optional = false, priority = 200, usageScope = "game", usageLimit = 1, effects = effects.ToArray() } } }, scene == "paid-loss" ? ["paid", "mutated"] : ["paid"]);
            }
            Add(Quiet, new() { ["triggers"] = new[] { new { id = "quiet-real-turn", window = "afterNormalDraw", subject = "owner", optional = false,
                effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } } } });
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            var own = new List<string> { Health, Gain };
            if (scene == "stack") own.AddRange(Skills);
            if (scene.EndsWith("limited", StringComparison.Ordinal)) own.Add(scene.EndsWith("unlimited", StringComparison.Ordinal) ? Skills[5] : Skills[0]);
            if (scene is "source-boundary" or "paid-loss" or "paid-death") own.Add(Gate);
            if (scene is "paid-loss" or "paid-death") own.Add(Mutation);
            if (scene is "reaction" or "zaiqi") own.AddRange([Skills[5], Blood, RecoveryDraw]);
            if (scene == "reaction") own.Add(Kuanggu);
            if (scene == "zaiqi") own.Add(Zaiqi);
            if (scene == "redirect") own.Add(Skills[0]);
            if (scene == "support") own.AddRange([Yizheng, Cost]);
            var maxHp = scene is "reaction" or "zaiqi" ? 12 : 20;
            b.AddGeneral(new(Owner, "药理真实支付者", "", Driver, "wu", maxHp, own));
            var count = scene is "reaction" or "zaiqi" or "redirect" ? 2 : 4;
            for (var seat = 1; seat < count; seat++) b.AddGeneral(new(Peer(seat), "药理真实同行或敌军", "", scene == "redirect" && seat == 1 ? Jiuyuan : Quiet,
                "wu", scene == "support" ? 12 : 20, scene == "redirect" && seat == 1 ? [Skills[1], Skills[5]] : scene == "support" && seat == 1 ? [Skills[5]] : [])
                { InitialHp = (scene is "redirect" or "support") && seat == 1 ? 1 : scene == "support" ? 12 : 20 });
            b.AddDeck(new(Deck, "固定实体牌堆", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 112).Select(_ => new ContentDeckPhysicalCard("standard:dodge", scene == "zaiqi" ? Suit.Heart : Suit.Spade, 7)).ToArray() });
            var allied = scene is "paid-death" or "support";
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "药理真实恢复", count, count, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: Enumerable.Range(1, count - 1).Select(Peer).Prepend(Owner).ToArray(), ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = allied ? 2 : 1, [ShanheBattleSetup.EnemyTeamId] = allied ? 2 : count - 1 }));
        }
    }
}
