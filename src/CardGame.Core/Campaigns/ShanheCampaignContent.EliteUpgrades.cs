namespace CardGame.Core.Campaigns;

public static partial class ShanheCampaignContent
{
    private static void ValidateEliteUpgradeProfiles(ShanheCampaignDefinition definition)
    {
        var nodes = definition.Nodes.ToDictionary(node => node.Id, StringComparer.Ordinal);
        foreach (var node in definition.Nodes.Where(node => node.EliteUpgradeNodeId is not null))
            Require(node.Kind == ShanheNodeKind.Battle &&
                nodes.TryGetValue(node.EliteUpgradeNodeId!, out var elite) &&
                elite.Kind == ShanheNodeKind.EliteBattle &&
                elite.ChapterNumber == node.ChapterNumber,
                "An ordinary upgrade must reference an existing same-chapter elite battle profile.");
    }
}
