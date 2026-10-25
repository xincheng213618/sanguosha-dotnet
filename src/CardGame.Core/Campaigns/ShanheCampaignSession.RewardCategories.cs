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
        !string.IsNullOrWhiteSpace(reward.TacticSkillId) || reward.OpeningSkillRewardIds is not null ||
        reward.CampaignEconomy is not null || reward.Bonuses is { } bonuses &&
            (bonuses.MaxHp != 0 || bonuses.InitialHand != 0 || bonuses.Draw != 0 || bonuses.Slash != 0)
            ? ShanheRewardCategory.Tactic :
        reward.SkillId is not null ? ShanheRewardCategory.Skill :
        reward.StartingCards?.Count > 0 || reward.StartingEquipment?.Count > 0 ? ShanheRewardCategory.Card :
        ShanheRewardCategory.Supply;

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

    private static RunState ClearPrimaryRewardState(RunState state) => state with
    {
        RewardOfferIds = [], CategoryRewardGroups = [], SelectedRewardCategory = null
    };
}
