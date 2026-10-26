using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentSanbanfuSkillId = "shanhe:current-sanbanfu-2042";
    public const string CurrentSanbanfuRewardId = "tactic-sanbanfu-current-2042";
    public const string CurrentSanbanfu2SkillId = "shanhe:current-sanbanfu-2043";
    public const string CurrentSanbanfu2RewardId = "tactic-sanbanfu-current-2043";

    private static IReadOnlyList<ShanheRewardDefinition> CurrentThirdSlashTacticRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentSanbanfuRewardId, "战法·三板斧")
        { TacticSkillId = CurrentSanbanfuSkillId, Rarity = 3, Description = "你使用的每第3张【杀】伤害+1" },
        new ShanheRewardDefinition(CurrentSanbanfu2RewardId, "战法·三板斧Ⅱ")
        { TacticSkillId = CurrentSanbanfu2SkillId, Rarity = 4, Description = "你使用的每第3张【杀】伤害+2" }
    });

    private static ShanheCampaignDefinition WithCurrentThirdSlashTactics(ShanheCampaignDefinition definition)
    {
        var ids = CurrentThirdSlashTacticRewards.Select(reward => reward.Id).ToArray();
        var choices = CurrentThirdSlashTacticRewards.Select(reward => new ShanheNodeChoiceDefinition(
            "buy-" + reward.Id, $"{reward.Name} · {ShopMetadataByRewardId[reward.Id].Price}钱",
            GoldCost: ShopMetadataByRewardId[reward.Id].Price, RewardId: reward.Id, LeavesNode: false)).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(CurrentThirdSlashTacticRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(ids).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }

    private static bool RequiresCurrentThirdSlashRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is CurrentSanbanfuSkillId or CurrentSanbanfu2SkillId);
}
