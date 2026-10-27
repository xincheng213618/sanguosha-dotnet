namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private sealed partial record RunState
    {
        // Set even for an ordinary entry. Buying the tactic in that battle may
        // affect future nodes, but cannot rewrite this encounter or its loot.
        public string? BattleProfileNodeId { get; init; }
        public IReadOnlyList<string> UpgradedBattleNodeIds { get; init; } = [];
    }

    private string EntryBattleProfileId(RunState state, ShanheNodeDefinition node) =>
        node.Kind == ShanheNodeKind.Battle && node.EliteUpgradeNodeId is { } eliteId &&
        OwnedEconomy(state).Any(effect => effect.UpgradeOrdinaryBattles) ? eliteId : node.Id;

    private ShanheNodeDefinition ResolveBattleNode(RunState state, ShanheNodeDefinition node)
    {
        if (node.Kind != ShanheNodeKind.Battle) return node;
        var profileId = state.CurrentNodeId == node.Id && state.BattleProfileNodeId is { } locked
            ? locked : state.UpgradedBattleNodeIds.Contains(node.Id, StringComparer.Ordinal)
                ? node.EliteUpgradeNodeId! : state.CompletedNodeIds.Contains(node.Id, StringComparer.Ordinal)
                    ? node.Id : EntryBattleProfileId(state, node);
        if (profileId == node.Id) return node;
        var elite = _nodes[profileId];
        var cities = (node.Encounters ?? []).Select(encounter => encounter.SourceCityId).Distinct().ToArray();
        var cityId = cities.Length == 1 ? cities[0] : null;
        var terrain = node.Encounters?.FirstOrDefault()?.TerrainSkillId;
        return elite with
        {
            Id = node.Id, Name = node.Name + " · 精英", NextNodeIds = node.NextNodeIds,
            ChapterNumber = node.ChapterNumber, ChapterTitle = node.ChapterTitle,
            IsStoryBattle = node.IsStoryBattle,
            Encounters = ShanheCampaignContent.Freeze(elite.Encounters!.Select(encounter => encounter with
            {
                Id = "wolf:" + node.Id + ":" + encounter.Id,
                // This is the authored local elite profile at the actual city,
                // not a borrowed official battle or city qualification.
                SourceCityId = cityId, SourceBattleId = null,
                TerrainSkillId = terrain ?? encounter.TerrainSkillId
            }))
        };
    }

    private ShanheNodeDefinition? CurrentBattleNode(RunState state) =>
        state.BattleProfileNodeId is not null && state.CurrentNodeId is { } id &&
        state.Phase is ShanheCampaignPhase.Battle or ShanheCampaignPhase.Reward
            ? ResolveBattleNode(state, _nodes[id]) : null;
}
