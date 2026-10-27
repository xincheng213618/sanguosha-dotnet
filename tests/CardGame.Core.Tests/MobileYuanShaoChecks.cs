using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileYuanShaoChecks
{
    internal const string Hero = "mobile:yuan-shao", Skill = "mobile:luanji", Binding = "same-suit-pair-as-arrow-barrage";

    internal sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public ContentRegistry Registry { get; } = registry;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected real ordinary Yuan Shao command."); }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 160; i++)
            {
                if (ready()) return;
                if (P is { PlayerSeat: 0, Kind: DecisionKind.Nullification or DecisionKind.RespondDodge } prompt)
                {
                    var choice = prompt.Choices.First(c => c.Cards.Count == 0);
                    Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision)); continue;
                }
                Need(P is not { PlayerSeat: 0 }, "Unexpected ordinary Yuan Shao boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Fixed ordinary Yuan Shao flow did not reach its next native boundary.");
        }
        public void Cold()
        {
            string Dump() => string.Join("|", Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i)))) +
                JsonSerializer.Serialize(G.ResolutionStack) + GameCheckpointJson.Serialize(G.CreateCheckpoint());
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), Registry);
            Need(before == Dump(), "Cold preserves the original paid Arrow materials, actual finalized-target child and original activation.");
        }
        public void Need(bool yes, string why)
        { if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.Phase, Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Frames = G.ResolutionStack.Select(f => new { f.Id, f.Kind }) })); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var peer = new ShanheCombatantDefinition(Peer, Hp: 4) { StartingHand = [new(CardKind.Dodge, Suit.Diamond, 9)] };
            var setup = new ShanheBattleSetup(17, 1, "ordinary-mobile-yuan-shao-effective-pair", Hero, 4, 0, 0, 0, 0,
                ["classic:hongyan", "classic:luanji", Watch],
                [new(CardKind.Slash, Suit.Spade, 2), new(CardKind.Slash, Suit.Heart, 3), new(CardKind.Slash, Suit.Club, 4)],
                [new(CardKind.QinggangSword, Suit.Spade, 8)], [peer, peer], [peer], new string('f', 64));
            var scene = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry), registry);
            scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private const string Peer = "fixture:ordinary-mobile-yuan-shao-peer", Watch = "fixture:ordinary-mobile-yuan-shao-arrow-child";
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ordinary-mobile-yuan-shao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:yuan-shao", "classic:xiao-qiao");
            var skills = new[] { new { id = Watch, revision = 1, triggers = new[] {
                new { id = "actual-arrow-finalized", window = "cardUseTargetsFinalized", ownerRelation = "actor", cardKinds = new[] { "arrowBarrage" }, optional = false,
                    usageScope = "game", usageLimit = 1, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } } } }
            } } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Watch] = new { name = "真实万箭子窗", description = "原万箭目标已确认后继续。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续万箭" } } } }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "真实万箭响应者", "supporter", "standard:none", "wei", 4));
            const string deck = "fixture:ordinary-mobile-yuan-shao-deck";
            b.AddDeck(new(deck, "固定普通万箭费用", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "普通移动乱击有效花色", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
