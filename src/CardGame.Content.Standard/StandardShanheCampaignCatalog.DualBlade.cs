using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentDualBladeRewardId = "tactic-dual-blade-current-2033";
    public const string CurrentDualBlade2RewardId = "tactic-dual-blade-current-2034";
    private static IReadOnlyList<ShanheRewardDefinition> CurrentDualBladeRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentDualBladeRewardId, "战法·双刃")
        { TacticSkillId = StandardShanheDualBladePackage.SkillId, Rarity = 2,
            Description = "你每轮首张【杀】可以额外选择1个目标。" },
        new ShanheRewardDefinition(CurrentDualBlade2RewardId, "战法·双刃Ⅱ")
        { TacticSkillId = StandardShanheDualBladePackage.UpgradedSkillId, Rarity = 3,
            Description = "你每轮首张【杀】可以额外选择2个目标。" }
    });
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentDualBladeShopMetadata =>
        [new(CurrentDualBladeRewardId, new(2033, 300, 2, "战法")), new(CurrentDualBlade2RewardId, new(2034, 500, 3, "战法"))];
    // Neither source has a fixed node in the pinned current S5 tree.
    private static IEnumerable<string> CurrentDualBladeDefaultUnlockedRewardIds => CurrentDualBladeRewards.Select(reward => reward.Id);

    private static ShanheCampaignDefinition WithCurrentDualBladeTactics(ShanheCampaignDefinition definition)
    {
        var ids = CurrentDualBladeRewards.Select(reward => reward.Id).ToArray();
        var choices = CurrentDualBladeRewards.Select(reward => new ShanheNodeChoiceDefinition(
            "buy-" + reward.Id, reward.Name, ShopMetadataByRewardId[reward.Id].Price,
            RewardId: reward.Id, LeavesNode: false) { Description = reward.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(CurrentDualBladeRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(ids).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }
    private static bool RequiresCurrentDualBladeRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is StandardShanheDualBladePackage.SkillId or StandardShanheDualBladePackage.UpgradedSkillId);
}
