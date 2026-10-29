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

    /// <summary>Reads an available battle's effective route and reward profile without entering it.</summary>
    public ShanheNodeDefinition PreviewBattleNode(string nodeId)
    {
        Require(_state.Phase == ShanheCampaignPhase.Map && _state.AvailableNodeIds.Contains(nodeId, StringComparer.Ordinal),
            "Choose an available map node.");
        var node = _nodes[nodeId];
        Require(ShanheCampaignContent.IsBattle(node.Kind), "Only a battle node has an encounter preview.");
        return ResolveBattleNode(_state, node);
    }

    private ShanheNodeDefinition ResolveBattleNode(RunState state, ShanheNodeDefinition node)
    {
        node = BalancedBattleRewards(state, ResolveSourceSceneCityNode(state, node));
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
        return BalancedBattleRewards(state, elite with
        {
            Id = node.Id, Name = node.Name + " · 精英", NextNodeIds = node.NextNodeIds,
            ChapterNumber = node.ChapterNumber, ChapterTitle = node.ChapterTitle,
            ExtraChapterBattleIndex = node.ExtraChapterBattleIndex,
            ChapterRouteId = node.ChapterRouteId, ChapterRole = node.ChapterRole,
            ChapterProgressStep = node.ChapterProgressStep, RequiredChapterProgress = node.RequiredChapterProgress,
            IsStoryBattle = node.IsStoryBattle,
            Encounters = ShanheCampaignContent.Freeze(elite.Encounters!.Select(encounter => encounter with
            {
                Id = "wolf:" + node.Id + ":" + encounter.Id,
                // This is the authored local elite profile at the actual city,
                // not a borrowed official battle or city qualification.
                SourceCityId = cityId, SourceBattleId = null,
                TerrainSkillId = _options.UseSourceSceneCityBindings && Definition.SourceDifficultyId is not null
                    ? terrain : terrain ?? encounter.TerrainSkillId
            }))
        });
    }

    private ShanheNodeDefinition ResolveSourceSceneCityNode(RunState state, ShanheNodeDefinition node)
    {
        if (!_options.UseSourceSceneCityBindings || Definition.SourceDifficultyId is null ||
            node.Kind is not (ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle)) return node;
        // PrepareEncounter may receive an already-derived Wolf profile. Its
        // elite roster has already inherited the resolved city and terrain.
        if (node.Kind == ShanheNodeKind.EliteBattle && _nodes.TryGetValue(node.Id, out var original) &&
            original.Kind == ShanheNodeKind.Battle) return node;
        var openings = Definition.Nodes.Where(candidate => candidate.ChapterRouteId == node.ChapterRouteId &&
            candidate.ChapterRole == ShanheChapterNodeRole.OpeningBattle).ToArray();
        var selected = state.CompletedNodeIds.FirstOrDefault(id => openings.Any(opening => opening.Id == id));
        var selectedIndex = Array.FindIndex(openings, opening => opening.Id == selected);
        var openingIndex = Array.FindIndex(openings, opening => opening.Id == node.Id);
        if (ShanheChapterSceneLocation.NativeCityId(node, openingIndex, Math.Max(0, selectedIndex)) is not { } cityId ||
            !_nodes.TryGetValue("source-city-" + cityId.ToString(System.Globalization.CultureInfo.InvariantCulture), out var source) ||
            source.Encounters is not { Count: > 0 } encounters) return node;
        // Take the target city's complete original recipes and terrain together.
        // Logical route IDs still own commands, rewards and deterministic seeds.
        return node with
        {
            Encounters = node.ExtraChapterBattleIndex > 0
                ? ShanheCampaignContent.Freeze(encounters.Select(encounter => encounter with
                    { Id = node.Id + ":" + encounter.Id, SourceBattleId = null }))
                : encounters
        };
    }

    private ShanheNodeDefinition? CurrentBattleNode(RunState state) =>
        state.BattleProfileNodeId is not null && state.CurrentNodeId is { } id &&
        state.Phase is ShanheCampaignPhase.Battle or ShanheCampaignPhase.Reward
            ? ResolveBattleNode(state, _nodes[id]) : null;
}
