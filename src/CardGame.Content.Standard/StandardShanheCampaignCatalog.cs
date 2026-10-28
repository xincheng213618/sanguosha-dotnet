using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

/// <summary>Original local scenario inspired by the documented 2024 Shanhe campaign structure.</summary>
public static partial class StandardShanheCampaignCatalog
{
    private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, Lazy<ContentRegistry>> BattleRegistries = new(StringComparer.Ordinal);

    public static ShanheCampaignDefinition CreateDefinition(int difficulty = 1) =>
        WithStartingResourcesContract(CreateLegacyLocalDefinition(difficulty));

    private static ShanheCampaignDefinition CreateLegacyLocalDefinition(int difficulty)
    {
        if (difficulty is < 1 or > 3) throw new ArgumentOutOfRangeException(nameof(difficulty));
        var growth = difficulty - 1;
        var rewards = new ShanheRewardDefinition[]
        {
            new("supplies", "军资", Gold: 80) { Description = "获得80铜钱，可在沿途集市购买构筑。" },
            new("medicine", "军医包", Heal: 3) { Description = "恢复3点体力，至多回复至当前上限。" },
            new("vitality", "整军", Heal: 1, Bonuses: new(MaxHp: 1)) { Description = "体力上限+1，并回复1点体力。" },
            new("opening", "筹备", Bonuses: new(InitialHand: 1)) { Description = "今后每场战斗的初始手牌+1。本地战法。" },
            new("draw", "粮道", Bonuses: new(Draw: 1)) { Description = "今后每场战斗的摸牌阶段额外摸1张。本地战法。" },
            new("slash", "连击", Bonuses: new(Slash: 1)) { Description = "今后每场战斗正常出杀次数+1。本地战法。" },
            new("wusheng", "传习·武圣", SkillId: "classic:wusheng") { Description = "获得当前经典版武圣，占用一个额外技能槽。" },
            new("longdan", "传习·龙胆", SkillId: "classic:longdan") { Description = "获得当前经典版龙胆，占用一个额外技能槽。" },
            new("mashu", "传习·马术", SkillId: "classic:mashu") { Description = "获得当前经典版马术，占用一个额外技能槽。" },
            new("qicai", "传习·奇才", SkillId: "classic:qicai") { Description = "获得当前经典版奇才，占用一个额外技能槽。" },
            new("yingzi", "传习·英姿", SkillId: "classic:yingzi") { Description = "获得当前经典版英姿，摸牌阶段多摸一张；占用额外技能槽。" },
            new("jizhi", "传习·集智", SkillId: "classic:jizhi") { Description = "获得当前经典版集智，使用普通锦囊后获得补牌；占用额外技能槽。" },
            new("zhiheng", "传习·制衡", SkillId: "classic:zhiheng") { Description = "获得当前经典版制衡，通过原生技能按钮发动；占用额外技能槽。" },
            new("tianyi", "传习·天义", SkillId: "classic:tianyi") { Description = "获得当前经典版天义，与一名角色拼点；可与赌术构筑配合。" },
            new("tactic-ruoxi", "战法·弱袭") { TacticSkillId = "shanhe:ruoxi", Description = "手牌少于当前体力时，你造成的伤害+1，含技能伤害和铁索传导。不占额外技能槽。" },
            new("tactic-jinshen", "战法·近身术") { TacticSkillId = "shanhe:jinshen", Description = "你到其他角色的最终距离固定为1。不提供额外出杀次数，不占技能槽。" },
            new("tactic-bingfan", "战法·兵反") { TacticSkillId = "shanhe:bingfan", Description = "兵粮寸断判定效果反转；相同效果只反转一次。不占技能槽。" },
            new("tactic-lezhuan", "战法·乐转") { TacticSkillId = "shanhe:lezhuan", Description = "乐不思蜀判定效果反转；相同效果只反转一次。不占技能槽。" },
            new("tactic-dushu", "战法·赌术 I") { TacticSkillId = "shanhe:dushu-1", Description = "有效拼点点数+3，至多K；可搭配天义等原生拼点技能。不占技能槽。" },
            new("tactic-fanci", "战法·反刺") { TacticSkillId = "shanhe:fanci", Description = "每回合首次受到伤害后，对所有敌人各造成1点伤害。不占技能槽。" },
            new("tactic-shixue", "战法·噬血 I") { TacticSkillId = "shanhe:shixue-1", Description = "回复体力后摸1张牌。不占技能槽。" },
            new("tactic-zengshou", "战法·增寿 I") { TacticSkillId = "shanhe:zengshou-1", Description = "每场战斗开始时增加1点空体力上限，不回复体力；该场增加的上限不跨战累加。不占技能槽。" },
            new("tactic-mouming", "战法·谋命") { TacticSkillId = "shanhe:mouming", Description = "每轮你的锦囊首次命中的牌伤改为扣减目标等量体力上限；附加伤不计入。上限归零直接死亡，不进入濒死。不占技能槽。" },
            new("tactic-xiaoming", "战法·削命") { TacticSkillId = "shanhe:xiaoming", Description = "每轮你的杀首次命中的牌伤改为扣减目标等量体力上限；酒及牌伤加成计入，弱袭等附加伤不计入。不占技能槽。" },
            new("tactic-xueren", "战法·血刃") { TacticSkillId = "shanhe:xueren", Description = "实际回复体力后，随机对一名存活敌人造成1点伤害；可与狂骨、噬血等联动。不占技能槽。" },
            new("tactic-zengshou-2", "战法·增寿 II") { TacticSkillId = "shanhe:zengshou-2", Description = "每场战斗开始时增加2点空体力上限，不回复体力，不跨战累加。不占技能槽。" },
            new("tactic-neihong", "战法·内讧") { TacticSkillId = "shanhe:neihong", Description = "敌人对另一名敌人造成伤害时，伤害+1；你不必是伤害双方之一。不占技能槽。" },
            new("tactic-miaoji-1", "战法·妙技 I") { TacticSkillId = "shanhe:miaoji-1", Description = "每个真实回合，你使用的第一张锦囊造成的伤害+1。无伤害锦囊也占次数，附加伤不计入谋命或绝情的转换金额。不占技能槽。" },
            new("tactic-miaoji-2", "战法·妙技 II") { TacticSkillId = "shanhe:miaoji-2", Description = "每个真实回合，你使用的前两张锦囊造成的伤害+1。无懈与非伤害锦囊也占次数，同一张锦囊的多个目标共用序号。不占技能槽。" },
            new("peach", "随身药囊", StartingCards: [new(CardKind.Peach, Suit.Heart, 3)])
                { Description = "每场初始手牌外再获得一张红桃3桃；本地版按普通实体牌流转。" },
            new("duel", "决战手札", StartingCards: [new(CardKind.Duel, Suit.Diamond, 1)])
                { Description = "每场初始手牌外再获得一张方块A决斗。" },
            new("armor", "八卦护身", StartingEquipment: [new(CardKind.BaguaFormation, Suit.Spade, 2)])
                { Description = "每场开始装备八卦阵；同槽新装备替换已有开场装备。" },
            new("blade", "青釭利刃", StartingEquipment: [new(CardKind.QinggangSword, Suit.Spade, 6)])
                { Description = "每场开始装备青釭剑；同槽新装备替换已有开场装备。" },
        }.Concat(StandardShanheOriginalSkills.CreateRewards()).ToArray();
        string[] spoils = ["medicine", "vitality", "opening", "draw", "slash", "wusheng", "longdan", "mashu", "qicai", "yingzi", "jizhi", "zhiheng", "tianyi", "peach", "duel", "tactic-ruoxi", "tactic-jinshen", "tactic-bingfan", "tactic-lezhuan", "tactic-dushu", "tactic-fanci", "tactic-shixue", "tactic-zengshou"];
        spoils = spoils.Concat(StandardShanheOriginalSkills.RewardIds)
            .Concat(new[] { "tactic-neihong", "tactic-miaoji-1", "tactic-miaoji-2" }).ToArray();
        var advancedSpoils = spoils.Concat(new[] { "tactic-mouming", "tactic-xiaoming", "tactic-xueren", "tactic-zengshou-2" }).ToArray();
        var nodes = new List<ShanheNodeDefinition>
        {
            Departure("hulao-start", "北路出征 · 虎牢", "hulao"),
            Departure("guangcheng-start", "南路出征 · 广成", "guangcheng"),
            Battle("hulao", "第一章 · 虎牢关", "关前旌旗密布。你沿北路接近虎牢，须先击退巡关将领，才能与联军会合。",
                ["village"], "classic:zhang-liao"),
            Battle("guangcheng", "第一章 · 广成关", "山道上散落着逃难者的车辙。你从广成关出发，截住一支离营的西凉军。",
                ["village"], "classic:gao-shun"),
            new("village", "荒村炊烟", ShanheNodeKind.Event, ["market1", "camp1"], Choices:
            [
                new("relief", "分粮救民", GoldCost: 30, RewardId: "medicine") { Description = "花30铜钱换得乡民的药材，恢复3点体力。" },
                new("training", "与乡勇切磋", HpCost: 1, RewardId: "wusheng") { Description = "花1点体力，学习武圣。" },
                new("gather", "协助收集军资", RewardId: "supplies") { Description = "获得80铜钱，继续行军。" }
            ]) { StoryText = "乡老将最后一袋粮食递给你：‘若诸侯真为救民而来，请莫让这炊烟再熄。’你决定如何回应？" },
            Market("market1", "关前集市", ["huaxiong"]),
            Camp("camp1", "联军营地", ["huaxiong"]),
            Battle("huaxiong", "第一章首领 · 汜水扬威", "孙坚的前锋被阻于关外。华雄领兵来战，你与孙坚并肩迎敌。",
                ["dagu", "huanyuan"], "classic:hua-xiong", boss: true, ally: "classic:sun-jian"),
            Battle("dagu", "第二章 · 大谷关", "董军的辎重正从南路撤走。你决定穿过大谷关，截断运粮道路。",
                ["caravan"], "ol:guo-si", rewards: advancedSpoils),
            Battle("huanyuan", "第二章 · 轘辕关", "关道狭窄，敌军借险立阵。你选择东线，与据守关隘的部队交锋。",
                ["rumor"], "classic:gao-shun", rewards: advancedSpoils),
            new("caravan", "失散的商旅", ShanheNodeKind.Event, ["market2", "camp2"], Choices:
            [
                new("escort", "护送商旅", HpCost: 1, RewardId: "supplies") { Description = "花1点体力，获得80铜钱。" },
                new("learn", "询问通关之法", RewardId: "mashu") { Description = "商旅赠你骑术心得，获得马术。" },
                new("leave", "告别继续赶路")
            ]) { StoryText = "一队商旅被困在烧毁的桥边。护送他们会拖慢行军，却也可能带来急需的消息和物资。" },
            new("rumor", "夜闻密议", ShanheNodeKind.Event, ["market2", "camp2"], Choices:
            [
                new("study", "研读遗下的图纸", RewardId: "qicai") { Description = "获得奇才。" },
                new("prepare", "收拢散落军械", RewardId: "opening") { Description = "今后每战初始手牌+1。" },
                new("rest", "不追疑兵，原地休整", RewardId: "medicine")
            ]) { StoryText = "破庙里传来密议，火光却转眼熄灭。你找到一卷残图，也看见通向两支董军的足迹。" },
            Market("market2", "行军集市", ["lijue", "guosi"]),
            Camp("camp2", "河畔营火", ["lijue", "guosi"]),
            Battle("lijue", "第二章首领 · 西凉铁骑", "李傕率军迎面压来。诸侯各有盘算，你只能先破眼前的铁骑，再作决定。",
                ["finalcamp"], "ol:li-jue", boss: true, ally: "classic:sun-jian", rewards: advancedSpoils),
            Battle("guosi", "第二章首领 · 乱军追击", "郭汜借撤军设下埋伏。你护住粮道，与回援的联军夹击乱军。",
                ["finalcamp"], "ol:guo-si", boss: true, ally: "classic:liu-bei", rewards: advancedSpoils),
            Camp("finalcamp", "第三章 · 最后的营火", ["luoyang", "suanzao", "henei"]),
            Battle("luoyang", "洛阳 · 直取董卓", "洛阳火光冲天。你选择直入京畿，与孙坚攻向董卓和李儒的军阵。",
                [], "classic:dong-zhuo", boss: true, ally: "classic:sun-jian", secondEnemy: "classic:li-ru", rewards: advancedSpoils),
            Battle("suanzao", "酸枣 · 重整诸侯", "联军因粮草和指挥争执不休。你折返酸枣，要在军阵演武中说服盟军重新合力。",
                [], "classic:yuan-shao", boss: true, ally: "classic:liu-bei", secondEnemy: "classic:yuan-shu", rewards: advancedSpoils),
            Battle("henei", "河内 · 护民北渡", "你放下追击之名，转向护送百姓渡河。西凉追兵将至，这是通往北岸的最后一战。",
                [], "ol:zhang-ji", boss: true, ally: "classic:cao-cao", secondEnemy: "classic:zhang-liao", rewards: advancedSpoils)
        };
        return WithChapterRewards(new ShanheCampaignDefinition($"shanhe:zhuhou-local-{difficulty}", "山河图 · 诸侯伐董（本地起步版）",
            GetPlayerGeneralIds(),
            ["hulao-start", "guangcheng-start"], nodes, rewards, StartingGold: 90, ExtraSkillSlots: 1,
            StartingBonuses: new(MaxHp: 2, InitialHand: 2, Draw: 1, Slash: 1))
        {
            RandomStartingNode = true,
            Description = $"剧情单机征程 · 本地难度 {difficulty}/3 · 三章分路、奇遇、商店、营地与真实战斗。阵容和数值为本地编排。",
            IntroText = "初平元年，董卓擅权，诸侯举兵。你带着一队义士踏上征程。\n关隘之外有敌军，也有求援者；洛阳、酸枣与河内，将因你的选择留下不同的故事。\n每战保留剩余体力；沿途可恢复、购买战法和习得技能。任一战败则本次征程结束，可以重新出征。",
            VictoryText = "此段征程已结束。洛阳的烽火、酸枣的盟誓、河内的归舟，都曾因你的选择而改变。你可以换一位武将、另一条路线，再走一段不同的山河。",
            DefeatText = "军旗暂歇，义军退守。此番征程虽止，曾经的选择仍可成为下次出征的经验。重新整备，再赴山河。"
        });

        ShanheNodeDefinition Battle(string id, string name, string story, string[] next, string enemy,
            bool boss = false, string? ally = null, string? secondEnemy = null, string[]? rewards = null)
        {
            var enemies = new List<ShanheCombatantDefinition> { new(enemy, MaxHpBonus: growth + (boss ? 1 : 0),
                InitialHandBonus: growth, DrawBonus: difficulty == 3 ? 1 : 0) };
            if (secondEnemy is not null) enemies.Add(new(secondEnemy, MaxHpBonus: growth));
            return new(id, name, boss ? ShanheNodeKind.BossBattle : ShanheNodeKind.Battle, next,
                [new(id + ":encounter", name, enemies, ally is null ? [] : [new(ally, MaxHpBonus: 1)],
                    VictoryGold: boss ? 100 : 55, HealAfterVictory: boss ? 2 : 1)], rewards ?? spoils)
                {
                    StoryText = story,
                    AddSkillSlotsOnVictory = boss && next.Length != 0 ? 1 : 0,
                    RewardSkipGold = 100,
                    VictoryText = id switch
                    {
                        "luoyang" => "【结局 · 洛阳破晓】\n董军败退，城门终于打开。你和孙坚立在余烬之间，扶起一面尚未烧尽的汉旗。天下未定，但京畿百姓等来了这一日的曙光。",
                        "suanzao" => "【结局 · 重续盟誓】\n酸枣军阵收兵。你用这一战赢得诸侯的认可，分散的军旗再次转向西方。有人为名，有人为利，而你终于让他们共同向前一步。",
                        "henei" => "【结局 · 北渡归舟】\n追兵退去，最后一艘渡船驶向北岸。你没有把名字留在洛阳城头，却让一村人的灯火在河内重新亮起。山河之大，也容得下这样的胜利。",
                        _ => ""
                    },
                    Description = $"{(boss ? "首领战" : "遭遇战")} · 胜利获得铜钱与战利品；剩余体力带入下一战。"
                };
        }

        ShanheNodeDefinition Departure(string id, string name, string next) => new(id, name, ShanheNodeKind.Event, [next], Choices:
        [
            new("supplies", "向盟军领取军资", RewardId: "supplies") { Description = "获得80铜钱，为沿途集市作准备。" },
            new("prepare", "与义士整备行装", RewardId: "opening") { Description = "每场战斗的初始手牌+1。" },
            new("train", "操练刀阵", RewardId: "slash") { Description = "每场战斗正常出杀次数+1。" },
            new("tactic", "研习战法·近身术", RewardId: "tactic-jinshen") { Description = "掌握独立战法：你到其他角色的距离固定为1，不占额外技能槽。" }
        ]) { StoryText = "诸侯于酸枣会盟。你接过出征的军令，先为同行的义士作最后一次准备。营门之外，一段属于你的征程正要开始。", Description = "选择一项出征准备，随后进入关隘。" };

        ShanheNodeDefinition Market(string id, string name, string[] next) => new(id, name, ShanheNodeKind.Shop, next, Choices:
        [
            new("buy-medicine", "军医包 · 35钱", 35, RewardId: "medicine", LeavesNode: false),
            new("buy-draw", "粮道 · 100钱", 100, RewardId: "draw", LeavesNode: false),
            new("buy-slash", "连击 · 90钱", 90, RewardId: "slash", LeavesNode: false),
            new("buy-ruoxi", "战法·弱袭 · 100钱", 100, RewardId: "tactic-ruoxi", LeavesNode: false),
            new("buy-fanci", "战法·反刺 · 100钱", 100, RewardId: "tactic-fanci", LeavesNode: false),
            new("buy-dushu", "战法·赌术 I · 80钱", 80, RewardId: "tactic-dushu", LeavesNode: false),
            new("buy-tianyi", "技能·天义 · 80钱", 80, RewardId: "tianyi", LeavesNode: false),
            new("buy-neihong", "战法·内讧 · 60钱", 60, RewardId: "tactic-neihong", LeavesNode: false),
            new("buy-miaoji", "战法·妙技 I · 60钱", 60, RewardId: "tactic-miaoji-1", LeavesNode: false),
            new("buy-armor", "八卦护身 · 100钱", 100, RewardId: "armor", LeavesNode: false),
            new("buy-blade", "青釭利刃 · 80钱", 80, RewardId: "blade", LeavesNode: false),
            .. (id == "market2" ? new ShanheNodeChoiceDefinition[]
            {
                new("buy-mouming", "战法·谋命 · 160钱", 160, RewardId: "tactic-mouming", LeavesNode: false),
                new("buy-xiaoming", "战法·削命 · 180钱", 180, RewardId: "tactic-xiaoming", LeavesNode: false),
                new("buy-xueren", "战法·血刃 · 220钱", 220, RewardId: "tactic-xueren", LeavesNode: false),
                new("buy-miaoji-2", "战法·妙技 II · 100钱", 100, RewardId: "tactic-miaoji-2", LeavesNode: false)
            } : [])
        ]) { StoryText = "商人支起灯笼：‘行军可以缺酒，不能缺粮。将军要什么？’每件货物仅可购买一次。", Description = "花费本次征程的铜钱购买成长，购买后仍可继续挑选。" };

        ShanheNodeDefinition Camp(string id, string name, string[] next) => new(id, name, ShanheNodeKind.Camp, next, Choices:
        [
            new("rest", "疗伤休整", RewardId: "medicine") { Description = "恢复3点体力。" },
            new("train", "操练军阵", HpCost: 1, RewardId: "vitality") { Description = "先花1点体力，体力上限+1并回复1点。" },
            new("prepare", "准备行装", RewardId: "opening") { Description = "今后每场战斗初始手牌+1。" }
        ]) { StoryText = "夜色落在营帐之间。你听见士卒的乡音，思量着明日的道路。趁出发前，再作一次整备。", Description = "选择一种整备后继续前进。" };
    }

    public static ContentRegistry CreateBattleRegistry(ShanheBattleSetup setup)
    {
        setup = ShanheBattleSetup.Freeze(setup);
        if (!setup.RebirthAvailable && setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(ShanheGeneralRanks.RebirthSkillId, StringComparer.Ordinal))
            throw new InvalidOperationException("The rank-five rebirth skill requires this campaign's unspent rebirth privilege.");
        // Reserve the second form only for content dependency discovery. This
        // view never becomes a playable roster; the mode below uses the real setup.
        var requirements = setup.BossTransition is { } second
            ? setup with { EnemyRoster = Array.AsReadOnly(setup.EnemyRoster.Append(new ShanheCombatantDefinition(second.GeneralId)).ToArray()) }
            : setup;
        if (setup.MarketSetup is { } market)
        {
            var rewards = market.Definition.Rewards;
            // Dependency discovery includes future purchases without granting them.
            requirements = requirements with
            {
                ExtraSkillIds = Array.AsReadOnly(requirements.ExtraSkillIds.Concat(rewards.Select(reward => reward.SkillId)
                    .OfType<string>()).Distinct(StringComparer.Ordinal).ToArray()),
                TacticSkillIds = Array.AsReadOnly(requirements.TacticSkillIds.Concat(rewards.SelectMany(reward =>
                    new[] { reward.TacticSkillId, reward.CampaignEconomy?.BattleSkillId }.OfType<string>()))
                    .Concat((market.Definition.Schools ?? []).Select(ShanheSchoolEffects.SkillId).OfType<string>())
                    .Distinct(StringComparer.Ordinal).ToArray())
            };
        }
        var ids = new[] { requirements.PlayerGeneralId }.Concat(requirements.AllyRoster.Select(m => m.GeneralId))
            .Concat(requirements.EnemyRoster.Select(m => m.GeneralId)).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal);
        var localTroops = ids.Any(id => id is StandardShanheLocalTroopPackage.FootmanId or
            StandardShanheLocalTroopPackage.ArcherId or StandardShanheLocalTroopPackage.GuardId or
            StandardShanheLocalTroopPackage.MedicId or StandardShanheLocalTroopPackage.RedBladeId or
            StandardShanheLocalTroopPackage.CaptainId);
        var codexEquipment = RequiresCodexEquipmentRegistry(requirements);
        var currentFarStrike = RequiresCurrentFarStrikeRegistry(requirements);
        var currentJinnangJi = RequiresCurrentJinnangJiRegistry(requirements);
        var currentDuoduoYishan = RequiresCurrentDuoduoYishanRegistry(requirements);
        var currentYaoli = RequiresCurrentYaoliRegistry(requirements);
        var currentLegendaryTactics = RequiresCurrentLegendaryTacticsRegistry(requirements);
        var buildTactics = RequiresBuildTacticsRegistry(requirements);
        var reactionTactics = RequiresReactionTacticsRegistry(requirements);
        var thirdSlashTactics = RequiresCurrentThirdSlashRegistry(requirements);
        var miaoshouTactic = RequiresCurrentMiaoshouRegistry(requirements);
        var lateRoundTactics = RequiresCurrentLateRoundTacticsRegistry(requirements);
        var teamHealthTactic = RequiresCurrentTeamHealthRegistry(requirements);
        var wineTactics = RequiresCurrentWineTacticsRegistry(requirements);
        var zuiquanTactic = RequiresCurrentZuiquanRegistry(requirements);
        var guandaoTactic = RequiresCurrentGuandaoRegistry(requirements);
        var lowHpDrawTactics = RequiresCurrentLowHpDrawRegistry(requirements);
        var ershengsanTactic = RequiresCurrentErshengsanRegistry(requirements);
        var heartBladeTactic = RequiresCurrentHeartBladeRegistry(requirements);
        var dualBladeTactics = RequiresCurrentDualBladeRegistry(requirements);
        var handLimitStock = RequiresCurrentHandLimitStockRegistry(requirements);
        var doubleStealTactic = RequiresCurrentDoubleStealRegistry(requirements);
        var doubleResponseTactics = RequiresCurrentDoubleResponseRegistry(requirements);
        var trickResponseTactics = RequiresCurrentTrickResponseRegistry(requirements);
        var qianlongTactic = RequiresCurrentQianlongRegistry(requirements);
        var multiTargetTrickTactics = RequiresCurrentMultiTargetTrickRegistry(requirements);
        var xuyanTactic = RequiresCurrentXuyanRegistry(requirements);
        var alliedTrickTactics = RequiresCurrentAlliedTrickRegistry(requirements);
        var yongzhanTactics = RequiresCurrentYongzhanRegistry(requirements);
        var qiaoquHaoduoTactic = RequiresCurrentQiaoquHaoduoRegistry(requirements);
        var targetHandVisibility = RequiresCurrentTargetHandVisibilityRegistry(requirements);
        var dismantlementStock = RequiresCurrentDismantlementStockRegistry(requirements);
        var badaoTactics = RequiresCurrentBadaoRegistry(requirements);
        var steadyHandTactic = RequiresCurrentSteadyHandRegistry(requirements);
        var counterspellClaimTactic = RequiresCurrentCounterspellClaimRegistry(requirements);
        var mobilizationTactic = RequiresCurrentMobilizationRegistry(requirements);
        var extraSkills = requirements.ExtraSkillIds.Concat(requirements.TacticSkillIds)
            .Concat(requirements.AllyRoster.Concat(requirements.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .ToHashSet(StringComparer.Ordinal);
        var schoolEffects = extraSkills.Any(id => ShanheSchoolEffects.SchoolId(id) is not null);
        var guideSkills = extraSkills.Contains(StandardShanheGuideSkillsPackage.BuwuSkillId) ||
            extraSkills.Contains(StandardShanheGuideSkillsPackage.ShuanghunSkillId);
        var xiasheng = extraSkills.Contains(StandardShanheXiashengPackage.SkillId);
        var chanrangEdict = extraSkills.Contains(StandardShanheChanrangEdictPackage.SkillId) ||
            ShanheRecipeCards(setup).Any(card => card.Kind == CardKind.ShanRangZhaoShu) ||
            setup.MarketSetup?.Definition.Rewards.Any(reward => (reward.StartingEquipment ?? []).Concat(reward.StartingCards ?? [])
                .Any(card => card.Kind == CardKind.ShanRangZhaoShu)) == true;
        var currentFuqi = extraSkills.Contains(StandardShanheFuqiPackage.SkillId);
        var currentWangong = extraSkills.Contains(StandardShanheWangongPackage.SkillId);
        var currentLonghun = extraSkills.Contains(StandardShanheLonghunPackage.SkillId);
        var currentYiji = extraSkills.Contains(StandardShanheYijiPackage.SkillId);
        var currentCappedRefill = extraSkills.Contains(StandardShanheCappedRefillSkillsPackage.SkillId);
        var currentBeige = extraSkills.Contains(StandardShanheBeigeSkillsPackage.SkillId);
        var fengxianBoss = ids.Any(id => id is StandardShanheFengxianBossPackage.FirstId or StandardShanheFengxianBossPackage.RageId or
            StandardShanheFengxianBossPackage.SourceRageHardId or StandardShanheFengxianBossPackage.SourceRageHellId) ||
            extraSkills.Contains(StandardShanheFengxianBossPackage.XiuluoId) || extraSkills.Contains(StandardShanheFengxianBossPackage.MashuId) ||
            extraSkills.Contains(StandardShanheFengxianBossPackage.ShenweiId) || extraSkills.Contains(StandardShanheFengxianBossPackage.ShenjiId);
        var confidentLvbu = ids.Contains(StandardShanheConfidentLvbuPackage.LvbuId, StringComparer.Ordinal) ||
            extraSkills.Contains(StandardShanheConfidentLvbuPackage.XiaoruiId);
        var eliteDongwu = ids.Any(id => id is StandardShanheEliteDongwuPackage.HuangGaiId or StandardShanheEliteDongwuPackage.ChengPuId) ||
            extraSkills.Contains(StandardShanheEliteDongwuPackage.ChunlaoId);
        var eliteHandang = ids.Contains(StandardShanheEliteHandangPackage.HandangId, StringComparer.Ordinal) ||
            extraSkills.Contains(StandardShanheEliteHandangPackage.ZizhongId) || extraSkills.Contains(StandardShanheEliteHandangPackage.MengwuId);
        var recruitment = extraSkills.Contains(StandardShanheRecruitmentPackage.HuihuoSkillId) ||
            ids.Any(id => id is StandardShanheRecruitmentPackage.XunyuNormalId or StandardShanheRecruitmentPackage.XunyuEliteId);
        var caosongCompanion = ids.Contains(StandardShanheCaosongCompanionPackage.EliteId, StringComparer.Ordinal) ||
            extraSkills.Contains(StandardShanheCaosongCompanionPackage.HuanghanSkillId);
        var caoruiCompanion = ids.Any(id => id is StandardShanheCaoruiCompanionPackage.NormalId or StandardShanheCaoruiCompanionPackage.EliteId) ||
            extraSkills.Contains(StandardShanheCaoruiCompanionPackage.LuanxinSkillId);
        var wuguotaiCompanion = ids.Contains(StandardShanheWuguotaiCompanionPackage.EliteId, StringComparer.Ordinal);
        var guanpingCompanion = ids.Contains(StandardShanheGuanpingCompanionPackage.EliteId, StringComparer.Ordinal) ||
            extraSkills.Contains(StandardShanheGuanpingCompanionPackage.MoyanSkillId);
        var chilianCompanion = ids.Contains(StandardShanheChilianCompanionPackage.ChilianId, StringComparer.Ordinal) ||
            extraSkills.Contains(StandardShanheChilianCompanionPackage.ChunlaoId) || extraSkills.Contains(StandardShanheChilianCompanionPackage.LiangyinId);
        var currentDingcuo = extraSkills.Contains(StandardShanheDingcuoPackage.SkillId);
        var currentGuanchao = extraSkills.Contains(StandardShanheGuanchaoPackage.SkillId);
        var wealthTactics = extraSkills.Contains(StandardShanheWealthPackage.WealthHandLimitSkillId) ||
            extraSkills.Contains(StandardShanheWealthPackage.WealthInitialHandSkillId) ||
            extraSkills.Contains(StandardShanheWealthPackage.WealthDrawSkillId) ||
            extraSkills.Contains(StandardShanheWealthPackage.WealthDamageSkillId);
        var currentSlashTactics = RequiresCurrentSlashTacticsRegistry(requirements);
        var s5Chengxiong = RequiresChengxiongBossRegistry(requirements);
        var s5CaoxingHigh = RequiresCaoxingHighRegistry(requirements);
        var s5Shidu = RequiresShiduBossRegistry(requirements) || s5CaoxingHigh;
        var s5Huaxiong = RequiresHuaxiongBossRegistry(requirements);
        var s5Taishici = RequiresTaishiciBossRegistry(requirements);
        var s5Caocao = RequiresCaocaoBossRegistry(requirements);
        var s5Yujin = RequiresYujinBossRegistry(requirements);
        var s5Yudu = RequiresYuduBossRegistry(requirements);
        var s5Liru = RequiresLiruBossRegistry(requirements);
        var s5Yuanshao = RequiresThreeBossRegistry(requirements, StandardShanheS5YuanshaoPackage.YuanshaoId,
            StandardShanheS5YuanshaoPackage.LuanjiId);
        var s5Zhangliang = RequiresThreeBossRegistry(requirements, StandardShanheS5ZhangliangPackage.ZhangliangId,
            StandardShanheS5ZhangliangPackage.BaoyuId, StandardShanheS5ZhangliangPackage.WuyingId);
        var s5Xuchu = RequiresThreeBossRegistry(requirements, StandardShanheS5XuchuPackage.XuchuId,
            StandardShanheS5XuchuPackage.LuoyiId);
        var s5Zhuling = RequiresThreeBossRegistry(requirements, StandardShanheS5ZhulingPackage.ZhulingId,
            StandardShanheS5ZhulingPackage.JixianId, StandardShanheS5ZhulingPackage.BaoliId);
        var s5YanliangWenchou = RequiresThreeBossRegistry(requirements, StandardShanheS5YanliangWenchouPackage.YanliangWenchouId,
            StandardShanheS5YanliangWenchouPackage.ShuangxiongId);
        var s5Quyi = RequiresThreeBossRegistry(requirements, StandardShanheS5QuyiPackage.QuyiId,
            StandardShanheS5QuyiPackage.FuqiId, StandardShanheS5QuyiPackage.JiaoziId);
        var s5Zhanghe = RequiresThreeBossRegistry(requirements, StandardShanheS5ZhanghePackage.ZhangheId,
            StandardShanheS5ZhanghePackage.ZhouxuanId);
        var s5Yuanshu = RequiresThreeBossRegistry(requirements, StandardShanheS5YuanshuPackage.YuanshuId,
            StandardShanheS5YuanshuPackage.TongjiId, StandardShanheS5YuanshuPackage.WuzangId) ||
            ids.Contains(StandardShanheS5YuanshuPackage.YuanyaoId, StringComparer.Ordinal) ||
            ids.Contains(StandardShanheS5YuanshuPackage.YuanyinId, StringComparer.Ordinal);
        var s5Gongsun = RequiresThreeBossRegistry(requirements, StandardShanheS5GongsunZanPackage.GongsunZanId,
            StandardShanheS5GongsunZanPackage.QiaomengId, StandardShanheS5GongsunZanPackage.JiuzhuId) ||
            ids.Contains(StandardShanheS5GongsunZanPackage.GongsunYueId, StringComparer.Ordinal);
        var s5Taoyuan = RequiresThreeBossRegistry(requirements, StandardShanheS5TaoyuanPackage.ZhangfeiId,
            StandardShanheS5TaoyuanPackage.TishenId) ||
            ids.Contains(StandardShanheS5TaoyuanPackage.LiubeiId, StringComparer.Ordinal) ||
            ids.Contains(StandardShanheS5TaoyuanPackage.GuanyuId, StringComparer.Ordinal);
        var s5Lijue = RequiresThreeBossRegistry(requirements, StandardShanheS5LiJuePackage.LijueId,
            StandardShanheS5LiJuePackage.LangxiId);
        var s5Guosi = RequiresThreeBossRegistry(requirements, StandardShanheS5GuosiPackage.GuosiId,
            StandardShanheS5GuosiPackage.ZhanhunDaoSkillId) || ShanheRecipeCards(setup).Any(card => card.Kind == CardKind.ZhanhunDao);
        var s5MaPair = RequiresThreeBossRegistry(requirements, StandardShanheS5MaPairPackage.MaPairId,
            StandardShanheS5MaPairPackage.AnqiFeirenSkillId) || ShanheRecipeCards(setup).Any(card => card.Kind == CardKind.AnqiFeiren);
        var s5XuRong = RequiresThreeBossRegistry(requirements, StandardShanheS5XuRongPackage.XurongId,
            StandardShanheS5XuRongPackage.ShajueId, StandardShanheS5XuRongPackage.YaodaoChitongSkillId) ||
            ShanheRecipeCards(setup).Any(card => card.Kind == CardKind.YaodaoChitong);
        var s5XiliangDark = RequiresThreeBossRegistry(requirements, StandardShanheS5XiliangDarkPackage.QinyiId,
            StandardShanheS5XiliangDarkPackage.YimieId) ||
            ids.Any(id => id is StandardShanheS5XiliangDarkPackage.ChenweiId or StandardShanheS5XiliangDarkPackage.LiheiId);
        var s5Simayi = RequiresThreeBossRegistry(requirements, StandardShanheS5SimayiPackage.SimayiId,
            StandardShanheS5SimayiPackage.RenjieId, StandardShanheS5SimayiPackage.BaiyinId);
        var s5Dianwei = RequiresThreeBossRegistry(requirements, StandardShanheS5DianweiWeiziPackage.DianweiId,
            StandardShanheS5DianweiWeiziPackage.QiangxiId) || ids.Contains(StandardShanheS5DianweiWeiziPackage.WeiziId, StringComparer.Ordinal);
        var s5Xiahou = RequiresThreeBossRegistry(requirements, StandardShanheS5XiahouPairPackage.XiahoudunId,
            StandardShanheS5XiahouPairPackage.GanglieId, StandardShanheS5XiahouPairPackage.ShensuId) ||
            ids.Contains(StandardShanheS5XiahouPairPackage.XiahouyuanId, StringComparer.Ordinal);
        var s5Zhangxiu = RequiresThreeBossRegistry(requirements, StandardShanheS5ZhangxiuPackage.ZhangxiuId,
            StandardShanheS5ZhangxiuPackage.XiongluanId, StandardShanheS5ZhangxiuPackage.LeizhenSkillId) ||
            ShanheRecipeCards(setup).Any(c => c.Kind == CardKind.Leizhen);
        var s5Zuoci = RequiresThreeBossRegistry(requirements, StandardShanheS5ZuociHuatuoPackage.ZuociId,
            StandardShanheS5ZuociHuatuoPackage.HuashenId, StandardShanheS5ZuociHuatuoPackage.XinshengId) ||
            ids.Contains(StandardShanheS5ZuociHuatuoPackage.HuatuoId, StringComparer.Ordinal);
        var s5CaoWei = RequiresThreeBossRegistry(requirements, StandardShanheS5CaoWeiPioneersPackage.CaocaoId,
            StandardShanheS5CaoWeiPioneersPackage.JianxiongId, StandardShanheS5CaoWeiPioneersPackage.HujiaId) ||
            ids.Any(id => id is StandardShanheS5CaoWeiPioneersPackage.CaozhenId or StandardShanheS5CaoWeiPioneersPackage.CaohongId);
        var s5Xingtian = ShanheRecipeCards(setup).Any(c => c.Kind == CardKind.ShanheXingtianAxe) ||
            setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
                .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
                .Contains(StandardShanheXingtianPackage.SkillId, StringComparer.Ordinal);
        var sourceSanshou = ShanheRecipeCards(setup).Any(card => card.Kind == CardKind.ShanheSanshou) ||
            extraSkills.Contains(StandardShanheSanshouArmorPackage.SkillId) ||
            setup.TacticSkillIds.Contains(StandardShanheSanshouArmorPackage.SkillId, StringComparer.Ordinal);
        var blackLightArmor = ShanheRecipeCards(setup).Any(card => card.Kind == CardKind.BlackLightArmor) ||
            extraSkills.Contains(StandardShanheBlackLightArmorPackage.SkillId) ||
            setup.TacticSkillIds.Contains(StandardShanheBlackLightArmorPackage.SkillId, StringComparer.Ordinal);
        var gouXiangArmor = ShanheRecipeCards(setup).Any(card => card.Kind == CardKind.GouXiang) ||
            extraSkills.Contains(StandardShanheGouXiangArmorPackage.SkillId) ||
            setup.TacticSkillIds.Contains(StandardShanheGouXiangArmorPackage.SkillId, StringComparer.Ordinal);
        var duanhun = ShanheRecipeCards(setup).Any(card => card.Kind == CardKind.Duanhun) ||
            extraSkills.Contains(StandardShanheDuanhunPackage.SkillId) ||
            setup.TacticSkillIds.Contains(StandardShanheDuanhunPackage.SkillId, StringComparer.Ordinal);
        var kunlunMirror = ShanheRecipeCards(setup).Any(card => card.Kind == CardKind.KunlunMirror) ||
            extraSkills.Contains(StandardShanheKunlunMirrorPackage.SkillId) ||
            setup.TacticSkillIds.Contains(StandardShanheKunlunMirrorPackage.SkillId, StringComparer.Ordinal);
        var zhaoguMirror = ShanheRecipeCards(setup).Any(card => card.Kind == CardKind.ZhaoguMirror) ||
            extraSkills.Contains(StandardShanheZhaoguMirrorPackage.SkillId) ||
            setup.TacticSkillIds.Contains(StandardShanheZhaoguMirrorPackage.SkillId, StringComparer.Ordinal);
        var huxinMirror = ShanheRecipeCards(setup).Any(card => card.Kind == CardKind.HuxinMirror) ||
            extraSkills.Contains(StandardShanheHuxinMirrorPackage.SkillId) ||
            setup.TacticSkillIds.Contains(StandardShanheHuxinMirrorPackage.SkillId, StringComparer.Ordinal);
        var s5Zhangliao = RequiresThreeBossRegistry(requirements, StandardShanheS5BossPackage.ZhangliaoBaseId,
            StandardShanheS5BossPackage.TuxiId);
        var s5MaSiblings = s5MaPair || RequiresThreeBossRegistry(requirements, StandardShanheS5MaSiblingsPackage.MachaoId,
            StandardShanheS5MaSiblingsPackage.MashuId, StandardShanheS5MaSiblingsPackage.TieqiId) ||
            ids.Contains(StandardShanheS5MaSiblingsPackage.MayunluId, StringComparer.Ordinal);
        var s5Dongwu = RequiresThreeBossRegistry(requirements, StandardShanheS5DongwuVeteransPackage.ChengpuId,
            StandardShanheS5DongwuVeteransPackage.ChunlaoId) || ids.Contains(StandardShanheS5DongwuVeteransPackage.HandangId, StringComparer.Ordinal);
        var s5Moushi = RequiresThreeBossRegistry(requirements, StandardShanheS5MoushiPackage.XunchenId,
            StandardShanheS5MoushiPackage.MoushiId, StandardShanheS5MoushiPackage.JiemingId) ||
            ids.Contains(StandardShanheS5MoushiPackage.GuotuId, StringComparer.Ordinal) ||
            ids.Contains(StandardShanheS5MoushiPackage.ShenpeiId, StringComparer.Ordinal);
        var s5HanEmperors = RequiresThreeBossRegistry(requirements, StandardShanheS5HanEmperorsPackage.LiuxieId,
            StandardShanheS5HanEmperorsPackage.ShiyuanId) || ids.Contains(StandardShanheS5HanEmperorsPackage.LiubianId, StringComparer.Ordinal);
        var sourceShenNu = RequiresThreeBossRegistry(requirements,
            StandardShanheSourceShenNuStagePackage.GeneralId, StandardShanheSourceShenNuStagePackage.SkillId);
        var sourceQiangWang = RequiresThreeBossRegistry(requirements, StandardShanheSourceQiangWangPackage.GeneralId);
        var sourceXuSheng = RequiresThreeBossRegistry(requirements, StandardShanheSourceXuShengPackage.BaseGeneralId,
            StandardShanheSourceXuShengPackage.SkillId) || ids.Contains(StandardShanheSourceXuShengPackage.MediumGeneralId, StringComparer.Ordinal);
        var sourceSunCe = RequiresThreeBossRegistry(requirements, StandardShanheSourceSunCePackage.SunCeId,
            StandardShanheSourceSunCePackage.HunziSkillId, StandardShanheSourceSunCePackage.YinghunSkillId);
        var sourceXunYu = RequiresThreeBossRegistry(requirements, StandardShanheSourceXunYuPackage.GeneralId,
            StandardShanheSourceXunYuPackage.QuhuSkillId);
        var sourceMouDongZhuo = RequiresThreeBossRegistry(requirements, StandardShanheSourceMouDongZhuoPackage.GeneralId,
            StandardShanheSourceMouDongZhuoPackage.XiongniSkillId, StandardShanheSourceMouDongZhuoPackage.FengshangSkillId,
            StandardShanheSourceMouDongZhuoPackage.ZhibingSkillId, StandardShanheSourceMouDongZhuoPackage.ZhixiSkillId);
        var sourceVeteran = RequiresSourceVeteranRegistry(requirements);
        var sourceWuhuanRider = RequiresThreeBossRegistry(requirements, StandardShanheSourceWuhuanRiderPackage.BaseId,
            StandardShanheSourceWuhuanRiderPackage.TuxiSkillId, StandardShanheSourceWuhuanRiderPackage.JixiSkillId) ||
            ids.Any(id => id is StandardShanheSourceWuhuanRiderPackage.HardId or StandardShanheSourceWuhuanRiderPackage.HellId);
        var sourceWarrior = RequiresThreeBossRegistry(requirements, StandardShanheSourceWarriorPackage.BaseId,
            StandardShanheSourceWarriorPackage.YoudiSkillId, StandardShanheSourceWarriorPackage.LuoyiSkillId) ||
            ids.Any(id => id is StandardShanheSourceWarriorPackage.HardId or StandardShanheSourceWarriorPackage.HellId);
        var sourceQiangRaider = ids.Any(id => id is StandardShanheSourceQiangRaiderPackage.BaseId or
            StandardShanheSourceQiangRaiderPackage.HardId or StandardShanheSourceQiangRaiderPackage.NightmareId);
        var sourceXunYue = RequiresThreeBossRegistry(requirements, StandardShanheSourceXunYuePackage.BaseId,
            StandardShanheSourceXunYuePackage.QiangzhiSkillId) ||
            ids.Any(id => id is StandardShanheSourceXunYuePackage.HardId or StandardShanheSourceXunYuePackage.HellId);
        var sourceJiangdongVeteran = RequiresThreeBossRegistry(requirements, StandardShanheSourceJiangdongVeteranPackage.BaseId,
            StandardShanheSourceJiangdongVeteranPackage.FenliSkillId) ||
            ids.Any(id => id is StandardShanheSourceJiangdongVeteranPackage.HardId or StandardShanheSourceJiangdongVeteranPackage.NightmareId);
        var sourceCoalitionScout = ids.Any(id => id is StandardShanheSourceCoalitionScoutPackage.BaseId or
            StandardShanheSourceCoalitionScoutPackage.HardId or StandardShanheSourceCoalitionScoutPackage.HellId);
        var sourceCoalitionSpy = ids.Any(id => id is StandardShanheSourceCoalitionSpyPackage.BaseId or
            StandardShanheSourceCoalitionSpyPackage.HardId or StandardShanheSourceCoalitionSpyPackage.NightmareId);
        var sourceLocalMagnate = ids.Any(id => id is StandardShanheSourceLocalMagnatePackage.BaseId or
            StandardShanheSourceLocalMagnatePackage.HardId or StandardShanheSourceLocalMagnatePackage.NightmareId);
        var sourceLinZhongGu = ids.Any(id => id is StandardShanheSourceLinZhongGuPackage.BaseId or
            StandardShanheSourceLinZhongGuPackage.NightmareId);
        var sourceXinPing = ids.Contains(StandardShanheSourceXinPingPackage.GeneralId, StringComparer.Ordinal);
        var sourceDemonAssassin = ids.Any(id => id is StandardShanheSourceDemonAssassinPackage.BaseId or
            StandardShanheSourceDemonAssassinPackage.HardId or StandardShanheSourceDemonAssassinPackage.HellId);
        var sourceBuHeng = RequiresThreeBossRegistry(requirements, StandardShanheSourceBuHengPackage.BaseId,
            StandardShanheSourceBuHengPackage.ShengxiSkillId) ||
            ids.Any(id => id is StandardShanheSourceBuHengPackage.HardId or StandardShanheSourceBuHengPackage.NightmareId);
        var sourceMilitiaTrio = extraSkills.Contains(StandardShanheSourceMilitiaTrioPackage.QingjianSkillId) ||
            ids.Any(id => id.StartsWith("shanhe:source-militia-trio-2475-", StringComparison.Ordinal));
        var sourceCaoSongTrio = extraSkills.Contains(StandardShanheSourceCaoSongTrioPackage.ChuanyunSkillId) ||
            extraSkills.Contains(StandardShanheSourceCaoSongTrioPackage.LongweiSkillId) ||
            ids.Any(id => id.StartsWith("shanhe:source-caosong-trio-2485-", StringComparison.Ordinal));
        var sourceWuCong = extraSkills.Contains(StandardShanheSourceWuCongPackage.TiepaoSkillId) ||
            ids.Any(id => id is StandardShanheSourceWuCongPackage.BaseId or StandardShanheSourceWuCongPackage.HardId);
        var sourceQianZhaoPair = ids.Any(id => id.StartsWith("shanhe:source-qianzhao-pair-2471-", StringComparison.Ordinal));
        var sourceYulinOfficer = ids.Any(id => id is StandardShanheSourceYulinOfficerPackage.BaseId or
            StandardShanheSourceYulinOfficerPackage.HardId or StandardShanheSourceYulinOfficerPackage.HellId);
        var sourceYulinGuard = sourceMilitiaTrio || sourceYulinOfficer || RequiresThreeBossRegistry(requirements, StandardShanheSourceYulinGuardPackage.BaseId,
            StandardShanheSourceYulinGuardPackage.JushouSkillId) ||
            ids.Any(id => id is StandardShanheSourceYulinGuardPackage.HardId or StandardShanheSourceYulinGuardPackage.HellId);
        var sourceAgricultureOfficer = RequiresThreeBossRegistry(requirements, StandardShanheSourceAgricultureOfficerPackage.BaseId,
            StandardShanheSourceAgricultureOfficerPackage.TuntianSkillId) ||
            ids.Any(id => id is StandardShanheSourceAgricultureOfficerPackage.HardId or StandardShanheSourceAgricultureOfficerPackage.HellId);
        var sourceKeshou = sourceBuHeng || sourceYulinGuard || extraSkills.Contains(StandardShanheSourceKeshouPackage.SkillId);
        var sourceWhiteHorse = RequiresThreeBossRegistry(requirements, StandardShanheSourceWhiteHorsePackage.BaseId,
            StandardShanheSourceWhiteHorsePackage.XiyingSkillId) ||
            ids.Any(id => id is StandardShanheSourceWhiteHorsePackage.HardId or StandardShanheSourceWhiteHorsePackage.HellId);
        var sourceDemonRaider = extraSkills.Contains(StandardShanheSourceDemonRaiderPackage.JielueSkillId) ||
            ids.Any(id => id is StandardShanheSourceDemonRaiderPackage.BaseId or StandardShanheSourceDemonRaiderPackage.HardId or
                StandardShanheSourceDemonRaiderPackage.HellId or StandardShanheSourceDemonRaiderPackage.TrioBaseId or
                StandardShanheSourceDemonRaiderPackage.TrioHardId or StandardShanheSourceDemonRaiderPackage.TrioHellId);
        var sourceDemonArcher = ids.Any(id => id is StandardShanheSourceDemonArcherPackage.BaseId or
            StandardShanheSourceDemonArcherPackage.HardId or StandardShanheSourceDemonArcherPackage.NightmareId or
            StandardShanheSourceDemonArcherPackage.TrioBaseId or StandardShanheSourceDemonArcherPackage.TrioHardId or
            StandardShanheSourceDemonArcherPackage.TrioNightmareId);
        var sourceVanguard = ids.Any(id => id is StandardShanheSourceVanguardPackage.BaseId or
            StandardShanheSourceVanguardPackage.HardId or StandardShanheSourceVanguardPackage.NightmareId);
        var sourceMaRiDi = sourceCaoSongTrio || extraSkills.Contains(StandardShanheSourceMaRiDiPackage.WeizhongSkillId) ||
            ids.Any(id => id is StandardShanheSourceMaRiDiPackage.BaseId or StandardShanheSourceMaRiDiPackage.HardId or
                StandardShanheSourceMaRiDiPackage.NightmareId or StandardShanheSourceMaRiDiPackage.HellId);
        var sourceZhangHe = ids.Any(id => id is StandardShanheSourceZhangHePackage.BaseId or StandardShanheSourceZhangHePackage.WeijingId);
        var sourceWarElephant = ids.Any(id => id is StandardShanheSourceWarElephantPackage.BaseId or
            StandardShanheSourceWarElephantPackage.HardId or StandardShanheSourceWarElephantPackage.HellId);
        var sourceCaiWenji = ids.Any(id => id is StandardShanheSourceCaiWenjiPackage.BaseId or
            StandardShanheSourceCaiWenjiPackage.HardId or StandardShanheSourceCaiWenjiPackage.NightmareId);
        var sourceRanger = ids.Any(id => id is StandardShanheSourceRangerPackage.BaseId or
            StandardShanheSourceRangerPackage.HardId or StandardShanheSourceRangerPackage.NightmareId or StandardShanheSourceRangerPackage.HellId);
        s5Dongwu |= sourceRanger;
        var sourceYouyan = ids.Any(id => id is StandardShanheSourceYouyanWarriorPackage.BaseId or StandardShanheSourceYouyanWarriorPackage.HardId);
        var sourceGaoGan = ids.Any(id => id is StandardShanheSourceGaoGanPackage.BaseId or
            StandardShanheSourceGaoGanPackage.HardId or StandardShanheSourceGaoGanPackage.HellId);
        var sourceArmory = ids.Any(id => id is StandardShanheSourceArmoryCommanderPackage.BaseId or
            StandardShanheSourceArmoryCommanderPackage.HardId or StandardShanheSourceArmoryCommanderPackage.NightmareId);
        var sourceChengZhan = ids.Any(id => id is StandardShanheSourceChengZhanPackage.BaseId or
            StandardShanheSourceChengZhanPackage.HardId or StandardShanheSourceChengZhanPackage.HellId);
        var sourceZuoCi = ids.Any(id => id is StandardShanheSourceZuoCiPackage.BaseId or
            StandardShanheSourceZuoCiPackage.HardId or StandardShanheSourceZuoCiPackage.NightmareId);
        var sourceLiuSandao = ids.Any(id => id is StandardShanheSourceLiuSandaoPackage.BaseId or
            StandardShanheSourceLiuSandaoPackage.MediumId or StandardShanheSourceLiuSandaoPackage.HardId or
            StandardShanheSourceLiuSandaoPackage.NightmareId or StandardShanheSourceLiuSandaoPackage.HellId);
        var sourceJiangdongTrio = ids.Any(id => id.StartsWith("shanhe:source-jiangdong-trio-2492-", StringComparison.Ordinal));
        var sourceCoalitionTrio = extraSkills.Contains(StandardShanheSourceCoalitionTrioPackage.CangchuSkillId) ||
            extraSkills.Contains(StandardShanheSourceCoalitionTrioPackage.SushouSkillId) ||
            ids.Any(id => id is StandardShanheSourceCoalitionTrioPackage.SpyBaseId or StandardShanheSourceCoalitionTrioPackage.SpyHardId or
                StandardShanheSourceCoalitionTrioPackage.SpyNightmareId or StandardShanheSourceCoalitionTrioPackage.ScoutBaseId or
                StandardShanheSourceCoalitionTrioPackage.ScoutHardId or StandardShanheSourceCoalitionTrioPackage.ScoutHellId or
                StandardShanheSourceCoalitionTrioPackage.QuartermasterBaseId or StandardShanheSourceCoalitionTrioPackage.QuartermasterHardId);
        var sourceWuqueAssassin = sourceDemonAssassin || RequiresThreeBossRegistry(requirements, StandardShanheSourceWuqueAssassinPackage.BaseId,
            StandardShanheSourceWuqueAssassinPackage.YiduSkillId) ||
            ids.Any(id => id is StandardShanheSourceWuqueAssassinPackage.HardId or StandardShanheSourceWuqueAssassinPackage.HellId);
        var sourceExecutioner = RequiresThreeBossRegistry(requirements, StandardShanheSourceExecutionerPackage.BaseId,
            StandardShanheSourceExecutionerPackage.YishiSkillId, StandardShanheSourceExecutionerPackage.HuanfuSkillId) ||
            ids.Any(id => id is StandardShanheSourceExecutionerPackage.MediumId or StandardShanheSourceExecutionerPackage.HardId);
        var sourceZhangYan = sourceQiangWang || RequiresThreeBossRegistry(requirements,
            StandardShanheSourceZhangYanPackage.ZhangniujiaoMediumId,
            StandardShanheSourceZhangYanPackage.YanyuSkillId,
            StandardShanheSourceZhangYanPackage.BiriSkillId,
            StandardShanheSourceZhangYanPackage.DongdangSkillId) ||
            ids.Any(id => id is StandardShanheSourceZhangYanPackage.ZhangYanBaseId or
                StandardShanheSourceZhangYanPackage.ZhangYanMediumId or
                StandardShanheSourceZhangYanPackage.ZhangniujiaoHardId or
                StandardShanheSourceZhangYanPackage.ZhangniujiaoNightmareId or
                StandardShanheSourceZhangYanPackage.ZhangYanHardId);
        var s5Xuncan = sourceZhangYan || RequiresThreeBossRegistry(requirements, StandardShanheS5XuncanCaoshiPackage.XuncanId,
            StandardShanheS5XuncanCaoshiPackage.YunshenId, StandardShanheS5XuncanCaoshiPackage.ShangshenId,
            StandardShanheS5XuncanCaoshiPackage.FenchaiId, StandardShanheS5XuncanCaoshiPackage.LeijieId) ||
            ids.Contains(StandardShanheS5XuncanCaoshiPackage.CaoshiId, StringComparer.Ordinal);
        var s5Zaiqi = RequiresS5ZaiqiRegistry(requirements);
        var key = $"{setup.AllyRoster.Count}/{setup.EnemyRoster.Count}/" + string.Join("|", ids) +
            (guideSkills ? "/guide-skills" : "") + (xiasheng ? "/xiasheng-3362" : "") +
            (chanrangEdict ? "/chanrang-edict-1510" : "") +
            (setup.TerrainSkillId is { } terrain ? "/" + terrain : "") +
            (s5MaPair ? "/s5-ma-pair-2640" : "") + (s5XuRong ? "/s5-xurong-2641" : "") +
            (s5XiliangDark ? "/s5-xiliang-dark-2663" : "") +
            (codexEquipment ? "/codex-equipment" : "") + (currentFarStrike ? "/current-yuanji-2129" : "") +
            (currentJinnangJi ? "/current-jinnangji-2011" : "") +
            (currentDuoduoYishan ? "/current-duoduo-yishan" : "") +
            (currentYaoli ? "/current-yaoli" : "") +
            (currentLegendaryTactics ? "/current-legendary-tactics" : "") +
            (buildTactics ? "/current-build-tactics" : "") +
            (reactionTactics ? "/current-reaction-tactics" : "") +
            (thirdSlashTactics ? "/current-third-slash-tactics" : "") +
            (miaoshouTactic ? "/current-miaoshou" : "") +
            (lateRoundTactics ? "/current-late-round-tactics" : "") +
            (teamHealthTactic ? "/current-team-health" : "") +
            (wineTactics ? "/current-wine-tactics" : "") +
            (zuiquanTactic ? "/current-zuiquan-2001" : "") +
            (guandaoTactic ? "/current-guandao-2010" : "") +
            (lowHpDrawTactics ? "/current-low-hp-draw" : "") +
            (ershengsanTactic ? "/current-ershengsan-2030" : "") +
            (heartBladeTactic ? "/current-heart-blade-2045" : "") +
            (dualBladeTactics ? "/current-dual-blade" : "") +
            (handLimitStock ? "/current-hand-limit-stock" : "") +
            (doubleStealTactic ? "/current-double-steal-2337" : "") +
            (doubleResponseTactics ? "/current-double-response" : "") +
            (trickResponseTactics ? "/current-trick-response" : "") +
            (qianlongTactic ? "/current-qianlong" : "") +
            (multiTargetTrickTactics ? "/current-multi-target-tricks" : "") +
            (xuyanTactic ? "/current-xuyan" : "") +
            (alliedTrickTactics ? "/current-allied-tricks" : "") +
            (yongzhanTactics ? "/current-yongzhan" : "") +
            (qiaoquHaoduoTactic ? "/current-qiaoqu-haoduo" : "") +
            (targetHandVisibility ? "/current-target-hand-visibility" : "") +
            (dismantlementStock ? "/current-dismantlement-stock" : "") +
            (badaoTactics ? "/current-badao" : "") +
            (steadyHandTactic ? "/current-steady-hand" : "") +
            (counterspellClaimTactic ? "/current-counterspell-claim" : "") +
            (mobilizationTactic ? "/current-mobilization" : "") +
            (wealthTactics ? "/current-wealth-tactics" : "") +
            (currentFuqi ? "/current-fuqi-3100" : "") +
            (currentWangong ? "/current-wangong-3263" : "") +
            (currentLonghun ? "/current-longhun-3023" : "") +
            (currentYiji ? "/current-yiji-3081" : "") +
            (currentCappedRefill ? "/current-jieming-3083" : "") +
            (currentBeige ? "/current-beige-3140" : "") +
            (fengxianBoss ? "/fengxian-two-stage" : "") +
            (confidentLvbu ? "/confident-lvbu" : "") +
            (eliteDongwu ? "/elite-dongwu" : "") + (eliteHandang ? "/elite-handang" : "") +
            (recruitment ? "/recruitment-huihuo" : "") +
            (caosongCompanion ? "/caosong-companion" : "") +
            (caoruiCompanion ? "/caorui-companion" : "") +
            (wuguotaiCompanion ? "/wuguotai-companion" : "") +
            (guanpingCompanion ? "/guanping-companion" : "") +
            (chilianCompanion ? "/chilian-companion" : "") +
            (currentDingcuo ? "/current-dingcuo-3291" : "") +
            (currentGuanchao ? "/current-guanchao-3076" : "") +
            (localTroops ? "/local-troops" : "") +
            (currentSlashTactics ? "/current-slash-tactics" : "") +
            (s5Chengxiong ? "/s5-chengxiong-14055" : "") +
            (s5Shidu ? "/s5-shidu-7003" : "") +
            (s5CaoxingHigh ? "/s5-shihun-14071" : "") +
            (s5Huaxiong ? "/s5-huaxiong-22647" : "") +
            (s5Taishici ? "/s5-taishici-2632" : "") +
            (s5Caocao ? "/s5-caocao-2649" : "") +
            (s5Yujin ? "/s5-yujin-2648" : "") +
            (s5Yudu ? "/s5-yudu-2633" : "") +
            (s5Liru ? "/s5-liru-2668" : "") +
            (s5Yuanshao ? "/s5-yuanshao-2660" : "") +
            (s5Zhangliang ? "/s5-zhangliang-2656" : "") +
            (s5Xuchu ? "/s5-xuchu-2676" : "") +
            (s5Zhuling ? "/s5-zhuling-2664" : "") +
            (s5YanliangWenchou ? "/s5-yanliang-wenchou-2665" : "") +
            (s5Quyi ? "/s5-quyi-2666" : "") +
            (s5Zhanghe ? "/s5-zhanghe-2673" : "") +
            (s5Yuanshu ? "/s5-yuanshu-2654" : "") +
            (s5Gongsun ? "/s5-gongsun-2662" : "") +
            (s5Taoyuan ? "/s5-taoyuan-2683" : "") +
            (s5Lijue ? "/s5-lijue-2636" : "") +
            (s5Guosi ? "/s5-guosi-2637" : "") +
            (s5MaSiblings ? "/s5-ma-siblings-2642" : "") +
            (s5Dongwu ? "/s5-dongwu-2659" : "") +
            (s5Moushi ? "/s5-moushi-2674" : "") +
            (s5HanEmperors ? "/s5-han-emperors-2643" : "") +
            (s5Xuncan ? "/s5-xuncan-caoshi-2657" : "") +
            (sourceZhangYan ? "/source-zhangyan-265301" : "") +
            (sourceShenNu ? "/source-shennu-268701" : "") +
            (sourceQiangWang ? "/source-qiangwang-2669" : "") +
            (blackLightArmor ? "/black-light-armor-6011" : "") +
            (gouXiangArmor ? "/gouxiang-armor-14088" : "") +
            (duanhun ? "/duanhun-14079" : "") +
            (kunlunMirror ? "/kunlun-mirror-14080" : "") +
            (zhaoguMirror ? "/zhaogu-mirror-6018" : "") +
            (huxinMirror ? "/huxin-mirror-2055" : "") +
            (sourceXuSheng ? "/source-xusheng-2635" : "") +
            (sourceSunCe ? "/source-sunce-2655" : "") +
            (sourceXunYu ? "/source-xunyu-2675" : "") +
            (sourceMouDongZhuo ? "/source-mou-dongzhuo-2684" : "") +
            (sourceVeteran ? "/source-veteran-2474" : "") +
            (sourceWuhuanRider ? "/source-wuhuan-rider-2467" : "") +
            (sourceWarrior ? "/source-warrior-2487" : "") +
            (sourceQiangRaider ? "/source-qiang-raider-2473" : "") +
            (sourceXunYue ? "/source-xunyue-2480" : "") +
            (sourceJiangdongVeteran ? "/source-jiangdong-veteran-2497" : "") +
            (sourceCoalitionScout ? "/source-coalition-scout-2455" : "") +
            (sourceCoalitionSpy ? "/source-coalition-spy-2459" : "") +
            (sourceLocalMagnate ? "/source-local-magnate-2460" : "") +
            (sourceLinZhongGu ? "/source-linzhonggu-2476" : "") +
            (sourceXinPing ? "/source-xinping-2489" : "") +
            (sourceDemonAssassin ? "/source-demon-assassin-2457" : "") +
            (sourceBuHeng ? "/source-buheng-2451" : "") +
            (sourceYulinGuard ? "/source-yulin-guard-2498" : "") +
            (sourceAgricultureOfficer ? "/source-agriculture-officer-2500" : "") +
            (sourceKeshou ? "/source-keshou-993" : "") +
            (sourceYulinOfficer ? "/source-yulin-officer-2499" : "") +
            (sourceCoalitionTrio ? "/source-coalition-trio-2465" : "") +
            (sourceWhiteHorse ? "/source-white-horse-2454" : "") +
            (sourceDemonRaider ? "/source-demon-raider-2458" : "") +
            (sourceDemonArcher ? "/source-demon-archer-2461" : "") +
            (sourceVanguard ? "/source-vanguard-2494" : "") +
            (sourceMaRiDi ? "/source-maridi-2452" : "") +
            (sourceZhangHe ? "/source-zhanghe-2468" : "") +
            (sourceWarElephant ? "/source-war-elephant-2453" : "") +
            (sourceCaiWenji ? "/source-caiwenji-2469" : "") +
            (sourceRanger ? "/source-ranger-2481" : "") +
            (sourceYouyan ? "/source-youyan-warrior-2466" : "") +
            (sourceGaoGan ? "/source-gaogan-2484" : "") +
            (sourceArmory ? "/source-armory-commander-2486" : "") +
            (sourceChengZhan ? "/source-chengzhan-2462" : "") +
            (sourceZuoCi ? "/source-zuoci-2470" : "") +
            (sourceLiuSandao ? "/source-liusandao-2479" : "") +
            (sourceJiangdongTrio ? "/source-jiangdong-trio-2492" : "") +
            (sourceMilitiaTrio ? "/source-militia-trio-2475" : "") +
            (sourceCaoSongTrio ? "/source-caosong-trio-2485" : "") +
            (sourceWuCong ? "/source-wucong-2478" : "") +
            (sourceQianZhaoPair ? "/source-qianzhao-pair-2471" : "") +
            (sourceWuqueAssassin ? "/source-wuque-assassin-2493" : "") +
            (sourceExecutioner ? "/source-executioner-2495" : "") +
            (s5Simayi ? "/s5-simayi-2677" : "") +
            (s5Dianwei ? "/s5-dianwei-weizi-2650" : "") +
            (s5Xiahou ? "/s5-xiahou-pair-2651" : "") +
            (s5Zhangxiu ? "/s5-zhangxiu-2638" : "") +
            (s5Zuoci ? "/s5-zuoci-huatuo-2682" : "") +
            (s5CaoWei ? "/s5-cao-wei-2679" : "") +
            (s5Xingtian ? "/s5-xingtian-20766" : "") +
            (sourceSanshou ? "/source-sanshou-20776-20777" : "") +
            (s5Zhangliao ? "/s5-zhangliao-722" : "") +
            (s5Zaiqi ? "/s5-zaiqi-887" : "") +
            (setup.RebirthAvailable ? "/rank-five-rebirth" : "") +
            (schoolEffects ? "/school-effects" : "") +
            (setup.MarketSetup is { } marketCache ? "/market-" + ShanheCampaignContent.GetContentHash(marketCache.Definition) : "");
        return BattleRegistries.GetOrAdd(key, _ => new Lazy<ContentRegistry>(() =>
        {
            var packages = new List<IGameContentPackage> { new StandardShanheBossPackage() };
            if (schoolEffects)
            {
                packages.Add(new StandardShanheSchoolBattlePackage());
                packages.Add(new StandardShanheSchoolDrawPackage());
                packages.Add(new StandardShanheSchoolVitalPackage());
                packages.Add(new StandardShanheSchoolTrickPackage());
                packages.Add(new StandardShanheSchoolLatePackage());
                packages.Add(new StandardShanheSchoolTeamPackage());
            }
            if (guideSkills) packages.Add(new StandardShanheGuideSkillsPackage());
            if (xiasheng) packages.Add(new StandardShanheXiashengPackage());
            if (chanrangEdict) packages.Add(new StandardShanheChanrangEdictPackage());
            if (ids.Any(StandardShanheMuluPackage.GeneralIds.Contains)) packages.Add(new StandardShanheMuluPackage());
            if (setup.TerrainSkillId is not null) packages.Add(new StandardShanheTerrainPackage());
            if (ids.Contains(StandardShanheGujinPackage.GujinId, StringComparer.Ordinal))
                packages.Add(new StandardShanheGujinPackage());
            if (ids.Contains(StandardShanheCompanionPackage.ManzhuId, StringComparer.Ordinal))
                packages.Add(new StandardShanheCompanionPackage());
            if (ids.Any(StandardShanheExpandedCompanionPackage.Contains))
                packages.Add(new StandardShanheExpandedCompanionPackage());
            if (ids.Any(StandardShanheAdditionalRecruitmentAPackage.Contains))
                packages.Add(new StandardShanheAdditionalRecruitmentAPackage());
            if (ids.Any(StandardShanheAdditionalRecruitmentBPackage.GeneralIds.Contains) ||
                extraSkills.Contains(StandardShanheAdditionalRecruitmentBPackage.DuanziSkillId) ||
                extraSkills.Contains(StandardShanheAdditionalRecruitmentBPackage.ShunxueSkillId))
                packages.Add(new StandardShanheAdditionalRecruitmentBPackage());
            if (ids.Any(id => id is StandardShanheOpeningEncounterPackage.ZhangheId or StandardShanheOpeningEncounterPackage.TadunId))
                packages.Add(new StandardShanheOpeningEncounterPackage());
            if (setup.MarketSetup is not null) packages.Add(new StandardShanheMarketPackage());
            if (currentDingcuo) packages.Add(new StandardShanheDingcuoPackage());
            if (currentGuanchao) packages.Add(new StandardShanheGuanchaoPackage());
            if (localTroops) packages.Add(new StandardShanheLocalTroopPackage());
            if (currentLegendaryTactics) packages.Add(new StandardShanheLegendaryTacticsPackage());
            if (buildTactics) packages.Add(new StandardShanheBuildTacticsPackage());
            if (thirdSlashTactics) packages.Add(new StandardShanheThirdSlashPackage());
            if (miaoshouTactic) packages.Add(new StandardShanheMiaoshouPackage());
            if (lateRoundTactics) packages.Add(new StandardShanheLateRoundTacticsPackage());
            if (teamHealthTactic) packages.Add(new StandardShanheTeamHealthPackage());
            if (wineTactics) packages.Add(new StandardShanheWineTacticsPackage());
            if (zuiquanTactic) packages.Add(new StandardShanheZuiquanPackage());
            if (guandaoTactic) packages.Add(new StandardShanheGuandaoPackage());
            if (lowHpDrawTactics) packages.Add(new StandardShanheLowHpDrawPackage());
            if (ershengsanTactic) packages.Add(new StandardShanheErshengsanPackage());
            if (heartBladeTactic) packages.Add(new StandardShanheHeartBladePackage());
            if (dualBladeTactics) packages.Add(new StandardShanheDualBladePackage());
            if (handLimitStock) packages.Add(new StandardShanheHandLimitStockPackage());
            if (doubleStealTactic) packages.Add(new StandardShanheDoubleStealPackage());
            if (doubleResponseTactics) packages.Add(new StandardShanheDoubleResponsePackage());
            if (trickResponseTactics) packages.Add(new StandardShanheTrickResponsePackage());
            if (qianlongTactic) packages.Add(new StandardShanheQianlongPackage());
            if (multiTargetTrickTactics) packages.Add(new StandardShanheMultiTargetTricksPackage());
            if (xuyanTactic) packages.Add(new StandardShanheXuyanPackage());
            if (alliedTrickTactics) packages.Add(new StandardShanheAlliedTricksPackage());
            if (yongzhanTactics) packages.Add(new StandardShanheYongzhanPackage());
            if (qiaoquHaoduoTactic) packages.Add(new StandardShanheQiaoquHaoduoPackage());
            if (targetHandVisibility) packages.Add(new StandardShanheTargetHandVisibilityPackage());
            if (dismantlementStock) packages.Add(new StandardShanheDismantlementStockPackage());
            if (badaoTactics) packages.Add(new StandardShanheBadaoPackage());
            if (steadyHandTactic) packages.Add(new StandardShanheSteadyHandPackage());
            if (counterspellClaimTactic) packages.Add(new StandardShanheCounterspellClaimPackage());
            if (mobilizationTactic) packages.Add(new StandardShanheMobilizationPackage());
            if (reactionTactics)
            {
                packages.Add(new StandardShanheArmorTacticsPackage());
                packages.Add(new StandardShanheDamageDrawTacticsPackage());
                packages.Add(new StandardShanheSustainTacticsPackage());
                packages.Add(new StandardShanheDirectSkillDamagePackage());
                packages.Add(new StandardShanheArmorBreakTacticsPackage());
                packages.Add(new StandardShanheWoxinPackage());
            }
            if (currentFuqi) packages.Add(new StandardShanheFuqiPackage());
            if (currentWangong) packages.Add(new StandardShanheWangongPackage());
            if (currentLonghun) packages.Add(new StandardShanheLonghunPackage());
            if (currentYiji) packages.Add(new StandardShanheYijiPackage());
            if (currentCappedRefill) packages.Add(new StandardShanheCappedRefillSkillsPackage());
            if (currentBeige) packages.Add(new StandardShanheBeigeSkillsPackage());
            if (fengxianBoss) packages.Add(new StandardShanheFengxianBossPackage());
            if (confidentLvbu) packages.Add(new StandardShanheConfidentLvbuPackage());
            if (eliteDongwu) packages.Add(new StandardShanheEliteDongwuPackage(importBaoli: !s5Zhuling));
            if (eliteHandang) packages.Add(new StandardShanheEliteHandangPackage());
            if (recruitment) packages.Add(new StandardShanheRecruitmentPackage());
            if (caosongCompanion) packages.Add(new StandardShanheCaosongCompanionPackage());
            if (caoruiCompanion) packages.Add(new StandardShanheCaoruiCompanionPackage());
            if (wuguotaiCompanion) packages.Add(new StandardShanheWuguotaiCompanionPackage());
            if (guanpingCompanion) packages.Add(new StandardShanheGuanpingCompanionPackage());
            if (chilianCompanion) packages.Add(new StandardShanheChilianCompanionPackage());
            if (s5Zhangliao || ids.Any(id => id == StandardShanheS5BossPackage.ZhangliaoBaseId ||
                id == StandardShanheS5BossPackage.ZhangliaoLingdongId || id == StandardShanheS5BossPackage.ZhangliaoHuangkongId ||
                id == StandardShanheS5BossPackage.DongZhuoLuoyangId || id == StandardShanheS5BossPackage.DongMinBaseId ||
                id == StandardShanheS5BossPackage.DongMinCuanchaoId || id == StandardShanheS5BossPackage.LvbuBajiId ||
                id == StandardShanheS5BossPackage.DiaoChanBiyueId || id == StandardShanheS5BossPackage.DongZhuoFengyiId ||
                id == StandardShanheS5GaichaoPackage.DongZhuoGaichaoId ||
                id == StandardShanheS5DongshiPackage.DongZhuoBaxinId || id == StandardShanheS5DongshiPackage.DongJiQiaoxiaoId))
                packages.Add(new StandardShanheS5BossPackage());
            if (RequiresFengyiBossRegistry(requirements)) packages.Add(new StandardShanheS5FengyiPackage());
            if (RequiresGaichaoBossRegistry(requirements)) packages.Add(new StandardShanheS5GaichaoPackage());
            if (RequiresDongshiBossRegistry(requirements)) packages.Add(new StandardShanheS5DongshiPackage());
            if (RequiresGaoshunBossRegistry(requirements)) packages.Add(new StandardShanheS5GaoshunPackage());
            if (RequiresCaoxingBossRegistry(requirements)) packages.Add(new StandardShanheS5CaoxingPackage());
            if (s5Shidu) packages.Add(new StandardShanheS5ShiduPackage());
            if (s5CaoxingHigh) packages.Add(new StandardShanheS5CaoxingHighPackage());
            if (s5Huaxiong) packages.Add(new StandardShanheS5HuaxiongPackage());
            if (s5Taishici) packages.Add(new StandardShanheS5TaishiciPackage());
            if (s5Caocao) packages.Add(new StandardShanheS5CaocaoPackage());
            if (s5Yujin) packages.Add(new StandardShanheS5YujinPackage());
            if (s5Yudu) packages.Add(new StandardShanheS5YuduPackage());
            if (s5Liru) packages.Add(new StandardShanheS5LiruPackage());
            if (s5Yuanshao) packages.Add(new StandardShanheS5YuanshaoPackage());
            if (s5Zhangliang) packages.Add(new StandardShanheS5ZhangliangPackage());
            if (s5Xuchu) packages.Add(new StandardShanheS5XuchuPackage());
            if (s5Zhuling) packages.Add(new StandardShanheS5ZhulingPackage());
            if (s5YanliangWenchou) packages.Add(new StandardShanheS5YanliangWenchouPackage());
            if (s5Quyi) packages.Add(new StandardShanheS5QuyiPackage());
            if (s5Zhanghe) packages.Add(new StandardShanheS5ZhanghePackage());
            if (s5Yuanshu) packages.Add(new StandardShanheS5YuanshuPackage());
            if (s5Gongsun) packages.Add(new StandardShanheS5GongsunZanPackage());
            if (s5Taoyuan) packages.Add(new StandardShanheS5TaoyuanPackage());
            if (s5Lijue) packages.Add(new StandardShanheS5LiJuePackage());
            if (s5Guosi) packages.Add(new StandardShanheS5GuosiPackage());
            if (s5MaSiblings) packages.Add(new StandardShanheS5MaSiblingsPackage());
            if (s5MaPair) packages.Add(new StandardShanheS5MaPairPackage());
            if (s5XuRong) packages.Add(new StandardShanheS5XuRongPackage());
            if (s5XiliangDark) packages.Add(new StandardShanheS5XiliangDarkPackage());
            if (s5Simayi) packages.Add(new StandardShanheS5SimayiPackage());
            if (s5Dianwei) packages.Add(new StandardShanheS5DianweiWeiziPackage());
            if (s5Xiahou) packages.Add(new StandardShanheS5XiahouPairPackage());
            if (s5Zhangxiu) packages.Add(new StandardShanheS5ZhangxiuPackage());
            if (s5Zuoci) packages.Add(new StandardShanheS5ZuociHuatuoPackage());
            if (s5CaoWei) packages.Add(new StandardShanheS5CaoWeiPioneersPackage());
            if (s5Xingtian) packages.Add(new StandardShanheXingtianPackage());
            if (sourceSanshou) packages.Add(new StandardShanheSanshouArmorPackage());
            if (blackLightArmor) packages.Add(new StandardShanheBlackLightArmorPackage());
            if (gouXiangArmor) packages.Add(new StandardShanheGouXiangArmorPackage());
            if (duanhun) packages.Add(new StandardShanheDuanhunPackage());
            if (kunlunMirror) packages.Add(new StandardShanheKunlunMirrorPackage());
            if (zhaoguMirror) packages.Add(new StandardShanheZhaoguMirrorPackage());
            if (huxinMirror) packages.Add(new StandardShanheHuxinMirrorPackage());
            if (ids.Contains(StandardShanheS5ZhangniujiaoPackage.ZhangniujiaoId, StringComparer.Ordinal))
                packages.Add(new StandardShanheS5ZhangniujiaoPackage());
            if (ids.Any(id => id is StandardShanheS5LvBuBasePackage.LvBuFiveId or StandardShanheS5LvBuBasePackage.LvBuFourId))
                packages.Add(new StandardShanheS5LvBuBasePackage());
            if (s5Dongwu) packages.Add(new StandardShanheS5DongwuVeteransPackage());
            if (s5Moushi) packages.Add(new StandardShanheS5MoushiPackage());
            if (s5HanEmperors) packages.Add(new StandardShanheS5HanEmperorsPackage());
            if (s5Xuncan) packages.Add(new StandardShanheS5XuncanCaoshiPackage());
            if (sourceZhangYan) packages.Add(new StandardShanheSourceZhangYanPackage());
            if (sourceShenNu) packages.Add(new StandardShanheSourceShenNuStagePackage());
            if (sourceQiangWang) packages.Add(new StandardShanheSourceQiangWangPackage());
            if (sourceXuSheng) packages.Add(new StandardShanheSourceXuShengPackage());
            if (sourceSunCe) packages.Add(new StandardShanheSourceSunCePackage());
            if (sourceXunYu) packages.Add(new StandardShanheSourceXunYuPackage());
            if (sourceMouDongZhuo) packages.Add(new StandardShanheSourceMouDongZhuoPackage());
            if (sourceVeteran) packages.Add(new StandardShanheSourceVeteranPackage());
            if (sourceWuhuanRider) packages.Add(new StandardShanheSourceWuhuanRiderPackage());
            if (sourceWarrior) packages.Add(new StandardShanheSourceWarriorPackage());
            if (sourceQiangRaider) packages.Add(new StandardShanheSourceQiangRaiderPackage());
            if (sourceXunYue) packages.Add(new StandardShanheSourceXunYuePackage());
            if (sourceJiangdongVeteran) packages.Add(new StandardShanheSourceJiangdongVeteranPackage());
            if (sourceCoalitionScout) packages.Add(new StandardShanheSourceCoalitionScoutPackage());
            if (sourceCoalitionSpy) packages.Add(new StandardShanheSourceCoalitionSpyPackage());
            if (sourceLocalMagnate) packages.Add(new StandardShanheSourceLocalMagnatePackage());
            if (sourceLinZhongGu) packages.Add(new StandardShanheSourceLinZhongGuPackage());
            if (sourceXinPing) packages.Add(new StandardShanheSourceXinPingPackage());
            if (sourceKeshou) packages.Add(new StandardShanheSourceKeshouPackage());
            if (sourceBuHeng) packages.Add(new StandardShanheSourceBuHengPackage());
            if (sourceYulinGuard) packages.Add(new StandardShanheSourceYulinGuardPackage());
            if (sourceYulinOfficer) packages.Add(new StandardShanheSourceYulinOfficerPackage());
            if (sourceCoalitionTrio) packages.Add(new StandardShanheSourceCoalitionTrioPackage());
            if (sourceWhiteHorse) packages.Add(new StandardShanheSourceWhiteHorsePackage());
            if (sourceDemonRaider) packages.Add(new StandardShanheSourceDemonRaiderPackage());
            if (sourceDemonArcher) packages.Add(new StandardShanheSourceDemonArcherPackage());
            if (sourceVanguard) packages.Add(new StandardShanheSourceVanguardPackage());
            if (sourceMaRiDi) packages.Add(new StandardShanheSourceMaRiDiPackage());
            if (sourceZhangHe) packages.Add(new StandardShanheSourceZhangHePackage());
            if (sourceWarElephant) packages.Add(new StandardShanheSourceWarElephantPackage());
            if (sourceCaiWenji) packages.Add(new StandardShanheSourceCaiWenjiPackage());
            if (sourceRanger) packages.Add(new StandardShanheSourceRangerPackage());
            if (sourceYouyan) packages.Add(new StandardShanheSourceYouyanWarriorPackage());
            if (sourceGaoGan) packages.Add(new StandardShanheSourceGaoGanPackage());
            if (sourceArmory) packages.Add(new StandardShanheSourceArmoryCommanderPackage());
            if (sourceChengZhan) packages.Add(new StandardShanheSourceChengZhanPackage());
            if (sourceZuoCi) packages.Add(new StandardShanheSourceZuoCiPackage());
            if (sourceLiuSandao) packages.Add(new StandardShanheSourceLiuSandaoPackage());
            if (sourceJiangdongTrio) packages.Add(new StandardShanheSourceJiangdongTrioPackage());
            if (sourceMilitiaTrio) packages.Add(new StandardShanheSourceMilitiaTrioPackage());
            if (sourceCaoSongTrio) packages.Add(new StandardShanheSourceCaoSongTrioPackage());
            if (sourceWuCong) packages.Add(new StandardShanheSourceWuCongPackage());
            if (sourceQianZhaoPair) packages.Add(new StandardShanheSourceQianZhaoPairPackage());
            if (sourceAgricultureOfficer) packages.Add(new StandardShanheSourceAgricultureOfficerPackage());
            if (sourceWuqueAssassin) packages.Add(new StandardShanheSourceWuqueAssassinPackage());
            if (sourceDemonAssassin) packages.Add(new StandardShanheSourceDemonAssassinPackage());
            if (sourceExecutioner) packages.Add(new StandardShanheSourceExecutionerPackage());
            if (ids.Any(id => id is StandardShanheS5SunJianWuguotaiPackage.SunjianId or StandardShanheS5SunJianWuguotaiPackage.WuguotaiId))
                packages.Add(new StandardShanheS5SunJianWuguotaiPackage());
            if (ids.Contains(StandardShanheS5DongZhuoBasePackage.DongzhuoId, StringComparer.Ordinal))
                packages.Add(new StandardShanheS5DongZhuoBasePackage());
            // These forms reuse existing skills. The roster, already present
            // in the cache key, is their only additional package requirement.
            if (ids.Contains(StandardShanheS5HuaxiongBasePackage.HuaxiongId, StringComparer.Ordinal))
                packages.Add(new StandardShanheS5HuaxiongBasePackage());
            if (ids.Contains(StandardShanheS5LukuangLuxiangPackage.LukuangLuxiangId, StringComparer.Ordinal))
                packages.Add(new StandardShanheS5LukuangLuxiangPackage());
            if (ids.Contains(StandardShanheS5ChenlinPackage.ChenlinId, StringComparer.Ordinal))
                packages.Add(new StandardShanheS5ChenlinPackage());
            if (ids.Contains(StandardShanheS5ZhouyuPackage.ZhouyuId, StringComparer.Ordinal))
                packages.Add(new StandardShanheS5ZhouyuPackage());
            if (ids.Contains(StandardShanheS5XuyouJushouPackage.XuyouId, StringComparer.Ordinal) ||
                ids.Contains(StandardShanheS5XuyouJushouPackage.JushouId, StringComparer.Ordinal))
                packages.Add(new StandardShanheS5XuyouJushouPackage());
            if (ids.Contains(StandardShanheS5CaorenZhaoyunPackage.CaorenId, StringComparer.Ordinal) ||
                ids.Contains(StandardShanheS5CaorenZhaoyunPackage.ZhaoyunId, StringComparer.Ordinal))
                packages.Add(new StandardShanheS5CaorenZhaoyunPackage());
            if (ids.Contains(StandardShanheS5XujingXuqiuPackage.XujingId, StringComparer.Ordinal) ||
                ids.Contains(StandardShanheS5XujingXuqiuPackage.XuqiuId, StringComparer.Ordinal))
                packages.Add(new StandardShanheS5XujingXuqiuPackage());
            if (s5Chengxiong) packages.Add(new StandardShanheS5ChengxiongPackage());
            if (setup.RebirthAvailable) packages.Add(new StandardShanheRankPrivilegePackage());
            packages.Add(new ShanheEncounterPackage(setup, requirements));
            if (codexEquipment) packages.Add(new ShanheCodexEquipmentPackage());
            if (wealthTactics) packages.Add(new StandardShanheWealthPackage());
            if (reactionTactics)
            {
                packages.Add(new StandardShanheDrawTacticsPackage());
                packages.Add(new StandardShanhePreparationTacticsPackage());
                packages.Add(new StandardShanheLogisticsTacticsPackage());
                packages.Add(new StandardShanheBossOpeningTacticsPackage());
                packages.Add(new StandardShanheCycleTacticsPackage());
                packages.Add(new StandardShanheSustainOpeningTacticsPackage());
                packages.Add(new StandardShanheHandStockTacticsPackage());
                packages.Add(new StandardShanheResponseTacticsPackage());
                packages.Add(new StandardShanheFuyiTacticsPackage());
                packages.Add(new StandardShanheBowenTacticsPackage());
                packages.Add(new StandardShanheRecastTacticsPackage());
                packages.Add(new StandardShanheAppliedDamageReflectionPackage());
                packages.Add(new StandardShanheDrawThresholdPackage());
                packages.Add(new StandardShanheLowHpDamagePackage());
                packages.Add(new StandardShanheRoundRecoveryPackage());
                packages.Add(new StandardShanheIncomingReductionPackage());
            }
            return ComposedSkillContentRegistry.CreateShowcase(includeHulaoActivities: sourceLiuSandao, additionalPackages: packages);
        })).Value;
    }

    private static bool RequiresCurrentFarStrikeRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(CurrentFarStrikeSkillId, StringComparer.Ordinal);

    private static bool RequiresS5ZaiqiRegistry(ShanheBattleSetup setup) =>
        RequiresFengyiBossRegistry(setup) || RequiresSourceVeteranRegistry(setup) ||
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(S5ZaiqiSkillId, StringComparer.Ordinal);

    private static bool RequiresSourceVeteranRegistry(ShanheBattleSetup setup) => new[] { setup.PlayerGeneralId }
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).Select(member => member.GeneralId))
            .Any(id => id is StandardShanheSourceVeteranPackage.BaseGeneralId or StandardShanheSourceVeteranPackage.MediumGeneralId or
                StandardShanheSourceVeteranPackage.HardGeneralId or StandardShanheSourceVeteranPackage.NightmareGeneralId or
                StandardShanheSourceVeteranPackage.HellGeneralId);

    private static bool RequiresCurrentJinnangJiRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(CurrentJinnangJiSkillId, StringComparer.Ordinal);

    private static bool RequiresCurrentDuoduoYishanRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is CurrentDuoduoYishanSkillId or CurrentDuoduoYishan2SkillId or CurrentDuoduoYishan3SkillId);

    private static bool RequiresCurrentYaoliRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is CurrentYaoliSkillId or CurrentYaoli2SkillId or CurrentYaoli3SkillId or
                CurrentYaoli4SkillId or CurrentYaoli5SkillId or CurrentYaoli6SkillId);

    private static bool RequiresFengyiBossRegistry(ShanheBattleSetup setup) =>
        setup.PlayerGeneralId == StandardShanheS5BossPackage.DongZhuoFengyiId ||
        setup.AllyRoster.Concat(setup.EnemyRoster).Any(member => member.GeneralId == StandardShanheS5BossPackage.DongZhuoFengyiId) ||
        RequiresGaichaoBossRegistry(setup) || RequiresDongshiBossRegistry(setup);

    private static bool RequiresGaichaoBossRegistry(ShanheBattleSetup setup) =>
        setup.PlayerGeneralId == StandardShanheS5GaichaoPackage.DongZhuoGaichaoId ||
        setup.AllyRoster.Concat(setup.EnemyRoster).Any(member => member.GeneralId == StandardShanheS5GaichaoPackage.DongZhuoGaichaoId);

    public static GameEngine CreateBattle(ShanheBattleSetup setup) => CreateBattle(setup, false);

    public static GameEngine CreateBattle(ShanheBattleSetup setup, bool confirmOpeningHand) => GameEngine.CreateStandard(new GameOptions
    {
        Seed = setup.BattleSeed, PlayerCount = 1 + setup.AllyRoster.Count + setup.EnemyRoster.Count,
        HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
        ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
        ConfirmOpeningHand = confirmOpeningHand,
        UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 150,
        AiPolicyVersion = AiDecisionPolicy.CurrentVersion
    }, CreateBattleRegistry(setup));

    private sealed class ShanheEncounterPackage(ShanheBattleSetup setup, ShanheBattleSetup requirements) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("shanhe-campaign", new Version(1, 0, 0),
            [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

        public void Register(IContentRegistryBuilder builder)
        {
            EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-passives", skill => skill with { Tags = SkillTag.Locked });
            EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-reactions", skill => skill with { Tags = SkillTag.Locked });
            EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-replacements", skill => skill with { Tags = SkillTag.Locked });
            // Import current source content only when this exact recipe owns it;
            // historical expeditions without this tactic retain their registry.
            if (RequiresCurrentFarStrikeRegistry(requirements))
                EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-current-yuanji", skill => skill with
                {
                    Tags = SkillTag.Locked,
                    CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
                });
            if (RequiresS5ZaiqiRegistry(requirements) && !RequiresFengyiBossRegistry(requirements))
                EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-s5-zaiqi");
            if (RequiresCurrentJinnangJiRegistry(requirements))
                EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-current-jinnangji", skill => skill with
                {
                    Tags = SkillTag.Locked,
                    CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
                });
            if (RequiresCurrentDuoduoYishanRegistry(requirements))
                EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-current-duoduo-yishan", skill => skill with
                {
                    Tags = SkillTag.Locked,
                    CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
                });
            if (RequiresCurrentYaoliRegistry(requirements))
                EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-current-yaoli", skill => skill with
                {
                    Tags = SkillTag.Locked,
                    CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
                });
            if (RequiresCurrentSlashTacticsRegistry(requirements))
                EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-current-slash-tactics", skill => skill with
                {
                    Tags = SkillTag.Locked,
                    CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
                });
            EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-team-2026", skill => skill with { Tags = SkillTag.Locked });
            EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-tactics-2026", skill => skill with
            {
                Tags = SkillTag.Locked,
                CampaignTacticAcquisition = skill.Id is "shanhe:jingyu-moulue-2-2026" or "shanhe:duipai-2026" or "shanhe:duijia-2026"
                    ? new(skill.Id, Repeatable: true)
                    // The source confirms the rule, but not repeated acquisition.
                    // The one-copy expedition limit is an explicit local choice.
                    : skill.Id == "shanhe:duisha-2026" ? new(skill.Id, Repeatable: false) : null
            });
            // The runtime pool also supplies native private general libraries.
            // Campaign seats remain fixed by the opening recipe in BuildCampaignPlayers;
            // a pool containing only occupied seats leaves Zuo Ci with no avatars.
            var ids = GetPlayerGeneralIds().Append(setup.PlayerGeneralId).Concat(setup.AllyRoster.Select(m => m.GeneralId))
                .Concat(setup.EnemyRoster.Select(m => m.GeneralId))
                .Concat(setup.BossTransition is { } transition ? new[] { transition.GeneralId } : [])
                .Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
            var count = 1 + setup.AllyRoster.Count + setup.EnemyRoster.Count;
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "山河图 · 剧情战斗", count, count,
                new Dictionary<string, int>(), "classic:standard-deck", GeneralPoolIds: ids,
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                {
                    [ShanheBattleSetup.PlayerTeamId] = 1 + setup.AllyRoster.Count,
                    [ShanheBattleSetup.EnemyTeamId] = setup.EnemyRoster.Count
                }));
        }
    }
}
