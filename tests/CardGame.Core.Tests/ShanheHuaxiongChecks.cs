using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheHuaxiongChecks
{
    private const string Yaowu = StandardShanheS5HuaxiongPackage.YaowuId, Shizhan = StandardShanheS5HuaxiongPackage.ShizhanId;
    private const string Activation = "round-limited-owner-duel", Medicine = StandardShanheCampaignCatalog.CurrentYaoli6SkillId;
    private const string Owner = "fixture:s5-hx-owner", Victim = "fixture:s5-hx-victim", Peer = "fixture:s5-hx-peer", Deck = "fixture:s5-hx-deck";
    private const string Driver = "fixture:s5-hx-driver", DamageWatch = "fixture:s5-hx-damage", Health = "fixture:s5-hx-health", Gate = "fixture:s5-hx-gate", Insert = "fixture:s5-hx-insert", Quiet = "fixture:s5-hx-quiet";
    private static readonly Lazy<ContentRegistry> Production = new(() => StandardShanheCampaignCatalog.CreateBattleRegistry(
        new ShanheBattleSetup(83, 1, "fixture:s5-hx-production", StandardShanheS5HuaxiongPackage.HuaxiongId, 3, 0, 0, 0, 0,
            [], [], [], [new("classic:cao-cao")], [], new string('a', 64)) { TacticSkillIds = [Medicine] }));

    public static void PairedYaowuDuelInteractionsAndActualRecipesStayIsolatedCold()
    {
        foreach (var name in new[] { "paired", "paid-loss" })
        {
            var s = Scene.Start(name); s.Use("armor-one", [0]); s.BeginDuel(); s.Until(s.AtHealth);
            var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardKind == CardKind.Duel);
            var paid = s.ColorPaid().Single(); var responses = s.E<DuelResponseEvent>().Where(f => f.ResponderSeat == 1 && f.UsedSlash).ToArray();
            Need(use.CardId == 0 && use.PhysicalCardIds is { Count: 0 } && use.Action is { ActorSeat: 0, Type: CardActionType.Use, EffectiveKind: CardKind.Duel } action &&
                action.TargetSeats.SequenceEqual([1]) && paid.CardUseFrameId == use.Id && paid.CardActionId == action.ActionId &&
                paid is { OwnerSeat: 0, DamageSourceSeat: 1, Amount: 1, IsRed: false, EffectiveCardKind: CardKind.Duel, EffectiveSuit: Suit.None } &&
                responses.Length == 2 && s.E<RequiredResponseProgressEvent>().Any(f => f.ResolutionId == use.Id && f.ResponderSeat == 1 && f.ResponseCount == 2 && f.RequiredResponseCount == 2) &&
                s.G.CardMovements.Where(m => m.From == CardLocation.Hand(1) && m.To == CardLocation.Processing && m.CardKind == CardKind.Slash).Select(m => m.CardId).Distinct().Count() == 2 &&
                s.E<RecoveryAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 0, Amount: 1, RemainingHp: 11 }] && s.E<DamageAppliedEvent>().Length == 0 &&
                s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == paid.ProgramFrameId && f.DamageCardColorHealth == paid),
                "The new owner-to-other native Duel really demands two distinct NPC Slash entities through Wushuang, then returns incoming colorless Duel damage to the owner's Yaowu and pauses inside its genuine whole-recovery child before Armor. " + s.Diagnostic());
            s.Cold(); s.Continue();
            if (name == "paid-loss")
            {
                s.Until(() => s.AtHealth() && s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "lost"));
                Need(s.V().Hp == 9 && !s.Has(Yaowu) && !s.Has(Shizhan) && s.ColorPaid().Length == 1 && s.E<DamageAppliedEvent>().Length == 0,
                    "A real recovery child loses two HP and withdraws both HP-dependent production skills after recovery and the original Duel use are already paid, while its damage is still pending.");
                s.Cold(); s.Continue();
            }
            s.Play();
            Need(s.V().Hp == (name == "paired" ? 11 : 9) && s.V().Armor == 0 &&
                s.E<DamageAppliedEvent>() is [{ SourceSeat: 1, TargetSeat: 0, Amount: 1, ArmorPayment: { ArmorAbsorbed: 1, HpLossAmount: 0 } }] &&
                s.E<RecoveryAppliedEvent>().Length == 1 && s.ColorPaid().Length == 1 &&
                s.E<ProgramDamageCardColorHealthCompletedEvent>().Count(f => f.ProgramFrameId == paid.ProgramFrameId) == 1 &&
                s.E<ProgramSkillResolvedEvent>().Count(f => f.SkillId == Shizhan && f.Completed) == 1 &&
                s.E<CardUseFinishedEvent>().Count(f => f.ResolutionId == use.Id) == 1 && s.G.ResolutionStack.All(f => f.Id != paid.ProgramFrameId),
                "The paid recovery and two actual responses survive child cold replay and source loss; the same pending Duel pays Armor once and returns to its active program once. " + s.Diagnostic()); s.Cold();
        }
        VerifyActualRecipes();
    }

    private static void VerifyActualRecipes()
    {
        const string boss = StandardShanheS5HuaxiongPackage.HuaxiongId;
        var old = StandardShanheCampaignCatalog.CreateDefinition(3, StandardShanheCampaignCatalog.LocalEditionId);
        Need(old.Nodes.SelectMany(n => n.Encounters ?? []).All(e => e.Enemies.All(m => m.GeneralId != boss)), "The new-player 22647 two-skill form is absent from the 2024 edition.");
        for (var tier = 1; tier <= 3; tier++)
        {
            var definition = StandardShanheCampaignCatalog.CreateDefinition(tier, StandardShanheCampaignCatalog.Returning2026EditionId);
            var encounter = definition.Nodes.Single(n => n.Id == "hulao").Encounters!.Single(e => e.Id == "hulao:huaxiong-newplayer-s5");
            Need(encounter.Enemies is [{ GeneralId: boss, MaxHpBonus: 0, Hp: null, InitialHandBonus: 0, DrawBonus: 0, StartingHand: null }] &&
                (encounter.Allies?.Count ?? 0) == 0,
                "Every local tier uses the complete raw new-player form without unrelated 2647 growth, fixed upper-band equipment or invented allies.");
            var setup = new ShanheBattleSetup(83, 1, "hulao", "classic:guan-yu", 4, 0, 0, 0, 0, [], [], [], encounter.Enemies, [], ShanheCampaignContent.GetContentHash(definition));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup); var game = StandardShanheCampaignCatalog.CreateBattle(setup);
            Need(game.Submit(new StartGameCommand()).Accepted, "The actual conditional boss recipe must start."); var v = game.CreateSnapshot(0).Players.Single(p => p.GeneralId == boss);
            Need(v is { Hp: 3, MaxHp: 3, HandCount: 4, Armor: 0 } && v.Equipment.Count == 0 && v.Skills?.Select(s => s.ContentId).ToHashSet().SetEquals([Yaowu, Shizhan]) == true &&
                game.Events.All(e => e.Payload is not DamageAppliedEvent), "The actual raw three-HP boss enters with exactly the two new production skills and four ordinary opening cards, without initialization damage.");
            Need(State(GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry)) == State(game), "The actual new-player recipe and both production skills cold replay identically.");
            var plain = setup with { EnemyRoster = [new("classic:cao-cao")] };
            Need(!StandardShanheCampaignCatalog.CreateBattleRegistry(plain).Skills.ContainsKey(Yaowu) && registry.Skills.ContainsKey(Yaowu) &&
                !StandardShanheCampaignCatalog.CreateBattleRegistry(plain).Generals.ContainsKey(boss), "Warming the exact new boss cannot leak its package or skill into an ordinary recipe with no owner.");
        }
    }

    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
    private static string State(GameEngine g) => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, g.PlayerCount).Select(i => SnapshotJson.Serialize(g.CreateSnapshot(i))),
        Frames = g.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), Facts = g.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())),
        g.CardMovements, Zones = g.CreateCardZoneDiagnostics(), Checkpoint = GameCheckpointJson.Serialize(g.CreateCheckpoint()) });

    private sealed class Scene(GameEngine game, ContentRegistry registry, string name)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public int Round => E<RoundStartedEvent>().Last().RoundNumber;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public ProgramDamageCardColorHealthReceipt[] ColorPaid() => E<ProgramDamageCardColorHealthPaidEvent>().Select(e => e.Receipt).ToArray();
        public bool Has(string skill) => V().Skills?.Any(s => s.ContentId == skill) == true;
        public bool AtDamage() => P is { PlayerSeat: 0 } && P.SkillPrompt?.SkillId == DamageWatch && P.Choices.Any(c => c.Parameters.ContainsKey("option-id"));
        public bool AtHealth() => P is { PlayerSeat: 0 } && P.SkillPrompt?.SkillId == Health && P.Choices.Any(c => c.Parameters.ContainsKey("option-id"));
        public void Accept(GameCommand c) { var r = G.Submit(c); if (!r.Accepted) throw new InvalidOperationException((r.Error?.Message ?? "Rejected native Hua Xiong command.") + " " + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> select) => Accept(new AnswerPromptCommand(0, P!.PromptId, P.Choices.First(select).Id, G.Revision));
        public void Continue() => Answer(c => c.Parameters.ContainsKey("option-id"));
        public void Begin(string id, int[] targets) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId));
        public void Use(string id, int[] targets) { Begin(id, targets); Play(); }
        public bool CanDuel() => G.GetHumanLegalActions().Any(a => a.ProgramSkillId == Shizhan);
        public void BeginDuel() => Accept(new UseProgramSkillCommand(0, Shizhan, Activation, [], [1], G.Revision, P!.PromptId));
        public void Duel() { BeginDuel(); Play(); }
        public void RejectDuel() { Need(!CanDuel(), "Spent native current-round quota hides Shizhan."); var before = State(G); var r = G.Submit(new UseProgramSkillCommand(0, Shizhan, Activation, [], [1], G.Revision, P!.PromptId)); Need(!r.Accepted && State(G) == before, "A stale extra Duel is rejected atomically without use, response or damage facts."); }
        public int Card(CardKind kind, int[] targets)
        {
            var self = targets.Length == 0 && kind == CardKind.Alcohol; var legal = G.GetHumanLegalActions().ToArray();
            var a = legal.FirstOrDefault(a => a.Kind != LegalActionKind.Recast && a.ConversionSource is null && a.CardId is { } id && V().Hand.Any(c => c.Id == id && c.Kind == kind) &&
                (a.TargetSeats.SequenceEqual(targets) || self && a.TargetSeats.SequenceEqual([0])));
            if (a is null) throw new InvalidOperationException("Missing real physical action " + kind + ": " + Diagnostic() + " " + JsonSerializer.Serialize(new
            { OwnHand = V().Hand.Select(c => new { c.Id, c.Kind, c.Suit, c.Rank }), Legal = legal.Select(x => new { x.Kind, x.CardId, x.TargetSeats, x.ProgramSkillId }) }));
            Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)); return a.CardId.Value;
        }
        public void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Finish(int card) { Play(); Need(E<CardUseFinishedEvent>().Count(f => f.CardId == card) == 1, "The real physical use finishes exactly once."); }
        public void EndPlay() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Until(Func<bool> done)
        {
            for (var i = 0; i < 240; i++)
            {
                if (done()) return;
                if (P is { PlayerSeat: 0 } p)
                {
                    if (p.Choices.Any(c => c.Parameters.ContainsKey("option-id"))) { Continue(); continue; }
                    if (p.SkillPrompt?.SkillId == "classic:jizhi" && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate")) { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); continue; }
                    if (p.Kind == DecisionKind.Nullification || p.Kind is DecisionKind.RespondSlash or DecisionKind.RespondDodge) { Answer(c => c.Cards.Count == 0); continue; }
                    throw new InvalidOperationException("Unexpected real Hua Xiong boundary: " + Diagnostic());
                }
                Need(G.State.Winner == Winner.None, "The bounded Hua Xiong scene must remain live. " + Diagnostic()); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded real Hua Xiong continuation was not reached: " + Diagnostic());
        }
        public void Cold() { var before = State(G); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(State(G) == before, "Cold replay preserves the whole color-health payment, original native Duel/response material, phase quota and exact child return."); }
        public string Diagnostic() => JsonSerializer.Serialize(new { name, G.State.TurnNumber, G.State.Winner,
            Players = G.CreateSnapshot(0).Players.Select(p => new { p.Seat, p.Hp, p.MaxHp, p.Armor, p.HandCount }),
            Prompt = P is null ? null : new { P.Kind, P.PlayerSeat, Skill = P.SkillPrompt?.SkillId, Choices = P.Choices.Select(c => c.Id) },
            Uses = E<CardUseDeclaredEvent>(), Damage = E<DamageAppliedEvent>(), Recovery = E<RecoveryAppliedEvent>(), Color = ColorPaid(), Frames = G.ResolutionStack.Select(f => new { f.Id, f.Kind }) });
        public static Scene Start(string name)
        {
            var count = name == "chain" ? 3 : 2; var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(name));
            ShanheStartingCard[] hand = name == "chain" ? [new(CardKind.IronChain, Suit.Club, 3), new(CardKind.Alcohol, Suit.Spade, 9), new(CardKind.FireSlash, Suit.Diamond, 7)] :
                name == "quota" ? [new(CardKind.Alcohol, Suit.Spade, 9)] : name == "nullification" ? [new(CardKind.Nullification, Suit.Club, 11)] :
                name is "paired" or "paid-loss" or "colorless-target" ? [] : [new(CardKind.Alcohol, Suit.Spade, 9), new(CardKind.Slash, name == "red" ? Suit.Heart : Suit.Club, 7)];
            var paired = name is "paired" or "paid-loss";
            var target = new ShanheCombatantDefinition(Victim, Hp: paired ? 18 : name is "quota" or "nullification" ? 20 : 12)
                { StartingHand = paired ? [new(CardKind.Slash, Suit.Club, 2), new(CardKind.Slash, Suit.Spade, 3)] : [] };
            var setup = new ShanheBattleSetup(83, 1, "fixture:s5-hx-" + name, Owner, paired ? 10 : 20, 0, 0, 0, 0, [], hand, [],
                count == 3 ? [target, new(Peer, Hp: 12) { StartingHand = [] }] : [target], [], new string('b', 64));
            var g = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = count, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r);
            var s = new Scene(g, r, name); s.Accept(new StartGameCommand()); s.Play();
            if (paired) Need(s.V(1) is { Hp: 18, MaxHp: 20, HandCount: 2 }, "The real NPC responder starts wounded enough to value its two original Slash responses."); return s;
        }
    }

    private sealed class Fixture(string name) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-s5-hx", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var standard = ContentRegistry.Build(new StandardContentPackage());
            foreach (var id in new[] { Yaowu, Shizhan, Medicine, "classic:jizhi", "classic:wushuang", "classic:jueqing", "shanhe:xiaoming" })
                if (!standard.Skills.ContainsKey(id)) b.AddSkill(Production.Value.GetSkill(id));
            var defs = new List<Dictionary<string, object>>(); var labels = new Dictionary<string, object>();
            object Pause(string id) => new { op = "chooseOption", target = "owner", resultBind = id, options = new[] { new { id } } };
            void Add(string id, Dictionary<string, object> value, params string[] options) { value["id"] = id; value["revision"] = 1; defs.Add(value); labels[id] = new { name = id, description = "真实用牌、整段受伤前健康收益及原生归返", optionLabels = options.ToDictionary(x => x, x => x) }; }
            object Act(string id, int targets, object[] effects) => new { id, minCards = 0, maxCards = 0, minTargets = targets, maxTargets = targets, targetKind = "anyLiving", usesPerTurn = (int?)null, effects };
            Add(Driver, new() { ["activations"] = new[] { Act("armor-one", 1, [new { op = "grantArmor", target = "selectedTarget", amount = 1 }]), Act("armor-two", 1, [new { op = "grantArmor", target = "selectedTarget", amount = 2 }]),
                Act("damage", 1, [new { op = "damage", target = "selectedTarget", amount = 1 }]), Act("receive", 0, [new { op = "receiveOwnerDamage", target = "owner", amount = 1 }]),
                Act("lose-one", 0, [new { op = "loseHp", target = "owner", amount = 1 }]), Act("recover-one", 0, [new { op = "recover", target = "owner", amount = 1 }]) },
                ["modifiers"] = new[] { new { id = "real-fire-range", query = "attackRange", operation = "add", value = 2, priority = 0 } } });
            Add(DamageWatch, new() { ["triggers"] = new[] { new { id = "paid-whole-damage", window = "afterDamageApplied", subject = "any", damageOccurrence = "perDamage", optional = false, priority = -100, effects = new[] { Pause("continue") } } } }, "continue");
            var recovery = new List<object> { Pause("paid") }; if (name == "paid-loss") { recovery.Add(new { op = "loseHp", target = "owner", amount = 2 }); recovery.Add(Pause("lost")); }
            Add(Health, new() { ["triggers"] = new[] { new { id = "real-whole-recovery-child", window = "afterHpRecovered", subject = "owner", hpChangeOccurrence = "perEvent", optional = false,
                usageScope = "game", usageLimit = 1, effects = recovery.ToArray() } } }, name == "paid-loss" ? ["paid", "lost"] : ["paid"]);
            Add(Gate, new() { ["hpSkillGrants"] = name == "paid-loss" ? new[] { new { id = "real-live-health-source", skillId = Yaowu, minimumHp = 10 }, new { id = "real-live-duel-source", skillId = Shizhan, minimumHp = 10 } } :
                new[] { new { id = "real-live-health-source", skillId = name == "quota" ? Shizhan : Yaowu, minimumHp = name == "quota" ? 20 : 10 } } });
            Add(Insert, new() { ["triggers"] = new[] { new { id = "native-preparation-play", window = "turnStartBeforeNormalFlow", subject = "owner", optional = false, usageScope = "game", usageLimit = 1,
                effects = new[] { new { op = "insertPhase", target = "owner", phase = "play", phaseContinuation = "beforeNormalPreparation" } } } } });
            Add(Quiet, new() { ["triggers"] = new[] { new { id = "quiet-real-opponent", window = "afterNormalDraw", subject = "owner", optional = false, effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } } } });
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = defs }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            var source = new List<string> { DamageWatch };
            if (name == "quota") source.AddRange([Gate, Insert]); else if (name == "nullification") source.AddRange([Shizhan, "classic:jizhi"]);
            else if (name == "colorless-target") source.Add(Shizhan);
            else if (name == "paired") source.AddRange([Shizhan, "classic:wushuang", Health, Yaowu]);
            else if (name == "paid-loss") source.AddRange(["classic:wushuang", Health, Gate]);
            else if (name == "source-less") source.Add(Yaowu);
            if (name == "jueqing") source.Add("classic:jueqing"); if (name == "maximum") source.Add("shanhe:xiaoming");
            b.AddGeneral(new(Owner, "真实双技专版检查来源", "", Driver, "qun", 20, source));
            var targetSkills = name is "quota" or "nullification" or "paired" or "paid-loss" ? Array.Empty<string>() : name == "medicine" ? new[] { Yaowu, Medicine } : new[] { Yaowu };
            b.AddGeneral(new(Victim, "真实受击华雄", "", Quiet, "qun", 20, targetSkills));
            b.AddGeneral(new(Peer, "真实铁索华雄", "", Quiet, "qun", 20, [Yaowu]));
            b.AddDeck(new(Deck, "固定无自然摸牌的原生牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:nullification", Suit.Club, 7)).ToArray() });
            var count = name == "chain" ? 3 : 2;
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "真实专版双技华雄", count, count, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Owner, Victim, Peer],
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = count - 1 }));
        }
    }
}
