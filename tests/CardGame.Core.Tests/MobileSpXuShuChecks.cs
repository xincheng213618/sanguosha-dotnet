using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileSpXuShuChecks
{
    private const string Hero = "mobile:sp-xu-shu", Awaken = "mobile:qianxin", Skill = "mobile:jianyan", Driver = "fixture:mobile-sp-xu-shu-driver",
        Watch = "fixture:mobile-sp-xu-shu-gain", Peer = "fixture:mobile-sp-xu-shu-peer", Deck = "fixture:mobile-sp-xu-shu-deck";
    public static void AwakeningKeepsFailedRevealsUntilGiftChildCold()
    {
        var s = Scene.Start(); s.Play();
        s.Need(s.V().Hp == 3 && s.V().MaxHp == 4 && s.V().Skills!.All(k => k.ContentId != Skill), "The actual wounded owner has not awakened before dealing damage.");
        s.Use("damage", [], [1]); s.Play();
        s.Need(s.E<DamageAppliedEvent>().Any(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1) && s.V().MaxHp == 3 &&
            s.V().Skills!.Any(k => k.ContentId == Skill) && s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Awaken && e.Activated && e.Completed) == 1,
            "A genuine applied damage occurrence awakens the wounded source, pays one maximum HP and grants mobile Jianyan.");
        var match = s.V().Hand.Single(c => c.Kind == CardKind.BaguaFormation).Id;
        var failed = s.V().Hand.Where(c => c.Kind == CardKind.Dodge).Select(c => c.Id).ToArray();
        foreach (var id in new[] { match, failed[0], failed[1] }) { s.Use("top", [id], []); s.Play(); }
        s.Accept(new UseProgramSkillCommand(0, Skill, "declare-and-give", [], [], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("criterion") == "equipment") == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("criterion") == "equipment");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Targets.Count == 1));
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill); var rootId = root.Id;
        var reveals = s.E<MobileSpXuShuJianyanRevealedEvent>().Where(e => e.FrameId == rootId).ToArray();
        s.Need(reveals.Length == 3 && reveals.Select(e => e.CardId).SequenceEqual([failed[1], failed[0], match]) &&
            reveals.Select(e => e.Matched).SequenceEqual([false, false, true]) &&
            root.MobileSpXuShuJianyan!.RevealedIds.SequenceEqual(reveals.Select(e => e.CardId)) &&
            reveals.All(e => s.G.CardMovements.Last(m => m.CardId == e.CardId).To == CardLocation.Processing) &&
            !s.G.CardMovements.Any(m => failed.Contains(m.CardId) && m.To == CardLocation.DiscardPile),
            "The fixed native top cards are revealed one by one; failed cards remain public in Processing through the recipient choice.");
        s.Answer(c => c.Targets.SequenceEqual([0])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var gift = s.E<MobileSpXuShuJianyanGiftPaidEvent>().Single(e => e.FrameId == rootId);
        s.Need(s.V().Hand.Single().Id == match && failed.All(id => s.G.CardMovements.Last(m => m.CardId == id).To == CardLocation.Processing) &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == rootId).MobileSpXuShuJianyan!.Stage == "gift-children" &&
            !s.G.CardMovements.Any(m => failed.Contains(m.CardId) && m.To == CardLocation.DiscardPile),
            "The male owner's actual obtain child starts after the matching gift, with every unmatched card still outside the discard pile.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.V().Hand.Single().Id == match && failed.All(id => s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Processing &&
                m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.mobile:jianyan.unmatched" && m.Sequence > gift.MovementSequence) == 1) &&
            s.E<MobileSpXuShuJianyanRevealedEvent>().Count(e => e.FrameId == rootId) == 3 &&
            s.E<MobileSpXuShuJianyanGiftPaidEvent>().Count(e => e.FrameId == rootId) == 1 &&
            s.E<MobileSpXuShuJianyanCleanupPaidEvent>().Single(e => e.FrameId == rootId).Count == 2 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == rootId && e.Completed) == 1 &&
            s.G.ResolutionStack.All(f => f.Id != rootId) && s.G.GetHumanLegalActions().All(a => a.ProgramSkillId != Skill),
            "Cold return discards only the two unmatched entities after the paid gift child, completes the original frame once and preserves the Play limit.");
    }
    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string id, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => Accept(new UseProgramSkillCommand(0, Driver, id, cards, targets, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected mobile SP Xu Shu command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 180 && !done(); i++)
            { Need(P is null, "Unexpected mobile SP Xu Shu prompt."); Accept(new AdvanceOneStepCommand(G.Revision)); }
            Need(done(), "Missing bounded mobile SP Xu Shu boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the original reveal pool and paid gift child."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P, V().Hp, V().MaxHp, V().HandCount })); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(109, 1, "fixture-mobile-sp-xu-shu", Hero, 3, 0, 0, 0, 0, [Driver, Watch],
                [new(CardKind.BaguaFormation, Suit.Club, 1), new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Spade, 3)], [],
                Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray(), [], new string('c', 64));
            var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, r));
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-sp-xu-shu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "boundary:xu-shu");
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new object[] {
                    new { id = "damage", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "damage", target = "selectedTarget", sourceRef = new { kind = "owner" }, amount = 1 } } },
                    new { id = "top", minCards = 1, maxCards = 1, sourceZones = new[] { "hand" }, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                        effects = new object[] { new { op = "captureSelectedCards", target = "owner", resultBind = "top" }, new { op = "moveBoundCards", target = "owner", sourceBind = "top", destination = "drawPileTop", awaitMovementTriggers = true } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "actual-gift-child", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch",
                    movementReasons = new[] { "skill-program.mobile:jianyan.obtain" }, optional = false, usageScope = "game", usageLimit = 1,
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" } } } } } } } };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "原生潜心与牌堆准备", description = "实际伤害与实际牌堆顶移动。" },
                [Watch] = new { name = "实际荐言获得孩子", description = "取得命中牌后再清理其他亮出牌。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Peer, "真实同行", "supporter", "standard:none", "wei", 5));
            b.AddDeck(new(Deck, "固定正常牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动SP徐庶原生流程", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
