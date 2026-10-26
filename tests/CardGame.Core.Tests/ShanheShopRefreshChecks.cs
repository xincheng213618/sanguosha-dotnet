using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheShopRefreshChecks
{
    private const string FreeTwo = "tactic-economy-2291", FreeFour = "tactic-economy-2292", Member = "tactic-economy-2107";

    public static void FreeRefreshesStackBeforeMembershipPaidAllowanceAndNeedNoCopper()
    {
        var definition = Definition(500);
        var ordinary = Create(definition);
        Enter(ordinary, "shop");
        Budget(ordinary, 0, 1, 100);
        Accept(ordinary, new RefreshShanheShopCommand(ordinary.Revision));
        Budget(ordinary, 0, 0, 100);
        Need(ordinary.Snapshot.Gold == 400, "The unowned default still pays one hundred for exactly one refresh.");
        RejectUnchanged(ordinary, new RefreshShanheShopCommand(ordinary.Revision));

        var session = Create(definition, FreeTwo, FreeFour, Member);
        Enter(session, "shop");
        Budget(session, 6, 5, 0);
        var first = session.Snapshot;
        var ids = new HashSet<string>(StringComparer.Ordinal);
        RecordStock(session, 0, ids);
        for (var index = 0; index < 6; index++)
        {
            Accept(session, new RefreshShanheShopCommand(session.Revision));
            Budget(session, 5 - index, 5, index < 5 ? 0 : 100);
            Need(session.Snapshot.Gold == 500, "All six held free refreshes preserve both the wallet and every membership paid opportunity.");
            RecordStock(session, index + 1, ids);
        }
        Need(first.ShopFreeRefreshRemaining == 6 && first.ShopPaidRefreshRemaining == 5 && first.ShopRefreshCost == 0,
            "Previously published refresh quotes remain frozen as the live allowance is consumed.");
        RejectUnchanged(session, new RefreshShanheShopCommand(first.Revision));
        for (var index = 0; index < 5; index++)
        {
            Accept(session, new RefreshShanheShopCommand(session.Revision));
            Budget(session, 0, 4 - index, 100);
            Need(session.Snapshot.Gold == 400 - index * 100, "After free exhaustion each membership refresh spends exactly one hundred copper.");
            RecordStock(session, index + 7, ids);
        }
        Need(session.Snapshot.Gold == 0, "The last legal paid refresh may spend the final hundred copper.");
        RejectUnchanged(session, new RefreshShanheShopCommand(session.Revision));

        var broke = Create(Definition(0), FreeTwo, FreeFour);
        Enter(broke, "shop");
        for (var index = 0; index < 6; index++)
        {
            Budget(broke, 6 - index, 1, 0);
            Accept(broke, new RefreshShanheShopCommand(broke.Revision));
            Need(broke.Snapshot.Gold == 0, "Every free refresh remains executable with a zero-copper wallet.");
        }
        Budget(broke, 0, 1, 100);
        RejectUnchanged(broke, new RefreshShanheShopCommand(broke.Revision));

        var fixedStock = ShanheCampaignSession.Create(definition,
            new(71, ShanheCampaignChecks.Player) { EquippedTacticIds = [FreeTwo, FreeFour] }, ShanheCampaignChecks.Registry);
        Enter(fixedStock, "shop");
        Budget(fixedStock, 0, 0, 0);
        RejectUnchanged(fixedStock, new RefreshShanheShopCommand(fixedStock.Revision));

        foreach (var invalid in new[] { -1, 21 })
        {
            try
            {
                _ = ShanheCampaignContent.FreezeAndValidate(definition with
                {
                    Rewards = definition.Rewards.Append(new("invalid-free", "越界免费刷新")
                        { CampaignEconomy = new(FreeShopRefreshes: invalid) }).ToArray()
                });
            }
            catch (ArgumentException) { continue; }
            throw new InvalidOperationException("Free refresh definitions must reject negative or more-than-twenty per-source allowances.");
        }
    }

    public static void MidShopAcquisitionAddsFreeUsesAndNextShopResetsEveryCounter()
    {
        var session = Create(Definition(1100));
        Enter(session, "shop");
        Accept(session, new RefreshShanheShopCommand(session.Revision));
        Budget(session, 0, 0, 100);
        var card = session.Snapshot.ShopItems.First(item => item.Reward?.Id.StartsWith("card-", StringComparison.Ordinal) == true);
        Need(card.GoldCost == 100, "The first ordinary card purchase keeps its own authored price.");
        Accept(session, new ChooseShanheNodeOptionCommand(card.ChoiceId, null, session.Revision));
        Buy(session, FreeTwo, 300);
        Budget(session, 2, 0, 0);
        var twoQuote = session.Snapshot;
        var ids = new HashSet<string>(StringComparer.Ordinal);
        RecordStock(session, 1, ids);
        Accept(session, new RefreshShanheShopCommand(session.Revision));
        Budget(session, 1, 0, 0);
        RecordStock(session, 2, ids);
        RejectUnchanged(session, new ChooseShanheNodeOptionCommand(card.ChoiceId, null, session.Revision));
        Buy(session, FreeFour, 600);
        Budget(session, 5, 0, 0);
        Need(session.Snapshot.Gold == 0 && twoQuote.ShopFreeRefreshRemaining == 2 &&
            twoQuote.ShopItems.Single(item => item.ChoiceId == card.ChoiceId).IsPurchased,
            "Buying both real source tactics after the paid allowance is spent adds 2+4 current-shop uses without refunding the free refresh already used, while category pricing, purchase history and the old quote remain intact.");
        for (var index = 0; index < 5; index++)
        {
            Accept(session, new RefreshShanheShopCommand(session.Revision));
            Budget(session, 4 - index, 0, index < 4 ? 0 : 100);
            Need(session.Snapshot.Gold == 0 && session.Snapshot.ShopItems.All(item => item.GoldCost == 200),
                "A zero-copper free refresh preserves the earlier card-category surcharge instead of resetting shop purchases.");
            RecordStock(session, index + 3, ids);
        }
        RejectUnchanged(session, new RefreshShanheShopCommand(session.Revision));
        Accept(session, new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
        Budget(session, 0, 0, 0);
        Enter(session, "next");
        Budget(session, 6, 1, 0);
        Need(session.Snapshot.ShopItems.All(item => item.GoldCost == 100 && !item.IsPurchased),
            "The next real shop resets free and paid usage plus category and purchase histories, while retaining the held source tactics.");
        RecordStock(session, 0, new HashSet<string>(StringComparer.Ordinal));
        Accept(session, new RefreshShanheShopCommand(session.Revision));
        Budget(session, 5, 1, 0);
        Need(session.Snapshot.Gold == 0, "The reset second-shop free allowance remains executable without copper.");
    }

    private static ShanheCampaignDefinition Definition(int gold)
    {
        var sources = StandardShanheCampaignCatalog.CurrentEconomyTactics.Where(source => source.SourceId is 2291 or 2292 or 2107).ToArray();
        var economy = sources.Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
            { CampaignEconomy = source.Effect, Rarity = source.Rarity, Description = source.Description }).ToArray();
        var cards = Enumerable.Range(1, 4).Select(index => new ShanheRewardDefinition("card-" + index, "军资" + index,
            StartingCards: [new(CardKind.Dodge, Rank: index)])).ToArray();
        var choices = sources.Where(source => source.SourceId is 2291 or 2292)
            .Select(source => new ShanheNodeChoiceDefinition("buy-" + source.SourceId, source.Name,
                source.Price, RewardId: source.RewardId, LeavesNode: false))
            .Concat(cards.Select(reward => new ShanheNodeChoiceDefinition("buy-" + reward.Id, reward.Name,
                100, RewardId: reward.Id, LeavesNode: false))).ToArray();
        return new("fixture:free-shop-refresh", "免费刷新", [ShanheCampaignChecks.Player], ["shop"],
            [new("shop", "第一店", ShanheNodeKind.Shop, ["next"], Choices: choices),
             new("next", "第二店", ShanheNodeKind.Shop, ["battle"], Choices: choices),
             new("battle", "终点", ShanheNodeKind.Battle, [], [new("guard", "守军", [new(ShanheCampaignChecks.Enemy)])])],
            economy.Concat(cards).ToArray(), StartingGold: gold);
    }
    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition, params string[] tactics) =>
        ShanheCampaignSession.Create(definition, new(71, ShanheCampaignChecks.Player)
            { UseCodexRewardPool = true, EquippedTacticIds = tactics }, ShanheCampaignChecks.Registry);
    private static void Enter(ShanheCampaignSession session, string id) => Accept(session, new MoveShanheNodeCommand(id, session.Revision));
    private static void Buy(ShanheCampaignSession session, string reward, int price)
    {
        var item = session.Snapshot.ShopItems.Single(item => item.Reward?.Id == reward);
        Need(item.GoldCost == price, "The real source tactic purchase must pay the quoted price including earlier tactic purchases.");
        Accept(session, new ChooseShanheNodeOptionCommand(item.ChoiceId, null, session.Revision));
    }
    private static void Budget(ShanheCampaignSession session, int free, int paid, int cost) => Need(
        session.Snapshot.ShopFreeRefreshRemaining == free && session.Snapshot.ShopPaidRefreshRemaining == paid &&
        session.Snapshot.ShopRefreshRemaining == free + paid && session.Snapshot.ShopRefreshCost == cost,
        "The published remaining total and next actual price must equal the independent free and paid allowances.");
    private static void RecordStock(ShanheCampaignSession session, int generation, HashSet<string> seen)
    {
        var ids = session.Snapshot.ShopItems.Select(item => item.ChoiceId).ToArray();
        Need(ids.Length > 0 && ids.All(id => id.StartsWith("$shop:" + generation + ":", StringComparison.Ordinal) && seen.Add(id)),
            "Each accepted free or paid refresh advances the same stock generation and never reuses a first-shop choice identity.");
    }
    private static void RejectUnchanged(ShanheCampaignSession session, ShanheCampaignCommand command)
    {
        var before = session.Snapshot;
        Need(!session.Submit(command).Accepted && ReferenceEquals(before, session.Snapshot),
            "A stale, unaffordable, unavailable or exhausted command cannot pay, consume allowance or reroll stock.");
    }
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Rejected real free-refresh command."); }
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
}
