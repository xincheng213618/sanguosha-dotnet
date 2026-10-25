using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryFuHuangHouScenario
{
    internal const string Zhuikong = "boundary:zhuikong-mobile", Qiuyuan = "boundary:qiuyuan-mobile", Watch = "fixture:boundary-fuhuanghou-watch",
        ClaimReason = "program.foreign-turn-contest.claim", GiftReason = "program.same-type-actual-use-aid.gift";
    private const string Hero = "boundary:fu-huang-hou-mobile", Owner = "fixture:boundary-fuhuanghou-owner", Quiet = "fixture:boundary-fuhuanghou-quiet",
        Mode = "identity:classic-boundary-fuhuanghou-fixture", Deck = "fixture:boundary-fuhuanghou-deck";

    // Four externally controlled seats use the public identity setup/commands, never a multiplayer Shanhe exception.
    internal static (ContentRegistry Registry, GameEngine Game) CreateScene(bool weakPeers = false, bool classicAid = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(weakPeers, classicAid));
        var game = GameEngine.CreateStandard(new GameOptions {
            Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 9
        }, registry);
        Accept(game, new StartGameCommand());
        for (var step = 0; step < 100; step++)
        {
            var prompt = Decision(game);
            if (prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return (registry, game);
            if (prompt is { Kind: DecisionKind.SelectGeneral }) {
                var id = prompt.PlayerSeat == 0 ? Owner : classicAid
                    ? "fixture:boundary-fuhuanghou-peer-" + prompt.PlayerSeat
                    : prompt.ValidContentIds.First(id => id != Owner);
                Accept(game, new SelectGeneralCommand(prompt.PlayerSeat, id, game.Revision, prompt.PromptId));
            }
            else if (prompt is not null) throw new InvalidOperationException("Unexpected Fu Huang Hou setup decision: " + prompt.Kind);
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The public identity setup did not reach its first actual Play.");
    }
    internal static PendingDecision? Decision(GameEngine game) => game.PendingDecision ?? Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
    internal static PlayerSnapshot Player(GameEngine game, int seat) => game.CreateSnapshot(seat).Players[seat];
    internal static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); if (!result.Accepted) throw new InvalidOperationException(result.Error?.Message ?? "Rejected Fu Huang Hou native scene command."); }
    internal static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Decision(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    internal static bool Optional(PendingDecision? prompt, string skill) => prompt?.SkillPrompt?.SkillId == skill && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    internal static void Activate(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    internal static void Continue(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
    internal static void End(GameEngine game) { var prompt = Decision(game)!; Accept(game, new EndPlayPhaseCommand(prompt.PlayerSeat, game.Revision, prompt.PromptId)); }
    internal static void Reach(GameEngine game, Func<PendingDecision?, bool> done, bool skipContests = false)
    {
        for (var step = 0; step < 240; step++)
        {
            var prompt = Decision(game); if (done(prompt)) return;
            if (prompt is { Kind: DecisionKind.PlayCard }) End(game);
            else if (prompt is { Kind: DecisionKind.RespondDodge }) Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "take-damage");
            else if (Optional(prompt, Qiuyuan) || skipContests && Optional(prompt, Zhuikong)) Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
            else if (prompt is not null) throw new InvalidOperationException("Unexpected Fu Huang Hou decision: " + prompt.Kind + ", seat " + prompt.PlayerSeat + ", skill " + prompt.SkillPrompt?.SkillId);
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded Fu Huang Hou scene did not reach its next actual boundary.");
    }
    internal static void Contest(GameEngine game, bool win)
    {
        var actor = game.ResolutionStack.OfType<ForeignActualTurnStartWindowFrame>().Single().OwnerSeat;
        Activate(game); Reach(game, prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-source-card" || choice.Parameters.GetValueOrDefault("program-action") == "select-target") == true);
        if (Decision(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-target"))
            Answer(game, choice => choice.Targets.SequenceEqual([actor]));
        Reach(game, prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-source-card") == true);
        var source = Player(game, 0).Hand.First(card => card.Kind == (win ? CardKind.Dodge : CardKind.Slash)).Id;
        Answer(game, choice => choice.Cards.SequenceEqual([source]));
        Reach(game, prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-card") == true);
        var peer = Player(game, Decision(game)!.PlayerSeat).Hand.First(card => card.Kind == CardKind.Slash).Id;
        Answer(game, choice => choice.Cards.SequenceEqual([peer]));
    }

    private sealed class Fixture(bool weakPeers, bool classicAid) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-fuhuanghou", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            if (classicAid) CharacterFixtureContent.Register(builder, "classic:fu-huanghou");
            var rules = $$$$"""
                {"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[
                  {"id":"{{{{Quiet}}}}","revision":1,"modifiers":[{"id":"keep-real-inputs","query":"handLimit","operation":"add","value":20,"priority":0}]},
                  {"id":"{{{{Watch}}}}","revision":1,"triggers":[{"id":"original-real-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{{ClaimReason}}}}","{{{{GiftReason}}}}"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}
                ]}
                """;
            var labels = new Dictionary<string, object> { [Quiet] = new { name = "保留原生手牌", description = "实际回合与真实拼点输入" },
                [Watch] = new { name = "原生得牌子窗", description = "取得原拼点牌或求援实际赠与后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var nodes = JsonNode.Parse(rules)!;
            if (classicAid) nodes["skills"]![0]!["activations"] = JsonNode.Parse("""
                [{"id":"two-original-targets","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                  "effects":[{"op":"grantTurnRuleModifier","target":"owner","ruleQuery":"cardTargetCount","ruleOperation":"add","amount":1,"cardKinds":["slash","fireSlash","thunderSlash","coldSlash"]}]}]
                """);
            var programs = SkillProgramCatalog.Load(nodes.ToJsonString(), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in programs.Programs) builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "移动版技能真实来源", "supporter", classicAid ? "classic:qiuyuan" : Zhuikong, "qun", 3,
                classicAid ? [Quiet, Watch] : [Qiuyuan, Quiet, Watch], GeneralGender.Female));
            var ids = new List<string> { Owner };
            for (var index = 1; index < 4; index++) { var id = "fixture:boundary-fuhuanghou-peer-" + index; ids.Add(id); builder.AddGeneral(new(id, "可控制的其他角色", "supporter", Quiet, "wei", weakPeers ? 2 : 4,
                classicAid && index == 2 ? ["classic:qiuyuan", Watch] : [])); }
            builder.AddDeck(new(Deck, "固定真实杀闪桃锦囊", 8, 0, []) { PhysicalCards = Enumerable.Range(0, 128).Select(index => (index % 4) switch {
                0 => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7), 1 => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 13),
                2 => new ContentDeckPhysicalCard("standard:peach", Suit.Diamond, 7), _ => new ContentDeckPhysicalCard("standard:dismantlement", Suit.Club, 2)
            }).ToArray() });
            builder.AddMode(new(Mode, "四席真实身份输入", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: ids));
        }
    }
}
