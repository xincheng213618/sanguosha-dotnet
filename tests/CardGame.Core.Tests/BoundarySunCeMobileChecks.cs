using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundarySunCeMobileChecks
{
    private const string Hero = "boundary:sun-ce-mobile", Zhiba = "boundary:zhiba-mobile", Hunzi = "boundary:hunzi-mobile", Driver = "fixture:mobile-sun-ce-driver", Mode = "identity:mobile-sun-ce";
    public static void WuInitiatesLordTieClaimColdAndAwakenedRefusal()
    {
        var s = Scene.Start(); var max = s.V(0).MaxHp;
        s.Need(s.V(0).Hp == 5 && max == 5, "The fixed four-player identity Lord includes its native one-HP bonus before the real three-HP loss.");
        s.Need(s.G.GetHumanLegalActions().All(a => a.ProgramSkillId != Zhiba), "The Lord has no mobile Zhiba action against another Wu player.");
        s.Accept(new UseProgramSkillCommand(0, Driver, "lower", [], [], s.G.Revision, s.P!.PromptId)); s.Play(0);
        s.Need(s.V(0).Hp == 2 && !s.E<ProgramBindingResolvedEvent>().Any(e => e.SkillId == Hunzi), "Reaching two HP during Play waits until the actual next preparation to awaken.");
        s.End(0); s.Play(1); var offered = s.V(1).Hand.First().Id;
        s.Challenge(offered); s.Reach(() => s.P is { PlayerSeat: 0 } p && p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "pindian-card"));
        s.Need(!s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "boundary-sun-ce-challenge"), "An unawakened Lord cannot refuse the other Wu player's actual challenge.");
        var lordCard = s.P.Choices.First(c => c.Cards.Count == 1).Cards.Single(); s.Answer(c => c.Cards.SequenceEqual([lordCard]));
        s.Reach(() => s.P is { PlayerSeat: 0 } p && p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "pindian-claim"));
        var pair = s.G.ResolutionStack.OfType<PindianFrame>().Single(); var native = pair.Id;
        s.Need(pair is { SourceSeat: 1, OpponentSeat: 0, Result.SourceWon: false } && pair.Result!.SourceRank == pair.Result.OpponentRank &&
            new[] { offered, lordCard }.All(id => s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location == CardLocation.Processing),
            "The other Wu player remains the native source; a real tie is a non-win, and both original paid cards await the Lord's optional claim.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("take") == "true"); s.Play(1);
        s.Need(new[] { offered, lordCard }.All(id => s.V(0).Hand.Any(c => c.Id == id) && s.G.CardMovements.Count(m => m.CardId == id && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.PindianReveal) == 1) &&
            s.E<PindianResultDeterminedEvent>().Count(e => e.FrameId == native) == 1 && !s.G.ResolutionStack.Any(f => f.Id == native),
            "Cold returns both exact original tie cards once through the native Pindian continuation without revealing or paying again.");
        var repeat = s.G.Submit(new UseProgramSkillCommand(1, Zhiba, "accept-wu-challenge", [s.V(1).Hand.First().Id], [0], s.G.Revision, s.P!.PromptId) { SkillOwnerSeat = 0 });
        s.Need(!repeat.Accepted, "Each Wu provider has only one Zhiba challenge in that actual Play phase.");
        s.End(1); s.Play(2); s.End(2); s.Play(3); s.End(3); s.Play(0);
        s.Need(s.V(0).MaxHp == max - 1 && s.V(0).Skills!.Any(k => k.ContentId == "boundary:yingzi") && s.V(0).Skills!.Any(k => k.ContentId == "classic:yinghun") &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.OwnerSeat == 0 && e.SkillId == Hunzi && e.Completed) == 1,
            "Two HP awakens in the real next preparation, pays one maximum HP and grants complete mobile Yingzi/Yinghun without an OL ending reward.");
        s.End(0); s.Play(1); var retained = s.V(1).Hand.First().Id; var results = s.E<PindianResultDeterminedEvent>().Count();
        s.Challenge(retained); s.Reach(() => s.P is { PlayerSeat: 0 } p && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "boundary-sun-ce-challenge"));
        s.Need(!s.G.ResolutionStack.OfType<PindianFrame>().Any(), "An awakened refusal opportunity precedes native card payment.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("accept") == "False"); s.Play(1);
        s.Need(s.V(1).Hand.Any(c => c.Id == retained) && s.E<PindianResultDeterminedEvent>().Count() == results &&
            s.E<BoundarySunCeChallengeAnsweredEvent>().Count(e => !e.Accepted) == 1,
            "The next phase refreshes the provider's challenge; an actually awakened Lord may refuse without paying a new Pindian card.");
    }
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Mobile Sun Ce command rejected."); }
        public void Answer(Func<PromptChoice, bool> test) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(test).Id, G.Revision)); }
        public void Challenge(int card) => Accept(new UseProgramSkillCommand(1, Zhiba, "accept-wu-challenge", [card], [0], G.Revision, P!.PromptId) { SkillOwnerSeat = 0 });
        public void End(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision));
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void Reach(Func<bool> done)
        {
            for (var n = 0; n < 140 && !done(); n++)
            {
                Need(G.State.Winner == Winner.None, "Missing mobile Sun Ce boundary before terminal.");
                if (P is { } p && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); continue; }
                Need(P is null, "Unexpected mobile Sun Ce prompt."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "Missing bounded mobile Sun Ce native boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Sun Ce cold preserves four private views and the exact original native contest/answer."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, G.State.TurnNumber, Views = Enumerable.Range(0, 4).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }).ToArray() })); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var g = GameEngine.CreateStandard(new() { Seed = 47, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
                UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, r);
            var s = new Scene(g, r); s.Accept(new StartGameCommand());
            for (var n = 0; n < 100; n++)
            {
                if (s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return s;
                if (s.P is { Kind: DecisionKind.SelectGeneral } p) { s.Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? "fixture:mobile-sun-ce-owner" : "fixture:mobile-sun-ce-peer-" + p.PlayerSeat, g.Revision, p.PromptId)); continue; }
                s.Need(s.P is null, "Unexpected mobile Sun Ce opening boundary."); s.Accept(new AdvanceOneStepCommand(g.Revision));
            }
            throw new InvalidOperationException("Sun Ce fixed identity opening missing.");
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-sun-ce", new(1, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "boundary:zhou-yu", "classic:sun-jian");
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new[] { new { id = Driver, revision = 1,
                activations = new[] { new { id = "lower", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "loseHp", target = "owner", amount = 3 } } } } } } }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> { [Driver] = new { name = "真实觉醒准备", description = "含主公加成的五体力角色在实际出牌阶段失去三点体力。" } } }));
            b.AddSkill(new(Driver, "真实觉醒准备", "含主公加成的五体力角色在实际出牌阶段失去三点体力。") { Program = c.Programs[Driver], ProgramPresentation = c.Presentations[Driver] });
            // This fixed identity role uses the production skills plus one native HP-loss driver.
            var ids = new List<string> { "fixture:mobile-sun-ce-owner" };
            b.AddGeneral(new(ids[0], "移动孙策原生主公", "supporter", "boundary:jiang-mobile", "wu", 4, [Hunzi, Zhiba, Driver]) { InitialHp = 4 });
            for (var i = 1; i < 4; i++) { var id = "fixture:mobile-sun-ce-peer-" + i; ids.Add(id); b.AddGeneral(new(id, "吴方挑战者", "supporter", "standard:none", "wu", 4)); }
            b.AddDeck(new("fixture:mobile-sun-ce-deck", "固定同点数", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            b.AddMode(new(Mode, "移动制霸原生身份", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, "fixture:mobile-sun-ce-deck", GeneralCandidateCount: 4, GeneralPoolIds: ids));
        }
    }
}
