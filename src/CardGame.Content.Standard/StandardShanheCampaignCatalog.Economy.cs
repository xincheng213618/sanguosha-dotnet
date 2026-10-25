using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public sealed record ShanheEconomyTacticSource(int SourceId, string RewardId, string Name, int Rarity,
    int Price, string Description, ShanheCampaignEconomy Effect);

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentInterest2RewardId = "tactic-economy-2146";

    // Public S5 tactics.json / tactic-tree.json, generated 2026-10-21.
    // 133 and 128 are random unlocks, not fixed grants of 2090 or 2181.
    // Only the higher interest variant has a fixed S5 tree node: 105.
    public static IReadOnlyList<ShanheEconomyTacticSource> CurrentEconomyTactics => Array.AsReadOnly(new[]
    {
        new ShanheEconomyTacticSource(2089, "tactic-economy-2089", "打秋风", 2, 300,
            "通过战斗获取的铜币增加20%。", new(BattleGoldPercent: 20)),
        new(2090, "tactic-economy-2090", "利滚利", 2, 300,
            "每场战斗后，你的总铜币数量增加5%。", new(PostBattleInterestPercent: 5)),
        new(2146, CurrentInterest2RewardId, "利滚利Ⅱ", 4, 700,
            "每场战斗后，你的总铜币数量增加10%。", new(PostBattleInterestPercent: 10)),
        new(2091, "tactic-economy-2091", "议价Ⅰ", 2, 300,
            "集市购买时降低10%铜币消耗。", new(ShopDiscountPercent: 10)),
        new(2145, "tactic-economy-2145", "议价Ⅱ", 3, 500,
            "集市购买时降低20%铜币消耗。", new(ShopDiscountPercent: 20)),
        new(2107, "tactic-economy-2107", "会员Ⅱ", 3, 500,
            "你可以消耗100铜币刷新商店，可刷新5次。", new(ShopRefreshLimit: 5)),
        new(2136, "tactic-economy-2136", "商品+1", 2, 300,
            "商店每类商品数量+1。", new(ExtraStockPerCategory: 1)),
        new(2137, "tactic-economy-2137", "商品+2", 3, 500,
            "商店每类商品数量+2。", new(ExtraStockPerCategory: 2)),
        new(2092, "tactic-economy-2092", "回收", 1, 200,
            "在战斗外，当你的装备被顶替时，获得200铜币。", new(ReplacedEquipmentGold: 200)),
        new(2012, "tactic-economy-2012", "集市霸王", 3, 500,
            "集市的购买价格不再递增", new(SuppressShopPriceEscalation: true)),
        new(2112, "tactic-economy-2112", "家财万贯", 1, 200,
            "开始征程时，你获得200铜币", new(StartingGold: 200)),
        new(2139, "tactic-economy-2139", "家财万贯Ⅱ", 2, 300,
            "开始征程时，你获得300铜币", new(StartingGold: 300)),
        new(2140, "tactic-economy-2140", "家财万贯Ⅲ", 3, 500,
            "开始征程时，你获得500铜币", new(StartingGold: 500)),
        new(2141, "tactic-economy-2141", "家财万贯Ⅳ", 4, 700,
            "开始征程时，你获得700铜币", new(StartingGold: 700)),
        new(2178, "tactic-economy-2178", "守财奴", 1, 200,
            "铜币大于300时，你的手牌上限+2", new(GoldAbove: 300, BattleSkillId: StandardShanheWealthPackage.WealthHandLimitSkillId)),
        new(2179, "tactic-economy-2179", "守财奴Ⅱ", 1, 200,
            "铜币大于500时，你的初始手牌+2", new(GoldAbove: 500, BattleSkillId: StandardShanheWealthPackage.WealthInitialHandSkillId)),
        new(2180, "tactic-economy-2180", "守财奴Ⅲ", 1, 200,
            "铜币大于700时，你的摸牌数+2", new(GoldAbove: 700, BattleSkillId: StandardShanheWealthPackage.WealthDrawSkillId)),
        new(2181, "tactic-economy-2181", "守财奴Ⅳ", 3, 500,
            "铜币大于1000时，你的伤害+1", new(GoldAbove: 1000, BattleSkillId: StandardShanheWealthPackage.WealthDamageSkillId)),
        new(2013, "tactic-economy-2013", "王牌战法", 2, 300,
            "击败精英关卡后，额外获得一个稀有战法", new(ExtraEliteRareTactics: 1)),
        new(2014, "tactic-economy-2014", "王牌战技", 2, 300,
            "击败首领关卡后，额外获得一个史诗技能", new(ExtraBossEpicSkills: 1)),
        new(2017, "tactic-economy-2017", "谋略过人", 2, 300,
            "选择【战法奖励】时，候选项从三个变为四个", new(TacticRewardChoiceCount: 4)),
        new(2018, "tactic-economy-2018", "慧黠", 1, 200,
            "选择【卡牌奖励】时，候选项从三个变为四个", new(CardRewardChoiceCount: 4)),
        new(2016, "tactic-economy-2016", "思维敏捷", 2, 300,
            "选择【技能奖励】时，候选项从三个变为四个", new(SkillRewardChoiceCount: 4))
    });

    private static ShanheCampaignDefinition WithCurrentEconomyTactics(ShanheCampaignDefinition definition)
    {
        var sources = CurrentEconomyTactics;
        var rewards = sources.Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
        { Description = source.Description, CampaignEconomy = source.Effect }).ToArray();
        // Opening-only wealth tactics remain available to the expedition loadout.
        // Acquiring them halfway through a run cannot trigger a new expedition.
        var obtainable = sources.Where(source => source.Effect.StartingGold == 0).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(rewards).ToArray(),
            IntroText = definition.IntroText + "\n另有利滚利、集市霸王、守财奴、议价、会员Ⅱ与回收等经济战法；只有实际持有时才生效。家财万贯系列仅在征程前装备后于开局获得铜币。",
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(obtainable.Select(source => source.RewardId)).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(obtainable.Select(source =>
                    new ShanheNodeChoiceDefinition("buy-economy-" + source.SourceId, source.Name, source.Price,
                        RewardId: source.RewardId, LeavesNode: false) { Description = source.Description })).ToArray() : node.Choices
            }).ToArray()
        };
    }
}
