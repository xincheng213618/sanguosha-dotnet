using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheHuangkongChecks
{
    private const string Huangkong = StandardShanheS5BossPackage.HuangkongId;
    private const string Tuxi = StandardShanheS5BossPackage.TuxiId;
    private const string Duipai = "shanhe:duipai-2026", Hongde = "ol:hongde";
    private const string Owner = "fixture:huangkong-owner", Ally = "fixture:huangkong-ally";
    private const string EnemyA = "fixture:huangkong-enemy-a", EnemyB = "fixture:huangkong-enemy-b";
    private const string Driver = "fixture:huangkong-driver", EndWatch = "fixture:huangkong-discard-end";
    private const string GainWatch = "fixture:huangkong-paid-gain", Restrict = "fixture:huangkong-restrict";
    private const string LoseHuangkong = "fixture:huangkong-real-dying-loss";
    private const string Skip = "fixture:huangkong-skip", Quiet = "fixture:huangkong-quiet", Deck = "fixture:huangkong-deck";
    private static readonly Lazy<ContentRegistry> Production = new(() => StandardShanheCampaignCatalog.CreateBattleRegistry(
        new(83, 1, "fixture-huangkong-production", StandardShanheS5BossPackage.ZhangliaoHuangkongId,
            4, 0, 0, 0, 0, [], [], [], [new("standard:cao-cao")], [], new string('b', 64))));

    public static void DoubledDiscardQuotaPaysFourWithoutChangingHandLimitAndResumesChildrenCold()
    {
        var s = Scene.Start(new("human-discard") { Hand = Cards(6), WatchOwnEnd = true }); s.Play();
        Need(s.V().HandCount == 6 && s.V().HandLimit == 4, "The real six-card hand still has the ordinary four-card limit.");
        s.EndPlay(); s.Reach(() => s.P is { Kind: DecisionKind.DiscardCards, PlayerSeat: 0 });
        Need(s.P!.RequiredCardCount == 4 && s.P.Prompt.Contains("手牌上限为 4") && s.P.ValidCardIds.Count == 6 && s.V().HandLimit == 4,
            "Huangkong owes (six minus four) times two actual discards, rather than changing the hand limit to two.");
        var chosen = s.P.ValidCardIds.Take(4).Order().ToArray(); s.Cold();
        foreach (var invalid in new[] { Array.Empty<int>(), new[] { chosen[0], chosen[0] }, s.P.ValidCardIds.Take(5).ToArray() })
        {
            var before = s.State(); var result = s.G.Submit(new DiscardCardsCommand(0, invalid, s.P!.PromptId, s.G.Revision));
            Need(!result.Accepted && result.Error?.Code == CommandErrorCode.InvalidChoice && s.State() == before,
                "Illegal doubled-discard quantities are rejected atomically without paying, advancing or replacing the real prompt.");
        }
        s.Discard(chosen); s.Reach(() => s.P?.SkillPrompt?.SkillId == EndWatch);
        Need(s.V().HandCount == 2 && s.Events<HandLimitDiscardedEvent>().Single() is { ActorSeat: 0 } discarded &&
            discarded.CardIds.SequenceEqual(chosen) && s.HandLimitPayments(0).Select(m => m.CardId).Order().SequenceEqual(chosen) &&
            s.Events<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.Reason == CardMoveReasons.HandLimitDiscard)
                .All(e => e is { ExecutorSeat: 0, ActualCount: 4, HandCount: 4 }),
            "The published four-card obligation pays the exact original entities once and enters the real discard-phase-end child with two cards remaining.");
        s.Cold(); s.Continue(); s.Reach(() => s.P?.SkillPrompt?.SkillId == EndWatch);
        Need(s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "paid") && s.V().HandCount == 3 &&
            s.V().Hp == 1 && s.V().Skills?.Any(skill => skill.ContentId == Huangkong) != true &&
            s.Events<ProgramOwnerSkillsReplacedEvent>().Length == 1 &&
            s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) &&
                m.Reason.Value.Contains(EndWatch, StringComparison.Ordinal)) == 1,
            "The real phase-end child draws, enters dying, genuinely recovers and loses Huangkong; neither the new one-HP limit nor the now-default multiplier reopens its already-paid four-card obligation.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "paid");
        s.Reach(() => s.G.State.CurrentSeat != 0);
        Need(s.HandLimitPayments(0).Length == 4 && s.Events<HandLimitDiscardedEvent>().Length == 1 && s.V().HandCount == 3,
            "Returning through a paid child and cold replay neither repeats the discard nor demands the normal two-card alternative."); s.Cold();
    }

    public static void PartialDiscardKeepsFrozenDoubledQuotaCold()
    {
        var s = Scene.Start(new("human-discard") { Hand = Cards(6), WatchOwnEnd = true }); s.Play();
        s.EndPlay(); s.Reach(() => s.P is { Kind: DecisionKind.DiscardCards, PlayerSeat: 0 });
        var chosen = s.P!.ValidCardIds.Take(4).ToArray();
        s.Discard([chosen[0]]); s.Cold();
        Need(s.P is { Kind: DecisionKind.DiscardCards, RequiredCardCount: 3 } && s.V().HandCount == 5,
            "A one-card payment leaves three of the frozen four-card quota, even though recalculating the multiplier would demand two.");
        s.Discard([chosen[1]]); s.Cold();
        Need(s.P is { Kind: DecisionKind.DiscardCards, RequiredCardCount: 2 } && s.V().HandCount == 4,
            "Reaching the ordinary hand limit must not forgive the remaining doubled-discard debt.");
        s.Discard(chosen.Skip(2).ToArray()); s.Reach(() => s.P?.SkillPrompt?.SkillId == EndWatch);
        Need(s.HandLimitPayments(0).Length == 4 && s.V().HandCount == 2 &&
            s.Events<HandLimitDiscardedEvent>().SelectMany(e => e.CardIds).Order().SequenceEqual(chosen.Order()),
            "Paying the exact remaining debt enters the native phase-end child once without losing or repeating cards.");
        s.Cold();
    }

    private static ShanheStartingCard[] Cards(int count) => Enumerable.Range(0, count).Select(i => new ShanheStartingCard(CardKind.Dodge, Suit.Club, i % 13 + 1)).ToArray();
    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
    private sealed record Case(string Name)
    {
        public int BaseDraw { get; init; }
        public IReadOnlyList<string> Skills { get; init; } = [Huangkong];
        public IReadOnlyList<string> AllySkills { get; init; } = [];
        public IReadOnlyList<ShanheStartingCard> Hand { get; init; } = [];
        public IReadOnlyList<ShanheStartingCard> AllyHand { get; init; } = Cards(1);
        public bool SkipDraw { get; init; }
        public bool SkipDiscard { get; init; }
        public bool WatchOwnEnd { get; init; }
        public bool WatchForeignEnd { get; init; }
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardMovementRecord[] PhaseDraws() => G.CardMovements.Where(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason == CardMoveReasons.Draw).ToArray();
        public CardMovementRecord[] HandLimitPayments(int seat) => G.CardMovements.Where(m => m.From == CardLocation.Hand(seat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.HandLimitDiscard).ToArray();
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "A genuine Huangkong command was rejected."); }
        public void Answer(Func<PromptChoice, bool> choose) => Accept(new AnswerPromptCommand(0, P!.PromptId, P.Choices.First(choose).Id, G.Revision));
        public void Activate() => Answer(c => Action(c) == "activate");
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void AtTuxi() => Reach(() => P?.SkillPrompt?.SkillId == Tuxi);
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Use(string id) => Accept(new UseProgramSkillCommand(0, Driver, id, [], [], G.Revision, P!.PromptId));
        public void EndPlay() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Discard(int[] ids) => Accept(new DiscardCardsCommand(0, ids, P!.PromptId, G.Revision));
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The bounded Huangkong fixture must not finish a match.");
                if (P?.PlayerSeat == 0)
                {
                    if (P.SkillPrompt?.SkillId is "classic:yingzi" or "classic:luoyi" or "boundary:luoyi") Activate();
                    else throw new InvalidOperationException("Unexpected Huangkong boundary: " + JsonSerializer.Serialize(new
                    { P.Kind, P.PlayerSeat, P.Prompt, Skill = P.SkillPrompt?.SkillId, Hand = V().HandCount,
                        Draws = Events<NativeDrawInvocationRecordedEvent>().Select(e => new { e.OwnerSeat, e.Requested, e.ActualCount, e.Reason }),
                        Adjustments = Events<ProgramNormalDrawAdjustedEvent>(), Frames = G.ResolutionStack.Select(f => new { f.Id, f.Kind }) }));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded Huangkong continuation.");
        }
        public string State() => JsonSerializer.Serialize(new
        { Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat))),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())),
            Facts = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), G.CardMovements,
            Zones = G.CreateCardZoneDiagnostics(), Checkpoint = GameCheckpointJson.Serialize(G.CreateCheckpoint()) });
        public void Cold()
        {
            var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(State() == before, "Cold replay preserves the exact planned draw, native theft/discard payments, private hands and typed child returns.");
            var cards = G.CreateCardZoneDiagnostics(); Need(cards.Select(c => c.CardId).Distinct().Count() == cards.Count, "All real cards occupy exactly one zone.");
        }
        public static Scene Start(Case c)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(c));
            var skills = c.Skills.Concat(c.WatchOwnEnd || c.WatchForeignEnd ? [EndWatch] : Array.Empty<string>())
                .Concat(c.WatchOwnEnd ? [LoseHuangkong] : Array.Empty<string>())
                .Concat(c.SkipDraw || c.SkipDiscard ? [Skip] : Array.Empty<string>()).ToArray();
            var setup = new ShanheBattleSetup(83, 1, "fixture-huangkong-" + c.Name, Owner, 4, 0, 0, 0, 0,
                skills, c.Hand, [], [new(EnemyA) { StartingHand = Cards(1) }, new(EnemyB) { StartingHand = Cards(1) }],
                [new(Ally, ExtraSkillIds: c.AllySkills) { StartingHand = c.AllyHand }], new string('b', 64));
            var g = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false,
                MaxTurns = 3, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var s = new Scene(g, registry); s.Accept(new StartGameCommand()); return s;
        }
    }

    private sealed class Fixture(Case c) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-huangkong", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            // The ordinary package already registers the migrated standard
            // Yingzi program. Keep that exact definition instead of copying it.
            foreach (var id in new[] { Huangkong, Tuxi, Duipai, "boundary:yingzi", "classic:yingzi", "classic:luoyi", "boundary:luoyi", Hongde })
                b.AddSkill(Production.Value.GetSkill(id));
            object Pause(string bind, string option) => new { op = "chooseOption", target = "owner", resultBind = bind, options = new[] { new { id = option } } };
            object Activation(string id, params object[] effects) => new
            { id, minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, effects };
            var definitions = new List<object>
            {
                new { id = Driver, revision = 1, activations = new[] {
                    Activation("exempt", new { op = "grantTurnHandLimitCardKindExemption", target = "owner", cardKinds = new[] { "slash" } }),
                    Activation("self-hit", new { op = "damage", target = "owner", sourceRef = new { kind = "owner" }, amount = 1 }, new { op = "recover", target = "owner", amount = 1 }) } },
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "skip-play", window = "afterNormalDraw", subject = "owner", optional = false,
                    effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } } } },
                new { id = GainWatch, revision = 1, triggers = new[] { new { id = "actual-hongde-payment", window = "cardsGained", subject = "owner",
                    movementOccurrence = "perBatch", destinationZones = new[] { "hand" }, movementReasons = new[] { "program.bu-zhi.draw" },
                    optional = false, usageScope = "game", usageLimit = 1, effects = new[] { Pause("paid-gain", "continue") } } } },
                new { id = Restrict, revision = 1, triggers = new[] { new { id = "protect-actual-source", window = "afterDamageApplied", subject = "owner",
                    requireDamageSource = true, damageOccurrence = "perDamage", optional = true, effects = new object[] {
                        new { op = "chooseOption", target = "owner", resultBind = "category", options = new[] { new { id = "basic" }, new { id = "trick" }, new { id = "equipment" } } },
                        new { op = "restrictDamageSourceHandCategory", target = "owner", cardCategories = new[] { "basic" }, condition = new { kind = "choiceIs", sourceBind = "category", optionId = "basic" } },
                        new { op = "restrictDamageSourceHandCategory", target = "owner", cardCategories = new[] { "trick" }, condition = new { kind = "choiceIs", sourceBind = "category", optionId = "trick" } },
                        new { op = "restrictDamageSourceHandCategory", target = "owner", cardCategories = new[] { "equipment" }, condition = new { kind = "choiceIs", sourceBind = "category", optionId = "equipment" } } } } } }
            };
            if (c.WatchOwnEnd) definitions.Add(new { id = EndWatch, revision = 1, triggers = new[] { new { id = "real-own-discard-end", window = "discardPhaseEnded",
                subject = "owner", turnOwnerScope = "own", allowOwnDiscardPhaseEnded = true, optional = false, usageScope = "game", usageLimit = 1,
                effects = new object[] { Pause("before-child", "continue"), new { op = "draw", target = "owner", amount = 1 },
                    new { op = "loseHp", target = "owner", amount = 4 }, Pause("after-child", "paid") } } } });
            if (c.WatchOwnEnd) definitions.Add(new { id = LoseHuangkong, revision = 1, triggers = new[] { new { id = "legal-dying-loss", window = "dyingEntering",
                subject = "owner", optional = false, usageScope = "game", usageLimit = 1, effects = new object[] {
                    new { op = "recover", target = "owner", amount = 1 },
                    new { op = "loseOwnerSkillsAndGrant", target = "owner", skillIds = new[] { Huangkong }, sourceBind = "standard:none" } } } } });
            if (c.WatchForeignEnd) definitions.Add(new { id = EndWatch, revision = 1, triggers = new[] { new { id = "real-npc-discard-end", window = "discardPhaseEnded",
                subject = "owner", turnOwnerScope = "otherLiving", optional = false, usageScope = "game", usageLimit = 1, effects = new[] { Pause("after-npc-payment", "continue") } } } });
            if (c.SkipDraw || c.SkipDiscard) definitions.Add(new { id = Skip, revision = 1, triggers = new[] { new { id = "actual-skip", window = c.SkipDraw ? "turnStartBeforeNormalFlow" : "discardPhaseStarting",
                subject = "owner", optional = false, effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { c.SkipDraw ? "draw" : "discard" } } } } } });
            var labels = new Dictionary<string, object>
            {
                [Driver] = new { name = "真实惶恐测试操作", description = "真实实体、伤害、恢复和手牌策略" },
                [Quiet] = new { name = "只跳过NPC出牌", description = "保留实际摸牌和弃牌阶段" },
                [GainWatch] = new { name = "实际弘德受益子窗口", description = "实际支付后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                [Restrict] = new { name = "真实来源类别保护", description = "使用既有合法伤害来源类别限制", optionLabels = new Dictionary<string, string> { ["basic"] = "基本", ["trick"] = "锦囊", ["equipment"] = "装备" } }
            };
            if (c.WatchOwnEnd || c.WatchForeignEnd) labels[EndWatch] = c.WatchOwnEnd
                ? new { name = "真实弃牌结束子窗口", description = "已支付弃牌后真正摸一张", optionLabels = new Dictionary<string, string> { ["continue"] = "继续", ["paid"] = "已支付" } }
                : new { name = "真实弃牌结束子窗口", description = "已支付NPC弃牌后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            if (c.SkipDraw || c.SkipDiscard) labels[Skip] = new { name = "真实阶段跳过", description = "使用既有实际阶段策略" };
            if (c.WatchOwnEnd) labels[LoseHuangkong] = new { name = "真实濒死失去惶恐", description = "只在合法濒死窗口回复一点并终止式失去惶恐" };
            var programs = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in programs.Programs) b.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description)
                { Program = program, ProgramPresentation = programs.Presentations[id] });
            b.AddGeneral(new(Owner, "真实惶恐拥有者", "", Driver, "wei", 4));
            foreach (var id in new[] { Ally, EnemyA, EnemyB }) b.AddGeneral(new(id, "真实其他参战者", "", Quiet, "qun", 4));
            b.AddDeck(new(Deck, "固定实体摸牌堆", 0, c.BaseDraw, [])
                { PhysicalCards = Enumerable.Range(0, 80).Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, i % 13 + 1)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "真实惶恐计划和支付", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Ally, EnemyA, EnemyB], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
