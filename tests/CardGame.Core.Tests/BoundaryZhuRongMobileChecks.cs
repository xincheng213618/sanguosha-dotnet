using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryZhuRongMobileChecks
{
    private const string Hero = "boundary:zhu-rong-mobile", Skill = "boundary:lieren-mobile", Gain = "fixture:mobile-zhu-rong-gain", Peer = "fixture:mobile-zhu-rong-peer";
    private const string Exchange = "skill-program.boundary:lieren-mobile.exchange";

    public static void DesignationExchangePaysOriginalPindianCardsOnceCold()
    {
        var s = Scene.Start(); s.Play();
        var slash = s.V(0).Hand.Single(c => c.Kind == CardKind.Slash).Id;
        var material = s.V(0).Hand.Single(c => c.Kind == CardKind.Dodge).Id;
        s.Accept(new PlayCardCommand(0, slash, [1], s.G.Revision, s.P!.PromptId)); s.Offer();
        s.Need(s.V(1).Hp == 8 && !s.E<DamageAppliedEvent>().Any(), "Mobile Lieren offers its real Slash designation before any response or damage.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "pindian-source-card") == true);
        s.Answer(c => c.Cards.SequenceEqual([material])); s.Hold(Gain);
        var root = s.Root!.Id; var contest = s.E<PindianResultDeterminedEvent>().Single(e => e.SkillId == Skill).Result;
        s.Need(!contest.SourceWon && contest.SourceRank == 2 && contest.OpponentRank == 9 && contest.SourceCardId == material && s.V(1).Hp == 8 &&
            s.Root.PendingMovementContinuation is not null && s.V(0).Hand.Single().Id == contest.OpponentCardId &&
            s.ExchangeMoves(contest.SourceCardId, 1) == 1 && s.ExchangeMoves(contest.OpponentCardId, 0) == 1 && !s.E<DamageAppliedEvent>().Any(),
            "The non-winning original contest moves both exact public cards to the opposite hands before the original paid gain child; the Slash has still dealt no damage.");
        s.Cold(); s.Continue(); s.Play();
        s.Need(s.E<PindianResultDeterminedEvent>().Count(e => e.SkillId == Skill) == 1 && s.ExchangeMoves(contest.SourceCardId, 1) == 1 && s.ExchangeMoves(contest.OpponentCardId, 0) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == root && e.Completed) == 1 && s.E<CardUseFinishedEvent>().Count(e => e.CardId == slash) == 1 &&
            s.G.ResolutionStack.All(f => f.Id != root) && s.V(0).Hand.Any(c => c.Id == contest.OpponentCardId) &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
            "Cold returns through the same paid exchange, never replays either original Pindian material, and then completes the original Slash once.");
        s.Cold();
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Skill);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public int ExchangeMoves(int id, int target) => G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(target) && m.Reason.Value == Exchange);
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Offer() => Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Hold(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill);
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Answer(Func<PromptChoice, bool> match) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(match).Id, G.Revision)); }
        public void Reach(Func<bool> ready)
        { for (var i = 0; i < 180; i++) { if (ready()) return; Need(P is not { PlayerSeat: 0 }, "Unexpected native mobile Zhu Rong boundary."); Accept(new AdvanceOneStepCommand(G.Revision)); } throw new InvalidOperationException("Bounded Lieren flow was not reached."); }
        public void Cold()
        { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the original native Pindian result, public entity binds and both paid exchange movements."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { P, G.State.CurrentSeat, G.State.Phase, Players = Enumerable.Range(0, 4).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }) })); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Zhu Rong command."); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var peer = new ShanheCombatantDefinition(Peer, Hp: 8) { StartingHand = [] };
            var target = peer with { StartingHand = [new(CardKind.Dodge, Suit.Spade, 9), new(CardKind.Dodge, Suit.Club, 1)] };
            var setup = new ShanheBattleSetup(37, 1, "mobile-zhu-rong-native-contest", Hero, 4, 0, 0, 0, 0, [Gain],
                [new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Dodge, Suit.Diamond, 2)], [], [peer, peer], [target], new string('d', 64));
            var engine = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry);
            var scene = new Scene(engine, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-zhu-rong-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "boundary:zhu-rong", Hero);
            var skills = new[] { new { id = Gain, revision = 1, triggers = new[] { new { id = "paid-exchange", window = "cardsGained", subject = "owner", optional = false,
                destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { Exchange }, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } } } } } } };
            var labels = new Dictionary<string, object> { [Gain] = new { name = "烈刃已付交换", description = "原拼点牌实际获得后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 8));
            const string deck = "fixture:mobile-zhu-rong-deck";
            b.AddDeck(new(deck, "固定烈刃牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动烈刃真实指定目标", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
