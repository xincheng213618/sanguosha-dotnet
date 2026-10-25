using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentTouxiSkillId = "shanhe:current-touxi-2009";
    public const string CurrentTouxiRewardId = "tactic-touxi-current-2009";
    public const string CurrentTiebushan4SkillId = "shanhe:current-tiebushan-2074";
    public const string CurrentTiebushan4RewardId = "tactic-tiebushan-current-2074";
    public const string CurrentJingjijiaSkillId = "shanhe:current-jingjijia-2116";
    public const string CurrentJingjijiaRewardId = "tactic-jingjijia-current-2116";
    public const string CurrentPianzhuanjiaSkillId = "shanhe:current-pianzhuanjia-2117";
    public const string CurrentPianzhuanjiaRewardId = "tactic-pianzhuanjia-current-2117";

    private static IReadOnlyList<ShanheRewardDefinition> CurrentLegendaryTacticRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentTouxiRewardId, "战法·偷袭")
        { TacticSkillId = CurrentTouxiSkillId, Description = "传说战法：使用黑桃杀无次数限制；其他花色仍按实际出杀额度。不占额外技能槽，本次征程限获得一份。" },
        new ShanheRewardDefinition(CurrentTiebushan4RewardId, "战法·铁布衫Ⅳ")
        { TacticSkillId = CurrentTiebushan4SkillId, Description = "传说战法：每场战斗开局获得6点护甲，随后按原生护甲规则抵挡伤害。不占额外技能槽，本次征程限获得一份。" },
        new ShanheRewardDefinition(CurrentJingjijiaRewardId, "战法·荆棘甲")
        { TacticSkillId = CurrentJingjijiaSkillId, Description = "传说战法：每次实际受到伤害后，对该次伤害的来源造成1点普通伤害；无来源伤害没有反伤对象。不占额外技能槽，本次征程限获得一份。" },
        new ShanheRewardDefinition(CurrentPianzhuanjiaRewardId, "战法·偏转甲")
        { TacticSkillId = CurrentPianzhuanjiaSkillId, Description = "传说战法：每次实际受到伤害后，对一名随机存活敌方造成1点普通伤害；无存活敌方时跳过。不占额外技能槽，本次征程限获得一份。" }
    });

    private static ShanheCampaignDefinition WithCurrentLegendaryTactics(ShanheCampaignDefinition definition)
    {
        var rewardIds = CurrentLegendaryTacticRewards.Select(reward => reward.Id).ToArray();
        var choices = CurrentLegendaryTacticRewards.Select(reward => new ShanheNodeChoiceDefinition(
            "buy-" + reward.Id, reward.Name + " · 700钱", GoldCost: 700, RewardId: reward.Id, LeavesNode: false)).ToArray();
        return WithChapterRewards(definition with
        {
            Rewards = definition.Rewards.Concat(CurrentLegendaryTacticRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(rewardIds).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        });
    }

    private static bool RequiresCurrentLegendaryTacticsRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is CurrentTouxiSkillId or CurrentTiebushan4SkillId or CurrentJingjijiaSkillId or CurrentPianzhuanjiaSkillId);
}
