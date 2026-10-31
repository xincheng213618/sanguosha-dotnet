using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class WushuangSkillsBChecks
{
    private const string Mode = "identity:classic-wushuang-b", Driver = "fixture:wushuang-b-driver", Human = "fixture:wushuang-b-human";
    private static string Skill(int id) => $"wushuang:skill-{id}";

    private static IEnumerable<T> Facts<T>(GameEngine g) where T : IGameEvent => g.Events.Select(e => e.Payload).OfType<T>();
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 4).Select(s => g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static void Accept(GameEngine g, GameCommand c) { var r = g.Submit(c); Require(r.Accepted, r.Error?.Message ?? "B skill command rejected."); }
    private static void Answer(GameEngine g, Func<PromptChoice, bool> select) { var p = P(g)!; Accept(g, new AnswerPromptCommand(p.PlayerSeat, p.PromptId, (p.Choices.FirstOrDefault(select) ?? throw new InvalidOperationException(JsonSerializer.Serialize(p))).Id, g.Revision)); }
    private static void Use(GameEngine g, LegalAction a) => Accept(g, new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, g.Revision, P(g)!.PromptId, a.PlayedCardKind) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
    private static void Activate(GameEngine g, string skill, string activation, int[] targets) => Accept(g, new UseProgramSkillCommand(0, skill, activation,
        activation == "discard-one" ? [g.CreateSnapshot(0).Players[0].Hand[0].Id] : [], targets, g.Revision, P(g)!.PromptId));
    private static void End(GameEngine g) => Accept(g, new EndPlayPhaseCommand(0, g.Revision, P(g)!.PromptId));
    private static void Play(GameEngine g) => Reach(g, p => p.Kind == DecisionKind.PlayCard && p.PlayerSeat == 0);
    private static void Reach(GameEngine g, Func<PendingDecision, bool> done)
    { for (var i = 0; i < 220; i++) { if (P(g) is { } p && done(p)) return; Step(g); } throw new InvalidOperationException("Wushuang B boundary missing: " + JsonSerializer.Serialize(P(g))); }
    private static void Step(GameEngine g)
    {
        if (P(g) is { Kind: DecisionKind.DiscardCards } d) Accept(g, new DiscardCardsCommand(d.PlayerSeat, d.ValidCardIds.Take(d.RequiredCardCount).ToArray(), d.PromptId, g.Revision));
        else if (P(g) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) End(g);
        else if (P(g) is { } p && p.PlayerSeat == 0 && p.Choices.Count > 0)
        {
            var c = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "let-die") ?? p.Choices[0];
            Accept(g, new AnswerPromptCommand(0, p.PromptId, c.Id, g.Revision));
        }
        else { if (g.State.Status == EngineStatus.Completed) throw new InvalidOperationException("Completed: " + string.Join("\n",g.Events.TakeLast(35).Select(e=>e.Payload.GetType().Name+":"+JsonSerializer.Serialize(e.Payload,e.Payload.GetType())))); Accept(g, new AdvanceOneStepCommand(g.Revision)); }
    }
    private static void Cold(GameEngine g, ContentRegistry r)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), r);
        Require(JsonSerializer.Serialize(g.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack) &&
            JsonSerializer.Serialize(g.CardMovements) == JsonSerializer.Serialize(restored.CardMovements) &&
            Enumerable.Range(0, 4).All(s => SnapshotJson.Serialize(g.CreateSnapshot(s)) == SnapshotJson.Serialize(restored.CreateSnapshot(s))),
            "B skills cold-restore their owning frames, hidden prompts and exact physical movements.");
    }
    private static void Require(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
    private static (GameEngine, ContentRegistry) Create(int[] skills, CardKind cards, int initialHp = 3, int otherHp = 8, bool attackOther = false, bool dinganDamage = false)
    {
        var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(skills, cards, initialHp, otherHp, attackOther, dinganDamage));
        var g = GameEngine.CreateStandard(new GameOptions { Seed = 17, ModeId = Mode, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = "player", UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 20 }, r);
        Accept(g, new StartGameCommand()); Reach(g, p => p.Kind == DecisionKind.SelectGeneral && p.PlayerSeat == 0);
        Accept(g, new SelectGeneralCommand(0, Human, g.Revision, P(g)!.PromptId)); return (g, r);
    }
    private sealed class Fixture(int[] skills, CardKind cards, int initialHp, int otherHp, bool attackOther, bool dinganDamage) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-wushuang-b", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var asm = typeof(StandardContentPackage).Assembly;
            string Read(string suffix) { using var stream = asm.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.wushuang-skills-b." + suffix + ".json")!; using var reader = new StreamReader(stream); return reader.ReadToEnd(); }
            var prod = SkillProgramCatalog.Load(Read("rules"), Read("presentation"));
            using (var auditStream = asm.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.wushuang-skills-b-audit.rules.json")!)
            using (var auditPresentation = asm.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.wushuang-skills-b-audit.presentation.json")!)
            using (var auditRulesReader = new StreamReader(auditStream))
            using (var auditPresentationReader = new StreamReader(auditPresentation))
            {
                var audit = SkillProgramCatalog.Load(auditRulesReader.ReadToEnd(), auditPresentationReader.ReadToEnd());
                foreach (var (id, program) in audit.Programs) b.AddSkill(new(id, audit.Presentations[id].Name, audit.Presentations[id].Description) { Program = program, ProgramPresentation = audit.Presentations[id] });
            }
            foreach (var (id, program) in prod.Programs) b.AddSkill(new(id, prod.Presentations[id].Name, prod.Presentations[id].Description) { Program = program, ProgramPresentation = prod.Presentations[id] });
            if (skills.Contains(3218)) b.AddSkill(StandardWushuangContentRegistry.Create().Skills["ol:weijing"]);
            var fixtures = SkillProgramCatalog.Load(Rules.Replace("$SCHEMA$", SkillProgramCatalog.RulesSchemaVersion.ToString()),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "行为驱动", description = "有实体牌和原生命令的技能规则夹具" },
                        ["fixture:wushuang-b-quiet"] = new { name = "固定对手", description = "跳过出牌" },
                        ["fixture:wushuang-b-attack"] = new { name = "对手原生杀", description = "出牌阶段对唯一对手使用杀" },
                        ["fixture:wushuang-b-twin"] = new { name = "双闪多目标夹具", description = "双目标杀各需两闪" } } }));
            foreach (var (id, program) in fixtures.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = fixtures.Presentations[id] });
            if (dinganDamage)
            {
                const string watcher = "fixture:wushuang-b-dingan-damage";
                var damageFixture = SkillProgramCatalog.Load(
                    "{\"schemaVersion\":" + SkillProgramCatalog.RulesSchemaVersion + ",\"skills\":[{\"id\":\"" + watcher + "\",\"revision\":1," +
                    "\"modifiers\":[{\"id\":\"two-slashes\",\"query\":\"slashLimit\",\"operation\":\"add\",\"value\":1,\"priority\":0}]," +
                    "\"triggers\":[{\"id\":\"damage-child\",\"window\":\"afterDamageApplied\",\"subject\":\"damageSource\",\"damageOccurrence\":\"perDamage\",\"optional\":true," +
                    "\"effects\":[{\"op\":\"draw\",\"target\":\"owner\",\"amount\":1}]}]}]}",
                    JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                        skills = new Dictionary<string, object> { [watcher] = new { name = "伤害冷恢复边界", description = "第二张原生杀及伤害观察" } } }));
                b.AddSkill(new(watcher, "伤害冷恢复边界", "第二张原生杀及伤害观察") { Program = damageFixture.Programs[watcher], ProgramPresentation = damageFixture.Presentations[watcher] });
            }
            b.AddGeneral(new(Human, "无双B组主将", "", Driver, "qun", 8, skills.Select(id => id == 3218 ? "ol:weijing" : Skill(id)).Concat(skills.Contains(3287) ? ["fixture:wushuang-b-twin"] : Array.Empty<string>()).Concat(dinganDamage ? ["fixture:wushuang-b-dingan-damage"] : Array.Empty<string>()).ToArray()) { InitialHp = initialHp });
            for (var i = 1; i < 4; i++)
                b.AddGeneral(new($"fixture:wushuang-b-other-{i}", "固定对手", "", "fixture:wushuang-b-quiet", "qun", otherHp, attackOther ? ["fixture:wushuang-b-attack"] : []));
            var cardId = cards == CardKind.DrawTwo ? "standard:draw_two" : cards == CardKind.Duel ? "standard:duel" : cards == CardKind.Dodge ? "standard:dodge" : "standard:slash";
            b.AddDeck(new("fixture:wushuang-b-deck", "同色原生实体", 4, 2, [])
                { PhysicalCards = Enumerable.Range(0, 240).Select(i => new ContentDeckPhysicalCard(skills.Contains(3287) ? i % 3 == 0 ? "standard:slash" : i % 3 == 1 ? "standard:dodge" : "standard:draw_two" : cards == CardKind.Duel && i % 2 == 1 ? "standard:nullification" : cardId, cards == CardKind.Dodge ? Suit.Club : Suit.Heart, cards == CardKind.Duel ? i % 2 == 1 ? 9 : 2 : 7)).ToArray() });
            b.AddMode(new(Mode, "官方技能B组夹具", 4, 4, new Dictionary<string, int>(), "fixture:wushuang-b-deck", 2,
                [Human, "fixture:wushuang-b-other-1", "fixture:wushuang-b-other-2", "fixture:wushuang-b-other-3"], ContentModeKind.Team, new Dictionary<string, int> { ["player"] = 1, ["other"] = 3 }));
        }
    }
    private const string Rules = """
    {"schemaVersion":$SCHEMA$,"skills":[
    {"id":"fixture:wushuang-b-twin","revision":1,"modifiers":[{"id":"extra-target","query":"cardTargetCount","operation":"add","value":1,"priority":0,"cardKinds":["slash","fireSlash","thunderSlash"]}],"cardPolicies":[{"id":"two-dodges","kind":"minimumResponseCount","cardKinds":["slash","fireSlash","thunderSlash"],"requiredCardKinds":["dodge"],"value":2}]},
    {"id":"fixture:wushuang-b-driver","revision":1,"activations":[
      {"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
      {"id":"self-damage","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":1}]},
      {"id":"other-hp-loss","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":1}]},
      {"id":"discard-one","minCards":1,"maxCards":1,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":1}]}]},
    {"id":"fixture:wushuang-b-quiet","revision":1,"triggers":[{"id":"skip","window":"afterNormalDraw","subject":"owner","priority":-100,"optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
    {"id":"fixture:wushuang-b-attack","revision":1,"triggers":[{"id":"slash","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"distanceUnlimitedAgainstTarget","useCardActionWindows":true}]}]}
    ]}
    """;
}

