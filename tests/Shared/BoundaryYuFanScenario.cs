using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryYuFanScenario
{
    internal const string Zongxuan = "boundary:zongxuan-mobile", Zhiyan = "boundary:zhiyan-mobile", Driver = "fixture:boundary-yufan-driver";
    private const string Hero = "boundary:yu-fan-mobile", Peer = "fixture:boundary-yufan-peer", Quiet = "fixture:boundary-yufan-quiet", Deck = "fixture:boundary-yufan-deck";

    internal static (ContentRegistry Registry, GameEngine Game) CreateScene(bool withJudgmentCard = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        ShanheStartingCard[] hand = withJudgmentCard ? [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Lightning, Suit.Spade, 1)] : [new(CardKind.Dodge, Suit.Heart, 2)];
        var setup = new ShanheBattleSetup(51, 1, "boundary-yufan", Hero, 3, 0, 0, 0, 0, [Quiet, Driver],
            hand, [new(CardKind.Crossbow, Suit.Club, 1)],
            [new(Peer, Hp: 5) { StartingHand = [] }, new(Peer, Hp: 5) { StartingHand = [] }], [new(Peer, Hp: 5) { StartingHand = [] }], new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 5
        }, registry);
        Accept(game, new StartGameCommand()); Play(game); return (registry, game);
    }

    internal static PendingDecision? Decision(GameEngine game) => game.PendingDecision ?? game.CreateSnapshot(0).PendingDecision;
    internal static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); if (!result.Accepted) throw new InvalidOperationException(result.Error?.Message ?? "Rejected Yu Fan scene command."); }
    internal static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Decision(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    internal static void Reach(GameEngine game, Func<PendingDecision?, bool> done)
    {
        for (var step = 0; step < 120; step++)
        {
            var prompt = Decision(game); if (done(prompt)) return;
            if (prompt is not null) throw new InvalidOperationException("Unexpected Yu Fan scene decision: " + prompt.Kind + ", seat " + prompt.PlayerSeat + ", skill " + prompt.SkillPrompt?.SkillId);
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded Yu Fan scene did not reach its decision.");
    }
    internal static void Play(GameEngine game) => Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    internal static void BeginZongxuan(GameEngine game) => Accept(game, new UseProgramSkillCommand(0, Zongxuan, "draw-then-top", [], [], game.Revision, Decision(game)!.PromptId));
    internal static bool TopChoice(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Zongxuan && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards");
    internal static void DrawOne(GameEngine game) => Accept(game, new UseProgramSkillCommand(0, Driver, "draw", [], [], game.Revision, Decision(game)!.PromptId));
    internal static bool OptionalZongxuan(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Zongxuan && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    internal static bool DiscardTopChoice(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Zongxuan && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "discard-top-finish");
    internal static void DiscardMixed(GameEngine game) => Accept(game, new UseProgramSkillCommand(0, Driver, "discard-mixed", [], [], game.Revision, Decision(game)!.PromptId));

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-yufan", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$$"""
                {"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[
                  {"id":"{{{{Quiet}}}}","revision":1,"triggers":[{"id":"no-draw","isSkillInvocation":false,"window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{{{Driver}}}}","revision":1,"activations":[
                    {"id":"draw","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":1}]},
                    {"id":"discard-mixed","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"selectOwnedCards","target":"owner","zones":["hand","judgment"],"minimumCards":2,"maximumCards":2,"resultBind":"discarded"},
                      {"op":"moveBoundCards","target":"owner","sourceBind":"discarded","destination":"discardPile","awaitMovementTriggers":true}
                    ]}
                  ]}
                ]}
                """;
            var programs = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Quiet, Driver }.ToDictionary(id => id, id => new { name = id, description = "原生纵玄实体场景" }) }));
            foreach (var (id, program) in programs.Programs) builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Peer, "正常角色", "supporter", Quiet, "wei", 5));
            builder.AddDeck(new(Deck, "固定实际桃", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界虞翻原生场景", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
