using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

namespace CardGame.Core.Tests;

internal static class MobileSunCeChecks
{
    private const string Hero = "mobile:sun-ce", Jiang = "mobile:jiang", Hunzi = "classic:hunzi", Zhiba = "mobile:zhiba",
        Driver = "fixture:ordinary-mobile-sun-ce-driver", Watch = "fixture:ordinary-mobile-sun-ce-drawn", Late = "fixture:ordinary-mobile-sun-ce-late",
        Owner = "fixture:ordinary-mobile-sun-ce-owner", Mode = "fixture:ordinary-mobile-sun-ce-mode", DrawReason = "skill-program.mobile:jiang.Draw";

    public static void RedThunderActorAndTargetDrawAtDesignationAndPaidCold()
    {
        foreach (var isTarget in new[] { false, true })
        {
            var s = Scene.Start(jiang: true);
            if (isTarget) { s.End(0); s.Play(1); }
            var actor = isTarget ? 1 : 0; var target = isTarget ? 0 : 1;
            var card = s.V(actor).Hand.First(c => c.Kind == CardKind.ThunderSlash).Id;
            var before = s.V(0).Hand.Count;
            s.Native(actor, card, target); s.Reach(() => s.P?.SkillPrompt?.SkillId == Jiang);
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
            var original = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Jiang);
            var use = original.WindowContext!.CardUse!; var frameId = original.Id;
            s.Need(original.WindowContext.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && use.ActorSeat == actor &&
                use.EffectiveKind == CardKind.ThunderSlash && use.IsPublicRed == true &&
                (isTarget ? original.TriggerId == "draw-on-targeted-by-red-slash" : original.TriggerId == "draw-on-using-red-slash") &&
                s.V(0).Hand.Count == before + (isTarget ? 1 : 0) && s.Zone(card) == CardLocation.Processing &&
                s.G.CardMovements.Count(m => m.Reason.Value == DrawReason && m.To == CardLocation.Hand(0)) == 1 &&
                s.G.Events.All(e => e.Payload is not DamageAppliedEvent),
                "A real red Thunder Slash uses the complete Slash family and draws for its actual actor/target at the original designation boundary, before target effects or damage.");
            if (!isTarget) s.Cold();
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Reach(() => s.P?.SkillPrompt?.SkillId == Late);
            var late = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Late);
            s.Need(late.WindowContext is { Window: SkillProgramTriggerWindow.CardUseBeforeTargetEffects, CardUse: { } laterUse } &&
                laterUse.CardActionId == use.CardActionId && laterUse.ParentCardUseFrameId == use.ParentCardUseFrameId &&
                s.G.ResolutionStack.All(f => f.Id != frameId) && s.Facts<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Jiang && e.Activated && e.Completed) == 1 &&
                s.G.CardMovements.Count(m => m.Reason.Value == DrawReason && m.To == CardLocation.Hand(0)) == 1,
                "The paid Jiang child returns once to that original use; the later before-target-effects observer sees its completed draw without repeating it.");
        }
    }

    public static void OrdinaryHunziControlsNativeChallengeRefusalAndCold()
    {
        ValidateAwakeningAdmission();
        var s = Scene.Start(jiang: false); var maximum = s.V(0).MaxHp;
        s.Need(s.V(0).Hp == 5, "The real identity Lord bonus produces five HP before paying the four-HP setup cost.");
        s.Submit(new UseProgramSkillCommand(0, Driver, "lower", [], [], s.G.Revision, s.P!.PromptId)); s.Play(0);
        s.Need(s.V(0).Hp == 1 && !s.Facts<ProgramBindingResolvedEvent>().Any(e => e.SkillId == Hunzi), "One HP reached during Play does not awaken before the actual next preparation.");
        for (var seat = 0; seat < 4; seat++) { s.End(seat); s.Play((seat + 1) % 4); }
        s.Need(s.V(0).MaxHp == maximum - 1 && s.V(0).Skills!.Any(k => k.ContentId == "classic:yingzi") &&
            s.V(0).Skills!.Any(k => k.ContentId == "classic:yinghun") && s.Facts<ProgramBindingResolvedEvent>().Count(e => e.OwnerSeat == 0 && e.SkillId == Hunzi && e.Completed) == 1 &&
            s.Facts<ProgramBindingResolvedEvent>().All(e => e.SkillId != "boundary:hunzi-mobile"),
            "Only the real ordinary one-HP Hunzi awakens, pays maximum HP once and grants ordinary Yingzi/Yinghun, without borrowing the boundary awakening.");
        s.End(0); s.Play(1); var offered = s.V(1).Hand.First().Id; s.Challenge(1, offered);
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "boundary-sun-ce-challenge") == true);
        s.Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Zhiba).BoundarySunCeChallenge is { Awakened: true, ProviderSeat: 1 },
            "The capability-indexed mobile Zhiba source reads the completed classic Hunzi as its explicit awakening witness.");
        s.Answer(c => c.Parameters.GetValueOrDefault("accept") == "True");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "pindian-card") == true);
        var lordCard = s.P!.Choices.First(c => c.Cards.Count == 1).Cards.Single(); s.Answer(c => c.Cards.SequenceEqual([lordCard]));
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "pindian-claim") == true);
        var pair = s.G.ResolutionStack.OfType<PindianFrame>().Single(); var pairId = pair.Id;
        s.Need(pair.SkillId == Zhiba && pair is { SourceSeat: 1, OpponentSeat: 0, Result.SourceWon: false } &&
            new[] { offered, lordCard }.All(id => s.Zone(id) == CardLocation.Processing),
            "Accepting the declared mobile source enters the original native contest and retains both real paid tie entities for its typed claim return.");
        s.Answer(c => c.Parameters.GetValueOrDefault("take") == "true"); s.Play(1);
        s.Need(new[] { offered, lordCard }.All(id => s.Zone(id) == CardLocation.Hand(0)) &&
            s.Facts<PindianResultDeterminedEvent>().Count(e => e.FrameId == pairId) == 1 && s.G.ResolutionStack.All(f => f.Id != pairId),
            "The generic source's native Pindian returns the two exact cards once, with no alternate contest or duplicate payment.");
        s.End(1); s.Play(2); var retained = s.V(2).Hand.First().Id; s.Challenge(2, retained);
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "boundary-sun-ce-challenge") == true);
        s.Need(!s.G.ResolutionStack.OfType<PindianFrame>().Any(), "Ordinary awakened refusal is offered before paying a new native contest card.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("accept") == "False"); s.Play(2);
        s.Need(s.Zone(retained) == CardLocation.Hand(2) && s.Facts<PindianResultDeterminedEvent>().Length == 1 &&
            s.Facts<BoundarySunCeChallengeStartedEvent>().Count(e => e.Source.SkillId == Zhiba && e.Awakened) == 2 &&
            s.Facts<BoundarySunCeChallengeAnsweredEvent>().Count(e => !e.Accepted) == 1,
            "Cold retains the already-issued classic-awakening witness; refusing leaves the other Wu provider's real hand card unpaid and creates no second contest.");
    }

    private static void ValidateAwakeningAdmission()
    {
        Reject(["classic:hunzi", "boundary:hunzi-mobile"], "Zhiba accepts at most one awakening skill");
        Reject(["fixture:missing-awakening"], "references an unavailable awakening skill");
        Reject(["standard:none"], "references an unavailable awakening skill");
        static void Reject(string[] skills, string expected)
        {
            try { ContentRegistry.Build(new StandardContentPackage(), new InvalidAwakening(skills)); }
            catch (InvalidOperationException e) when (e.Message.Contains(expected, StringComparison.Ordinal)) { return; }
            throw new InvalidOperationException("The new optional Zhiba awakening admission did not reject its precise invalid binding: " + expected);
        }
    }
    private sealed class InvalidAwakening(string[] skills) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-sun-ce-invalid-awakening", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            const string id = "fixture:mobile-sun-ce-awakening-admission";
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new[] { new { id, revision = 1,
                activations = new[] { new { id = "challenge", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLivingWithHand", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "resolveBoundarySunCeZhiba", target = "owner", skillIds = skills } } } } } } });
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> { [id] = new { name = "觉醒来源准入", description = "只验证新增可选引用合同。" } } }));
            b.AddSkill(new(id, "觉醒来源准入", "只验证新增可选引用合同。") { Program = c.Programs[id], ProgramPresentation = c.Presentations[id], Tags = SkillTag.Lord });
        }
    }
    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Submit(GameCommand command) { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(r.Accepted, r.Error?.Message ?? "Mobile Sun Ce command rejected."); }
        public void Answer(Func<PromptChoice, bool> test) { var p = P!; Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(test).Id, G.Revision)); }
        public void Challenge(int seat, int card) => Submit(new UseProgramSkillCommand(seat, Zhiba, "accept-wu-challenge", [card], [0], G.Revision, P!.PromptId) { SkillOwnerSeat = 0 });
        public void End(int seat) => Submit(new EndPlayPhaseCommand(seat, G.Revision));
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void Native(int actor, int card, int target)
        {
            var a = G.GetLegalActions(actor).Single(a => a.CardId == card && a.TargetSeats.SequenceEqual([target]));
            Submit(new PlayCardCommand(actor, card, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)
                { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 140; i++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The named mobile Sun Ce boundary must precede a native terminal.");
                if (P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip") == true) { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); continue; }
                Need(P is null, "Unexpected native Sun Ce boundary."); Submit(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The bounded real mobile Sun Ce flow missed its named boundary.");
        }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements, Commands = CommandJson.Serialize(G.AcceptedCommands) });
        public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(State() == before, "Cold retains four viewers and the same issued source, paid native draw or original challenge witness."); }
        public void Need(bool ok, string why)
        {
            if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase,
                Pending = P?.Kind, Skill = P?.SkillPrompt?.SkillId, Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Movements = G.CardMovements.TakeLast(8).ToArray() }));
        }
        public static Scene Start(bool jiang)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(jiang));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
                UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, registry);
            var s = new Scene(registry, game); s.Submit(new StartGameCommand());
            for (var i = 0; i < 100; i++)
            {
                if (s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return s;
                if (s.P is { Kind: DecisionKind.SelectGeneral } p) { s.Submit(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : Owner + "-peer-" + p.PlayerSeat, game.Revision, p.PromptId)); continue; }
                s.Need(s.P is null, "Unexpected mobile Sun Ce fixed opening."); s.Submit(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("The fixed native mobile Sun Ce opening did not complete.");
        }
    }
    private sealed class Fixture(bool jiang) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ordinary-mobile-sun-ce", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:sun-ce", "classic:zhou-yu", "classic:sun-jian");
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[]
            {
                new { id = Driver, revision = 1, activations = new[] { new { id = "lower", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "loseHp", target = "owner", amount = 4 } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "drawn", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch",
                    movementReasons = new[] { DrawReason }, optional = false, isSkillInvocation = false, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "hold", options = new[] { new { id = "continue" } } } } } } },
                new { id = Late, revision = 1, triggers = new[] { new { id = "late", window = "cardUseBeforeTargetEffects", ownerRelation = "observer", cardKinds = new[] { "thunderSlash" },
                    optional = false, isSkillInvocation = false, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "late", options = new[] { new { id = "continue" } } } } } } }
            } });
            var names = new Dictionary<string, object> { [Driver] = new { name = "真实一体力准备", description = "含主公加成的五体力角色原生失去四点体力。" } };
            foreach (var id in new[] { Watch, Late }) names[id] = new { name = id, description = "原生领取或更晚目标效果边界。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            var ids = new List<string> { Owner };
            b.AddGeneral(new(Owner, "普通移动孙策原生主公", "supporter", Jiang, "wu", 4, [Hunzi, Zhiba, Driver, Watch, Late]) { InitialHp = 4 });
            for (var i = 1; i < 4; i++) { var id = Owner + "-peer-" + i; ids.Add(id); b.AddGeneral(new(id, "吴方实际挑战者", "supporter", "standard:none", "wu", 4)); }
            const string deck = "fixture:ordinary-mobile-sun-ce-deck";
            b.AddDeck(new(deck, "固定真实红雷杀或同点拼点实体", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard(jiang ? "standard:thunder_slash" : "standard:dodge", jiang ? Suit.Heart : Suit.Club, 7)).ToArray() });
            b.AddMode(new(Mode, "普通移动孙策真实身份", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: ids));
        }
    }
}
