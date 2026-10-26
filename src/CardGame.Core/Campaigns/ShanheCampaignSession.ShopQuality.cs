namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private int ShopPromotionPercent(RunState state, ShopRewardCategory category) => Math.Min(100,
        OwnedEconomy(state).Sum(effect => category switch
        {
            ShopRewardCategory.Tactic => effect.ShopTacticPromotionPercent,
            ShopRewardCategory.Skill => effect.ShopSkillPromotionPercent,
            ShopRewardCategory.Equipment or ShopRewardCategory.Card => effect.ShopMaterialPromotionPercent,
            _ => 0
        }));

    // Local execution ruling: each newly generated position gets one independent chance to
    // reach the next available higher authored rarity. Published stock is never rolled again.
    private ShanheNodeChoiceDefinition PromoteShopItem(RunState state, ShanheNodeDefinition node,
        IReadOnlyList<ShanheNodeChoiceDefinition> stock, int position, int restockOrdinal = 0)
    {
        var baseline = stock[position];
        if (baseline.RewardId is not { } id || baseline.LeavesNode || !HasShopStockPrice(baseline)) return baseline;
        var original = _rewards[id];
        if (original.Rarity is < 1 or >= 4) return baseline;
        var category = ShopCategory(original);
        var chance = ShopPromotionPercent(state, category);
        if (chance == 0) return baseline;

        // Do not consume the ordinary stock/replenishment generator, or another category's roll.
        var random = new DeterministicRandom(ShanheBattleSetup.DeriveSeed(_options.Seed,
            $"{node.Id}:shop-quality:{category}:{state.ShopRefreshCount}:{position}", restockOrdinal));
        if (random.Next(100) >= chance) return baseline;
        var occupied = stock.Where((choice, index) => index != position && choice.RewardId is not null &&
                !state.PurchasedChoiceIds.Contains(choice.Id, StringComparer.Ordinal))
            .Select(choice => RewardAcquisitionIdentity(_rewards[choice.RewardId!])).ToHashSet(StringComparer.Ordinal);
        var originalIdentity = RewardAcquisitionIdentity(original);
        var candidates = (node.Choices ?? []).Where(choice => choice is { RewardId: not null, LeavesNode: false } &&
                HasShopStockPrice(choice) && _rewards[choice.RewardId!] is var reward &&
                ShopCategory(reward) == category && reward.Rarity > original.Rarity &&
                RewardAcquisitionIdentity(reward) != originalIdentity &&
                !occupied.Contains(RewardAcquisitionIdentity(reward)) &&
                CanReceiveReward(state, reward) && CanApplyBossReward(state, reward))
            .ToArray();
        if (candidates.Length == 0) return baseline;
        var nextRarity = candidates.Min(choice => _rewards[choice.RewardId!].Rarity);
        var tier = candidates.Where(choice => _rewards[choice.RewardId!].Rarity == nextRarity)
            .DistinctBy(choice => RewardAcquisitionIdentity(_rewards[choice.RewardId!])).ToArray();
        var authored = tier[random.Next(tier.Length)];
        var promoted = _rewards[authored.RewardId!];
        var prefix = restockOrdinal == 0 ? $"$shop:{state.ShopRefreshCount}:" : $"$restock:{state.ShopRefreshCount}:{restockOrdinal}:";
        return new(prefix + promoted.Id, promoted.Name, authored.GoldCost, authored.HpCost, promoted.Id, LeavesNode: false)
            { Description = promoted.Description };
    }
}
