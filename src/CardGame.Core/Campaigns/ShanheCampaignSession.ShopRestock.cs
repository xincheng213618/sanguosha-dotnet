namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    // Only the purchase command enters here; snapshot availability queries use pure preparation.
    private RunState RestockPurchasedShopItem(RunState state, string choiceId)
    {
        if (!IsRandomShop(state) || !RestocksShopOnPurchase(state)) return state;
        var stock = state.ShopStock.ToList();
        var position = stock.FindIndex(choice => choice.Id == choiceId);
        if (position < 0 || stock[position] is not { RewardId: { } boughtId, LeavesNode: false } bought ||
            bought.GoldCost == 9999) return state;

        var node = _nodes[state.CurrentNodeId!];
        var category = ShopCategory(_rewards[boughtId]);
        var occupied = stock.Where((choice, index) => index != position &&
                !state.PurchasedChoiceIds.Contains(choice.Id, StringComparer.Ordinal) && choice.RewardId is not null)
            .Select(choice => RewardAcquisitionIdentity(_rewards[choice.RewardId!])).ToHashSet(StringComparer.Ordinal);
        var candidates = (node.Choices ?? []).Where(choice => choice is { RewardId: not null, LeavesNode: false } &&
                choice.GoldCost != 9999 && ShopCategory(_rewards[choice.RewardId!]) == category)
            // The shared reward-cap preview bypasses research for earned boss loot;
            // shop candidates must first retain their ordinary unlock and ownership gate.
            .Where(choice => CanReceiveReward(state, _rewards[choice.RewardId!]) &&
                CanApplyBossReward(state, _rewards[choice.RewardId!]) &&
                !occupied.Contains(RewardAcquisitionIdentity(_rewards[choice.RewardId!])))
            .DistinctBy(choice => RewardAcquisitionIdentity(_rewards[choice.RewardId!])).ToList();
        var boughtIdentity = RewardAcquisitionIdentity(_rewards[boughtId]);
        var different = candidates.Where(choice =>
            RewardAcquisitionIdentity(_rewards[choice.RewardId!]) != boughtIdentity).ToList();
        if (different.Count > 0) candidates = different;
        if (candidates.Count == 0) return state;

        var ordinal = checked(state.ShopRestockCount + 1);
        var random = new DeterministicRandom(ShanheBattleSetup.DeriveSeed(_options.Seed,
            $"{node.Id}:shop-restock:{state.ShopRefreshCount}:{position}", ordinal));
        var authored = candidates[random.Next(candidates.Count)];
        var reward = _rewards[authored.RewardId!];
        stock[position] = new($"$restock:{state.ShopRefreshCount}:{ordinal}:{reward.Id}", reward.Name,
            authored.GoldCost, authored.HpCost, reward.Id, LeavesNode: false) { Description = reward.Description };
        stock[position] = PromoteShopItem(state, node, stock, position, ordinal);
        return state with { ShopStock = ShanheCampaignContent.Freeze(stock), ShopRestockCount = ordinal };
    }

    private IEnumerable<ShanheNodeChoiceDefinition> RetainedShopSupplyChoices(RunState state, ShanheNodeDefinition node) =>
        (state.ShopStock.Count == 0 ? node.Choices ?? [] : state.ShopStock)
            .Where(choice => choice.RewardId is not { } id || ShopCategory(_rewards[id]) == ShopRewardCategory.Supply);
}
