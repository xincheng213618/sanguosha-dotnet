using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheShopStockChecks
{
    private const string Crossbow = "codex-card-4001", SilverLion = "codex-card-4018",
        DrawTwo = "codex-card-4141", BlackSlash = "codex-card-4062", RedSlash = "codex-card-4063";

    public static void PurchasesNativeEquipmentAndCards()
    {
        var source = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        string[] ids = [Crossbow, SilverLion, DrawTwo, BlackSlash, RedSlash];
        var rewards = ids.Select(id => source.Rewards.Single(reward => reward.Id == id)).ToArray();
        var choices = source.Nodes.First(node => node.Kind == ShanheNodeKind.Shop).Choices!
            .Where(choice => choice.RewardId is { } id && ids.Contains(id, StringComparer.Ordinal)).ToArray();
        var definition = new ShanheCampaignDefinition("fixture:shanhe-shop-stock", "商店购入后实战", ["classic:cao-cao"], ["shop"],
            [new("shop", "集市", ShanheNodeKind.Shop, ["battle"], Choices: choices),
             new("battle", "试阵", ShanheNodeKind.Battle, [],
                 [new("footman", "空手步卒", [new(StandardShanheLocalTroopPackage.FootmanId) { StartingHand = [] }])])],
            rewards, StartingGold: 3000)
        { StartingResourcesExpireOnUse = source.StartingResourcesExpireOnUse };
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:cao-cao")
        {
            UseCodexRewardPool = true, UnlockedRewardIds = [Crossbow, SilverLion]
        }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        Accept(session, new MoveShanheNodeCommand("shop", session.Revision));
        Require(session.Snapshot.ShopItems.Count == 5 &&
            session.Snapshot.ShopItems.Select(item => item.Reward!.Id).ToHashSet(StringComparer.Ordinal).SetEquals(ids),
            "The small real stock must offer both unlocked equipment faces and all three physical cards.");

        Buy(session, Crossbow, 600);
        Require(Item(session, SilverLion).GoldCost == 300 && Item(session, DrawTwo).GoldCost == 250 &&
            Item(session, BlackSlash).GoldCost == 150 && Item(session, RedSlash).GoldCost == 150,
            "Buying the real Crossbow adds 100 only to equipment, preserving each card's source price.");
        Buy(session, SilverLion, 300);
        Buy(session, DrawTwo, 250);
        Require(Item(session, BlackSlash).GoldCost == 250 && Item(session, RedSlash).GoldCost == 250,
            "The real Draw Two starts the independent card category's 100-coin surcharge.");
        Buy(session, BlackSlash, 250);
        Buy(session, RedSlash, 350);
        Require(session.Snapshot.Gold == 1250 && session.Snapshot.ShopItems.All(item => item.IsPurchased),
            "Five actual purchases must debit 600 + 300 + 250 + 250 + 350 from 3000 coins.");

        Accept(session, new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
        Accept(session, new MoveShanheNodeCommand("battle", session.Revision));
        var setup = session.PendingBattleSetup!;
        ShanheStartingCard[] equipment = [new(CardKind.Crossbow, Suit.Diamond, 1), new(CardKind.SilverLion, Suit.Club, 1)];
        ShanheStartingCard[] cards = [new(CardKind.DrawTwo, Suit.Diamond, 10), new(CardKind.Slash, Suit.Spade, 1), new(CardKind.Slash, Suit.Heart, 1)];
        Require(setup.StartingResourcesExpireOnUse && setup.StartingEquipment.SequenceEqual(equipment) && setup.StartingCards.SequenceEqual(cards) &&
            setup.SlashBonus == 0 && setup.ExtraSkillIds.Count == 0 && setup.TacticSkillIds.Count == 0,
            "Leaving the real shop must carry the exact purchased faces and expiring-resource contract into an unmodified Cao Cao battle.");

        var game = StandardShanheCampaignCatalog.CreateBattle(setup);
        var opening = game.CreateSnapshot(0).Players[0];
        Require(opening.Equipment.Select(card => new ShanheStartingCard(card.Kind, card.Suit, card.Rank)).ToHashSet().SetEquals(equipment) &&
            game.CreateSnapshot(0).Players[1].HandCount == 0,
            "Purchased equipment is installed directly with its exact faces while the real Footman begins empty-handed.");
        Accept(game, new StartGameCommand());
        var origins = game.Events.Select(entry => entry.Payload).OfType<ShanheStartingResourceCreatedEvent>().ToArray();
        Require(origins.Length == 5 && origins.Select(origin => origin.CardId).Distinct().Count() == 5,
            "Every purchased resource must create its own genuine opening physical entity.");
        int Initial(bool isEquipment, int index) => origins.Single(origin => origin.EquipmentResource == isEquipment && origin.RecipeIndex == index).CardId;
        foreach (var origin in origins)
        {
            var face = opening.Hand.Concat(opening.Equipment).Single(card => card.Id == origin.CardId);
            Require(new ShanheStartingCard(face.Kind, face.Suit, face.Rank) ==
                (origin.EquipmentResource ? equipment : cards)[origin.RecipeIndex],
                "Each original hand or equipment entity preserves its purchased kind, suit, rank and recipe index.");
        }

        ReachPlay(game);
        var turn = game.CreateSnapshot(0).TurnNumber;
        foreach (var id in new[] { Initial(true, 0), Initial(true, 1) })
        {
            Require(Zone(game, id) == CardLocation.Equipment(0) &&
                game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.OutsideGame &&
                    move.To == CardLocation.Equipment(0) && move.Reason == CardMoveReasons.EquipmentEnter) == 1 &&
                !game.Events.Any(entry => entry.Payload is CardUseDeclaredEvent use && use.CardId == id),
                "A purchased equipment entity installs exactly once before play without fabricating a card-use declaration.");
        }
        var drawTwo = Initial(false, 0);
        var beforeHand = game.CreateSnapshot(0).Players[0].HandCount;
        var movementCount = game.CardMovements.Count;
        Play(game, drawTwo, []);
        ReachPlay(game);
        Require(game.CreateSnapshot(0).Players[0].HandCount == beforeHand + 1 &&
            game.CardMovements.Skip(movementCount).Count(move => move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw) == 2 &&
            Zone(game, drawTwo) == CardLocation.OutsideGame &&
            game.Events.Select(entry => entry.Payload).OfType<ShanheStartingResourceDestroyedEvent>().Count(fact => fact.CardId == drawTwo) == 1 &&
            game.CardMovements.All(move => move.CardId != drawTwo || move.To != CardLocation.DiscardPile),
            "The purchased Draw Two must really draw two cards and move its spent original outside the game exactly once.");

        var enemyHp = game.CreateSnapshot(0).Players[1].Hp;
        foreach (var slash in new[] { Initial(false, 1), Initial(false, 2) })
        {
            Require(game.CreateSnapshot(0).Players[1].HandCount == 0 &&
                game.GetHumanLegalActions().Any(action => action.Kind == LegalActionKind.Slash && action.CardId == slash && action.TargetSeats.SequenceEqual([1])),
                "The purchased Crossbow must expose each bought Slash as a real legal attack in this same play phase.");
            Play(game, slash, [1]);
            ReachPlay(game);
            enemyHp--;
            Require(game.CreateSnapshot(0) is { CurrentSeat: 0 } view && view.TurnNumber == turn && view.Players[1].Hp == enemyHp &&
                Zone(game, slash) == CardLocation.OutsideGame,
                "Each bought Slash must really deal one damage and expire, including the second native Slash in the same turn.");
        }
        Require(game.CreateSnapshot(0).Players[0].Equipment.Select(card => card.Id).ToHashSet()
            .SetEquals([Initial(true, 0), Initial(true, 1)]),
            "The exact bought Crossbow and Silver Lion must remain installed after both real attacks.");
    }

    private static ShanheShopItemSnapshot Item(ShanheCampaignSession session, string id) =>
        session.Snapshot.ShopItems.Single(item => item.Reward?.Id == id);

    private static void Buy(ShanheCampaignSession session, string id, int cost)
    {
        var item = Item(session, id);
        Require(item.CanPurchase && item.GoldCost == cost &&
            session.Snapshot.NodeChoices.Single(choice => choice.Id == item.ChoiceId).GoldCost == cost,
            "The real purchasable command and published stock must agree on the current debit for " + id + ".");
        var gold = session.Snapshot.Gold;
        Accept(session, new ChooseShanheNodeOptionCommand(item.ChoiceId, null, session.Revision));
        Require(session.Snapshot.Gold == gold - cost && Item(session, id).IsPurchased,
            "A real purchase must spend exactly its displayed price once and mark the product as purchased.");
    }

    private static CardLocation Zone(GameEngine game, int id) => game.CreateCardZoneDiagnostics().Single(card => card.CardId == id).Location;

    private static void Play(GameEngine game, int cardId, IReadOnlyList<int> targets)
    {
        var action = game.GetHumanLegalActions().First(action => action.CardId == cardId && action.ConversionSource is null && action.TargetSeats.SequenceEqual(targets));
        Accept(game, new PlayCardCommand(0, cardId, targets, game.Revision, game.PendingDecision!.PromptId, action.PlayedCardKind, action.TargetCardId));
    }

    private static void ReachPlay(GameEngine game)
    {
        for (var step = 0; step < 160; step++)
        {
            if (game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
            if (game.PendingDecision is { Kind: DecisionKind.Nullification } prompt)
                Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(choice => choice.Cards.Count == 0).Id, game.Revision));
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The purchased-card battle did not return to Cao Cao's play phase.");
    }

    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Require(result.Accepted, result.Error ?? "Shop-stock campaign command rejected."); }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Require(result.Accepted, result.Error?.Message ?? "Shop-stock battle command rejected."); }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
