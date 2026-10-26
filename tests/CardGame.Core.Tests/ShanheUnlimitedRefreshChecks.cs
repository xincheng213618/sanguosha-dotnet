using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheUnlimitedRefreshChecks
{
    private const string Unlimited = StandardShanheCampaignCatalog.CurrentResourceSetRewardId;
    private const string Member = "tactic-economy-2107", Free = "tactic-economy-2291", Restock = "tactic-economy-2252";

    public static void BuyingResourceSetUnlocksMoreThanTwentyPaidRefreshesAtItsRealPrice()
    {
        var source = StandardShanheCampaignCatalog.CurrentEconomyTactics.Single(item => item.SourceId == 2230);
        var definition = Definition(12_699);
        var session = Enter(definition);
        Need(!session.Snapshot.ShopPaidRefreshUnlimited && session.Snapshot.TacticIds.Count == 0,
            "The source row and local default availability do not grant the resource set to a new expedition.");
        Accept(session, new RefreshShanheShopCommand(session.Revision));
        Need(session.Snapshot.Gold == 12_599 && session.Snapshot.ShopPaidRefreshRemaining == 0,
            "The ordinary paid allowance is really spent before acquiring the resource set.");
        Buy(session, session.Snapshot.ShopItems.First(item => item.Reward?.Id.StartsWith("card-", StringComparison.Ordinal) == true), 100);
        var oldQuote = session.Snapshot;
        var slots = oldQuote.ExtraSkillSlots;
        Buy(session, session.Snapshot.ShopItems.Single(item => item.Reward?.Id == Unlimited), 9999);
        Need(source is { Price: 9999, Rarity: 4, Description: "商店可无限刷新" } &&
            session.Snapshot.Gold == 2500 && session.Snapshot.TacticIds.SequenceEqual([Unlimited]) &&
            session.Snapshot.ExtraSkillSlots == slots && session.Snapshot.ExtraSkillIds.Count == 0 &&
            !oldQuote.ShopPaidRefreshUnlimited,
            "The actual source purchase spends 9999 once, grants only the economic tactic and leaves the old quote unchanged.");
        var seen = new HashSet<string>(StringComparer.Ordinal);
        for (var used = 1; used <= 25; used++)
        {
            UnlimitedBudget(session, 0, 100);
            Accept(session, new RefreshShanheShopCommand(session.Revision));
            Need(session.Snapshot.Gold == 2500 - used * 100 &&
                session.Snapshot.ShopItems.All(item => item.ChoiceId.StartsWith("$shop:" + (used + 1) + ":", StringComparison.Ordinal) &&
                    seen.Add(item.ChoiceId) && item.GoldCost == 200),
                "Every real paid refresh, including those beyond five and twenty, spends 100, advances stock identities and retains both category surcharges.");
            Need(session.Snapshot.ShopItems.All(item => item.Reward?.Id != "unsupported-bundle"),
                "A real 9999 economy source does not make unsupported 9999 placeholder bundles available.");
        }
        UnlimitedBudget(session, 0, 100);
        RejectUnchanged(session);
        Accept(session, new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
        Need(!session.Snapshot.ShopPaidRefreshUnlimited, "Infinity is a quote for the active random shop, not a map action.");
        Accept(session, new MoveShanheNodeCommand("next", session.Revision));
        UnlimitedBudget(session, 0, 100);
        Need(session.Snapshot.ShopItems.All(item => item.ChoiceId.StartsWith("$shop:0:", StringComparison.Ordinal) && item.GoldCost == 100),
            "The next shop resets stock generation and category purchases while retaining the held unlimited ability.");
        RejectUnchanged(session);

        var current = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var reward = current.Rewards.Single(item => item.Id == Unlimited);
        Need(reward.Rarity == 4 && reward.CampaignEconomy?.UnlimitedPaidShopRefreshes == true &&
            StandardShanheCampaignCatalog.GetDefaultCodexUnlockedRewardIds(current).Contains(Unlimited) &&
            StandardShanheCampaignCatalog.ShopMetadataByRewardId[Unlimited] is { Price: 9999, Rarity: 4 } &&
            current.Nodes.Where(node => node.Kind == ShanheNodeKind.Shop).All(node =>
                node.Choices!.Any(choice => choice.RewardId == Unlimited && choice.GoldCost == 9999)),
            "The playable current catalogue carries the exact-price source into default local supply and all real merchant definitions.");
    }

    public static void UnlimitedRefreshesKeepFreePriorityAndFiniteMembershipBoundaries()
    {
        var definition = Definition(0);
        var free = Enter(definition, Unlimited, Free, Member);
        for (var remaining = 2; remaining > 0; remaining--)
        {
            UnlimitedBudget(free, remaining, 0);
            Accept(free, new RefreshShanheShopCommand(free.Revision));
            Need(free.Snapshot.Gold == 0, "Held free refreshes remain usable with zero copper before unlimited paid refreshes.");
        }
        UnlimitedBudget(free, 0, 100);
        RejectUnchanged(free);
        var shortWallet = Enter(Definition(99), Unlimited);
        UnlimitedBudget(shortWallet, 0, 100);
        RejectUnchanged(shortWallet);
        var fixedStock = ShanheCampaignSession.Create(definition, new(71, ShanheCampaignChecks.Player)
            { EquippedTacticIds = [Unlimited] }, ShanheCampaignChecks.Registry);
        Accept(fixedStock, new MoveShanheNodeCommand("shop", fixedStock.Revision));
        Need(!fixedStock.Snapshot.ShopPaidRefreshUnlimited && fixedStock.Snapshot.ShopRefreshRemaining == 0,
            "A held resource set does not turn a fixed-stock merchant into a refreshable random shop.");
        RejectUnchanged(fixedStock);
        foreach (var held in new[] { Array.Empty<string>(), new[] { Member } })
        {
            var finite = Enter(Definition(1000), held);
            var limit = held.Length == 0 ? 1 : 5;
            for (var remaining = limit; remaining > 0; remaining--)
            {
                Need(!finite.Snapshot.ShopPaidRefreshUnlimited && finite.Snapshot.ShopPaidRefreshRemaining == remaining &&
                    finite.Snapshot.ShopRefreshRemaining == remaining,
                    "Unowned resource sets preserve the ordinary one-use and membership five-use paid allowances.");
                Accept(finite, new RefreshShanheShopCommand(finite.Revision));
            }
            Need(finite.Snapshot.Gold == 1000 - limit * 100, "Finite refreshes still pay exactly 100 for every allowed use.");
            RejectUnchanged(finite);
        }
    }

    public static void ExactPriceEconomyStockParticipatesInRestockAndQualityPromotion()
    {
        var source = Source(2230);
        // Eight tactic identities at the existing verified seed 71 sample positions 2,6,7,3.
        // The first source is therefore absent until a real quality roll or replenishment selects it.
        var fillers = Enumerable.Range(1, 7).Select(index => new ShanheRewardDefinition("stock-" + index, "经济货品" + index)
            { Rarity = 1, CampaignEconomy = new(FreeShopRefreshes: 1) }).ToArray();
        var quality = new ShanheRewardDefinition("forced-quality", "定向品质检查")
            { CampaignEconomy = new(ShopTacticPromotionPercent: 100) };
        var goods = new[] { source }.Concat(fillers).ToArray();
        var definition = ShopDefinition(goods.Concat([Source(2252), quality]).ToArray(), goods.Select(reward =>
            new ShanheNodeChoiceDefinition("buy-" + reward.Id, reward.Name, reward.Id == Unlimited ? 9999 : 100,
                RewardId: reward.Id, LeavesNode: false)).ToArray(), 50_000);
        var baseline = Enter(definition);
        Need(baseline.Snapshot.ShopItems.All(item => item.Reward?.Id != Unlimited),
            "The representative baseline genuinely leaves the exact-price economy source outside the initial shelf.");
        var promoted = Enter(definition, "forced-quality", Restock);
        var resource = promoted.Snapshot.ShopItems.Single(item => item.Reward?.Id == Unlimited);
        Buy(promoted, resource, 9999);
        Need(promoted.Snapshot.ShopItems.Any(item => item.ChoiceId.StartsWith("$restock:0:1:", StringComparison.Ordinal)) &&
            promoted.Snapshot.ShopItems.All(item => !item.IsPurchased && item.Reward?.Id != Unlimited),
            "The forced higher-tier roll offers the real 9999 source, and purchasing it replenishes its former position through the same stock-price contract.");
        var replenished = Enter(definition, Restock);
        var purchases = 0;
        while (replenished.Snapshot.ShopItems.All(item => item.Reward?.Id != Unlimited) && purchases < fillers.Length)
        {
            Buy(replenished, replenished.Snapshot.ShopItems.First(item => item.CanPurchase), 100 + purchases * 100);
            purchases++;
        }
        Need(replenished.Snapshot.ShopItems.Any(item => item.Reward?.Id == Unlimited &&
                item.ChoiceId.StartsWith("$restock:", StringComparison.Ordinal) && item.GoldCost == 9999 + purchases * 100),
            "Replenishment candidates can supply the true-price economy source after ordinary goods are bought, retaining same-category purchase surcharges.");
    }

    private static ShanheCampaignDefinition Definition(int gold)
    {
        var cards = Enumerable.Range(1, 6).Select(index => new ShanheRewardDefinition("card-" + index, "军资" + index,
            StartingCards: [new(CardKind.Dodge, Rank: index)])).ToArray();
        var training = new ShanheRewardDefinition("training", "行军操练", Bonuses: new(MaxHp: 1));
        var unsupported = new ShanheRewardDefinition("unsupported-bundle", "未接入组合", StartingCards: [new(CardKind.Slash)]);
        var goods = new[] { Source(2230), training }.Concat(cards).Append(unsupported).ToArray();
        return ShopDefinition(goods.Concat([Source(2291), Source(2107)]).ToArray(), goods.Select(reward =>
            new ShanheNodeChoiceDefinition("buy-" + reward.Id, reward.Name,
                reward.Id is Unlimited or "unsupported-bundle" ? 9999 : 100,
                RewardId: reward.Id, LeavesNode: false)).ToArray(), gold);
    }
    private static ShanheCampaignDefinition ShopDefinition(ShanheRewardDefinition[] rewards, ShanheNodeChoiceDefinition[] choices, int gold) =>
        new("fixture:unlimited-shop-refresh", "资源流商店", [ShanheCampaignChecks.Player], ["shop"],
            [new("shop", "第一店", ShanheNodeKind.Shop, ["next"], Choices: choices),
             new("next", "第二店", ShanheNodeKind.Shop, ["battle"], Choices: choices),
             new("battle", "终点", ShanheNodeKind.Battle, [], [new("guard", "守军", [new(ShanheCampaignChecks.Enemy)])])],
            rewards, StartingGold: gold);
    private static ShanheRewardDefinition Source(int id)
    {
        var source = StandardShanheCampaignCatalog.CurrentEconomyTactics.Single(item => item.SourceId == id);
        return new(source.RewardId, source.Name) { CampaignEconomy = source.Effect, Rarity = source.Rarity, Description = source.Description };
    }
    private static ShanheCampaignSession Enter(ShanheCampaignDefinition definition, params string[] tactics)
    {
        var session = ShanheCampaignSession.Create(definition, new(71, ShanheCampaignChecks.Player)
            { UseCodexRewardPool = true, EquippedTacticIds = tactics }, ShanheCampaignChecks.Registry);
        Accept(session, new MoveShanheNodeCommand("shop", session.Revision));
        return session;
    }
    private static void UnlimitedBudget(ShanheCampaignSession session, int free, int cost) => Need(
        session.Snapshot.ShopPaidRefreshUnlimited && session.Snapshot.ShopPaidRefreshRemaining == 0 &&
        session.Snapshot.ShopFreeRefreshRemaining == free && session.Snapshot.ShopRefreshRemaining == free &&
        session.Snapshot.ShopRefreshCost == cost, "Unlimited paid refreshes have an explicit flag, finite free quota and actual next-use price.");
    private static void Buy(ShanheCampaignSession session, ShanheShopItemSnapshot item, int cost)
    {
        Need(item.CanPurchase && item.GoldCost == cost, "An actual economy purchase must use its published price and availability.");
        Accept(session, new ChooseShanheNodeOptionCommand(item.ChoiceId, null, session.Revision));
    }
    private static void RejectUnchanged(ShanheCampaignSession session)
    {
        var before = session.Snapshot;
        Need(!session.Submit(new RefreshShanheShopCommand(session.Revision)).Accepted && ReferenceEquals(before, session.Snapshot),
            "An unaffordable or exhausted refresh cannot pay, consume quota or regenerate stock.");
    }
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Rejected real unlimited-refresh command."); }
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
}
