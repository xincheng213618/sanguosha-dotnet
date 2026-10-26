using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string OpeningCompanionRewardId = "codex-1";
    public const string EliteOpeningCompanionRewardId = "codex-75";

    private static ShanheCombatantDefinition ManzhuCompanion() => new(StandardShanheCompanionPackage.ManzhuId)
    {
        StartingHand = [new(CardKind.Dodge, Suit.Club, 13), new(CardKind.Dodge, Suit.Club, 12),
            new(CardKind.Dodge, Suit.Club, 11), new(CardKind.Slash, Suit.Heart, 1)]
    };

    private static ShanheCombatantDefinition ChilianCompanion() => new(StandardShanheChilianCompanionPackage.ChilianId)
    {
        StartingHand = [new(CardKind.Slash, Suit.Heart, 5), new(CardKind.Alcohol, Suit.Spade, 3)]
    };

    private static ShanheCombatantDefinition FenghuangCompanion() => new(StandardShanheBossPackage.FenghuangId)
    {
        StartingHand = [new(CardKind.Slash, Suit.Heart, 5), new(CardKind.Dodge, Suit.Heart, 13),
            new(CardKind.ArrowBarrage, Suit.Heart, 1), new(CardKind.Alcohol, Suit.Spade, 3)]
    };

    private static ShanheCampaignDefinition WithOpeningCompanions(ShanheCampaignDefinition definition, string editionId)
    {
        if (editionId != Returning2026EditionId) return definition;
        return definition with
        {
            Rewards = definition.Rewards.Append(new ShanheRewardDefinition(OpeningCompanionRewardId, "战法·和光盟")
            {
                Description = "开始征程时获得一名特殊队友：曼珠、赤炼、风篁或阿绫。本次征程固定；" +
                    "可与和光盟Ⅱ同时装备。席位不足时保留原阵容，部分同伴暂不入战。",
                OpeningCompanionPool = [ManzhuCompanion(), ChilianCompanion(), FenghuangCompanion(),
                    new(StandardShanheExpandedCompanionPackage.AlingId)
                    { StartingHand = [new(CardKind.Nullification, Suit.Club, 13), new(CardKind.Nullification, Suit.Club, 13)] }]
            }).Append(new ShanheRewardDefinition(EliteOpeningCompanionRewardId, "战法·和光盟Ⅱ")
            {
                Rarity = 3,
                Description = "开始征程时获得一名精英特殊队友：夭华、段青、青璃或紫鸢。本次征程固定；可与和光盟同时装备。",
                OpeningCompanionPool =
                [
                    new(StandardShanheExpandedCompanionPackage.YaohuaId)
                    { StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Peach, Suit.Heart, 6), 4).ToArray() },
                    new(StandardShanheExpandedCompanionPackage.DuanqingId)
                    { StartingHand = [new(CardKind.BaiBiDao, Suit.Spade, 2), new(CardKind.Dodge, Suit.Heart, 13),
                        new(CardKind.Dodge, Suit.Diamond, 2), new(CardKind.Slash, Suit.Heart, 1),
                        new(CardKind.Dismantlement, Suit.Spade, 12), new(CardKind.DrawTwo, Suit.Heart, 11)] },
                    new(StandardShanheExpandedCompanionPackage.QingliId)
                    { StartingHand = [new(CardKind.Dodge, Suit.Diamond, 3), new(CardKind.Dodge, Suit.Diamond, 4),
                        new(CardKind.Nullification, Suit.Spade, 13), new(CardKind.Peach, Suit.Heart, 6),
                        new(CardKind.Dodge, Suit.Heart, 9), new(CardKind.Nullification, Suit.Club, 13)] },
                    new(StandardShanheExpandedCompanionPackage.ZiyuanId)
                    { StartingHand = [new(CardKind.Slash, Suit.Heart, 10), new(CardKind.Slash, Suit.Heart, 10),
                        new(CardKind.Slash, Suit.Heart, 9), new(CardKind.Slash, Suit.Heart, 9),
                        new(CardKind.Slash, Suit.Heart, 5), new(CardKind.Dodge, Suit.Heart, 13)] }
                ]
            }).ToArray()
        };
    }
}
