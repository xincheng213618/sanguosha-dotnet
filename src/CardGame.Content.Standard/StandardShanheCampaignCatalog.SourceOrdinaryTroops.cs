using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // Pool membership is source-backed. Server premises and probabilities are
    // unavailable, so admission alongside local fallback recipes is local policy.
    private static IEnumerable<ShanheEncounterDefinition> Source2026KnownOrdinaryTroops(
        ShanheSourceRouteCityDefinition city, int difficulty)
    {
        var potential = StandardShanheSourceRouteCatalog.Season5.GetPotentialBattleIds(city.SourceCityId);
        foreach (var group in new[] { 2474, 2467, 2487, 2473, 2480, 2497 })
        {
            if (!potential.Contains(group)) continue;
            var general = OrdinaryTroopGeneral(group, difficulty);
            var member = new ShanheCombatantDefinition(general);
            if (group == 2497) member = member with { StartingCards = JiangdongVeteranGear(difficulty) };
            yield return WithSource2026EnemyData(new("source-city-" + city.SourceCityId + ":troop-" + group,
                OrdinaryTroopName(group), [member], [],
                VictoryGold: 200, HealAfterVictory: 1)
            {
                SourceBattleId = group, SourceCityId = city.SourceCityId,
                TerrainSkillId = Source2026Terrain(city.SourceTerrainSkillId, difficulty),
                StoryText = OrdinaryTroopStory(group)
            }, group);
        }
    }

    private static ShanheCampaignDefinition WithReturningOrdinaryTroops(ShanheCampaignDefinition definition, int difficulty)
    {
        if (!definition.Id.StartsWith("shanhe:zhuhou-2026-", StringComparison.Ordinal) || definition.SourceDifficultyId is not null)
            return definition;
        if (difficulty is < 1 or > 3) throw new ArgumentOutOfRangeException(nameof(difficulty));
        var growth = difficulty - 1;
        // These are explicit local three-tier choices; they do not attach or
        // relabel this campaign as one of the 25 source difficulties.
        var veteran = StandardShanheSourceVeteranPackage.GetGeneralId(difficulty switch { 1 => 6, 2 => 16, _ => 21 });
        var rider = StandardShanheSourceWuhuanRiderPackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var warrior = StandardShanheSourceWarriorPackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var raider = StandardShanheSourceQiangRaiderPackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 16 });
        var xunyue = StandardShanheSourceXunYuePackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var jiangdongTier = difficulty switch { 1 => 1, 2 => 11, _ => 21 };
        var jiangdong = StandardShanheSourceJiangdongVeteranPackage.GetGeneralId(jiangdongTier);
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            Nodes = definition.Nodes.Select(node => node.Id is "hulao" or "guangcheng" ? node with
            {
                Encounters = node.Encounters!.Append(new ShanheEncounterDefinition(node.Id + ":qiang-raider-2473-local",
                    OrdinaryTroopName(2473), [new(raider, MaxHpBonus: difficulty, Hp: 4 + difficulty,
                        DrawBonus: difficulty)], [], VictoryGold: 200, HealAfterVictory: 1)
                    { StoryText = OrdinaryTroopStory(2473) }).ToArray(),
                Description = node.Description + "羌人劫匪也在此劫掠行军队伍。"
            } : node.Id is not ("dagu" or "huanyuan") ? node : node with
            {
                Encounters = node.Encounters!.Concat(new[]
                {
                    Local(node.Id + ":veteran-2474-local", "经验传承 · 百战老兵", veteran,
                        "百战老兵以战绝与连营接战，高档还能运用龙胆、武圣和再起。"),
                    Local(node.Id + ":wuhuan-rider-2467-local", "乌桓贼匪 · 乌桓骑手", rider,
                        "乌桓骑手以怀异夺牌，高档还可突袭与急袭。"),
                    Local(node.Id + ":warrior-2487-local", "探听虚实 · 武卫兵", warrior, OrdinaryTroopStory(2487)),
                    Local(node.Id + ":xunyue-2480-local", OrdinaryTroopName(2480), xunyue, OrdinaryTroopStory(2480), baseHp: 3),
                    Local(node.Id + ":jiangdong-veteran-2497-local", OrdinaryTroopName(2497), jiangdong, OrdinaryTroopStory(2497),
                        startingCards: JiangdongVeteranGear(jiangdongTier))
                }).ToArray(),
                Description = node.Description + "另有百战老兵、乌桓骑手、武卫兵、荀悦与江东老兵驻守。"
            }).ToArray()
        });

        ShanheEncounterDefinition Local(string id, string name, string general, string story, int baseHp = 4,
            IReadOnlyList<ShanheStartingCard>? startingCards = null) =>
            new(id, name, [new(general, MaxHpBonus: 7 + 2 * growth - baseHp, Hp: 7 + 2 * growth,
                InitialHandBonus: 1 + growth, DrawBonus: 1) { StartingCards = startingCards }], [], VictoryGold: 200, HealAfterVictory: 2)
            { StoryText = story };
    }

    private static string OrdinaryTroopGeneral(int group, int difficulty) => group switch
    {
        2474 => StandardShanheSourceVeteranPackage.GetGeneralId(difficulty),
        2467 => StandardShanheSourceWuhuanRiderPackage.GetGeneralId(difficulty),
        2487 => StandardShanheSourceWarriorPackage.GetGeneralId(difficulty),
        2473 => StandardShanheSourceQiangRaiderPackage.GetGeneralId(difficulty),
        2480 => StandardShanheSourceXunYuePackage.GetGeneralId(difficulty),
        2497 => StandardShanheSourceJiangdongVeteranPackage.GetGeneralId(difficulty),
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };
    private static string OrdinaryTroopName(int group) => group switch
    {
        2474 => "经验传承 · 百战老兵", 2467 => "乌桓贼匪 · 乌桓骑手", 2487 => "探听虚实 · 武卫兵",
        2473 => "趁人之危 · 羌人劫匪", 2480 => "空谈无益 · 荀悦",
        2497 => "狭路相逢 · 江东老兵",
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };
    private static string OrdinaryTroopStory(int group) => group switch
    {
        2474 => "百战老兵凭战绝压阵，随难度累积连营、龙胆、武圣与再起。",
        2467 => "乌桓骑手以怀异夺牌，高档还可突袭手牌、以锦囊急袭。",
        2487 => "武卫兵受伤后可以积攒锋牌，高档还能诱敌与裸衣奋战。",
        2473 => "羌人劫匪以雉盗夺取辎重，高档还能咆哮连攻、伺机顺手夺牌。",
        2480 => "荀悦以强识辨别牌类，高档还能在回合外明哲自守、出牌时誉虚调度。",
        2497 => "江东老兵持青釭剑接战，高档配白银狮子与照骨镜，兼有奋励与平寇。",
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };

    private static IReadOnlyList<ShanheStartingCard> JiangdongVeteranGear(int sourceDifficulty)
    {
        var gear = new List<ShanheStartingCard> { new(CardKind.QinggangSword, Suit.Club, 6) };
        if (sourceDifficulty >= 11) gear.Add(new(CardKind.SilverLion, Suit.Diamond, 1));
        if (sourceDifficulty >= 21) gear.Add(new(CardKind.ZhaoguMirror, Suit.Heart, 1));
        return Array.AsReadOnly(gear.ToArray());
    }
}
