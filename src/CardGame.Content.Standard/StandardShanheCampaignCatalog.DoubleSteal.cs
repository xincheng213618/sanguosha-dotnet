using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentDoubleStealRewardId = "tactic-double-steal-current-2337";
    private static ShanheRewardDefinition CurrentDoubleStealReward => new(CurrentDoubleStealRewardId, "战法·双掠")
    {
        TacticSkillId = StandardShanheDoubleStealPackage.SkillId, Rarity = 2,
        Description = "你使用的【顺手牵羊】额外结算一次。"
    };

    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentDoubleStealShopMetadata =>
        [new(CurrentDoubleStealRewardId, new(2337, 300, 2, "战法"))];

    // The pinned current S5 tree has no source-2337 node.
    private static IEnumerable<string> CurrentDoubleStealDefaultUnlockedRewardIds => [CurrentDoubleStealRewardId];

    private static ShanheCampaignDefinition WithCurrentDoubleStealTactic(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Append(CurrentDoubleStealReward).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Append(CurrentDoubleStealRewardId).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop
                ? (node.Choices ?? []).Append(new("buy-" + CurrentDoubleStealRewardId, "战法·双掠 · 300钱", 300,
                    RewardId: CurrentDoubleStealRewardId, LeavesNode: false)
                    { Description = CurrentDoubleStealReward.Description }).ToArray() : node.Choices
        }).ToArray()
    };

    private static bool RequiresCurrentDoubleStealRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(StandardShanheDoubleStealPackage.SkillId, StringComparer.Ordinal);
}
