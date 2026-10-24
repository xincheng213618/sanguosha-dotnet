using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryGuYongScenario
{
    internal const string Shenxing = "boundary:shenxing-mobile", Bingyi = "boundary:bingyi-mobile", Driver = "fixture:boundary-guyong-driver", Watch = "fixture:boundary-guyong-watch";
    private const string Hero = "boundary:gu-yong-mobile", Peer = "fixture:boundary-guyong-peer", Quiet = "fixture:boundary-guyong-quiet", Deck = "fixture:boundary-guyong-deck";

    // A single real HumanSeat is legal for this public-API Shanhe scenario and the WPF model.
    internal static (ContentRegistry Registry, GameEngine Game) CreateScene(string handPattern = "type", bool withWatch = false, bool atBingyi = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        ShanheStartingCard[] hand = handPattern switch
        {
            "cost" => [new(CardKind.Slash, Suit.Spade, 2), new(CardKind.Dodge, Suit.Heart, 3), new(CardKind.Peach, Suit.Diamond, 4),
                new(CardKind.Slash, Suit.Spade, 5), new(CardKind.Dodge, Suit.Heart, 6), new(CardKind.Slash, Suit.Spade, 7)],
            "color" => [new(CardKind.Slash, Suit.Heart, 2), new(CardKind.Indulgence, Suit.Diamond, 3)],
            "tricks" => [new(CardKind.DrawTwo, Suit.Spade, 2), new(CardKind.Indulgence, Suit.Heart, 3)],
            "fail" => [new(CardKind.Slash, Suit.Spade, 2), new(CardKind.Indulgence, Suit.Heart, 3)],
            _ => [new(CardKind.Slash, Suit.Spade, 2), new(CardKind.Dodge, Suit.Heart, 3)]
        };
        var setup = new ShanheBattleSetup(46, 1, "boundary-guyong-" + handPattern, Hero, 3, 0, 0, 0, 0,
            withWatch ? [Quiet, Driver, Watch] : [Quiet, Driver], hand,
            handPattern == "cost" ? [new(CardKind.Crossbow, Suit.Club, 1)] : [],
            [new(Peer, Hp: 4) { StartingHand = [] }, new(Peer, Hp: 4) { StartingHand = [] }],
            [new(Peer, Hp: 4) { StartingHand = [] }], new string('e', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry);
        Accept(game, new StartGameCommand()); Play(game);
        if (atBingyi) BeginBingyi(game);
        return (registry, game);
    }

    internal static PendingDecision? Decision(GameEngine game) => game.PendingDecision ?? game.CreateSnapshot(0).PendingDecision;
    internal static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); if (!result.Accepted) throw new InvalidOperationException(result.Error?.Message ?? "Rejected Gu Yong scene command."); }
    internal static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Decision(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    internal static void Reach(GameEngine game, Func<PendingDecision?, bool> done)
    {
        for (var step = 0; step < 160; step++)
        {
            var prompt = Decision(game); if (done(prompt)) return;
            if (prompt is not null) throw new InvalidOperationException("Unexpected Gu Yong decision: " + prompt.Kind + ", seat " + prompt.PlayerSeat + ", skill " + prompt.SkillPrompt?.SkillId);
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded Gu Yong scene did not reach its next native decision.");
    }
    internal static void Play(GameEngine game) => Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    internal static void Use(GameEngine game, string skill, string activation, int[] cards) =>
        Accept(game, new UseProgramSkillCommand(0, skill, activation, cards, [], game.Revision, Decision(game)!.PromptId));
    internal static void BeginBingyi(GameEngine game)
    {
        Play(game); Accept(game, new EndPlayPhaseCommand(0, game.Revision, Decision(game)!.PromptId));
        Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Bingyi && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    }
    internal static bool SelectingRecipients(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Bingyi &&
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-targets");
    internal static bool WatchPending(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Watch;
    internal static void ContinueWatch(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-guyong", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Quiet}}","revision":1,"modifiers":[{"id":"keep-originals","query":"handLimit","operation":"add","value":20,"priority":0}],"triggers":[
                    {"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]},
                    {"id":"native-observer","window":"nativeDrawStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"setChainedState","target":"owner","chained":false}]}]},
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"wound","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]},
                    {"id":"heal","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"owner","amount":1}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[
                    {"id":"paid-cost","window":"cardsMoved","subject":"owner","sourceZones":["hand","equipment"],"movementReasons":["skill-program.boundary:shenxing-mobile.DiscardSelected"],"movementOccurrence":"perOwnerBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"cost-return","options":[{"id":"continue"}]}]},
                    {"id":"native-draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.boundary:shenxing-mobile.Draw","skill-program.boundary:bingyi-mobile.Draw"],"movementOccurrence":"perBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"draw-return","options":[{"id":"continue"}]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
                { [Quiet] = new { name = "保持原牌", description = "跳过普通摸牌并保留原手牌。" }, [Driver] = new { name = "体力变化", description = "通过真实失血与回复检验动态次数。" },
                    [Watch] = new { name = "真实子窗", description = "在原弃牌或摸牌触发返回前暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "返回原结算" } } } }));
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Peer, "受赠角色", "supporter", Quiet, "shu", 4));
            builder.AddDeck(new(Deck, "固定实体摸牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "顾雍实际结算场景", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
