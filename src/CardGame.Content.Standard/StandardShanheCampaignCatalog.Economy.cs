using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public sealed record ShanheEconomyTacticSource(int SourceId, string RewardId, string Name, int Rarity,
    int Price, string Description, ShanheCampaignEconomy Effect);

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentInterest2RewardId = "tactic-economy-2146";

    // Public S5 tactics.json / tactic-tree.json, generated 2026-10-21.
    // 133 is a random rare unlock, not a fixed grant of source 2090.
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
            "在战斗外，当你的装备被顶替时，获得200铜币。", new(ReplacedEquipmentGold: 200))
    });

    private static ShanheCampaignDefinition WithCurrentEconomyTactics(ShanheCampaignDefinition definition)
    {
        var sources = CurrentEconomyTactics;
        var rewards = sources.Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
        { Description = source.Description, CampaignEconomy = source.Effect }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(rewards).ToArray(),
            IntroText = definition.IntroText + "\n另有利滚利、议价、会员Ⅱ、商品扩容与回收等征程经济战法；只有实际持有时才生效。",
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(rewards.Select(reward => reward.Id)).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(sources.Select(source =>
                    new ShanheNodeChoiceDefinition("buy-economy-" + source.SourceId, source.Name, source.Price,
                        RewardId: source.RewardId, LeavesNode: false) { Description = source.Description })).ToArray() : node.Choices
            }).ToArray()
        };
    }
}
