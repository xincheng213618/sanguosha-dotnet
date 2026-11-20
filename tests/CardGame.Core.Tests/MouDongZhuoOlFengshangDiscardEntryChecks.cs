using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class MouDongZhuoOlFengshangDiscardEntryChecks
{
    private const string Fengshang = "mou-ol:fengshang", Xiongni = "mou-ol:xiongni";
    private const string Watch = "fixture:fengshang-gain-reentry", Mode = "identity:fengshang-entry", Deck = "fixture:fengshang-entry-deck";
    private const string XiongniDiscard = "skill-program.mou-ol:xiongni.discard", FengshangGift = "skill-program.mou-ol:fengshang.give";
    private const string Rediscard = "skill-program.fixture:fengshang-gain-reentry.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = ["fixture:fengshang-owner", "fixture:fengshang-first-peer", "fixture:fengshang-second-peer", "fixture:fengshang-third-peer"];

    internal static void FengshangSecondGiftRequiresItsOriginalDiscardEntryCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        var first = game.CreateSnapshot(0).Players[0].Hand.Single().Id;
        var second = game.CreateSnapshot(1).Players[1].Hand.Single().Id;
        Require(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Xiongni } && game.CreateSnapshot(0) is { DrawPileCount: 0, DiscardPileCount: 0 },
            "All four physical cards are dealt before the original Xiongni offer.");
        Activate(game);
        Choose(game, Xiongni, 0, "discard", first);
        Choose(game, Xiongni, 1, "discard", second);
        Choose(game, Xiongni, 2, "damage");
        Choose(game, Xiongni, 3, "damage");
        var play = Next(game);
        var costs = game.CardMovements.Where(move => move.Reason.Value == XiongniDiscard).ToArray();
        Require(play is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && game.CreateSnapshot(0) is { DrawPileCount: 0, DiscardPileCount: 2 } &&
            costs is [var firstDiscard, var secondDiscard] && firstDiscard.CardId == first && firstDiscard.From == CardLocation.Hand(0) &&
            secondDiscard.CardId == second && secondDiscard.From == CardLocation.Hand(1) && costs.All(cost => cost.To == CardLocation.DiscardPile) &&
            Facts<MouDongZhuoOlPaidEvent>(game).Select(fact => fact.Sequence).SequenceEqual(costs.Select(move => (long)move.Sequence)),
            "The real Xiongni pays twice with Clubs from two original hands and leaves only that pair in the discard pile.");
        Accept(game, new UseProgramSkillCommand(0, Fengshang, "distribute-pair", [], [], game.Revision, play.PromptId));
        Choose(game, Fengshang, 0, "card", first);
        Choose(game, Fengshang, 0, "recipient", target: 0);
        Choose(game, Fengshang, 0, "card", second);
        Choose(game, Fengshang, 0, "recipient", target: 2);

        var firstGain = Next(game);
        var binding = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Fengshang);
        var gift = Facts<MouDongZhuoOlGiftEvent>(game).Single();
        var movement = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(window => window.Batch.ParentFrameId == binding.Id);
        Require(firstGain is { PlayerSeat: 0, SkillPrompt.SkillId: Watch } && binding.MouDongZhuoOl is { Paid: true, Stage: MouDongZhuoOlStage.GiftChildren, Cursor: 1 } receipt &&
            receipt.Gifts.Select(item => (item.CardId, item.Recipient)).SequenceEqual([(first, 0), (second, 2)]) &&
            gift.FrameId == binding.Id && gift.CardId == first && gift.Recipient == 0 &&
            movement.Batch.Movements is [var paid] && paid.Sequence == gift.Sequence && paid.Reason.Value == FengshangGift && paid.From == CardLocation.DiscardPile && paid.To == CardLocation.Hand(0) &&
            Zone(game, first) == CardLocation.Hand(0) && Zone(game, second) == CardLocation.DiscardPile,
            "Both distributions are selected before payment and the first real gift is awaiting its own gain child.");

        var control = Cold(game, content);
        Skip(control);
        Require(Next(control) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
            Facts<MouDongZhuoOlGiftEvent>(control).Where(fact => fact.FrameId == binding.Id).Select(fact => (fact.CardId, fact.Recipient)).SequenceEqual([(first, 0), (second, 2)]) &&
            Zone(control, first) == CardLocation.Hand(0) && Zone(control, second) == CardLocation.Hand(2) && CompletedOnce(control, binding.Id),
            "Both unchanged original discard entries are delivered once to their individually selected recipients.");

        Activate(game);
        var rethrow = Next(game);
        var pickup = game.CardMovements.Where(move => move.CardId == second && move.Sequence > gift.Sequence).ToArray();
        Require(rethrow is { PlayerSeat: 0, SkillPrompt.SkillId: Watch } && rethrow.Choices.Any(choice => choice.Cards.SequenceEqual([second])) &&
            game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(window => window.Id == movement.Id) &&
            pickup is [var shuffled, var drawn] && shuffled.Reason == CardMoveReasons.Reshuffle && shuffled.From == CardLocation.DiscardPile &&
            shuffled.To == CardLocation.DrawPile && drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0),
            "The first gift child really shuffles and draws the other selected card out of its original discard entry.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([second]));

        var fresh = game.CardMovements.Single(move => move.CardId == second && move.Reason.Value == Rediscard);
        Require(fresh.From == CardLocation.Hand(0) && fresh.To == CardLocation.DiscardPile && fresh.Sequence > pickup[^1].Sequence &&
            fresh.TurnNumber == costs[1].TurnNumber && fresh.BatchId != costs[1].BatchId,
            "The actual hand discard makes a newer entry for the selected card during the same turn.");
        Require(Next(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && Zone(game, first) == CardLocation.Hand(0) && Zone(game, second) == CardLocation.DiscardPile &&
            Facts<MouDongZhuoOlGiftEvent>(game).Where(fact => fact.FrameId == binding.Id).SequenceEqual([gift]) && CompletedOnce(game, binding.Id),
            "The first gift and paid Fengshang complete without giving away the second card from its newer discard entry.");
    }

    private static bool CompletedOnce(GameEngine game, long id) =>
        Facts<MouDongZhuoOlFengshangEvent>(game).Single(fact => fact.FrameId == id) is { Owner: 0, Dying: false, Suit: Suit.Club } &&
        Facts<MouDongZhuoOlCompletedEvent>(game).Count(fact => fact.FrameId == id) == 1 &&
        !Facts<MouDongZhuoOlWineIssuedEvent>(game).Any(fact => fact.FrameId == id);

    private static void Choose(GameEngine game, string skill, int seat, string branch, int? card = null, int? target = null)
    {
        var prompt = Next(game);
        Require(prompt.PlayerSeat == seat && prompt.SkillPrompt?.SkillId == skill, $"The {branch} choice is offered to seat {seat} by {skill}.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("branch") == branch &&
            (card is null || choice.Cards.SequenceEqual([card.Value])) && (target is null || choice.Targets.SequenceEqual([target.Value])));
    }

    private static GameEngine Start(ContentRegistry content)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
        return game;
    }

    private static CardLocation Zone(GameEngine game, int id) => game.CreateCardZoneDiagnostics().Single(fact => fact.CardId == id).Location;
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static PendingDecision Next(GameEngine game) { Reach(game, _ => true); return P(game)!; }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold restoration preserves both selected recipients, the actual gain child and each seat's prompt.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mou-dong-zhuo-ol-fengshang-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "mou:dong-zhuo-ol");
            var rules = $$"""
                {"skills":[{"id":"{{Watch}}","revision":1,"triggers":[{"id":"draw-and-discard-after-gift","window":"cardsGained",
                  "subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{FengshangGift}}"],
                  "optional":true,"effects":[{"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true},
                  {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                   "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "封赏获牌后摸弃", description = "因封赏获得牌后，可以摸一张牌并弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Generals[0], "谋董卓", "supporter", Fengshang, "qun", 4, [Xiongni, Watch]));
            foreach (var general in Generals.Skip(1))
                builder.AddGeneral(new(general, "同桌", "supporter", "standard:none", "wei", 5));
            builder.AddDeck(new(Deck, "四张真实梅花杀", InitialHandSize: 1, DrawPerTurn: 0, [])
            {
                PhysicalCards = Enumerable.Range(2, 4).Select(rank => new ContentDeckPhysicalCard("standard:slash", Suit.Club, rank)).ToArray()
            });
            builder.AddMode(new(Mode, "OL 谋董卓封赏原弃牌", 4, 4, new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2
            }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
