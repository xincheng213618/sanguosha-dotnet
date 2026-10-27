using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheLingdongChecks
{
    private const string Lingdong = "shanhe:s5-lingdong-14045", Guicai = "classic:guicai", Tiandu = "classic:tiandu";
    private const string Jueqing = "classic:jueqing", Xiaoming = "shanhe:xiaoming", Reason = "shanhe.s5.lingdong";
    private const string Hongyan = "classic:hongyan", LoseColor = "fixture:lingdong-lose-color";
    private const string Driver = "fixture:lingdong-driver", Watch = "fixture:lingdong-damage-child", ClaimWatch = "fixture:lingdong-claim-child";
    private const string Owner = "fixture:lingdong-owner", Ally = "fixture:lingdong-ally", EnemyA = "fixture:lingdong-enemy-a";
    private const string EnemyB = "fixture:lingdong-enemy-b", Deck = "fixture:lingdong-deck";
    private static readonly Lazy<ContentRegistry> Production = new(() => StandardShanheCampaignCatalog.CreateBattleRegistry(
        new(83, 1, "fixture-lingdong-production", StandardShanheS5BossPackage.ZhangliaoLingdongId,
            4, 0, 0, 0, 0, [], [], [], [new("standard:cao-cao")], [], new string('a', 64))));

    public static void NativeTianduKeepsClaimedEntityWhileReplacementsBypassAndRealDamageJudgesCold()
    {
        var s = Scene.Start("tiandu"); s.Use("armor-three", [0]);
        s.Begin("incoming", [2]);
        s.Reach(() => s.P is { Kind: DecisionKind.ProgramJudgmentTrigger, PlayerSeat: 0 } &&
            s.G.ResolutionStack.LastOrDefault() is ProgramJudgmentTriggerWindowFrame window &&
            window.Candidates[window.CandidateIndex].SkillId == Tiandu);
        var final = s.Events<JudgmentResolvedEvent>().Single(e => e.Reason == Reason);
        Need(final is { CardId: not null, Suit: Suit.Heart } &&
            s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == final.CardId).Location == CardLocation.Judgment(0),
            "Native Tiandu sees the original finalized red judgment card in its actual judgment zone.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("action") == "program-judgment-trigger-activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == ClaimWatch);
        var claim = s.Events<ProgramJudgmentCardClaimedEvent>().Single();
        Need(claim.SkillId == Tiandu && claim.CardId == final.CardId && claim.JudgmentFrameId == final.ResolutionId &&
            s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == claim.CardId).Location == CardLocation.Hand(0) &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == Tiandu && frame.PendingMovementContinuation is not null),
            "The real claim pays the exact judged entity into hand and pauses in a genuine CardsGained child owned by Tiandu.");
        s.Cold(); s.Continue(); s.Play();
        var outcome = s.Events<ProgramJudgmentOutcomeRecordedEvent>().Single().Outcome;
        Need(outcome.CardId == claim.CardId && outcome.Suit == Suit.Heart &&
            s.V() is { Hp: 12, Armor: 3, HandCount: 1 } && s.V().Hand.Single().Id == claim.CardId &&
            s.Events<ProgramDamagePreventedEvent>() is [{ SkillId: Lingdong, TargetSeat: 0, Amount: 2 }] &&
            s.Events<DamageAppliedEvent>().Length == 0 && s.Events<AfterDamageEvent>().Length == 0 &&
            s.Events<ProgramJudgmentCardClaimedEvent>().Length == 1 &&
            s.G.CardMovements.Count(move => move.CardId == claim.CardId && move.From == CardLocation.Judgment(0) && move.To == CardLocation.Hand(0)) == 1 &&
            !s.G.CardMovements.Any(move => move.CardId == claim.CardId && move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile),
            "The frozen final red result still prevents damage after Tiandu claims it; cleanup never steals or discards the claimed entity, including after cold child replay.");
        s.Cold();
        VerifyClaimedCardRelocationAndLostHongyanKeepFinalRed();

        var j = Scene.Start("jueqing"); j.Use("armor-three", [3]); j.Use("lose-two", [3]); j.PlayCard(CardKind.Slash, [3]);
        Need(j.V(3) is { Hp: 9, Armor: 3 } && j.Events<ProgramSkillHpLostEvent>() is [{ TargetSeat: 3, Amount: 2 }] &&
            j.Events<DamageReplacedWithHpLossEvent>() is [{ TargetSeat: 3, Amount: 1 }] &&
            j.Events<JudgmentRequestedEvent>().Length == 0 && j.Events<ProgramJudgmentOutcomeRecordedEvent>().Length == 0 &&
            j.Events<DamageAppliedEvent>().Length == 0,
            "Real loseHp and the real native Jueqing Slash replacement bypass Lingdong and Armor without manufacturing damage or judgment.");
        j.Cold();
        var x = Scene.Start("maximum"); x.Use("armor-three", [3]); x.PlayCard(CardKind.Slash, [3]);
        Need(x.V(3) is { Hp: 11, MaxHp: 11, Armor: 3 } &&
            x.Events<CardDamageReplacedWithMaximumHpLossEvent>() is [{ Receipt.TargetSeat: 3, Receipt.Amount: 1 }] &&
            x.Events<JudgmentRequestedEvent>().Length == 0 && x.Events<ProgramJudgmentOutcomeRecordedEvent>().Length == 0 &&
            x.Events<DamageAppliedEvent>().Length == 0,
            "The genuine first-Slash maximum-HP replacement resolves before any Lingdong judgment and leaves Armor unspent.");
        x.Cold();
        var program = Scene.Start("program"); program.Use("incoming", [2]); program.Cold(); program.Use("source-less");
        Need(program.Events<JudgmentRequestedEvent>().Where(e => e.Reason == Reason).Count() == 2 &&
            program.Events<ProgramJudgmentOutcomeRecordedEvent>().Length == 2 &&
            program.Events<DamageAppliedEvent>() is [{ SourceSeat: 2, TargetSeat: 0, Amount: 2, SourceLess: false },
                { TargetSeat: 0, Amount: 2, SourceLess: true }] && program.V().Hp == 8 &&
            program.Events<AfterDamageEvent>().Select(e => e.Amount).SequenceEqual([2, 2]),
            "Actual program damage and source-less damage each start their own real black Lingdong judgment and complete their native damage-after flow once.");
        program.Cold();
        var sourceLessRed = Scene.Start("black-red"); sourceLessRed.Use("armor-three", [0]);
        var redPayment = sourceLessRed.V().Hand.Single().Id;
        sourceLessRed.Begin("source-less"); sourceLessRed.AtGuicai(0); sourceLessRed.Cold();
        sourceLessRed.Answer(c => c.Cards.SequenceEqual([redPayment])); sourceLessRed.Play();
        Need(sourceLessRed.Events<ProgramDamagePreventedEvent>() is
                [{ SkillId: Lingdong, SourceSeat: null, TargetSeat: 0, Amount: 2 }] &&
            sourceLessRed.Events<ProgramJudgmentOutcomeRecordedEvent>() is
                [{ Outcome: { CardId: var finalRed, Suit: Suit.Heart } }] && finalRed == redPayment &&
            sourceLessRed.Events<JudgmentResolvedEvent>() is [{ CardId: var judgedRed, Suit: Suit.Heart }] && judgedRed == redPayment &&
            sourceLessRed.V() is { Hp: 12, Armor: 3 } && sourceLessRed.Events<DamageAppliedEvent>().Length == 0 &&
            sourceLessRed.Events<AfterDamageEvent>().Length == 0 &&
            sourceLessRed.G.CreateCardZoneDiagnostics().Single(c => c.CardId == redPayment).Location == CardLocation.DiscardPile &&
            sourceLessRed.G.CardMovements.Count(move => move.CardId == redPayment && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1,
            "A genuine source-less damage attempt can be prevented by the real paid red Guicai judgment; its prevention keeps a null source, spends no Armor/HP and cleans the exact replacement entity once.");
        sourceLessRed.Cold();
    }

    private static void VerifyClaimedCardRelocationAndLostHongyanKeepFinalRed()
    {
        var s = Scene.Start("tiandu-relocated"); s.Use("armor-three", [0]); s.Begin("incoming", [2]);
        s.Reach(() => s.P is { Kind: DecisionKind.ProgramJudgmentTrigger, PlayerSeat: 0 } &&
            s.G.ResolutionStack.LastOrDefault() is ProgramJudgmentTriggerWindowFrame window &&
            window.Candidates[window.CandidateIndex].SkillId == Tiandu);
        var judged = s.Events<JudgmentResolvedEvent>().Single(e => e.Reason == Reason);
        Need(judged is { Suit: Suit.Heart, CardId: not null } && s.V().Skills!.Any(skill => skill.ContentId == Hongyan),
            "Real Hongyan makes the fixed printed Spade judgment finally Heart before Tiandu takes its original entity.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("action") == "program-judgment-trigger-activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == ClaimWatch);
        var claimed = s.Events<ProgramJudgmentCardClaimedEvent>().Single();
        Need(claimed.CardId == judged.CardId && s.V().Hand.Single().Id == claimed.CardId,
            "The nested disposal starts only after native Tiandu has actually paid the exact finalized judgment card into hand.");
        s.Cold(); s.Continue();
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        s.Cold(); s.Answer(c => c.Cards.SequenceEqual([claimed.CardId]));
        if (s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards") == true)
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == ClaimWatch && s.V().Skills?.Any(skill => skill.ContentId == Hongyan) != true);
        Need(s.V() is { Hp: 1, Armor: 3, HandCount: 0 } &&
            s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == claimed.CardId).Location == CardLocation.DiscardPile &&
            s.Events<ProgramOwnerSkillsReplacedEvent>() is [{ LostSkillIds: var lost }] && lost.SequenceEqual([Hongyan]) &&
            s.G.ResolutionStack.OfType<JudgmentFrame>().Single().Id == judged.ResolutionId,
            "The claimed card really leaves hand, a legal dying-entry recovery removes Hongyan, and the original finalized judgment remains suspended over its exact native descendants.");
        s.Cold(); s.Continue(); s.Play();
        Need(s.Events<ProgramJudgmentOutcomeRecordedEvent>() is [{ Outcome: { CardId: var cardId, Suit: Suit.Heart } }] && cardId == claimed.CardId &&
            s.Events<ProgramDamagePreventedEvent>() is [{ SkillId: Lingdong, TargetSeat: 0, Amount: 2 }] &&
            s.Events<DamageAppliedEvent>().Length == 0 && s.V() is { Hp: 1, Armor: 3, HandCount: 0 } &&
            s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == claimed.CardId).Location == CardLocation.DiscardPile &&
            s.Events<ProgramJudgmentCardClaimedEvent>().Length == 1 &&
            s.G.CardMovements.Count(move => move.CardId == claimed.CardId && move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile) == 1,
            "The old finalized Heart outcome survives actual card relocation and loss of Hongyan; Lingdong prevents damage without reclaiming the entity or repeating the nested discard/recovery.");
        s.Cold();
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "Lingdong rejected a genuine native command."); }
        public void Begin(string id, int[]? targets = null) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets ?? [], G.Revision, P!.PromptId));
        public void Use(string id, int[]? targets = null) { Begin(id, targets); Play(); }
        public void Answer(Func<PromptChoice, bool> choose) => Accept(new AnswerPromptCommand(0, P!.PromptId, P.Choices.First(choose).Id, G.Revision));
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void SkipGuicai() => Answer(c => c.Parameters.GetValueOrDefault("action") == "program-judgment-replace-skip");
        public void AtGuicai(int target) => Reach(() => P is { Kind: DecisionKind.ProgramJudgmentReplacement } &&
            G.ResolutionStack.OfType<JudgmentFrame>().Single().TargetSeat == target);
        public void AtDamageChild() => Reach(() => P?.SkillPrompt?.SkillId == Watch);
        public void BeginCard(CardKind kind, int[] targets)
        {
            var action = G.GetHumanLegalActions().First(a => a.CardId is { } id && V().Hand.Any(c => c.Id == id && c.Kind == kind) &&
                a.ConversionSource is null && a.Kind != LegalActionKind.Recast && a.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
        }
        public void PlayCard(CardKind kind, int[] targets) { BeginCard(kind, targets); Play(); }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The bounded Lingdong fixture must not finish a match.");
                if (P?.PlayerSeat == 0)
                {
                    if (P.SkillPrompt?.SkillId is Watch or ClaimWatch) Continue();
                    else throw new InvalidOperationException("Unexpected Lingdong native boundary: " + Diagnostic());
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded Lingdong continuation: " + Diagnostic());
        }
        private string Diagnostic()
        {
            var attack = G.ResolutionStack.OfType<CardUseFrame>().LastOrDefault()?.CardAttack;
            return JsonSerializer.Serialize(new
            {
                Prompt = P is null ? null : new { P.Kind, P.PlayerSeat, Skill = P.SkillPrompt?.SkillId },
                Seats = Enumerable.Range(0, 4).Select(seat => new { Seat = seat, V(seat).Hp, V(seat).Armor, V(seat).IsChained }),
                Judgments = Events<JudgmentResolvedEvent>(),
                Outcomes = Events<ProgramJudgmentOutcomeRecordedEvent>().Select(e => new { e.ProgramFrameId, e.Outcome.TargetSeat, e.Outcome.CardId, e.Outcome.Suit }),
                Damage = Events<DamageAppliedEvent>().Select(e => new { e.SourceSeat, e.TargetSeat, e.Amount, e.Nature, e.ArmorPayment }),
                Prevention = Events<ProgramDamagePreventedEvent>(), Chains = Events<ChainedDamagePropagatedEvent>(),
                ChainStates = Events<IronChainStateChangedEvent>(),
                Attack = attack is null ? null : new { attack.TargetSeat, attack.DamageAmount, attack.DamageAmountFinalized,
                    attack.ChainedTargetSeats, attack.ChainedTargetIndex, attack.IsChainPropagation, attack.BeforeDamageProgramsResolved,
                    attack.RangePreventionVisitedTargets },
                Frames = G.ResolutionStack.Select(frame => new { frame.Id, frame.Kind })
            });
        }
        private string State() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat))),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())),
            Facts = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), G.CardMovements,
            Zones = G.CreateCardZoneDiagnostics(), Checkpoint = GameCheckpointJson.Serialize(G.CreateCheckpoint())
        });
        public void Cold()
        {
            var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(State() == before, "Cold command replay preserves every actual judgment, outcome, native child, Armor receipt, private view and physical entity exactly.");
            var zones = G.CreateCardZoneDiagnostics();
            Need(zones.Select(c => c.CardId).Distinct().Count() == zones.Count, "Every actual Lingdong card still occupies exactly one zone.");
        }
        public static Scene Start(string scene)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(scene));
            var own = new List<string> { Watch };
            if (scene is "black-red" or "red-black" or "tiandu" or "tiandu-relocated" or "program" or "empty") own.Add(Lingdong);
            if (scene is "black-red" or "red-black" or "chain" or "chain-origin-only") own.Add(Guicai);
            if (scene is "tiandu" or "tiandu-relocated") own.AddRange([Tiandu, ClaimWatch]);
            if (scene == "tiandu-relocated") own.AddRange([Hongyan, LoseColor]);
            if (scene == "jueqing") own.Add(Jueqing);
            var targetSkills = scene is "chain" or "jueqing" or "maximum" ? new[] { Lingdong } : [];
            ShanheStartingCard[] cards = scene switch
            {
                "black-red" => [new(CardKind.Dodge, Suit.Heart, 11)],
                "red-black" => [new(CardKind.Dodge, Suit.Spade, 11)],
                "chain" or "chain-origin-only" => [new(CardKind.IronChain, Suit.Club, 6), new(CardKind.IronChain, Suit.Spade, 8),
                    new(CardKind.Alcohol, Suit.Spade, 9), new(CardKind.ThunderSlash, Suit.Spade, 7),
                    new(CardKind.Dodge, Suit.Heart, 11), new(CardKind.Dodge, Suit.Club, 12)],
                "jueqing" or "maximum" => [new(CardKind.Slash, Suit.Spade, 6)], _ => []
            };
            var setup = new ShanheBattleSetup(83, 1, "fixture-lingdong-" + scene, Owner, 12, 0, 0, 0, 3, own, cards, [],
                [new(EnemyA, ExtraSkillIds: targetSkills), new(EnemyB, ExtraSkillIds: targetSkills)],
                [new(Ally, ExtraSkillIds: scene == "chain" ? targetSkills : scene == "chain-origin-only" ? [Lingdong] : [])], new string('c', 64))
            { TacticSkillIds = scene == "maximum" ? [Xiaoming] : [] };
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 3, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var s = new Scene(game, registry); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-lingdong", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            foreach (var id in new[] { Lingdong, Guicai, Tiandu, Jueqing, Xiaoming, Hongyan }) b.AddSkill(Production.Value.GetSkill(id));
            object Activation(string id, int targets, params object[] effects) => new
            { id, minCards = 0, maxCards = 0, minTargets = targets, maxTargets = targets, targetKind = "anyLiving", usesPerTurn = (int?)null, effects };
            object Pause(string bind = "child") => new { op = "chooseOption", target = "owner", resultBind = bind, options = new[] { new { id = "continue" } } };
            var claimEffects = new List<object> { Pause() };
            if (scene == "tiandu-relocated") claimEffects.AddRange([
                new { op = "selectOwnedCards", target = "owner", amount = 1, zones = new[] { "hand" }, resultBind = "claimed-card" },
                new { op = "moveBoundCards", target = "owner", sourceBind = "claimed-card", destination = "discardPile", awaitMovementTriggers = true },
                new { op = "loseHp", target = "owner", amount = 12 }, Pause("after-loss")]);
            var definitions = new List<object>
            {
                new { id = Driver, revision = 1, activations = new[]
                {
                    Activation("incoming", 1, new { op = "damage", target = "owner", sourceRef = new { kind = "selectedTarget" }, amount = 2 }),
                    Activation("source-less", 0, new { op = "receiveOwnerDamage", target = "owner", amount = 2 }),
                    Activation("lose-two", 1, new { op = "loseHp", target = "selectedTarget", amount = 2 }),
                    Activation("armor-one", 1, new { op = "grantArmor", target = "selectedTarget", amount = 1 }),
                    Activation("armor-three", 1, new { op = "grantArmor", target = "selectedTarget", amount = 3 })
                } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "paid-damage", window = "afterDamageApplied", subject = scene == "chain-origin-only" ? "any" : "owner",
                    damageOccurrence = "perDamage", optional = false, effects = new[] { Pause() } } } },
                new { id = ClaimWatch, revision = 1, triggers = new[] { new { id = "paid-tiandu", window = "cardsGained", subject = "owner",
                    destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { "skill-program.classic:tiandu.claimJudgmentCard" },
                    optional = false, usageScope = "game", usageLimit = 1, effects = claimEffects.ToArray() } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实灵动测试操作", description = "仅调用原生伤害、护甲和失去体力指令" } };
            if (scene == "tiandu-relocated")
            {
                definitions.Add(new { id = LoseColor, revision = 1, triggers = new[] { new { id = "real-loss-of-hongyan", window = "dyingEntering", subject = "owner",
                    optional = false, usageScope = "game", usageLimit = 1, effects = new object[] {
                        new { op = "recover", target = "owner", amount = 1 },
                        new { op = "loseOwnerSkillsAndGrant", target = "owner", skillIds = new[] { Hongyan }, sourceBind = "standard:none" } } } } });
                labels[LoseColor] = new { name = "真实失去红颜", description = "只在合法濒死窗口回复一点并终止式移除红颜" };
            }
            foreach (var id in new[] { Watch, ClaimWatch }) labels[id] = new { name = "真实已支付子流程", description = "在原生支付完成后暂停",
                optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Owner, "真实判定拥有者", "", Driver, "shu", 12));
            foreach (var id in new[] { Ally, EnemyA, EnemyB }) b.AddGeneral(new(id, "实际参战角色", "", "standard:none", "qun", 12));
            b.AddDeck(new(Deck, "固定同色实体判定牌堆", scene == "empty" ? 1 : 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, scene == "empty" ? 4 : 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", scene is "red-black" or "tiandu" or "jueqing" or "maximum" ? Suit.Heart : Suit.Spade, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "真实灵动结算", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Ally, EnemyA, EnemyB], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
