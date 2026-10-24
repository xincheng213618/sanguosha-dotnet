using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryWangJiScenario
{
    internal const string Qizhi = "boundary:qizhi-mobile", Jinqu = "boundary:jinqu-mobile", Watch = "fixture:boundary-wangji-watch";
    private const string Hero = "boundary:wang-ji-mobile", Peer = "fixture:boundary-wangji-peer", Quiet = "fixture:boundary-wangji-quiet", Deck = "fixture:boundary-wangji-deck";
    internal const string ForeignEmpty = "mou:meng-huo-mobile", ForeignFull = "mou:jiang-wei-mobile";
    internal static (ContentRegistry Registry, GameEngine Game) CreateScene(bool withWatch = false, bool foreignCharge = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(foreignCharge));
        var setup = new ShanheBattleSetup(48, 1, "boundary-wangji", Hero, 3, 0, 0, 0, 0,
            withWatch ? [Quiet, Watch] : [Quiet],
            [new(CardKind.DrawTwo, Suit.Heart, 7), new(CardKind.DrawTwo, Suit.Diamond, 8), new(CardKind.DrawTwo, Suit.Heart, 9),
             new(CardKind.DrawTwo, Suit.Diamond, 10), new(CardKind.Alcohol, Suit.Spade, 3), new(CardKind.IronChain, Suit.Club, 12), new(CardKind.Crossbow, Suit.Club, 1)], [],
            [new(Peer, Hp: 4) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Peach, Suit.Diamond, 3)] }, new(foreignCharge ? ForeignFull : Peer, Hp: 4) { StartingHand = [] }],
            [new(foreignCharge ? ForeignEmpty : Peer, Hp: 4) { StartingHand = [] }], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry);
        Accept(game, new StartGameCommand()); Play(game); return (registry, game);
    }
    internal static PendingDecision? Decision(GameEngine game) => game.PendingDecision ?? game.CreateSnapshot(0).PendingDecision;
    internal static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); if (!result.Accepted) throw new InvalidOperationException(result.Error?.Message ?? "Rejected Wang Ji scene command."); }
    internal static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Decision(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    internal static void Reach(GameEngine game, Func<PendingDecision?, bool> done)
    {
        for (var step = 0; step < 160; step++)
        {
            var prompt = Decision(game); if (done(prompt)) return;
            if (prompt is not null) throw new InvalidOperationException("Unexpected Wang Ji scene decision: " + prompt.Kind + ", seat " + prompt.PlayerSeat + ", skill " + prompt.SkillPrompt?.SkillId);
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded Wang Ji scene did not reach its next real decision.");
    }
    internal static bool Inspecting(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Qizhi && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "discard");
    internal static bool Charging(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Jinqu && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "charge");
    internal static bool WatchPending(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Watch;
    internal static void ContinueWatch(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
    internal static void DrainUntil(GameEngine game, Func<bool> done)
    {
        for (var step = 0; step < 200; step++)
        {
            if (done()) return;
            if (WatchPending(Decision(game))) { ContinueWatch(game); continue; }
            if (Decision(game) is { } unexpected) throw new InvalidOperationException("Unexpected Wang Ji continuation: " + unexpected.Kind + ", " + unexpected.SkillPrompt?.SkillId);
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Wang Ji's original native children did not finish.");
    }
    internal static void Play(GameEngine game) => DrainUntil(game, () => Decision(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    internal static int EnemySeat(GameEngine game) => game.CreateSnapshot(0).Players.Single(player => player.Seat != 0 && player.HandCount > 0).Seat;
    internal static void ActivateQizhi(GameEngine game, int recipient) => Accept(game, new UseProgramSkillCommand(0, Qizhi, "charge-draw", [], [recipient], game.Revision, Decision(game)!.PromptId));
    internal static int UseCard(GameEngine game, CardKind kind, int[]? targets = null)
    {
        var card = game.CreateSnapshot(0).Players[0].Hand.First(card => card.Kind == kind);
        Accept(game, new PlayCardCommand(0, card.Id, targets ?? [], game.Revision, Decision(game)!.PromptId, kind)); return card.Id;
    }
    internal static string FixtureRules => $$$$"""
        {"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[
          {"id":"{{{{Quiet}}}}","revision":1,"modifiers":[{"id":"preserve-hand","query":"handLimit","operation":"add","value":30,"priority":0}],"triggers":[
            {"id":"no-normal-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]},
            {"id":"actual-draw-return","window":"nativeDrawStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"setChainedState","target":"owner","chained":false}]}]},
          {"id":"{{{{Watch}}}}","revision":1,"triggers":[
            {"id":"paid-discard","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementReasons":["skill-program.boundary:qizhi-mobile.discard"],"movementOccurrence":"perOwnerBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"discard-return","options":[{"id":"continue"}]}]},
            {"id":"native-draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["{{{{CardMoveReasons.Draw.Value}}}}","skill-program.boundary:qizhi-mobile.Draw","skill-program.boundary:jinqu-mobile.Draw"],"movementOccurrence":"perBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"draw-return","options":[{"id":"continue"}]}]}]}
        ]}
        """;
    internal static string FixturePresentation => $$$$"""
        {"schemaVersion":{{{{SkillProgramCatalog.PresentationSchemaVersion}}}},"skills":{
          "{{{{Quiet}}}}":{"name":"保留原实体","description":"跳过普通摸牌并保留原手牌。"},
          "{{{{Watch}}}}":{"name":"原生子窗","description":"在真实弃牌或摸牌的子窗暂停。","optionLabels":{"continue":"继续原结算"}}
        }}
        """;
    private sealed class Fixture(bool foreignCharge) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-wangji", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            if (foreignCharge) CharacterFixtureContent.Register(builder, ForeignEmpty, ForeignFull);
            var programs = SkillProgramCatalog.Load(FixtureRules, FixturePresentation);
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Peer, "实际受益角色", "supporter", Quiet, "shu", 4));
            builder.AddDeck(new(Deck, "实际实体摸牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "王基私看与蓄力场景", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: foreignCharge ? [Hero, Peer, ForeignEmpty, ForeignFull] : [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
