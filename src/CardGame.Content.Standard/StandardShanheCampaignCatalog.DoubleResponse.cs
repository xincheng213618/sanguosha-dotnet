using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentHanzhanRewardId = "tactic-hanzhan-current-2111";
    public const string CurrentXulijianRewardId = "tactic-xulijian-current-2242";
    private static IReadOnlyList<ShanheRewardDefinition> CurrentDoubleResponseRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentHanzhanRewardId, "战法·酣战")
        { TacticSkillId = StandardShanheDoubleResponsePackage.HanzhanSkillId, Rarity = 2,
            Description = "与你【决斗】的角色每次需打出两张【杀】。" },
        new ShanheRewardDefinition(CurrentXulijianRewardId, "战法·蓄力箭")
        { TacticSkillId = StandardShanheDoubleResponsePackage.XulijianSkillId, Rarity = 2,
            Description = "你使用的【万箭齐发】其他角色需要打出2张【闪】来响应。" }
    });
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentDoubleResponseShopMetadata =>
        [new(CurrentHanzhanRewardId, new(2111, 300, 2, "战法")), new(CurrentXulijianRewardId, new(2242, 300, 2, "战法"))];
    // Neither source has a fixed node in the pinned current S5 tree.
    private static IEnumerable<string> CurrentDoubleResponseDefaultUnlockedRewardIds => CurrentDoubleResponseRewards.Select(r => r.Id);

    private static ShanheCampaignDefinition WithCurrentDoubleResponseTactics(ShanheCampaignDefinition definition)
    {
        var ids = CurrentDoubleResponseRewards.Select(r => r.Id).ToArray();
        var choices = CurrentDoubleResponseRewards.Select(r => new ShanheNodeChoiceDefinition(
            "buy-" + r.Id, r.Name, ShopMetadataByRewardId[r.Id].Price, RewardId: r.Id, LeavesNode: false)
            { Description = r.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(CurrentDoubleResponseRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(ids).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }
    private static bool RequiresCurrentDoubleResponseRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is StandardShanheDoubleResponsePackage.HanzhanSkillId or StandardShanheDoubleResponsePackage.XulijianSkillId);
}
