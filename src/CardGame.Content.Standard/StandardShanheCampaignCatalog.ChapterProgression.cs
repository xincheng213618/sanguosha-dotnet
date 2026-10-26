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
                "全部席位含你、同行和敌人，至多八人。部分章尾首领分两阶段：第一阶段体力打空后原席进入第二阶段，不提前结算胜利；其余角色保留现状，首领开始新的自然回合。第二章另可随机遇到荥阳白马从龙双敌首领。" +
                "前两章分别领取史诗战法与史诗手牌，第三章胜利后直接结算图册与征程记录。",
            Nodes = definition.Nodes.Select(node =>
            {
                if (node.Encounters is not { Count: > 0 }) return node;
                var encounters = node.Encounters.AsEnumerable();
                if (node.Id == "guangcheng") encounters = encounters.Except(moved);
                if (node.Id == "dagu") encounters = encounters.Concat(moved.Select(encounter => encounter with
                    { Name = "第二章 · 大谷 · 代汉涂高（完整来源组）" }));
                var current = encounters.Select(encounter => ProgressLocalEncounter(node, encounter, growth)).ToList();
                if (node.Id is "hulao" or "guangcheng") current.Add(ChapterOneGujin(node));
                if (node.Id == "huaxiong") current.Add(ChapterOneFengxianBoss(node));
                if (node.Id is "dagu" or "huanyuan") current.Add(ChapterTwoTroopEncounter(node, growth));
                if (node.Id is "dagu" or "huanyuan") current.Add(ChapterTwoConfidentLvbu(node));
                if (node.Id is "lijue" or "guosi") current.Add(ChapterTwoWhiteHorseBoss(node));
                if (node.Id is "lijue" or "guosi") current.Add(ChapterTwoHebeiScreenshotBoss(node));
                if (node.ChapterNumber == 3) current.AddRange(ChapterFinalEncounters(node, growth));
                if (node.Id == "luoyang") current.Add(ChapterThreeDongClanBoss(node));
                var description = node.Id switch
                {
                    "hulao" or "guangcheng" => "【第一章】关隘驻军以单将、双将为主。可遇骨进6/6，摸6张、出杀2次，使用无双与膂力；积累行装，迎战章尾随机首领。",
                    "huaxiong" => "【第一章关底 · 随机首领】无双奉先：吕布6/6→暴怒的战神6/6，两阶段摸5张、出杀2次；二阶段自动装备无双方天戟、束发紫金冠。另可遇华雄，体力打空后进入虎牢双技形态，恢复至" + (8 + 2 * growth) + "体力并补2张牌。",
                    "dagu" or "huanyuan" => "【第二章】可遇自信的吕布6/6，摸5张、出杀2次，骁锐助友军续杀、奔袭强化用牌。敌军另有徐荣、于禁率领的三人军阵。",
                    "lijue" or "guosi" => "【第二章关底 · 随机首领】可遇河北双雄颜良文丑9/9，摸5张、出杀2次；或荥阳白马从龙公孙瓒8/8、赵云7/7，各摸3张、出杀1次。李傕、郭汜候选携带专属武器，体力打空后进入本地第二阶段，恢复至" + (14 + 2 * growth) + "体力并补3张牌。",
                    _ => "【第三章 · 两阶段首领】最终战可能迎战单体强首领、首领带兵，或郭嘉华佗支援阵，最多八人同场。郭嘉受伤可以补牌，华佗可以救援，须击败全部敌军。各候选阵容的体力和二阶段规则见右侧，胜利后直接进入征程结算。"
                };
                if (node.Id == "luoyang")
                    description += "另可遇董氏凶傑：董卓10/10→董旻8/8，两阶段摸6张、出杀2次；董旻以盾弓将单次伤害限制至3点。";
                if (node.Id == "luoyang" && difficulty == 2)
                    description += "司马隐谋中等档开场10/10体力，摸牌阶段摸6张、出杀1次；忍戒每受1点伤害或弃牌阶段每弃1张手牌获得4忍。";
                return node with
                {
                    Encounters = current,
                    Description = description + "\n" + node.Description,
                    StoryText = description + (ChapterBossIds.Contains(node.Id)
                        ? "\n" + (node.ChapterNumber == 2 ? "李傕、郭汜候选：" : "") + "第一阶段体力打空才转换；第二阶段开始首领的新自然回合，其余角色保留当时状态。\n"
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
        {
            // The supplied medium-difficulty group 2677 panel shows maximum HP 10,
            // normal draw 6 and Slash 1. Its displayed 9/10 is already wounded.
            // Keep the source general at 4/4; only this observed encounter is overlaid.
            if (encounter.Id == "luoyang:simayi-s5" && growth == 1)
                encounter = encounter with
                {
                    Name = "司马隐谋 · 中等",
                    Enemies = [encounter.Enemies[0] with { MaxHpBonus = 6, Hp = 10, DrawBonus = 4 }],
                    StoryText = "第三章首领司马懿：开场10/10体力，摸牌阶段摸6张、出杀1次。" +
                        "忍戒：每受到1点伤害，或于弃牌阶段弃置1张手牌后，获得4枚“忍”。"
                };
            return encounter with
            {
                Name = encounter.Name + " · 本地两阶段",
                // Keep the complete first-stage roster and every original resource,
                // apart from the explicitly observed medium Sima Yi panel above.
                BossTransition = new(0, encounter.Enemies[0].GeneralId, 20 + 4 * growth, 20 + 4 * growth, 3)
            };
        }
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
