using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileCaoRenChecks
{
    private const string Hero = "mobile:cao-ren", Skill = "mobile:jushou", Owner = "fixture:ordinary-mobile-cao-ren",
        Driver = "fixture:ordinary-cao-ren-turn", Watch = "fixture:ordinary-cao-ren-draw-child", Quiet = "fixture:ordinary-cao-ren-no-draw",
        Deck = "fixture:ordinary-cao-ren-deck", Mode = "identity:classic-ordinary-mobile-cao-ren", DrawReason = "skill-program.mobile:jushou.Draw";

    public static void FaceDownEndingDrawChildReturnsColdBeforeSingleTurnOver()
    {
        var s = Scene.Start(); s.Play();
        s.Accept(new UseProgramSkillCommand(0, Driver, "turn-back", [], [], s.G.Revision, s.P!.PromptId)); s.Play();
        s.Need(s.V.IsFaceDown && s.V.HandCount == 0 && s.Facts<CharacterStateChangedEvent>().Any(e =>
            e.Change.Window == SkillProgramTriggerWindow.CharacterTurnedOver && e.Change.TurnedOver is { WasFaceDown: false, IsFaceDown: true }),
            "An accepted real Play-phase TurnOver establishes the already-face-down premise without changing stored state directly.");
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill);
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill); var rootId = root.Id;
        var drawn = s.G.CardMovements.Where(m => m.Reason.Value == DrawReason && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0)).Select(m => m.CardId).ToArray();
        s.Need(root.InstructionIndex == 1 && drawn.Length == 3 && drawn.Distinct().Count() == 3 && s.V.IsFaceDown && s.V.HandCount == 3 &&
            drawn.All(id => s.V.Hand.Any(c => c.Id == id)) &&
            !s.Facts<CharacterStateChangedEvent>().Any(e => e.Change.ParentFrameId == rootId && e.Change.Window == SkillProgramTriggerWindow.CharacterTurnedOver),
            "The three actual Draw entities are already gained in a real child of the original Jushou frame; its later TurnOver has not happened yet.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.Facts<ProgramBindingResolvedEvent>().Any(e => e.FrameId == rootId && e.Completed));
        s.Need(!s.V.IsFaceDown && s.V.HandCount == 3 && drawn.All(id => s.V.Hand.Any(c => c.Id == id)) &&
            s.Facts<CharacterStateChangedEvent>().Where(e => e.Change.ParentFrameId == rootId && e.Change.Window == SkillProgramTriggerWindow.CharacterTurnedOver).ToArray() is
                [{ Change.TurnedOver: { WasFaceDown: true, IsFaceDown: false } }] &&
            s.G.CardMovements.Count(m => m.Reason.Value == DrawReason && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0)) == 3 &&
            s.Facts<ProgramBindingResolvedEvent>().Count(e => e.FrameId == rootId && e.Activated && e.Completed) == 1,
            "Cold returns to the same paid Draw3 frame, keeps all three original entities and turns the already-face-down owner face up exactly once.");
    }

    private static string Peer(int seat) => "fixture:ordinary-cao-ren-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V => G.CreateSnapshot(0).Players.Single(p => p.Seat == 0);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Cao Ren command."); }
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                if (P is { Kind: DecisionKind.SelectGeneral } choose) { Accept(new SelectGeneralCommand(choose.PlayerSeat, choose.PlayerSeat == 0 ? Owner : Peer(choose.PlayerSeat), G.Revision, choose.PromptId)); continue; }
                if (P?.SkillPrompt?.SkillId == Watch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip"))
                { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); continue; }
                Need(P is null, "Unexpected fixed native Cao Ren boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The fixed Cao Ren scene did not reach its next native boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the paid Draw3 entities, every view and the original pre-TurnOver parent and gain child.");
        }
        public void Need(bool yes, string why)
        { if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.Phase, V.IsFaceDown, V.HandCount,
            Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null })); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4 }, r), r);
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ordinary-mobile-cao-ren", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var definitions = new object[] {
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "no-normal-draw", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false,
                    effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Driver, revision = 1, activations = new[] { new { id = "turn-back", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0,
                    targetKind = "anyLiving", usesPerTurn = 1, usesPerPhase = 1, effects = new[] { new { op = "turnOver", target = "owner" } } } } },
                new { id = Watch, revision = 1, triggers = new object[] {
                    new { id = "real-jushou-gain-child", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch",
                        movementReasons = new[] { DrawReason }, optional = false, isSkillInvocation = false, usageScope = "game", usageLimit = 1,
                        effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "child", options = new[] { new { id = "continue" } } } } },
                    new { id = "native-turnover-fact", window = "characterTurnedOver", subject = "owner", optional = true, isSkillInvocation = false,
                        effects = new[] { new { op = "draw", target = "owner", amount = 1 } } }
                } }
            };
            var labels = new Dictionary<string, object> { [Quiet] = new { name = "原生零张正常摸牌", description = "保留真实阶段，正常摸牌修正减2。" },
                [Driver] = new { name = "真实翻背前置", description = "在合法Play使用原生TurnOver。", activationLabels = new Dictionary<string, string> { ["turn-back"] = "原生翻背" } },
                [Watch] = new { name = "真实摸牌子窗", description = "摸3已付且尚未翻面；可选翻面观察者均放弃。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续原据守" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "真实普通移动曹仁", "supporter", Skill, "wei", 4, [Driver, Watch, Quiet]));
            for (var i = 1; i < 4; i++) b.AddGeneral(new(Peer(i), "真实其他角色", "supporter", Quiet, "wei", 8));
            b.AddDeck(new(Deck, "固定真实据守摸牌实体", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray() });
            b.AddMode(new(Mode, "普通移动曹仁双向翻面", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
