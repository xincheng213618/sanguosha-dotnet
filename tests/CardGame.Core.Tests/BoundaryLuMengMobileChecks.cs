using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryLuMengMobileChecks
{
    private const string Hero = "boundary:lu-meng-mobile", Skill = "boundary:qinxue-mobile", Owner = "fixture:mobile-lu-meng-owner",
        Driver = "fixture:mobile-lu-meng-driver", Hp = "fixture:mobile-lu-meng-maximum", Mode = "identity:classic-mobile-lu-meng";

    public static void PreparationThresholdsGrantOnlyGongxinAfterNativeMaximumChildCold()
    {
        var small = Scene.Start(4); small.Play();
        small.Need(small.V(0).HandCount - small.V(0).Hp == 2 && !small.E<ProgramBindingStartedEvent>().Any(e => e.SkillId == Skill) &&
            !small.V(0).Skills!.Any(s => s.ContentId is "classic:gongxin" or "boundary:botu"),
            "The mobile four-player preparation does not awaken at exactly two excess hand cards and never inherits OL Botu.");
        small.Use("gain-one"); small.Play();
        small.Need(small.V(0).HandCount - small.V(0).Hp == 3, "One actual native draw reaches the official inclusive small-table threshold.");
        var maximum = small.V(0).MaxHp; var hand = small.V(0).HandCount; var turn = small.G.State.TurnNumber;
        small.Use("extra"); small.Play(); small.End(); small.AtMaximumChild();
        var root = small.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill); var id = root.Id;
        var child = small.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(f => f.ResumeFrameId == id);
        small.Need(root.InstructionIndex == 1 && root.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } &&
            small.G.State.TurnNumber > turn && small.E<TurnEndedEvent>().Any(e => e.TurnNumber == turn && e.ActorSeat == 0) &&
            child.Change is { Kind: HpChangeKind.MaximumHp, ParentFrameId: var parent, TargetSeat: 0 } && parent == id &&
            small.V(0).MaxHp == maximum - 1 && small.V(0).HandCount == hand &&
            !small.V(0).Skills!.Any(s => s.ContentId == "classic:gongxin") &&
            small.E<ProgramBindingStartedEvent>().Where(e => e.SkillId == Skill).All(e => e.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow),
            "Three excess cards qualify the next real preparation, after the actual previous turn ended: native maximum HP is paid on the original frame before granting Gongxin or any OL benefit.");
        small.Cold(); small.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); small.Play();
        small.Need(small.V(0).MaxHp == maximum - 1 && small.V(0).HandCount == hand &&
            small.V(0).Skills!.Count(s => s.ContentId == "classic:gongxin") == 1 &&
            !small.V(0).Skills!.Any(s => s.ContentId is "boundary:qinxue" or "boundary:botu") &&
            small.E<MaximumHpChangedEvent>().Count(e => e.SkillId == Skill && e.Delta == -1) == 1 &&
            small.E<SkillsAcquiredEvent>().Count(e => e.SourceSkillId == Skill && e.PlayerSeat == 0 && e.SkillIds.SequenceEqual(["classic:gongxin"])) == 1 &&
            small.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == id && e.Completed) == 1 &&
            !small.G.CardMovements.Any(m => m.Reason.Value == "skill-program.boundary:qinxue-mobile.Draw") &&
            small.G.GetHumanLegalActions().Any(a => a.ProgramSkillId == "classic:gongxin"),
            "Cold returns to the same preparation, pays maximum HP once, grants the actual usable Gongxin once, and does not add the OL draw/recovery choice or Botu.");

        var large = Scene.Start(8); large.AtMaximumChild();
        var largeRoot = large.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        large.Need(large.V(0).HandCount == 6 && large.V(0).Hp == 3 && large.V(0).MaxHp == 3 &&
            largeRoot.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } &&
            large.E<MaximumHpChangedEvent>().Count(e => e.SkillId == Skill && e.Delta == -1) == 1 &&
            !large.G.Events.Any(e => e.Payload is ProgramSkillStartedEvent { SkillId: Driver }),
            "At eight original seats the same six-card, HP4 opening qualifies at exactly two excess cards before any driver activation; its real maximum payment clamps current HP to three.");
        large.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); large.Play();
        large.Need(large.V(0).HandCount == 6 && large.V(0).Skills!.Any(s => s.ContentId == "classic:gongxin") &&
            large.E<SkillsAcquiredEvent>().Count(e => e.SourceSkillId == Skill && e.SkillIds.SequenceEqual(["classic:gongxin"])) == 1,
            "The eight-player inclusive threshold completes the same native maximum-HP and Gongxin flow without extra draws.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game, int count)
    {
        public GameEngine G = game;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public PendingDecision? P => Enumerable.Range(0, count).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void AtMaximumChild() => Reach(() => P?.SkillPrompt?.SkillId == Hp);
        public void Use(string id) => Accept(new UseProgramSkillCommand(0, Driver, id, [], [], G.Revision, P!.PromptId));
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Lu Meng command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 240 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select) { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } foreign) { Accept(new EndPlayPhaseCommand(foreign.PlayerSeat, G.Revision, foreign.PromptId)); continue; }
                if (P is { Kind: DecisionKind.SkipDiscardPolicy }) { Answer(c => c.Parameters.GetValueOrDefault("action") == "discard-policy-use"); continue; }
                Need(P is null, "Unexpected native mobile Lu Meng boundary.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The small fixed mobile Lu Meng scene missed its native boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the paid native maximum-HP child and original preparation without repeating payment."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, count).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool condition, string text) { if (!condition) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Hand = V(0).HandCount, Hp = V(0).Hp, Maximum = V(0).MaxHp })); }
        public static Scene Start(int count)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(count));
            var scene = new Scene(registry, GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = count, HumanSeat = 0, HumanSeats = Enumerable.Range(0, count).ToArray(), HumanRole = Role.Rebel,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 16 }, registry), count);
            scene.Accept(new StartGameCommand()); return scene;
        }
    }
    private static string Peer(int seat) => "fixture:mobile-lu-meng-peer-" + seat;

    private sealed class Fixture(int count) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-lu-meng-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "boundary:lu-meng", "classic:shen-lu-meng");
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new object[] {
                    new { id = "gain-one", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "draw", target = "owner", amount = 1 } } },
                    new { id = "extra", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "pendExtraTurn", target = "owner" } } } } },
                new { id = Hp, revision = 1, triggers = new[] { new { id = "native-maximum-child", window = "afterHealthChanged", subject = "owner", optional = false,
                    usageScope = "game", usageLimit = 1, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" } } } } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实门槛驱动", description = "实际多摸一张并进入下一准备。" },
                [Hp] = new { name = "真实体力上限子窗", description = "原生减体力上限后、授技前暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Owner, "移动吕蒙资格场景", "supporter", Skill, "wu", 4, AdditionalSkillIds: ["boundary:keji", Driver, Hp]));
            foreach (var seat in Enumerable.Range(1, count - 1)) b.AddGeneral(new(Peer(seat), "原生同行", "supporter", "standard:none", "wei", 4));
            const string deck = "fixture:mobile-lu-meng-deck";
            b.AddDeck(new(deck, "固定真实手牌差", 6, 0, []) { PhysicalCards = Enumerable.Range(0, 100).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 6)).ToArray() });
            b.AddMode(new(Mode, "移动勤学人数与准备门槛", count, count, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = count - 2 }, deck,
                GeneralCandidateCount: count, GeneralPoolIds: [Owner, .. Enumerable.Range(1, count - 1).Select(Peer)]));
        }
    }
}
