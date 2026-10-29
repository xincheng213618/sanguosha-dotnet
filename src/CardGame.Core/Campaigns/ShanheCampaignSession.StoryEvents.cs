namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private sealed partial record RunState
    {
        public string? StoryEventId { get; init; }
        public IReadOnlyList<string> VisitedStoryEventIds { get; init; } = Array.Empty<string>();
        public IReadOnlyList<ShanheNodeChoiceDefinition> StoryChoices { get; init; } = Array.Empty<ShanheNodeChoiceDefinition>();
        public IReadOnlyList<ShanheCombatantDefinition> RecruitedCompanions { get; init; } = Array.Empty<ShanheCombatantDefinition>();
        public IReadOnlyList<string> BalancedStoryCategoryVisits { get; init; } = Array.Empty<string>();
    }

    private static ShanheStoryEventDefinition? CurrentStoryEvent(RunState state, ShanheNodeDefinition node) =>
        (node.StoryEvents ?? []).Concat(node.SupplementalStoryEvents ?? []).SingleOrDefault(story => story.Id == state.StoryEventId);

    private RunState EnterStoryEvent(RunState state, ShanheNodeDefinition node)
    {
        var balanced = BalancedJourneyRewards(state);
        IReadOnlyList<ShanheStoryEventDefinition> stories = balanced
            ? (node.StoryEvents ?? []).Concat(node.SupplementalStoryEvents ?? []).ToArray() : node.StoryEvents ?? [];
        if (stories.Count == 0) return state;
        var unvisited = stories.Where(story => !state.VisitedStoryEventIds.Contains(story.Id, StringComparer.Ordinal)).ToArray();
        IReadOnlyList<ShanheStoryEventDefinition> pool = unvisited.Length > 0 ? unvisited : stories;
        var random = new DeterministicRandom(ShanheBattleSetup.DeriveSeed(_options.Seed, node.Id + ":story", state.CompletedNodeIds.Count));
        var chosen = balanced ? ChooseBalancedStory(state, node, stories, random) : pool[random.Next(pool.Count)];
        var choices = balanced ? ShanheCampaignContent.Freeze(chosen.Choices.Where(IsBalancedStoryChoice)) : chosen.Choices;
        if (chosen.SkillRewardIds is { } skills)
        {
            var available = skills.Where(id => CanApplyBossReward(state, _rewards[id])).ToList();
            random.Shuffle(available);
            choices = ShanheCampaignContent.Freeze(available.Take(2).Select(id => new ShanheNodeChoiceDefinition(
                "story-skill:" + id, _rewards[id].Name + " · 免费", RewardId: id)
                { Description = _rewards[id].Description }).Concat(choices));
        }
        else if (chosen.TacticRewardIds is { } tactics)
        {
            var available = tactics.Where(id => _rewards[id].Rarity >= 2 && CanApplyBossReward(state, _rewards[id]))
                .DistinctBy(id => RewardAcquisitionIdentity(_rewards[id])).ToList();
            random.Shuffle(available);
            choices = ShanheCampaignContent.Freeze(available.Take(2).Select(id => new ShanheNodeChoiceDefinition(
                "story-tactic:" + id, _rewards[id].Name + " · 免费", RewardId: id)
                { Description = _rewards[id].Description }).Concat(choices));
        }
        return state with
        {
            StoryEventId = chosen.Id,
            StoryChoices = choices,
            VisitedStoryEventIds = ShanheCampaignContent.Freeze(state.VisitedStoryEventIds.Append(chosen.Id)),
            // Derive history for legacy entries too, without changing their draw.
            // A later explicit upgrade can then respect this chapter's visits.
            BalancedStoryCategoryVisits = ShanheCampaignContent.Freeze(state.BalancedStoryCategoryVisits.Append(
                StoryCategoryKey(node, StoryCategory(chosen))))
        };
    }

    private enum BalancedStoryCategory { Recruitment, Tactic, Skill, Other }

    private static string StoryCategoryKey(ShanheNodeDefinition node, BalancedStoryCategory category) =>
        (node.ChapterRouteId ?? "chapter-" + node.ChapterNumber) + ":" + category;

    private BalancedStoryCategory StoryCategory(ShanheStoryEventDefinition story) =>
        story.Choices.Any(choice => choice.Companion is not null) ? BalancedStoryCategory.Recruitment :
        story.TacticRewardIds is not null || story.Choices.Any(choice => choice.RewardId is { } id &&
            _rewards.TryGetValue(id, out var reward) && RewardCategory(reward) == ShanheRewardCategory.Tactic && reward.Rarity >= 2)
            ? BalancedStoryCategory.Tactic :
        story.SkillRewardIds is not null || story.Choices.Any(choice => choice.RewardId is { } id &&
            _rewards.TryGetValue(id, out var reward) && RewardCategory(reward) == ShanheRewardCategory.Skill)
            ? BalancedStoryCategory.Skill : BalancedStoryCategory.Other;

    private bool IsBalancedStoryChoice(ShanheNodeChoiceDefinition choice) => choice.RewardId is not { } id ||
        RewardCategory(_rewards[id]) != ShanheRewardCategory.Tactic || _rewards[id].Rarity >= 2;

    private ShanheStoryEventDefinition ChooseBalancedStory(RunState state, ShanheNodeDefinition node,
        IReadOnlyList<ShanheStoryEventDefinition> stories, DeterministicRandom random)
    {
        var groups = stories.Where(story => HasMeaningfulStoryChoice(state, story))
            .GroupBy(StoryCategory).Where(group => group.Key != BalancedStoryCategory.Other)
            .OrderBy(group => group.Key).ToArray();
        var wolong = HasWolongMap(state);
        if (!wolong)
        {
            var unseen = groups.Where(group => !state.BalancedStoryCategoryVisits.Contains(StoryCategoryKey(node, group.Key), StringComparer.Ordinal)).ToArray();
            if (unseen.Length > 0) groups = unseen;
        }
        IReadOnlyList<ShanheStoryEventDefinition> candidates;
        if (groups.Length > 0)
        {
            // Local policy: draw categories, never individual recruitment entries.
            // Wolong allows repeats and gives tactics / already-visited categories
            // one extra weight each; no official encounter probability is claimed.
            int Weight(BalancedStoryCategory category) => !wolong ? 1 : 1 +
                (category == BalancedStoryCategory.Tactic ? 1 : 0) +
                (state.BalancedStoryCategoryVisits.Contains(StoryCategoryKey(node, category), StringComparer.Ordinal) ? 1 : 0);
            var roll = random.Next(groups.Sum(group => Weight(group.Key)));
            var selected = groups[0];
            foreach (var group in groups)
            {
                if (roll < Weight(group.Key)) { selected = group; break; }
                roll -= Weight(group.Key);
            }
            candidates = selected.ToArray();
        }
        else
        {
            var fallback = stories.Where(story => story.Choices.Any(choice => IsBalancedStoryChoice(choice) &&
                CanUseAuthoredStoryChoice(state, choice))).ToArray();
            candidates = fallback.Length > 0 ? fallback : stories;
        }
        var unvisited = candidates.Where(story => !state.VisitedStoryEventIds.Contains(story.Id, StringComparer.Ordinal)).ToArray();
        var pool = unvisited.Length > 0 ? unvisited : candidates;
        return pool[random.Next(pool.Count)];
    }

    private bool HasMeaningfulStoryChoice(RunState state, ShanheStoryEventDefinition story) => StoryCategory(story) switch
    {
        BalancedStoryCategory.Recruitment => story.Choices.Any(choice => choice.Companion is not null && CanUseAuthoredStoryChoice(state, choice)),
        BalancedStoryCategory.Tactic => (story.TacticRewardIds ?? []).Any(id => _rewards[id].Rarity >= 2 && CanApplyBossReward(state, _rewards[id])) ||
            story.Choices.Any(choice => choice.RewardId is { } id && RewardCategory(_rewards[id]) == ShanheRewardCategory.Tactic &&
                IsBalancedStoryChoice(choice) && CanUseAuthoredStoryChoice(state, choice)),
        BalancedStoryCategory.Skill => (story.SkillRewardIds ?? []).Any(id => CanApplyBossReward(state, _rewards[id])) ||
            story.Choices.Any(choice => choice.RewardId is { } id && RewardCategory(_rewards[id]) == ShanheRewardCategory.Skill &&
                CanUseAuthoredStoryChoice(state, choice) && CanApplyBossReward(state, _rewards[id])),
        _ => false
    };

    private bool CanUseAuthoredStoryChoice(RunState state, ShanheNodeChoiceDefinition choice) =>
        state.Gold >= choice.GoldCost && (choice.HpCost == 0 || state.Hp > choice.HpCost) &&
        (choice.Companion is null || CanRecruitStoryCompanion(state, choice.Companion)) &&
        (choice.RewardId is not { } id || CanReceiveReward(state, _rewards[id]));

    private static bool IsEarnedStoryReward(RunState state, ShanheNodeDefinition node, string rewardId) =>
        CurrentStoryEvent(state, node) is { } story &&
        (story.SkillRewardIds?.Contains(rewardId, StringComparer.Ordinal) == true ||
         story.TacticRewardIds?.Contains(rewardId, StringComparer.Ordinal) == true);

    private bool CanReceiveStoryReward(RunState state, ShanheNodeDefinition node, string rewardId) =>
        IsEarnedStoryReward(state, node, rewardId)
            ? CanApplyBossReward(state, _rewards[rewardId]) : CanReceiveReward(state, _rewards[rewardId]);

    private static bool CanRecruitStoryCompanion(RunState state, ShanheCombatantDefinition companion) =>
        state.StartingCompanions.Count + state.RecruitedCompanions.Count < 6 &&
        !state.StartingCompanions.Concat(state.RecruitedCompanions).Any(member => member.GeneralId == companion.GeneralId);

    private RunState RecruitStoryCompanion(RunState state, ShanheCombatantDefinition companion)
    {
        Require(CanRecruitStoryCompanion(state, companion), "This companion is already recruited or the companion roster is full.");
        var first = Definition.Nodes.First(node => ShanheCampaignContent.IsBattle(node.Kind)).Encounters![0];
        var probe = new ShanheBattleSetup(_options.Seed, 1, "companion-probe", _options.PlayerGeneralId,
            state.Hp, state.Bonuses.MaxHp, 0, 0, 0, [], [], [], first.Enemies.Take(6).ToArray(), [companion], ContentHash);
        var registry = _battleRegistryFactory(probe);
        Require(registry.IsGeneralPlayable(companion.GeneralId) && (companion.ExtraSkillIds ?? []).All(skill =>
            registry.Skills.TryGetValue(skill, out var definition) && definition.ImplementationStatus == SkillImplementationStatus.Complete),
            "The companion requires complete battle content.");
        return state with { RecruitedCompanions = ShanheCampaignContent.Freeze(state.RecruitedCompanions.Append(companion)) };
    }

    private ShanheEncounterDefinition WithRecruitedCompanions(ShanheEncounterDefinition encounter, ShanheNodeDefinition node)
    {
        if (_state.RecruitedCompanions.Count == 0) return encounter;
        var allies = encounter.Allies ?? [];
        var candidates = _state.RecruitedCompanions.Where(member => !allies.Any(ally => ally.GeneralId == member.GeneralId)).ToArray();
        var recruits = candidates.Take(Math.Max(0, 7 - encounter.Enemies.Count - allies.Count)).ToArray();
        return encounter with
        {
            Allies = ShanheCampaignContent.Freeze(allies.Concat(recruits)),
            StoryText = recruits.Length == candidates.Length ? encounter.StoryText :
                (encounter.StoryText ?? node.StoryText) + "\n\n本场席位已满，部分奇遇队友暂不参战。"
        };
    }
}
