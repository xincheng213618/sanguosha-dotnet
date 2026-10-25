using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryMengHuoMobileChecks
{
    private const string Hero = "boundary:meng-huo-mobile", Skill = "boundary:zaiqi-mobile", Driver = "fixture:mobile-meng-huo-driver",
        Gain = "fixture:mobile-meng-huo-gain", Ending = "fixture:mobile-meng-huo-ending", Peer = "fixture:mobile-meng-huo-peer";
    private const string DrawReason = "skill-program.red-discard-recovery.draw";

    public static void DiscardEndAllowsSelfAndPaidDrawReturnsCold()
    {
        var s = Scene.Start(); s.Play();
        s.Accept(new UseProgramSkillCommand(0, Driver, "red-budget", [], [], s.G.Revision, s.P!.PromptId)); s.Play();
        s.Need(s.V(0).Hp == 2 && s.V(0).HandCount == 0 && s.G.CardMovements.Count(m => m.To == CardLocation.DiscardPile && m.From == CardLocation.Hand(0)) == 2,
            "The fixed native producer really loses two HP and puts its two red hand entities into the discard pile during this actual turn.");
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId)); s.Hold(Skill);
        var root = s.Root!.Id;
        s.Need(s.Root.WindowContext?.Window == SkillProgramTriggerWindow.DiscardPhaseEnded && s.Root.RedDiscardRecoveryDraft is { Budget: 2, Committed: false } &&
            s.P!.Choices.Any(c => c.Targets.SequenceEqual([0])) && !s.E<ProgramBindingStartedEvent>().Any(e => e.SkillId == Ending),
            "Mobile Zaiqi opens at the actual discard end before TurnEnding and permits its own living owner within the true red-card budget.");
        s.Mode("seat:0"); s.Mode("seat:1"); s.Mode("finish"); s.Mode("draw"); s.Hold(Gain);
        s.Need(s.Root?.Id == root && s.Root.RedDiscardRecoveryDraft is { Committed: true, Cursor: 1, AskingSeat: null } && s.Root.PendingMovementContinuation is not null &&
            s.V(0).HandCount == 1 && s.Moved() == 1 && s.E<ProgramRedDiscardRecoveryCommittedEvent>().Single(e => e.FrameId == root).Seats.SequenceEqual([0, 1]),
            "The owner actually draws once from the committed self/ally selection, then suspends on the original paid gain child before asking the ally.");
        s.Cold(); s.Continue(); s.Hold(Ending);
        s.Need(s.V(0).Hp == 3 && s.V(0).HandCount == 1 && s.V(1).HandCount == 0 && s.Moved() == 1 &&
            s.E<ProgramRedDiscardRecoveryChosenEvent>().Count(e => e.FrameId == root && e.ChooserSeat == 0 && e.Option == "draw") == 1 &&
            s.E<ProgramRedDiscardRecoveryChosenEvent>().Count(e => e.FrameId == root && e.ChooserSeat == 1 && e.Option == "recovery") == 1 &&
            s.E<RecoveryAppliedEvent>().Count(e => e.TargetSeat == 0 && e.Amount == 1) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == root && e.Completed) == 1 && s.G.ResolutionStack.All(f => f.Id != root),
            "Cold returns to the same paid owner draw, asks the wounded owner's ally once, applies one real recovery, and completes Zaiqi before the separate TurnEnding boundary.");
        s.Cold();
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Skill);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public int Moved() => G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason.Value == DrawReason);
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Hold(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill);
        public void Mode(string mode) => Answer(c => c.Parameters.GetValueOrDefault("mode") == mode);
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Answer(Func<PromptChoice, bool> match) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(match).Id, G.Revision)); }
        public void Reach(Func<bool> ready)
        { for (var i = 0; i < 180; i++) { if (ready()) return; Need(P is not { PlayerSeat: 0 }, "Unexpected native mobile Meng Huo boundary."); Accept(new AdvanceOneStepCommand(G.Revision)); } throw new InvalidOperationException("Bounded Zaiqi flow was not reached."); }
        public void Cold()
        { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the actual discard-end budget, committed self selection and original paid draw child."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { P, G.State.CurrentSeat, G.State.Phase, Players = Enumerable.Range(0, 4).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }) })); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Meng Huo command."); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var peer = new ShanheCombatantDefinition(Peer, Hp: 8) { StartingHand = [] };
            var setup = new ShanheBattleSetup(37, 1, "mobile-meng-huo-discard-end", Hero, 4, 0, 0, 0, 0, [Driver, Gain, Ending],
                [new(CardKind.Dodge, Suit.Heart, 7), new(CardKind.Dodge, Suit.Diamond, 8)], [], [peer, peer], [peer], new string('c', 64));
            var engine = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry);
            var scene = new Scene(engine, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-meng-huo-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "boundary:meng-huo", Hero);
            object Pause() => new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } };
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] { new { id = "red-budget", usesPerTurn = (int?)null, usesPerGame = (int?)null, minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving",
                    effects = new object[] { new { op = "loseHp", target = "owner", amount = 2 }, new { op = "discardOwnedZoneCards", target = "owner", zones = new[] { "hand" } } } } } },
                new { id = Gain, revision = 1, triggers = new[] { new { id = "paid-self-draw", window = "cardsGained", subject = "owner", optional = false, destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { DrawReason }, effects = new[] { Pause() } } } },
                new { id = Ending, revision = 1, triggers = new[] { new { id = "separate-ending", window = "turnEnding", subject = "owner", optional = false, effects = new[] { Pause() } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实红牌预算", description = "实际失体力和红牌弃置" } };
            foreach (var id in new[] { Gain, Ending }) labels[id] = new { name = "再起原生边界", description = "原生摸牌或后继结束阶段", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 8));
            const string deck = "fixture:mobile-meng-huo-deck";
            b.AddDeck(new(deck, "固定再起牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动再起真实弃牌结束", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
