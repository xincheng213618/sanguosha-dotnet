using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryPangTongMobileChecks
{
    private const string Hero = "boundary:pang-tong-mobile", Chain = "boundary:lianhuan-mobile", Rebirth = "boundary:niepan-mobile",
        Driver = "fixture:mobile-pang-tong-driver", Watch = "fixture:mobile-pang-tong-discard", Peer = "fixture:mobile-pang-tong-peer", Deck = "fixture:mobile-pang-tong-deck";
    public static void HandOnlyChainAndProactiveNiepanShareDyingLimitWithPaidDiscardCold()
    {
        var s = Scene.Start(); s.Play();
        s.Card(s.G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Equip)); s.Play();
        s.Card(s.G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Lightning)); s.Play();
        var equipment = s.V().Equipment.Single().Id;
        var handClub = s.V().Hand.Single(c => c.Suit == Suit.Club).Id;
        s.Need(s.G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.IronChain && a.CardId == handClub && a.TargetSeats.Count == 3 && a.ConversionSource?.SkillId == Chain) &&
            s.G.GetHumanLegalActions().All(a => a.CardId != equipment || a.Kind is not (LegalActionKind.IronChain or LegalActionKind.Recast)),
            "Mobile Lianhuan keeps the third native Iron Chain target while refusing any equipped club as a use or recast material.");
        var recast = s.G.GetHumanLegalActions().Single(a => a.Kind == LegalActionKind.Recast && a.CardId == handClub && a.ConversionSource?.SkillId == Chain);
        var uses = s.E<CardUseDeclaredEvent>().Count();
        s.Accept(new RecastCardCommand(0, handClub, s.G.Revision, s.P!.PromptId) { ConversionSource = recast.ConversionSource }); s.Play();
        s.Need(s.E<CardUseDeclaredEvent>().Count() == uses && s.G.CardMovements.Count(m => m.CardId == handClub && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 1,
            "A real club hand recast pays once and draws without manufacturing a trick Use.");
        s.Use("prepare"); s.Play(); var original = s.V().Hand.Select(c => c.Id).Concat(s.V().Equipment.Select(c => c.Id)).Concat(s.V().Judgment.Select(c => c.Id)).ToArray();
        s.Need(s.V() is { Hp: 1, MaxHp: 4, IsChained: true, IsFaceDown: true, Equipment.Count: 1, Judgment.Count: 1 }, "The actual proactive rebirth starts with paid entities in all three owned zones and both altered states.");
        s.Accept(new UseProgramSkillCommand(0, Rebirth, "rebirth", [], [], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Rebirth); var rootId = root.Id;
        s.Need(root.TriggerId is null && root.InstructionIndex == 1 && s.V() is { HandCount: 0, Equipment.Count: 0, Judgment.Count: 0, Hp: 1, IsChained: true, IsFaceDown: true } &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(f => f.Batch.ParentFrameId == rootId),
            "The newly available Play activation first pays every original HEJ entity, then waits on its native discard child before drawing, recovering or restoring state.");
        s.Cold(); s.Option("continue");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == Rebirth) == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == Rebirth);
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish") == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish"); s.Play();
        s.Need(s.V() is { Hp: 3, MaxHp: 3, HandCount: 3, IsChained: false, IsFaceDown: false } && !s.V().Skills!.Any(k => k.ContentId == Rebirth) &&
            !s.V().Skills!.Any(k => k.ContentId is "classic:bazhen" or "classic:huoji" or "classic:kanpo") &&
            original.All(id => s.G.CardMovements.Count(m => m.CardId == id && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.boundary:niepan-mobile.DiscardOwnedZoneCards") == 1) &&
            s.G.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program.boundary:niepan-mobile.Draw") == 3 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == rootId && e.Completed) == 1,
            "Cold and actual source loss complete the original limited effect once: three actual draws, recovery to three and restoration, with no OL acquired skill or repeated discard.");
        s.Use("restore"); s.Play();
        s.Need(s.V().Skills!.Any(k => k.ContentId == Rebirth) && s.G.GetHumanLegalActions().All(a => a.ProgramSkillId != Rebirth),
            "Regranting the source does not reset the one native game entitlement shared with the dying entry.");
        s.Use("dying"); s.Reach(() => s.G.State.Winner != Winner.None);
        s.Need(s.E<PlayerDyingEvent>().Count(e => e.VictimSeat == 0) == 1 &&
            s.E<PlayerDiedEvent>().Count(e => e.VictimSeat == 0) == 1 &&
            s.E<ProgramSkillStartedEvent>().Count(e => e.OwnerSeat == 0 && e.SkillId == Rebirth) == 1 &&
            !s.E<ProgramBindingStartedEvent>().Any(e => e.OwnerSeat == 0 && e.SkillId == Rebirth),
            "After native regranting, a genuine later self-dying occurrence automatically passes without rescue cards and ends in actual death; the consumed shared entitlement permits neither a second activation nor a dying binding.");
    }
    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string id) => Accept(new UseProgramSkillCommand(0, Driver, id, [], [], G.Revision, P!.PromptId));
        public void Card(LegalAction a) => Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource });
        public void Option(string id) => Answer(c => c.Parameters.GetValueOrDefault("option-id") == id);
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected mobile Pang Tong command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 140 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.Nullification }) { Answer(c => c.Parameters.GetValueOrDefault("response") == "pass"); continue; }
                Need(P is null, "Unexpected mobile Pang Tong boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "Missing bounded mobile Pang Tong boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the original proactive limited receipt and paid HEJ child."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, Skill = p.SkillPrompt?.SkillId } : null, V().Hp, V().HandCount })); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var extra = Enumerable.Range(0, 6).Select(i => "fixture:mobile-pang-tong-dummy-" + i).ToArray();
            // The genuine awakening source-loss child pays one maximum HP before Niepan resumes.
            var setup = new ShanheBattleSetup(97, 1, "fixture-mobile-pang-tong", Hero, 1, 1, 0, 0, 0, [Driver, Watch, .. extra],
                [new(CardKind.Dodge, Suit.Club, 1), new(CardKind.BaguaFormation, Suit.Club, 2), new(CardKind.Lightning, Suit.Spade, 3), new(CardKind.Peach, Suit.Heart, 4)], [],
                Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray(), [], new string('a', 64));
            var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, r));
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-pang-tong-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new object[] {
                    new { id = "prepare", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                        effects = new object[] { new { op = "setChainedState", target = "owner", chained = true }, new { op = "setFaceState", target = "owner", faceDown = true } } },
                    new { id = "restore", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "grantSkills", target = "owner", skillIds = new[] { Rebirth } } } },
                    new { id = "dying", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "loseHp", target = "owner", amount = 4 } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "paid-original-discard", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" }, movementOccurrence = "perBatch",
                    movementReasons = new[] { "skill-program.boundary:niepan-mobile.DiscardOwnedZoneCards" }, optional = false, usageScope = "game", usageLimit = 1,
                    effects = new object[] { new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" } } }, new { op = "replaceSkillsOnAwakening", target = "owner", skillIds = new[] { "standard:none" } } } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实涅槃准备", description = "原生状态和限定重获资格" }, [Watch] = new { name = "原弃牌孩子", description = "真实HEJ费用后失去来源", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            foreach (var i in Enumerable.Range(0, 6)) b.AddSkill(new("fixture:mobile-pang-tong-dummy-" + i, "真实失技资格", "用于原生选择一次失去技能。"));
            b.AddGeneral(new(Peer, "原生同行", "supporter", "standard:none", "wei", 5));
            b.AddDeck(new(Deck, "固定正常补牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动庞统原生流程", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
