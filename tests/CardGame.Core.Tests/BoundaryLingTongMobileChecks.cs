using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryLingTongMobileChecks
{
    private const string Hero = "boundary:ling-tong-mobile", Skill = "boundary:xuanfeng-mobile", Owner = "fixture:boundary-ling-tong-owner",
        Driver = "fixture:boundary-ling-tong-driver", Hp = "fixture:boundary-ling-tong-hp", Phase = "fixture:boundary-ling-tong-phase",
        Mode = "identity:classic-boundary-ling-tong";
    private const string Transfer = "skill-program." + Skill + ".SelectAndMoveOwnedCard", Discard = "skill-program." + Skill + ".ChooseOtherOwnedCardDiscard";

    public static void EquipmentTransferAndSequentialDiscardsResumeNativeChildrenCold()
    {
        var s = Scene.Start(); s.Play(); s.Use("equip", 0); s.Play(); s.Use("equip", 1); s.Play(); s.Use("hurt", 1); s.Play();
        var lion = s.V(1).Equipment.Single(c => c.Kind == CardKind.SilverLion).Id;
        s.DiscardOwnEquipment(); s.Reach(s.Offered); s.Activate(); s.Reach(s.Branch); s.ChooseBranch("move");
        s.Reach(() => s.Action("boundary-ling-tong-equipment"));
        s.Need(s.P!.Choices.All(c => c.Targets.Count == 2 && c.Targets[0] != 0 && c.Targets[1] != 0 && c.Targets[0] != c.Targets[1]) &&
            s.P.Choices.Any(c => c.Targets.SequenceEqual(new[] { 1, 2 })), "Both equipment participants are other distinct living roles and only actual compatible empty slots are offered.");
        s.Cold(); s.Answer(c => c.Targets.SequenceEqual([1, 2]));
        s.Reach(() => s.Action("select-and-move-owned-card")); s.Cold(); s.Answer(c => c.Cards.SequenceEqual([lion]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Hp);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        var rootId = root.Id;
        s.Need(root.SelectedTargetSeats.SequenceEqual(new[] { 1, 2 }) && root.PendingMovementContinuation is not null &&
            s.V(2).Equipment.Any(c => c.Id == lion) && s.V(1).Hp == s.V(1).MaxHp &&
            s.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Any(f => f.ResumeFrameId == rootId),
            "The genuine equipped Silver Lion moves once and opens its recovery child before the original Xuanfeng frame completes.");
        s.Cold(); s.Answer(_ => true); s.Play();
        s.Need(s.G.CardMovements.Count(m => m.CardId == lion && m.From == CardLocation.Equipment(1) && m.To == CardLocation.Equipment(2) && m.Reason.Value == Transfer) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Skill && e.Completed && e.Activated) == 1,
            "Paid native movement and recovery cold-return exactly once to the original skill, retaining the actual physical equipment.");

        s.Use("equip", 0); s.Play(); s.Use("hurt", 2); s.Play(); s.DiscardOwnEquipment(); s.Reach(s.Offered); s.Activate(); s.Reach(s.Branch); s.ChooseBranch("discard");
        s.Reach(() => s.Action("choose-other-owned-card-discard")); s.Answer(c => c.Cards.SequenceEqual([lion]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Hp);
        var paidRoot = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill).Id;
        s.Need(s.G.CardMovements.Count(m => m.Reason.Value == Discard) == 1 && s.V(2).Hp == s.V(2).MaxHp,
            "The first sequential discard really removes the armor and resolves native recovery before any second discard prompt.");
        s.Cold(); s.Answer(_ => true); s.Reach(() => s.Action("choose-other-owned-card-discard"));
        s.Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill).Id == paidRoot,
            "The second optional discard resumes the same paid program frame.");
        s.Answer(c => c.Targets.SequenceEqual([3]) && c.Parameters.GetValueOrDefault("source-zone") == "Hand"); s.Play();
        s.Need(s.G.CardMovements.Where(m => m.Reason.Value == Discard).Select(m => m.From.OwnerSeat).SequenceEqual(new int?[] { 2, 3 }) &&
            s.G.CardMovements.Count(m => m.CardId == lion && m.Reason.Value == Discard) == 1 && s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Skill) == 2,
            "The two sequential native discards can hit different other roles; recovery/cold do not repeat either payment or create an extra Xuanfeng activation.");
    }

    public static void DiscardPhaseCountsActualHeDiscardsAndDoesNotBroadenHandLoss()
    {
        var s = Scene.Start(phaseDiscard: true); s.Play();
        s.Use("discard-hand-two"); s.Reach(() => s.Action("select-owned-cards"));
        var handPaid = s.V(0).Hand.Take(2).Select(c => c.Id).ToArray();
        foreach (var card in handPaid) { s.Reach(() => s.Action("select-owned-cards")); s.Answer(c => c.Cards.SequenceEqual([card])); }
        s.Play(); s.Need(s.E<ProgramBindingStartedEvent>().All(e => e.SkillId != Skill),
            "Losing two hand cards in Play is not the OL all-zone two-card-loss trigger.");
        s.Use("equip", 0); s.Play(); var gear = s.V(0).Equipment.Single().Id;
        s.EndPlay(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Phase && s.Action("select-owned-cards"));
        var phaseHand = s.V(0).Hand[0].Id; s.Answer(c => c.Cards.SequenceEqual([gear]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Phase && s.Action("select-owned-cards")); s.Answer(c => c.Cards.SequenceEqual([phaseHand]));
        s.Reach(s.Offered);
        s.Need(s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(f => f.Candidates.Any(c => c.SkillId == Skill && c.BindingId == "equipment-left")),
            "A real HE discard first issues one owner-batch equipment-loss opportunity during the actual Discard phase.");
        s.Skip(); s.Reach(s.Offered);
        var boundary = s.G.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(f => f.Window == SkillProgramTriggerWindow.DiscardPhaseEnded);
        s.Need(boundary.Candidates.Count(c => c.SkillId == Skill && c.BindingId == "discard-phase") == 1 &&
            s.E<NativeLogicalDiscardInvocationRecordedEvent>().Any(e => e.ExecutorSeat == 0 && e.ActualCount == 2 && e.HandCount == 1 && e.Phase?.Kind == ActualDiscardRecoveryPhaseKind.Discard),
            "The completed own Discard phase counts its real equipment plus hand discard, despite the legacy hand-only counter being just one.");
        s.Cold(); s.Activate(); s.Reach(s.Branch);
        s.Need(s.P!.Choices.All(c => c.Parameters.GetValueOrDefault("option-id") != "move"),
            "A transfer branch is absent when no other role has a legal equipped entity; it never offers a no-op or equipment replacement.");
        s.ChooseBranch("discard"); s.Reach(() => s.Action("choose-other-owned-card-discard"));
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "choose-other-owned-card-decline");
        s.Reach(() => s.Action("choose-other-owned-card-discard")); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "choose-other-owned-card-decline");
        s.Reach(() => s.G.CreateSnapshot(0).TurnNumber >= 2);
        s.Need(s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Skill) == 1 && s.G.CardMovements.All(m => m.Reason.Value != Discard),
            "The mobile up-to-two branch can decline native discards; its separate end-of-Discard opportunity resolves once without treating the earlier Play hand loss as eligible.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public bool Action(string name) => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == name) == true;
        public bool Offered() => P?.SkillPrompt?.SkillId == Skill && Action("activate");
        public bool Branch() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "xuanfeng-branch") == true;
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string id, params int[] targets) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId));
        public void EndPlay() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Skip() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
        public void ChooseBranch(string id) => Answer(c => c.Parameters.GetValueOrDefault("option-id") == id);
        public void DiscardOwnEquipment()
        { var id = V(0).Equipment.Single().Id; Use("discard-gear"); Reach(() => Action("select-owned-cards")); Answer(c => c.Cards.SequenceEqual([id])); }
        public void Answer(Func<PromptChoice, bool> choose) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(choose).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Ling Tong command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 200 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p) { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : "fixture:boundary-ling-tong-peer-" + p.PlayerSeat, G.Revision, p.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } foreign) { Accept(new EndPlayPhaseCommand(foreign.PlayerSeat, G.Revision, foreign.PromptId)); continue; }
                if (P?.SkillPrompt?.SkillId == Hp) { Answer(_ => true); continue; }
                if (P is { Kind: DecisionKind.ProgramTrigger }) throw new InvalidOperationException("Unexpected Ling Tong native boundary: " + Short());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "Bounded Ling Tong scene missed the expected native boundary.");
        }
        public void Cold()
        { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold retains native source/destination, private choice, physical paid movement and original program children exactly."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        private string Short() => JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Players = Enumerable.Range(0, 5).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount, Equipment = V(i).Equipment.Count }) });
        public void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message + " " + Short()); }
        public static Scene Start(bool phaseDiscard = false)
        { var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(phaseDiscard)); var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, r)); s.Accept(new StartGameCommand()); return s; }
    }

    private sealed class Fixture(bool phaseDiscard) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-ling-tong-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            object Select(string zone, int count) => new { op = "selectOwnedCards", target = "owner", minimumCards = count, maximumCards = count, zones = new[] { zone }, resultBind = "paid" };
            object Move() => new { op = "moveBoundCards", target = "owner", sourceBind = "paid", destination = "discardPile", awaitMovementTriggers = true };
            object Activation(string id, int targets, params object[] effects) => new { id, minCards = 0, maxCards = 0, minTargets = targets, maxTargets = targets, targetKind = "anyLiving", usesPerTurn = (int?)null, effects };
            object Pause(string id, string window) => new { id, window, subject = "owner", optional = false, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } } } };
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] {
                    Activation("equip", 1, new { op = "useRandomDeckEquipment", target = "owner", resultBind = "equipped" }),
                    Activation("hurt", 1, new { op = "loseHp", target = "selectedTarget", amount = 1 }),
                    Activation("discard-gear", 0, Select("equipment", 1), Move()),
                    Activation("discard-hand-two", 0, Select("hand", 2), Move()) } },
                new { id = Hp, revision = 1, triggers = new[] { Pause("actual-recovery", "afterHpRecovered") } },
                new { id = Phase, revision = 1, triggers = new[] { new { id = "actual-discard-start", window = "discardPhaseStarting", subject = "owner", turnOwnerScope = "own", optional = false,
                    effects = new object[] { new { op = "selectOwnedCards", target = "owner", minimumCards = 2, maximumCards = 2, zones = new[] { "hand", "equipment" }, resultBind = "paid" }, Move() } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实原生装备付款", description = "实际装备失血与弃牌" }, [Hp] = Label("回复孩子"), [Phase] = new { name = "实际弃牌阶段付款", description = "实际选择HE并原生弃置" } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddCard(new("classic:silver-lion", "白银狮子", "装备牌", CardCatalog.Get(CardKind.SilverLion).Description, CardKind.SilverLion));
            b.AddGeneral(new(Owner, "移动界凌统真实场景", "supporter", Skill, "wu", 8, AdditionalSkillIds: phaseDiscard ? [Driver, Hp, Phase] : [Driver, Hp]));
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:boundary-ling-tong-peer-" + i).ToArray();
            foreach (var peer in peers) b.AddGeneral(new(peer, "固定实际角色", "supporter", "standard:none", "wu", 8, AdditionalSkillIds: [Hp]));
            const string deck = "fixture:boundary-ling-tong-mobile-deck";
            b.AddDeck(new(deck, "原生白银狮子实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 120).Select(i => new ContentDeckPhysicalCard("classic:silver-lion", Suit.Club, i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "移动界凌统原生场景", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
        private static object Label(string name) => new { name, description = "原生子窗口", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
    }
}
