namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    // Finalize only during the winning battle command, after every baseline offer exists.
    // Per-position chance and the next available higher tier are local execution rulings.
    private RunState PromoteBattlePrimaryRewards(RunState state, ShanheNodeDefinition node)
    {
        var chance = Math.Min(100, OwnedEconomy(state).Sum(effect => effect.BattlePrimaryPromotionPercent));
        if (chance == 0 || state.Phase != ShanheCampaignPhase.Reward) return state;

        var groups = new List<(string Key, string[] Offers, IReadOnlyList<string> Pool, bool Earned, bool Protected)>();
        if (state.BossRewardGroups.Count > 0)
        {
            foreach (var group in state.BossRewardGroups)
                groups.Add(("boss:" + group.Id, group.OfferIds.ToArray(),
                    group.IsBonus ? [] : node.BossRewards!.Groups.Single(authored => authored.Id == group.Id).RewardIds,
                    true, group.IsBonus));
        }
        else if (state.CategoryRewardGroups.Count > 0)
        {
            foreach (var group in state.CategoryRewardGroups)
                groups.Add(("category:" + group.Category, group.OfferIds.ToArray(), node.RewardIds ?? [], false, false));
        }
        else groups.Add(("mixed", state.RewardOfferIds.ToArray(), node.RewardIds ?? [], false, false));

        var occupied = groups.SelectMany(group => group.Offers)
            .Concat(state.PendingBonusRewardGroups.SelectMany(group => group.OfferIds))
            .Select(id => RewardAcquisitionIdentity(_rewards[id])).GroupBy(id => id, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.Count(), StringComparer.Ordinal);
        foreach (var group in groups.Where(group => !group.Protected))
        for (var position = 0; position < group.Offers.Length; position++)
        {
            var original = _rewards[group.Offers[position]];
            var category = RewardCategory(original);
            if (category is not (ShanheRewardCategory.Tactic or ShanheRewardCategory.Skill) || original.Rarity is < 1 or >= 4) continue;
            // No draw from a baseline sampler or from another position's generator.
            var random = new DeterministicRandom(ShanheBattleSetup.DeriveSeed(_options.Seed,
                $"{node.Id}:battle-quality:{group.Key}:{position}", state.EncounterOrdinal));
            if (random.Next(100) >= chance) continue;
            var identity = RewardAcquisitionIdentity(original);
            var candidates = group.Pool.Select(id => _rewards[id]).Where(reward =>
                    RewardCategory(reward) == category && reward.Rarity > original.Rarity &&
                    RewardAcquisitionIdentity(reward) != identity &&
                    occupied.GetValueOrDefault(RewardAcquisitionIdentity(reward)) == 0 &&
                    (group.Earned || CanReceiveReward(state, reward)) && CanApplyBossReward(state, reward))
                .ToArray();
            if (candidates.Length == 0) continue;
            var rarity = candidates.Min(reward => reward.Rarity);
            var tier = candidates.Where(reward => reward.Rarity == rarity).DistinctBy(RewardAcquisitionIdentity).ToArray();
            var promoted = tier[random.Next(tier.Length)];
            group.Offers[position] = promoted.Id;
            occupied[identity]--;
            var promotedIdentity = RewardAcquisitionIdentity(promoted);
            occupied[promotedIdentity] = occupied.GetValueOrDefault(promotedIdentity) + 1;
        }

        if (state.BossRewardGroups.Count > 0)
            return state with { BossRewardGroups = ShanheCampaignContent.Freeze(state.BossRewardGroups.Select((group, index) =>
                group with { OfferIds = ShanheCampaignContent.Freeze(groups[index].Offers) })) };
        if (state.CategoryRewardGroups.Count > 0)
            return state with { CategoryRewardGroups = ShanheCampaignContent.Freeze(state.CategoryRewardGroups.Select((group, index) =>
                group with { OfferIds = ShanheCampaignContent.Freeze(groups[index].Offers) })) };
        return state with { RewardOfferIds = ShanheCampaignContent.Freeze(groups[0].Offers) };
    }
}
