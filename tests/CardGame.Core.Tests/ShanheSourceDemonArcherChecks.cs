using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceDemonArcherChecks
{
    private const string Youyan = StandardShanheSourceDemonArcherPackage.YouyanSkillId,
        Saogu = StandardShanheSourceDemonArcherPackage.SaoguSkillId,
        Watch = "fixture:source-demon-archer-child", ExtraWatch = "fixture:source-demon-archer-extra-child", Deck = "fixture:source-demon-archer-deck";
    private const string GainReason = "skill-program.ol:youyan.youyan-gain", FeeReason = "skill-program.duan-jiong.saogu.discard";
    private const string ExtraReason = "skill-program." + Watch + ".MoveBoundCards";

    public static void GongqiAndActualDiscardTriggerYouyanButSaoguEndingReturnsWithoutIt()
    {
        var s = Scene.Start();
        var initial = s.V.Hand.ToArray();
        var cost = initial.Single(c => c.Suit == Suit.Heart).Id;
        var firstDiscard = initial.Where(c => c.Suit is Suit.Spade or Suit.Club).Select(c => c.Id).ToArray();
        s.Accept(new UseProgramSkillCommand(0, StandardShanheSourceDemonArcherPackage.GongqiSkillId,
            "discard-for-unlimited-range", [cost], [], s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Youyan);
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        var play = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Youyan);
        Need(play.WindowContext?.MovementBatch?.DiscardRecoveryPhase is { ActorSeat: 0, Kind: ActualDiscardRecoveryPhaseKind.Play } &&
            s.Events<ProgramYouyanGainEvent>() is [{ GainedCount: 3 }] && s.V.HandCount == 6 &&
            s.G.CardMovements.Count(m => m.CardId == cost && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 1 &&
            s.G.CardMovements.Count(m => m.Reason.Value == GainReason && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0)) == 3,
            "The source archer's actual Gongqi hand payment triggers Youyan in its real own Play: the original heart is discarded once and three other-suit deck entities arrive before the same gain child returns.");
        s.Continue(); s.Play();
        Need(s.Events<ProgramBindingResolvedEvent>().Count(e => e.FrameId == play.Id && e.Completed) == 1 &&
            !s.G.ResolutionStack.Any(f => f.Id == play.Id),
            "The original Play Youyan completes once and returns through its actual Gongqi payment parent.");

        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P is { Kind: DecisionKind.DiscardCards });
        Need(s.G.State.Phase == TurnPhase.Discard && s.P!.RequiredCardCount == 2,
            "The three gained cards create a real two-card hand-limit obligation in the subsequent actual discard phase.");
        s.Accept(new DiscardCardsCommand(0, firstDiscard, s.P.PromptId, s.G.Revision));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Youyan);
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        var discard = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Youyan);
        var phases = new[] { play.WindowContext!.MovementBatch!.DiscardRecoveryPhase!, discard.WindowContext!.MovementBatch!.DiscardRecoveryPhase! };
        Need(discard.Id != play.Id && discard.WindowContext?.MovementBatch?.DiscardRecoveryPhase is
                { ActorSeat: 0, Kind: ActualDiscardRecoveryPhaseKind.Discard } &&
            s.Events<ProgramYouyanGainEvent>().Select(e => e.GainedCount).SequenceEqual([3, 2]) && s.V.HandCount == 6 &&
            phases[0].Token != phases[1].Token && s.Events<SkillUsageConsumedEvent>().Where(e => e.SkillId == Youyan)
                .Select(e => (e.SkillOwnerSeat, e.UsageId, e.Scope, e.Count)).SequenceEqual(phases.Select(p =>
                    (0, "youyan-phase:" + p.Token.ToString(System.Globalization.CultureInfo.InvariantCulture), SkillUsageScope.Turn, 1))) &&
            firstDiscard.All(id => s.G.CardMovements.Count(m => m.CardId == id && m.Reason == CardMoveReasons.HandLimitDiscard &&
                m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 1),
            "The next actual Discard has its separate once-per-phase opportunity: its two original discarded suits exclude both suits from a two-entity gain.");
        var extra = s.V.Hand.First().Id;
        var watch = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Watch);
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "discard-extra");
        s.Until(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        s.Answer(c => c.Cards.SequenceEqual([extra]));
        s.Until(() => s.P?.SkillPrompt?.SkillId == ExtraWatch);
        var extraWindow = s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(f => f.Batch.Movements.Any(m =>
            m.CardId == extra && m.Reason.Value == ExtraReason));
        Need(extraWindow.Batch.DiscardRecoveryPhase == phases[1] && extraWindow.Batch.ParentFrameId == watch.Id &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == watch.Id) &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == discard.Id) &&
            extraWindow.Batch.Movements.Count(m => m.CardId == extra && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 1 &&
            s.Events<ProgramYouyanGainEvent>().Length == 2 && s.Events<SkillUsageConsumedEvent>().Count(e => e.SkillId == Youyan) == 2 &&
            s.Events<ProgramBindingStartedEvent>().Count(e => e.SkillId == Youyan) == 2,
            "The extra original card is really discarded within the same recorded Discard token, holding the original gain Watch and Youyan parents without offering or consuming Youyan again. " + ExtraDiagnostic());
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Until(() => s.P?.SkillPrompt?.SkillId == Saogu && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        Need(s.Events<ProgramYouyanGainEvent>().Length == 2 && s.V.HandCount == 5 &&
            s.Events<ProgramBindingStartedEvent>().Count(e => e.SkillId == Youyan) == 2 &&
            s.Events<ProgramBindingResolvedEvent>().Count(e => e.FrameId == discard.Id && e.Completed) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == extra && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 1 &&
            s.G.CardMovements.Count(m => m.Reason == CardMoveReasons.HandLimitDiscard && m.From == CardLocation.Hand(0)) == 2,
            "The actual gain child makes one further genuine discard inside the same Discard token, without reopening Youyan's spent opportunity. It returns to the original once-paid hand-limit obligation; the post-discard gains remain in hand before Ending. " + ExtraDiagnostic());
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Until(() => s.P?.SkillPrompt?.SkillId == Saogu && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "delegate"));
        var parent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Saogu);
        var fee = s.P!.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "delegate" && c.Targets.SequenceEqual([1])).Cards.Single();
        s.Answer(c => c.Cards.SequenceEqual([fee]) && c.Targets.SequenceEqual([1]));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        var feeWindow = s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(f => f.Batch.ParentFrameId == parent.Id);
        Need(parent.WindowContext?.Window == SkillProgramTriggerWindow.TurnEnding &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == parent.Id && f.DuanJiongSaogu is { Stage: DuanJiongSaoguStage.DelegateChildren }) &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(f => f.Batch.ParentFrameId == parent.Id &&
                f.Batch.DiscardRecoveryPhase is { ActorSeat: 0, Kind: ActualDiscardRecoveryPhaseKind.Ending }) &&
            s.G.CardMovements.Count(m => m.CardId == fee && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == FeeReason) == 1 &&
            s.Events<DuanJiongSaoguEntityPaidEvent>().Count(e => e.FrameId == parent.Id && e.DelegateFee && e.CardId == fee) == 1 &&
            feeWindow.Batch.DiscardRecoveryPhase is { } ending && phases.All(p => p.Token != ending.Token) &&
            s.Events<ProgramBindingStartedEvent>().Count(e => e.SkillId == Youyan) == 2,
            "Saogu's actual Ending fee really leaves the source hand and holds its original paid parent, but that Ending discard never creates a third Youyan opportunity.");
        s.Continue();
        s.Until(() => s.Events<TurnEndedEvent>().Any(e => e.ActorSeat == 0));
        Need(s.Events<ProgramYouyanGainEvent>().Length == 2 &&
            s.Events<ProgramBindingStartedEvent>().Count(e => e.SkillId == Youyan) == 2 &&
            s.Events<DuanJiongSaoguCompletedEvent>().Count(e => e.FrameId == parent.Id && e.Ending) == 1 &&
            s.Events<ProgramBindingResolvedEvent>().Count(e => e.FrameId == parent.Id && e.Completed) == 1 &&
            !s.G.ResolutionStack.Any(f => f.Id == parent.Id) && s.V.HandCount == 4,
            "The real fee child returns to the same Ending exactly once. No out-of-phase Youyan gain or duplicate payment is inserted before the actual turn ends.");

        string ExtraDiagnostic() => JsonSerializer.Serialize(new { s.V.HandCount, ExtraCard = extra, DiscardToken = phases[1].Token,
            ExtraBatchPhase = extraWindow.Batch.DiscardRecoveryPhase,
            Starts = s.Events<ProgramBindingStartedEvent>().Where(e => e.SkillId == Youyan),
            Returns = s.Events<ProgramBindingResolvedEvent>().Where(e => e.SkillId == Youyan),
            HandLimitMoves = s.G.CardMovements.Count(m => m.Reason == CardMoveReasons.HandLimitDiscard && m.From == CardLocation.Hand(0)),
            ExtraMoves = s.G.CardMovements.Where(m => m.CardId == extra) });
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand c) { var result = G.Submit(c); Need(result.Accepted, (result.Error?.Message ?? "Rejected source archer command.") + " " + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> choose)
        { var p = P!; var c = p.Choices.FirstOrDefault(choose); Need(c is not null, "Missing actual source archer choice. " + Diagnostic()); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> stop)
        {
            for (var step = 0; step < 220; step++)
            {
                if (stop()) return;
                Need(G.State.Winner == Winner.None && G.State.TurnNumber <= 1, "Source archer fixture remains inside one real turn. " + Diagnostic());
                if (P?.SkillPrompt?.SkillId == Watch) { Continue(); continue; }
                if (P is not null) throw new InvalidOperationException("Unexpected source archer prompt. " + Diagnostic());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Source archer exceeded its bounded actual flow. " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new { G.State.Phase, G.State.TurnNumber, Pending = P,
            Stack = G.ResolutionStack.Select(f => new { Type = f.GetType().Name, f.Id }), Recent = G.Events.TakeLast(8).Select(e => e.Payload.ToString()) });
        public static Scene Start()
        {
            var setup = new ShanheBattleSetup(283, 1, "fixture:source-demon-archer", StandardShanheSourceDemonArcherPackage.NightmareId,
                4, 0, 0, 0, 0, [Watch, ExtraWatch], [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Club, 3),
                    new(CardKind.Dodge, Suit.Diamond, 4), new(CardKind.Dodge, Suit.Spade, 5)], [],
                [new("classic:guan-yu", MaxHpBonus: 16, Hp: 20) { StartingHand = [] }], [], "fixture:source-demon-archer") { ExtraSkillSlots = 2 };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] }).WithExtension(new Fixture(),
                mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0,
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-demon-archer", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            object Condition() => new { kind = "choiceIs", sourceBind = "flow", optionId = "discard-extra" };
            object[] Effects() =>
            [
                new { op = "chooseOption", target = "owner", resultBind = "flow", options = new[] { new { id = "continue" }, new { id = "discard-extra" } } },
                new { op = "selectOwnedCards", target = "owner", minimumCards = 1, maximumCards = 1, zones = new[] { "hand" }, resultBind = "extra-discard", condition = Condition() },
                new { op = "moveBoundCards", target = "owner", sourceBind = "extra-discard", destination = "discardPile", awaitMovementTriggers = true, condition = Condition() }
            ];
            var programs = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new[] { new
            { id = Watch, revision = 1, triggers = new object[]
                { new { id = "original-youyan-gain", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                    movementOccurrence = "perBatch", movementReasons = new[] { GainReason }, optional = false, effects = Effects() },
                  new { id = "original-saogu-fee", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" },
                    movementOccurrence = "perBatch", movementReasons = new[] { FeeReason }, optional = false, effects = Effects() } } },
              new { id = ExtraWatch, revision = 1, triggers = new object[]
                { new { id = "original-extra-discard", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" },
                    movementOccurrence = "perBatch", movementReasons = new[] { ExtraReason }, optional = false,
                    effects = new object[] { new { op = "chooseOption", target = "owner", resultBind = "return", options = new[] { new { id = "continue" } } } } } } } } }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
                { [Watch] = new { name = "原诱言与扫谷移牌孩子", description = "返回实际原技能帧。", optionLabels = new Dictionary<string, string>
                    { ["continue"] = "继续", ["discard-extra"] = "在当前原移牌孩子中实际再弃一张牌" } },
                  [ExtraWatch] = new { name = "同阶段原弃牌孩子", description = "观察实际单牌弃置及原阶段。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            foreach (var (id, program) in programs.Programs) builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description)
            { Program = program, ProgramPresentation = programs.Presentations[id] });
            var suits = new[] { Suit.Spade, Suit.Heart, Suit.Club, Suit.Diamond };
            builder.AddDeck(new(Deck, "零自然起手摸牌的真实花色牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 48).Select(i => new ContentDeckPhysicalCard("standard:dodge", suits[i % 4], i % 13 + 1)).ToArray() });
        }
    }
    private static void Need(bool yes, string message) { if (!yes) throw new InvalidOperationException(message); }
}
