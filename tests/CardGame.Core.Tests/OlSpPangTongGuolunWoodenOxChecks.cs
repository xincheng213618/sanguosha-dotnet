using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlSpPangTongGuolunWoodenOxChecks
{
    private const string Hero = "ol:pang-tong-sp", Guolun = "ol:guolun", WoodenOx = "classic:wooden-ox";
    private const string Mode = "identity:classic-ol-guolun-wooden-ox", Deck = "fixture:guolun-wooden-ox-deck";
    private static readonly string[] Generals = [Hero, "fixture:guolun-ox-peer-1", "fixture:guolun-ox-peer-2", "fixture:guolun-ox-peer-3"];

    public static void GuolunCanExchangeHeldCardsButNotWoodenOxGrainCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        var startingHand = game.CreateSnapshot(0).Players[0].Hand;
        var ox = startingHand.First(card => card.Kind == CardKind.WoodenOx).Id;
        var duels = startingHand.Where(card => card.Kind == CardKind.Duel).Take(2).Select(card => card.Id).ToArray();
        var grain = duels[0];
        Accept(game, new PlayCardCommand(0, ox, [], game.Revision, P(game)!.PromptId));
        Settle(game);
        Accept(game, new UseEquipmentEffectCommand(0, CardKind.WoodenOx, [grain], [], game.Revision, P(game)!.PromptId));
        Settle(game);
        var owner = game.CreateSnapshot(0).Players[0];
        Require(owner.Equipment.Any(card => card.Id == ox) && owner.WoodenOxGrain!.Single().Id == grain &&
            game.CreateSnapshot(1).Players[0].WoodenOxGrain!.Count == 0 &&
            game.CardMovements.Single(move => move.CardId == grain && move.Reason == CardMoveReasons.WoodenOxStore).To == CardLocation.WoodenOxGrain(0),
            "A real public equipment command privately stores one physical Duel without moving the equipped treasure.");

        Accept(game, new UseProgramSkillCommand(0, Guolun, "reveal-exchange", [], [1], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, IsPrivate: true, SkillPrompt.SkillId: Guolun } &&
            prompt.Choices.Any(choice => Branch(choice) == "reveal"));
        var foreignHand = game.CreateSnapshot(1).Players[1].Hand.ToArray();
        var slot = Array.FindIndex(foreignHand, card => card.Kind == CardKind.WoodenOx);
        Require(slot >= 0 && P(game)!.Choices.Where(choice => Branch(choice) == "reveal").All(choice => choice.Cards.Count == 0),
            "Guolun offers the opponent's real hand as opaque slots before the chosen card is revealed.");
        Answer(game, choice => Branch(choice) == "reveal" && choice.Parameters.GetValueOrDefault("slot-index") == slot.ToString(System.Globalization.CultureInfo.InvariantCulture));
        Reach(game, prompt => prompt is { PlayerSeat: 0, IsPrivate: true, SkillPrompt.SkillId: Guolun } &&
            prompt.Choices.Any(choice => Branch(choice) == "exchange"));
        var foreign = foreignHand[slot];
        Require(game.Events.Select(entry => entry.Payload).OfType<GuolunRevealedEvent>().Single() is { TargetSeat: 1 } revealed &&
            revealed.CardId == foreign.Id && P(game)!.Choices.Any(choice => Branch(choice) == "exchange" && choice.Cards.SequenceEqual([ox])),
            "The real revealed card is one of seat one's original hand cards, and the owner can offer the equipped treasure itself.");

        var choices = P(game)!.Choices.ToArray();
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the real revealed identity and each viewer's exchange menu.");
        var own = duels[1];
        Answer(restored, choice => Branch(choice) == "exchange" && choice.Cards.SequenceEqual([own]));
        Settle(restored);
        var exchange = restored.CardMovements.Where(move => move.Reason.Value == "skill-program.guolun.exchange").ToArray();
        Require(exchange.Length == 2 && exchange[0].BatchId is > 0 && exchange[0].BatchId == exchange[1].BatchId &&
            exchange.Any(move => move.CardId == own && move.From == CardLocation.Hand(0) && move.To == CardLocation.Hand(1)) &&
            exchange.Any(move => move.CardId == foreign.Id && move.From == CardLocation.Hand(1) && move.To == CardLocation.Hand(0)) &&
            restored.CardMovements.Single(move => move.Reason.Value == "skill-program.sp-pang-tong.draw").To == CardLocation.Hand(0),
            "The legal held Duel and revealed hand card really exchange in one batch, then the lower-rank payer draws once.");
        Require(choices.All(choice => !choice.Cards.Contains(grain)) &&
            restored.CreateSnapshot(0).Players[0].WoodenOxGrain!.Single().Id == grain,
            $"Guolun must leave real stored card {grain} out of the exchange menu while its treasure is equipped.");
    }

    private static string? Branch(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch");

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
            var owner = game.CreateSnapshot(0).Players[0].Hand;
            if (owner.Any(card => card.Kind == CardKind.WoodenOx) && owner.Count(card => card.Kind == CardKind.Duel) >= 2 &&
                game.CreateSnapshot(1).Players[1].Hand.Any(card => card.Kind == CardKind.WoodenOx))
                return game;
        }
        throw new InvalidOperationException("The Guolun fixture requires a physical Wooden Ox for each participant and two actual Duel cards for the owner.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-guolun-wooden-ox", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            new StandardClassicGeneralPackage().Register(new TreasureOnlyBuilder(builder));
            foreach (var peer in Generals.Skip(1))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "真实木牛与决斗", 8, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(index => index % 2 == 0 ? new ContentDeckPhysicalCard(WoodenOx, Suit.Diamond, 5) :
                    new ContentDeckPhysicalCard("standard:duel", Suit.Spade, 1)).ToArray() });
            builder.AddMode(new(Mode, "过论木牛", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class TreasureOnlyBuilder(IContentRegistryBuilder inner) : IContentRegistryBuilder
    {
        public void AddCard(ContentCardDefinition card)
        {
            if (card.Id == WoodenOx) inner.AddCard(card);
        }

        public void AddGeneral(ContentGeneralDefinition general) { }
        public void AddSkill(ContentSkillDefinition skill) { }
        public void AddDeck(ContentDeckRecipe deck) { }
        public void AddMode(ContentModeDefinition mode) { }
    }
}
