using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileMengHuoChecks
{
    private const string Skill = "mobile:zaiqi", Owner = "fixture:ordinary-mobile-meng-huo",
        Wound = "fixture:ordinary-mobile-meng-huo-wound", Watch = "fixture:ordinary-mobile-meng-huo-recovery",
        Deck = "fixture:ordinary-mobile-meng-huo-deck", Mode = "identity:classic-ordinary-mobile-meng-huo",
        MoveReason = "skill-program.mobile:zaiqi.MoveBoundCards";

    public static void MixedRevealRecoversBeforeGainingCardsAndReturnsCold()
    {
        var s = Scene.Start(); var hp = s.V.Hp;
        s.Need(s.V.MaxHp - hp == 2 && s.V.HandCount == 0 && s.NormalDraws == 0,
            "A real preparation HP payment establishes exactly two lost HP before the optional native normal-draw replacement.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var reveal = s.E<ProgramCardsRevealedEvent>().Single(e => e.SkillId == Skill);
        var heart = reveal.Cards.Single(c => c.Suit == Suit.Heart).Id;
        var other = reveal.Cards.Single(c => c.Suit == Suit.Club).Id;
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        var child = s.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single();
        s.Need(reveal.Cards.Count == 2 && heart != other && root.Id == reveal.FrameId && root.InstructionIndex == 3 &&
            root.WindowContext?.Window == SkillProgramTriggerWindow.DrawPhaseStarting &&
            child.Change is { Kind: HpChangeKind.Recovery, SourceSeat: 0, TargetSeat: 0, Amount: 1 } && child.Change.ParentFrameId == root.Id &&
            s.V.Hp == hp + 1 && s.V.HandCount == 0 && s.Zone(heart) == CardLocation.Processing && s.Zone(other) == CardLocation.Processing &&
            s.E<ProgramRecoveryAppliedEvent>().Single().ProgramFrameId == root.Id &&
            s.G.CardMovements.Count(m => reveal.Cards.Any(c => c.Id == m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing) == 2 &&
            !s.G.CardMovements.Any(m => m.Reason.Value == MoveReason) && s.NormalDraws == 0,
            "The exact lost-HP two-card public reveal owns the real one-point recovery child while both original entities remain in Processing; no non-heart is obtained first and no normal Draw is paid.");
        var rootId = root.Id; s.Cold();
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.V.Hp == hp + 1 && s.V.Hand is [{ Id: var held }] && held == other && s.Zone(heart) == CardLocation.DiscardPile &&
            s.Zone(other) == CardLocation.Hand(0) && s.NormalDraws == 0 &&
            s.E<ProgramCardsRevealedEvent>().Count(e => e.SkillId == Skill) == 1 &&
            s.E<RecoveryAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 0 && e.Amount == 1) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == heart && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason.Value == MoveReason) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == other && m.From == CardLocation.Processing && m.To == CardLocation.Hand(0) && m.Reason.Value == MoveReason) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == rootId && e.Activated && e.Completed) == 1,
            "Cold returns to the same recovery-paid replacement, discards its heart and obtains its original non-heart exactly once without repeating reveal, healing or the replaced two-card draw.");
    }

    private static string Peer(int seat) => "fixture:ordinary-mobile-meng-huo-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V => G.CreateSnapshot(0).Players.Single(p => p.Seat == 0);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int NormalDraws => G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason == CardMoveReasons.Draw);
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "Rejected real mobile Meng Huo command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var c = p.Choices.FirstOrDefault(predicate); Need(c is not null, "Missing actual mobile Meng Huo choice."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 140; step++)
            {
                if (ready()) return;
                if (P is { Kind: DecisionKind.SelectGeneral } choose)
                { Accept(new SelectGeneralCommand(choose.PlayerSeat, choose.PlayerSeat == 0 ? Owner : Peer(choose.PlayerSeat), G.Revision, choose.PromptId)); continue; }
                Need(P is null, "Unexpected native Meng Huo boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded native Meng Huo scene did not reach its next boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the exact two revealed entities, paid recovery, all views and original replacement/recovery child frames.");
        }
        public void Need(bool ok, string why)
        { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.Phase, V.Hp, V.MaxHp, V.HandCount,
            Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null })); }
        public static Scene Start()
        {
            // One fixed opening records its actual top IDs before any reveal;
            // the real second recipe assigns their suits without a seed search.
            var stock = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray();
            var probe = Boot(ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock)));
            var first = probe.G.CreateCardZoneDiagnostics().Where(c => c.Location == CardLocation.DrawPile).OrderByDescending(c => c.ZoneIndex).First().CardId;
            stock[first - 1] = new("standard:dodge", Suit.Heart, 2);
            return Boot(ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock)));
        }
        private static Scene Boot(ContentRegistry r)
        {
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4 }, r), r);
            s.Accept(new StartGameCommand()); s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill); return s;
        }
    }
    private sealed class Fixture(IReadOnlyList<ContentDeckPhysicalCard> stock) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ordinary-mobile-meng-huo", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:meng-huo", "classic:meng-huo");
            var programs = new object[] {
                new { id = Wound, revision = 1, triggers = new[] { new { id = "once-real-preparation-hp-payment", window = "turnStartBeforeNormalFlow", subject = "owner", optional = false,
                    isSkillInvocation = false, usageScope = "game", usageLimit = 1, effects = new[] { new { op = "loseHp", target = "owner", amount = 2 } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "actual-recovery-before-entity-gain", window = "afterHpRecovered", subject = "owner", hpChangeOccurrence = "perEvent",
                    optional = false, isSkillInvocation = false, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "recovery-return", options = new[] { new { id = "continue" } } } } } } }
            };
            var labels = new Dictionary<string, object> { [Wound] = new { name = "真实准备失去体力", description = "一次真实失去2体力，不改存储状态。" },
                [Watch] = new { name = "实际回复后暂停", description = "原生回复已付，非红桃尚未进入手牌。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续原再起" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = programs }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "真实普通移动孟获", "supporter", Skill, "shu", 4, [Wound, Watch]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "真实其他参与者", "supporter", "standard:none", "wei", 8));
            b.AddDeck(new(Deck, "真实混合花色再起", 0, 2, []) { PhysicalCards = stock });
            b.AddMode(new(Mode, "普通移动孟获回复收牌顺序", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
