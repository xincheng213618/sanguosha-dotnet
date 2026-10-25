using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryZhangZhaoMobileChecks
{
    private const string Hero = "boundary:zhang-zhao-zhang-hong-mobile", Zhijian = "boundary:zhijian-mobile", Guzheng = "boundary:guzheng-mobile", Mode = "identity:mobile-two-zhangs",
        Owner = "fixture:mobile-two-zhangs-owner", Watch = "fixture:mobile-two-zhangs-placement", Drop = "fixture:mobile-two-zhangs-drop", Gain = "fixture:mobile-two-zhangs-gain";
    public static void EmptySlotHandPlacementAndWholeDiscardPhaseReturnClaimCold()
    {
        var s = Scene.Start(); var before = s.V(0).HandCount;
        s.Accept(new UseProgramSkillCommand(0, Zhijian, "equip-target", [], [1], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        var card = s.P!.Choices.First(c => c.Cards.Count == 1).Cards.Single(); s.Answer(c => c.Cards.SequenceEqual([card]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        s.Need(s.V(0).HandCount == before - 1 && s.V(1).Equipment.Single().Id == card &&
            s.E<BoundaryZhangZhaoZhijianPaidEvent>().Single().CardId == card,
            "The mobile hand equipment really enters another player's empty slot before its one draw, with no card use or replacement.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play(0);
        s.Need(s.V(0).HandCount == before && s.G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Hand(0) && m.To == CardLocation.Equipment(1)) == 1,
            "The original placement movement child returns once after cold and rewards exactly one native draw.");
        var equip = s.G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Equip); var own = equip.CardId!.Value;
        s.Accept(new PlayCardCommand(0, own, equip.TargetSeats, s.G.Revision, s.P!.PromptId, equip.PlayedCardKind)); s.Play(0);
        s.Need(s.V(0).HandCount == before && s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Zhijian && e.BindingId == "draw-on-play-equipment" && e.Completed) == 1 &&
            s.G.GetHumanLegalActions().Where(a => a.ProgramSkillId == Zhijian).All(a => !a.SelectableTargetSeats.Contains(1)),
            "An actual own Play equipment use draws once, while the occupied recipient slot is excluded from placement.");
        s.End(0); s.Play(1);
        s.Accept(new UseProgramSkillCommand(1, Drop, "lower-before-discard", [], [], s.G.Revision, s.P!.PromptId)); s.Play(1);
        s.Need(s.V(1) is { Hp: 2, MaxHp: 4, HandCount: 4 }, "A real native two-HP loss establishes the exact two-card hand-limit obligation before the actual discard phase.");
        s.End(1);
        s.Reach(() => s.P?.PlayerSeat == 1 && s.P.Choices.Any(c => c.Cards.SequenceEqual([card]))); s.Answer(c => c.Cards.SequenceEqual([card]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Guzheng && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "boundary-zhang-zhao-phase") == true);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Guzheng); var rootId = root.Id; var pool = root.BoundaryZhangZhaoPhase!.OriginalEntities;
        s.Need(pool.Count == 3 && pool.Count(e => e.OriginalFrom == CardLocation.Hand(1)) == 2 && pool.Single(e => e.CardId == card).OriginalFrom == CardLocation.Equipment(1) &&
            s.P!.Choices.All(c => c.Cards.Count == 1 && c.Cards[0] != card),
            "The exact completed discard phase includes both native hand-limit discards and the equipment discarded in its actual phase-end child; only the phase owner's hand cards can be returned.");
        var returned = s.P.Choices.First().Cards.Single(); s.Answer(c => c.Cards.SequenceEqual([returned]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Gain);
        s.Need(s.V(1).Hand.Any(c => c.Id == returned) && pool.Where(e => e.CardId != returned).All(e => s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == e.CardId).Location == CardLocation.DiscardPile),
            "One real hand card has been returned before the optional remaining-card claim, and its genuine gain child is still pending.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "claim") == true);
        s.Need(s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "decline"), "Claiming the remaining original phase discards stays optional after the real return.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "claim"); s.Play(2);
        s.Need(s.V(1).Hand.Any(c => c.Id == returned) && pool.Where(e => e.CardId != returned).All(e => s.V(0).Hand.Any(c => c.Id == e.CardId)) &&
            s.E<BoundaryZhangZhaoReturnedEvent>().Count(e => e.FrameId == rootId) == 1 && s.E<BoundaryZhangZhaoClaimFinishedEvent>().Single(e => e.FrameId == rootId) is { Taken: true, Count: 2 } &&
            !s.G.ResolutionStack.Any(f => f.Id == rootId),
            "The original paid return survives cold, then claims exactly the other hand card and equipment once through native movement and phase-end return.");
    }
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game; public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Mobile two Zhangs command rejected."); }
        public void Answer(Func<PromptChoice, bool> test) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(test).Id, G.Revision)); }
        public void End(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision));
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void Reach(Func<bool> done) { for (var n = 0; n < 140 && !done(); n++) { Need(G.State.Winner == Winner.None && P is null, "Unexpected mobile two Zhangs prompt."); Accept(new AdvanceOneStepCommand(G.Revision)); } Need(done(), "Missing mobile two Zhangs native boundary."); }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Two Zhangs cold preserves original paid frames and all four private views."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, G.State.TurnNumber, V(0).HandCount })); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var g = GameEngine.CreateStandard(new() { Seed = 47, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
                UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, r);
            var s = new Scene(g, r); s.Accept(new StartGameCommand());
            for (var n = 0; n < 100; n++)
            {
                if (s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return s;
                if (s.P is { Kind: DecisionKind.SelectGeneral } p) { s.Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : "fixture:mobile-two-zhangs-peer-" + p.PlayerSeat, g.Revision, p.PromptId)); continue; }
                s.Need(s.P is null, "Unexpected two Zhangs opening prompt."); s.Accept(new AdvanceOneStepCommand(g.Revision));
            }
            throw new InvalidOperationException("Two Zhangs fixed opening missing.");
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-two-zhangs", new(1, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var pause = new object[] { new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" } } } };
            var skills = new object[] {
                new { id = Watch, revision = 1, triggers = new[] { new { id = "real-placement-child", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { "skill-program.boundary:zhijian-mobile.SelectAndMoveOwnedCard" }, optional = false, effects = pause } } },
                new { id = Gain, revision = 1, triggers = new[] { new { id = "real-return-child", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { "skill-program.boundary:guzheng-mobile.phase-return" }, optional = false, effects = pause } } },
                new { id = Drop, revision = 1, activations = new[] { new { id = "lower-before-discard", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "loseHp", target = "owner", amount = 2 } } } }, triggers = new[] { new { id = "original-discard-phase-equipment", window = "discardPhaseEnded", subject = "owner", turnOwnerScope = "own", allowOwnDiscardPhaseEnded = true, optional = false, usageScope = "game", usageLimit = 1,
                    effects = new object[] { new { op = "selectAndMoveOwnedCard", target = "owner", chooserRef = new { kind = "owner" }, cardOwnerRef = new { kind = "owner" }, zones = new[] { "equipment" }, count = 1, destination = "discardPile", awaitMovementTriggers = true } } } } } };
            var continueLabels = new Dictionary<string, string> { ["continue"] = "继续" };
            var labels = new Dictionary<string, object> { [Watch] = new { name = "真实放装孩子", description = "手牌进入他人空装备槽后的移动。", optionLabels = continueLabels }, [Gain] = new { name = "真实固政归还孩子", description = "实际归还手牌后的获得窗口。", optionLabels = continueLabels }, [Drop] = new { name = "真实阶段装备弃置", description = "在自己的弃牌阶段结束窗口弃置实际装备。" } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            var ids = new List<string> { Owner }; b.AddGeneral(new(Owner, "原生移动二张", "supporter", Zhijian, "wu", 3, [Guzheng, Watch]));
            for (var i = 1; i < 4; i++) { var id = "fixture:mobile-two-zhangs-peer-" + i; ids.Add(id); b.AddGeneral(new(id, "真实弃牌方", "supporter", "standard:none", "wei", 4, i == 1 ? [Drop, Gain] : [])); }
            b.AddDeck(new("fixture:mobile-two-zhangs-deck", "固定装备牌", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:bagua", Suit.Club, 7)).ToArray() });
            b.AddMode(new(Mode, "二张原生弃牌阶段", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, "fixture:mobile-two-zhangs-deck", GeneralCandidateCount: 4, GeneralPoolIds: ids));
        }
    }
}
