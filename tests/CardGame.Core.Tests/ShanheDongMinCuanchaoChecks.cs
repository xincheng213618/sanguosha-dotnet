using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDongMinCuanchaoChecks
{
    private const string Skill = "shanhe:current-cuanchao-14069", Cap = "shanhe:current-dungong-3387";
    private const string Boss = "shanhe:current-dongmin-dungong-cuanchao", Jizhi = "classic:jizhi", Fengzi = "ol:fengzi";
    private const string Owner = "fixture:cuanchao-owner", Peer = "fixture:cuanchao-peer", Source = "fixture:cuanchao-source";
    private const string Driver = "fixture:cuanchao-driver", Quiet = "fixture:cuanchao-quiet", Attack = "fixture:cuanchao-real-source";
    private const string DamageWatch = "fixture:cuanchao-paid-damage", RecoverWatch = "fixture:cuanchao-paid-recovery";
    private const string Suppress = "fixture:cuanchao-suppression", Deck = "fixture:cuanchao-deck";
    private const string Independent = "fixture:cuanchao-independent-conversion", HpGate = "fixture:cuanchao-hp-gate", PaidLoss = "fixture:cuanchao-paid-loss";
    private static readonly Lazy<ContentRegistry> Production = new(() => StandardShanheCampaignCatalog.CreateBattleRegistry(
        new(83, 1, "fixture-cuanchao-production", "classic:guan-yu", 4, 0, 0, 0, 0, [], [], [], [new(Boss)], [], new string('f', 64))));

    public static void WholeUseStateCancelsAllRecipientsRoundsAndDelayedPlacementCold()
    {
        var s = Scene.Start("group"); s.BeginCard(CardKind.Peach); s.AtConversion(); s.ChooseConversion(true); s.Play();
        Need(s.Polarity == SkillPolarity.Yin, "A real self Peach acceptance, rather than a state injection, establishes Yin before the global trick.");
        var cost = s.V().Hand.Single(card => card.Kind == CardKind.Duel).Id;
        var garden = s.BeginCard(CardKind.PeachGarden); s.Reach(() => s.Optional(Fengzi));
        s.Answer(c => Action(c) == "activate"); s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "cost" && c.Cards.Contains(cost)) == true);
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "cost" && c.Cards.Contains(cost)); s.AtConversion();
        var use = s.NativeUse(CardKind.PeachGarden); var id = use.Id;
        Need(use.TargetSeats.Order().SequenceEqual([0, 1, 2, 3]) && s.Events<ExtraCardEffectIssuedEvent>() is [{ CardUseFrameId: var extraId, PaidCardId: var paid }] &&
            extraId == id && paid == cost && use.ExtraCardEffect is not null && s.JizhiDraws() == 1,
            "The genuine global Use first retains all four recipients, one actual Jizhi gain and the real Fengzi same-category Hand cost/extra-effect receipt.");
        s.Cold(); s.ChooseConversion(true); s.Play();
        Need(Enumerable.Range(0, 4).All(seat => s.V(seat).Hp == 8) && s.Events<RecoveryAppliedEvent>().Length == 0 &&
            s.Nullifications().Count(e => e.ParentUseFrameId == id) == 1 && s.Events<ExtraCardEffectStartedEvent>().Count(e => e.CardUseFrameId == id) == 1 &&
            s.JizhiDraws() == 1 && s.Polarity == SkillPolarity.Yang && s.Commits().Length == 2 &&
            s.G.CardMovements.Count(m => m.CardId == cost && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 1,
            "Whole-use invalidity prevents every target and the genuine extra effect from recovering, while keeping the already-paid on-Use Jizhi and Fengzi results and exactly one conversion commitment.");
        s.AssertFinished(id, garden); s.Cold();
        var nextGarden = s.BeginCard(CardKind.PeachGarden); s.Play();
        Need(Enumerable.Range(0, 4).All(seat => s.V(seat).Hp == 9) && s.Events<RecoveryAppliedEvent>().Length == 4 && s.JizhiDraws() == 2 &&
            s.Commits().Length == 2 && s.Nullifications().Length == 2,
            "A new trick Use in Yang is genuinely effective for all four recipients, retains its separate on-Use draw and cannot inherit the earlier whole-use status or consumed extra-effect payment."); s.Cold();
        Need(s.G.CardMovements.Count(m => m.CardId == nextGarden && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
            "The next real global card pays its own original Hand material once.");

        var delayed = Scene.Start("delayed"); delayed.BeginCard(CardKind.Peach); delayed.AtConversion(); delayed.ChooseConversion(true); delayed.Play();
        var lightning = delayed.BeginCard(CardKind.Lightning); delayed.AtConversion(); var delayedId = delayed.NativeUse(CardKind.Lightning).Id;
        delayed.Cold(); delayed.ChooseConversion(true); delayed.Play();
        Need(delayed.Nullifications().Single(e => e.ParentUseFrameId == delayedId).EffectiveKind == CardKind.Lightning &&
            delayed.V().Judgment.All(card => card.Id != lightning) && delayed.G.CreateCardZoneDiagnostics().Single(c => c.CardId == lightning).Location == CardLocation.DiscardPile &&
            delayed.Events<JudgmentRequestedEvent>().Length == 0,
            "A real self Lightning is invalidated at its native placement Use, pays and finishes without a counterfeit judgment-zone card or later judgment window.");
        delayed.AssertFinished(delayedId, lightning); delayed.Cold(); var turn = delayed.G.State.TurnNumber; delayed.End(); delayed.Play();
        Need(delayed.G.State.TurnNumber > turn && delayed.Events<JudgmentRequestedEvent>().Length == 0 && delayed.Commits().Length == 2,
            "The next real actual turn cannot resurrect the canceled delayed placement or trigger another polarity payment."); delayed.Cold();
    }
    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game; public PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public SkillPolarity? Polarity => V().SkillRuntimeStates?.SingleOrDefault(s => s.SkillId == Skill)?.Polarity;
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public ProgramConversionPolarityCommittedEvent[] Commits() => Events<ProgramConversionPolarityCommittedEvent>().Where(e => e.OwnerSeat == 0 && e.SkillId == Skill).ToArray();
        public ProgramCardUseNullifiedEvent[] Nullifications() => Events<ProgramCardUseNullifiedEvent>().Where(e => e.SkillId == Skill && e.OwnerSeat == 0).ToArray();
        public int JizhiDraws() => G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program.classic:jizhi.Draw");
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "The real Cuanchao command was rejected."); }
        public void Answer(Func<PromptChoice, bool> choose) => Accept(new AnswerPromptCommand(0, P!.PromptId, P.Choices.First(choose).Id, G.Revision));
        public bool Optional(string skill) => P is { PlayerSeat: 0 } && P.SkillPrompt?.SkillId == skill && P.Choices.Any(c => Action(c) == "activate");
        public void AtConversion() => Reach(() => Optional(Skill));
        public void ChooseConversion(bool accept) => Answer(c => Action(c) == (accept ? "activate" : "skip"));
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void AtDamage() => Reach(() => P?.SkillPrompt?.SkillId == DamageWatch);
        public CardUseFrame NativeUse(CardKind kind) => G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardKind == kind && f.TargetSeats.Contains(0));
        public void RequestSlash() => Accept(new UseProgramSkillCommand(0, Driver, "request", [], [3], G.Revision, P!.PromptId));
        public void Use(string id) { Accept(new UseProgramSkillCommand(0, Driver, id, [], [], G.Revision, P!.PromptId)); Play(); }
        public int BeginCard(CardKind kind)
        {
            var a = G.GetHumanLegalActions().First(a => a.CardId is { } id && V().Hand.Any(c => c.Id == id && c.Kind == kind) && a.ConversionSource is null && a.Kind != LegalActionKind.Recast);
            Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)); return a.CardId.Value;
        }
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 220; step++)
            {
                if (done()) return;
                if (P is { PlayerSeat: 0 } p)
                {
                    if (p.SkillPrompt?.SkillId is DamageWatch or RecoverWatch) { Continue(); continue; }
                    if (Optional(Jizhi)) { Answer(c => Action(c) == "activate"); continue; }
                    if (Optional(Fengzi)) { Answer(c => Action(c) == "skip"); continue; }
                    if (p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    { Answer(c => c.Parameters.GetValueOrDefault("response") is "take-damage" or "pass" or "let-die"); continue; }
                    throw new InvalidOperationException("Unexpected actual Cuanchao boundary: " + Diagnostic());
                }
                Need(G.State.Winner == Winner.None, "The bounded native Cuanchao fixture unexpectedly ended: " + Diagnostic()); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing native Cuanchao boundary: " + Diagnostic());
        }
        public void AssertFinished(long useId, int card)
        {
            Need(Events<CardUseDeclaredEvent>().Count(e => e.ResolutionId == useId && e.CardId == card) == 1 &&
                Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == useId && e.CardId == card) == 1 &&
                Events<CardActionAcceptedEvent>().Count(e => e.Action.Type == CardActionType.Use &&
                    e.Action.PhysicalCards.Count == 1 && e.Action.PhysicalCards[0].CardId == card) == 1 &&
                G.CardMovements.Count(m => m.CardId == card && m.From.Zone == CardZoneKind.Hand && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1 &&
                G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.UseFinished) == 1,
                "An effective or ineffective card retains one genuine declared Use, one original Hand payment and one whole native completion, rather than an early-pop shortcut.");
        }
        private string Diagnostic() => JsonSerializer.Serialize(new { Prompt = P is null ? null : new { P.Kind, P.PlayerSeat, P.Prompt, Skill = P.SkillPrompt?.SkillId },
            Polarity, Commits = Commits(), Nullified = Nullifications(), Damage = Events<DamageAppliedEvent>(),
            Frames = G.ResolutionStack.Select(f => new { f.Id, f.Kind }) });
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat))),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), Facts = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Checkpoint = GameCheckpointJson.Serialize(G.CreateCheckpoint()) });
        public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(State() == before, "Cold replay preserves native polarity, whole-use invalidity, exact source/target/cost, additional effects and typed self-card returns without repeating a commitment or payment."); }
        public static Scene Start(string scene)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(scene));
            ShanheStartingCard[] cards = scene switch {
                "polarity" => [new(CardKind.DrawTwo, Suit.Heart, 7)],
                "group" => [new(CardKind.Peach, Suit.Heart, 3), new(CardKind.PeachGarden, Suit.Heart, 4), new(CardKind.PeachGarden, Suit.Heart, 5), new(CardKind.Duel, Suit.Spade, 6)],
                "delayed" => [new(CardKind.Peach, Suit.Heart, 3), new(CardKind.Lightning, Suit.Spade, 1)],
                "self" => [new(CardKind.Peach, Suit.Heart, 3), new(CardKind.DrawTwo, Suit.Heart, 4), new(CardKind.Alcohol, Suit.Spade, 5)], _ => [] };
            var own = new List<string> { Cap, DamageWatch }; if (scene == "paid-loss") own.AddRange([HpGate, PaidLoss]); else own.Add(Skill);
            if (scene == "dual") own.Add(Independent); if (scene == "group") own.AddRange([Jizhi, Fengzi]);
            if (scene == "live") own.AddRange([Suppress, RecoverWatch]);
            if (scene == "rescue") cards = [new(CardKind.Peach, Suit.Heart, 3)];
            var sourceMember = new ShanheCombatantDefinition(Source, Hp: scene == "group" ? 8 : 12) { StartingHand = scene is "polarity" or "live" or "dual" or "paid-loss" or "rescue" ? Enumerable.Range(0, 6)
                .Select(i => new ShanheStartingCard(CardKind.Slash, Suit.Spade, i + 1)).ToArray() : [] };
            var setup = new ShanheBattleSetup(83, 1, "fixture-cuanchao-" + scene, Owner, scene == "live" ? 12 : scene == "rescue" ? 1 : 8, 0, 0, 0, 0, own, cards, [],
                scene == "rescue" ? [new(Peer) { StartingHand = [] }, sourceMember] :
                    [new(Peer, Hp: scene == "group" ? 8 : 12) { StartingHand = [] }, new(Peer, Hp: scene == "group" ? 8 : 12) { StartingHand = [] }, sourceMember],
                scene == "rescue" ? [new(Peer) { StartingHand = [] }] : [], new string('a', 64));
            var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8,
                AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var s = new Scene(game, registry); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-cuanchao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            foreach (var id in new[] { Skill, Cap, Jizhi, Fengzi }) b.AddSkill(Production.Value.GetSkill(id));
            object A(string id, int targets, params object[] effects) => new { id, minCards = 0, maxCards = 0, minTargets = targets,
                maxTargets = targets, targetKind = "anyLiving", usesPerTurn = (int?)null, effects };
            object Pause() => new { op = "chooseOption", target = "owner", resultBind = "paid-child", options = new[] { new { id = "continue" } } };
            var source = new Dictionary<string, object> { ["id"] = Attack, ["revision"] = 1,
                ["damageModifiers"] = new[] { new { id = "real-five-point-slash", cardKinds = new[] { "slash" }, amount = 4, condition = "always", sourceScope = "ownerUsed" } } };
            if (scene == "live") source["triggers"] = new[] { new { id = "actual-ending-recovery", window = "playEnding", subject = "owner", optional = false,
                usageScope = "game", usageLimit = 1, effects = new object[] { new { op = "selectTarget", target = "owner", targetKind = "anyWounded" }, new { op = "recover", target = "selectedTarget", amount = 1 } } } };
            var definitions = new object[] {
                new { id = Driver, revision = 1, activations = new[] { A("request", 1, new { op = "requestSlashByTarget", target = "selectedTarget", resultBind = "native-request" }),
                    A("self-five", 0, new { op = "damage", target = "owner", amount = 5 }), A("armor-five", 0, new { op = "grantArmor", target = "owner", amount = 5 }),
                    A("lose-one", 0, new { op = "loseHp", target = "owner", amount = 1 }),
                    A("lose-conversion", 0, new { op = "loseOwnerSkillsAndGrant", target = "owner", skillIds = new[] { Skill }, sourceBind = "standard:none" }) } },
                source,
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "bounded-native-quiet-turn", window = "afterNormalDraw", subject = "owner", optional = false,
                    effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } } } },
                new { id = DamageWatch, revision = 1, triggers = new[] { new { id = "actual-payment", window = "afterDamageApplied", subject = "owner", damageOccurrence = "perDamage", optional = false, effects = new[] { Pause() } } } },
                new { id = RecoverWatch, revision = 1, triggers = new[] { new { id = "actual-recovery", window = "afterHpRecovered", subject = "owner", optional = false, effects = new[] { Pause() } } } }
                ,new { id = Independent, revision = 1, triggers = new[] { new { id = "independent-whole-use", window = "cardUseBeforeTargetEffects", ownerRelation = "target",
                    cardCategories = new[] { "basic" }, optional = true, requiredPolarity = "yang", priority = -10,
                    effects = new object[] { new { op = "commitConversionPolarity", target = "owner" }, new { op = "nullifyCurrentCardUse", target = "owner" } } } } }
                ,new { id = HpGate, revision = 1, hpSkillGrants = new[] { new { id = "healthy-production-conversion", skillId = Skill, minimumHp = 2 } } }
                ,new { id = PaidLoss, revision = 1, triggers = new[] { new { id = "paid-after-whole-use", window = "cardUseBeforeTargetEffects", ownerRelation = "target",
                    cardCategories = new[] { "basic" }, optional = false, priority = -100, usageScope = "game", usageLimit = 1,
                    effects = new object[] { new { op = "loseHp", target = "owner", amount = 7 }, Pause() } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实篡朝测试操作", description = "原生实体杀请求、护甲和真实失血；不注入极性" },
                [Quiet] = new { name = "安静原生同行", description = "跳过非测试角色的出牌阶段" }, [Attack] = new { name = "真实五伤来源", description = "实际杀附加四伤和真实结束回复" } };
            labels[Independent] = new { name = "独立转换机制对照", description = "不同ID的通用whole-use幂等来源，不是生产技能第二份" };
            labels[HpGate] = new { name = "实际健康HP授予", description = "只在至少两点体力时授予真实生产转换" };
            foreach (var id in new[] { DamageWatch, RecoverWatch, PaidLoss }) labels[id] = new { name = "真实已支付子窗口", description = "在原生伤害/回复付款后暂停",
                optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var actual = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in actual.Programs) b.AddSkill(new(id, actual.Presentations[id].Name, actual.Presentations[id].Description)
                { Program = program, ProgramPresentation = actual.Presentations[id], Tags = id == Independent ? SkillTag.Conversion : SkillTag.None });
            b.AddSkill(new(Suppress, "HP11实际失效", "既有共享抑制规则") { SuppressionRule = new(11) });
            b.AddGeneral(new(Owner, "真实篡朝使用者", "", Driver, "qun", 12)); b.AddGeneral(new(Peer, "真实安静同行", "", Quiet, "qun", 12));
            b.AddGeneral(new(Source, "实际杀请求来源", "", Attack, "wei", 12));
            var delayed = CardCatalog.Get(CardKind.Lightning); b.AddCard(new("fixture:cuanchao-lightning", delayed.DisplayName, delayed.CategoryName, delayed.Description, CardKind.Lightning));
            b.AddDeck(new(Deck, "固定无自然摸牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 64).Select(i => new ContentDeckPhysicalCard("standard:slash", Suit.Club, i % 13 + 1)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "真实整牌无效与转换", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer, Source], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = scene == "rescue" ? 2 : 1, [ShanheBattleSetup.EnemyTeamId] = scene == "rescue" ? 2 : 3 }));
        }
    }
}
