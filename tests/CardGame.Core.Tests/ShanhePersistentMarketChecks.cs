using CardGame.Core;
using CardGame.Core.Campaigns;
using MarketPurchaseCommand = CardGame.Core.Campaigns.BuyShanheMarketItemCommand;

internal static class ShanhePersistentMarketChecks
{
    public static void ChapterShelfSurvivesMapChoicesAndOnlyResetsAcrossChapters()
    {
        var session = Create();
        Need(session.HasPersistentMarket && session.Snapshot.Phase == ShanheCampaignPhase.Map &&
            session.Snapshot.AvailableNodes.Select(node => node.Id).SequenceEqual(["event-one"]) &&
            session.VisibleMapNodes.All(node => node.Kind != ShanheNodeKind.Shop) &&
            session.VisibleMapNodes.Single(node => node.Id == "event-one").NextNodeIds.SequenceEqual(["event-two"]) &&
            session.Snapshot.ShopItems.Count == 5,
            "A new persistent expedition exposes five real market products while hidden shop templates are transparently removed from the playable map.");
        var first = session.Snapshot.ShopItems.First();
        var quoted = session.Snapshot;
        Need(session.ValidateMarketPurchase(first.ChoiceId, null) is null && ReferenceEquals(quoted, session.Snapshot),
            "A native purchase preview prepares the same legal transaction without issuing a command or mutating its quote.");
        Accept(session, new MarketPurchaseCommand(first.ChoiceId, null, session.Revision));
        Accept(session, new RefreshShanheShopCommand(session.Revision));
        var shelf = session.Snapshot;
        Need(shelf.Gold == 800 && shelf.StartingCards.Count == 1 && shelf.ShopPaidRefreshRemaining == 0 &&
            shelf.ShopItems.All(item => item.GoldCost == 200),
            "An actual map purchase and refresh spend once, preserve the acquired recipe and advance this chapter's card surcharge.");
        Accept(session, new MoveShanheNodeCommand("event-one", session.Revision));
        Need(session.Snapshot.Phase == ShanheCampaignPhase.NodeChoice &&
            session.Snapshot.ShopItems.Select(item => item.ChoiceId).SequenceEqual(shelf.ShopItems.Select(item => item.ChoiceId)) &&
            session.Snapshot.ShopPaidRefreshRemaining == 0 && session.ValidateMarketRefresh() is not null,
            "Moving or closing and reopening a market cannot reset this chapter's real shelf or spent refresh allowance.");
        var second = session.Snapshot.ShopItems.First(item => item.CanPurchase);
        Accept(session, new MarketPurchaseCommand(second.ChoiceId, null, session.Revision));
        Need(session.Snapshot.Gold == 600 && session.Snapshot.StartingCards.Count == 2 &&
            session.Snapshot.ShopItems.All(item => item.GoldCost == 300),
            "The same market remains purchasable during a real event choice without replacing the event or resetting same-category prices.");
        Accept(session, new ChooseShanheNodeOptionCommand("continue", null, session.Revision));
        Accept(session, new MoveShanheNodeCommand("event-two", session.Revision));
        Need(session.Snapshot.Gold == 600 && session.Snapshot.ShopPaidRefreshRemaining == 0 &&
            session.Snapshot.ShopItems.Single(item => item.ChoiceId == second.ChoiceId).IsPurchased,
            "Ordinary movement within the chapter retains the purchased positions and wallet.");
        Accept(session, new ChooseShanheNodeOptionCommand("continue", null, session.Revision));
        Need(session.Snapshot.MarketChapterNumber == 2 && session.Snapshot.AvailableNodes.Select(node => node.Id).SequenceEqual(["event-three"]) &&
            session.Snapshot.ShopPaidRefreshRemaining == 1 && session.Snapshot.ShopItems.Count == 5 &&
            session.Snapshot.ShopItems.All(item => item.GoldCost == 100 && !item.IsPurchased) &&
            session.Snapshot.StartingCards.Count == 2 && session.Snapshot.Gold == 600,
            "Entering the next chapter resets only its shelf, refresh allowance and surcharge, retaining the purchased build and real wallet.");
    }

    public static void BattleMarketStartsFromFrozenStateWithoutCampaignHistoryOrDoublePayment()
    {
        var campaign = Create();
        var item = campaign.Snapshot.ShopItems.First();
        Accept(campaign, new MarketPurchaseCommand(item.ChoiceId, null, campaign.Revision));
        Accept(campaign, new MoveShanheNodeCommand("event-one", campaign.Revision));
        Accept(campaign, new ChooseShanheNodeOptionCommand("continue", null, campaign.Revision));
        Accept(campaign, new MoveShanheNodeCommand("event-two", campaign.Revision));
        Accept(campaign, new ChooseShanheNodeOptionCommand("continue", null, campaign.Revision));
        Accept(campaign, new MoveShanheNodeCommand("event-three", campaign.Revision));
        Accept(campaign, new ChooseShanheNodeOptionCommand("continue", null, campaign.Revision));
        Accept(campaign, new MoveShanheNodeCommand("battle", campaign.Revision));
        var setup = campaign.PendingBattleSetup?.MarketSetup ?? throw new InvalidOperationException("Missing actual battle market setup.");
        Need(setup.Definition.Nodes.Count == 2 && setup.State.Gold == 900 &&
            setup.State.StartingCards.Count == 1 && setup.Options.EquippedTacticIds.Count == 0,
            "The actual battle owns only a frozen two-node market catalogue and the already-paid wallet/build seed, without embedding historical commands.");
        var original = campaign.Snapshot;
        var isolated = ShanheCampaignSession.CreateBattleMarketSession(setup, ShanheCampaignChecks.Registry);
        Need(isolated.Revision == 0 && isolated.AcceptedCommands.Count == 0 && isolated.Snapshot.Gold == 900 &&
            isolated.Snapshot.StartingCards.Count == 1 && isolated.Snapshot.ShopItems.Count == 5,
            "Creating the isolated battle market does not replay old purchases or grant the old card twice.");
        var next = isolated.Snapshot.ShopItems.First(item => item.CanPurchase);
        Need(isolated.ValidateMarketPurchase(next.ChoiceId, null) is null, "The battle market uses the shared real-price preview.");
        Accept(isolated, new MarketPurchaseCommand(next.ChoiceId, null, isolated.Revision));
        var transfer = isolated.CreateBattleMarketTransfer();
        Need(transfer.Gold == 800 && transfer.StartingCards.Count == 2 && transfer.ShopCardPurchases == 1 &&
            ReferenceEquals(original, campaign.Snapshot) &&
            !campaign.Submit(new MarketPurchaseCommand(next.ChoiceId, null, campaign.Revision)).Accepted,
            "Only the isolated session spends the battle purchase; the outer campaign refuses concurrent shopping while its battle owns the market.");
        var marketRevision = isolated.Revision;
        var stableQuote = isolated.Snapshot;
        var available = stableQuote.ShopItems.First(product => !product.IsPurchased);
        var nativeHealthQuote = isolated.CreateBattleMarketSnapshot(0, 2);
        Need(nativeHealthQuote is { Hp: 0, MaxHp: 2, Gold: 800 } &&
            isolated.ValidateMarketPurchase(available.ChoiceId, null, 0, 2) is null &&
            isolated.ValidateMarketPurchase("not-on-this-shelf", null, 1, 2) is not null &&
            ReferenceEquals(stableQuote, isolated.Snapshot) && isolated.Revision == marketRevision,
            "A zero-health no-HP-cost quote remains eligible while detached quotations and rejected offers cannot replace the live market quote, change its wallet or advance its journal.");
        isolated.SynchronizeBattleMarketHealth(0, 2);
        Need(isolated.Snapshot is { Hp: 0, MaxHp: 2, Gold: 800 } && isolated.Revision == marketRevision &&
            isolated.ValidateMarketPurchase(isolated.Snapshot.ShopItems.First(item => !item.IsPurchased).ChoiceId, null) is null,
            "A zero-HP native health projection permits no-HP-cost goods without spending the wallet or advancing the transaction journal during preview.");
        isolated.SynchronizeBattleMarketHealth(1, 2);
        Need(isolated.Snapshot is { Hp: 1, MaxHp: 2, Gold: 800 } && isolated.Snapshot.Bonuses.MaxHp == 0 && isolated.Revision == marketRevision,
            "Current battle health and maximum are a synchronized quotation view, not a permanent build bonus or another transaction.");
        var rejected = false;
        try { ShanheCampaignSession.CreateBattleMarketSession(setup with { State = setup.State with { Gold = -1 } }, ShanheCampaignChecks.Registry); }
        catch (ArgumentException) { rejected = true; }
        Need(rejected, "A forged invalid wallet cannot seed a battle transaction session.");
    }

    private static ShanheCampaignSession Create()
    {
        var cards = Enumerable.Range(1, 7).Select(index => new ShanheRewardDefinition("market-card-" + index,
            "军资" + index, StartingCards: [new(CardKind.Dodge, Rank: index)])).ToArray();
        var choices = cards.Select(card => new ShanheNodeChoiceDefinition("buy-" + card.Id, card.Name,
            GoldCost: 100, RewardId: card.Id, LeavesNode: false)).ToArray();
        var definition = new ShanheCampaignDefinition("fixture:persistent-market", "章节常驻市场", [ShanheCampaignChecks.Player], ["shop-one"],
        [
            new("shop-one", "首章商店模板", ShanheNodeKind.Shop, ["event-one"], Choices: choices) { ChapterNumber = 1 },
            new("event-one", "首章事件一", ShanheNodeKind.Event, ["shop-bridge"], Choices: [new("continue", "继续")]) { ChapterNumber = 1 },
            new("shop-bridge", "首章过路模板", ShanheNodeKind.Shop, ["event-two"], Choices: choices) { ChapterNumber = 1 },
            new("event-two", "首章事件二", ShanheNodeKind.Event, ["shop-two"], Choices: [new("continue", "继续")]) { ChapterNumber = 1 },
            new("shop-two", "次章商店模板", ShanheNodeKind.Shop, ["event-three"], Choices: choices) { ChapterNumber = 2 },
            new("event-three", "次章事件", ShanheNodeKind.Event, ["battle"], Choices: [new("continue", "继续")]) { ChapterNumber = 2 },
            new("battle", "最终守军", ShanheNodeKind.Battle, [], [new("guard", "守军", [new(ShanheCampaignChecks.Enemy)])]) { ChapterNumber = 2 }
        ], cards, StartingGold: 1000);
        return ShanheCampaignSession.Create(definition, new(71, ShanheCampaignChecks.Player)
            { UseCodexRewardPool = true, PersistentMarket = true }, ShanheCampaignChecks.Registry);
    }
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Rejected persistent market command."); }
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
}
