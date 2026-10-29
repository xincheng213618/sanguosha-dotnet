namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private bool HasWolongMap(RunState state) => BalancedJourneyRewards(state) &&
        OwnedEconomy(state).Any(effect => effect.FavorOwnedSchoolRewards);

    private bool IsOwnedSchoolReward(RunState state, ShanheRewardDefinition reward) =>
        reward.SchoolMembership is { } member && state.TacticIds.Any(id =>
            _rewards[id].SchoolMembership?.SchoolId == member.SchoolId);

    private bool FilterBalancedShopCandidate(RunState state, ShanheRewardDefinition reward) =>
        !BalancedJourneyRewards(state) ? reward.CampaignEconomy?.FavorOwnedSchoolRewards != true :
            reward.Rarity != 1 || HasWolongMap(state) && IsOwnedSchoolReward(state, reward);

    // The published effect says "more likely", without publishing a numeric rate.
    // Three units versus one is this client's local weighted-selection rule.
    private int OwnedSchoolRewardWeight(RunState state, ShanheRewardDefinition reward) =>
        HasWolongMap(state) && IsOwnedSchoolReward(state, reward) ? 3 : 1;

    private int SelectOwnedSchoolRewardIndex(RunState state, IReadOnlyList<ShanheRewardDefinition> candidates,
        DeterministicRandom random)
    {
        var roll = random.Next(candidates.Sum(reward => OwnedSchoolRewardWeight(state, reward)));
        for (var index = 0; index < candidates.Count; index++)
        {
            roll -= OwnedSchoolRewardWeight(state, candidates[index]);
            if (roll < 0) return index;
        }
        throw new InvalidOperationException("The owned-school reward weights could not be sampled.");
    }

    private RunState ReconcileBalancedShopStock(RunState state)
    {
        if (!BalancedJourneyRewards(state) || state.ShopStock.Count == 0) return state;
        var nodeId = state.MarketNodeId ?? state.CurrentNodeId;
        if (nodeId is null || !_nodes.TryGetValue(nodeId, out var node) || node.Kind != ShanheNodeKind.Shop) return state;
        var stock = state.ShopStock.ToList();
        for (var position = 0; position < stock.Count; position++)
        {
            var previous = stock[position];
            if (previous.RewardId is not { } oldId || FilterBalancedShopCandidate(state, _rewards[oldId])) continue;
            // Purchased positions keep their journal identity in PurchasedChoiceIds;
            // removing their old white artwork does not refund or offer a new item.
            if (state.PurchasedChoiceIds.Contains(previous.Id, StringComparer.Ordinal))
            { stock.RemoveAt(position--); continue; }
            var occupied = stock.Where((choice, index) => index != position && choice.RewardId is not null)
                .Select(choice => RewardAcquisitionIdentity(_rewards[choice.RewardId!])).ToHashSet(StringComparer.Ordinal);
            var category = ShopCategory(_rewards[oldId]);
            var candidates = (node.Choices ?? []).Where(choice => choice is { RewardId: not null, LeavesNode: false } &&
                    HasShopStockPrice(choice) && _rewards[choice.RewardId!] is var reward &&
                    ShopCategory(reward) == category && FilterBalancedShopCandidate(state, reward) &&
                    CanReceiveReward(state, reward) && CanApplyBossReward(state, reward) &&
                    !occupied.Contains(RewardAcquisitionIdentity(reward)))
                .DistinctBy(choice => RewardAcquisitionIdentity(_rewards[choice.RewardId!])).ToArray();
            if (candidates.Length == 0) { stock.RemoveAt(position--); continue; }
            var random = new DeterministicRandom(ShanheBattleSetup.DeriveSeed(_options.Seed,
                $"{node.Id}:balanced-stock:{previous.Id}:{position}", state.ShopRestockCount));
            if (SelectShopRarity(state, node, candidates.Select(choice => _rewards[choice.RewardId!]), random) is { } rarity)
                candidates = candidates.Where(choice => _rewards[choice.RewardId!].Rarity == rarity).ToArray();
            var index = HasWolongMap(state)
                ? SelectOwnedSchoolRewardIndex(state, candidates.Select(choice => _rewards[choice.RewardId!]).ToArray(), random)
                : random.Next(candidates.Length);
            var authored = candidates[index];
            var selected = _rewards[authored.RewardId!];
            stock[position] = new($"$balanced:{position}:{selected.Id}", selected.Name,
                authored.GoldCost, authored.HpCost, selected.Id, LeavesNode: false) { Description = selected.Description };
        }
        // This migration changes only invalid shelf positions. Wallet, refresh
        // allowances, restock ordinal and category purchase/price counters survive.
        return state with { ShopStock = ShanheCampaignContent.Freeze(stock) };
    }
}
