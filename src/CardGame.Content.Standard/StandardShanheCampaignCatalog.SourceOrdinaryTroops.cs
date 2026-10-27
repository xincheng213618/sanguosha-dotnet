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
        foreach (var group in new[] { 2474, 2467, 2487, 2473, 2480, 2497, 2455, 2459, 2460, 2476, 2489, 2493, 2495,
                     2451, 2457, 2498, 2500, 2454, 2456, 2458, 2461 })
        {
            if (!potential.Contains(group)) continue;
            var general = OrdinaryTroopGeneral(group, difficulty);
            var member = new ShanheCombatantDefinition(general);
            if (group == 2497) member = member with { StartingCards = JiangdongVeteranGear(difficulty) };
            if (group == 2495) member = member with { StartingCards = ExecutionerGear(difficulty) };
            yield return WithSource2026EnemyData(new("source-city-" + city.SourceCityId + ":troop-" + group,
                OrdinaryTroopName(group), [member], [],
                VictoryGold: 200, HealAfterVictory: 1)
            {
                SourceBattleId = group, SourceCityId = city.SourceCityId,
                TerrainSkillId = Source2026Terrain(city.SourceTerrainSkillId, difficulty),
                StoryText = OrdinaryTroopStory(group)
            }, group);
        }
        foreach (var group in new[] { 2465, 2499, 2464 })
        {
            if (!potential.Contains(group)) continue;
            yield return WithSource2026EnemyData(new("source-city-" + city.SourceCityId + ":troop-" + group,
                OrdinaryTroopName(group), MixedOrdinaryMembers(group, difficulty), [], VictoryGold: 200, HealAfterVictory: 1)
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
        var scout = StandardShanheSourceCoalitionScoutPackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var spy = StandardShanheSourceCoalitionSpyPackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var magnate = StandardShanheSourceLocalMagnatePackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var linzhonggu = StandardShanheSourceLinZhongGuPackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var xinping = StandardShanheSourceXinPingPackage.GetGeneralId(difficulty);
        var buheng = StandardShanheSourceBuHengPackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var demon = StandardShanheSourceDemonAssassinPackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var yulin = StandardShanheSourceYulinGuardPackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var farmer = StandardShanheSourceAgricultureOfficerPackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var whiteHorse = StandardShanheSourceWhiteHorsePackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var supply = StandardShanheSourceCoalitionTrioPackage.GetQuartermasterGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var demonRaider = StandardShanheSourceDemonRaiderPackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        var demonArcher = StandardShanheSourceDemonArcherPackage.GetGeneralId(difficulty switch { 1 => 1, 2 => 11, _ => 21 });
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            Nodes = definition.Nodes.Select(node => node.Id is "hulao" or "guangcheng" ? node with
            {
                Encounters = node.Encounters!.Concat(new[]
                {
                    FirstChapter(node.Id + ":qiang-raider-2473-local", 2473, raider),
                    FirstChapter(node.Id + ":coalition-scout-2455-local", 2455, scout),
                    FirstChapter(node.Id + ":coalition-spy-2459-local", 2459, spy, baseHp: 3),
                    FirstChapter(node.Id + ":local-magnate-2460-local", 2460, magnate),
                    FirstChapter(node.Id + ":demon-assassin-2457-local", 2457, demon, baseHp: 3),
                    FirstChapter(node.Id + ":yulin-guard-2498-local", 2498, yulin),
                    FirstChapter(node.Id + ":white-horse-2454-local", 2454, whiteHorse),
                    FirstChapter(node.Id + ":supply-officer-2456-local", 2456, supply, baseHp: 3),
                    FirstChapter(node.Id + ":demon-raider-2458-local", 2458, demonRaider),
                    FirstChapter(node.Id + ":demon-archer-2461-local", 2461, demonArcher)
                }).ToArray(),
                Description = node.Description + "另有白马义从、联军斥候等边军在此游弋。"
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
                        startingCards: JiangdongVeteranGear(jiangdongTier)),
                    Local(node.Id + ":linzhonggu-2476-local", OrdinaryTroopName(2476), linzhonggu, OrdinaryTroopStory(2476)),
                    Local(node.Id + ":xinping-2489-local", OrdinaryTroopName(2489), xinping, OrdinaryTroopStory(2489)),
                    Local(node.Id + ":buheng-2451-local", OrdinaryTroopName(2451), buheng, OrdinaryTroopStory(2451)),
                    Local(node.Id + ":agriculture-officer-2500-local", OrdinaryTroopName(2500), farmer, OrdinaryTroopStory(2500), baseHp: 3),
                    ReturningMixedOrdinary(2499, difficulty, node.Id + ":yulin-officer-2499-local", chapter: 2)
                }).ToArray(),
                Description = node.Description + "边军与地方兵马在此驻守。"
            }).ToArray()
        });

        ShanheEncounterDefinition Local(string id, string name, string general, string story, int baseHp = 4,
            IReadOnlyList<ShanheStartingCard>? startingCards = null) =>
            new(id, name, [new(general, MaxHpBonus: 7 + 2 * growth - baseHp, Hp: 7 + 2 * growth,
                InitialHandBonus: 1 + growth, DrawBonus: 1) { StartingCards = startingCards }], [], VictoryGold: 200, HealAfterVictory: 2)
            { StoryText = story };

        ShanheEncounterDefinition FirstChapter(string id, int group, string general, int baseHp = 4)
        {
            var tier = difficulty switch { 1 => 1, 2 => 11, _ => group == 2473 ? 16 : 21 };
            var attributes = Source2026EnemyDataByGroup[group].Single().TierAttributes
                .Where(t => t.DifficultyGroupId <= (tier + 4) / 5).Select(t => t.Attributes).ToArray();
            var extraHp = attributes.Sum(a => a.Hp);
            return new(id, OrdinaryTroopName(group), [new(general,
                MaxHpBonus: 4 + difficulty - baseHp + extraHp, Hp: 4 + difficulty + extraHp,
                DrawBonus: difficulty, InitialHandBonus: attributes.Sum(a => a.StartingCards))], [],
                VictoryGold: 200, HealAfterVictory: 1) { StoryText = OrdinaryTroopStory(group) };
        }
    }

    private static string OrdinaryTroopGeneral(int group, int difficulty) => group switch
    {
        2454 => StandardShanheSourceWhiteHorsePackage.GetGeneralId(difficulty),
        2456 => StandardShanheSourceCoalitionTrioPackage.GetQuartermasterGeneralId(difficulty),
        2458 => StandardShanheSourceDemonRaiderPackage.GetGeneralId(difficulty),
        2461 => StandardShanheSourceDemonArcherPackage.GetGeneralId(difficulty),
        2451 => StandardShanheSourceBuHengPackage.GetGeneralId(difficulty),
        2457 => StandardShanheSourceDemonAssassinPackage.GetGeneralId(difficulty),
        2498 => StandardShanheSourceYulinGuardPackage.GetGeneralId(difficulty),
        2500 => StandardShanheSourceAgricultureOfficerPackage.GetGeneralId(difficulty),
        2474 => StandardShanheSourceVeteranPackage.GetGeneralId(difficulty),
        2467 => StandardShanheSourceWuhuanRiderPackage.GetGeneralId(difficulty),
        2487 => StandardShanheSourceWarriorPackage.GetGeneralId(difficulty),
        2473 => StandardShanheSourceQiangRaiderPackage.GetGeneralId(difficulty),
        2480 => StandardShanheSourceXunYuePackage.GetGeneralId(difficulty),
        2497 => StandardShanheSourceJiangdongVeteranPackage.GetGeneralId(difficulty),
        2455 => StandardShanheSourceCoalitionScoutPackage.GetGeneralId(difficulty),
        2459 => StandardShanheSourceCoalitionSpyPackage.GetGeneralId(difficulty),
        2460 => StandardShanheSourceLocalMagnatePackage.GetGeneralId(difficulty),
        2476 => StandardShanheSourceLinZhongGuPackage.GetGeneralId(difficulty),
        2489 => StandardShanheSourceXinPingPackage.GetGeneralId(difficulty),
        2493 => StandardShanheSourceWuqueAssassinPackage.GetGeneralId(difficulty),
        2495 => StandardShanheSourceExecutionerPackage.GetGeneralId(difficulty),
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };
    private static string OrdinaryTroopName(int group) => group switch
    {
        2454 => "白马义从", 2465 => "联军三将", 2499 => "山崖巧遇 · 羽林卫尉",
        2456 => "联军督运使", 2458 => "魔军暴徒", 2461 => "魔军弓骑", 2464 => "魔军三将",
        2451 => "步珩", 2457 => "擒拿刺客 · 魔军刺客", 2498 => "羽林军", 2500 => "农都尉",
        2474 => "经验传承 · 百战老兵", 2467 => "乌桓贼匪 · 乌桓骑手", 2487 => "探听虚实 · 武卫兵",
        2473 => "趁人之危 · 羌人劫匪", 2480 => "空谈无益 · 荀悦",
        2497 => "狭路相逢 · 江东老兵",
        2455 => "贪得无厌 · 联军斥候", 2459 => "暗中破坏 · 联军细作", 2460 => "巧破城楼 · 本地豪强",
        2476 => "林中鹘", 2489 => "辛评",
        2493 => "乌鹊暗杀者", 2495 => "校事典刑官",
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };
    private static string OrdinaryTroopStory(int group) => group switch
    {
        2454 => "白马义从以袭营封锁对手，高档还能清剿换牌、义从策马。",
        2456 => "联军督运使以督粮调度辎重，高档可储粮、宿守分牌。",
        2458 => "魔军暴徒以强袭冲阵，高档凭劫掠夺取辎重，再以狼蹈施压。",
        2461 => "魔军弓骑弃牌拓展射程，高档以诱言补牌、扫谷连攻。",
        2464 => "刺客潜袭封牌，弓骑扫谷驰援，暴徒强袭劫掠。",
        2465 => "细作与斥候结阵推进，督运使在后督粮、宿守接济友军。",
        2499 => "羽林卫尉率羽林军列阵，高档兼有恪守减伤与卫境应变。",
        2451 => "步珩以镇骨调度双方手牌，高档还能生息补给、恪守减伤。",
        2457 => "魔军刺客凭判定潜袭封锁手牌，高档还会诱敌夺牌、遗毒追逼。",
        2498 => "羽林军据守补牌，高档还能恪守减伤、废除装备区后穷守。",
        2500 => "农都尉以屯田积攒田牌，高档还能天妒取判定、断粮施压。",
        2474 => "百战老兵凭战绝压阵，随难度累积连营、龙胆、武圣与再起。",
        2467 => "乌桓骑手以怀异夺牌，高档还可突袭手牌、以锦囊急袭。",
        2487 => "武卫兵受伤后可以积攒锋牌，高档还能诱敌与裸衣奋战。",
        2473 => "羌人劫匪以雉盗夺取辎重，高档还能咆哮连攻、伺机顺手夺牌。",
        2480 => "荀悦以强识辨别牌类，高档还能在回合外明哲自守、出牌时誉虚调度。",
        2497 => "江东老兵持青釭剑接战，高档配白银狮子与照骨镜，兼有奋励与平寇。",
        2455 => "联军斥候以当先抢攻，高档还能看破锦囊，并以点虎紧盯敌手。",
        2459 => "联军细作以奇策调度全手牌，高档兼有替身与断发，弃置黑色牌换取手牌。",
        2460 => "本地豪强以束辎交换辎重，高档还能凭伪诚补牌、顺世增加下次摸牌、出杀次数与手牌上限。",
        2476 => "林中鹘以长标突击，高档还可用腹鳞留住回合内取得的手牌。",
        2489 => "辛评每次造成或受到伤害后，依次失去当前技能并摸牌，在回合结束后恢复。",
        2493 => "乌鹊暗杀者以鸩毒扰乱出牌，高档还有遗毒与绝情，出杀次数逐档增加。",
        2495 => "校事典刑官以义释夺牌、公清守御，高档可用宦浮下注、峻刑压制，并携带护心镜。",
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
