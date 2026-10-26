using System.Globalization;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string Source2026EditionId = "lords-dong-source-2026";
    public const string Source2026CampaignIdPrefix = "shanhe:zhuhou-source-2026-";

    public static string Source2026CampaignId(int sourceDifficultyId)
    {
        if (sourceDifficultyId is < 1 or > 25) throw new ArgumentOutOfRangeException(nameof(sourceDifficultyId));
        return Source2026CampaignIdPrefix + sourceDifficultyId.ToString(CultureInfo.InvariantCulture);
    }

    public static bool IsSource2026Campaign(string? campaignId, int? sourceDifficultyId) =>
        sourceDifficultyId is >= 1 and <= 25 &&
        string.Equals(campaignId, Source2026CampaignId(sourceDifficultyId.Value), StringComparison.Ordinal);

    public static ShanheCampaignDefinition CreateSourceCodexDefinition(int sourceDifficultyId) =>
        CreateSourceDefinition(sourceDifficultyId, includeCodex: true);

    /// <summary>
    /// Uses the original region graph with a documented compressed local itinerary. Source battle
    /// identities belong only to the supported source roster; ordinary local troops carry city identity only.
    /// </summary>
    public static ShanheCampaignDefinition CreateSourceDefinition(int sourceDifficultyId, bool includeCodex = true)
    {
        var id = Source2026CampaignId(sourceDifficultyId);
        var template = includeCodex ? CreateCodexDefinition(1, Returning2026EditionId) : CreateDefinition(1, Returning2026EditionId);
        var graph = StandardShanheSourceRouteCatalog.Season5;
        var regions = graph.Regions.Where(region => region.MinimumSourceDifficultyId <= sourceDifficultyId).ToArray();
        var regionIds = regions.Select(region => region.SourceUniqueId).ToHashSet();
        var encounters = template.Nodes.SelectMany(node => node.Encounters ?? []).GroupBy(encounter => encounter.Id, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.Ordinal);
        var bindings = Source2026BossBindings.Where(binding => sourceDifficultyId < 3 || binding.BattleId is not (2653 or 2687))
            .ToDictionary(binding => binding.BattleId);
        var chapterBundle = template.Nodes.Single(node => node.Id == "huaxiong").BossRewards;
        var normalRewards = template.Rewards.Where(reward => reward.OpeningCompanionPool is null &&
                reward.Rarity is 1 or 2)
            .Select(reward => reward.Id).ToArray();
        var nodes = new List<ShanheNodeDefinition>();
        foreach (var region in regions)
        {
            var cities = region.CityIds.Select(cityId => graph.ByCityId[cityId]).ToArray();
            var ordinaryCities = cities.Where(city => !city.IsBoss).ToArray();
            var bossCity = cities.Single(city => city.IsBoss);
            var marketId = Source2026RegionNodeId(region.SourceUniqueId, "market");
            var storyId = Source2026RegionNodeId(region.SourceUniqueId, "story");
            var campId = Source2026RegionNodeId(region.SourceUniqueId, "camp");
            var bossId = Source2026CityNodeId(bossCity.SourceCityId);
            var exits = region.NextRegionIds.Where(regionIds.Contains).Select(next => Source2026RegionNodeId(next, "market")).ToArray();
            var chapterTitle = "第" + region.ChapterNumber.ToString(CultureInfo.InvariantCulture) + "章 · " + region.Name;
            var market = template.Nodes.FirstOrDefault(node => node.Kind == ShanheNodeKind.Shop && node.ChapterNumber == region.ChapterNumber)
                ?? template.Nodes.First(node => node.Kind == ShanheNodeKind.Shop);
            nodes.Add(market with
            {
                Id = marketId, Name = region.Name + " · 集市", NextNodeIds = ordinaryCities.Select(city => Source2026CityNodeId(city.SourceCityId)).ToArray(),
                ChapterNumber = region.ChapterNumber, ChapterTitle = chapterTitle,
                StoryText = "行军来到" + region.Name + "。整备后选择一处前战地点。"
            });
            foreach (var city in ordinaryCities)
            {
                var index = Array.IndexOf(ordinaryCities, city) + 1;
                var name = region.Name + " · " + (city.SourceCityId == 5107 ? "碳墟" : "前战" + index.ToString(CultureInfo.InvariantCulture));
                var ordinary = Source2026OrdinaryEncounters(region, city, sourceDifficultyId, template);
                nodes.Add(new(Source2026CityNodeId(city.SourceCityId), name, ShanheNodeKind.Battle,
                    [region.ChapterNumber == 3 ? storyId : campId], ordinary, normalRewards)
                {
                    ChapterNumber = region.ChapterNumber, ChapterTitle = chapterTitle, CategorizedRewards = true,
                    PrimaryTacticRarity = 1, PrimarySkillRarity = region.ChapterNumber >= 2 ? 2 : 1, RewardSkipGold = 100,
                    StoryText = "驻军拦住去路。击败敌人后继续行军。",
                    Description = "本地前战驻军；此处获胜按实际地点记录，前战敌军不冒认原版战斗编号。"
                });
            }
            var stories = Source2026Stories(template, region.ChapterNumber);
            nodes.Add(new(storyId, region.Name + " · 奇遇", ShanheNodeKind.Event,
                region.ChapterNumber == 3 ? [campId] : exits, Choices: [])
            {
                ChapterNumber = region.ChapterNumber, ChapterTitle = chapterTitle, StoryEvents = stories,
                StoryText = "行军途中遇到来归的帮手、商旅或求援者。",
                Description = "随机奇遇；第一章限定的帮手仍只在第一章出现。"
            });
            var camp = template.Nodes.First(node => node.Kind == ShanheNodeKind.Camp);
            nodes.Add(camp with
            {
                Id = campId, Name = region.Name + " · 营地", NextNodeIds = [bossId],
                ChapterNumber = region.ChapterNumber, ChapterTitle = chapterTitle
            });
            var bosses = bossCity.DirectBattleIds.Where(bindings.ContainsKey).Select(battleId =>
                Source2026BossEncounter(bindings[battleId], bossCity, sourceDifficultyId, encounters)).ToArray();
            if (bosses.Length == 0) throw new InvalidOperationException("The supported source route lost every boss candidate for " + region.Name + ".");
            nodes.Add(new(bossId, region.Name + " · 关底", ShanheNodeKind.BossBattle,
                region.ChapterNumber == 3 ? [] : [storyId], bosses, [])
            {
                ChapterNumber = region.ChapterNumber, ChapterTitle = chapterTitle,
                BossRewards = region.ChapterNumber < 3 ? chapterBundle : null,
                StoryText = "驻守" + region.Name + "的首领已经现身。击败全部敌军才能继续前进。",
                Description = "关底采用已支持的来源阵容；有第二阶段时须将其一并击败。"
            });
        }
        var growth = StandardShanheSourceEnemyGrowth.Season5;
        return ShanheCampaignContent.FreezeAndValidate(template with
        {
            Id = id, Name = "山河图 · 诸侯伐董 · " + ShanheGuideTaskCatalog.SourceDifficultyName(sourceDifficultyId),
            SourceDifficultyId = sourceDifficultyId, SourceEnemyGrowth = growth,
            SourceEnemyGrowthHash = growth.ContentHash, SourceGrowthPolicy = ShanheSourceDifficultyRuntime.PolicyId,
            StartingNodeIds = regions.Where(region => region.ChapterNumber == 1)
                .Select(region => Source2026RegionNodeId(region.SourceUniqueId, "market")).ToArray(),
            RandomStartingNode = true, Nodes = nodes,
            Description = "原版25档与地区连边；已支持的来源阵容及明确本地补齐。",
            IntroText = "本入口采用普通1至炼狱5和原版地区连边，开局随机进入广成关或虎牢关。" +
                "地区内压缩为选择一处前战、营地与关底，奇遇在前两章关底后、第三章关底前揭晓。" +
                "前战驻军为本地编排，首领从已支持的来源基础阵容中选取；普通3起接入已实现的原二阶段。" +
                "二阶段只采用已核基础技能，尚未接入的高阶神威、神戟、神躯等附加技能不计为已实现；" +
                "缺失的首领与张牛角、改朝罪魁的未实现二阶段暂不进入相应候选。" +
                "各档均可直接选择；本地以每场战斗的序号作日数，体力成长同时增加体力上限，" +
                "护甲从第二轮起按每轮回复量补充至上限，已有护甲超过上限时不倒扣。" +
                "普通档前战后回复1点体力、关底后回复2点；中等1起取消自动战后回复，困难1起启用原地点地形。",
            VictoryText = "三章征程完成。", DefeatText = "此番征程止步，整备后再出发。"
        });
    }

    public static string Source2026CityNodeId(int sourceCityId) => "source-city-" + sourceCityId.ToString(CultureInfo.InvariantCulture);
    private static string Source2026RegionNodeId(int sourceRegionId, string suffix) =>
        "source-region-" + sourceRegionId.ToString(CultureInfo.InvariantCulture) + ":" + suffix;

    private static IReadOnlyList<ShanheStoryEventDefinition> Source2026Stories(ShanheCampaignDefinition template, int chapterNumber)
    {
        // The older itinerary has no chapter-three story node. Its node depth is not a story
        // eligibility rule: take common implemented stories, then rebuild the chapter-aware recruits.
        var chapterRecruitmentIds = CreateAdditionalRecruitmentAStories(1).Concat(CreateAdditionalRecruitmentBStories(1))
            .Select(story => story.Id).ToHashSet(StringComparer.Ordinal);
        var common = template.Nodes.SelectMany(node => node.StoryEvents ?? [])
            .Where(story => !chapterRecruitmentIds.Contains(story.Id) && story.Choices.All(choice => choice.BattleNodeId is null) &&
                (chapterNumber == 1 || story.Id != "story:zhijinanpeng"))
            .DistinctBy(story => story.Id, StringComparer.Ordinal);
        return common.Concat(CreateAdditionalRecruitmentAStories(chapterNumber)).Concat(CreateAdditionalRecruitmentBStories(chapterNumber)).ToArray();
    }

    private static string? Source2026Terrain(int? sourceSkillId, int difficulty) => difficulty < 11 ? null : sourceSkillId switch
    {
        13273 => StandardShanheTerrainPackage.MountainId, 13274 => StandardShanheTerrainPackage.ForestId,
        13275 => StandardShanheTerrainPackage.PlainId, 13276 => StandardShanheTerrainPackage.RiverId,
        13277 => StandardShanheTerrainPackage.CityId, 13278 => StandardShanheTerrainPackage.BattlefieldId,
        null => null, _ => throw new InvalidOperationException("Unsupported source route terrain.")
    };

    private static IReadOnlyList<ShanheEncounterDefinition> Source2026OrdinaryEncounters(ShanheSourceRouteRegionDefinition region,
        ShanheSourceRouteCityDefinition city, int difficulty, ShanheCampaignDefinition template)
    {
        // The original pools' server premises are not executable here. These local bodies have
        // no source battle identity; only the player's genuinely selected source city is retained.
        var candidates = new List<ShanheEncounterDefinition>
        {
            new("source-local:" + city.SourceCityId, region.Name + "驻军",
                region.ChapterNumber == 1 ? [new(StandardShanheLocalTroopPackage.FootmanId)] :
                    region.ChapterNumber == 2 ? [new(StandardShanheLocalTroopPackage.FootmanId), new(StandardShanheLocalTroopPackage.ArcherId)] :
                    [new(StandardShanheLocalTroopPackage.ArcherId), new(StandardShanheLocalTroopPackage.GuardId)],
                [], VictoryGold: 200, HealAfterVictory: 1)
            {
                SourceCityId = city.SourceCityId, TerrainSkillId = Source2026Terrain(city.SourceTerrainSkillId, difficulty),
                StoryText = "联军遭遇本地编排的西凉驻军。"
            }
        };
        if (region.SourceUniqueId == 10)
        {
            var mulu = template.Nodes.Single(node => node.Id == MuluEncounterNodeId).Encounters!.Single();
            candidates.Add(WithSource2026EnemyData(mulu with
            {
                Id = "source-city-" + city.SourceCityId + ":mulu-2397", Name = "呼风唤雨",
                SourceBattleId = 2397, SourceCityId = city.SourceCityId, Allies = [], BossTransition = null,
                TerrainSkillId = Source2026Terrain(city.SourceTerrainSkillId, difficulty), HealAfterVictory = 1
            }, 2397));
        }
        return candidates;
    }

    private sealed record Source2026BossBinding(int BattleId, string TemplateEncounterId, string Name);
    private static readonly IReadOnlyList<Source2026BossBinding> Source2026BossBindings = Array.AsReadOnly(new Source2026BossBinding[]
    {
        new(2632, "dagu:taishici-s5", "笃烈之士"), new(2633, "dagu:yudu-s5", "黑山匪害"),
        new(2634, "dagu:xujing-xuqiu-s5", "其实难副"),
        new(2636, "guangcheng:lijue-s5", "乱世恶徒"), new(2637, "guangcheng:guosi-s5", "祸国凶贼"),
        new(2638, "guangcheng:zhangxiu-s5", "枪王张绣"), new(2639, "guangcheng:caoxing-s5", "夺目一箭"),
        new(2640, "dagu:ma-pair-s5", "青年双俊"), new(2641, "dagu:xurong-s5", "辽东猛将"),
        new(2642, "dagu:ma-siblings-s5", "西凉兄妹"), new(2643, "dagu:han-emperors-s5", "汉末双帝"),
        new(2644, "hulao:lvbu-2644-s5", "无双奉先"), new(2645, "hulao:lvbu-2645-s5", "无双奉先"),
        new(2646, "hulao:zhangliao-armed-s5", "文远之威"), new(2647, "hulao:huaxiong-base-s5", "华雄耀武"),
        new(2648, "huanyuan:yujin-s5", "毅重镇军"), new(2649, "huanyuan:caocao-s5", "俊彦曹操"),
        new(2650, "dagu:dianwei-weizi-s5", "忠烈双卫"), new(2651, "hulao:xiahou-pair-s5", "盟中夏侯"),
        new(2652, "huanyuan:zhouyu-s5", "年少风华"), new(2653, "huanyuan:zhangniujiao-s5", "黄巾余匪"),
        new(2654, "guangcheng:yuanshu-s5", "代汉涂高"), new(2656, "guangcheng:zhangliang-s5", "末路残梁"),
        new(2657, "huanyuan:xuncan-caoshi-s5", "狂傲荀粲"), new(2658, "huanyuan:sunjian-wuguotai-s5", "贤侣虎将"),
        new(2659, "huanyuan:dongwu-veterans-s5", "东吴宿将"), new(2660, "dagu:yuanshao-s5", "盟首袁绍"),
        new(2661, "dagu:xuyou-jushou-s5", "谋臣双璧"), new(2662, "dagu:gongsun-zan-yue-s5", "白马将军"),
        new(2663, "huanyuan:xiliang-dark-s5", "西凉暗军"), new(2664, "hulao:zhuling-s5", "悍将朱灵"),
        new(2665, "dagu:yanliang-wenchou-s5", "河北双雄"), new(2666, "hulao:quyi-s5", "凉州猛将"),
        new(2667, "hulao:lukuang-luxiang-s5", "幽燕双骑"), new(2668, "luoyang:liru-s5", "毒士谋深"),
        new(2670, "luoyang:dongzhuo-2670-s5", "急功悍将"), new(2671, "luoyang:dongzhuo-2671-s5", "董氏凶僚"),
        new(2672, "huanyuan:chenlin-s5", "檄文雄才"), new(2673, "huanyuan:zhanghe-s5", "俊乂张郃"),
        new(2674, "huanyuan:moushi-s5", "谋士争衡"), new(2676, "huanyuan:xuchu-s5", "虎痴许褚"),
        new(2677, "luoyang:simayi-s5", "司马隐谋"), new(2678, "huanyuan:caoren-zhaoyun-s5", "双雄际会"),
        new(2679, "huanyuan:cao-wei-pioneers-s5", "曹魏先驱"), new(2680, "hulao:gaoshun-s5", "陷阵高顺"),
        new(2681, "huanyuan:gongsun-zan-zhaoyun-s5", "白马从龙"), new(2682, "dagu:zuoci-huatuo-s5", "奇人异士"),
        new(2683, "huanyuan:taoyuan-s5", "桃园三英"), new(2685, "luoyang:dongshi-s5", "董氏乱朝"),
        new(2686, "luoyang:fengyi-s5", "凤仪情仇"), new(2687, "luoyang:gaichao-s5", "西凉霸主")
    });

    private static ShanheEncounterDefinition Source2026BossEncounter(Source2026BossBinding binding,
        ShanheSourceRouteCityDefinition city, int difficulty, IReadOnlyDictionary<string, ShanheEncounterDefinition> templates)
    {
        var original = templates[binding.TemplateEncounterId];
        return WithSource2026EnemyData(original with
        {
            Id = "source-battle-" + binding.BattleId, Name = binding.Name,
            SourceBattleId = binding.BattleId, SourceCityId = city.SourceCityId,
            Allies = [], HealAfterVictory = 2, VictoryGold = 500,
            BossTransition = Source2026BossTransition(binding.BattleId, difficulty),
            TerrainSkillId = Source2026Terrain(city.SourceTerrainSkillId, difficulty),
            StoryText = "驻守此地的" + binding.Name + "已经列阵。"
        }, binding.BattleId);
    }

    private static ShanheBossTransition? Source2026BossTransition(int battleId, int difficulty) => difficulty < 3 ? null : battleId switch
    {
        2644 => Source2026RageTransition(difficulty),
        2645 => new(0, "classic:lu-bu", 3, 3, 4) { Combatant = Source2026StageCombatant(264501, "classic:lu-bu") },
        2670 => new(0, "ol:niu-fu", 5, 5, 4) { Combatant = Source2026StageCombatant(267001, "ol:niu-fu") },
        2671 => new(0, StandardShanheS5BossPackage.DongMinBaseId, 5, 5, 4)
        { Combatant = Source2026StageCombatant(267101, StandardShanheS5BossPackage.DongMinBaseId) },
        _ => null
    };

    private static ShanheBossTransition Source2026RageTransition(int difficulty)
    {
        var generalId = difficulty >= 21 ? StandardShanheFengxianBossPackage.SourceRageHellId :
            difficulty >= 11 ? StandardShanheFengxianBossPackage.SourceRageHardId : StandardShanheFengxianBossPackage.RageId;
        return new(0, generalId, 5, 5)
        {
            DrawCards = 4, Combatant = Source2026StageCombatant(264401, generalId),
            StartingEquipment = [new(CardKind.WushuangHalberd, Suit.Heart, 12)]
        };
    }
}
