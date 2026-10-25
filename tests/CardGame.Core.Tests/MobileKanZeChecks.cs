using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileKanZeChecks
{
    private const string Driver = "fixture:mobile-kan-ze-driver", Owner = "fixture:mobile-kan-ze-owner",
        Mode = "identity:mobile-kan-ze-fixture", Deck = "fixture:mobile-kan-ze-deck";

    public static void OnlyFirstHeavyDamageConsumesProtectionAndKeepsDrawDebtCold()
    {
        foreach (var skill in new[] { "mobile:kuanshi", "ol:kuanshi" })
        {
            var s = Scene.Start(skill);
            s.Play(0); var hp = s.V(0).Hp;
            s.End(0); s.Reach(() => s.P?.SkillPrompt?.SkillId == skill &&
                s.P.Choices.Any(c => Key(c) == "activate"));
            s.Answer(c => Key(c) == "activate");
            s.Reach(() => s.P?.Choices.Any(c => c.Targets.SequenceEqual([0])) == true);
            s.Answer(c => c.Targets.SequenceEqual([0])); s.Play(1);
            var grant = s.E<KuanshiArmedEvent>().Single().Grant;
            s.Use("one"); s.Play(1);
            s.Need(s.V(0).Hp == hp - 1 && !s.E<KuanshiDamagePreventedEvent>().Any() &&
                !s.E<KuanshiExpiredEvent>().Any(e => e.GrantFrameId == grant.FrameId),
                "A real one-point damage applies and does not consume either version's heavy-damage grant.");
            s.Use("two"); s.Play(1);
            s.Need(s.V(0).Hp == hp - 1 && s.E<KuanshiDamagePreventedEvent>().Count() == 1,
                "The first actual finalized two-point damage is prevented by the native grant.");
            s.Cold();
            s.Use("two"); s.Play(1);
            var mobile = skill == "mobile:kuanshi";
            s.Need(s.V(0).Hp == hp - (mobile ? 3 : 1) &&
                s.E<KuanshiDamagePreventedEvent>().Count() == (mobile ? 1 : 2) &&
                s.E<KuanshiExpiredEvent>().Count(e => e.GrantFrameId == grant.FrameId &&
                    e.Reason == "mobile-first-heavy-damage") == (mobile ? 1 : 0),
                "Only the ordinary mobile version consumes its first protection; the existing OL version retains its repeated prevention.");
            s.End(1); s.Play(2); s.End(2); s.Play(3); s.End(3); s.Play(0);
            s.Need(s.E<KuanshiDrawSkippedEvent>().Count(e => e.OwnerSeat == 0) == 1,
                "Consuming protection and cold replay preserve exactly one native next-turn draw-skip debt.");
            s.Cold();
        }
    }

    private static string? Key(PromptChoice c) => c.Parameters.GetValueOrDefault("program-action");
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Need(bool condition, string message)
        { if (!condition) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P, G.State.TurnNumber })); }
        public void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected mobile Kan Ze command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision, P!.PromptId));
        public void Use(string action) => Accept(new UseProgramSkillCommand(1, Driver, action, [], [0], G.Revision, P!.PromptId));
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 150 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select)
                { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { } p) throw new InvalidOperationException("Unexpected mobile Kan Ze boundary: " + JsonSerializer.Serialize(p));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded native mobile Kan Ze boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var original = Dump();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(original == Dump(), "Cold replay preserves protection consumption, debt, all player views and exact original native events.");
        }
        public static Scene Start(string skill)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(skill));
            var game = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
                HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
            var s = new Scene(game, registry); s.Accept(new StartGameCommand()); return s;
        }
    }
    private static string Peer(int seat) => "fixture:mobile-kan-ze-peer-" + seat;
    private sealed class Fixture(string skill) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-kan-ze", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:kan-ze", "ol:kan-ze");
            var catalog = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,"activations":[
              {"id":"one","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
              {"id":"two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":2}]}
            ]}]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "真实伤害驱动", description = "原生一点与两点伤害。" } } }));
            b.AddSkill(new(Driver, "真实伤害驱动", "原生一点与两点伤害。") { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            b.AddGeneral(new(Owner, "实际宽释持有者", "supporter", skill, "wu", 20));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "实际同桌", "supporter", Driver, "wei", 20));
            b.AddDeck(new(Deck, "固定闪牌堆", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, "移动宽释次数", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1,
                [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
