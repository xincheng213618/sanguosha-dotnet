using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryWuGuoTaiMobileChecks
{
    private const string Hero = "boundary:wu-guo-tai-mobile", Ganlu = "boundary:ganlu-mobile", Buyi = "boundary:buyi-mobile", Owner = "fixture:mobile-wu-guo-tai-owner",
        Driver = "fixture:mobile-wu-guo-tai-driver", Watch = "fixture:mobile-wu-guo-tai-discard-child", Hp = "fixture:mobile-wu-guo-tai-hp-child", Mode = "identity:classic-mobile-wu-guo-tai";
    private const string ExchangeReason = "skill-program." + Ganlu + ".equipment-exchange", BuyiDiscard = "skill-program." + Buyi + ".MoveBoundCards";

    public static void SelfPairBypassesDifferenceAndNativeExchangeReturnsCold()
    {
        var s = Scene.Start(); s.Play(); s.Use("equip", 1); s.Play();
        var lion = s.V(1).Equipment.Single(c => c.Kind == CardKind.SilverLion).Id;
        s.Need(s.V(0).Hp == s.V(0).MaxHp && s.V(0).Equipment.Count == 0 && s.V(1).Equipment.Count == 1,
            "The fixed native producers establish a one-equipment difference at actual lost HP zero.");
        var action = s.G.GetLegalActions(0).Single(a => a.ProgramSkillId == Ganlu);
        s.Need(action.SelectableTargetSequences!.Any(p => p.SequenceEqual(new[] { 0, 1 })) &&
            action.SelectableTargetSequences.All(p => !p.SequenceEqual(new[] { 1, 2 })) &&
            action.SelectableTargetSequences.Any(p => p.SequenceEqual(new[] { 2, 3 })),
            "Including Wu Guo Tai bypasses the difference; a foreign unequal pair still fails, and the mobile text adds no classic total-equipment minimum.");
        s.Use("hurt", 1); s.Play(); s.Ganlu(0, 1); s.Reach(() => s.P?.SkillPrompt?.SkillId == Hp);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Ganlu); var id = root.Id;
        s.Need(root.SelectedTargetSeats.SequenceEqual(new[] { 0, 1 }) && root.EquipmentPairPayment is null && root.PendingMovementContinuation is not null &&
            s.V(0).Equipment.Any(c => c.Id == lion) && s.V(1).Equipment.Count == 0 && s.V(1).Hp == s.V(1).MaxHp &&
            s.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Any(f => f.ResumeFrameId == id),
            "The mobile exchange has no OL discard cost and moves the real equipment atomically before its native removal recovery child returns to the original frame.");
        s.Cold(); s.Answer(_ => true); s.Play();
        s.Need(s.G.CardMovements.Count(m => m.CardId == lion && m.From == CardLocation.Equipment(1) && m.To == CardLocation.Equipment(0) && m.Reason.Value == ExchangeReason) == 1,
            "Native recovery cold-return exchanges the original physical equipment exactly once.");
        s.Need(s.G.GetLegalActions(0).All(a => a.ProgramSkillId != Ganlu),
            "The original Ganlu activation remains used in its actual Play phase after recovery and cold-return.");
        s.Need(s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == id && e.SkillId == Ganlu && e.Completed) == 1 &&
            s.G.ResolutionStack.All(f => f.Id != id),
            "The original active-skill frame emits its native successful completion exactly once and leaves the stack after its child returns.");

        var empty = Scene.Start(); empty.Play(); empty.Ganlu(2, 3); empty.Play();
        empty.Need(empty.G.CardMovements.All(m => m.Reason.Value != ExchangeReason) && empty.G.GetLegalActions(0).All(a => a.ProgramSkillId != Ganlu),
            "An allowed empty pair completes as a zero-entity exchange, consumes the original phase use and never constructs a fake movement batch.");
    }

    public static void BuyiDyingHolderPerformsRevealedHandDiscardBeforeRecoveryCold()
    {
        var s = Scene.Start(); s.Play(); var initialHand = s.V(1).HandCount; s.Use("dying", 1);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Buyi && s.Action("activate")); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Buyi && s.Action("select-target"));
        s.Answer(c => c.Targets.SequenceEqual([1])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Buyi); var frameId = root.Id;
        var revealed = root.CardSetBindings.Single(b => b.Name == "revealed").CardIds.Single();
        var cost = root.CardSetBindings.Single(b => b.Name == "nonbasic");
        var discard = s.E<NativeLogicalDiscardInvocationRecordedEvent>().Single(e => e.Reason.Value == BuyiDiscard);
        s.Need(cost.SelectionActorSeat == 1 && cost.CardIds.SequenceEqual(new[] { revealed }) && cost.SourceLocations.SequenceEqual(new[] { CardLocation.Hand(1) }) &&
            discard.InitiatorSeat == 0 && discard.ExecutorSeat == 1 && discard.SubjectSeat == 1 && discard.ActualCount == 1 && discard.ParentFrameId == frameId &&
            s.V(1).Hp == 0 && s.V(1).HandCount == initialHand - 1 && root.PendingMovementContinuation is not null &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(f => f.Batch.Movements.Any(m => m.CardId == revealed && m.Reason.Value == BuyiDiscard)),
            "The exact publicly revealed nonbasic hand entity is discarded by the dying holder, with the original Wu Guo Tai initiator; its native movement child pauses before Recover1.");
        s.Cold(); s.Answer(_ => true); s.Reach(() => s.P?.SkillPrompt?.SkillId == Hp);
        s.Need(s.V(1).Hp == 1 && s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Buyi).Id == frameId,
            "Only after the actual self-discard child does the same original Buyi frame issue its native recovery.");
        s.Answer(_ => true); s.Play();
        s.Need(s.V(1).IsAlive && s.V(1).Hp == 1 && s.G.CardMovements.Count(m => m.CardId == revealed && m.Reason.Value == BuyiDiscard) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Buyi && e.Completed && e.Activated) == 1,
            "The real dying stack resumes after recovery, with no repeated revealed-card payment or repeated rescue after cold restore.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public bool Action(string value) => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == value) == true;
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string id, params int[] targets) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId));
        public void Ganlu(int first, int second) => Accept(new UseProgramSkillCommand(0, BoundaryWuGuoTaiMobileChecks.Ganlu, "exchange-equipment", [], [first, second], G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Wu Guo Tai command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 180 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p) { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : "fixture:mobile-wu-guo-tai-peer-" + p.PlayerSeat, G.Revision, p.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } foreign) { Accept(new EndPlayPhaseCommand(foreign.PlayerSeat, G.Revision, foreign.PromptId)); continue; }
                if (P?.SkillPrompt?.SkillId is Hp or Watch) { Answer(_ => true); continue; }
                if (P is { Kind: DecisionKind.ProgramTrigger }) throw new InvalidOperationException("Unexpected mobile Wu Guo Tai native boundary: " + Short());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "Bounded mobile Wu Guo Tai scene missed its expected boundary.");
        }
        public void Cold()
        { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves original native equipment movement, the dying holder's discard identity and exact typed child return."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        private string Short() => JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Players = Enumerable.Range(0, 5).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount, Equipment = V(i).Equipment.Count }) });
        public void Need(bool condition, string text) { if (!condition) throw new InvalidOperationException(text + " " + Short()); }
        public static Scene Start()
        { var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture()); var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, r)); s.Accept(new StartGameCommand()); return s; }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-wu-guo-tai-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            // Enable the existing authoritative discard ledger without giving
            // another character's skill to any fixture participant.
            CharacterFixtureContent.Register(b, "boundary:ling-tong-mobile");
            object Activation(string id, int amount) => new { id, minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "anyLiving", usesPerTurn = (int?)null,
                effects = new object[] { id == "equip" ? new { op = "useRandomDeckEquipment", target = "owner", resultBind = "equipped" } : (object)new { op = "loseHp", target = "selectedTarget", amount } } };
            object Pause() => new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } };
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] { Activation("equip", 0), Activation("hurt", 1), Activation("dying", 3) } },
                new { id = Hp, revision = 1, triggers = new[] { new { id = "recovered", window = "afterHpRecovered", subject = "owner", optional = false, effects = new[] { Pause() } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "self-discarded", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" }, movementReasons = new[] { BuyiDiscard }, movementOccurrence = "perOwnerBatch", optional = false, effects = new[] { Pause() } } } }
            };
            object Label(string name) => new { name, description = "真实原生孩子", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "原生装备与失血", description = "真实装备与濒死路径" }, [Hp] = Label("原生回复"), [Watch] = Label("实际自己弃置后") };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddCard(new("classic:silver-lion", "白银狮子", "装备牌", CardCatalog.Get(CardKind.SilverLion).Description, CardKind.SilverLion));
            b.AddGeneral(new(Owner, "移动界吴国太真实场景", "supporter", Ganlu, "wu", 3, AdditionalSkillIds: [Buyi, Driver, Hp], Gender: GeneralGender.Female));
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:mobile-wu-guo-tai-peer-" + i).ToArray();
            foreach (var peer in peers) b.AddGeneral(new(peer, "固定真实角色", "supporter", "standard:none", "wu", 3, AdditionalSkillIds: [Watch, Hp]));
            const string deck = "fixture:mobile-wu-guo-tai-deck";
            b.AddDeck(new(deck, "真实装备实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 100).Select(i => new ContentDeckPhysicalCard("classic:silver-lion", Suit.Club, i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "移动界吴国太原生流程", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
