namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private sealed partial record RunState
    {
        public string? StoryEventId { get; init; }
        public IReadOnlyList<string> VisitedStoryEventIds { get; init; } = Array.Empty<string>();
        public IReadOnlyList<ShanheNodeChoiceDefinition> StoryChoices { get; init; } = Array.Empty<ShanheNodeChoiceDefinition>();
        public IReadOnlyList<ShanheCombatantDefinition> RecruitedCompanions { get; init; } = Array.Empty<ShanheCombatantDefinition>();
    }

    private static ShanheStoryEventDefinition? CurrentStoryEvent(RunState state, ShanheNodeDefinition node) =>
        node.StoryEvents?.SingleOrDefault(story => story.Id == state.StoryEventId);

    private RunState EnterStoryEvent(RunState state, ShanheNodeDefinition node)
    {
        if (node.StoryEvents is not { Count: > 0 } stories) return state;
        var unvisited = stories.Where(story => !state.VisitedStoryEventIds.Contains(story.Id, StringComparer.Ordinal)).ToArray();
        IReadOnlyList<ShanheStoryEventDefinition> pool = unvisited.Length > 0 ? unvisited : stories;
        var random = new DeterministicRandom(ShanheBattleSetup.DeriveSeed(_options.Seed, node.Id + ":story", state.CompletedNodeIds.Count));
        var chosen = pool[random.Next(pool.Count)];
        var choices = chosen.Choices;
        if (chosen.SkillRewardIds is { } skills)
        {
            var available = skills.Where(id => CanApplyBossReward(state, _rewards[id])).ToList();
            random.Shuffle(available);
            choices = ShanheCampaignContent.Freeze(available.Take(2).Select(id => new ShanheNodeChoiceDefinition(
                "story-skill:" + id, _rewards[id].Name + " · 免费", RewardId: id)
                { Description = _rewards[id].Description }).Concat(choices));
        }
        return state with
        {
            StoryEventId = chosen.Id,
            StoryChoices = choices,
            VisitedStoryEventIds = ShanheCampaignContent.Freeze(state.VisitedStoryEventIds.Append(chosen.Id))
        };
    }

    private bool CanReceiveStoryReward(RunState state, ShanheNodeDefinition node, string rewardId) =>
        CurrentStoryEvent(state, node)?.SkillRewardIds?.Contains(rewardId, StringComparer.Ordinal) == true
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
