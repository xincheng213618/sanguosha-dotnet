using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryXunYuMobileChecks
{
    private const string Hero = "boundary:xun-yu-mobile", Skill = "boundary:jieming-mobile", Driver = "fixture:mobile-xun-yu-driver",
        Gain = "fixture:mobile-xun-yu-gain", Bonus = "fixture:mobile-xun-yu-bonus", Peer = "fixture:mobile-xun-yu-peer";
    private const string DrawReason = "skill-program.boundary:jieming-mobile.Draw", BonusReason = "skill-program.boundary:jieming-mobile.owner-bonus";

    public static void PerPointDrawUsesPostChildHandAndBonusReturnsCold()
    {
        var s = Scene.Start(); s.Play(); var initial = s.V().HandCount;
        s.Accept(new UseProgramSkillCommand(0, Driver, "injure", [], [], s.G.Revision, s.P!.PromptId));
        s.Offer(); s.Activate(); s.Self(); s.Hold(Gain); var first = s.Root!.Id;
        s.Need(s.V().Hp == 1 && s.V().HandCount == 3 && s.V().MaxHp == 3 && initial == 1 && s.BonusCards() == 0,
            "One actual two-point injury issues separate mobile occurrences; the first real two-card draw reaches MaxHp before its movement child.");
        s.Cold(); s.Continue(); s.DiscardGainCard(); s.Hold(Bonus);
        s.Need(s.Root?.Id == first && s.Root.PendingMovementContinuation is not null && s.V().HandCount == 3 && s.BonusCards() == 1 &&
            s.Moved(DrawReason) == 2 && s.Moved("skill-program." + Gain + ".SelectAndMoveOwnedCard") == 1,
            "The child's actual discard makes the target short only after the original draw; Jieming then pays one real owner bonus and waits on its native movement child.");
        s.Cold(); s.Continue(); s.Offer(); s.Activate(); s.Self(); s.Hold(Gain); var second = s.Root!.Id;
        s.Need(second != first && s.V().HandCount == 5 && s.BonusCards() == 1,
            "The second actual damage point owns a new optional binding and another two-card draw, without replaying the first paid bonus.");
        s.Cold(); s.Continue(); s.DiscardGainCard(); s.Play();
        s.Need(s.V().HandCount == 4 && s.V().Hp == 1 && s.Moved(DrawReason) == 4 && s.BonusCards() == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Skill && e.Completed && (e.FrameId == first || e.FrameId == second)) == 2 &&
            s.E<DamageAppliedEvent>().Count(e => e.TargetSeat == 0 && e.Amount == 2) == 1 && s.G.ResolutionStack.All(f => f.Id != first && f.Id != second),
            "After the second child's discard the target still exceeds MaxHp, so no second bonus is issued; both original per-point frames complete with four target draws and one bonus total.");
        s.Cold();
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Skill);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public int Moved(string reason) => G.CardMovements.Count(m => m.Reason.Value == reason);
        public int BonusCards() => Moved(BonusReason);
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Offer() => Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Self() { Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Targets.SequenceEqual([0]))); Answer(c => c.Targets.SequenceEqual([0])); }
        public void Hold(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill);
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void DiscardGainCard() { Reach(() => P?.SkillPrompt?.SkillId == Gain && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card")); Answer(c => c.Cards.Count == 1); }
        public void Answer(Func<PromptChoice, bool> match) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(match).Id, G.Revision)); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 180; i++) { if (ready()) return; Need(P is not { PlayerSeat: 0 }, "Unexpected native Xun Yu boundary."); Accept(new AdvanceOneStepCommand(G.Revision)); }
            throw new InvalidOperationException("Bounded mobile Xun Yu flow was not reached.");
        }
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the exact per-point target draw, real child discard and paid one-card bonus.");
        }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { P, G.State.CurrentSeat, G.State.Phase, Hand = V().HandCount, Bonus = BonusCards() })); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Xun Yu command."); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var peer = new ShanheCombatantDefinition(Peer, Hp: 8) { StartingHand = [] };
            var setup = new ShanheBattleSetup(37, 1, "mobile-xun-yu-native-draw", Hero, 3, 0, 0, 0, 0, [Driver, Gain, Bonus],
                [new(CardKind.Dodge, Suit.Heart, 7)], [], [peer, peer], [peer], new string('b', 64));
            var engine = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry);
            var scene = new Scene(engine, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-xun-yu-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "boundary:xun-yu", Hero);
            object Pause() => new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } };
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] { new { id = "injure", usesPerTurn = (int?)null, minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", effects = new[] { new { op = "damage", target = "owner", amount = 2 } } } } },
                new { id = Gain, revision = 1, triggers = new[] { new { id = "target-draw-child", window = "cardsGained", subject = "owner", optional = false,
                    destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { DrawReason },
                    // Native ordinals qualify only the last physical card of each complete draw invocation; the intervening invocation is the one-card bonus.
                    condition = new { kind = "any", children = new[] { new { kind = "nativeDrawActualTurnOrdinalIs", value = 1 }, new { kind = "nativeDrawActualTurnOrdinalIs", value = 3 } } },
                    effects = new object[] { Pause(), new { op = "selectAndMoveOwnedCard", target = "owner", chooserRef = new { kind = "owner" }, cardOwnerRef = new { kind = "owner" }, zones = new[] { "hand" }, count = 1, destination = "discardPile", awaitMovementTriggers = true } } } } },
                new { id = Bonus, revision = 1, triggers = new[] { new { id = "paid-owner-bonus", window = "cardsGained", subject = "owner", optional = false,
                    destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { BonusReason }, effects = new[] { Pause() } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实两点伤害", description = "真实伤害生产者" } };
            foreach (var id in new[] { Gain, Bonus }) labels[id] = new { name = "节命真实移动子窗", description = "真实付款后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 8));
            const string deck = "fixture:mobile-xun-yu-deck";
            b.AddDeck(new(deck, "固定节命牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动节命真实伤害与摸牌", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
