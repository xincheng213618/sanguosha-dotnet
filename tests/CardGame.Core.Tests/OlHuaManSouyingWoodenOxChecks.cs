using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlHuaManSouyingWoodenOxChecks
{
    private const string HuaMan = "ol:hua-man", Souying = "ol:souying";
    private const string WoodenOx = "classic:wooden-ox", Discard = "skill-program.huaman.discard";
    private const string Mode = "identity:classic-ol-souying-wooden-ox", Deck = "fixture:ol-souying-wooden-ox-deck";
    private static readonly string[] Generals = [HuaMan, "fixture:souying-ox-peer-1", "fixture:souying-ox-peer-2", "fixture:souying-ox-peer-3"];

    internal static void SouyingCanDiscardTreasureButCannotDirectlyDiscardItsGrainCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        var hand = game.CreateSnapshot(0).Players[0].Hand;
        var ox = hand.First(card => card.Kind == CardKind.WoodenOx).Id;
        var duels = hand.Where(card => card.Kind == CardKind.Duel).Take(3).Select(card => card.Id).ToArray();
        var grain = duels[0];
        Accept(game, new PlayCardCommand(0, ox, [], game.Revision, P(game)!.PromptId));
        Settle(game);
        Require(game.GetLegalActions(0).Any(action => action.Kind == LegalActionKind.UseEquipmentEffect &&
            action.EquipmentKind == CardKind.WoodenOx && action.SelectableCardIds.Contains(grain)),
            "The equipped Wooden Ox publishes its native storage action and the actual hand card is eligible.");
        Accept(game, new UseEquipmentEffectCommand(0, CardKind.WoodenOx, [grain], [], game.Revision, P(game)!.PromptId));
        Settle(game);
        var held = game.CreateSnapshot(0).Players[0];
        Require(held.Equipment.Any(card => card.Id == ox) && held.WoodenOxGrain!.Single().Id == grain &&
            game.CreateSnapshot(1).Players[0].WoodenOxGrain!.Count == 0 &&
            game.GetLegalActions(0).Any(action => action.CardId == grain && action.Kind == LegalActionKind.Duel),
            "The native treasure is equipped and its real stored Duel is private yet playable as a card.");

        PlayDuel(game, duels[1]);
        Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } || IsSouyingOffer(prompt));
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && Finished(game, duels[1]).Count() == 1,
            "The first directed Duel actually finishes without a Souying offer.");
        PlayDuel(game, duels[2]);
        Reach(game, IsSouyingOffer);
        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, IsPrivate: true, SkillPrompt.SkillId: Souying } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "discard"));
        var choices = P(game)!.Choices;
        Require(choices.Any(choice => choice.Cards.SequenceEqual([ox])), "Souying must offer the actual equipped Wooden Ox.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the private payment and the actual treasure contents.");
        Answer(restored, choice => choice.Cards.SequenceEqual([ox]));
        Settle(restored);
        var paid = restored.CardMovements.Single(move => move.Reason.Value == Discard);
        var dropped = restored.CardMovements.Single(move => move.Reason == CardMoveReasons.WoodenOxGrainDiscard);
        var finished = Finished(restored, duels[2]).Single();
        Require(paid.CardId == ox && paid.From == CardLocation.Equipment(0) && paid.To == CardLocation.DiscardPile &&
            dropped.CardId == grain && dropped.From == CardLocation.WoodenOxGrain(0) && dropped.To == CardLocation.DiscardPile &&
            restored.Events.Select(entry => entry.Payload).OfType<SouyingNativeMutationEvent>().Single() is { ClaimedCount: 1 } mutation &&
            mutation.NativeUseId == finished.ResolutionId && restored.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == duels[2]),
            "Discarding the equipment pays once, clears the grain through native treasure loss and claims the original second Duel.");

        Require(choices.All(choice => !choice.Cards.Contains(grain)) &&
            game.CardMovements.Single(move => move.CardId == grain && move.Reason == CardMoveReasons.WoodenOxStore).To == CardLocation.WoodenOxGrain(0),
            "Souying must not offer the real stored Duel as direct discard payment while the treasure remains equipped.");
    }

    private static bool IsSouyingOffer(PendingDecision prompt) => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Souying } &&
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");

    private static void PlayDuel(GameEngine game, int cardId)
    {
        var action = game.GetLegalActions(0).Single(candidate => candidate.Kind == LegalActionKind.Duel &&
            candidate.CardId == cardId && candidate.TargetSeats.SequenceEqual([1]));
        Accept(game, new PlayCardCommand(0, cardId, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));
    }

    private static IEnumerable<CardUseFinishedEvent> Finished(GameEngine game, int cardId) => game.Events
        .Select(entry => entry.Payload).OfType<CardUseFinishedEvent>().Where(entry => entry.CardId == cardId && entry.CardKind == CardKind.Duel);

    private static GameEngine Start(ContentRegistry content)
    {
        for (var seed = 47; seed < 55; seed++)
        {
            var game = GameEngine.CreateStandard(new GameOptions { Seed = seed, PlayerCount = 4, ModeId = Mode,
                HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
            Accept(game, new StartGameCommand());
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
            while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
            {
                Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
                Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
            }

            var hand = game.CreateSnapshot(0).Players[0].Hand;
            if (hand.Any(card => card.Kind == CardKind.WoodenOx) && hand.Count(card => card.Kind == CardKind.Duel) >= 3)
                return game;
        }

        throw new InvalidOperationException("The Souying fixture requires a real Wooden Ox and three distinct Duel cards at setup.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-souying-wooden-ox", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, HuaMan);
            new StandardClassicGeneralPackage().Register(new TreasureOnlyBuilder(builder));
            foreach (var peer in Generals.Skip(1))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "木牛与决斗", 8, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(index => index % 2 == 0 ? new ContentDeckPhysicalCard(WoodenOx, Suit.Diamond, 5) :
                    new ContentDeckPhysicalCard("standard:duel", Suit.Spade, 1)).ToArray() });
            builder.AddMode(new(Mode, "薮影木牛", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class TreasureOnlyBuilder(IContentRegistryBuilder inner) : IContentRegistryBuilder
    {
        public void AddCard(ContentCardDefinition card)
        {
            if (card.Id == WoodenOx)
                inner.AddCard(card);
        }

        public void AddGeneral(ContentGeneralDefinition general) { }
        public void AddSkill(ContentSkillDefinition skill) { }
        public void AddDeck(ContentDeckRecipe deck) { }
        public void AddMode(ContentModeDefinition mode) { }
    }
}
