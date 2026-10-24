using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

namespace CardGame.Core.Tests;

internal static class BoundaryYuJinMobileChecks
{
    private const string Hero = "boundary:yu-jin-mobile", Skill = "boundary:jieyue-mobile", Owner = "fixture:mobile-yj-owner",
        Quiet = "fixture:mobile-yj-quiet", Driver = "fixture:mobile-yj-driver", Gain = "fixture:mobile-yj-gain", Paid = "fixture:mobile-yj-paid",
        Mode = "identity:classic-mobile-yu-jin", GiftReason = "skill-program." + Skill + ".SelectAndMoveOwnedCard", DiscardReason = "skill-program." + Skill + ".MoveBoundCards";

    public static void NativeGiftRecipientBranchesAtomicHeRemainderAndPaidChildrenCold()
    {
        foreach (var (retain, equipment) in new[] { (true, true), (false, true), (true, false) })
        {
            var s = Scene.Start(); s.Play();
            if (equipment) { s.EquipTarget(CardKind.Crossbow); s.EquipTarget(CardKind.DefensiveHorse); }
            var lion = s.V(0).Hand.First(c => c.Kind == CardKind.SilverLion).Id;
            if (equipment)
            {
                s.PlayCard(s.G.GetLegalActions(0).Single(a => a.Kind == LegalActionKind.Equip && a.CardId == lion)); s.Play();
                s.Drive("hurt"); s.Play();
            }
            var gift = equipment ? lion : s.V(0).Hand.First().Id;
            var from = equipment ? CardLocation.Equipment(0) : CardLocation.Hand(0);
            var beforeOwner = s.V(0).HandCount; var beforeTarget = s.V(1).HandCount;
            s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Targets.SequenceEqual([1])));
            s.Answer(c => c.Targets.SequenceEqual([1]));
            s.Reach(() => s.P?.Choices.Any(c => c.Cards.SequenceEqual([gift])) == true); s.Answer(c => c.Cards.SequenceEqual([gift]));
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Gain);
            var original = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
            s.Need(s.P!.PlayerSeat == 1 && original.InstructionIndex == 2 && original.PendingMovementContinuation is not null &&
                s.V(1).Hand.Any(c => c.Id == gift) &&
                s.G.CardMovements.Count(m => m.CardId == gift && m.From == from && m.To == CardLocation.Processing && m.Reason.Value == GiftReason) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == gift && m.From == CardLocation.Processing && m.To == CardLocation.Hand(1) && m.Reason.Value == GiftReason) == 1 &&
                (!equipment || s.V(0).Hp == s.V(0).MaxHp),
                "The original Ending frame really pays one own HE gift and waits for the recipient's real gain child after Silver Lion departure recovery, before any recipient option. " +
                JsonSerializer.Serialize(new { original.InstructionIndex, Waiting = original.PendingMovementContinuation is not null, OwnerHp = s.V(0).Hp, OwnerMaxHp = s.V(0).MaxHp,
                    GiftMovements = s.G.CardMovements.Where(m => m.CardId == gift && m.Reason.Value == GiftReason).Select(m => new { m.From, m.To }), RecipientHasGift = s.V(1).Hand.Any(c => c.Id == gift) }));
            var mother = original.Id; s.Cold(); s.Continue();
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "retain"));
            s.Need(s.P!.PlayerSeat == 1 && s.P.Choices.Count == 2, "The actual recipient, rather than Yu Jin, owns both complete choices after the paid gift returns.");
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == (retain ? "retain" : "draw"));
            if (retain)
            {
                s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "jieyue-kept-hand") == true);
                s.Answer(c => c.Cards.SequenceEqual([gift]));
                int? keptEquipment = null;
                if (equipment)
                {
                    s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "jieyue-kept-equipment") == true);
                    keptEquipment = s.V(1).Equipment.Single(c => c.Kind == CardKind.Crossbow).Id;
                    s.Need(s.P!.PlayerSeat == 1, "Equipment retention also belongs to the actual equipment owner.");
                    s.Answer(c => c.Cards.SequenceEqual([keptEquipment.Value]));
                }
                s.Reach(() => s.P?.SkillPrompt?.SkillId == Paid);
                var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == mother);
                var remainder = root.CardSetBindings.Single(b => b.Name == "jieyue-remainder");
                var moves = s.G.CardMovements.Where(m => m.Reason.Value == DiscardReason).ToArray();
                var batch = s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Last(f => f.Batch.Movements.Any(m => m.Reason.Value == DiscardReason)).Batch;
                s.Need(root.InstructionIndex == 8 && root.PendingMovementContinuation is not null && remainder.SelectionActorSeat == 1 &&
                    s.V(1).Hand.Select(c => c.Id).SequenceEqual([gift]) && s.V(1).Equipment.Count == (equipment ? 1 : 0) &&
                    (keptEquipment is null || s.V(1).Equipment.Single().Id == keptEquipment) && moves.Length == beforeTarget + (equipment ? 1 : 0) &&
                    moves.All(m => m.From.OwnerSeat == 1 && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && m.To == CardLocation.DiscardPile) &&
                    moves.Select(m => m.CardId).Order().SequenceEqual(remainder.CardIds.Order()) && batch.Movements.Where(m => m.Reason.Value == DiscardReason).Count() == moves.Length,
                    "The recipient retains its chosen physical H/E entities and self-discards the complete remaining HE in one real native batch; an absent equipment zone is an empty retention, not a canceled branch.");
                if (equipment) s.Need(moves.Select(m => m.From.Zone).Distinct().Count() == 2, "Hand and equipment remainder share the very same native discard batch.");
                s.Cold(); s.Continue(); s.Done(original);
                s.Need(s.V(0).HandCount == beforeOwner - (equipment ? 0 : 1) && s.E<CardsDrawnEvent>().All(e => e.PlayerSeat != 0 || e.Count != 3),
                    "The retained branch pays only the gift and its remainder, without taking the alternative three-card draw.");
            }
            else
            {
                s.Reach(() => s.P?.SkillPrompt?.SkillId == Gain); s.Need(s.P!.PlayerSeat == 0, "The alternative really draws three into Yu Jin's own hand.");
                s.Cold(); s.FinishDrawChildren(original); s.Done(original);
                s.Need(s.V(0).HandCount == beforeOwner + 3 && s.V(1).HandCount == beforeTarget + 1 && s.V(1).Equipment.Count == 2 &&
                    s.E<CardsDrawnEvent>().Count(e => e.PlayerSeat == 0 && e.Count == 3) == 1 && !s.G.CardMovements.Any(m => m.Reason.Value == DiscardReason),
                    "The actual recipient's draw choice preserves all its cards and draws exactly three for the giver through the original native draw child.");
            }
            s.Need(s.G.CardMovements.Count(m => m.CardId == gift && m.From == from && m.To == CardLocation.Processing && m.Reason.Value == GiftReason) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == gift && m.From == CardLocation.Processing && m.To == CardLocation.Hand(1) && m.Reason.Value == GiftReason) == 1 &&
                s.CompletionCount(original) == 1,
                "Cold native children return to the same paid Ending frame and complete once without a second gift, discard or draw.");
        }
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public int CompletionCount(ProgramSkillFrame frame) => E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == frame.Id &&
            e.SkillId == frame.SkillId && e.BindingId == frame.TriggerId && e.SkillInstanceId == frame.SkillInstanceId && e.OwnerSeat == frame.OwnerSeat &&
            e.Window == SkillProgramTriggerWindow.TurnEnding && e.Activated && e.Completed);
        public void Done(ProgramSkillFrame frame)
        {
            Reach(() => G.ResolutionStack.All(f => f.Id != frame.Id));
            Need(CompletionCount(frame) == 1, "The exact paid Ending binding must complete once. " + JsonSerializer.Serialize(new {
                frame.Id, frame.TriggerId, frame.SkillInstanceId, Parent = frame.WindowContext!.ParentFrameId,
                Bindings = E<ProgramBindingResolvedEvent>().Where(e => e.SkillId == Skill),
                Activations = E<ProgramSkillResolvedEvent>().Where(e => e.SkillId == Skill) }));
        }
        public void FinishDrawChildren(ProgramSkillFrame frame)
        {
            for (var childCount = 0; childCount < 4; childCount++)
            {
                Reach(() => P?.SkillPrompt?.SkillId == Gain || G.ResolutionStack.All(f => f.Id != frame.Id));
                if (G.ResolutionStack.All(f => f.Id != frame.Id)) return;
                var root = G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == frame.Id);
                var child = G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Last();
                Need(P!.PlayerSeat == 0 && root.InstructionIndex is 4 or 5 && child.Batch.ParentFrameId == frame.Id &&
                    (root.NativeDraw is null || root.NativeDraw is { InstructionIndex: 4, RequestedCount: 3, RecipientApplied: true }) &&
                    child.Batch.Movements.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program." + Skill + ".Draw"),
                    "Each remaining native per-batch gain child belongs to the same original Draw3, not a repeated gift or draw instruction.");
                Continue();
            }
            throw new InvalidOperationException("The one native Draw3 issued too many movement children.");
        }
        public void Drive(string id) => Accept(new UseProgramSkillCommand(0, Driver, id, [], [], G.Revision, P!.PromptId));
        public void EquipTarget(CardKind kind)
        {
            var card = V(0).Hand.First(c => c.Kind == kind).Id;
            Accept(new UseProgramSkillCommand(0, Driver, "equip-target", [], [1], G.Revision, P!.PromptId));
            Reach(() => P?.Choices.Any(c => c.Cards.SequenceEqual([card])) == true); Answer(c => c.Cards.SequenceEqual([card])); Play();
        }
        public void PlayCard(LegalAction a) => Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind));
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected native Jieyue command."); }
        public void Reach(Func<bool> goal)
        {
            for (var step = 0; step < 160; step++)
            {
                if (goal()) return;
                Need(P is null, "Unexpected human boundary interrupted the mobile Jieyue scene."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The native Jieyue fixture exceeded its step bound.");
        }
        public void Cold()
        {
            var json = GameCheckpointJson.Serialize(G.CreateCheckpoint()); var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(json), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == json && G.CardMovements.SequenceEqual(restored.CardMovements) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack),
                "Cold preserves the original paid gift, recipient retention source bindings and atomic native discard/draw child."); G = restored;
        }
        public void Need(bool yes, string why)
        {
            if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, G.State.Phase,
                Pending = P?.Kind, Seat = P?.PlayerSeat, Skill = P?.SkillPrompt?.SkillId, Stack = G.ResolutionStack.Select(f => f.Kind) }));
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4 }, registry);
            var s = new Scene(game, registry); s.Accept(new StartGameCommand());
            for (var step = 0; step < 80; step++)
            {
                if (s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return s;
                if (s.P is { Kind: DecisionKind.SelectGeneral } p) s.Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : p.ValidContentIds.First(id => id != Owner), game.Revision, p.PromptId));
                else if (s.P is not null) throw new InvalidOperationException("Unexpected Jieyue setup prompt " + s.P.Kind);
                else s.Accept(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("The fixed identity fixture did not reach its first actual own Play.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-boundary-yu-jin", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            b.AddCard(new("fixture:mobile-yj-silver-lion", "白银狮子", "装备牌", CardCatalog.Get(CardKind.SilverLion).Description, CardKind.SilverLion));
            object[] Pause(string bind) => [new { op = "chooseOption", target = "owner", resultBind = bind, options = new[] { new { id = "continue" } } }];
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[] {
                new { id = Quiet, revision = 1, modifiers = new[] { new { id = "preserve-original-hand", query = "handLimit", operation = "add", value = 20, priority = 0 } } },
                new { id = Driver, revision = 1, activations = new object[] {
                    new { id = "hurt", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "damage", target = "owner", amount = 1 } } },
                    new { id = "equip-target", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "selectAndMoveOwnedCard", target = "owner", chooserRef = new { kind = "owner" }, cardOwnerRef = new { kind = "owner" }, targetRef = new { kind = "selectedTarget" }, zones = new[] { "hand" }, cardCategories = new[] { "equipment" }, count = 1, destination = "selectedTargetEquipment" } } }
                } },
                new { id = Gain, revision = 1, triggers = new[] { new { id = "gain", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { GiftReason, "skill-program." + Skill + ".Draw" },
                    optional = false, isSkillInvocation = false, effects = Pause("gained") } } },
                new { id = Paid, revision = 1, triggers = new[] { new { id = "paid", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand", "equipment" }, movementOccurrence = "perOwnerBatch", movementReasons = new[] { DiscardReason },
                    optional = false, isSkillInvocation = false, effects = Pause("paid") } } }
            } });
            var names = new Dictionary<string, object> { [Quiet] = new { name = "保留原手牌", description = "避免无关弃牌。" }, [Driver] = new { name = "真实装备与伤害", description = "安装真实装备并制造狮子回复条件。" } };
            foreach (var id in new[] { Gain, Paid }) names[id] = new { name = id, description = "真实获得或整批自弃子窗。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = p, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Owner, "移动版界于禁原生来源", "", Skill, "wei", 8, [Quiet, Driver, Gain, Paid]));
            var pool = new List<string> { Owner };
            for (var i = 1; i <= 3; i++) { var id = "fixture:mobile-yj-peer-" + i; pool.Add(id); b.AddGeneral(new(id, "真实收牌者", "", Quiet, "wei", 8, [Gain, Paid])); }
            const string deck = "fixture:mobile-yj-deck";
            var kinds = new[] { "fixture:mobile-yj-silver-lion", "standard:crossbow", "standard:defensive_horse", "standard:dodge" };
            b.AddDeck(new(deck, "固定真实手牌与装备", 12, 0, []) { PhysicalCards = Enumerable.Range(0, 128).Select(i => new ContentDeckPhysicalCard(kinds[i % kinds.Length], Suit.Club, i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "真实四人身份节钺", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, deck, GeneralCandidateCount: 4, GeneralPoolIds: pool));
        }
    }
}
