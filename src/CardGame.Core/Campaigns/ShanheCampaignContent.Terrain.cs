namespace CardGame.Core.Campaigns;

public static partial class ShanheCampaignContent
{
    internal static void ValidateTerrainSkill(string skillId) => Require(skillId is
        "shanhe:terrain-city" or "shanhe:terrain-battlefield" or "shanhe:terrain-river" or
        "shanhe:terrain-forest" or "shanhe:terrain-plain" or "shanhe:terrain-mountain",
        "Unknown Shanhe terrain skill.");
}
