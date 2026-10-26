using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // Current public tactics.json schoolId=6; no same-name or rarity inference.
    private static readonly IReadOnlyDictionary<string, int> TeamGrowthSourceRewards = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        [OpeningCompanionRewardId] = 2219, [EliteOpeningCompanionRewardId] = 2220
    };

    private static ShanheCampaignDefinition WithTeamGrowth(ShanheCampaignDefinition definition, string editionId)
    {
        if (editionId != Returning2026EditionId) return definition;
        return definition with
        {
            Schools = CurrentSchools,
            // Opening-companion rows are not shop goods, but are concrete owned
            // source tactics and must participate in the same school recipe.
            Rewards = definition.Rewards.Select(reward =>
                TeamGrowthSourceRewards.TryGetValue(reward.Id, out var sourceId) &&
                CurrentTacticSchoolMembershipBySourceId.TryGetValue(sourceId, out var member)
                    ? reward with { Rarity = member.Rarity, SchoolMembership = member } : reward).ToArray()
        };
    }
}
