using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // Run after every local chapter/story branch has been composed. References
    // are profiles only; the original ordinary nodes and routes stay authored.
    private static ShanheCampaignDefinition WithCurrentWolfBattles(ShanheCampaignDefinition definition)
    {
        if (!(definition.Id.StartsWith("shanhe:zhuhou-2026-", StringComparison.Ordinal) ||
              definition.Id.StartsWith(Source2026CampaignIdPrefix, StringComparison.Ordinal)) ||
            !definition.Rewards.Any(reward => reward.Id == CurrentWolfBattlesRewardId &&
                reward.CampaignEconomy?.UpgradeOrdinaryBattles == true)) return definition;
        var elites = definition.Nodes.Where(node => node.Kind == ShanheNodeKind.EliteBattle &&
            node.Encounters is { Count: > 0 }).ToArray();
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            Nodes = definition.Nodes.Select(node =>
            {
                if (node.Kind != ShanheNodeKind.Battle || node.EliteUpgradeNodeId is not null) return node;
                var profile = elites.Where(elite => elite.ChapterNumber == node.ChapterNumber)
                    .OrderBy(elite => elite.IsStoryBattle ? 1 : 0)
                    .ThenBy(elite => elite.NextNodeIds.Intersect(node.NextNodeIds, StringComparer.Ordinal).Any() ? 0 : 1)
                    .ThenBy(elite => elite.Id, StringComparer.Ordinal).FirstOrDefault()
                    ?? throw new InvalidOperationException("A current Wolf Battles reward needs an existing elite profile for chapter " + node.ChapterNumber + ".");
                return node with { EliteUpgradeNodeId = profile.Id };
            }).ToArray()
        });
    }
}
