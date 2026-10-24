using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryZuoCiScenario
{
    internal const string Huashen = "boundary:huashen-mobile", Xinsheng = "boundary:xinsheng-mobile", Borrowed = "fixture:boundary-zuoci-borrowed", Driver = "fixture:boundary-zuoci-driver";
    private const string Hero = "boundary:zuo-ci-mobile", Peer = "fixture:boundary-zuoci-peer", Quiet = "fixture:boundary-zuoci-quiet", Excluded = "fixture:boundary-zuoci-limited", Deck = "fixture:boundary-zuoci-deck";
    // Uses the actual mobile-edition manifest and the shared native private-library transaction.
    internal static (ContentRegistry Registry, GameEngine Game) CreateScene()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(50, 1, "boundary-zuoci", Hero, 3, 0, 0, 0, 0, [Quiet, Driver], [], [],
            [new(Peer, Hp: 6) { StartingHand = [] }, new(Peer, Hp: 6) { StartingHand = [] }], [new(Peer, Hp: 6) { StartingHand = [] }], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 6
        }, registry);
        Accept(game, new StartGameCommand()); Reach(game, prompt => LibraryChoice(prompt));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("value")?.StartsWith("fixture:boundary-zuoci-avatar-", StringComparison.Ordinal) == true);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("value") == Borrowed);
        Reach(game, prompt => Optional(prompt, Huashen)); Activate(game); Reach(game, prompt => AvatarChoice(prompt, "exchange"));
        return (registry, game);
    }
    internal static PendingDecision? Decision(GameEngine game) => game.PendingDecision ?? game.CreateSnapshot(0).PendingDecision;
    internal static PrivateGeneralLibrarySnapshot Library(GameEngine game) => game.CreateSnapshot(0).Players[0].PrivateGeneralLibraries!.Single();
    internal static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); if (!result.Accepted) throw new InvalidOperationException(result.Error?.Message ?? "Rejected Zuo Ci scene command."); }
    internal static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Decision(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    internal static bool Optional(PendingDecision? prompt, string skill) => prompt?.SkillPrompt?.SkillId == skill && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    internal static bool LibraryChoice(PendingDecision? prompt) => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "private-general-library") == true;
    internal static bool AvatarChoice(PendingDecision? prompt, string action) => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "shanhe-s5-avatar" && choice.Parameters.GetValueOrDefault("action") == action) == true;
    internal static void Activate(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    internal static void Skip(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
    internal static void Reach(GameEngine game, Func<PendingDecision?, bool> done)
    {
        for (var step = 0; step < 140; step++)
        {
            var prompt = Decision(game); if (done(prompt)) return;
            if (prompt is not null) throw new InvalidOperationException("Unexpected Zuo Ci scene decision: " + prompt.Kind + ", seat " + prompt.PlayerSeat + ", skill " + prompt.SkillPrompt?.SkillId);
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded Zuo Ci native-library scene did not reach its decision.");
    }
    internal static void Play(GameEngine game) => Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    internal static void Hurt(GameEngine game) => Accept(game, new UseProgramSkillCommand(0, Driver, "hurt", [], [], game.Revision, Decision(game)!.PromptId));
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-zuoci", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$$"""
                {"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[
                  {"id":"{{{{Quiet}}}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{{{Driver}}}}","revision":1,"activations":[{"id":"hurt","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":2}]}]},
                  {"id":"{{{{Borrowed}}}}","revision":1,"activations":[{"id":"draw","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":1}]}]}
                ]}
                """;
            var programs = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Quiet, Driver, Borrowed }.ToDictionary(id => id, id => new { name = id, description = "原生化身场景能力" }) }));
            foreach (var (id, program) in programs.Programs) builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddSkill(new(Excluded, "限定技", "原生化身过滤的限定技能") { Tags = SkillTag.Limited });
            builder.AddGeneral(new(Peer, "当前角色", "supporter", Quiet, "wei", 6));
            var ids = new List<string> { Hero, Peer };
            for (var index = 0; index < 12; index++)
            { var id = $"fixture:boundary-zuoci-avatar-{index}"; ids.Add(id); builder.AddGeneral(new(id, "化身" + index, "supporter", Borrowed, "wu", 3, [Excluded], GeneralGender.Female)); }
            builder.AddDeck(new(Deck, "实际原生实体", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界左慈原生私库场景", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: ids, ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
