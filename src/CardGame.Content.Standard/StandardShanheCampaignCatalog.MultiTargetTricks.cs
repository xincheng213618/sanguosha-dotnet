using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentHengjiangRewardId = "tactic-hengjiang-current-2007";
    public const string CurrentDantiaowangRewardId = "tactic-dantiaowang-current-2212";
    private static IReadOnlyList<ShanheRewardDefinition> CurrentMultiTargetTrickRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentHengjiangRewardId, "战法·横江锁")
        { TacticSkillId = StandardShanheMultiTargetTricksPackage.HengjiangSkillId, Rarity = 3,
            Description = "【铁索连环】能指定任意个目标。" },
        new ShanheRewardDefinition(CurrentDantiaowangRewardId, "战法·单挑王")
        { TacticSkillId = StandardShanheMultiTargetTricksPackage.DantiaowangSkillId, Rarity = 2,
            Description = "你使用【决斗】可以指定任意个目标。" }
    });
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentMultiTargetTrickShopMetadata =>
        [new(CurrentHengjiangRewardId, new(2007, 500, 3, "战法")), new(CurrentDantiaowangRewardId, new(2212, 300, 2, "战法"))];
    // Neither source has a fixed node in the pinned S5 tree.
    private static IEnumerable<string> CurrentMultiTargetTrickDefaultUnlockedRewardIds => CurrentMultiTargetTrickRewards.Select(reward => reward.Id);

    private static ShanheCampaignDefinition WithCurrentMultiTargetTrickTactics(ShanheCampaignDefinition definition)
    {
        var ids = CurrentMultiTargetTrickRewards.Select(reward => reward.Id).ToArray();
        var choices = CurrentMultiTargetTrickRewards.Select(reward => new ShanheNodeChoiceDefinition(
            "buy-" + reward.Id, reward.Name, ShopMetadataByRewardId[reward.Id].Price,
            RewardId: reward.Id, LeavesNode: false) { Description = reward.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(CurrentMultiTargetTrickRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(ids).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }
    private static bool RequiresCurrentMultiTargetTrickRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is StandardShanheMultiTargetTricksPackage.HengjiangSkillId or StandardShanheMultiTargetTricksPackage.DantiaowangSkillId);
}
