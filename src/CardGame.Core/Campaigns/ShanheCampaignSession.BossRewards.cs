namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private sealed record BossRewardGroupState(string Id, string Name, IReadOnlyList<string> OfferIds,
        string? SelectedRewardId = null, bool IsBonus = false);

    private RunState BeginBossRewards(RunState state, ShanheNodeDefinition node)
    {
        var groups = node.BossRewards!.Groups.Select(group =>
        {
            var offers = group.RewardIds.Where(id => CanApplyBossReward(state, _rewards[id]))
                .DistinctBy(id => RewardAcquisitionIdentity(_rewards[id])).ToList();
            var random = new DeterministicRandom(ShanheBattleSetup.DeriveSeed(_options.Seed,
                node.Id + ":boss:" + group.Id, state.EncounterOrdinal));
            random.Shuffle(offers);
            var categories = group.RewardIds.Select(id => RewardCategory(_rewards[id])).Distinct().ToArray();
            var choiceCount = categories.Length == 1
                ? CategorizedRewardChoiceCount(state, categories[0], group.ChoiceCount) : group.ChoiceCount;
            return new BossRewardGroupState(group.Id, group.Name, ShanheCampaignContent.Freeze(offers.Take(choiceCount)));
        });
        return ClearPrimaryRewardState(state) with
        {
            Phase = ShanheCampaignPhase.Reward,
            BossRewardGroups = ShanheCampaignContent.Freeze(groups.Concat(state.PendingBonusRewardGroups)),
            PendingBonusRewardGroups = []
        };
    }

    private bool CanApplyBossReward(RunState state, ShanheRewardDefinition reward)
    {
        // Victory rewards are earned by this run. Account research gates shop
        // stock and initial loadouts, not earned chapter or bonus skills.
        if (reward.SkillId is { } skill && HasSkill(state, skill) ||
            reward.OpeningSkillRewardIds is not null && state.TacticIds.Contains(reward.Id, StringComparer.Ordinal) ||
            reward.TacticSkillId is { } tactic && !CanAcquireTactic(state, tactic)) return false;
        var replacements = reward.SkillId is not null && state.ExtraSkillIds.Count >= state.ExtraSkillSlots + reward.AddSkillSlots
            ? state.ExtraSkillIds.Cast<string?>() : new string?[] { null };
        foreach (var replacement in replacements)
        {
            try { _ = ApplyReward(state, reward, replacement, allowLockedReward: true); return true; }
            catch (Exception error) when (error is ArgumentException or OverflowException) { }
        }
        return false;
    }

    private RunState ChooseBossReward(ChooseShanheBossRewardCommand command)
    {
        Require(_state.Phase == ShanheCampaignPhase.Reward && _state.BossRewardGroups.Count > 0,
            "There is no chapter boss reward bundle.");
        var group = _state.BossRewardGroups.SingleOrDefault(group => group.Id == command.GroupId);
        Require(group is not null && group.SelectedRewardId is null && group.OfferIds.Contains(command.RewardId, StringComparer.Ordinal),
            "Choose one of this unclaimed boss group's frozen rewards.");
        var next = ApplyReward(_state, _rewards[command.RewardId], command.ReplaceSkillId, allowLockedReward: true);
        return next with
        {
            BossRewardGroups = PruneUnavailableBossRewardOffers(next,
                _state.BossRewardGroups.Select(item =>
                    item.Id == command.GroupId ? item with { SelectedRewardId = command.RewardId } : item))
        };
    }

    private RunState ConfirmBossReward()
    {
        Require(_state.Phase == ShanheCampaignPhase.Reward && _state.BossRewardGroups.Count > 0,
            "There is no chapter boss completion to confirm.");
        // Fully exhausted pools are explicitly reported by the snapshot, never silently rerolled.
        Require(_state.BossRewardGroups.All(group => group.SelectedRewardId is not null || group.OfferIds.Count == 0),
            "Every available boss reward group must be claimed before continuing.");
        return FinishNode(_state);
    }

    private ShanheBossRewardSummary? CreateBossRewardSummary(RunState state, long revision)
    {
        if (state.Phase != ShanheCampaignPhase.Reward || state.BossRewardGroups.Count == 0 ||
            state.CurrentNodeId is not { } id) return null;
        var node = _nodes[id];
        var isBonusOnly = state.BossRewardGroups.All(group => group.IsBonus);
        var bundle = node.BossRewards;
        return new(node.ChapterNumber, isBonusOnly ? node.Name + "·追加奖励" : node.ChapterTitle ?? node.Name,
            isBonusOnly ? 0 : bundle!.FixedGold, isBonusOnly ? 0 : bundle!.SkillSlots,
            ShanheCampaignContent.Freeze(state.BossRewardGroups.Select(group => new ShanheBossRewardGroupSnapshot(group.Id,
                group.Name,
                ShanheCampaignContent.Freeze(group.OfferIds.Select(rewardId => _rewards[rewardId])),
                group.SelectedRewardId is { } selected ? _rewards[selected] : null))), revision)
        { IsBonusOnly = isBonusOnly };
    }

    private IReadOnlyList<BossRewardGroupState> PruneUnavailableBossRewardOffers(RunState state,
        IEnumerable<BossRewardGroupState> groups) => ShanheCampaignContent.Freeze(groups.Select(group =>
            group.SelectedRewardId is not null ? group : group with
            { OfferIds = ShanheCampaignContent.Freeze(group.OfferIds.Where(id => CanApplyBossReward(state, _rewards[id]))) }));
}
