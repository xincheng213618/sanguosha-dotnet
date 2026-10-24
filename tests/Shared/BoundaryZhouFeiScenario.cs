using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryZhouFeiScenario
{
    internal const string Liangyin = "boundary:liangyin-mobile", Kongsheng = "boundary:kongsheng-mobile", Driver = "fixture:boundary-zhoufei-driver", Control = "fixture:boundary-zhoufei-control", Pile = "fixture:boundary-zhoufei-pile", Watch = "fixture:boundary-zhoufei-watch";
    private const string Hero = "boundary:zhou-fei-mobile", Peer = "fixture:boundary-zhoufei-peer", Quiet = "fixture:boundary-zhoufei-quiet", Gone = "fixture:boundary-zhoufei-gone", Deck = "fixture:boundary-zhoufei-deck";
    internal static (ContentRegistry Registry, GameEngine Game) CreateScene(bool foreign = true, bool withWatch = false, bool atPreparation = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(foreign));
        var extra = new[] { Quiet, foreign ? Driver : Control, foreign ? Driver : Pile }.Distinct().Concat(withWatch ? [Watch] : Array.Empty<string>()).ToArray();
        ShanheStartingCard[] peerHand = foreign ? [new(CardKind.DrawTwo, Suit.Heart, 7), new(CardKind.Crossbow, Suit.Club, 1)] : [];
        var setup = new ShanheBattleSetup(49, 1, "boundary-zhoufei", Hero, 3, 0, 0, 0, 0, extra,
            [new(CardKind.DrawTwo, Suit.Heart, 7), new(CardKind.DrawTwo, Suit.Diamond, 8), new(CardKind.DrawTwo, Suit.Heart, 9), new(CardKind.Peach, Suit.Heart, 3)], [],
            [new(Peer, Hp: 4) { StartingHand = peerHand }, new(Peer, Hp: 4) { StartingHand = peerHand }],
            [new(Peer, Hp: 4) { StartingHand = peerHand }], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 16
        }, registry);
        Accept(game, new StartGameCommand()); Reach(game, prompt => Optional(prompt, Kongsheng));
        if (atPreparation) return (registry, game);
        Skip(game); Play(game);
        if (foreign)
        { Accept(game, new EndPlayPhaseCommand(0, game.Revision, Decision(game)!.PromptId)); Reach(game, prompt => Optional(prompt, Kongsheng)); }
        return (registry, game);
    }
    internal static PendingDecision? Decision(GameEngine game) => game.PendingDecision ?? game.CreateSnapshot(0).PendingDecision;
    internal static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); if (!result.Accepted) throw new InvalidOperationException(result.Error?.Message ?? "Rejected Zhou Fei scenario command."); }
    internal static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Decision(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    internal static bool Optional(PendingDecision? prompt, string skill) => prompt?.SkillPrompt?.SkillId == skill && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    internal static void Activate(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    internal static void Skip(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") != "activate");
    internal static bool Branch(PendingDecision? prompt, string branch) => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "boundary-zhou-fei" && choice.Parameters.GetValueOrDefault("branch") == branch) == true;
    internal static bool SourceChoice(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Driver && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") is "keep" or "remove");
    internal static bool WatchPending(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Watch;
    internal static void ContinueWatch(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
    internal static void Reach(GameEngine game, Func<PendingDecision?, bool> done)
    {
        for (var step = 0; step < 220; step++)
        {
            var prompt = Decision(game); if (done(prompt)) return;
            if (WatchPending(prompt)) { ContinueWatch(game); continue; }
            if (prompt is not null) throw new InvalidOperationException("Unexpected Zhou Fei decision: " + prompt.Kind + ", seat " + prompt.PlayerSeat + ", skill " + prompt.SkillPrompt?.SkillId);
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded Zhou Fei scenario did not reach its native decision.");
    }
    internal static void Play(GameEngine game) => Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    internal static void StoreAll(GameEngine game)
    {
        Activate(game); Reach(game, prompt => Branch(prompt, "confirm-store"));
        var count = Decision(game)!.Choices.Count(choice => choice.Parameters.GetValueOrDefault("branch") == "toggle");
        for (var index = 0; index < count; index++)
        { var choice = Decision(game)!.Choices.Where(choice => choice.Parameters.GetValueOrDefault("branch") == "toggle").ElementAt(index); Answer(game, candidate => candidate.Id == choice.Id); }
        Answer(game, choice => choice.Parameters.GetValueOrDefault("branch") == "confirm-store");
    }
    internal static void DrawLiangyin(GameEngine game, int recipient)
    { Reach(game, prompt => Optional(prompt, Liangyin)); Activate(game); Reach(game, prompt => Branch(prompt, "target")); Answer(game, choice => choice.Targets.SequenceEqual([recipient])); }
    internal static void UseDrawTwo(GameEngine game)
    { var card = game.CreateSnapshot(0).Players[0].Hand.First(card => card.Kind == CardKind.DrawTwo); Accept(game, new PlayCardCommand(0, card.Id, [], game.Revision, Decision(game)!.PromptId, CardKind.DrawTwo)); }
    internal static void UseControl(GameEngine game, string activation) => Accept(game, new UseProgramSkillCommand(0, Control, activation, [], [], game.Revision, Decision(game)!.PromptId));
    internal static string FixtureRules => $$$$"""
        {"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[
          {"id":"{{{{Quiet}}}}","revision":1,"modifiers":[{"id":"keep-hand","query":"handLimit","operation":"add","value":40,"priority":0}],"triggers":[
            {"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]},
            {"id":"native-invoice","window":"nativeDrawStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"setChainedState","target":"owner","chained":false}]}]},
          {"id":"{{{{Driver}}}}","revision":1,"triggers":[{"id":"foreign-health","window":"turnStartBeforeNormalFlow","subject":"owner","turnOwnerScope":"otherLiving","usageScope":"game","usageLimit":1,"optional":false,"effects":[
            {"op":"loseHp","target":"owner","amount":1},{"op":"recover","target":"owner","amount":1},
            {"op":"chooseOption","target":"owner","resultBind":"keep-source","options":[{"id":"keep"}]}]}]},
          {"id":"{{{{Control}}}}","revision":1,"activations":[
            {"id":"wound","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]},
            {"id":"obtain","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"obtainPublicPileCard","target":"owner","skillIds":["{{{{Pile}}}}"]}]},
            {"id":"drop-source","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["{{{{Kongsheng}}}}"],"sourceBind":"{{{{Gone}}}}"}]}]},
          {"id":"{{{{Pile}}}}","revision":1,"triggers":[{"id":"real-pile","window":"cardUseCompleted","ownerRelation":"actor","cardKinds":["drawTwo"],"optional":false,"effects":[{"op":"storeTopCardInPublicPile","target":"owner","amount":16}]}]},
          {"id":"{{{{Watch}}}}","revision":1,"triggers":[{"id":"stored-payment","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementReasons":["skill.boundary-zhou-fei.store"],"movementOccurrence":"perOwnerBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"store-child","options":[{"id":"continue"}]}]}]}
        ]}
        """;
    internal static string FixturePresentation => $$$$"""
        {"schemaVersion":{{{{SkillProgramCatalog.PresentationSchemaVersion}}}},"skills":{
          "{{{{Quiet}}}}":{"name":"固定原牌","description":"跳过普通摸牌，保留所有原实体。"},
          "{{{{Driver}}}}":{"name":"外人回合真实体力变化","description":"原生失去与回复体力。","optionLabels":{"keep":"继续当前回合"}},
          "{{{{Control}}}}":{"name":"原生获得与失血","description":"公开程序能力驱动另一个真正武将牌堆的获得。"},
          "{{{{Pile}}}}":{"name":"另一技能的武将牌堆","description":"原生使用无中生有后将牌堆顶牌置于自己的武将牌上。"},
          "{{{{Watch}}}}":{"name":"付款子窗","description":"原始扣牌付款后暂停。","optionLabels":{"continue":"返回原付款"}}
        }}
        """;
    private sealed class Fixture(bool foreign) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-zhoufei", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero); var programs = SkillProgramCatalog.Load(FixtureRules, FixturePresentation);
            foreach (var (id, program) in programs.Programs) builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddSkill(new(Gone, "已失箜声", "用于实际发行后的来源失效。"));
            builder.AddGeneral(new(Peer, "当前回合角色", "supporter", Quiet, "shu", 4));
            builder.AddDeck(new(Deck, "真实固定实体", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard(foreign ? "standard:slash" : "standard:dodge", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "周妃真实伏牌与全局良姻场景", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
