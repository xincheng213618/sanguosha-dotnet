using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileLuSuChecks
{
    private const string Hero = "mobile:lu-su", Skill = "mobile:dimeng", Watch = "fixture:mobile-lu-su-discard",
        Peer = "fixture:mobile-lu-su-peer", Deck = "fixture:mobile-lu-su-deck", Reason = "skill-program.mobile:dimeng.MoveBoundCards";
    public static void CostChildrenSourceLossAndHandSwapCold()
    {
        var shortCost = Scene.Start(insufficient: true); shortCost.Play();
        var before = Enumerable.Range(0, 4).Select(seat => shortCost.V(seat).Hand.Select(c => c.Id).Order().ToArray()).ToArray();
        var revision = shortCost.G.Revision;
        var rejected = shortCost.G.Submit(new UseProgramSkillCommand(0, Skill, "alliance-pact", [], [1, 2], revision, shortCost.P!.PromptId));
        shortCost.Need(!rejected.Accepted && shortCost.G.Revision == revision &&
            Enumerable.Range(0, 4).All(seat => shortCost.V(seat).Hand.Select(c => c.Id).Order().SequenceEqual(before[seat])) &&
            !shortCost.G.CardMovements.Any(m => m.Reason.Value == Reason) && !shortCost.E<ProgramSkillStartedEvent>().Any(e => e.SkillId == Skill) &&
            shortCost.G.GetHumanLegalActions().Any(a => a.ProgramSkillId == Skill && a.SelectableTargetSequences!.All(pair => !pair.Contains(2))),
            "An actual one-card owner cannot submit a two-card difference; rejection preserves every hand and leaves the Play entitlement available for a payable pair.");
        var s = Scene.Start(); s.Play();
        var first = s.V(1).Hand.Select(c => c.Id).Order().ToArray();
        var second = s.V(2).Hand.Select(c => c.Id).Order().ToArray();
        var cost = s.V(0).Hand.Take(2).Select(c => c.Id).ToArray();
        s.Accept(new UseProgramSkillCommand(0, Skill, "alliance-pact", [], [1, 2], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        s.Answer(c => c.Cards.SequenceEqual([cost[0]])); s.Answer(c => c.Cards.SequenceEqual([cost[1]]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill); var rootId = root.Id;
        s.Need(root.InstructionIndex == 2 && root.PendingMovementContinuation is not null &&
            root.CardSetBindings.Single(b => b.Name == "dimeng-cost").CardIds.SequenceEqual(cost) &&
            s.V(1).Hand.Select(c => c.Id).Order().SequenceEqual(first) && s.V(2).Hand.Select(c => c.Id).Order().SequenceEqual(second) &&
            cost.All(id => s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == Reason) == 1),
            "Mobile Dimeng pays the exact two original entities and awaits their real discard child before either hand is exchanged.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill) == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill);
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish") == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish"); s.Play();
        s.Need(!s.V(0).Skills!.Any(k => k.ContentId == Skill) &&
            s.V(1).Hand.Select(c => c.Id).Order().SequenceEqual(second) && s.V(2).Hand.Select(c => c.Id).Order().SequenceEqual(first) &&
            cost.All(id => s.G.CardMovements.Count(m => m.CardId == id && m.To == CardLocation.DiscardPile && m.Reason.Value == Reason) == 1) &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == rootId && e.Completed) == 1 && s.G.ResolutionStack.All(f => f.Id != rootId),
            "The cold original paid activation completes one whole swap after actual source loss without repeating its cost.");
    }
    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected mobile Lu Su command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 160 && !done(); i++)
            {
                if (P?.SkillPrompt?.SkillId == "classic:haoshi" && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip"))
                { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); continue; }
                Need(P is null, "Unexpected mobile Lu Su prompt."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "Missing bounded mobile Lu Su boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves original paid Dimeng and its discard child."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P, Hand = V(0).HandCount })); }
        public static Scene Start(bool insufficient = false)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var extra = Enumerable.Range(0, 6).Select(i => "fixture:mobile-lu-su-dummy-" + i).ToArray();
            var setup = new ShanheBattleSetup(107, 1, "fixture-mobile-lu-su", Hero, 3, 0, 0, 0, 0, [Watch, .. extra],
                new[] { new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 1), new ShanheStartingCard(CardKind.Dodge, Suit.Club, 2), new ShanheStartingCard(CardKind.Dodge, Suit.Spade, 3) }.Take(insufficient ? 1 : 3).ToArray(), [],
                [new(Peer) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 4)] },
                 new(Peer) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 5), new(CardKind.Dodge, Suit.Spade, 6), new(CardKind.Dodge, Suit.Club, 7)] },
                 new(Peer) { StartingHand = [] }], [], new string('b', 64));
            var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, r));
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-lu-su", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:lu-su");
            var skills = new object[] { new { id = Watch, revision = 1, triggers = new[] { new { id = "actual-paid-discard", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" }, movementOccurrence = "perBatch",
                movementReasons = new[] { Reason }, optional = false, usageScope = "game", usageLimit = 1,
                effects = new object[] { new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" } } }, new { op = "replaceSkillsOnAwakening", target = "owner", skillIds = new[] { "standard:none" } } } } } } };
            var labels = new Dictionary<string, object> { [Watch] = new { name = "实际缔盟弃牌孩子", description = "原费用后合法失去来源。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            foreach (var i in Enumerable.Range(0, 6)) b.AddSkill(new("fixture:mobile-lu-su-dummy-" + i, "真实失技资格", "原生失技菜单的合法候选。"));
            b.AddGeneral(new(Peer, "真实同行", "supporter", "standard:none", "wei", 5));
            b.AddDeck(new(Deck, "固定补牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动鲁肃原生流程", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
