using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentQianlongRewardId = "tactic-qianlong-current-2226";
    private static ShanheRewardDefinition CurrentQianlongReward => new(CurrentQianlongRewardId, "战法·潜龙")
    {
        TacticSkillId = StandardShanheQianlongPackage.SkillId, Rarity = 1,
        Description = "每有一个已解锁的空技能槽，出杀次数+2。"
    };

    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentQianlongShopMetadata =>
        [new(CurrentQianlongRewardId, new(2226, 200, 1, "战法"))];
    private static IEnumerable<string> CurrentQianlongDefaultUnlockedRewardIds => [CurrentQianlongRewardId];

    private static ShanheCampaignDefinition WithCurrentQianlongTactic(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Append(CurrentQianlongReward).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Append(CurrentQianlongRewardId).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop
                ? (node.Choices ?? []).Append(new("buy-" + CurrentQianlongRewardId, "战法·潜龙 · 200钱", 200,
                    RewardId: CurrentQianlongRewardId, LeavesNode: false)
                    { Description = CurrentQianlongReward.Description }).ToArray() : node.Choices
        }).ToArray()
    };

    private static bool RequiresCurrentQianlongRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(StandardShanheQianlongPackage.SkillId, StringComparer.Ordinal);
}
