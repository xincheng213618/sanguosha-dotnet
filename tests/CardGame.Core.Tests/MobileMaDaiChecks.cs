using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileMaDaiChecks
{
    private const string Hero = "mobile:ma-dai", Skill = "mobile:ma-dai-qianxi", Owner = "fixture:mobile-ma-dai-owner", Mode = "identity:classic-mobile-ma-dai",
        Quiet = "fixture:mobile-ma-dai-quiet", Final = "fixture:mobile-ma-dai-judgment", Health = "fixture:mobile-ma-dai-maximum", Prepare = "fixture:mobile-ma-dai-prepare";
    private const string Reason = "skill.mobile.ma-dai-qianxi";

    public static void NativeNearSlashJudgmentPreventionMaximumChildAndHeartCold()
    {
        var s = Scene.Start(Suit.Spade); s.Play(); s.PrepareJudgment(); s.Play(); s.Alcohol(); s.Play(); var slash = s.Slash(); s.Offer();
        s.Need(s.V(2).Hp == 4 && s.V(2).MaxHp == 4 && !s.E<DamageAppliedEvent>().Any() && !s.E<JudgmentRequestedEvent>().Any(),
            "Mashu makes the opposite seat's actual distance one: the real wine Slash offers Qianxi before damage or judgment, without paying a maximum yet.");
        s.Activate(); s.Hold(Final); var id = s.Root.Id;
        s.Need(s.G.ResolutionStack.OfType<JudgmentFrame>().Any(j => j.ParentFrameId == id && j.TargetSeat == 0) &&
            s.E<JudgmentResolvedEvent>().Single(e => e.Reason == Reason) is { TargetSeat: 0, Suit: Suit.Spade } &&
            !s.E<MobileMaDaiMaximumReducedEvent>().Any() && s.V(2).Hp == 4,
            "The native owner judgment is finalized while its real child still owns the original parent; no maximum loss overtakes that child.");
        s.Cold(); s.Continue(); s.Hold(Health);
        var root = s.Root; var outcome = root.JudgmentOutcomeBindings!.Single();
        var hp = s.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(h => h.ResumeFrameId == id);
        var reduced = s.E<MobileMaDaiMaximumReducedEvent>().Single();
        s.Need(root.Id == id && root.InstructionIndex == 3 && root.PendingMovementContinuation is null &&
            hp.Continuation == PostEventContinuation.Program && hp.Change is { Kind: HpChangeKind.MaximumHp, TargetSeat: 2, Amount: 1 } && hp.Change.ParentFrameId == id &&
            s.G.ResolutionStack.OfType<BeforeDamageProgramWindowFrame>().Any(w => w.Id == reduced.BeforeDamageFrameId && w.Prevented) &&
            reduced is { OwnerSeat: 0, TargetSeat: 2, MaximumBefore: 4, MaximumAfter: 3 } && reduced.ProgramFrameId == id && reduced.JudgmentFrameId == outcome.JudgmentFrameId &&
            s.V(2).Hp == 3 && s.V(2).MaxHp == 3 && !s.E<DamageAppliedEvent>().Any() && s.Zone(slash) == CardLocation.Processing &&
            s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
            "A non-heart result first prevents the actual two-point Slash, then pays exactly one target maximum HP and clamps HP once; its native health child returns to the already committed original instruction.");
        s.Cold(); s.Continue(); s.Play();
        s.Need(s.V(2).Hp == 3 && s.V(2).MaxHp == 3 && !s.E<DamageAppliedEvent>().Any() &&
            s.E<MaximumHpChangedEvent>().Count(e => e.SkillId == Skill && e.PlayerSeat == 2 && e.Delta == -1) == 1 &&
            s.E<MobileMaDaiMaximumReducedEvent>().Count(e => e.ProgramFrameId == id) == 1 &&
            s.E<JudgmentResolvedEvent>().Count(e => e.Reason == Reason) == 1 && s.Zone(outcome.CardId!.Value) == CardLocation.DiscardPile &&
            s.Zone(slash) == CardLocation.DiscardPile && s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.CardId == slash && e.CardKind == CardKind.Slash) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == id && e.Completed) == 1 && s.G.ResolutionStack.All(f => f.Id != id),
            "Cold health return completes original judgment cleanup and Slash once, with one material payment and one maximum reduction, and no delayed or repeated damage.");

        var heart = Scene.Start(Suit.Heart); heart.Play(); heart.PrepareJudgment(); heart.Play(); heart.Alcohol(); heart.Play(); var material = heart.Slash(); heart.Offer();
        heart.Activate(); heart.Hold(Final); heart.Continue(); heart.Play();
        heart.Need(heart.V(2).MaxHp == 4 && heart.V(2).Hp == 2 &&
            heart.E<JudgmentResolvedEvent>().Single(e => e.Reason == Reason) is { TargetSeat: 0, Suit: Suit.Heart } &&
            !heart.E<MobileMaDaiMaximumReducedEvent>().Any() && !heart.E<MaximumHpChangedEvent>().Any(e => e.SkillId == Skill) &&
            heart.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 2 && e.Amount == 2) == 1 &&
            heart.Zone(material) == CardLocation.DiscardPile && heart.E<CardUseFinishedEvent>().Count(e => e.CardKind == CardKind.Slash) == 1,
            "A real heart judgment leaves the original wine-enhanced two-point Slash damage intact and never reduces maximum HP.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public ProgramSkillFrame Root => G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Offer() => Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Hold(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill);
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Alcohol() => Native(V().Hand.Single(c => c.Kind == CardKind.Alcohol).Id, 0);
        public void PrepareJudgment() => Accept(new UseProgramSkillCommand(0, Prepare, "stock-judgment", [V().Hand.Single(c => c.Kind == CardKind.Dodge).Id], [], G.Revision, P!.PromptId));
        public int Slash() { var id = V().Hand.Single(c => c.Kind == CardKind.Slash).Id; Native(id, 2); return id; }
        private void Native(int card, int target)
        {
            var action = G.GetLegalActions(0).Single(a => a.CardId == card &&
                (a.TargetSeats.SequenceEqual([target]) || target == 0 && a.Kind == LegalActionKind.Alcohol && a.TargetSeats.Count == 0));
            Accept(new PlayCardCommand(0, card, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        }
        public void Answer(Func<PromptChoice, bool> match) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(match).Id, G.Revision)); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 160; i++)
            {
                if (ready()) return;
                if (P is { Kind: DecisionKind.SelectGeneral } select) { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.RespondDodge }) { Answer(c => c.Parameters.GetValueOrDefault("response") == "take-damage"); continue; }
                Need(P is null, "Unexpected native mobile Ma Dai boundary.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded native mobile Ma Dai scene missed its original child boundary.");
        }
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves all original native judgment, prevented damage, already-paid maximum HP and typed health-return frames.");
        }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Ma Dai command."); }
        public void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { P, G.State.CurrentSeat, G.State.Phase, Players = Enumerable.Range(0, 4).Select(V).Select(p => new { p.Seat, p.Hp, p.MaxHp, p.HandCount }), Frames = G.ResolutionStack.Select(f => new { f.Id, f.Kind }) })); }
        public static Scene Start(Suit suit)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(suit));
            var engine = GameEngine.CreateStandard(new() { Seed = 47, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
            var scene = new Scene(engine, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private static string Peer(int seat) => "fixture:mobile-ma-dai-peer-" + seat;

    private sealed class Fixture(Suit suit) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-ma-dai", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "classic:ma-chao", Hero);
            object Pause() => new { op = "chooseOption", target = "owner", resultBind = "paid", options = new[] { new { id = "continue" } } };
            var skills = new object[] {
                new { id = Quiet, revision = 1, modifiers = new[] { new { id = "fixed-three-opening-entities", query = "initialHandSize", operation = "add", value = 3, priority = 0 } }, triggers = new[] { new { id = "quiet", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false,
                    effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Prepare, revision = 1, activations = new[] { new { id = "stock-judgment", minCards = 1, maxCards = 1, sourceZones = new[] { "hand" }, cardKinds = new[] { "dodge" },
                    minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, usesPerGame = (int?)null,
                    effects = new object[] { new { op = "captureSelectedCards", target = "owner", resultBind = "prepared" }, new { op = "moveBoundCards", target = "owner", sourceBind = "prepared", destination = "drawPileTop", awaitMovementTriggers = true } } } } },
                new { id = Final, revision = 1, triggers = new[] { new { id = "actual-final-judgment", window = "judgmentFinalized", subject = "any", optional = false, isSkillInvocation = false,
                    judgmentReasons = new[] { Reason }, suits = new[] { "spade", "club", "diamond", "heart" }, excludedReasons = Array.Empty<string>(), minimumRank = 1, maximumRank = 13, effects = new[] { Pause() } } } },
                new { id = Health, revision = 1, triggers = new[] { new { id = "actual-maximum-child", window = "afterHealthChanged", subject = "owner", optional = false, isSkillInvocation = false,
                    condition = new { kind = "healthChangeKindIs", hpChangeKind = "maximumHp" }, effects = new[] { Pause() } } } }
            };
            var labels = new Dictionary<string, object> { [Quiet] = new { name = "固定开局实体", description = "三张实体全部原生发给本体" }, [Prepare] = new { name = "原生准备判定", description = "实际将手中固定闪放回牌堆顶" } };
            foreach (var id in new[] { Final, Health }) labels[id] = new { name = "马岱原生孩子", description = "原生判定及实付上限子窗", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Owner, "移动马岱固定身份", "supporter", "classic:mashu", "shu", 4, AdditionalSkillIds: [Skill, Quiet, Final, Prepare], Gender: GeneralGender.Male));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "固定上限同桌", "supporter", Health, "wei", 4));
            const string deck = "fixture:mobile-ma-dai-deck";
            b.AddDeck(new(deck, "固定真实判定", 0, 2, []) { PhysicalCards = [new("standard:alcohol", Suit.Heart, 4), new("standard:slash", Suit.Club, 6), new("standard:dodge", suit, 7)] });
            b.AddMode(new(Mode, "移动马岱原生潜袭", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
