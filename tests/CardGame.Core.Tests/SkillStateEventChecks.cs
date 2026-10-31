using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class SkillStateEventChecks
{
    private const string Driver = "fixture:state-event-driver", Probe = "fixture:state-event-probe";
    private const string Locked = "fixture:state-event-locked", Fengdong = "shenwu:fengdong", Wansha = "boundary:wansha";
    private const string Mode = "identity:classic-skill-state-events", Owner = "fixture:state-event-owner";

    public static void NativeTurnAndDyingDeclarationsFollowOriginalWindowsCold()
    {
        TurnApplicationsOutliveTheirSourceAndDoNotRepeat();
        foreach (var survive in new[] { true, false }) DyingApplicationsPrecedeCandidatesAndEndWithTheirWindow(survive);
    }

    private static void TurnApplicationsOutliveTheirSourceAndDoNotRepeat()
    {
        var s = Scene.Start(fengdong: true);
        Require(s.Effective(0, Probe) && !s.Effective(1, Probe) && !s.Effective(2, Probe) &&
            s.Effective(1, Locked) && s.Leases().Length == 4,
            "The real TurnStarted early entry applies Fengdong before the ordinary start skills, exempting its owner and locked skills.");
        s.Play(0);
        Require(s.E<ProgramBindingResolvedEvent>().Where(e => e.SkillId == Probe && e.BindingId == "turn")
            .Select(e => e.OwnerSeat).SequenceEqual([0]),
            "Only the owner's ordinary non-locked start trigger executes during the frozen turn.");
        s.Use("lose-fengdong"); s.Play(0);
        Require(!s.Effective(0, Fengdong) && !s.Effective(1, Probe) && s.Leases().Length == 4,
            "Losing the actual source mid-turn retains the already applied turn restriction.");
        s.Cold(); s.Play(1);
        Require(s.Leases().Length == 0 && s.Effective(1, Probe) && s.E<SkillStateExpiredEvent>().Length == 4,
            "The actual owner TurnEnded clears the four applications once after source loss.");
        s.Play(0);
        Require(s.Leases().Length == 0 && s.Effective(1, Probe),
            "The next owner turn cannot reissue a declaration whose source was lost.");

        s.Use("gain-fengdong"); s.Play(0);
        Require(s.Effective(0, Fengdong) && !s.Effective(1, Probe) && s.Leases().Length == 4,
            "A genuine mid-turn grant admits the same declaration into the already active native turn.");
        s.Use("lose-fengdong"); s.Play(0); s.Cold();
        Require(!s.Effective(1, Probe) && s.Leases().Length == 4,
            "The granted source can also be lost without cancelling this turn's accepted application.");
        s.Play(1); s.Play(0);
        Require(s.Leases().Length == 0 && s.Effective(1, Probe) && s.E<SkillStateExpiredEvent>().Length == 8,
            "The second native turn end clears the new application once and the following turn does not repeat it.");
        s.Cold();
    }

    private static void DyingApplicationsPrecedeCandidatesAndEndWithTheirWindow(bool survive)
    {
        var s = Scene.Start(wansha: true); s.Play(0);
        s.Use("hurt", 1);
        s.Reach(() => s.Pending?.Choices.Any(c => c.Parameters.GetValueOrDefault("skill-id") == Probe &&
            c.Parameters.GetValueOrDefault("program-action") == "activate") == true);
        var dying = s.G.ResolutionStack.OfType<DyingFrame>().Single();
        var entry = s.G.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>()
            .Single(f => f.Window == SkillProgramTriggerWindow.DyingEntering);
        Require(entry.Candidates.Where(c => c.SkillId == Probe).Select(c => c.OwnerSeat).Order().SequenceEqual([0, 1]) &&
            s.Effective(0, Probe) && s.Effective(1, Probe) && !s.Effective(2, Probe) && !s.Effective(3, Probe) &&
            s.Effective(2, Locked) && s.Leases().Length == 4,
            "Wansha applies before ordinary Dying candidates are collected: issuer and victim are exempt, other non-locked skills are suppressed.");
        s.Cold();
        Require(s.Leases().Length == 4 && s.G.ResolutionStack.OfType<DyingFrame>().Single().Id == dying.Id,
            "Cold recovery retains one application per recipient on the original native Dying window.");
        s.Reach(() => s.Pending is { Kind: DecisionKind.RescueDying });
        Require(!s.E<ProgramBindingResolvedEvent>().Any(e => e.SkillId == Probe && e.BindingId == "dying" && e.OwnerSeat is 2 or 3),
            "Suppressed peers never receive an ordinary non-locked Dying activation.");
        if (survive) s.Answer(c => c.Parameters.GetValueOrDefault("response") == "peach");
        s.Play(0);
        Require(s.G.State.Players[1].IsAlive == survive &&
            s.E<DyingResolvedEvent>().Count(e => e.ResolutionId == dying.Id && e.Survived == survive) == 1 &&
            s.Leases().Length == 0 && s.E<SkillStateExpiredEvent>().Length == 4 && s.Effective(2, Probe),
            "Both a real Peach rescue and native death finish the original Dying, clearing its restriction exactly once.");
        s.Cold();
        Require(s.E<PlayerDyingEvent>().Count(e => e.ResolutionId == dying.Id) == 1 &&
            s.E<SkillStateExpiredEvent>().Length == 4 && s.Leases().Length == 0,
            "Completed Dying recovery does not reissue an application or repeat its expiry.");
    }

    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? Pending => Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        private CharacterState Player(int seat) => ((IReadOnlyList<CharacterState>)FixtureRuntimeAccess.Engine
            .GetField("_players", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(G)!)[seat];
        public bool Effective(int seat, string skill) => G.NationalSkillsEntries.HasRuntimeSkill(Player(seat), skill);
        public SkillStateRestriction[] Leases() => Enumerable.Range(0, 4).SelectMany(seat => Player(seat).SkillGrants.Restrictions).ToArray();
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Use(string activation, params int[] targets) => Submit(new UseProgramSkillCommand(0, Driver, activation,
            [], targets, G.Revision, Pending!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = Pending!; Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Play(int seat) => Reach(() => Pending is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void Reach(Func<bool> stop)
        {
            for (var step = 0; step < 450; step++)
            {
                if (stop()) return;
                if (Pending is not { } p) { Submit(new AdvanceOneStepCommand(G.Revision)); continue; }
                if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip"))
                { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); continue; }
                switch (p.Kind)
                {
                    case DecisionKind.SelectGeneral:
                        Submit(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : Peer(p.PlayerSeat), G.Revision, p.PromptId)); break;
                    case DecisionKind.PlayCard:
                        Submit(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId)); break;
                    case DecisionKind.DiscardCards:
                        Submit(new DiscardCardsCommand(p.PlayerSeat, p.ValidCardIds.Take(p.RequiredCardCount).ToArray(), p.PromptId, G.Revision)); break;
                    case DecisionKind.RescueDying:
                        Answer(c => c.Parameters.GetValueOrDefault("response") == "let-die"); break;
                    default: throw new InvalidOperationException("Unexpected state-event prompt: " + JsonSerializer.Serialize(p));
                }
            }
            throw new InvalidOperationException("The shared state-event fixture missed its bounded native window.");
        }
        private void Submit(GameCommand command)
        { var result = G.Submit(command); Require(result.Accepted, result.Error?.Message ?? "Native state-event command rejected."); }
        public void Cold()
        {
            var views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat))).ToArray();
            var ids = Leases().Select(state => state.Id).Order(StringComparer.Ordinal).ToArray();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Require(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat)) == views[seat]) &&
                Leases().Select(state => state.Id).Order(StringComparer.Ordinal).SequenceEqual(ids),
                "Native event applications and their public behavior survive command replay without duplicate rows.");
        }
        public static Scene Start(bool fengdong = false, bool wansha = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(fengdong, wansha));
            var scene = new Scene(GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4,
                HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
                UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 16 }, registry), registry);
            scene.Submit(new StartGameCommand());
            scene.Reach(() => scene.E<TurnStartedEvent>().Any(e => e.ActorSeat == 0));
            return scene;
        }
    }

    private static string Peer(int seat) => $"fixture:state-event-peer-{seat}";
    private sealed class Fixture(bool fengdong, bool wansha) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:skill-state-events", "1.0.0", "Shared native state event windows");
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "boundary:jia-xu");
            var assembly = typeof(StandardContentPackage).Assembly;
            using var rules = new StreamReader(assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.shenwu-expanded.rules.json")!);
            using var presentation = new StreamReader(assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.shenwu-expanded.presentation.json")!);
            var production = SkillProgramCatalog.Load(rules.ReadToEnd(), presentation.ReadToEnd());
            builder.AddSkill(new(Fengdong, Fengdong, "Production turn-start declaration")
                { Program = production.Programs[Fengdong], Tags = SkillTag.Locked });
            var catalog = SkillProgramCatalog.Load($$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Driver}}}","revision":1,"activations":[
                {"id":"gain-fengdong","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                 "effects":[{"op":"grantSkills","target":"owner","skillIds":["{{{Fengdong}}}"]}]},
                {"id":"lose-fengdong","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                 "effects":[{"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{{Fengdong}}}"]}}]},
                {"id":"hurt","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                 "effects":[{"op":"loseHp","target":"selectedTarget","amount":3}]}]},
              {"id":"{{{Probe}}}","revision":1,"triggers":[
                {"id":"turn","window":"turnStartBeforeNormalFlow","subject":"owner","turnOwnerScope":"anyLiving","optional":false,"effects":[{"op":"draw","target":"owner","amount":1}]},
                {"id":"dying","window":"dyingEntering","subject":"any","optional":true,"effects":[{"op":"draw","target":"owner","amount":1}]}]},
              {"id":"{{{Locked}}}","revision":1,"activations":[{"id":"pulse","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                "effects":[{"op":"draw","target":"owner","amount":1}]}]}
            ]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Probe, Locked }.ToDictionary(id => id, id => new { name = id, description = "Shared native state window" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, Tags = id == Probe ? SkillTag.None : SkillTag.Locked });
            builder.AddGeneral(new(Owner, "Event issuer", "supporter", Driver, "wei", 8,
                new[] { Probe, Locked }.Concat(fengdong ? [Fengdong] : []).Concat(wansha ? [Wansha] : []).ToArray()));
            for (var seat = 1; seat < 4; seat++)
                builder.AddGeneral(new(Peer(seat), "Event target", "supporter", Probe, "qun", 3, [Locked]));
            builder.AddDeck(new("fixture:state-event-deck", "Native rescue cards", 4, 2, [])
            { PhysicalCards = Enumerable.Range(0, 160).Select(i => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, i % 13 + 1)).ToArray() });
            builder.AddMode(new(Mode, "Native skill state events", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Renegade)] = 3 }, "fixture:state-event-deck",
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
