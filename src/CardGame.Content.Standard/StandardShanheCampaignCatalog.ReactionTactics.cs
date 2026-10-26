using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentJiyiSkillId = "shanhe:current-jiyi-2171";
    public const string CurrentJiyi2SkillId = "shanhe:current-jiyi-2172";
    public const string CurrentJiyi3SkillId = "shanhe:current-jiyi-2173";
    public const string CurrentCuixueSkillId = "shanhe:current-cuixue-2048";
    public const string CurrentXialuXiangfengSkillId = "shanhe:current-xialu-xiangfeng-2028";
    public const string CurrentZhongjijiSkillId = "shanhe:current-zhongjiji-2130";
    public const string CurrentCedingTianxiaSkillId = "shanhe:current-ceding-tianxia-2085";
    public const string CurrentCedingTianxia2SkillId = "shanhe:current-ceding-tianxia-2086";
    public const string CurrentYuanzhuSkillId = "shanhe:current-yuanzhu-2076";
    public const string CurrentYuanzhu2SkillId = "shanhe:current-yuanzhu-2077";
    public const string CurrentSouguaSkillId = "shanhe:current-sougua-2168";
    public const string CurrentSougua2SkillId = "shanhe:current-sougua-2169";

    private sealed record ReactionTacticSource(int SourceId, string Name, string SkillId, int Price, int Rarity,
        int? CodexNodeId, string Description, bool BossBattleOnly = false, string? RewardIdOverride = null)
    {
        public string RewardId => RewardIdOverride ?? "tactic-build-" + SourceId.ToString(CultureInfo.InvariantCulture);
        public ShanheRewardDefinition Reward => new(RewardId, "战法·" + Name)
        { TacticSkillId = SkillId, BossBattleOnly = BossBattleOnly, Description = Description + "不占额外技能槽，本次征程限获得一份。" };
    }

    // Keep projections independent of partial-class static initialization order.
    private static IReadOnlyList<ReactionTacticSource> CurrentReactionTactics => Array.AsReadOnly(new[]
    {
        new ReactionTacticSource(2071, "铁布衫", "shanhe:current-tiebushan-2071", 200, 1, null,
            "游戏开始时，你获得1点护甲"),
        new ReactionTacticSource(2072, "铁布衫Ⅱ", "shanhe:current-tiebushan-2072", 300, 2, null,
            "游戏开始时，你获得2点护甲"),
        new ReactionTacticSource(2073, "铁布衫Ⅲ", "shanhe:current-tiebushan-2073", 500, 3, null,
            "游戏开始时，你获得4点护甲"),
        new ReactionTacticSource(2069, "护甲Ⅰ", "shanhe:current-hujia-2069", 300, 2, null,
            "每轮开始时，你获得1点护甲"),
        new ReactionTacticSource(2070, "护甲Ⅱ", "shanhe:current-hujia-2070", 500, 3, null,
            "每轮开始时，你获得2点护甲"),
        new ReactionTacticSource(2054, "体魄Ⅰ", "shanhe:current-tipo-2054", 300, 2, null,
            "游戏开始时，你增加1点体力上限并回复等量体力"),
        new ReactionTacticSource(2055, "体魄Ⅱ", "shanhe:current-tipo-2055", 500, 3, null,
            "游戏开始时，你增加2点体力上限并回复等量体力"),
        new ReactionTacticSource(2056, "体魄Ⅲ", "shanhe:current-tipo-2056", 700, 4, 102,
            "游戏开始时，你增加3点体力上限并回复等量体力"),
        new ReactionTacticSource(2048, "淬血", CurrentCuixueSkillId, 200, 1, null,
            "你每轮【杀】首次造成伤害后摸1张牌"),
        new ReactionTacticSource(2028, "狭路相逢", CurrentXialuXiangfengSkillId, 200, 1, null,
            "受到【决斗】伤害后回复1点体力"),
        new ReactionTacticSource(2130, "重击技", CurrentZhongjijiSkillId, 300, 2, null,
            "对敌方造成伤害一次大于等于3点时，摸1张牌"),
        new ReactionTacticSource(2085, "策定天下", CurrentCedingTianxiaSkillId, 300, 2, null,
            "出牌阶段限一次,你使用锦囊牌造成伤害后,摸1张牌"),
        new ReactionTacticSource(2086, "策定天下Ⅱ", CurrentCedingTianxia2SkillId, 500, 3, null,
            "出牌阶段限一次,你使用锦囊牌造成伤害后,摸2张牌"),
        new ReactionTacticSource(2076, "援助", CurrentYuanzhuSkillId, 300, 2, null,
            "回合结束时，你摸1张牌"),
        new ReactionTacticSource(2077, "援助Ⅱ", CurrentYuanzhu2SkillId, 500, 3, null,
            "回合结束时，你摸2张牌"),
        new ReactionTacticSource(2168, "搜刮Ⅰ", CurrentSouguaSkillId, 200, 1, null,
            "当你击杀其他角色后，你回复1点体力值"),
        new ReactionTacticSource(2169, "搜刮Ⅱ", CurrentSougua2SkillId, 300, 2, null,
            "当你击杀其他角色后，你回复2点体力值"),
        new ReactionTacticSource(2171, "技艺Ⅰ", CurrentJiyiSkillId, 500, 3, 69,
            "当你的技能直接造成伤害时，此伤害+1"),
        new ReactionTacticSource(2172, "技艺Ⅱ", CurrentJiyi2SkillId, 700, 4, 123,
            "当你的技能直接造成伤害时，此伤害+2"),
        new ReactionTacticSource(2173, "技艺Ⅲ", CurrentJiyi3SkillId, 700, 4, null,
            "当你的技能直接造成伤害时，此伤害+3"),
        new ReactionTacticSource(2177, "隔山打牛", StandardShanheArmorBreakTacticsPackage.GeshanDaniuSkillId, 700, 4, 126,
            "你对其他人造成伤害时，无视其护甲"),
        new ReactionTacticSource(2193, "剔甲术", StandardShanheArmorBreakTacticsPackage.TijiaShuSkillId, 500, 3, null,
            "对护甲造成双倍伤害"),
        new ReactionTacticSource(2127, "卧薪尝胆", StandardShanheWoxinPackage.SkillId, 500, 3, null,
            "回合外每受到1次伤害，你的下次造成伤害+1"),
        new ReactionTacticSource(2059, "摸牌Ⅰ", StandardShanheDrawTacticsPackage.MopaiSkillId, 300, 2, null,
            "摸牌阶段，你的摸牌数+1"),
        new ReactionTacticSource(2060, "摸牌Ⅱ", StandardShanheDrawTacticsPackage.Mopai2SkillId, 500, 3, null,
            "摸牌阶段，你的摸牌数+2"),
        new ReactionTacticSource(2061, "急行军", StandardShanheDrawTacticsPackage.JixingjunSkillId, 200, 1, null,
            "摸牌阶段，你的摸牌数+1"),
        new ReactionTacticSource(2064, "筹备", StandardShanhePreparationTacticsPackage.ChoubeiSkillId, 200, 1, null,
            "游戏开始时，你获得1张随机手牌"),
        new ReactionTacticSource(2065, "筹备Ⅱ", StandardShanhePreparationTacticsPackage.Choubei2SkillId, 300, 2, null,
            "游戏开始时，你获得2张随机手牌"),
        new ReactionTacticSource(2066, "筹备Ⅲ", StandardShanhePreparationTacticsPackage.Choubei3SkillId, 500, 3, null,
            "游戏开始时，你获得3张随机手牌"),
        new ReactionTacticSource(2103, "后发先至", StandardShanheDrawTacticsPackage.HoufaXianzhiSkillId, 300, 2, null,
            "摸牌阶段，你的摸牌数-1；回合结束时，你摸3张牌"),
        new ReactionTacticSource(2159, "招募后勤Ⅱ", StandardShanheLogisticsTacticsPackage.ZhaomuHouqin2SkillId, 300, 2, null,
            "初始手牌-1，摸牌数+1"),
        new ReactionTacticSource(2160, "招募后勤", StandardShanheLogisticsTacticsPackage.ZhaomuHouqinSkillId, 200, 1, null,
            "初始手牌-2，摸牌数+1"),
        new ReactionTacticSource(2161, "招募后勤Ⅲ", StandardShanheLogisticsTacticsPackage.ZhaomuHouqin3SkillId, 500, 3, 86,
            "初始手牌-2，摸牌数+3"),
        new ReactionTacticSource(2201, "队·牌", StandardShanheDrawTacticsPackage.DuipaiSkillId, 200, 1, null,
            "你的所有队友摸牌数+2"),
        new ReactionTacticSource(2025, "未雨绸缪", StandardShanheBossOpeningTacticsPackage.WeiyuChoumouSkillId, 200, 1, null,
            "仅首领战中，你随机获得2张初始手牌", BossBattleOnly: true),
        new ReactionTacticSource(2026, "未雨绸缪Ⅱ", StandardShanheBossOpeningTacticsPackage.WeiyuChoumou2SkillId, 300, 2, null,
            "仅首领战中，你随机获得3张初始手牌", BossBattleOnly: true),
        new ReactionTacticSource(2027, "未雨绸缪Ⅲ", StandardShanheBossOpeningTacticsPackage.WeiyuChoumou3SkillId, 500, 3, null,
            "仅首领战中，你随机获得4张初始手牌", BossBattleOnly: true),
        new ReactionTacticSource(2079, "手到擒来", StandardShanheCycleTacticsPackage.ShoudaoQinlaiSkillId, 200, 1, null,
            "每回合你使用第7张牌后,你摸1张牌"),
        new ReactionTacticSource(2080, "手到擒来Ⅱ", StandardShanheCycleTacticsPackage.ShoudaoQinlai2SkillId, 300, 2, null,
            "每回合你使用第5张牌后,你摸1张牌"),
        new ReactionTacticSource(2081, "手到擒来Ⅲ", StandardShanheCycleTacticsPackage.ShoudaoQinlai3SkillId, 500, 3, 65,
            "每回合你使用第6张牌后,你摸2张牌"),
        new ReactionTacticSource(2227, "弱反馈", StandardShanheSustainOpeningTacticsPackage.RuoFankuiSkillId, 500, 3, null,
            "你受到1点伤害后，摸一张牌"),
        new ReactionTacticSource(2182, "粮草储备", StandardShanheSustainOpeningTacticsPackage.LiangcaoChubeiSkillId, 200, 1, 17,
            "游戏开始时，你回复1点体力"),
        new ReactionTacticSource(2132, "皮囊", StandardShanheHandStockTacticsPackage.PinangSkillId, 200, 1, null,
            "手牌上限+1"),
        new ReactionTacticSource(2133, "皮囊Ⅱ", StandardShanheHandStockTacticsPackage.Pinang2SkillId, 300, 2, null,
            "手牌上限+2"),
        new ReactionTacticSource(2135, "皮囊Ⅲ", StandardShanheHandStockTacticsPackage.Pinang3SkillId, 700, 4, null,
            "手牌上限+5"),
        new ReactionTacticSource(2194, "精于谋略", StandardShanheHandStockTacticsPackage.JingyuMoulueSkillId, 300, 2, null,
            "手牌数小于4时，你使用【杀】造成的伤害+1"),
        new ReactionTacticSource(2167, "绝对无懈", StandardShanheResponseTacticsPackage.JueduiWuxieSkillId, 500, 3, null,
            "其他角色无法响应你的【无懈可击】"),
        new ReactionTacticSource(2308, "赌术II", StandardShanheResponseTacticsPackage.Dushu2SkillId, 300, 2, 46,
            "你的拼点牌点数+6（最大为K）"),
        new ReactionTacticSource(2093, "拂衣去·杀", StandardShanheFuyiTacticsPackage.FuyiSlashSkillId, 300, 2, null,
            "回合开始时，你获得1张【杀】"),
        new ReactionTacticSource(2144, "拂衣去·闪", StandardShanheFuyiTacticsPackage.FuyiDodgeSkillId, 300, 2, null,
            "回合开始时，你获得1张【闪】"),
        new ReactionTacticSource(2094, "拂衣去·桃", StandardShanheFuyiTacticsPackage.FuyiPeachSkillId, 700, 4, 138,
            "回合开始时，你获得2张【桃】"),
        new ReactionTacticSource(2095, "拂衣去·拆", StandardShanheFuyiTacticsPackage.FuyiDismantleSkillId, 500, 3, null,
            "回合开始时，你获得1张【过河拆桥】"),
        new ReactionTacticSource(2096, "拂衣去·顺", StandardShanheFuyiTacticsPackage.FuyiStealSkillId, 500, 3, null,
            "回合开始时，你获得1张【顺手牵羊】"),
        new ReactionTacticSource(2097, "拂衣去·决", StandardShanheFuyiTacticsPackage.FuyiDuelSkillId, 500, 3, null,
            "回合开始时，你获得1张【决斗】"),
        new ReactionTacticSource(2098, "拂衣去·锁", StandardShanheFuyiTacticsPackage.FuyiIronChainSkillId, 500, 3, null,
            "回合开始时，你获得1张【铁索连环】"),
        new ReactionTacticSource(2099, "拂衣去·火", StandardShanheFuyiTacticsPackage.FuyiFireAttackSkillId, 300, 2, null,
            "回合开始时，你获得1张【火攻】"),
        new ReactionTacticSource(2256, "拂衣去·万箭", StandardShanheFuyiTacticsPackage.FuyiArrowBarrageSkillId, 500, 3, null,
            "回合开始时，你获得1张【万箭齐发】"),
        new ReactionTacticSource(2257, "拂衣去·南蛮", StandardShanheFuyiTacticsPackage.FuyiBarbarianAssaultSkillId, 500, 3, null,
            "回合开始时，你获得1张【南蛮入侵】"),
        new ReactionTacticSource(2249, "博闻Ⅰ", StandardShanheBowenTacticsPackage.BowenSkillId, 300, 2, 53,
            "你的回合开始时，从牌堆中获得1张随机锦囊牌"),
        new ReactionTacticSource(2250, "博闻Ⅱ", StandardShanheBowenTacticsPackage.Bowen2SkillId, 500, 3, null,
            "你的回合开始时，从牌堆中获得2张随机锦囊牌"),
        new ReactionTacticSource(2251, "博闻Ⅲ", StandardShanheBowenTacticsPackage.Bowen3SkillId, 700, 4, null,
            "你的回合开始时，从牌堆中获得3张随机锦囊牌"),
        new ReactionTacticSource(2039, "铸刀", StandardShanheRecastTacticsPackage.ZhudaoSkillId, 300, 2, null,
            "你使用【杀】后可以重铸1张牌", RewardIdOverride: "tactic-zhudao-current-2039"),
        new ReactionTacticSource(2040, "铸刀Ⅱ", StandardShanheRecastTacticsPackage.Zhudao2SkillId, 700, 4, null,
            "你使用【杀】后可以重铸至多2张牌", RewardIdOverride: "tactic-zhudao-current-2040"),
        new ReactionTacticSource(2128, "及时雨", StandardShanheRecastTacticsPackage.JishiyuSkillId, 200, 1, 29,
            "回合外失去最后一张手牌后，摸两张牌", RewardIdOverride: "tactic-jishiyu-current-2128"),
        new ReactionTacticSource(2131, "破釜沉舟", StandardShanheAppliedDamageReflectionPackage.PofuChenzhouSkillId, 300, 2, null,
            "回合外受到伤害一次大于等于3点时，对伤害来源造成等量同属性伤害。", RewardIdOverride: "tactic-pofu-chenzhou-current-2131"),
    });

    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentReactionTacticShopMetadata =>
        CurrentReactionTactics.Select(source => new KeyValuePair<string, ShanheShopMetadata>(source.RewardId,
            new(source.SourceId, source.Price, source.Rarity, "战法")));
    private static IEnumerable<KeyValuePair<int, string>> CurrentReactionTacticCodexBindings =>
        CurrentReactionTactics.Where(source => source.CodexNodeId.HasValue)
            .Select(source => new KeyValuePair<int, string>(source.CodexNodeId.GetValueOrDefault(), source.RewardId));
    private static IEnumerable<string> CurrentReactionTacticDefaultUnlockedRewardIds =>
        CurrentReactionTactics.Where(source => source.CodexNodeId is null).Select(source => source.RewardId);

    private static ShanheCampaignDefinition WithCurrentReactionTactics(ShanheCampaignDefinition definition)
    {
        var sources = CurrentReactionTactics;
        var rewardIds = sources.Select(source => source.RewardId).ToArray();
        var choices = sources.Select(source => new ShanheNodeChoiceDefinition("buy-" + source.RewardId,
            $"战法·{source.Name} · {source.Price}钱", source.Price, RewardId: source.RewardId, LeavesNode: false)
            { Description = source.Reward.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(sources.Select(source => source.Reward)).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(rewardIds).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }

    private static bool RequiresReactionTacticsRegistry(ShanheBattleSetup setup)
    {
        var skills = setup.ExtraSkillIds.Concat(setup.TacticSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .ToHashSet(StringComparer.Ordinal);
        return CurrentReactionTactics.Any(source => skills.Contains(source.SkillId));
    }
}
