using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheShopRestockChecks
{
    private const string Restock = "tactic-economy-2252";
    private const string Guard = "fixture:shanhe-guard", Raid = "fixture:shanhe-raid";

    public static void RestockingKeepsPositionsPricesSuppliesAndRefreshBudgets()
    {
        var perks = Sources(2252, 2291, 2107, 2091);
        var growth = Enumerable.Range(1, 12).Select(index => new ShanheRewardDefinition("growth-" + index,
            "操练" + index, Bonuses: new(MaxHp: 1))).ToArray();
        var cards = Enumerable.Range(1, 6).Select(index => new ShanheRewardDefinition("card-" + index,
            "军资" + index, StartingCards: [new(CardKind.Dodge, Rank: index)])).ToArray();
        var medicine = new ShanheRewardDefinition("medicine", "军医包", Heal: 1);
        var choices = growth.Concat(cards).Select(reward => Choice(reward, 100))
            .Prepend(Choice(medicine, 35)).ToArray();
        var definition = Definition(perks.Concat(growth).Concat(cards).Append(medicine).ToArray(), choices, 1000);
        var session = Create(definition, perks.Select(reward => reward.Id).ToArray());
        Enter(session, "shop");
        Need(session.Snapshot.ShopRestockOnPurchase, "The held real 2252 source enables restocking in the active merchant.");
        Budget(session, 2, 5);
        var index = session.Snapshot.ShopItems.ToList().FindIndex(item => item.Reward?.Bonuses?.MaxHp > 0);
        var original = session.Snapshot;
        for (var purchase = 0; purchase < 2; purchase++)
        {
            var before = session.Snapshot;
            var product = before.ShopItems[index];
            Need(product.GoldCost == (100 + purchase * 100) * 9 / 10, "The real quote applies this category's surcharge and the existing ten-percent discount.");
            Buy(session, product);
            var after = session.Snapshot;
            SameOtherPositions(before, after, index);
            Need(after.Gold == before.Gold - product.GoldCost && after.Bonuses.MaxHp == before.Bonuses.MaxHp + 1 &&
                after.ShopItems[index].ChoiceId != product.ChoiceId && after.ShopItems[index].Reward!.Id != product.Reward!.Id &&
                after.ShopItems[index].GoldCost == (200 + purchase * 100) * 9 / 10 &&
                after.ShopItems.Where(item => item.Reward?.StartingCards?.Count > 0).All(item => item.GoldCost == 90),
                "A successful purchase pays once, grants the real growth once, and fills exactly its former position with another eligible same-category product at the next purchase price.");
            Need(after.ShopItems.Where(item => !item.IsPurchased && !item.IsAlreadyOwned)
                    .Select(item => item.Reward!.Id).Distinct().Count() == after.ShopItems.Count(item => !item.IsPurchased && !item.IsAlreadyOwned),
                "The new shelf cannot duplicate another still-listed acquisition identity.");
            Budget(session, 2, 5);
            RejectUnchanged(session, new ChooseShanheNodeOptionCommand(product.ChoiceId, null, session.Revision));
            RejectUnchanged(session, new ChooseShanheNodeOptionCommand(after.ShopItems[index].ChoiceId, null, before.Revision));
        }
        Need(original.Gold == 1000 && original.ShopItems[index].GoldCost == 90,
            "Published pre-purchase stock and prices stay detached from subsequent same-slot replacements.");

        var supplyIndex = session.Snapshot.ShopItems.ToList().FindIndex(item => item.Reward?.Id == medicine.Id);
        for (var purchase = 0; purchase < 2; purchase++)
        {
            var before = session.Snapshot;
            var product = before.ShopItems[supplyIndex];
            Need(product.GoldCost == 31, "Fixed supplies keep their authored base price, the existing discount and no category surcharge.");
            Buy(session, product);
            SameOtherPositions(before, session.Snapshot, supplyIndex);
            Need(session.Snapshot.Hp == before.Hp + 1 && session.Snapshot.Gold == before.Gold - 31 &&
                session.Snapshot.ShopItems[supplyIndex] is { CanPurchase: true, GoldCost: 31 } refill &&
                refill.ChoiceId != product.ChoiceId && refill.Reward!.Id == medicine.Id,
                "The only authored medicine can refill itself as a new paid product, preserving its position and performing an actual one-HP recovery on both purchases.");
            Budget(session, 2, 5);
        }
        var currentSupplyId = session.Snapshot.ShopItems[supplyIndex].ChoiceId;
        var beforeRefresh = session.Snapshot;
        Accept(session, new RefreshShanheShopCommand(session.Revision));
        Budget(session, 1, 5);
        Need(session.Snapshot.Gold == beforeRefresh.Gold && session.Snapshot.ShopItems[supplyIndex].ChoiceId == currentSupplyId &&
            session.Snapshot.ShopItems[supplyIndex].CanPurchase &&
            session.Snapshot.ShopItems.Where(item => item.Reward?.Bonuses?.MaxHp > 0).All(item => item.GoldCost == 270),
            "A real free whole-shop refresh consumes only its free quota, retains the replenished supply identity, and preserves all category price counters.");

        Accept(session, new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
        Need(!session.Snapshot.ShopRestockOnPurchase, "The restock merchant hint ends on leaving the merchant.");
        Enter(session, "next");
        Budget(session, 2, 5);
        Need(session.Snapshot.ShopItems.Single(item => item.Reward?.Id == medicine.Id).ChoiceId == "buy-medicine" &&
            session.Snapshot.ShopItems.All(item => !item.IsPurchased) &&
            session.Snapshot.ShopItems.Where(item => item.Reward?.Bonuses?.MaxHp > 0).All(item => item.GoldCost == 90),
            "The next merchant starts its own authored supply shelf and purchase prices instead of leaking replenished slots from the previous merchant.");
    }

    public static void BuyingRestockAndReplacingSkillsAreAtomicAndHonorUniqueStock()
    {
        var tactics = Sources(2252, 2091, 2107, 2136, 2012);
        var guard = new ShanheRewardDefinition("guard", "守御", SkillId: Guard);
        var alias = new ShanheRewardDefinition("guard-alias", "守御别名", SkillId: Guard);
        var raid = new ShanheRewardDefinition("raid", "突袭", SkillId: Raid);
        var locked = new ShanheRewardDefinition("locked", "未研习咆哮", SkillId: "standard:paoxiao");
        var rewards = tactics.Concat([guard, alias, raid, locked]).ToArray();
        var sourcePrices = StandardShanheCampaignCatalog.CurrentEconomyTactics.ToDictionary(source => source.RewardId, source => source.Price);
        var choices = rewards.Select(reward => Choice(reward, sourcePrices.GetValueOrDefault(reward.Id, 100))).ToArray();
        var definition = Definition(rewards, choices, 3000) with { ExtraSkillSlots = 1 };
        var session = Create(definition, [], rewards.Where(reward => reward.Id != locked.Id).Select(reward => reward.Id).ToArray());
        Enter(session, "shop");
        // A normal refresh deliberately prefers an unseen item if this small shelf did not initially include 2252.
        if (!session.Snapshot.ShopItems.Any(item => item.Reward?.Id == Restock))
            Accept(session, new RefreshShanheShopCommand(session.Revision));
        var before = session.Snapshot;
        Need(!before.ShopRestockOnPurchase && !before.ShopItems.Any(item => item.Reward?.Id == locked.Id) &&
            before.ShopItems.Count(item => item.Reward?.SkillId == Guard) == 1,
            "Unowned restock stays inactive, locked skills stay outside stock, and skill aliases share one shelf identity.");
        var restockIndex = before.ShopItems.ToList().FindIndex(item => item.Reward?.Id == Restock);
        var product = before.ShopItems[restockIndex];
        Buy(session, product);
        SameOtherPositions(before, session.Snapshot, restockIndex);
        Need(product.GoldCost == 300 && session.Snapshot.Gold == before.Gold - 300 && session.Snapshot.TacticIds.Contains(Restock) &&
            session.Snapshot.ShopRestockOnPurchase && session.Snapshot.ShopItems[restockIndex].ChoiceId != product.ChoiceId &&
            session.Snapshot.ShopItems[restockIndex].Reward!.Id != Restock && !before.ShopRestockOnPurchase,
            "Purchasing the actual restock tactic immediately replenishes its own position after acquisition; its unique source cannot be sold again.");
        Need(session.Snapshot.ShopRefreshRemaining == before.ShopRefreshRemaining,
            "Buying the restock perk itself cannot consume a whole-shop refresh.");

        var guardIndex = session.Snapshot.ShopItems.ToList().FindIndex(item => item.Reward?.SkillId == Guard);
        Buy(session, session.Snapshot.ShopItems[guardIndex]);
        Need(session.Snapshot.ExtraSkillIds.SequenceEqual([Guard]) && session.Snapshot.ShopItems[guardIndex].IsPurchased,
            "The last available Guard identity enters the real skill slot and its exhausted position remains an explicit purchased placeholder.");
        var raidIndex = session.Snapshot.ShopItems.ToList().FindIndex(item => item.Reward?.SkillId == Raid);
        var raidQuote = session.Snapshot.ShopItems[raidIndex];
        Need(raidQuote is { CanPurchase: true, RequiresSkillReplacement: true, GoldCost: 200 } &&
            raidQuote.ReplaceableSkillIds.SequenceEqual([Guard]), "The real second skill quote requires the owned slot's exact replacement and includes the skill-category surcharge.");
        RejectUnchanged(session, new ChooseShanheNodeOptionCommand(raidQuote.ChoiceId, null, session.Revision));
        RejectUnchanged(session, new ChooseShanheNodeOptionCommand(raidQuote.ChoiceId, "not-owned", session.Revision));
        var skillBefore = session.Snapshot;
        Buy(session, raidQuote, Guard);
        SameOtherPositions(skillBefore, session.Snapshot, raidIndex);
        Need(session.Snapshot.Gold == skillBefore.Gold - 200 && session.Snapshot.ExtraSkillIds.SequenceEqual([Raid]) &&
            session.Snapshot.ShopItems[raidIndex] is { GoldCost: 300, RequiresSkillReplacement: true } replacement &&
            replacement.ChoiceId != raidQuote.ChoiceId && replacement.Reward!.SkillId == Guard &&
            session.Snapshot.ShopItems.Count(item => !item.IsPurchased && item.Reward?.SkillId == Guard) == 1 &&
            !session.Snapshot.ShopItems.Any(item => !item.IsPurchased && item.Reward?.SkillId == Raid) &&
            session.Snapshot.ShopRefreshRemaining == skillBefore.ShopRefreshRemaining,
            "Only successful real slot replacement releases the old skill identity into the replacement position, at the next skill price and without duplicating an alias or the newly held skill.");
        RejectUnchanged(session, new ChooseShanheNodeOptionCommand(raidQuote.ChoiceId, Guard, session.Revision));
    }

    private static ShanheRewardDefinition[] Sources(params int[] ids) => StandardShanheCampaignCatalog.CurrentEconomyTactics
        .Where(source => ids.Contains(source.SourceId)).Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
            { CampaignEconomy = source.Effect, Rarity = source.Rarity, Description = source.Description }).ToArray();
    private static ShanheNodeChoiceDefinition Choice(ShanheRewardDefinition reward, int price) =>
        new("buy-" + reward.Id, reward.Name, price, RewardId: reward.Id, LeavesNode: false);
    private static ShanheCampaignDefinition Definition(IReadOnlyList<ShanheRewardDefinition> rewards,
        IReadOnlyList<ShanheNodeChoiceDefinition> choices, int gold) =>
        new("fixture:shop-restock", "真实原位补货", [ShanheCampaignChecks.Player], ["shop"],
            [new("shop", "第一店", ShanheNodeKind.Shop, ["next"], Choices: choices),
             new("next", "第二店", ShanheNodeKind.Shop, ["battle"], Choices: choices),
             new("battle", "终点", ShanheNodeKind.Battle, [], [new("guard", "守军", [new(ShanheCampaignChecks.Enemy)])])],
            rewards, StartingGold: gold);
    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition, IReadOnlyList<string> tactics,
        IReadOnlyList<string>? unlocked = null) => ShanheCampaignSession.Create(definition,
            new(71, ShanheCampaignChecks.Player) { UseCodexRewardPool = true, EquippedTacticIds = tactics, UnlockedRewardIds = unlocked },
            ShanheCampaignChecks.Registry);
    private static void Enter(ShanheCampaignSession session, string id) => Accept(session, new MoveShanheNodeCommand(id, session.Revision));
    private static void Buy(ShanheCampaignSession session, ShanheShopItemSnapshot item, string? replace = null) =>
        Accept(session, new ChooseShanheNodeOptionCommand(item.ChoiceId, replace, session.Revision));
    private static void Budget(ShanheCampaignSession session, int free, int paid) => Need(
        session.Snapshot.ShopFreeRefreshRemaining == free && session.Snapshot.ShopPaidRefreshRemaining == paid,
        "Same-position replenishment never spends an independent free or paid whole-shop refresh.");
    private static void SameOtherPositions(ShanheCampaignSnapshot before, ShanheCampaignSnapshot after, int replaced) => Need(
        before.ShopItems.Count == after.ShopItems.Count && Enumerable.Range(0, before.ShopItems.Count)
            .Where(index => index != replaced).All(index => before.ShopItems[index].ChoiceId == after.ShopItems[index].ChoiceId),
        "Every other shelf position and product identity must remain exactly where it was.");
    private static void RejectUnchanged(ShanheCampaignSession session, ShanheCampaignCommand command)
    {
        var before = session.Snapshot;
        Need(!session.Submit(command).Accepted && ReferenceEquals(before, session.Snapshot),
            "A retired, stale or invalid replacement command cannot charge, grant or refill any shelf position.");
    }
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Rejected real restock command."); }
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
}
