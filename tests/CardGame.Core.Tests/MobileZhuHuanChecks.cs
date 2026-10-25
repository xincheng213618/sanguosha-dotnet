using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileZhuHuanChecks
{
    private const string Hero = "mobile:zhu-huan", Fenli = "boundary:fenli-mobile", Pingkou = "mobile:pingkou", Peer = "fixture:mobile-zhu-huan-peer", Deck = "fixture:mobile-zhu-huan-deck";
    public static void ActualSkipsSequentialNativeDamageAndDyingColdWithoutEquipment()
    {
        var s = Scene.Start();
        for (var phase = 0; phase < 3; phase++)
        { s.Reach(() => s.P?.SkillPrompt?.SkillId == Fenli); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); }
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Pingkou); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.Root?.MobileZhuHuanPingkou is { Sealed: false });
        var root = s.Root!.Id; var hand = s.V(0).HandCount;
        s.Need(s.Root.MobileZhuHuanPingkou!.Maximum == 3 && s.E<BoundaryZhuHuanPhaseSkippedEvent>().Where(e => e.OwnerSeat == 0)
                .Select(e => e.Phase).SequenceEqual([SkillProgramTurnPhase.Draw, SkillProgramTurnPhase.Play, SkillProgramTurnPhase.Discard]) &&
            s.E<TurnEndedEvent>().Any(e => e.ActorSeat == 0 && e.TurnNumber == s.G.State.TurnNumber),
            "Ordinary mobile Fenli retains three actual native skips, and Pingkou freezes their quota only after the true ended-turn boundary.");
        foreach (var target in new[] { 2, 1 }) s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "target" && c.Targets.SequenceEqual([target]));
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "finish"); s.Reach(() => s.P is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 });
        var peach = s.P!.Choices.First(c => c.Parameters.GetValueOrDefault("response") == "peach").Cards.Single();
        s.Need(s.V(1).Hp == 0 && s.V(2).Hp == 4 && s.Root!.MobileZhuHuanPingkou!.Targets.SequenceEqual([1, 2]) &&
            s.Root.AttackAttempt is { TargetSeat: 1 } && s.E<MobileZhuHuanPingkouDamageIssuedEvent>().Count() == 1,
            "Targets use native action order; the first target's actual damage and dying child block the second target rather than resolving both at once.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("response") == "peach" && c.Cards.SequenceEqual([peach]));
        s.Reach(() => s.E<MobileZhuHuanPingkouCompletedEvent>().Any(e => e.FrameId == root));
        s.Need(s.V(1).Hp == 1 && s.V(2).Hp == 3 && s.V(0).HandCount == hand - 1 &&
            s.E<MobileZhuHuanPingkouDamageIssuedEvent>().Where(e => e.FrameId == root).Select(e => e.Target).SequenceEqual([1, 2]) &&
            s.E<MobileZhuHuanPingkouDamageReturnedEvent>().Where(e => e.FrameId == root).Select(e => e.Target).SequenceEqual([1, 2]) &&
            s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat is 1 or 2 && e.Amount == 1) == 2 &&
            !s.E<BoundaryZhuHuanEquipmentObtainedEvent>().Any() && !s.G.CardMovements.Any(m => m.Reason.Value == "skill.boundary-zhu-huan.pingkou.equipment") &&
            s.G.ResolutionStack.All(f => f.Id != root),
            "Cold return completes both original native damage occurrences exactly once. Ordinary Pingkou gives no equipment even with an equipment-filled deck; the owner's only hand loss is the real rescue Peach.");
    }
    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.MobileZhuHuanPingkou is not null);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected mobile Zhu Huan command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 160 && !done(); i++) { Need(P is null, "Unexpected mobile Zhu Huan prompt."); Accept(new AdvanceOneStepCommand(G.Revision)); }
            Need(done(), "Missing bounded mobile Zhu Huan boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves original actual skips, sealed native targets and first damage's real dying child."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 3).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P, Root = Root?.MobileZhuHuanPingkou, V(0).Hp, Hand = V(0).HandCount })); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(46, 1, "mobile-zhu-huan-native", Hero, 4, 0, 0, 0, 0, [], [new(CardKind.Peach, Suit.Heart, 3)], [new(CardKind.Crossbow, Suit.Club, 1)],
                [new(Peer, Hp: 1) { StartingHand = Enumerable.Range(0, 5).Select(i => new ShanheStartingCard(CardKind.Crossbow, Suit.Club, i + 1)).ToArray() },
                 new(Peer, Hp: 4) { StartingHand = Enumerable.Range(0, 5).Select(i => new ShanheStartingCard(CardKind.Crossbow, Suit.Club, i + 1)).ToArray() }], [], new string('e', 64));
            var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, r));
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-zhu-huan", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "boundary:zhu-huan-mobile");
            b.AddGeneral(new(Peer, "真实同行", "supporter", "standard:none", "wei", 4));
            b.AddDeck(new(Deck, "固定真实装备库存", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 80).Select(i => new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, i % 13 + 1)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动朱桓原生流程", 3, 3, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
