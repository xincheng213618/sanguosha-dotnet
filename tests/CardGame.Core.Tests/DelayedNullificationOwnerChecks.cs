using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class DelayedNullificationOwnerChecks
{
    private const string Mode = "identity:delayed-nullification-owner", Deck = "fixture:delayed-nullification-owner-deck";
    private static readonly string[] Generals = Enumerable.Range(0, 4).Select(seat => $"fixture:delayed-nullification-owner-{seat}").ToArray();

    internal static void HeldIndulgenceOwnsItsNativeCounterspellPaymentCold()
    {
        var (game, content) = Start();
        var effect = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Indulgence).Id;
        var counterspell = game.CreateSnapshot(1).Players[1].Hand.Single(card => card.Kind == CardKind.Nullification).Id;
        var placingTurn = game.State.TurnNumber;
        var action = game.GetHumanLegalActions().Single(item => item.CardId == effect &&
            item.Kind == LegalActionKind.Indulgence && item.TargetSeats.SequenceEqual([1]));
        Accept(game, new PlayCardCommand(0, effect, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));
        ReachPlay(game, 0);
        var placement = Facts<CardUseFinishedEvent>(game).Single(entry => entry.CardId == effect);
        Require(placement.CardKind == CardKind.Indulgence &&
            game.CardMovements.Count(move => move.CardId == effect && move.From == CardLocation.Processing &&
                move.To == CardLocation.Judgment(1) && move.Reason == CardMoveReasons.DelayedCardPlace) == 1 &&
            !Facts<NullificationRequestedEvent>(game).Any(),
            "The actual Indulgence completes placement while its recipient already holds the physical counterspell.");

        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.Nullification, IncomingCard: CardKind.Indulgence });
        var window = game.ResolutionStack.OfType<NullificationWindowFrame>().Single();
        Require(window.DelayedReturn is { OwnerSeat: 1, Phase: TurnPhase.Draw, EffectiveKind: CardKind.Indulgence } held &&
            held.CardId == effect && held.Phase == game.State.Phase && held.TurnNumber == game.State.TurnNumber && held.TurnNumber > placingTurn &&
            held.ParentFrameId == window.ParentFrameId &&
            game.ResolutionStack.OfType<CardUseFrame>().All(frame => frame.Id != window.ParentFrameId && frame.Id != placement.ResolutionId) &&
            Zone(game, effect) == CardLocation.Judgment(1) && Zone(game, counterspell) == CardLocation.Hand(1),
            "The actual Judgment-phase window owns the still-held delayed entity through DelayedReturn after the placement frame has finished.");

        var passed = Cold(game, content);
        Answer(passed, choice => choice.Parameters.GetValueOrDefault("response") == "pass");
        ReachPlay(passed, 1);
        Require(Facts<NullificationResolvedEvent>(passed).Single(entry => entry.WindowFrameId == window.Id) is
                { EffectNullified: false, ChainDepth: 0 } &&
            Facts<DelayedCardResolvedEvent>(passed).Single(entry => entry.CardId == effect) is
                { TargetSeat: 1, JudgmentSucceeded: true, SkippedPlayPhase: false } &&
            Zone(passed, effect) == CardLocation.DiscardPile && Zone(passed, counterspell) == CardLocation.Hand(1),
            "Passing the same original window leaves the counterspell in hand and lets the held effect actually judge and finish.");

        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([counterspell]) && choice.Parameters.GetValueOrDefault("response") == "nullification");
        ReachPlay(game, 1);
        var entered = game.CardMovements.Single(move => move.CardId == counterspell && move.Reason == CardMoveReasons.Nullification);
        var finished = game.CardMovements.Single(move => move.CardId == counterspell && move.Reason == CardMoveReasons.NullificationFinished);
        Require(entered.From == CardLocation.Hand(1) && entered.To == CardLocation.Processing &&
            finished.From == CardLocation.Processing && finished.To == CardLocation.DiscardPile && finished.Sequence > entered.Sequence &&
            Facts<NativeTrueUseProcessingEntryEvent>(game).Single(entry => entry.CardId == counterspell) ==
                new NativeTrueUseProcessingEntryEvent(window.Id, 0, counterspell, CardKind.Nullification, entered.From, entered.Sequence,
                    1, NativeTrueUseDiscardKind.Nullification),
            "The real counterspell payment keeps its own Processing entry and the original delayed window identity without a card-use action parent.");
        Require(Facts<NullificationResolvedEvent>(game).Single(entry => entry.WindowFrameId == window.Id) is
                { EffectNullified: true, ChainDepth: 1 } &&
            !Facts<DelayedCardResolvedEvent>(game).Any(entry => entry.CardId == effect) &&
            game.CardMovements.Count(move => move.CardId == effect && move.From == CardLocation.Judgment(1) &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.DelayedCardFinish) == 1,
            "The exact held effect is nullified before judgment, finishes once, and returns to the recipient's ordinary Play phase.");
        _ = Cold(game, content);
    }

    private static CardLocation Zone(GameEngine game, int cardId) => game.CreateCardZoneDiagnostics().Single(item => item.CardId == cardId).Location;
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static void ReachPlay(GameEngine game, int seat) => Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard } && prompt.PlayerSeat == seat);

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay keeps the exact held delayed entity, paid movement and each seat's current response choices.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        ReachOpening();
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            ReachOpening();
        }
        return (game, content);

        void ReachOpening() => Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral ||
            prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-delayed-nullification-owner", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            // The real Chuanyu descriptor requests native use entry facts without occupying a seat.
            CharacterFixtureContent.Register(builder, "mou:hu-lie");
            foreach (var general in Generals)
                builder.AddGeneral(new(general, "同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "真乐不思蜀和无懈固定牌堆", 2, 0, [])
            {
                PhysicalCards = Enumerable.Range(1, 12).Select(rank => rank switch
                {
                    6 => new ContentDeckPhysicalCard("standard:indulgence", Suit.Club, rank),
                    9 => new ContentDeckPhysicalCard("standard:nullification", Suit.Club, rank),
                    _ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, rank)
                }).ToArray()
            });
            builder.AddMode(new(Mode, "延时牌判定原生无懈", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
