using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileLiDianChecks
{
    private const string Skill = "mobile:wangxi", Owner = "fixture:mobile-li-dian-owner", Driver = "fixture:mobile-li-dian-driver",
        Watch = "fixture:mobile-li-dian-watch", Mode = "identity:classic-mobile-li-dian";
    private const string DrawReason = "skill.mobile-li-dian.wangxi.draw";

    public static void OtherParticipantDrawsFirstAndEachPointReturnsCold()
    {
        var s = Scene.Start(); s.Play(0); s.End(0); s.Play(1);
        var hp = s.V(0).Hp; var own = s.V(0).HandCount; var other = s.V(1).HandCount;
        s.Accept(new UseProgramSkillCommand(1, Driver, "two-damage", [], [0], s.G.Revision, s.P!.PromptId));
        s.Offer();
        var damage = s.G.ResolutionStack.OfType<DamageTriggerWindowFrame>().Single();
        s.Need(s.V(0).Hp == hp - 2, "One actual source-one native damage call applies two points to the living Wangxi owner.");
        s.Activate(); s.Gain(1);
        var first = s.Root(); var firstId = first.Id; var occurrence = first.WindowContext!.OccurrenceIndex;
        var stage = first.MobileLiDianWangxi is { RecipientIndex: 0, AwaitingMovement: true, Draw.RecipientSeat: 1 };
        var cause = first.MobileLiDianWangxi?.DamageId == damage.ParentFrameId;
        var order = first.MobileLiDianWangxi?.Recipients.SequenceEqual([1, 0]) == true;
        var otherPaid = s.V(1).HandCount == other + 1; var ownerUnpaid = s.V(0).HandCount == own;
        var pending = first.PendingMovementContinuation?.SubjectSeat == 1;
        // NativeDraw creates its actual batch before its operation return adds
        // PendingMovementContinuation. The original parent and paid entities
        // prove this child; the batch's pre-return awaiting annotation is null.
        var child = first.MobileLiDianWangxi?.Draw is { } paidDraw &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(w => w.Batch.ParentFrameId == first.Id &&
                w.Batch.AwaitingProgramFrameId is null && w.ResumeProgramFrameId is null && w.Batch.OriginSkillId == Skill &&
                w.Batch.OriginSkillInstanceId == first.SkillInstanceId && w.Batch.OriginOwnerSeat == first.OwnerSeat &&
                w.Batch.Movements.Select(m => m.CardId).SequenceEqual(paidDraw.ActualDrawIds) &&
                w.Batch.Movements.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(1) && m.Reason.Value == DrawReason && s.G.CardMovements.Contains(m)));
        s.Need(stage && cause && order && otherPaid && ownerUnpaid && pending && child,
            "The accepted first point freezes seating order source-one before owner-zero and pays only the first native draw before that recipient's actual gain child.");
        s.Cold(); s.Continue(); s.Gain(0);
        s.Need(s.Root().Id == firstId && s.Root().MobileLiDianWangxi is { RecipientIndex: 1, Draw.RecipientSeat: 0 } &&
            s.V(1).HandCount == other + 1 && s.V(0).HandCount == own + 1,
            "The restored typed return advances the same original frame to its second recipient without repeating the paid first draw.");
        s.Continue(); s.Offer();
        var secondWindow = s.G.ResolutionStack.OfType<DamageTriggerWindowFrame>().Single();
        s.Need(secondWindow.Id == damage.Id && s.Facts<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Skill && e.Activated && e.Completed) == 1,
            "The second point independently offers Wangxi in the same actual damage window after the first complete pair.");
        s.Activate(); s.Gain(1);
        s.Need(s.Root().Id != firstId && s.Root().WindowContext!.OccurrenceIndex != occurrence &&
            s.Root().MobileLiDianWangxi!.DamageId == damage.ParentFrameId, "Each actual point owns a distinct original program frame and occurrence.");
        s.Continue(); s.Gain(0); s.Continue(); s.Play(1);
        var moves = s.G.CardMovements.Where(m => m.Reason.Value == DrawReason).ToArray();
        s.Need(moves.Length == 4 && moves.Select(m => m.To.OwnerSeat).SequenceEqual(new int?[] { 1, 0, 1, 0 }) &&
            moves.All(m => m.From == CardLocation.DrawPile && m.To.Zone == CardZoneKind.Hand) && moves.Select(m => m.CardId).Distinct().Count() == 4 &&
            s.V(1).HandCount == other + 2 && s.V(0).HandCount == own + 2 &&
            s.Facts<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Skill && e.Activated && e.Completed) == 2 &&
            s.Facts<DamageAppliedEvent>().Count(e => e.SourceSeat == 1 && e.TargetSeat == 0 && e.Amount == 2) == 1,
            "Two accepted per-point triggers finish four distinct actual one-card draws in seating order, preserve their single native damage cause and never duplicate the cold payment.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public IEnumerable<T> Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public ProgramSkillFrame Root() => G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        public void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null })); }
        public void Accept(GameCommand c) { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected fixed original command."); }
        public void Answer(Func<PromptChoice, bool> test) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(test).Id, G.Revision)); }
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Offer() => Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Gain(int seat) => Reach(() => P?.SkillPrompt?.SkillId == Watch && P.PlayerSeat == seat);
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision, P!.PromptId));
        public void Reach(Func<bool> goal)
        {
            for (var step = 0; step < 180 && !goal(); step++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } pick) { Accept(new SelectGeneralCommand(pick.PlayerSeat, pick.PlayerSeat == 0 ? Owner : "fixture:mobile-li-dian-peer-" + pick.PlayerSeat, G.Revision, pick.PromptId)); continue; }
                if (P is { } unexpected) throw new InvalidOperationException("Unexpected mobile Wangxi prompt: " + JsonSerializer.Serialize(new { unexpected.Kind, unexpected.PlayerSeat, unexpected.SkillPrompt, unexpected.Choices }));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(goal(), "Missing bounded original mobile Wangxi boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements, Facts = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "The original recipient order, native draw invoice, movement child and all private views survive checkpoint JSON.");
        }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var g = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5 }, r);
            var s = new Scene(g, r); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-li-dian", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:li-dian", "boundary:li-dian");
            object[] skills = [new { id = Driver, revision = 1, activations = new[] { new { id = "two-damage", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1,
                targetKind = "otherLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "damage", target = "selectedTarget", amount = 2 } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "actual-draw-gain", window = "cardsGained", subject = "owner", optional = false,
                    destinationZones = new[] { "hand" }, movementReasons = new[] { DrawReason }, movementOccurrence = "perBatch",
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "seen", options = new[] { new { id = "continue" } } } } } } }];
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实两点伤害", description = "同一正常伤害调用。" },
                [Watch] = new { name = "实际摸牌获得孩子", description = "收牌人已取得真实实体。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "移动忘隙真实持有人", "supporter", Skill, "wei", 4, [Watch]));
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:mobile-li-dian-peer-" + i).ToArray();
            foreach (var peer in peers) b.AddGeneral(new(peer, "固定真实伤害角色", "supporter", "standard:none", "wu", 4, [Driver, Watch]));
            const string deck = "fixture:mobile-li-dian-deck";
            b.AddDeck(new(deck, "固定真实摸牌实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, "原生逐点忘隙", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck,
                GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
