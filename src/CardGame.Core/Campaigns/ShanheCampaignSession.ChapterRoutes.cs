namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    public int ChapterProgressTarget => 6;
    public string? ActiveChapterRouteId => ActiveRouteId(_state);
    public int ChapterProgress => RouteProgress(_state, ActiveRouteId(_state));
    public bool IsChapterMarketUnlocked => ChapterMarketUnlocked(_state);
    public bool IsChapterNodeUnlocked(string nodeId) => _nodes.TryGetValue(nodeId, out var node) &&
        NodeChapterUnlocked(_state, node);
    public bool IsChapterNodeMainline(string nodeId) => _nodes.TryGetValue(nodeId, out var node) &&
        node.ChapterProgressStep > 0 && node.ChapterProgressStep > RouteProgress(_state, node.ChapterRouteId);

    private string? ActiveRouteId(RunState state)
    {
        var available = state.Phase == ShanheCampaignPhase.OpeningSkillDraft ? state.OpeningResumeNodeIds : state.AvailableNodeIds;
        return available.Select(id => _nodes[id].ChapterRouteId).FirstOrDefault(id => id is not null) ??
            (state.CurrentNodeId is { } current ? _nodes[current].ChapterRouteId : null);
    }

    private int RouteProgress(RunState state, string? routeId) => routeId is null ? 0 :
        state.CompletedNodeIds.Select(id => _nodes[id]).Where(node => node.ChapterRouteId == routeId && node.ChapterProgressStep > 0)
            .Select(node => node.ChapterProgressStep).Distinct().Count();

    private bool ChapterMarketUnlocked(RunState state)
    {
        if (_battleMarketSession) return true;
        var route = ActiveRouteId(state);
        return route is null || RouteProgress(state, route) >= 3;
    }

    private bool NodeChapterUnlocked(RunState state, ShanheNodeDefinition node) =>
        node.ChapterRouteId is null || node.ChapterRouteId == ActiveRouteId(state) &&
        node.ChapterRole != ShanheChapterNodeRole.Profile &&
        node.RequiredChapterProgress <= RouteProgress(state, node.ChapterRouteId) && IsExtraChapterBattleUnlocked(state, node);

    private IReadOnlyList<string> RouteAvailableNodes(RunState state, string routeId) =>
        ShanheCampaignContent.Freeze(Definition.Nodes.Where(node => node.ChapterRouteId == routeId &&
            node.ChapterRole is not (ShanheChapterNodeRole.Profile or ShanheChapterNodeRole.Market) &&
            node.RequiredChapterProgress <= RouteProgress(state, routeId) && IsExtraChapterBattleUnlocked(state, node) &&
            !state.CompletedNodeIds.Contains(node.Id, StringComparer.Ordinal)).Select(node => node.Id));

    private RunState InitializeChapterRoute(RunState state)
    {
        var route = ActiveRouteId(state);
        if (!Definition.RandomStartingNode && state.AvailableNodeIds.Select(id => _nodes[id].ChapterRouteId).Distinct().Count() > 1)
            return state;
        return route is null ? state : state with { AvailableNodeIds = RouteAvailableNodes(state, route) };
    }

    private RunState FinishChapterRouteNode(RunState state, ShanheNodeDefinition node)
    {
        state = state with { CompletedNodeIds = ShanheCampaignContent.Freeze(state.CompletedNodeIds.Append(node.Id)) };
        IReadOnlyList<string> available;
        if (node.ChapterRole == ShanheChapterNodeRole.Boss)
        {
            var routes = node.NextNodeIds.Select(id => _nodes[id].ChapterRouteId).OfType<string>().Distinct().ToArray();
            if (routes.Length == 0) available = [];
            else
            {
                var random = new DeterministicRandom(ShanheBattleSetup.DeriveSeed(_options.Seed, node.Id + ":next-chapter", 0));
                available = RouteAvailableNodes(state, routes[random.Next(routes.Length)]);
            }
        }
        else available = RouteAvailableNodes(state, node.ChapterRouteId!);
        return ClearPrimaryRewardState(state) with
        {
            Phase = available.Count == 0 ? ShanheCampaignPhase.Won : ShanheCampaignPhase.Map,
            AvailableNodeIds = available, BossRewardGroups = [], PendingBonusRewardGroups = [],
            BattleSetup = null, BattleProfileNodeId = null
        };
    }
}
