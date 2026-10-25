using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileWangYiChecks
{
    private const string Zhenlie = "mobile:zhenlie", Miji = "mobile:miji", Owner = "fixture:mobile-wang-yi-owner",
        Prime = "fixture:mobile-wang-yi-prime", Watch = "fixture:mobile-wang-yi-watch", Mode = "identity:classic-mobile-wang-yi";
    private const string GiftReason = "skill-program.mobile:miji.MoveBoundCards";

    public static void UnknownTopRetrialAndWholePrivateGiftReturnCold()
    {
        var s = Scene.Start(Suit.Spade); s.Offer();
        var ownHand = s.V(0).HandCount; var recipientHand = s.V(1).HandCount;
        var lost = s.V(0).MaxHp - s.V(0).Hp;
        s.Need(lost == 2, "The real opening HP-loss instruction wounds the actual owner before preparation.");
        s.Activate(); s.Retrial();
        var judgment = s.G.ResolutionStack.OfType<JudgmentFrame>().Single();
        var oldId = judgment.CardId!.Value;
        var topId = s.G.CreateCardZoneDiagnostics().Where(c => c.Location == CardLocation.DrawPile).OrderBy(c => c.ZoneIndex).Last().CardId;
        s.Need(s.P is { ValidCardIds.Count: 0, ValidTargetSeats.Count: 0, Choices.Count: 2 } &&
            s.P.Choices.All(c => c.Cards.Count == 0 && c.Targets.Count == 0) &&
            !s.G.CardMovements.Any(m => m.CardId == topId && m.Reason == CardMoveReasons.ProgramJudgmentReplace),
            "The actual own-judgment retrial offers only accept/skip, without a future material ID or an early top-card payment.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("action") == "program-judgment-top-replace"); s.Watch("replacement-payment", 0);
        var retrial = s.Root(Zhenlie); var retrialId = retrial.Id;
        s.Need(retrial.InstructionIndex == 1 && retrial.WindowContext?.JudgmentReplacement is
                { JudgmentFrameId: var parent, OldCardId: var old, ReplacementCardId: var paid, ReplacementSuit: Suit.Spade } &&
            parent == judgment.Id && old == oldId && paid == topId &&
            s.Location(oldId) == CardLocation.DiscardPile && s.Location(topId) == CardLocation.Judgment(0) &&
            s.G.CardMovements.Count(m => m.CardId == topId && m.Reason == CardMoveReasons.ProgramJudgmentReplace) == 2 &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(w => w.Batch.ParentFrameId == retrialId &&
                w.Batch.OriginSkillId == Zhenlie && w.Batch.Movements.Any(m => m.CardId == topId && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing)),
            "The original native replacement frame freezes and pays the real unknown top entity before its actual old-judgment movement child.");
        s.Cold(); s.Continue(); s.Target();
        var preparation = s.Root(Miji); var preparationId = preparation.Id;
        var cards = preparation.CardSetBindings.Single(b => b.Name == "miji-cards"); var firstIds = cards.CardIds.ToArray();
        s.Need(preparation.TriggerId == "prepare-black-top-gift" && preparation.InstructionIndex == 4 &&
            preparation.JudgmentOutcomeBindings!.Single().CardId == topId && cards.Visibility == SkillProgramCardSetVisibility.Private &&
            firstIds.Length == lost && firstIds.All(id => s.Location(id) == CardLocation.Processing) &&
            (s.G.CreateSnapshot(0).PrivateRevealedCards ?? []).Select(c => c.Id).SequenceEqual(firstIds) &&
            Enumerable.Range(1, 4).All(seat => (s.G.CreateSnapshot(seat).PrivateRevealedCards ?? []).Count == 0) &&
            s.P!.ValidTargetSeats.Contains(0) && s.P.ValidTargetSeats.Contains(1) &&
            s.V(0).HandCount == ownHand && s.V(1).HandCount == recipientHand,
            "Preparation uses the frozen final black result, privately shows exactly lost-HP top entities, and offers one whole-set recipient including self without first drawing them.");
        s.Answer(c => c.Targets.SequenceEqual([1])); s.Watch("gift-gain", 1);
        s.Need(s.Root(Miji).Id == preparationId && s.Root(Miji).InstructionIndex == 5 &&
            s.V(1).HandCount == recipientHand + lost && s.V(0).HandCount == ownHand &&
            firstIds.All(id => s.Location(id) == CardLocation.Hand(1)) &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(w => w.Batch.ParentFrameId == preparationId &&
                w.Batch.OriginSkillId == Miji && w.Batch.Movements.Select(m => m.CardId).SequenceEqual(firstIds) &&
                w.Batch.Movements.All(m => m.From == CardLocation.Processing && m.To == CardLocation.Hand(1) && m.Reason.Value == GiftReason)),
            "All viewed cards enter one actual recipient hand in one native batch before the original Miji's gain child returns.");
        s.Cold(); s.Continue(); s.Play(0); s.End(0); s.Offer(); s.Activate(); s.Retrial();
        s.Answer(c => c.Parameters.GetValueOrDefault("action") == "program-judgment-replace-skip"); s.Target();
        var ending = s.Root(Miji); var endingId = ending.Id;
        var secondIds = ending.CardSetBindings.Single(b => b.Name == "miji-cards").CardIds.ToArray(); var beforeSelf = s.V(0).HandCount;
        s.Need(ending.TriggerId == "ending-black-top-gift" && secondIds.Length == lost && endingId != preparationId,
            "The actual ending phase independently offers Miji after preparation has finished, with a separate original binding and fresh top entities.");
        s.Answer(c => c.Targets.SequenceEqual([0])); s.Watch("gift-gain", 0);
        s.Need(s.V(0).HandCount == beforeSelf + lost && secondIds.All(id => s.Location(id) == CardLocation.Hand(0)),
            "Choosing oneself receives the entire new top set through the same real hand-gain path.");
        s.Continue(); s.Play(1);
        s.Need(s.Facts<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Miji && e.Activated && e.Completed) == 2 &&
            s.Facts<ProgramJudgmentReplacementResolvedEvent>().Count(e => e.SkillId == Zhenlie && e.Activated && e.ReplacementCardId == topId) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == topId && m.Reason == CardMoveReasons.ProgramJudgmentReplace) == 2 &&
            firstIds.Concat(secondIds).Distinct().Count() == lost * 2 &&
            firstIds.Concat(secondIds).All(id => s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Processing &&
                m.To.Zone == CardZoneKind.Hand && m.Reason.Value == GiftReason) == 1) &&
            !s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId is Zhenlie or Miji),
            "Both natural phase bindings complete once; cold native child returns never replace, reveal or give an already-paid entity twice.");

        var red = Scene.Start(Suit.Heart); red.Offer(); red.Activate(); red.Retrial();
        red.Answer(c => c.Parameters.GetValueOrDefault("action") == "program-judgment-replace-skip"); red.Play(0);
        red.Need(red.Facts<ProgramJudgmentOutcomeRecordedEvent>().Single(e => e.Outcome.Reason == "skill.mobile-wang-yi.miji").Outcome.Suit == Suit.Heart &&
            red.Facts<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Miji && e.Activated && e.Completed) == 1 &&
            !red.G.CardMovements.Any(m => m.Reason.Value == GiftReason && m.To.Zone == CardZoneKind.Hand) &&
            !red.G.CardMovements.Any(m => m.Reason.Value == "skill-program.mobile:miji.prepare-black-top-gift.reveal"),
            "A red finalized preparation judgment skips all three black-guard instructions without requesting a recipient or moving hidden top cards.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public CardLocation Location(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public IEnumerable<T> Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public ProgramSkillFrame Root(string skill) => G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == skill);
        public void Need(bool ok, string message)
        { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null })); }
        public void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected native Wangyi command."); }
        public void Answer(Func<PromptChoice, bool> pick)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(pick).Id, G.Revision)); }
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Offer() => Reach(() => P?.SkillPrompt?.SkillId == Miji && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Retrial() => Reach(() => P?.Kind == DecisionKind.ProgramJudgmentReplacement);
        public void Target() => Reach(() => P?.SkillPrompt?.SkillId == Miji && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
        public void Watch(string trigger, int seat) => Reach(() => P?.SkillPrompt?.SkillId == MobileWangYiChecks.Watch && P.PlayerSeat == seat &&
            G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == MobileWangYiChecks.Watch && f.TriggerId == trigger));
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision, P!.PromptId));
        private void Reach(Func<bool> goal)
        {
            for (var step = 0; step < 180 && !goal(); step++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p)
                { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : "fixture:mobile-wang-yi-peer-" + p.PlayerSeat, G.Revision, p.PromptId)); continue; }
                if (P is { } unexpected) throw new InvalidOperationException("Unexpected Wangyi boundary: " + JsonSerializer.Serialize(new { unexpected.Kind, unexpected.PlayerSeat, unexpected.SkillPrompt, unexpected.Choices }));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(goal(), "Missing bounded real mobile Wangyi boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold JSON restores the unknown top-card choice, original paid native frame, real movement child and every private view exactly.");
        }
        public static Scene Start(Suit suit)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(suit));
            var game = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture(Suit suit) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-wang-yi", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:wang-yi");
            object Choice() => new { op = "chooseOption", target = "owner", resultBind = "seen", options = new[] { new { id = "continue" } } };
            object[] definitions = [
                new { id = Prime, revision = 1, triggers = new[] { new { id = "opening-wound", window = "gameStarting", subject = "owner", optional = false,
                    effects = new[] { new { op = "loseHp", target = "owner", amount = 2 } } } } },
                new { id = MobileWangYiChecks.Watch, revision = 1, triggers = new object[] {
                    new { id = "replacement-payment", window = "cardsMoved", subject = "owner", sourceZones = new[] { "judgment" }, movementOccurrence = "perBatch",
                        movementReasons = new[] { CardMoveReasons.ProgramJudgmentOldCard.Value }, optional = false, effects = new[] { Choice() } },
                    new { id = "gift-gain", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch",
                        movementReasons = new[] { GiftReason }, optional = false, effects = new[] { Choice() } }
                } }
            ];
            var labels = new Dictionary<string, object> { [Prime] = new { name = "实际开局受伤", description = "原生失去两点体力。" },
                [MobileWangYiChecks.Watch] = new { name = "已付真实移动孩子", description = "原生改判或秘计移牌后暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Owner, "普通移动王异原生技能", "supporter", Zhenlie, "wei", 3, [Miji, Prime, MobileWangYiChecks.Watch]));
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:mobile-wang-yi-peer-" + i).ToArray();
            foreach (var peer in peers) b.AddGeneral(new(peer, "真实收牌角色", "supporter", "standard:none", "wu", 4, [MobileWangYiChecks.Watch]));
            const string deck = "fixture:mobile-wang-yi-deck";
            b.AddDeck(new(deck, "固定同色真实实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 80).Select(i => new ContentDeckPhysicalCard("standard:dodge", suit, i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "秘计真实准备与结束", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1,
                [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
