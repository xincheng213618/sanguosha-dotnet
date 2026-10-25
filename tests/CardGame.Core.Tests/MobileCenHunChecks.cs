using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileCenHunChecks
{
    private const string Skill = "mobile:jishe", Driver = "fixture:mobile-cen-hun-clear", Owner = "fixture:mobile-cen-hun-owner",
        Mode = "identity:classic-mobile-cen-hun-fixture", Deck = "fixture:mobile-cen-hun-deck";

    public static void PositiveOneLimitAndZeroOrMultipleEndingTargetsCompleteCold()
    {
        foreach (var targets in new int[][] { [], [1, 2] })
        {
            var s = Scene.Start(); s.Play(); var hp = s.V(0).Hp; var initial = s.V(0).Hand.Count;
            for (var i = 0; i < hp; i++)
            {
                s.Need(s.G.GetHumanLegalActions().Any(a => a.ProgramSkillId == Skill), "Current mobile Jishe remains available at a positive hand limit, including exactly one.");
                s.Use(Skill, "draw-and-reduce-limit"); s.Play();
            }
            s.Need(s.V(0).Hand.Count == initial + hp && !s.G.GetHumanLegalActions().Any(a => a.ProgramSkillId == Skill),
                "The one-to-zero use draws one actual card and spends the last hand-limit point; zero prevents another use.");
            s.Use(Driver, "clear"); s.Play(); s.Need(s.V(0).Hand.Count == 0, "The original real hand was natively discarded before ending.");
            s.End(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill);
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-targets") == true);
            s.Need(s.P!.Choices.Any(c => c.Targets.Count == 0) && s.P.Choices.Any(c => c.Targets.SequenceEqual([1, 2])) &&
                s.P.Choices.All(c => c.Targets.Count <= hp), "The standard target UI offers zero through current-HP real recipients.");
            var frame = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
            s.Cold(); s.Answer(c => c.Targets.SequenceEqual(targets)); s.Reach(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
            s.Need(Enumerable.Range(0, 4).All(seat => s.V(seat).IsChained == targets.Contains(seat)) &&
                s.Facts<ProgramChainedStateSetEvent>().Count(e => e.SkillId == Skill) == targets.Length &&
                s.Facts<ProgramBindingResolvedEvent>().Count(e => e.FrameId == frame.Id && e.Activated && e.Completed) == 1,
                "An explicit zero-target choice completes once without chaining anyone; the ordinary two-target choice chains exactly those real recipients.");
        }
    }
    private static string Peer(int seat) => "fixture:mobile-cen-hun-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string skill, string activation) => Accept(new UseProgramSkillCommand(0, skill, activation, [], [], G.Revision, P!.PromptId));
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.Single(predicate).Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 160 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p)
                { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : Peer(p.PlayerSeat), G.Revision, p.PromptId)); continue; }
                Need(P is null, "Unexpected native mobile Cen Hun boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The small fixed Jishe fixture missed its next native boundary.");
        }
        public void Cold()
        {
            var views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray();
            var events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(views.SequenceEqual(Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i)))) &&
                events.SequenceEqual(G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType()))),
                "Cold restores the pending explicit zero-or-multiple recipient choice without repeating draw or hand discard.");
        }
        public void Need(bool condition, string message)
        { if (!condition) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(P)); }
        private void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected native Jishe command."); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var g = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
                HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 5 }, r);
            var s = new Scene(g, r); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-cen-hun", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:cen-hun", "classic:cen-hun");
            var c = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,"activations":[
            {"id":"clear","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
            "effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["hand"]}]}]}]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "原生清手牌", description = "真实结束阶段准备。" } } }));
            b.AddSkill(new(Driver, "原生清手牌", "真实结束阶段准备。") { Program = c.Programs[Driver], ProgramPresentation = c.Presentations[Driver] });
            b.AddGeneral(new(Owner, "移动岑昏完整极奢", "supporter", Skill, "wu", 3, [Driver]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "真实同桌", "supporter", "standard:none", "wu", 4));
            b.AddDeck(new(Deck, "固定真实手牌", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(Mode, "极奢真实零选与多人横置", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1,
                [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck, GeneralCandidateCount: 4,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
