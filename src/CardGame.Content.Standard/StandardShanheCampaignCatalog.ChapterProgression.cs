using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    /// <summary>Chapter recipes sit outside the source packages: source body statistics remain untouched.</summary>
    private static ShanheCampaignDefinition WithChapterProgression(ShanheCampaignDefinition definition, int difficulty)
    {
        if (!definition.Id.StartsWith("shanhe:zhuhou-2026-", StringComparison.Ordinal)) return definition;
        var growth = difficulty - 1;
        // Move the complete three-person group, including every original card and qualification.
        // Its stable encounter id identifies its source recipe, not its new local map location.
        var moved = definition.Nodes.Single(node => node.Id == "guangcheng").Encounters!
            .Where(encounter => encounter.Enemies.Count > 2).ToArray();
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            IntroText = definition.IntroText + "\n\n【本地三章征程】第一章主要迎战单将、双将，第二章开始出现三人军阵，第三章迎战强大首领或带兵支援阵。" +
                "全部席位含你、同行和敌人，至多八人。章尾首领分两阶段：第一阶段体力打空后原席进入第二阶段，不提前结算胜利；其余角色保留现状，首领开始新的自然回合。" +
                "前两章分别领取红色战法与技能，第三章胜利后直接结算图册与征程记录。",
            Nodes = definition.Nodes.Select(node =>
            {
                if (node.Encounters is not { Count: > 0 }) return node;
                var encounters = node.Encounters.AsEnumerable();
                if (node.Id == "guangcheng") encounters = encounters.Except(moved);
                if (node.Id == "dagu") encounters = encounters.Concat(moved.Select(encounter => encounter with
                    { Name = "第二章 · 大谷 · 代汉涂高（完整来源组）" }));
                var current = encounters.Select(encounter => ProgressLocalEncounter(node, encounter, growth)).ToList();
                if (node.Id is "dagu" or "huanyuan") current.Add(ChapterTwoTroopEncounter(node, growth));
                if (node.ChapterNumber == 3) current.AddRange(ChapterFinalEncounters(node, growth));
                var description = node.Id switch
                {
                    "hulao" or "guangcheng" => "【第一章】关隘驻军以单将、双将为主。积累行装，为章尾华雄做好准备。",
                    "huaxiong" => "【第一章关底 · 本地两阶段】华雄体力打空后进入虎牢双技形态，恢复至" + (8 + 2 * growth) + "体力并补2张牌。",
                    "dagu" or "huanyuan" => "【第二章】敌军开始集结三人军阵，部分主将的体力、起手与摸牌得到强化。留意徐荣的凶镬与于禁的整军。",
                    "lijue" or "guosi" => "【第二章关底 · 本地两阶段】首领携带专属武器，摸牌与出杀次数增加。体力打空后进入第二阶段，恢复至" + (14 + 2 * growth) + "体力并补3张牌。",
                    _ => "【第三章 · 本地两阶段】最终战可能迎战单体强首领、首领带兵，或郭嘉华佗支援阵，最多八人同场。郭嘉受伤可以补牌，华佗可以救援，须击败全部敌军。各候选阵容的体力和二阶段规则见右侧，胜利后直接进入征程结算。"
                };
                return node with
                {
                    Encounters = current,
                    Description = description + "\n" + node.Description,
                    StoryText = description + (ChapterBossIds.Contains(node.Id)
                        ? "\n第一阶段体力打空才转换；第二阶段开始首领的新自然回合，其余角色保留当时状态。\n"
                        : "\n") + node.StoryText
                };
            }).ToArray()
        });
    }

    private static ShanheEncounterDefinition ProgressLocalEncounter(ShanheNodeDefinition node, ShanheEncounterDefinition encounter, int growth)
    {
        if (node.Id == "huaxiong")
            return encounter with
            {
                Name = "华雄 · 本地两阶段",
                Enemies = [new(StandardShanheS5HuaxiongBasePackage.HuaxiongId, MaxHpBonus: growth, Hp: 5 + growth,
                    InitialHandBonus: growth)],
                BossTransition = new(0, StandardShanheS5HuaxiongPackage.HuaxiongId, 8 + 2 * growth, 8 + 2 * growth, 2)
            };
        if (node.Id is "lijue" or "guosi")
        {
            var leader = encounter.Enemies[0] with
            {
                MaxHpBonus = 5 + 2 * growth, Hp = (node.Id == "lijue" ? 11 : 9) + 2 * growth,
                InitialHandBonus = 2 + growth, DrawBonus = 1, SlashBonus = 1,
                StartingCards = node.Id == "lijue" ? [new(CardKind.ScarletBloodSword, Suit.Club, 6)]
                    : [new(CardKind.ZhanhunDao, Suit.Spade, 2)]
            };
            return encounter with
            {
                Name = encounter.Name + " · 本地两阶段",
                Enemies = new[] { leader }.Concat(encounter.Enemies.Skip(1)).ToArray(),
                BossTransition = new(0, node.Id == "lijue" ? StandardShanheS5LiJuePackage.LijueId
                    : StandardShanheS5GuosiPackage.GuosiId, 14 + 2 * growth, 14 + 2 * growth, 3)
            };
        }
        if (node.ChapterNumber == 3)
            return encounter with
            {
                Name = encounter.Name + " · 本地两阶段",
                // Keep the complete published first-stage roster and every original resource.
                BossTransition = new(0, encounter.Enemies[0].GeneralId, 20 + 4 * growth, 20 + 4 * growth, 3)
            };
        if (node.ChapterNumber == 2 && encounter.Id == node.Id + ":encounter")
            return encounter with
            {
                Name = encounter.Name + " · 本地第二章成长",
                Enemies = encounter.Enemies.Select((enemy, index) => index == 0 ? enemy with
                { MaxHpBonus = 3 + 2 * growth, Hp = 7 + 2 * growth, InitialHandBonus = 1 + growth, DrawBonus = 1 } : enemy).ToArray()
            };
        return encounter;
    }

    private static ShanheEncounterDefinition ChapterTwoTroopEncounter(ShanheNodeDefinition node, int growth) => new(
        node.Id + ":chapter-troops", node.Id == "dagu" ? "辽东猛将与护粮军（本地三敌）" : "轘辕盾阵（本地三敌）",
        [new(node.Id == "dagu" ? StandardShanheS5XuRongPackage.XurongId : StandardShanheS5YujinPackage.YujinId,
            MaxHpBonus: 3 + 2 * growth, Hp: 7 + 2 * growth, InitialHandBonus: 1 + growth, DrawBonus: 1)
            { StartingCards = node.Id == "dagu" ? [new(CardKind.YaodaoChitong, Suit.Spade, 6)] : null },
         new(StandardShanheLocalTroopPackage.ArcherId, MaxHpBonus: growth),
         new(StandardShanheLocalTroopPackage.GuardId, MaxHpBonus: growth)], [], VictoryGold: 100, HealAfterVictory: 2);

    private static IEnumerable<ShanheEncounterDefinition> ChapterFinalEncounters(ShanheNodeDefinition node, int growth)
    {
        var generalId = node.Id switch
        {
            "luoyang" => StandardShanheS5BossPackage.DongMinBaseId,
            "suanzao" => "classic:yuan-shao",
            _ => StandardShanheS5BossPackage.ZhangliaoBaseId
        };
        var secondId = node.Id switch
        {
            "luoyang" => StandardShanheS5BossPackage.DongMinCuanchaoId,
            "suanzao" => StandardShanheS5YuanshaoPackage.YuanshaoId,
            _ => growth == 2 ? StandardShanheS5BossPackage.ZhangliaoHuangkongId : StandardShanheS5BossPackage.ZhangliaoLingdongId
        };
        // Source InitialHp has precedence over a maximum-HP bonus. These local
        // bodies explicitly start at their grown maximum; source rosters above do not.
        var leader = new ShanheCombatantDefinition(generalId, MaxHpBonus: 10 + 4 * growth,
            Hp: (node.Id == "luoyang" ? 15 : 14) + 4 * growth,
            InitialHandBonus: 3 + growth, DrawBonus: 2, SlashBonus: 1);
        var transition = new ShanheBossTransition(0, secondId, 24 + 4 * growth, 24 + 4 * growth, 3);
        var allies = node.Encounters![0].Allies ?? [];
        yield return Recipe("single", "孤阵首领（本地两阶段单敌）", [leader], allies);
        yield return Recipe("warband", "首领与五卒（本地两阶段军阵）", new[] { leader }.Concat(Troops(5)).ToArray(), allies);
        yield return Recipe("encirclement", "首领与六卒（本地无同行两阶段围阵）", new[] { leader }.Concat(Troops(6)).ToArray(), []);
        yield return Recipe("support", "郭嘉华佗支援阵（本地两阶段军阵）",
            new[] { leader, new ShanheCombatantDefinition("classic:guo-jia", MaxHpBonus: 3 + growth, Hp: 6 + growth, InitialHandBonus: 1),
                new ShanheCombatantDefinition("classic:hua-tuo", MaxHpBonus: 3 + growth, Hp: 6 + growth, InitialHandBonus: 1) }
                .Concat(Troops(3)).ToArray(), allies);

        ShanheEncounterDefinition Recipe(string id, string name, IReadOnlyList<ShanheCombatantDefinition> enemies,
            IReadOnlyList<ShanheCombatantDefinition> companions) => new(node.Id + ":chapter-" + id, name,
                enemies, companions, VictoryGold: 500, HealAfterVictory: 2) { BossTransition = transition };
        IEnumerable<ShanheCombatantDefinition> Troops(int count) => Enumerable.Range(0, count).Select(index =>
            new ShanheCombatantDefinition((index % 3) switch
            {
                0 => StandardShanheLocalTroopPackage.FootmanId,
                1 => StandardShanheLocalTroopPackage.ArcherId,
                _ => StandardShanheLocalTroopPackage.GuardId
            }, MaxHpBonus: growth, InitialHandBonus: 1 + growth, DrawBonus: 1));
    }
}
