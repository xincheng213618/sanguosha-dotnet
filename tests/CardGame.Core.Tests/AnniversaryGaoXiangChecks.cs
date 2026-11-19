using System.Globalization;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class AnniversaryGaoXiangChecks
{
    private const string GaoXiang = "anniversary:gao-xiang", Chiying = "anniversary:chiying";
    private const string RelayOwner = "fixture:chiying-relay-owner";
    private const string Watch = "fixture:chiying-original-discard";
    private const string Mode = "identity:chiying-original-discard", Deck = "fixture:chiying-deck";
    private const string OriginalDiscard = "skill-program.anniversary:chiying.discard";
    private const string Rediscard = "skill-program.fixture:chiying-original-discard.SelectAndMoveOwnedCard";
    private const string Gain = "skill-program.anniversary:chiying.obtain-basics";
    private static readonly string[] Generals = [GaoXiang, "fixture:chiying-actor", "fixture:chiying-victim", "fixture:chiying-peer"];

    internal static void ChiyingCannotTransferABasicAtANewerDiscardEntryCold()
    {
        var (game, content) = Start();
        var initial = game.CreateSnapshot(2);
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && initial.DrawPileCount == 0 && initial.DiscardPileCount == 0 &&
            initial.Players[2].Hand is [{ Kind: CardKind.Dodge }],
            "Gao Xiang's first natural draw exhausts the stock and leaves exactly one basic card in the victim's hand.");
        var card = initial.Players[2].Hand.Single().Id;
        Accept(game, new UseProgramSkillCommand(0, Chiying, "assist-range-discards", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Chiying });
        Answer(game, choice => Step(choice) == "actor" && choice.Targets.SequenceEqual([1]));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Chiying });
        Answer(game, choice => Step(choice) == "victim" && choice.Targets.SequenceEqual([2]));
        Answer(game, choice => Step(choice) == "finish");
        Reach(game, prompt => prompt is { PlayerSeat: 1, TargetSeat: 2, SkillPrompt.SkillId: Chiying } && prompt.Choices.Any(choice => Step(choice) == "card"));
        Require(P(game) is { IsPrivate: true, Choices: [var slot] } && slot.Cards.Count == 0 &&
            slot.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand) &&
            slot.Parameters.GetValueOrDefault("slot-index") == "0" && game.CreateSnapshot(2).PendingDecision is null,
            "The chosen participant controls a private opaque slot from the other player's hand.");
        Answer(game, choice => Step(choice) == "card");
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Watch });

        var original = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == OriginalDiscard);
        var paid = Facts<GaoXiangChiyingMovedEvent>(game).Single(fact => !fact.Gain);
        Require(paid is { Victim: 2, Ordinal: 0, PrintedKind: CardKind.Dodge } && paid.CardId == card &&
            paid.BatchId == original.BatchId && paid.Sequence == original.Sequence &&
            original.From == CardLocation.Hand(2) && original.To == CardLocation.DiscardPile &&
            Facts<NativeLogicalDiscardMaterialCompletedEvent>(game).Any(fact => fact.CardId == card && fact.CompletionSequence == original.Sequence) &&
            game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paid.FrameId).GaoXiangChiying is
                { Actor: 1, Stage: GaoXiangChiyingStage.DiscardChildren },
            "Chiying waits for the native discard child after recording its exact payment.");

        var control = Cold(game, content);
        Skip(control);
        Settle(control);
        var gain = control.CardMovements.Single(move => move.Reason.Value == Gain);
        Require(gain.CardId == card && gain.From == CardLocation.DiscardPile && gain.To == CardLocation.Hand(1) &&
            gain.Sequence > original.Sequence && Facts<GaoXiangChiyingMovedEvent>(control).Count(fact => fact.Gain && fact.FrameId == paid.FrameId && fact.CardId == card) == 1,
            "If the original basic card stays at its discard entry, Chiying really transfers it once to the chosen participant.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Watch } && prompt.Choices.Any(choice => choice.Cards.SequenceEqual([card])));
        var afterPayment = game.CardMovements.Where(move => move.CardId == card && move.Sequence > original.Sequence).ToArray();
        Require(afterPayment is [var reshuffled, var drawn] && reshuffled.Reason == CardMoveReasons.Reshuffle &&
            reshuffled.From == CardLocation.DiscardPile && reshuffled.To == CardLocation.DrawPile && drawn.Reason.Value == $"skill-program.{Watch}.Draw" &&
            drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(2),
            "The real draw child reshuffles the only discard and draws that same basic card to its previous owner.");

        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        Settle(game);
        var newer = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Rediscard);
        Require(newer.From == original.From && newer.To == original.To && newer.Sequence > original.Sequence && newer.BatchId != original.BatchId &&
            Facts<GaoXiangChiyingMovedEvent>(game).Where(fact => !fact.Gain).SequenceEqual([paid]),
            "The public re-discard creates a newer discard-pile entry without paying the original skill twice.");
        Require(!game.CardMovements.Any(move => move.Reason.Value == Gain) && !Facts<GaoXiangChiyingMovedEvent>(game).Any(fact => fact.Gain) &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location == CardLocation.DiscardPile,
            $"Chiying must leave the basic card at its newer entry {newer.Sequence}; the original payment referred to entry {original.Sequence}.");
    }

    internal static void ChiyingAcceptsTwoPaymentsOfTheSameCardFromDifferentVictimsCold()
    {
        var (game, content) = Start(relayFirstCard: true);
        var initial = game.CreateSnapshot(0);
        Require(initial.DrawPileCount == 0 && initial.DiscardPileCount == 0,
            "The first turn's actual draw empties the six-card stock before either payment.");
        var card = game.CreateCardZoneDiagnostics().Single(item => item.Location == CardLocation.Hand(0) && item.ZoneIndex == 0).CardId;
        Accept(game, new UseProgramSkillCommand(0, Chiying, "assist-range-discards", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Chiying });
        Answer(game, choice => Step(choice) == "actor" && choice.Targets.SequenceEqual([1]));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Chiying });
        Answer(game, choice => Step(choice) == "victim" && choice.Targets.SequenceEqual([2]));
        Answer(game, choice => Step(choice) == "victim" && choice.Targets.SequenceEqual([0]));
        Answer(game, choice => Step(choice) == "finish");
        Reach(game, prompt => prompt is { PlayerSeat: 1, TargetSeat: 0, SkillPrompt.SkillId: Chiying } && prompt.Choices.Any(choice => Step(choice) == "card"));
        Answer(game, choice => Step(choice) == "card" && choice.Parameters.GetValueOrDefault("slot-index") == "0");

        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Watch } &&
            prompt.Choices.Any(choice => choice.Targets.SequenceEqual([2])));
        var original = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == OriginalDiscard);
        var firstPayment = Facts<GaoXiangChiyingMovedEvent>(game).Single();
        Require(firstPayment is { Gain: false, Victim: 0, Ordinal: 0, PrintedKind: CardKind.Dodge } && firstPayment.Sequence == original.Sequence &&
            firstPayment.BatchId == original.BatchId && original.From == CardLocation.Hand(0) && original.To == CardLocation.DiscardPile &&
            game.CardMovements.Where(move => move.CardId == card && move.Sequence > original.Sequence).ToArray() is [var reshuffled, var drawn] &&
            reshuffled.Reason == CardMoveReasons.Reshuffle && reshuffled.From == CardLocation.DiscardPile && reshuffled.To == CardLocation.DrawPile &&
            drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0),
            "The original seat pays first in turn order and its real draw child shuffles and recovers exactly that card.");
        Answer(game, choice => choice.Targets.SequenceEqual([2]));
        Reach(game, prompt => prompt is { PlayerSeat: 1, TargetSeat: 2, SkillPrompt.SkillId: Chiying } && prompt.Choices.Any(choice => Step(choice) == "card"));

        var forwarded = game.CardMovements.Last(move => move.CardId == card);
        Require(forwarded.From == CardLocation.Hand(0) && forwarded.To == CardLocation.Hand(2) && forwarded.Sequence > original.Sequence,
            "The first payment's child forwards that same physical card to the next committed victim.");
        game = Cold(game, content);
        var secondSlot = game.CreateCardZoneDiagnostics().Single(item => item.CardId == card && item.Location == CardLocation.Hand(2)).ZoneIndex;
        Answer(game, choice => Step(choice) == "card" && choice.Cards.Count == 0 &&
            choice.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand) &&
            choice.Parameters.GetValueOrDefault("slot-index") == secondSlot.ToString(CultureInfo.InvariantCulture));
        Settle(game);

        var second = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == OriginalDiscard && move.Sequence > original.Sequence);
        var receipts = Facts<GaoXiangChiyingMovedEvent>(game).ToArray();
        Require(second.From == CardLocation.Hand(2) && second.To == CardLocation.DiscardPile && second.Sequence > forwarded.Sequence &&
            second.BatchId != original.BatchId && receipts.Where(fact => !fact.Gain).ToArray() is [var first, var latest] &&
            first == firstPayment && latest is { Victim: 2, Ordinal: 1, PrintedKind: CardKind.Dodge } &&
            latest.FrameId == firstPayment.FrameId && latest.CardId == card && latest.Sequence == second.Sequence && latest.BatchId == second.BatchId &&
            Facts<NativeLogicalDiscardMaterialCompletedEvent>(game).Where(fact => fact.CardId == card && fact.To == CardLocation.DiscardPile)
                .Select(fact => fact.CompletionSequence).SequenceEqual([original.Sequence, second.Sequence]),
            "Both physical discards are valid separate native payments on the same skill frame, with their own victim, batch and sequence.");
        var gain = game.CardMovements.Single(move => move.Reason.Value == Gain);
        Require(gain.CardId == card && gain.From == CardLocation.DiscardPile && gain.To == CardLocation.Hand(1) && gain.Sequence > second.Sequence &&
            receipts.Where(fact => fact.Gain).ToArray() is [{ Victim: 2, Ordinal: 1 } transferred] &&
            transferred.FrameId == firstPayment.FrameId && transferred.Sequence == gain.Sequence && transferred.CardId == card,
            "Only the second original entry remains current, so Chiying transfers the card once against the second victim's payment.");
    }

    private static (GameEngine Game, ContentRegistry Content) Start(bool relayFirstCard = false)
    {
        string[] generals = relayFirstCard ? [RelayOwner, .. Generals.Skip(1)] : Generals;
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(generals, relayFirstCard));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "Seat zero starts the actual Play phase.");
        return (game, content);
    }

    private static string? Step(PromptChoice choice) => choice.Parameters.GetValueOrDefault("mode");

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves every seat's view of the pending decision and exact card entries.");
        return restored;
    }

    private sealed class Fixture(string[] generals, bool relayFirstCard) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-chiying-original-discard", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, GaoXiang);
            var continuation = relayFirstCard
                ? """
                    {"op":"draw","target":"owner","amount":1,"resultBind":"drawn"},
                    {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                    {"op":"moveBoundCards","target":"owner","sourceBind":"drawn","destination":"selectedTargetHand"}
                    """
                : """
                    {"op":"draw","target":"owner","amount":1},
                    {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                     "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}
                    """;
            var rules = $$$"""
                {"skills":[{"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"draw-and-discard","window":"cardsMoved","subject":"owner",
                    "sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{OriginalDiscard}}}"],"optional":{{{(relayFirstCard ? "false" : "true")}}},
                    "effects":[{{{continuation}}}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = relayFirstCard ? "摸取转交" : "摸取重弃",
                    description = relayFirstCard ? "摸一张牌，再交给另一名角色。" : "摸一张牌，再弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            if (relayFirstCard)
                builder.AddGeneral(new(RelayOwner, "高翔", "supporter", Chiying, "shu", 4, [Watch]));
            builder.AddGeneral(new(Generals[1], "执行者", "supporter", "standard:none", "wei", 3));
            builder.AddGeneral(new(Generals[2], "受弃者", "supporter", relayFirstCard ? "standard:none" : Watch, "wei", 4));
            builder.AddGeneral(new(Generals[3], "同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "六张固定闪", 1, 2, []) { PhysicalCards = Enumerable.Range(1, 6)
                .Select(rank => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, rank)).ToArray() });
            builder.AddMode(new(Mode, "驰应原入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: generals));
        }
    }
}
