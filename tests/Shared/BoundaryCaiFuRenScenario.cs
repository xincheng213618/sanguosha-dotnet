using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryCaiFuRenScenario
{
    internal const string Listen = "boundary:qieting-mobile", Donation = "boundary:xianzhou-mobile", Driver = "fixture:boundary-cfr-mobile-driver", Watch = "fixture:boundary-cfr-mobile-watch";
    internal const string HandReason = "skill-program.cai-fu-ren.qieting.hand", EquipmentReason = "skill-program.cai-fu-ren.qieting.move";
    private const string Hero = "boundary:cai-fu-ren-mobile", Owner = "fixture:boundary-cfr-mobile-owner", Quiet = "fixture:boundary-cfr-mobile-quiet", Mode = "identity:classic-boundary-cfr-mobile", Deck = "fixture:boundary-cfr-mobile-deck";
    internal static (ContentRegistry Registry, GameEngine Game) CreateScene(bool singleHuman = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(singleHuman));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = singleHuman ? null : [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
        Accept(game, new StartGameCommand());
        for (var step = 0; step < 120; step++)
        {
            var prompt = Decision(game);
            if (prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return (registry, game);
            if (prompt is { Kind: DecisionKind.SelectGeneral }) Accept(game, new SelectGeneralCommand(prompt.PlayerSeat, prompt.PlayerSeat == 0 ? Owner : prompt.ValidContentIds.First(id => id != Owner), game.Revision, prompt.PromptId));
            else if (prompt is not null) throw new InvalidOperationException("Unexpected Cai Fu Ren setup decision " + prompt.Kind);
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The Cai Fu Ren public identity setup did not reach its first Play.");
    }
    internal static PendingDecision? Decision(GameEngine game) => game.PendingDecision ?? Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
    internal static PlayerSnapshot Player(GameEngine game, int seat) => game.CreateSnapshot(seat).Players[seat];
    internal static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); if (!result.Accepted) throw new InvalidOperationException(result.Error?.Message ?? "Rejected Cai Fu Ren public command."); }
    internal static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Decision(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    internal static bool Optional(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Listen && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    internal static void Activate(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    internal static void Continue(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
    internal static void End(GameEngine game) { var prompt = Decision(game)!; Accept(game, new EndPlayPhaseCommand(prompt.PlayerSeat, game.Revision, prompt.PromptId)); }
    internal static void Reach(GameEngine game, Func<PendingDecision?, bool> done, bool skipListen = false)
    {
        for (var step = 0; step < 200; step++)
        {
            var prompt = Decision(game); if (done(prompt)) return;
            if (prompt is { Kind: DecisionKind.PlayCard }) End(game);
            else if (skipListen && Optional(prompt)) Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
            else if (prompt is not null) throw new InvalidOperationException("Unexpected Cai Fu Ren scene decision " + prompt.Kind + " seat " + prompt.PlayerSeat + " skill " + prompt.SkillPrompt?.SkillId);
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded Cai Fu Ren scene did not reach its next actual boundary.");
    }
    internal static void Use(GameEngine game, string activation, int[]? targets = null)
    { var prompt = Decision(game)!; Accept(game, new UseProgramSkillCommand(prompt.PlayerSeat, Driver, activation, [], targets ?? [], game.Revision, prompt.PromptId)); }
    internal static void PlayCard(GameEngine game, int id, int[]? targets = null)
    {
        var prompt = Decision(game)!;
        var action = game.GetLegalActions(prompt.PlayerSeat).First(candidate => candidate.CardId == id && (targets is null || candidate.TargetSeats.SequenceEqual(targets)));
        Accept(game, new PlayCardCommand(prompt.PlayerSeat, id, targets ?? action.TargetSeats, game.Revision, prompt.PromptId, action.PlayedCardKind));
    }
    internal static void BeginListening(GameEngine game)
    {
        End(game); Reach(game, Optional); Activate(game);
    }

    private sealed class Fixture(bool singleHuman) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-cfr-mobile", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var programs = SkillProgramCatalog.Load($$$$"""
              {"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[
                {"id":"{{{{Quiet}}}}","revision":1,"modifiers":[{"id":"keep-real-material","query":"handLimit","operation":"add","value":20,"priority":0}],"triggers":[{"id":"no-normal-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                {"id":"{{{{Driver}}}}","revision":1,"activations":[
                  {"id":"self-damage","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":1}]},
                  {"id":"other-damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                  {"id":"refill","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":8}]}
                ]},
                {"id":"{{{{Watch}}}}","revision":1,"triggers":[{"id":"real-listening-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{{HandReason}}}}"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"original-gain","options":[{"id":"continue"}]}]},
                  {"id":"real-equipment-child","window":"cardsMoved","subject":"owner","sourceZones":["equipment"],"movementOccurrence":"perOwnerBatch","movementReasons":["equipment.replace"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"original-equipment","options":[{"id":"continue"}]}]}]}
              ]}
              """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> {
                  [Quiet] = new { name = "保留真实手牌", description = "不进行普通摸牌并保留真实输入" }, [Driver] = new { name = "实际回合资格", description = "真Use、对自己及他人伤害" },
                  [Watch] = new { name = "原生付款子窗", description = "取得真实手牌或装备原生替换后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
              } }));
            foreach (var (id, program) in programs.Programs) builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "移动界蔡夫人实际来源", "supporter", Listen, "qun", 3, [Donation, Quiet, Driver, Watch], GeneralGender.Female));
            var pool = new List<string> { Owner };
            for (var index = 1; index < 4; index++) { var id = "fixture:boundary-cfr-mobile-peer-" + index; pool.Add(id); builder.AddGeneral(new(id, "可控制的其他角色", "supporter", Quiet, "wei", 4, singleHuman ? [] : [Driver])); }
            builder.AddDeck(new(Deck, "固定真实手牌与装备", 8, 0, []) { PhysicalCards = Enumerable.Range(0, 128).Select(index => new ContentDeckPhysicalCard((index % 4) switch { 0 => "standard:crossbow", 1 => "standard:peach", 2 => "standard:iron_chain", _ => "standard:dodge" }, (index % 2 == 0 ? Suit.Club : Suit.Heart), index % 13 + 1)).ToArray() });
            builder.AddMode(new(Mode, "真实四席身份回合", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: pool));
        }
    }
}
