using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentCuixue2SkillId = "shanhe:current-cuixue-2049";
    public const string CurrentCuixue2RewardId = "tactic-cuixue-current-2049";
    public const string CurrentYuanzhu3SkillId = "shanhe:current-yuanzhu-2078";
    public const string CurrentYuanzhu3RewardId = "tactic-yuanzhu-current-2078";
    public const string CurrentMuniuLiumaSkillId = "shanhe:current-muniu-liuma-2088";
    public const string CurrentMuniuLiumaRewardId = "tactic-muniu-liuma-current-2088";
    public const string CurrentSougua3SkillId = "shanhe:current-sougua-2170";
    public const string CurrentSougua3RewardId = "tactic-sougua-current-2170";
    public const string CurrentFenjinSkillId = "shanhe:current-fenjin-2228";
    public const string CurrentFenjinRewardId = "tactic-fenjin-current-2228";

    private static IReadOnlyList<ShanheRewardDefinition> BuildTacticRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentCuixue2RewardId, "战法·淬血Ⅱ")
        { TacticSkillId = CurrentCuixue2SkillId, Description = "史诗战法：每轮杀首次造成伤害后摸2张牌，后续杀伤和同次伤害的额外点数不重复奖励。不占额外技能槽，本次征程限获得一份。" },
        new ShanheRewardDefinition(CurrentYuanzhu3RewardId, "战法·援助Ⅲ")
        { TacticSkillId = CurrentYuanzhu3SkillId, Description = "传说战法：你的回合结束时摸3张牌，补充回合外防守手牌。不占额外技能槽，本次征程限获得一份。" },
        new ShanheRewardDefinition(CurrentMuniuLiumaRewardId, "战法·木牛流马")
        { TacticSkillId = CurrentMuniuLiumaSkillId, Description = "史诗战法：摸牌阶段摸牌数+2，手牌上限-1；技能摸牌和开场发牌不增加。不占额外技能槽，本次征程限获得一份。" },
        new ShanheRewardDefinition(CurrentSougua3RewardId, "战法·搜刮Ⅲ")
        { TacticSkillId = CurrentSougua3SkillId, Description = "史诗战法：击杀其他角色后回复4点体力，反伤击杀也走真实击杀与回复流程。不占额外技能槽，本次征程限获得一份。" },
        new ShanheRewardDefinition(CurrentFenjinRewardId, "战法·奋进")
        { TacticSkillId = CurrentFenjinSkillId, Description = "普通战法：你的回合开始时，体力大于2则失去1点体力并摸2张牌；失去体力不触发受伤反击。不占额外技能槽，本次征程限获得一份。" }
    });

    private static ShanheCampaignDefinition WithBuildTactics(ShanheCampaignDefinition definition)
    {
        var rewardIds = BuildTacticRewards.Select(reward => reward.Id).ToArray();
        var choices = BuildTacticRewards.Select(reward => new ShanheNodeChoiceDefinition(
            "buy-" + reward.Id, $"{reward.Name} · {BuildTacticPrice(reward.Id)}钱",
            GoldCost: BuildTacticPrice(reward.Id), RewardId: reward.Id, LeavesNode: false)).ToArray();
        return WithChapterRewards(definition with
        {
            Rewards = definition.Rewards.Concat(BuildTacticRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(rewardIds).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        });
    }

    private static int BuildTacticPrice(string rewardId) => rewardId switch
    {
        CurrentCuixue2RewardId or CurrentMuniuLiumaRewardId or CurrentSougua3RewardId => 500,
        CurrentYuanzhu3RewardId => 700,
        CurrentFenjinRewardId => 200,
        _ => throw new ArgumentOutOfRangeException(nameof(rewardId), rewardId, "Unknown verified build tactic.")
    };

    private static bool RequiresBuildTacticsRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is CurrentCuixue2SkillId or CurrentYuanzhu3SkillId or CurrentMuniuLiumaSkillId or CurrentSougua3SkillId or CurrentFenjinSkillId);
}
