using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // Source rows are matched by their complete rule, not just their names.
    // All runtime skills below already belong to the normal showcase registry.
    private sealed record CurrentBuildSkillSource(
        int SourceId, string Name, string SkillId, int Price, int Rarity, int? CodexNodeId, string Description)
    {
        public string RewardId => "build-skill-" + SourceId.ToString(CultureInfo.InvariantCulture);
        public ShanheRewardDefinition Reward => new(RewardId, "传习·" + Name, SkillId: SkillId)
            { Description = Description + "占用一个额外技能槽。" };
        public ShanheShopMetadata ShopMetadata => new(SourceId, Price, Rarity, "技能");
    }

    // This projection is independent of partial-class static initialization order:
    // Codex equipment construction can request shop metadata during that process.
    private static IReadOnlyList<CurrentBuildSkillSource> CurrentBuildSkills => Array.AsReadOnly(new[]
    {
        new CurrentBuildSkillSource(3028, "裸衣", "classic:luoyi", 1000, 4, null,
            "摸牌阶段，你可以少摸一张牌，然后你本回合使用【杀】或【决斗】造成的伤害+1。"),
        new CurrentBuildSkillSource(3037, "武神", "ol:wushen", 1000, 4, null,
            "锁定技，你的红桃手牌视为【杀】。你的红桃【杀】无距离和次数限制且不能被抵消。"),
        new CurrentBuildSkillSource(3073, "蒺藜", "classic:jili", 1000, 4, null,
            "当你每回合使用或打出第X张牌时，你可以摸X张牌。（X为你的攻击范围）"),
        new CurrentBuildSkillSource(3074, "渐营", "classic:jianying", 1000, 4, null,
            "当你于出牌阶段使用牌时，若此牌的点数或花色与你本阶段使用的上一张牌相同，你可以摸一张牌。"),
        new CurrentBuildSkillSource(3075, "强识", "mobile:qiangzhi", 1000, 4, 103,
            "出牌阶段开始时，你可以展示一名其他角色一张手牌，然后当你本阶段使用与展示牌类型相同的牌时，你可以摸一张牌。"),
        new CurrentBuildSkillSource(3100, "伏骑", StandardShanheFuqiPackage.SkillId, 1000, 4, null,
            "锁定技，与你距离为1的其他角色不能使用或打出牌响应你使用的牌。"),
        new CurrentBuildSkillSource(3101, "骄恣", "classic:jiaozi", 1000, 4, null,
            "锁定技，若你的手牌数为全场唯一最多，你造成或受到的伤害值+1。"),
        new CurrentBuildSkillSource(3254, "谋溃", "ol:moukui", 1000, 4, null,
            "当你使用【杀】指定目标后，你可以选择一项：\n1.摸一张牌；\n2.弃置其一张牌。\n然后若此【杀】被【闪】抵消，其弃置你一张牌。"),
        new CurrentBuildSkillSource(3256, "乱击", "classic:luanji", 1000, 4, 110,
            "你可以将两张花色相同的手牌当【万箭齐发】使用。"),
        new CurrentBuildSkillSource(3260, "奇谋", "boundary:qimou-mobile", 1000, 4, 99,
            "限定技，出牌阶段，你可以失去任意点体力，然后你本回合计算与其他角色的距离-X且使用【杀】的限制次数+X（X为你以此法失去的体力值）。"),
        new CurrentBuildSkillSource(3263, "挽弓", StandardShanheWangongPackage.SkillId, 1000, 4, 113,
            "锁定技，若你使用的上一张牌是基本牌，你使用【杀】无距离和次数限制且造成的伤害+1。"),
        new CurrentBuildSkillSource(3264, "咆哮", "classic:paoxiao", 1000, 4, null,
            "锁定技，你使用【杀】无次数限制。"),
        new CurrentBuildSkillSource(3292, "魅步", "ol:meibu", 1000, 4, 109,
            "其他角色出牌阶段开始时，若你在其攻击范围内，你可以弃置一张牌，令其本回合视为拥有“止息”。若你弃置的牌不是【杀】或黑色锦囊牌，本回合其与你的距离视为1。"),
        new CurrentBuildSkillSource(3297, "苦肉", "classic:kujin", 1000, 4, null,
            "出牌阶段，你可以失去1点体力，然后摸两张牌。"),
        new CurrentBuildSkillSource(3040, "倾国", "classic:qingguo", 100, 1, 22,
            "你可以将一张黑色手牌当【闪】使用或打出。"),
        new CurrentBuildSkillSource(3053, "矢北", "classic:shibei", 100, 1, null,
            "锁定技，当你每回合首次受到伤害后，你回复1点体力，然后当你本回合再受到伤害后，你失去1点体力。"),
        new CurrentBuildSkillSource(3089, "御策", "classic:yuce", 300, 2, null,
            "当你受到伤害后，你可以展示一张手牌令伤害来源选择一项：\n1.弃置一张与你展示牌类型不同的手牌；2.你回复1点体力。"),
        new CurrentBuildSkillSource(3164, "绝境", "ol:juejing", 300, 2, null,
            "锁定技，你的手牌上限+2。当你进入或脱离濒死状态时，你摸一张牌。")
    });

    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentBuildSkillShopMetadata =>
        CurrentBuildSkills.Select(source => new KeyValuePair<string, ShanheShopMetadata>(source.RewardId, source.ShopMetadata));

    private static IEnumerable<KeyValuePair<int, string>> CurrentBuildSkillCodexBindings =>
        CurrentBuildSkills.Where(source => source.CodexNodeId.HasValue)
            .Select(source => new KeyValuePair<int, string>(source.CodexNodeId.GetValueOrDefault(), source.RewardId));

    // No public S5 fixed node exists for these rows. Their local stock availability
    // is explicit; this does not grant a skill or spend an extra skill slot.
    private static IEnumerable<string> CurrentBuildSkillDefaultUnlockedRewardIds =>
        CurrentBuildSkills.Where(source => source.CodexNodeId is null).Select(source => source.RewardId);

    private static ShanheCampaignDefinition WithCurrentBuildSkills(ShanheCampaignDefinition definition)
    {
        if (!definition.Id.StartsWith("shanhe:zhuhou-2026-", StringComparison.Ordinal)) return definition;
        var existing = definition.Rewards.Select(reward => reward.Id).ToHashSet(StringComparer.Ordinal);
        var additions = CurrentBuildSkills.Where(source => !existing.Contains(source.RewardId)).ToArray();
        if (additions.Length == 0) return definition;
        var rewardIds = additions.Select(source => source.RewardId).ToArray();
        var choices = additions.Select(source => new ShanheNodeChoiceDefinition(
            "buy-" + source.RewardId, $"传习·{source.Name} · {source.Price}钱", GoldCost: source.Price,
            RewardId: source.RewardId, LeavesNode: false)
        { Description = source.Reward.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(additions.Select(source => source.Reward)).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(rewardIds).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }
}
