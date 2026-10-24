using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryLiuBiaoScenario
{
    internal const string Zishou = "boundary:zishou-mobile", Zongshi = "boundary:zongshi-mobile", Driver = "fixture:boundary-liubiao-driver", Watch = "fixture:boundary-liubiao-watch";
    private const string Hero = "boundary:liu-biao-mobile", Quiet = "fixture:boundary-liubiao-quiet", Extra = "fixture:boundary-liubiao-extra", Deck = "fixture:boundary-liubiao-deck";
    internal static (ContentRegistry Registry, GameEngine Game) CreateScene(bool unlimited = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        ShanheStartingCard[] hand = unlimited
            ? [new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Slash, Suit.Spade, 8), new(CardKind.Slash, Suit.Club, 9), new(CardKind.Dodge, Suit.Diamond, 2)]
            : [new(CardKind.Slash, Suit.Heart, 7), new(CardKind.DrawTwo, Suit.Heart, 8), new(CardKind.Crossbow, Suit.Club, 1), new(CardKind.IronChain, Suit.Club, 12)];
        var setup = new ShanheBattleSetup(53, 1, "boundary-liubiao", Hero, 3, 0, 0, 0, 0, unlimited ? [Driver, Extra] : [Driver, Watch], hand, [],
            [new("fixture:boundary-liubiao-wei", Hp: 8) { StartingHand = [] }, new("fixture:boundary-liubiao-wu", Hp: 8) { StartingHand = [] }],
            [new("fixture:boundary-liubiao-shu", Hp: 8) { StartingHand = [] }], new string('e', 64));
        var game = GameEngine.CreateStandard(new GameOptions {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 6
        }, registry);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => unlimited ? prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } : OptionalZishou(prompt));
        return (registry, game);
    }
    internal static PendingDecision? Decision(GameEngine game) => game.PendingDecision ?? game.CreateSnapshot(0).PendingDecision;
    internal static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); if (!result.Accepted) throw new InvalidOperationException(result.Error?.Message ?? "Rejected Liu Biao scene command."); }
    internal static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Decision(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    internal static bool OptionalZishou(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Zishou && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    internal static void Reach(GameEngine game, Func<PendingDecision?, bool> done)
    {
        for (var step = 0; step < 180; step++)
        {
            var prompt = Decision(game); if (done(prompt)) return;
            if (prompt is not null) throw new InvalidOperationException("Unexpected Liu Biao scene decision: " + prompt.Kind + ", seat " + prompt.PlayerSeat + ", skill " + prompt.SkillPrompt?.SkillId);
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded Liu Biao scene did not reach its decision.");
    }
    internal static void Play(GameEngine game) => Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    internal static void UseAction(GameEngine game, LegalAction action)
    {
        if (action.Kind == LegalActionKind.Recast) Accept(game, new RecastCardCommand(0, action.CardId!.Value, game.Revision, Decision(game)!.PromptId));
        else Accept(game, new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, game.Revision, Decision(game)!.PromptId, action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource });
    }
    internal static void End(GameEngine game) => Accept(game, new EndPlayPhaseCommand(0, game.Revision, Decision(game)!.PromptId));

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-liubiao", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$$"""
                {"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[
                  {"id":"{{{{Quiet}}}}","revision":1,"triggers":[{"id":"no-draw","isSkillInvocation":false,"window":"drawPhaseStarting","subject":"owner","optional":false,"priority":200,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{{{Extra}}}}","revision":1,"triggers":[{"id":"extra-play","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"priority":-100,"effects":[{"op":"insertPhase","target":"owner","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}]},
                  {"id":"{{{{Driver}}}}","revision":1,"activations":[{"id":"discard-two","minCards":2,"maxCards":2,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":2}]}]},
                  {"id":"{{{{Watch}}}}","revision":1,"triggers":[{"id":"paid-extra-draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.{{{{Zishou}}}}.Draw"],"optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}
                ]}
                """;
            var presentations = new Dictionary<string, object>();
            foreach (var id in new[] { Quiet, Extra, Driver, Watch }) presentations[id] = id == Watch
                ? new { name = id, description = "原生自守宗室场景", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                : (object)new { name = id, description = "原生自守宗室场景" };
            var programs = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, program) in programs.Programs) builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            foreach (var faction in new[] { "wei", "wu", "shu" }) builder.AddGeneral(new("fixture:boundary-liubiao-" + faction, faction, "supporter", Quiet, faction, 8));
            builder.AddDeck(new(Deck, "真实原生杀", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界刘表原生场景", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, "fixture:boundary-liubiao-wei", "fixture:boundary-liubiao-wu", "fixture:boundary-liubiao-shu"], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
