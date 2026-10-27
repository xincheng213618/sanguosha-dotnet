using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentCaochuanjiejianRewardId = "tactic-caochuanjiejian-current-2008";
    private static IReadOnlyList<ShanheRewardDefinition> CurrentCounterspellClaimRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentCaochuanjiejianRewardId, "战法·草船借箭")
        { TacticSkillId = StandardShanheCounterspellClaimPackage.SkillId, Rarity = 2,
            Description = "你使用【无懈可击】抵消锦囊牌的效果后，获得该锦囊牌。" }
    });
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentCounterspellClaimShopMetadata =>
        [new(CurrentCaochuanjiejianRewardId, new(2008, 300, 2, "战法"))];
    private static IEnumerable<string> CurrentCounterspellClaimDefaultUnlockedRewardIds => [CurrentCaochuanjiejianRewardId];

    private static ShanheCampaignDefinition WithCurrentCounterspellClaimTactics(ShanheCampaignDefinition definition)
    {
        var choices = CurrentCounterspellClaimRewards.Select(reward => new ShanheNodeChoiceDefinition(
            "buy-" + reward.Id, reward.Name, 300, RewardId: reward.Id, LeavesNode: false) { Description = reward.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(CurrentCounterspellClaimRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Append(CurrentCaochuanjiejianRewardId).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }
    private static bool RequiresCurrentCounterspellClaimRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(StandardShanheCounterspellClaimPackage.SkillId, StringComparer.Ordinal);
}
