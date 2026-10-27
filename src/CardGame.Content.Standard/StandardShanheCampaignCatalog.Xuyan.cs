using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentXuyanRewardId = "tactic-xuyan-current-2004";
    private static ShanheRewardDefinition CurrentXuyanReward => new(CurrentXuyanRewardId, "战法·虚焰")
    {
        TacticSkillId = StandardShanheXuyanPackage.SkillId, Rarity = 2,
        Description = "你使用【火攻】时，将弃置同花色手牌改为展示。"
    };
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentXuyanShopMetadata =>
        [new(CurrentXuyanRewardId, new(2004, 300, 2, "战法"))];
    private static IEnumerable<string> CurrentXuyanDefaultUnlockedRewardIds => [CurrentXuyanRewardId];

    private static ShanheCampaignDefinition WithCurrentXuyanTactic(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Append(CurrentXuyanReward).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Append(CurrentXuyanRewardId).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop
                ? (node.Choices ?? []).Append(new("buy-" + CurrentXuyanRewardId, "战法·虚焰", 300,
                    RewardId: CurrentXuyanRewardId, LeavesNode: false) { Description = CurrentXuyanReward.Description }).ToArray()
                : node.Choices
        }).ToArray()
    };

    private static bool RequiresCurrentXuyanRegistry(ShanheBattleSetup setup) => setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
        .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
        .Contains(StandardShanheXuyanPackage.SkillId, StringComparer.Ordinal);
}
