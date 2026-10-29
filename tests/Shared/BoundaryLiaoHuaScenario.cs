using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryLiaoHuaScenario
{
    internal const string Dangxian = "boundary:dangxian-mobile", Fuli = "boundary:fuli-mobile", Driver = "fixture:boundary-liaohua-driver",
        Gain = "fixture:boundary-liaohua-gain", Recovery = "fixture:boundary-liaohua-recovery", RecoveryPeer = "fixture:boundary-liaohua-recovery-peer",
        GainReason = "skill-program.boundary:liao-hua-mobile.discard-slash";
    private const string Hero = "boundary:liao-hua-mobile", Quiet = "fixture:boundary-liaohua-quiet", Seed = "fixture:boundary-liaohua-seed", Deck = "fixture:boundary-liaohua-deck";

    internal static (ContentRegistry Registry, GameEngine Game) CreateScene(bool discardSlash = true, bool recovery = false, bool raisePeer = false, int weiHp = 3)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var extras = new List<string> { Quiet, Driver };
        if (discardSlash) extras.AddRange([Seed, Gain]);
        if (recovery) extras.Add(raisePeer ? RecoveryPeer : Recovery);
        var setup = new ShanheBattleSetup(52, 1, "boundary-liaohua", Hero, recovery ? 2 : 4, recovery ? 2 : 0, 0, 0, 0, extras,
            discardSlash ? [new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Slash, Suit.Spade, 8)] : [], [],
            [new("fixture:boundary-liaohua-wei", Hp: weiHp) { StartingHand = [] }, new("fixture:boundary-liaohua-wu", Hp: 3) { StartingHand = [] }],
            [new("fixture:boundary-liaohua-qun", Hp: 3) { StartingHand = [] }], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 6
        }, registry);
        Accept(game, new StartGameCommand()); Reach(game, prompt => discardSlash ? SlashChoice(prompt) : prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        return (registry, game);
    }
    internal static PendingDecision? Decision(GameEngine game) => game.PendingDecision ?? game.CreateSnapshot(0).PendingDecision;
    internal static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); if (!result.Accepted) throw new InvalidOperationException(result.Error?.Message ?? "Rejected Liao Hua scene command."); }
    internal static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Decision(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    internal static void Reach(GameEngine game, Func<PendingDecision?, bool> done)
    {
        for (var step = 0; step < 160; step++)
        {
            var prompt = Decision(game); if (done(prompt)) return;
            if (prompt is not null) throw new InvalidOperationException("Unexpected Liao Hua scene decision: " + prompt.Kind + ", seat " + prompt.PlayerSeat + ", skill " + prompt.SkillPrompt?.SkillId);
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded Liao Hua scene did not reach its decision.");
    }
    internal static bool SlashChoice(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Dangxian && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "boundary-liao-hua-discard-slash");
    internal static void Continue(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
    internal static void Play(GameEngine game) => Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    internal static void End(GameEngine game) => Accept(game, new EndPlayPhaseCommand(0, game.Revision, Decision(game)!.PromptId));
    internal static void FatalDamage(GameEngine game) => Accept(game, new UseProgramSkillCommand(0, Driver, "fatal", [], [], game.Revision, Decision(game)!.PromptId));

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-liaohua", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$$"""
                {"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[
                  {"id":"{{{{Quiet}}}}","revision":1,"triggers":[{"id":"no-draw","isSkillInvocation":false,"window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{{{Seed}}}}","revision":1,"triggers":[{"id":"discard-real-hand","window":"gameStarting","subject":"owner","optional":false,"effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["hand"]}]}]},
                  {"id":"{{{{Driver}}}}","revision":1,"activations":[{"id":"fatal","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":6}]}]},
                  {"id":"{{{{Gain}}}}","revision":1,"triggers":[{"id":"original-gain-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{{GainReason}}}}"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]},
                  {"id":"{{{{Recovery}}}}","revision":1,"triggers":[{"id":"recovery-child","window":"afterHpRecovered","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]},
                  {"id":"{{{{RecoveryPeer}}}}","revision":1,"triggers":[{"id":"recovery-child","window":"afterHpRecovered","subject":"owner","optional":false,"effects":[
                    {"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]},
                    {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                    {"op":"recover","target":"selectedTarget","amount":1}
                  ]}]}
                ]}
                """;
            var presentations = new Dictionary<string, object>();
            foreach (var id in new[] { Quiet, Seed, Driver, Gain, Recovery, RecoveryPeer })
                presentations[id] = id is Gain or Recovery or RecoveryPeer
                    ? new { name = id, description = "原生当先与伏枥场景", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                    : (object)new { name = id, description = "原生当先与伏枥场景" };
            var programs = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, program) in programs.Programs) builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            foreach (var faction in new[] { "wei", "wu", "qun" }) builder.AddGeneral(new("fixture:boundary-liaohua-" + faction, faction, "supporter", Quiet, faction, 4));
            builder.AddDeck(new(Deck, "无杀的真实牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界廖化原生场景", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, "fixture:boundary-liaohua-wei", "fixture:boundary-liaohua-wu", "fixture:boundary-liaohua-qun"], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
