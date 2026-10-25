using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundarySunJianMobileChecks
{
    private const string Hero = "boundary:sun-jian-mobile", Skill = "boundary:polu-mobile", Driver = "fixture:mobile-sun-jian-driver",
        Gain = "fixture:mobile-sun-jian-gain", Peer = "fixture:mobile-sun-jian-peer";
    private const string DrawReason = "skill-program.boundary:polu-mobile.Draw";

    public static void KillThenOwnDeathShareGameCountAndNativeDrawChildrenCold()
    {
        var s = Scene.Start(); s.Play();
        s.Accept(new UseProgramSkillCommand(0, Driver, "kill", [], [3], s.G.Revision, s.P!.PromptId));
        s.Offer(); s.Activate(); s.Targets([0, 1]); s.HoldGain();
        var first = s.Root!.Id;
        s.Need(!s.V(3).IsAlive && s.V(0).IsAlive && s.V(1).IsAlive && s.V(2).IsAlive &&
            s.Root.MobileSunJianPolu is { Count: 1, RecipientIndex: 0, AwaitingMovement: true } &&
            s.V(0).HandCount == 1 && s.V(1).HandCount == 0 && s.Moved(0) == 1 && s.Moved(1) == 0 &&
            s.G.CreateSnapshot(2).Players[0].Hand.Count == 0 && s.G.CreateSnapshot(2).Players[0].HandCount == 1,
            "The first genuine kill pays one private native draw to the first selected role and pauses before drawing for the next recipient.");
        s.Cold(); s.Continue(); s.Play();
        s.Need(s.V(0).HandCount == 1 && s.V(1).HandCount == 1 && s.Moved(0) == 1 && s.Moved(1) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == first && e.SkillId == Skill && e.Completed) == 1 &&
            s.G.ResolutionStack.All(f => f.Id != first),
            "Cold returns to the same paid recipient cursor, draws once for the remaining role and completes the first original Polu frame.");
        s.Accept(new UseProgramSkillCommand(0, Driver, "die", [], [], s.G.Revision, s.P!.PromptId));
        s.Offer(); s.Activate(); s.Targets([1, 2]);
        s.Reach(() => s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Skill && e.Completed) == 2);
        s.Need(!s.V(0).IsAlive && !s.V(3).IsAlive && s.V(1).IsAlive && s.V(2).IsAlive &&
            s.V(1).HandCount == 3 && s.V(2).HandCount == 2 && s.Moved(0) == 1 && s.Moved(1) == 3 && s.Moved(2) == 2 &&
            s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Skill && e.OwnerSeat == 0) == 2 &&
            s.G.CreateSnapshot(0).Players[1].Hand.Count == 0 && s.G.CreateSnapshot(0).Players[2].Hand.Count == 0 &&
            s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 3 && e.Amount == 8) == 1,
            "The owner's later real death shares the same game-wide invocation count: both surviving selected roles draw two, preserving private hands and the original one-card payments.");
        s.Cold();
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Skill);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public int Moved(int seat) => G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(seat) && m.Reason.Value == DrawReason);
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Offer() => Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Targets(int[] targets) { Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Targets.SequenceEqual(targets))); Need(P!.Choices.Any(c => c.Targets.Count == 0), "Arbitrary living Polu targets include the real zero-target option."); Answer(c => c.Targets.SequenceEqual(targets)); }
        public void HoldGain() => Reach(() => P?.SkillPrompt?.SkillId == Gain);
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Answer(Func<PromptChoice, bool> match) { var p = P!; var choice = p.Choices.FirstOrDefault(match); Need(choice is not null, "Missing exact current native Sun Jian choice."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision)); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 200; i++)
            {
                if (ready()) return;
                if (P is { PlayerSeat: 0, Kind: DecisionKind.RescueDying } dying && dying.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "pass"))
                { Answer(c => c.Parameters.GetValueOrDefault("response") == "pass"); continue; }
                Need(P is not { PlayerSeat: 0 }, "Unexpected native Sun Jian boundary.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Bounded mobile Sun Jian flow was not reached.");
        }
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the original death binding, shared game invocation and already-paid native recipient draw.");
        }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { P, Root, Players = Enumerable.Range(0, 4).Select(i => new { i, V(i).Hp, V(i).IsAlive, V(i).HandCount }) })); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Sun Jian command."); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var peer = new ShanheCombatantDefinition(Peer, Hp: 8) { StartingHand = [] };
            var setup = new ShanheBattleSetup(37, 1, "mobile-sun-jian-native-deaths", Hero, 4, 0, 0, 0, 0, [Driver, Gain], [], [], [peer, peer], [peer], new string('c', 64));
            var engine = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry);
            var s = new Scene(engine, registry); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-sun-jian-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "boundary:sun-jian", Hero);
            object Activation(string id, string op, string target, int amount, int targets) => new { id, minCards = 0, maxCards = 0, minTargets = targets,
                maxTargets = targets, targetKind = "otherLiving", usesPerTurn = (int?)null, effects = new[] { new { op, target, amount } } };
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] { Activation("kill", "damage", "selectedTarget", 8, 1), Activation("die", "loseHp", "owner", 4, 0) } },
                new { id = Gain, revision = 1, triggers = new[] { new { id = "paid-recipient-draw", window = "cardsGained", subject = "owner", optional = false,
                    destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { DrawReason },
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "paid", options = new[] { new { id = "continue" } } } } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实击杀及自身死亡", description = "原生伤害和失体力驱动" },
                [Gain] = new { name = "破虏原生实付摸牌子窗", description = "真实获得牌后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "固定两方同桌", "supporter", "standard:none", "wei", 8));
            const string deck = "fixture:mobile-sun-jian-deck";
            b.AddDeck(new(deck, "固定无桃摸牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动破虏两次真实死亡", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
