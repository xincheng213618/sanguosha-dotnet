using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileLiYanChecks
{
    private const string Owner = "fixture:mobile-li-yan-owner", Driver = "fixture:mobile-li-yan-driver", Clock = "fixture:mobile-li-yan-clock", Pause = "fixture:mobile-li-yan-draw", Mode = "identity:mobile-li-yan";
    public static void StackedBonusWaitsThroughSkippedDrawAndSurvivesSkillLossCold()
    {
        var s = Scene.Start(skip: true); s.PrepareTwoGrantsAndLoseSkill(); s.Cold(); s.End(); s.Play(1);
        s.Need(s.E<MobileLiYanNextDrawBonusIssuedEvent>().Length == 2 && s.E<MobileLiYanNextDrawBonusAssignedEvent>().Length == 0 && s.CurrentTurnDraws().Length == 0,
            "The first target turn really skips Draw without consuming either already-issued future-phase bonus.");
        s.Cold(); s.End(); s.Play(2); s.End(); s.Play(3); s.End(); s.Play(0); s.End();
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Pause); s.Cold(); s.Continue(); s.Play(1);
        var issued = s.E<MobileLiYanNextDrawBonusAssignedEvent>();
        s.Need(issued.Length == 2 && issued.Select(e => e.Phase.Token).Distinct().Count() == 1 && s.CurrentTurnDraws().SequenceEqual([4]) &&
            !s.G.CreateSnapshot(0).Players[0].Skills!.Any(skill => skill.Id == "mobile:duliang"),
            "Both stacked grants survive loss of their original issuer skill and pay exactly two extra cards at the next real Draw phase.");
    }
    public static void InsertedDrawConsumesStackBeforeNaturalDrawCold()
    {
        var s = Scene.Start(skip: false); s.PrepareTwoGrantsAndLoseSkill(); s.End();
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Pause); s.Cold(); s.Continue(); s.Play(1);
        var phases = s.CurrentTurnEvents().OfType<ActualDiscardRecoveryPhaseStartedEvent>().Where(e => e.Phase.ActorSeat == 1 && e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Draw).Select(e => e.Phase).ToArray();
        var assignments = s.E<MobileLiYanNextDrawBonusAssignedEvent>();
        s.Need(phases.Length == 2 && assignments.Length == 2 && assignments.All(e => e.Phase == phases[0]) && s.CurrentTurnDraws().SequenceEqual([4, 2]),
            "The earlier inserted actual Draw consumes both bonuses and draws four native cards; the later natural Draw in that same turn draws only its ordinary two."); s.Cold();
    }
    public static void StartingWindowPaidSkipConsumesEnteredPhaseBonusCold()
    {
        var s = Scene.Start(skip: false, skipAfterStarting: true); s.PrepareTwoGrantsAndLoseSkill(); s.End();
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "alternative-phase-cost-card") == true);
        var phase = s.CurrentTurnEvents().OfType<ActualDiscardRecoveryPhaseStartedEvent>().Single(e => e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Draw && e.Phase.ActorSeat == 1).Phase;
        s.Need(s.G.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(f => f.OwnerSeat == 1 && f.Window == SkillProgramTriggerWindow.DrawPhaseStarting) &&
            s.E<MobileLiYanNextDrawBonusAssignedEvent>().Length == 2 && s.E<MobileLiYanNextDrawBonusAssignedEvent>().All(e => e.Phase == phase),
            "The actual Draw phase has begun and owns both bonuses before its starting-window substitute pays a real owned card.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "alternative-phase-cost-card"); s.Play(1);
        s.Need(s.CurrentTurnDraws().Length == 0 && s.E<ProgramAlternativePhaseCostPaidEvent>().Count(e => e.OwnerSeat == 1 && e.SkillId == Clock) == 1 &&
            s.E<ProgramTurnPhaseSubstitutedEvent>().Count(e => e.OwnerSeat == 1 && e.SkillId == Clock && e.Window == SkillProgramTriggerWindow.DrawPhaseStarting && e.SkippedPhase == SkillProgramTurnPhase.Draw) == 1,
            "A paid skip inside the already-entered Draw starting window suppresses native drawing while retaining the exact consumed-phase assignment.");
        s.Cold(); s.End(); s.Play(2); s.End(); s.Play(3); s.End(); s.Play(0); s.End();
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Pause); s.Cold(); s.Continue(); s.Play(1);
        s.Need(s.CurrentTurnDraws().SequenceEqual([2]) && s.E<MobileLiYanNextDrawBonusAssignedEvent>().Length == 2 &&
            s.E<MobileLiYanNextDrawBonusAssignedEvent>().All(e => e.Phase == phase),
            "Having reached its next Draw phase, a later starting-window skip does not roll the already-due bonuses forward to another actual Draw.");
    }
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public IGameEvent[] CurrentTurnEvents()
        {
            var all = G.Events.Select(e => e.Payload).ToArray();
            var start = Array.FindLastIndex(all, e => e is TurnStartedEvent t && t.TurnNumber == G.State.TurnNumber && t.ActorSeat == G.State.CurrentSeat);
            return all.Skip(start + 1).ToArray();
        }
        public int[] CurrentTurnDraws() => CurrentTurnEvents().OfType<CardsDrawnEvent>().Where(e => e.PlayerSeat == 1).Select(e => e.Count).ToArray();
        public void PrepareTwoGrantsAndLoseSkill()
        {
            Play(0); Grant(); Play(0); End(); Play(0); Grant(); Play(0);
            Accept(new UseProgramSkillCommand(0, Driver, "lose-duliang", [], [], G.Revision, P!.PromptId)); Play(0);
            Need(E<MobileLiYanNextDrawBonusIssuedEvent>().Length == 2 && E<MobileLiYanNextDrawBonusAssignedEvent>().Length == 0 &&
                !G.CreateSnapshot(0).Players[0].Skills!.Any(skill => skill.Id == "mobile:duliang"),
                "Two actual Play phases each issue one independent bonus after a real foreign-hand transfer, before any recipient Draw begins.");
        }
        public void Grant()
        {
            Accept(new UseProgramSkillCommand(0, "mobile:duliang", "take-hand-and-benefit", [], [1], G.Revision, P!.PromptId));
            Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card" && c.Parameters.GetValueOrDefault("skip") != "true");
            Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "next-draw") == true);
            Answer(c => c.Parameters.GetValueOrDefault("option-id") == "next-draw");
        }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected mobile Li Yan command."); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End() { var p = P!; Accept(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId)); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 260 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select) { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard }) { End(); continue; }
                if (P is { Kind: DecisionKind.DiscardCards } discard) { Accept(new DiscardCardsCommand(discard.PlayerSeat, discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray(), discard.PromptId, G.Revision)); continue; }
                if (P is { } p)
                {
                    var choice = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ?? p.Choices.FirstOrDefault(c => c.Cards.Count == 0 && c.Targets.Count == 0) ?? p.Choices.First();
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded mobile Li Yan phase boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(copy.ResolutionStack) && G.CardMovements.SequenceEqual(copy.CardMovements),
                "Cold replay preserves issued grants, actual phase assignments, private views and native frozen Draw frames."); G = copy;
        }
        public void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { P, Turn = G.State.TurnNumber, Current = G.State.CurrentSeat, Draws = CurrentTurnDraws() })); }
        public static Scene Start(bool skip, bool skipAfterStarting = false)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(skip, skipAfterStarting));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 10 }, r), r);
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private static string Peer(int seat) => "fixture:mobile-li-yan-peer-" + seat;
    private sealed class Fixture(bool skip, bool skipAfterStarting) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-li-yan", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:li-yan", "classic:li-yan");
            var clockEffect = skip ? """{"op":"skipTurnPhases","target":"owner","phases":["draw"]}""" : """{"op":"insertPhase","target":"owner","phase":"draw","phaseContinuation":"beforeNormalPreparation"}""";
            var clockTrigger = skipAfterStarting
                ? """{"id":"first-real-turn","window":"drawPhaseStarting","subject":"owner","turnOwnerScope":"own","optional":false,"priority":200,"usageScope":"game","usageLimit":1,"effects":[{"op":"payOwnedCardOrMarker","target":"owner","zones":["hand"],"marker":"bian"},{"op":"skipTurnPhases","target":"owner","phases":["draw"]}]}"""
                : $$"""{"id":"first-real-turn","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{{clockEffect}}]}""";
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
             {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"retain-materials","query":"handLimit","operation":"add","value":20,"priority":0}],
              "triggers":[{"id":"one-extra-play","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"insertPhase","target":"owner","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}],
              "activations":[{"id":"lose-duliang","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["mobile:duliang"],"sourceBind":"standard:none"}]}]},
             {"id":"{{Clock}}","revision":1,"triggers":[{{clockTrigger}}]},
             {"id":"{{Pause}}","revision":1,"triggers":[{"id":"frozen-next-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"next-draw-pause","options":[{"id":"continue"}]}]}]}
            ]}
            """;
            var labels = new Dictionary<string, object> { [Driver] = new { name = "两次真实出牌阶段", description = "原生阶段插入及真实失技" }, [Clock] = new { name = "首个真实回合", description = "跳过或插入真实摸牌阶段" }, [Pause] = new { name = "冻结正常摸牌数", description = "原生摸牌阶段暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, id, id) { Program = p });
            b.AddGeneral(new(Owner, "移动李严实际阶段", "supporter", "mobile:duliang", "shu", 8, ["classic:fulin", Driver]));
            b.AddGeneral(new(Peer(1), "未来真实摸牌受益者", "supporter", Clock, "wei", 8, [Pause]));
            for (var i = 2; i < 4; i++) b.AddGeneral(new(Peer(i), "固定同桌", "supporter", "standard:none", "wei", 8));
            const string deck = "fixture:mobile-li-yan-deck";
            b.AddDeck(new(deck, "同质原生实体闪", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray() });
            b.AddMode(new(Mode, "移动李严下一真实摸牌阶段", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
