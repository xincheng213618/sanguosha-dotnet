using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // Source rows are matched by their complete rule, not just their names.
    // Reuse ordinary definitions where exact; distinct S5 rules have conditional packages.
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
        new CurrentBuildSkillSource(3023, "龙魂", StandardShanheLonghunPackage.SkillId, 1000, 4, 107,
            "你可以将至多两张花色相同的牌按以下规则使用或打出：\n•红桃当【桃】;•方片当火【杀】;\n•梅花当【闪】;•黑桃当【无懈可击】。\n若你以此法转化使用了两张：\n•红色牌，此牌回复值或伤害值+1;\n•黑色牌，你弃置当前回合角色一张牌。"),
        new CurrentBuildSkillSource(3081, "遗计", StandardShanheYijiPackage.SkillId, 600, 3, null,
            "当你受到1点伤害后，你可以观看牌堆顶的两张牌，然后将这些牌交给任意角色。"),
        new CurrentBuildSkillSource(3083, "节命", StandardShanheCappedRefillSkillsPackage.SkillId, 600, 3, 92,
            "当你受到1点伤害后，你可以令一名角色将手牌摸至X张（X为其体力上限且至多为5）。"),
        new CurrentBuildSkillSource(3205, "焚城", "classic:fencheng", 100, 1, 24,
            "限定技，出牌阶段，你可以令所有其他角色依次选择一项：\n1.弃置任意张牌（须比上家弃置的牌多）；\n2.受到你造成的2点火焰伤害。"),
        new CurrentBuildSkillSource(3080, "奸雄", "ol:jianxiong", 600, 3, null,
            "当你受到伤害后，你可以摸一张牌，并获得造成此伤害的牌。"),
        new CurrentBuildSkillSource(3184, "权计", "classic:quanji", 100, 1, null,
            "当你受到1点伤害后，你可以摸一张牌，然后将一张手牌置于你的武将牌上，称为“权”。你每有一张“权”，你的手牌上限便+1。"),
        new CurrentBuildSkillSource(3095, "天香", "boundary:tianxiang", 600, 3, null,
            "当你受到伤害时，你可以弃置一张红桃牌防止之并选择一项，令一名其他角色：\n1.受到伤害来源的1点伤害并摸X张牌（X为其已损失体力值且至多为5）；\n2.失去1点体力并获得你弃置的牌。"),
        new CurrentBuildSkillSource(3094, "涅槃", StandardShanheS5YuduPackage.NiepanId, 600, 3, null,
            "限定技，当你处于濒死状态时，你可以弃置你区域内里的所有牌，复原你的武将牌，摸三张牌，回复至3点体力。"),
        new CurrentBuildSkillSource(3050, "英魂", "boundary:yinghun", 100, 1, null,
            "准备阶段，若你已受伤，你可以选择一名其他角色并选择一项：\n1.令其摸X张牌，然后弃置一张牌；\n2.令其摸一张牌，然后弃置X张牌。（X为你已损失体力值）"),
        new CurrentBuildSkillSource(3140, "悲歌", StandardShanheBeigeSkillsPackage.SkillId, 300, 2, null,
            "当一名角色受到【杀】的伤害后，你可以弃置一张牌令其判定，若结果为：\n•红桃，其回复1点体力；\n•方片，其摸两张牌；\n•梅花，伤害来源弃置两张牌；\n•黑桃，伤害来源翻面。"),
        new CurrentBuildSkillSource(3079, "明哲", "mobile:mingzhe", 600, 3, null,
            "当你于回合外使用、打出、弃置或被弃置一张红色牌时，你可以摸一张牌。"),
        new CurrentBuildSkillSource(3216, "复难", "classic:funan", 100, 1, null,
            "当其他角色响应你使用的牌时，你可以令其获得此牌，其本回合不能使用或打出此牌，然后你获得其响应时使用或打出的牌。"),
        new CurrentBuildSkillSource(3255, "秘计", "mobile:miji", 600, 3, null,
            "准备阶段和结束阶段，若你已受伤，你可以判定，若结果为黑色，你观看牌堆顶X张牌并令一名角色获得之（X为你已损失体力值）。"),
        new CurrentBuildSkillSource(3088, "贞烈", "mobile:zhenlie", 100, 1, null,
            "当你的判定牌生效前，你可以从牌堆顶亮出一张牌代替之。"),
        new CurrentBuildSkillSource(3188, "陷嗣", "classic:xiansi", 100, 1, 30,
            "准备阶段，你可以将至多两名角色的各一张牌置于你的武将牌上，称为“逆”。其他角色可以移去两张“逆”视为对你使用一张【杀】。"),
        new CurrentBuildSkillSource(3084, "筹策", "ol:chouce", 300, 2, null,
            "当你受到1点伤害后，你可以判定，若结果为：\n•红色，你令一名角色摸一张牌，若其为“先辅”目标，改为摸两张牌；\n•黑色，你弃置一名角色区域里的一张牌。"),
        new CurrentBuildSkillSource(3054, "良助", "ol:liangzhu", 600, 3, 61,
            "当一名角色于其出牌阶段内回复体力后，你可以选择一项：1.摸一张牌；2.令其摸两张牌。"),
        new CurrentBuildSkillSource(3113, "庸肆", "classic:yongsi", 300, 2, null,
            "锁定技，摸牌阶段，你多摸X张牌；弃牌阶段开始时，你弃置X张牌。（X为全场势力数）"),
        new CurrentBuildSkillSource(3148, "鸩毒", "ol:zhendu", 300, 2, 49,
            "每名角色出牌阶段开始时，你可以弃置一张手牌，然后其视为使用一张【酒】，若其不是你，你对其造成1点伤害。"),
        new CurrentBuildSkillSource(3204, "急攻", "classic:jigong", 300, 2, null,
            "出牌阶段开始时，你可以摸两张牌，然后你本回合手牌上限等于你本阶段造成的伤害值。"),
        new CurrentBuildSkillSource(3002, "下书", "ol:xiashu", 100, 1, null,
            "出牌阶段开始时，你可以交给一名其他角色所有手牌，然后其展示任意张手牌，你选择获得其：\n1.以此法展示的牌；\n2.未以此法展示的手牌。"),
        new CurrentBuildSkillSource(3104, "竭缘", "ol:jieyuan", 600, 3, null,
            "当你对其他角色造成伤害时，若其体力值大于等于你，你可以弃置一张黑色手牌令此伤害+1；\n当其他角色对你造成伤害时，若其体力值大于等于你，你可以弃置一张红色手牌令此伤害-1。"),
        new CurrentBuildSkillSource(3149, "戚乱", "ol:qiluan", 100, 1, null,
            "每回合结束后，你可以摸与本回合死亡角色数等量张牌，每有一名角色被你杀死，你额外摸两张牌。"),
        new CurrentBuildSkillSource(3198, "诛害", "boundary:zhuhai-current", 100, 1, null,
            "其他角色的结束阶段，若其本回合造成过伤害，你可以对其使用一张【杀】。"),
        new CurrentBuildSkillSource(3238, "断发", "mobile:duanfa", 300, 2, 58,
            "出牌阶段，你可以弃置任意张黑色牌，然后摸等量张牌。你本阶段以此法弃置的总牌数不能大于你的体力上限。"),
        new CurrentBuildSkillSource(3049, "青囊", "classic:qingnang", 100, 1, null,
            "出牌阶段限一次，你可以弃置一张手牌令一名角色回复1点体力。"),
        new CurrentBuildSkillSource(3052, "刚烈", "classic:ganglie", 300, 2, 43,
            "当你受到伤害后，你可以判定，若结果不为红桃，伤害来源选择弃置两张手牌或受到1点伤害。"),
        new CurrentBuildSkillSource(3271, "英姿", "boundary:yingzi", 300, 2, null,
            "锁定技，摸牌阶段，你多摸一张牌。你的手牌上限为你的体力上限。"),
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
        new CurrentBuildSkillSource(3089, "御策", "classic:yuce", 300, 2, 38,
            "当你受到伤害后，你可以展示一张手牌令伤害来源选择一项：\n1.弃置一张与你展示牌类型不同的手牌；2.你回复1点体力。"),
        new CurrentBuildSkillSource(3164, "绝境", "ol:juejing", 300, 2, null,
            "锁定技，你的手牌上限+2。当你进入或脱离濒死状态时，你摸一张牌。"),
        new CurrentBuildSkillSource(3087, "恩怨", "classic:enyuan", 300, 2, 39,
            "当你获得一名其他角色至少两张牌后，你可以令其摸一张牌。当你受到1点伤害后，你可以令伤害来源选择一项：\n1.交给你一张手牌；2.失去1点体力。"),
        new CurrentBuildSkillSource(3021, "奇袭", "classic:qixi", 300, 2, null,
            "你可以将一张黑色牌当【过河拆桥】使用。"),
        new CurrentBuildSkillSource(3029, "无双", "classic:wushuang", 600, 3, null,
            "锁定技，你使用的【杀】需两张【闪】才能抵消；与你【决斗】的角色每次需打出两张【杀】。"),
        new CurrentBuildSkillSource(3120, "忘隙", "classic:wangxi", 600, 3, null,
            "当你对其他角色造成1点伤害后，或受到其他角色的1点伤害后，你可以与其各摸一张牌。"),
        new CurrentBuildSkillSource(3103, "暗箭", "classic:anjian", 300, 2, null,
            "锁定技，当你使用【杀】对目标角色造成伤害时，若你不在其攻击范围内，此伤害+1。"),
        new CurrentBuildSkillSource(3096, "克己", "classic:keji", 100, 1, null,
            "若你未于本回合出牌阶段使用或打出过【杀】，你可以跳过弃牌阶段。"),
        new CurrentBuildSkillSource(3137, "红颜", "classic:hongyan", 100, 1, null,
            "锁定技，你的黑桃牌视为红桃牌。"),
        new CurrentBuildSkillSource(3061, "恂恂", "boundary:xunxun", 100, 1, null,
            "摸牌阶段开始时，你可以观看牌堆顶的四张牌，然后将其中两张牌置于牌堆顶，将剩余牌置于牌堆底。"),
        new CurrentBuildSkillSource(3082, "反馈", "boundary:feedback", 300, 2, null,
            "当你受到1点伤害后，你可以获得伤害来源的一张牌。"),
        new CurrentBuildSkillSource(3116, "落英", "boundary:luoying", 300, 2, null,
            "当其他角色的梅花牌因弃置或判定而置入弃牌堆后，你可以获得其中任意张牌。"),
        new CurrentBuildSkillSource(3162, "涉猎", "classic:shelie", 100, 1, null,
            "摸牌阶段，你可以改为亮出牌堆顶的五张牌，然后获得每种花色的牌各一张。"),
        new CurrentBuildSkillSource(3163, "攻心", "classic:gongxin", 300, 2, null,
            "出牌阶段限一次，你可以观看一名其他角色的手牌，然后你可以展示其中一张红桃牌并选择一项：\n1.弃置此牌；2.将此牌置于牌堆顶。"),
        new CurrentBuildSkillSource(3026, "铁骑", "classic:tieqi", 300, 2, null,
            "当你使用【杀】指定目标后，你可以判定，若为红色，其不能使用【闪】响应此【杀】。"),
        new CurrentBuildSkillSource(3032, "猛进", "classic:mengjin", 300, 2, null,
            "当你的【杀】被目标角色【闪】抵消后，你可以弃置其一张牌。"),
        new CurrentBuildSkillSource(3174, "禁酒", "classic:jinjiu", 100, 1, null,
            "锁定技，你的【酒】视为【杀】。"),
        new CurrentBuildSkillSource(3105, "享乐", "classic:xiangle", 300, 2, null,
            "锁定技，当你成为一名角色使用【杀】的目标后，除非其弃置一张基本牌，否则令此【杀】对你无效。"),
        new CurrentBuildSkillSource(3109, "毅重", "classic:yizhong", 300, 2, null,
            "锁定技，若你的装备区里没有防具牌，黑色【杀】对你无效。"),
        new CurrentBuildSkillSource(3121, "肉林", "classic:roulin", 100, 1, null,
            "锁定技，你对女性角色使用的【杀】和女性角色对你使用的【杀】均需使用两张【闪】才能抵消。"),
        new CurrentBuildSkillSource(3066, "琴音", "classic:qinyin", 100, 1, null,
            "弃牌阶段结束时，若你本阶段弃置过至少两张手牌，你可以选择令所有角色：\n1.各回复1点体力；2.各失去1点体力。"),
        new CurrentBuildSkillSource(3068, "绝策", "classic:juece", 100, 1, 25,
            "结束阶段，你可以对一名没有手牌的其他角色造成1点伤害。"),
        new CurrentBuildSkillSource(3158, "推锋", "ol:tuifeng", 600, 3, null,
            "当你受到1点伤害后，你可以将一张牌置于你的武将牌上，称为“锋”。准备阶段，你移去所有“锋”，摸两倍数量的牌，你本回合使用【杀】的限制次数+X（X为你本回合移去“锋”的数量）。"),
        new CurrentBuildSkillSource(3086, "称象", "classic:chengxiang", 600, 3, 76,
            "当你受到伤害后，你可以亮出牌堆顶四张牌，获得其中任意张点数和小于等于13的牌。"),
        new CurrentBuildSkillSource(3201, "樵拾", "classic:qiaoshi", 100, 1, null,
            "其他角色的结束阶段，若你与其手牌数相等，你可以与其各摸一张牌。"),
        new CurrentBuildSkillSource(3144, "甚贤", "ol:shenxian", 100, 1, null,
            "每回合限一次，当其他角色在你的回合外因弃置而失去基本牌后，你可以摸一张牌。"),
        new CurrentBuildSkillSource(3208, "瑰藻", "classic:guizao", 100, 1, null,
            "若你于弃牌阶段弃置过至少两张牌且这些牌花色均不相同，你可以回复1点体力或摸一张牌。"),
        new CurrentBuildSkillSource(3171, "励战", "ol:lizhan", 100, 1, null,
            "结束阶段，你可以令任意名已受伤角色摸一张牌。")
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
