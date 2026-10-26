using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileWuXianChecks
{
    private const string Mobile = "mobile:fumian", Classic = "classic:fumian", Owner = "fixture:mobile-wu-xian-owner",
        Driver = "fixture:mobile-wu-xian-driver", Clock = "fixture:mobile-wu-xian-clock", Pause = "fixture:mobile-wu-xian-first-draw",
        Mode = "identity:classic-mobile-wu-xian", Deck = "fixture:mobile-wu-xian-deck";

    public static void PreviousOwnTurnGapDoesNotReuseClassicLastChoice()
    {
        foreach (var mobile in new[] { false, true })
        {
            var s = Scene.Start(mobile); s.Prepare(); s.Choose("draw"); s.Play();
            s.Need(s.Facts<AlternatingChoiceBenefitResolvedEvent>().Single().Amount == 1, "The first real preparation issues the ordinary one-card benefit.");
            s.End(); s.Prepare(); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); s.Play();
            s.End(); s.Prepare(); s.Choose("targets"); s.Play();
            var issued = s.Facts<AlternatingChoiceBenefitResolvedEvent>().Last();
            s.Need(issued.Amount == (mobile ? 1 : 2) && issued.PendingOptionId == (mobile ? "targets" : null) &&
                (mobile ? issued.OwnerTurnOrdinal == 3 : issued.OwnerTurnOrdinal is null),
                "Skipping an intervening own preparation breaks mobile's previous-turn comparison, while classic retains its last-choice and reset contract with omitted ordinal.");
            var targets = s.Actions().Where(a => a.Kind == LegalActionKind.DrawTwo && a.ProgramActivationId == "red-additional-targets").ToArray();
            s.Need(targets.Any() && targets.Max(a => a.TargetSeats.Count) == (mobile ? 2 : 3),
                "The resolved real choice controls the actual additional-target quota, rather than only changing its displayed amount.");
        }
    }

    public static void IssuedNextDrawSurvivesSourceLossAndOnlyFirstExtraPhaseCold()
    {
        var s = Scene.Start(mobile: true, mode: "extra-loss"); s.Prepare(); s.Choose("draw");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == Mobile) == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == Mobile);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Pause);
        var issued = s.Facts<AlternatingChoiceDrawBenefitIssuedEvent>().Single();
        var assignment = s.Facts<AlternatingChoiceDrawPhaseAssignedEvent>().Single();
        s.Need(issued.Source.SkillId == Mobile && issued.Source.OwnerSeat == 0 && assignment.ProgramFrameId == issued.ProgramFrameId &&
            assignment.InstructionIndex == issued.InstructionIndex && assignment.Phase.ActorSeat == 0 &&
            assignment.Phase.TurnNumber == issued.TurnNumber && !s.V(0).Skills!.Any(k => k.ContentId == Mobile) && s.DrawCounts().Length == 0,
            "The original hashed source receipt binds the first actual inserted Draw before any cards are drawn, despite losing Fumian before entry.");
        s.Cold(); s.Continue(); s.Play();
        var phases = s.Facts<ActualDiscardRecoveryPhaseStartedEvent>().Where(e => e.Phase.ActorSeat == 0 &&
            e.Phase.TurnNumber == issued.TurnNumber && e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Draw).Select(e => e.Phase).ToArray();
        s.Need(phases.Length == 2 && assignment.Phase == phases[0] && s.DrawCounts().SequenceEqual([3, 2]) &&
            s.Facts<AlternatingChoiceDrawPhaseAssignedEvent>().Length == 1,
            "Cold returns to the same phase receipt: only the first actual Draw receives the extra card, and the later normal Draw remains two.");
    }

    public static void ReplacedEntryConsumesAndSkippedEntryDoesNotCarryPastTurn()
    {
        foreach (var mode in new[] { "replace", "skip" })
        {
            var s = Scene.Start(mobile: true, mode); s.Prepare(); s.Choose("draw");
            if (mode == "replace") { s.Reach(() => s.P?.SkillPrompt?.SkillId == Pause); s.Continue(); }
            s.Play();
            if (mode == "replace")
            {
                s.Need(s.DrawCounts().SequenceEqual([1, 2]) && s.Facts<AlternatingChoiceDrawPhaseAssignedEvent>().Length == 1,
                    "Replacing the entered first Draw consumes its assigned bonus without augmenting the replacement effect or a subsequent normal Draw.");
            }
            else
            {
                s.Need(s.DrawCounts().Length == 0 && s.Facts<AlternatingChoiceDrawPhaseAssignedEvent>().Length == 0,
                    "A Draw skipped before entry never fabricates a phase assignment.");
                s.End(); s.Prepare(); s.Choose("targets"); s.Reach(() => s.P?.SkillPrompt?.SkillId == Pause); s.Continue(); s.Play();
                s.Need(s.DrawCounts().SequenceEqual([2]) && s.Facts<AlternatingChoiceDrawPhaseAssignedEvent>().Length == 0,
                    "The skipped turn's unused next-Draw benefit expires with that actual turn and cannot pay a later turn's ordinary Draw.");
            }
        }
    }

    public static void IssuedRedTargetBenefitSurvivesLossAndActualUseCold()
    {
        foreach (var mobile in new[] { false, true })
        {
            var s = Scene.Start(mobile); s.Prepare(); s.Choose("targets"); s.Play();
            var issued = s.Facts<RedAdditionalTargetsGrantedEvent>().Single(); s.LoseSource(); s.Play();
            s.Need(issued.PersistAfterSkillLoss == (mobile ? true : null) && !s.V(0).Skills!.Any(k => k.ContentId == (mobile ? Mobile : Classic)),
                "Only mobile explicitly signs a persistent issued target benefit; classic keeps its nullable default and original source-loss behavior.");
            if (!mobile)
            {
                s.Need(!s.Actions().Any(a => a.ProgramActivationId == "red-additional-targets"), "Classic's default target availability still requires its source skill.");
                continue;
            }
            var action = s.Actions().First(a => a.Kind == LegalActionKind.DrawTwo && a.ProgramActivationId == "red-additional-targets" && a.TargetSeats.SequenceEqual([0, 2]));
            var own = s.V(0).Hand.Count; var other = s.V(2).Hand.Count; s.UseCard(action);
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Driver && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
            var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == action.CardId && f.CardKind == CardKind.DrawTwo);
            s.Need(use.TargetSeats.SequenceEqual([0, 2]) && use.Action!.PhysicalCards.Select(c => c.CardId).SequenceEqual([action.CardId!.Value]) &&
                s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == action.CardId).Location == CardLocation.Processing,
                "The real paid commitment retains the complete original-and-extra recipient list and the sole original physical entity in Processing.");
            s.Cold(); s.Continue(); s.Play();
            s.Need(s.V(0).Hand.Count == own + 1 && s.V(2).Hand.Count == other + 2 &&
                s.Facts<RedAdditionalTargetsConsumedEvent>().Single().Targets.SequenceEqual([0, 2]) &&
                s.Facts<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.SourceSeat == 0 &&
                    e.CardKind == CardKind.DrawTwo && e.CardId == action.CardId) == 1 &&
                s.Facts<TargetsConfirmedEvent>().Any(e => e.ResolutionId == use.Id && e.TargetSeats.SequenceEqual([0, 2])) &&
                s.Facts<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == action.CardId && e.CardKind == CardKind.DrawTwo) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == action.CardId && m.From == CardLocation.Hand(0) &&
                    m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1 &&
                !s.Actions().Any(a => a.ProgramActivationId == "red-additional-targets"),
                "The cold-restored issued benefit uses one real red trick with both native recipients, pays one material and is consumed exactly once after source loss. " +
                JsonSerializer.Serialize(new { OwnDelta = s.V(0).Hand.Count - own, OtherDelta = s.V(2).Hand.Count - other,
                    Consumed = s.Facts<RedAdditionalTargetsConsumedEvent>(),
                    Declared = s.Facts<CardUseDeclaredEvent>(), Finished = s.Facts<CardUseFinishedEvent>(),
                    Remaining = s.Actions().Count(a => a.ProgramActivationId == "red-additional-targets") }));
        }
    }

    private static string Peer(int seat) => "fixture:mobile-wu-xian-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry, string skill)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int[] DrawCounts() => G.Events.Select(e => e.Payload).SkipWhile(e => e is not TurnStartedEvent t || t.TurnNumber != G.State.TurnNumber)
            .OfType<CardsDrawnEvent>().Where(e => e.PlayerSeat == 0).Select(e => e.Count).ToArray();
        public IReadOnlyList<LegalAction> Actions() => G.GetLegalActions(0);
        public void Prepare() => Reach(() => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Choose(string option)
        { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == option) == true); Answer(c => c.Parameters.GetValueOrDefault("option-id") == option); }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void LoseSource() => Accept(new UseProgramSkillCommand(0, Driver, "lose-fumian", [], [], G.Revision, P!.PromptId));
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void UseCard(LegalAction action) => Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId,
            action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource });
        public void Answer(Func<PromptChoice, bool> select)
        { var prompt = P!; Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(select).Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 240 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select)
                { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard } play && play.PlayerSeat != 0)
                { Accept(new EndPlayPhaseCommand(play.PlayerSeat, G.Revision, play.PromptId)); continue; }
                if (P is { Kind: DecisionKind.Nullification } counter)
                { Answer(c => c.Parameters.GetValueOrDefault("response") == "pass"); continue; }
                Need(P is null, "Unexpected mobile Wu Xian native boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "Mobile Wu Xian did not reach the required native boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold restores exact phase assignment, source-loss obligations, views, movements and original native continuation.");
        }
        public void Need(bool condition, string message)
        { if (!condition) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(P)); }
        private void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Wu Xian command."); }
        public static Scene Start(bool mobile, string mode = "plain")
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(mobile, mode));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 14 }, r), r, mobile ? Mobile : Classic);
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture(bool mobile, string mode) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-wu-xian", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "classic:wu-xian");
            if (mobile) CharacterFixtureContent.Register(b, "mobile:wu-xian");
            var skill = mobile ? Mobile : Classic;
            var clockEffects = mode == "skip" ? """{"op":"skipTurnPhases","target":"owner","phases":["draw"]}""" :
                (mode == "extra-loss" ? """{"op":"replaceSkillsOnAwakening","target":"owner","skillIds":["standard:none"]},""" : "") +
                """{"op":"insertPhase","target":"owner","phase":"draw","phaseContinuation":"beforeNormalPreparation"}""";
            var extra = mode == "plain" ? "" : $$"""
                ,{"id":"{{Clock}}","revision":1,"triggers":[{"id":"first-real-draw","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"priority":-100,"usageScope":"game","usageLimit":1,"effects":[{{clockEffects}}]}
                {{(mode == "replace" ? ",{\"id\":\"replace-first\",\"window\":\"drawPhaseStarting\",\"drawPhaseMode\":\"replacement\",\"subject\":\"owner\",\"optional\":false,\"priority\":100,\"usageScope\":\"game\",\"usageLimit\":1,\"effects\":[{\"op\":\"draw\",\"target\":\"owner\",\"amount\":1}]}" : "")}}]},
                {"id":"{{Pause}}","revision":1,"triggers":[{"id":"assigned-before-cards","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"usageScope":"game","usageLimit":1,
                 "effects":[{"op":"chooseOption","target":"owner","resultBind":"phase","options":[{"id":"continue"}]}]}]}
                """;
            var presentation = new Dictionary<string, object> { [Driver] = new { name = "真实失源及用牌提交", description = "失去福绵原来源，并观察原生已付用牌。",
                optionLabels = new Dictionary<string, string> { ["continue"] = "继续原生用牌" } } };
            if (mode != "plain")
            {
                presentation[Clock] = new { name = "真实摸牌阶段", description = "额外、跳过或替代原生Draw。" };
                presentation[Pause] = new { name = "真实阶段入口", description = "已分配义务但尚未正常摸牌。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            }
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                {"id":"{{Driver}}","revision":1,"triggers":[{"id":"paid-real-trick","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["drawTwo"],"optional":false,"isSkillInvocation":false,
                  "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}],
                  "activations":[{"id":"lose-fumian","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                  "effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["{{skill}}"],"sourceBind":"standard:none"}]}]}{{extra}}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentation }));
            foreach (var (id, program) in programs.Programs) b.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description)
                { Program = program, ProgramPresentation = programs.Presentations[id] });
            b.AddGeneral(new(Owner, "移动吴苋真实来源", "supporter", skill, "shu", mode == "extra-loss" ? 2 : 30, mode == "plain" ? [Driver] : [Driver, Clock, Pause]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "真实锦囊受益人", "supporter", "standard:none", "wei", 30));
            b.AddDeck(new(Deck, "真实红色无中实体", 6, 2, []) { PhysicalCards = Enumerable.Range(0, 160)
                .Select(_ => new ContentDeckPhysicalCard("standard:draw_two", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(Mode, "移动吴苋实际回合与阶段", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1,
                [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
