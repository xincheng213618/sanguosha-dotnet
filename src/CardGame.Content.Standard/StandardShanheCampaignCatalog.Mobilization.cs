using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentMobilizationRewardId = "tactic-mobilization-current-2203";
    private static IReadOnlyList<ShanheRewardDefinition> CurrentMobilizationRewards =>
        [new(CurrentMobilizationRewardId, "战法·动员令")
        { TacticSkillId = StandardShanheMobilizationPackage.SkillId, Rarity = 3,
          Description = "队友阵亡后，下一轮开始时复活队友。" }];
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentMobilizationShopMetadata =>
        [new(CurrentMobilizationRewardId, new(2203, 500, 3, "战法"))];
    private static IEnumerable<string> CurrentMobilizationDefaultUnlockedRewardIds => [CurrentMobilizationRewardId];
    private static ShanheCampaignDefinition WithCurrentMobilizationTactic(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Concat(CurrentMobilizationRewards).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Append(CurrentMobilizationRewardId).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Append(new(
                "buy-" + CurrentMobilizationRewardId, "战法·动员令", 500, RewardId: CurrentMobilizationRewardId, LeavesNode: false)
                { Description = CurrentMobilizationRewards[0].Description }).ToArray() : node.Choices
        }).ToArray()
    };
    private static bool RequiresCurrentMobilizationRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(StandardShanheMobilizationPackage.SkillId, StringComparer.Ordinal);
}
