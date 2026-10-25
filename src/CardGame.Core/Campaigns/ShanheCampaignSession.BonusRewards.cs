namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private sealed partial record RunState
    {
        public IReadOnlyList<BossRewardGroupState> PendingBonusRewardGroups { get; init; } = [];
    }

    private IReadOnlyList<BossRewardGroupState> BuildBonusRewardGroups(RunState state, ShanheNodeDefinition node)
    {
        // The last chapter settles immediately; additional growth has no next encounter.
        if (node.NextNodeIds.Count == 0 && node.ChapterNumber >= 3) return [];
        var (count, category, rarity, key, name) = node.Kind switch
        {
            ShanheNodeKind.EliteBattle => (OwnedEconomy(state).Sum(effect => effect.ExtraEliteRareTactics),
                ShanheRewardCategory.Tactic, 2, "elite-rare-tactic", "王牌战法·追加稀有战法"),
            ShanheNodeKind.BossBattle => (OwnedEconomy(state).Sum(effect => effect.ExtraBossEpicSkills),
                ShanheRewardCategory.Skill, 3, "boss-epic-skill", "王牌战技·追加史诗技能"),
            _ => (0, ShanheRewardCategory.Supply, 0, "", "")
        };
        if (count == 0) return [];
        var pool = Definition.Rewards.Where(reward => reward.Rarity == rarity && RewardCategory(reward) == category &&
                reward.OpeningSkillRewardIds is null && reward.CampaignEconomy?.StartingGold is not > 0 &&
                CanApplyBossReward(state, reward))
            .DistinctBy(RewardAcquisitionIdentity).Select(reward => reward.Id).ToArray();
        var groups = new List<BossRewardGroupState>();
        var choiceCount = CategorizedRewardChoiceCount(state, category, 3);
        for (var index = 0; index < count; index++)
        {
            var id = "$bonus:" + key + ":" + (index + 1);
            var offers = pool.ToList();
            var random = new DeterministicRandom(ShanheBattleSetup.DeriveSeed(_options.Seed,
                node.Id + ":" + id, state.EncounterOrdinal));
            random.Shuffle(offers);
            groups.Add(new(id, name, ShanheCampaignContent.Freeze(offers.Take(choiceCount)), IsBonus: true));
        }
        return ShanheCampaignContent.Freeze(groups);
    }

    private RunState BeginPendingBonusRewardsOrFinish(RunState state)
    {
        if (state.PendingBonusRewardGroups.Count == 0) return FinishNode(state);
        var next = ClearPrimaryRewardState(state);
        return next with
        {
            Phase = ShanheCampaignPhase.Reward,
            BossRewardGroups = PruneUnavailableBossRewardOffers(next, state.PendingBonusRewardGroups),
            PendingBonusRewardGroups = []
        };
    }
}
