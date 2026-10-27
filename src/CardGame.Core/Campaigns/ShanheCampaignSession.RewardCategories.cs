namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private sealed record CategoryRewardGroupState(ShanheRewardCategory Category, IReadOnlyList<string> OfferIds);

    private sealed partial record RunState
    {
        public IReadOnlyList<CategoryRewardGroupState> CategoryRewardGroups { get; init; } = Array.Empty<CategoryRewardGroupState>();
        public ShanheRewardCategory? SelectedRewardCategory { get; init; }
    }

    private static ShanheRewardCategory RewardCategory(ShanheRewardDefinition reward) =>
        !string.IsNullOrWhiteSpace(reward.TacticSkillId) || reward.OpeningSkillRewardIds is not null || reward.OpeningCompanionPool is not null ||
        reward.CampaignEconomy is not null || reward.Bonuses is { } bonuses &&
            (bonuses.MaxHp != 0 || bonuses.InitialHand != 0 || bonuses.Draw != 0 || bonuses.Slash != 0)
            ? ShanheRewardCategory.Tactic :
        reward.SkillId is not null ? ShanheRewardCategory.Skill :
        reward.StartingCards?.Count > 0 || reward.StartingEquipment?.Count > 0 ? ShanheRewardCategory.Card :
        ShanheRewardCategory.Supply;

    private static bool IsPrimaryRewardRarity(ShanheNodeDefinition node, ShanheRewardDefinition reward)
    {
        var rarity = RewardCategory(reward) switch
        {
            ShanheRewardCategory.Tactic => node.PrimaryTacticRarity,
            ShanheRewardCategory.Skill => node.PrimarySkillRarity,
            ShanheRewardCategory.Card => node.PrimaryCardRarity,
            _ => 0
        };
        return rarity == 0 || reward.Rarity == rarity;
    }

    public ShanheRewardCategory? PreviewRewardCategory(string nodeId)
    {
        Require(_nodes.TryGetValue(nodeId, out var node), "Unknown reward location.");
        return AutomaticRewardCategory(ResolveBattleNode(_state, node!));
    }

    private ShanheRewardCategory? AutomaticRewardCategory(ShanheNodeDefinition node)
    {
        if (!node.AutomaticRewardCategory) return null;
        // Type is a property of the appearing location, independent of battle
        // order, later acquisitions and the elite quality of its candidates.
        var categories = (node.RewardIds ?? []).Select(id => _rewards[id])
            .Where(reward => IsRewardUnlocked(reward) && reward.OpeningCompanionPool is null &&
                reward.OpeningSkillRewardIds is null && reward.CampaignEconomy?.StartingGold is not > 0)
            .Select(RewardCategory).Distinct().Order().Where(category => CategoryWeight(category) > 0).ToArray();
        var total = categories.Sum(CategoryWeight);
        if (total == 0) return null;
        var random = new DeterministicRandom(ShanheBattleSetup.DeriveSeed(_options.Seed, node.Id + ":reward-category", 1));
        var roll = random.Next(total);
        foreach (var category in categories)
        {
            roll -= CategoryWeight(category);
            if (roll < 0) return category;
        }
        throw new InvalidOperationException("The location's reward category lost its weighted choice.");

        static int CategoryWeight(ShanheRewardCategory category) => category switch
        { ShanheRewardCategory.Tactic => 6, ShanheRewardCategory.Skill => 3, ShanheRewardCategory.Card => 1, _ => 0 };
    }

    private int CategorizedRewardChoiceCount(RunState state, ShanheRewardCategory category, int baseCount) =>
        Math.Max(baseCount, OwnedEconomy(state).Select(effect => category switch
        {
            ShanheRewardCategory.Tactic => effect.TacticRewardChoiceCount,
            ShanheRewardCategory.Card => effect.CardRewardChoiceCount,
            ShanheRewardCategory.Skill => effect.SkillRewardChoiceCount,
            _ => 0
        }).DefaultIfEmpty(0).Max());

    private RunState BeginCategorizedRewards(RunState state, ShanheNodeDefinition node, IReadOnlyList<string> rewardIds)
    {
        var groups = new List<CategoryRewardGroupState>();
        foreach (var category in Enum.GetValues<ShanheRewardCategory>())
        {
            var candidates = rewardIds.Where(id => RewardCategory(_rewards[id]) == category)
                .DistinctBy(id => RewardAcquisitionIdentity(_rewards[id])).ToList();
            if (candidates.Count == 0) continue;
            var random = new DeterministicRandom(ShanheBattleSetup.DeriveSeed(_options.Seed,
                node.Id + ":rewards:" + category, state.EncounterOrdinal));
            random.Shuffle(candidates);
            groups.Add(new(category, ShanheCampaignContent.Freeze(candidates.Take(
                CategorizedRewardChoiceCount(state, category, node.RewardChoiceCount)))));
        }
        if (groups.Count == 0) return BeginPendingBonusRewardsOrFinish(state);
        return ClearPrimaryRewardState(state) with
        {
            Phase = ShanheCampaignPhase.Reward,
            CategoryRewardGroups = ShanheCampaignContent.Freeze(groups)
        };
    }

    private RunState ChooseRewardCategory(ChooseShanheRewardCategoryCommand command)
    {
        Require(_state.Phase == ShanheCampaignPhase.Reward && _state.BossRewardGroups.Count == 0 &&
            _state.CategoryRewardGroups.Count > 0 && _state.SelectedRewardCategory is null,
            "Choose a reward category before its candidate offer is opened.");
        var group = _state.CategoryRewardGroups.SingleOrDefault(group => group.Category == command.Category);
        Require(group is not null, "Choose one of the published reward categories.");
        return _state with { SelectedRewardCategory = group!.Category, RewardOfferIds = group.OfferIds };
    }

    private static bool HasPrimaryReward(RunState state) =>
        state.Phase == ShanheCampaignPhase.Reward && state.BossRewardGroups.Count == 0 &&
        (state.RewardOfferIds.Count > 0 || state.CategoryRewardGroups.Count > 0 && state.SelectedRewardCategory is null);

    private int PrimaryRewardSkipGold(RunState state) => HasPrimaryReward(state)
        ? RewardSkipCompensation(state.RewardOfferIds, ResolveBattleNode(state, _nodes[state.CurrentNodeId!]).RewardSkipGold) : 0;

    private int RewardSkipCompensation(IReadOnlyList<string> offerIds, int configuredGold)
    {
        // A category must be committed before its actual reward can be declined.
        // Tactics are mandatory, including additive bonuses and economy tactics.
        if (configuredGold <= 0 || offerIds.Count == 0 || offerIds.Any(id => RewardCategory(_rewards[id]) == ShanheRewardCategory.Tactic))
            return 0;
        if (offerIds.All(id => _rewards[id].SkillId is not null))
        {
            var rarities = offerIds.Select(id => _rewards[id].Rarity).Distinct().ToArray();
            if (rarities is [1]) return 50;
            if (rarities is [2]) return 100;
        }
        // Card/equipment and unspecified epic compensation retain the authored
        // amount; no new epic resale price is inferred from the rarity label.
        return configuredGold;
    }

    private static RunState ClearPrimaryRewardState(RunState state) => state with
    {
        RewardOfferIds = [], CategoryRewardGroups = [], SelectedRewardCategory = null
    };
}
