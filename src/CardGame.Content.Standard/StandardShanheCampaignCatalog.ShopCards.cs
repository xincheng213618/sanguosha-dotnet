using CardGame.Core;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    private sealed record ShopCardSource(int SourceId, string Name, CardKind Kind, Suit Suit, int Rank, int Price, int Rarity)
    {
        public string RewardId => "codex-card-" + SourceId.ToString(System.Globalization.CultureInfo.InvariantCulture);
        public ShanheShopMetadata Metadata => new(SourceId, Price, Rarity,
            EquipmentCatalog.IsEquipment(Kind) ? "装备" : "牌", Kind, Suit, Rank);
    }

    // Exact faces/prices from cards.json, not its differently priced 4183+ rows.
    // This catalogue's suit codes are 1=Heart, 2=Diamond, 3=Spade, 4=Club,
    // as corroborated by the existing verified 4004/4008/4010/4015 faces.
    // They are independent of the enemy equipment table's local suit convention.
    // These rows have no fixed S5 research node; their initial availability is local stock.
    private static IReadOnlyList<ShopCardSource> CurrentShopCardSources => Array.AsReadOnly(new[]
    {
        new ShopCardSource(4001, "诸葛连弩", CardKind.Crossbow, Suit.Diamond, 1, 600, 4),
        new ShopCardSource(4003, "雌雄双股剑", CardKind.CixiongDoubleSwords, Suit.Spade, 2, 600, 4),
        new ShopCardSource(4005, "青龙偃月刀", CardKind.QinglongCrescentBlade, Suit.Spade, 5, 100, 1),
        new ShopCardSource(4007, "贯石斧", CardKind.StoneAxe, Suit.Diamond, 5, 400, 3),
        new ShopCardSource(4011, "方天画戟", CardKind.FangtianHalberd, Suit.Diamond, 12, 100, 1),
        new ShopCardSource(4018, "白银狮子", CardKind.SilverLion, Suit.Club, 1, 200, 2),
        new ShopCardSource(4019, "赤兔", CardKind.OffensiveHorse, Suit.Heart, 5, 600, 4),
        new ShopCardSource(4020, "紫騂", CardKind.Zixing, Suit.Diamond, 13, 600, 4),
        new ShopCardSource(4021, "大宛", CardKind.Dawan, Suit.Spade, 13, 600, 4),
        new ShopCardSource(4022, "绝影", CardKind.DefensiveHorse, Suit.Spade, 5, 600, 4),
        new ShopCardSource(4023, "的卢", CardKind.Dilu, Suit.Club, 5, 600, 4),
        new ShopCardSource(4024, "爪黄飞电", CardKind.Zhaohuangfeidian, Suit.Heart, 13, 600, 4),
        new ShopCardSource(4025, "骅骝", CardKind.Hualiu, Suit.Diamond, 13, 600, 4),
        new ShopCardSource(4027, "束发紫金冠", CardKind.PurpleGoldCrown, Suit.Diamond, 1, 400, 3),
        new ShopCardSource(4148, "雷震", CardKind.Leizhen, Suit.Club, 1, 400, 3),
        new ShopCardSource(4149, "暗器飞刃", CardKind.AnqiFeiren, Suit.Heart, 5, 200, 2),
        new ShopCardSource(4151, "斩魂刀", CardKind.ZhanhunDao, Suit.Spade, 2, 100, 1),
        new ShopCardSource(4152, "妖刀赤瞳", CardKind.YaodaoChitong, Suit.Spade, 6, 400, 3),
        new ShopCardSource(4062, "杀", CardKind.Slash, Suit.Spade, 1, 150, 1),
        new ShopCardSource(4063, "杀", CardKind.Slash, Suit.Heart, 1, 150, 1),
        new ShopCardSource(4066, "雷杀", CardKind.ThunderSlash, Suit.Spade, 13, 200, 2),
        new ShopCardSource(4068, "火杀", CardKind.FireSlash, Suit.Heart, 13, 200, 2),
        new ShopCardSource(4073, "闪", CardKind.Dodge, Suit.Diamond, 2, 150, 1),
        new ShopCardSource(4078, "酒", CardKind.Alcohol, Suit.Spade, 9, 250, 3),
        new ShopCardSource(4083, "顺手牵羊", CardKind.Snatch, Suit.Heart, 6, 250, 3),
        new ShopCardSource(4086, "过河拆桥", CardKind.Dismantlement, Suit.Spade, 6, 200, 2),
        new ShopCardSource(4094, "决斗", CardKind.Duel, Suit.Spade, 12, 200, 2),
        new ShopCardSource(4098, "南蛮入侵", CardKind.BarbarianAssault, Suit.Spade, 11, 250, 3),
        new ShopCardSource(4102, "万箭齐发", CardKind.ArrowBarrage, Suit.Spade, 10, 300, 4),
        new ShopCardSource(4111, "桃园结义", CardKind.PeachGarden, Suit.Heart, 8, 200, 2),
        new ShopCardSource(4114, "无懈可击", CardKind.Nullification, Suit.Spade, 7, 200, 2),
        new ShopCardSource(4119, "乐不思蜀", CardKind.Indulgence, Suit.Heart, 4, 300, 4),
        new ShopCardSource(4122, "铁索连环", CardKind.IronChain, Suit.Spade, 3, 200, 2),
        new ShopCardSource(4126, "兵粮寸断", CardKind.SupplyShortage, Suit.Spade, 4, 250, 3),
        new ShopCardSource(4130, "借刀杀人", CardKind.BorrowedSword, Suit.Spade, 3, 150, 1),
        new ShopCardSource(4135, "火攻", CardKind.FireAttack, Suit.Heart, 11, 200, 2),
        new ShopCardSource(4141, "无中生有", CardKind.DrawTwo, Suit.Diamond, 10, 250, 3)
    });

    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentShopCardMetadata =>
        CurrentShopCardSources.Select(source => new KeyValuePair<string, ShanheShopMetadata>(source.RewardId, source.Metadata));
}
