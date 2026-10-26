using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileZhouYuChecks
{
    private const string Skill = "mobile:fanjian", Hero = "mobile:zhou-yu", Jueyan = "mou-ol:jueyan";
    private const string Watch = "fixture:mobile-zhou-yu-watch", Drop = "fixture:mobile-zhou-yu-drop-source", Quiet = "fixture:mobile-zhou-yu-quiet";
    private const string Owner = "fixture:mobile-zhou-yu-owner", Recipient = "fixture:mobile-zhou-yu-recipient", Mode = "identity:classic-mobile-zhou-yu";
    private const string GiftReason = "skill-program.mobile:fanjian.transferRandomOwnedCard", DrawReason = "skill-program.mou-ol:jueyan.draw";

    public static void NativeRecipientGainRevealFrozenSuitPaidColdAndSourceLoss()
    {
        var s = Scene.Start(); s.Reach(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        var originalHand = s.V(0).Hand.Select(c => c.Id).ToArray(); var hp = s.V(1).Hp;
        s.Accept(new UseProgramSkillCommand(0, Skill, "guess-suit-and-take-gift", [], [1], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.PlayerSeat == 1);
        s.Need(s.P!.Choices.Select(c => c.Parameters.GetValueOrDefault("option-id")).ToHashSet().SetEquals(["spade", "heart", "club", "diamond"]) &&
            s.V(0).Hand.Select(c => c.Id).SequenceEqual(originalHand) && !s.E<ProgramCardsRevealedEvent>().Any(e => e.SkillId == Skill),
            "The real recipient guesses before an original Hand entity is moved or publicly shown.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "spade");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        var receipt = root.RandomOwnedTransfer!; var card = receipt.CardId; var frameId = root.Id;
        s.Need(receipt is { RecipientSeat: 1, PrintedKind: CardKind.Slash, Stage: RandomOwnedTransferStage.GainChildren, RevealedSuit: null } &&
            originalHand.Contains(card) && s.Zone(card) == CardLocation.Hand(1) && s.V(0).HandCount == originalHand.Length - 1 &&
            !root.CardSetBindings.Any(b => b.Name == "shown") && root.PublicHandRevealSelection is null &&
            !s.E<ProgramCardsRevealedEvent>().Any(e => e.SkillId == Skill) && s.V(1).Hp == hp,
            "The actual gain child owns one frozen random original entity, still private until the recipient really shows it; no reveal or damage was prepaid.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Cards.Contains(card)));
        root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == frameId);
        var shown = s.E<ProgramCardsRevealedEvent>().Single(e => e.FrameId == frameId && e.SkillId == Skill);
        s.Need(root.RandomOwnedTransfer is { Stage: RandomOwnedTransferStage.RevealChildren, RevealedSuit: Suit.Heart } &&
            shown.OwnerSeat == 1 && shown.Cards is [{ Id: var shownId, Suit: Suit.Heart }] && shownId == card &&
            root.CardSetBindings.Single(b => b.Name == "shown").FrozenRevealedSuit == Suit.Heart &&
            s.E<MouXiaoQiaoJueyanSuitClaimedEvent>().Count(e => e.RevealFrameId == frameId && e.Source.OwnerSeat == 1 && e.CardId == card && e.Suit == Suit.Heart) == 1 &&
            s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(1) && m.Reason.Value == DrawReason) == 1 &&
            s.V(1).Hp == hp,
            "Only the real receiver's owned Hand reveal issues its native Jueyan child; Hongyan freezes Heart for the original Spade entity, before any Fanjian damage.");
        s.Answer(c => c.Cards.SequenceEqual([card]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Targets.SequenceEqual([0])));
        s.Answer(c => c.Targets.SequenceEqual([0]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Drop && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill));
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        s.Need(s.Zone(card) == CardLocation.Hand(0) && s.V(0).Skills?.All(k => k.Id != Skill) == true &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == frameId).CardSetBindings.Single(b => b.Name == "shown").FrozenRevealedSuit == Suit.Heart &&
            !s.E<DamageAppliedEvent>().Any(e => e.SourceSeat == 0 && e.TargetSeat == 1),
            "A real reveal reward child returns the original shown entity to its former owner and removes the original skill through shared skill changes; the paid parent's frozen result remains authoritative.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        s.Need(s.V(1).Hp == hp - 1 && s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1) == 1 &&
            s.E<ProgramCardsRevealedEvent>().Count(e => e.FrameId == frameId && e.SkillId == Skill) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing && m.Reason.Value == GiftReason) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Processing && m.To == CardLocation.Hand(1) && m.Reason.Value == GiftReason) == 1 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.SkillId == Skill && e.Completed) == 1 &&
            !s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == frameId),
            "The original Fanjian finishes once after paid cold and source loss: frozen Heart mismatches the earlier Spade guess, so real Damage1 still resolves despite the gift leaving Hand.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand command) { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(r.Accepted, r.Error?.Message ?? "Rejected real Zhou Yu command."); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; var c = p.Choices.FirstOrDefault(predicate); Need(c is not null, "Missing exact native Zhou Yu choice."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 200; i++)
            {
                if (ready()) return;
                if (P is { Kind: DecisionKind.SelectGeneral } select) { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat switch { 0 => Owner, 1 => Recipient, _ => Peer(select.PlayerSeat) }, G.Revision, select.PromptId)); continue; }
                Need(P is null, "Unexpected native Zhou Yu boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Fixed Zhou Yu fixture did not reach its next native boundary.");
        }
        public void Cold()
        {
            string Dump() => string.Join("|", Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i)))) + JsonSerializer.Serialize(G.ResolutionStack) + GameCheckpointJson.Serialize(G.CreateCheckpoint());
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the exact private paid gain, random state, original entity and owning child.");
        }
        public void Need(bool yes, string why)
        { if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.Phase, Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Stack = G.ResolutionStack.Select(f => new { f.Id, f.Kind }) })); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false }, r), r);
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private static string Peer(int seat) => "fixture:mobile-zhou-yu-peer-" + seat;
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-zhou-yu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:zhou-yu", "classic:xiao-qiao", "mou:xiao-qiao-ol", "classic:huang-yueying");
            var definitions = new object[] {
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "normal-draw-minus-two", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false,
                    effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Drop, revision = 1, triggers = new[] { new { id = "shown-gift-return-removes-source", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                    movementOccurrence = "perBatch", movementReasons = new[] { "skill-program.fixture:mobile-zhou-yu-watch.MoveBoundCards" }, optional = false, isSkillInvocation = false, usageScope = "game", usageLimit = 1,
                    effects = new[] { new { op = "replaceSkillsOnAwakening", target = "owner", skillIds = new[] { "classic:jizhi" } } } } } },
                new { id = Watch, revision = 1, triggers = new object[] {
                    new { id = "private-original-gain", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { GiftReason },
                        optional = false, isSkillInvocation = false, usageScope = "game", usageLimit = 1, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "gain", options = new[] { new { id = "continue" } } } } },
                    new { id = "real-reveal-reward", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { DrawReason },
                        optional = false, isSkillInvocation = false, usageScope = "game", usageLimit = 1,
                        effects = new object[] { new { op = "selectOwnedCards", target = "owner", zones = new[] { "hand" }, amount = 1, resultBind = "discard-after-show" },
                            new { op = "selectTarget", target = "owner", targetKind = "currentTurnPlayer" },
                            new { op = "moveBoundCards", target = "owner", sourceBind = "discard-after-show", destination = "selectedTargetHand" },
                            new { op = "chooseOption", target = "owner", resultBind = "revealed", options = new[] { new { id = "continue" } } } } }
                } }
            };
            var labels = new Dictionary<string, object> { [Quiet] = new { name = "原生零张正常摸牌", description = "正常摸牌减2。" },
                [Drop] = new { name = "真实展示后失源", description = "原实体实际交回后，以合法原生觉醒选择移除原反间。" },
                [Watch] = new { name = "原生获得与展示子窗", description = "原赠牌已获得，展示收益后交回原实体。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续原反间" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            // This small source-loss driver meets the existing native awakening prerequisite: three owned skills exceed Lord maxHP2.
            b.AddGeneral(new(Owner, "真实反间与原生失源前置", "supporter", Skill, "wu", 1, [Quiet, Drop], GeneralGender.Male));
            b.AddGeneral(new(Recipient, "真实红颜绝颜接收者", "supporter", Jueyan, "wu", 3, ["classic:hongyan", Quiet, Watch], GeneralGender.Female));
            for (var i = 2; i < 4; i++) b.AddGeneral(new(Peer(i), "真实其他角色", "supporter", Quiet, "wei", 4));
            b.AddDeck(new("fixture:mobile-zhou-yu-deck", "固定真实黑桃杀实体", 3, 2, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, "普通反间受赠后展示", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, "fixture:mobile-zhou-yu-deck", GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Recipient, Peer(2), Peer(3)]));
        }
    }
}
