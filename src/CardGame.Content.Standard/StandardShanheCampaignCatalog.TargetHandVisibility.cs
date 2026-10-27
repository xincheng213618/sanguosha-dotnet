using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentYanxianRewardId = "tactic-yanxian-current-2005";
    public const string CurrentQiangquHaoduoRewardId = "tactic-qiangqu-haoduo-current-2006";
    private static IReadOnlyList<ShanheRewardDefinition> CurrentTargetHandVisibilityRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentYanxianRewardId, "战法·眼线")
        { TacticSkillId = StandardShanheTargetHandVisibilityPackage.YanxianSkillId, Rarity = 2,
            Description = "【过河拆桥】时目标手牌可见。" },
        new ShanheRewardDefinition(CurrentQiangquHaoduoRewardId, "战法·强取豪夺")
        { TacticSkillId = StandardShanheTargetHandVisibilityPackage.QiangquHaoduoSkillId, Rarity = 3,
            Description = "【顺手牵羊】时目标手牌可见。" }
    });
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentTargetHandVisibilityShopMetadata =>
        [new(CurrentYanxianRewardId, new(2005, 300, 2, "战法")),
         new(CurrentQiangquHaoduoRewardId, new(2006, 500, 3, "战法"))];
    private static IEnumerable<string> CurrentTargetHandVisibilityDefaultUnlockedRewardIds =>
        CurrentTargetHandVisibilityRewards.Select(reward => reward.Id);

    private static ShanheCampaignDefinition WithCurrentTargetHandVisibilityTactics(ShanheCampaignDefinition definition)
    {
        var ids = CurrentTargetHandVisibilityRewards.Select(reward => reward.Id).ToArray();
        var choices = CurrentTargetHandVisibilityRewards.Select(reward => new ShanheNodeChoiceDefinition(
            "buy-" + reward.Id, reward.Name, reward.Rarity == 2 ? 300 : 500,
            RewardId: reward.Id, LeavesNode: false) { Description = reward.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(CurrentTargetHandVisibilityRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(ids).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }
    private static bool RequiresCurrentTargetHandVisibilityRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is StandardShanheTargetHandVisibilityPackage.YanxianSkillId or
                StandardShanheTargetHandVisibilityPackage.QiangquHaoduoSkillId);
}
