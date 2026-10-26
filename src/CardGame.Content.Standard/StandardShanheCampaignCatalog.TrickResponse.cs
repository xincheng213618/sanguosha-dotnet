using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentYinyangRewardId = "tactic-yinyang-current-2190";
    public const string CurrentSuanwuyiceRewardId = "tactic-suanwuyice-current-2248";
    private static IReadOnlyList<ShanheRewardDefinition> CurrentTrickResponseRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentYinyangRewardId, "战法·阴阳术法")
        { TacticSkillId = StandardShanheTrickResponsePackage.YinyangSkillId, Rarity = 4,
            Description = "敌方无法响应你的伤害锦囊牌。" },
        new ShanheRewardDefinition(CurrentSuanwuyiceRewardId, "战法·算无遗策")
        { TacticSkillId = StandardShanheTrickResponsePackage.SuanwuyiceSkillId, Rarity = 4,
            Description = "目标无法响应你的普通锦囊牌。" }
    });
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentTrickResponseShopMetadata =>
        [new(CurrentYinyangRewardId, new(2190, 700, 4, "战法")), new(CurrentSuanwuyiceRewardId, new(2248, 700, 4, "战法"))];
    private static IEnumerable<string> CurrentTrickResponseDefaultUnlockedRewardIds => [CurrentSuanwuyiceRewardId];
    private static IEnumerable<KeyValuePair<int, string>> CurrentTrickResponseCodexBindings => [new(150, CurrentYinyangRewardId)];

    private static ShanheCampaignDefinition WithCurrentTrickResponseTactics(ShanheCampaignDefinition definition)
    {
        var ids = CurrentTrickResponseRewards.Select(reward => reward.Id).ToArray();
        var choices = CurrentTrickResponseRewards.Select(reward => new ShanheNodeChoiceDefinition(
            "buy-" + reward.Id, reward.Name, 700, RewardId: reward.Id, LeavesNode: false)
            { Description = reward.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(CurrentTrickResponseRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(ids).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }
    private static bool RequiresCurrentTrickResponseRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is StandardShanheTrickResponsePackage.YinyangSkillId or StandardShanheTrickResponsePackage.SuanwuyiceSkillId);
}
