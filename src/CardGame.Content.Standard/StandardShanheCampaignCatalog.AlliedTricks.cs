using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentFengshounianRewardId = "tactic-fengshounian-current-2241";
    public const string CurrentJiemengRewardId = "tactic-jiemeng-current-2243";
    private static IReadOnlyList<ShanheRewardDefinition> CurrentAlliedTrickRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentFengshounianRewardId, "战法·丰收年")
        { TacticSkillId = StandardShanheAlliedTricksPackage.FengshounianSkillId, Rarity = 1,
            Description = "你使用的【五谷丰登】仅友方角色可以获得牌。" },
        new ShanheRewardDefinition(CurrentJiemengRewardId, "战法·结盟")
        { TacticSkillId = StandardShanheAlliedTricksPackage.JiemengSkillId, Rarity = 1,
            Description = "你使用的【桃园结义】友方角色回复双倍体力。" }
    });
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentAlliedTrickShopMetadata =>
        [new(CurrentFengshounianRewardId, new(2241, 200, 1, "战法")), new(CurrentJiemengRewardId, new(2243, 200, 1, "战法"))];
    private static IEnumerable<string> CurrentAlliedTrickDefaultUnlockedRewardIds => CurrentAlliedTrickRewards.Select(reward => reward.Id);

    private static ShanheCampaignDefinition WithCurrentAlliedTrickTactics(ShanheCampaignDefinition definition)
    {
        var ids = CurrentAlliedTrickRewards.Select(reward => reward.Id).ToArray();
        var choices = CurrentAlliedTrickRewards.Select(reward => new ShanheNodeChoiceDefinition(
            "buy-" + reward.Id, reward.Name, 200, RewardId: reward.Id, LeavesNode: false)
            { Description = reward.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(CurrentAlliedTrickRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(ids).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }
    private static bool RequiresCurrentAlliedTrickRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is StandardShanheAlliedTricksPackage.FengshounianSkillId or StandardShanheAlliedTricksPackage.JiemengSkillId);
}
