using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // These chapter recipes use the local three-tier curve. A local run is
    // never relabelled as one of the 25 source difficulties.
    private static ShanheCampaignDefinition WithReturningSourceReinforcements(ShanheCampaignDefinition definition, int difficulty)
    {
        if (!definition.Id.StartsWith("shanhe:zhuhou-2026-", StringComparison.Ordinal) || definition.SourceDifficultyId is not null)
            return definition;
        if (difficulty is < 1 or > 3) throw new ArgumentOutOfRangeException(nameof(difficulty));
        var growth = difficulty - 1;
        var nodes = definition.Nodes.Select(node => node.Id switch
        {
            "dagu" => Add(node,
                new("dagu:xusheng-source-native", "徐盛 · 破军军阵",
                    [Leader(StandardShanheSourceXuShengPackage.MediumGeneralId, 4,
                        [new(CardKind.GhostDragonCrescentBlade, Suit.Club, 5)])], [], VictoryGold: 200, HealAfterVictory: 2)
                { StoryText = "徐盛可以封存杀目标的全部手牌至回合结束，牌数占优时杀伤害增加。" },
                new("dagu:qiangwang-source-native", "狂暴枪王 · 先发雄乱",
                    [Leader(StandardShanheSourceQiangWangPackage.GeneralId, 4,
                        [new(CardKind.WushuangHalberd, Suit.Heart, 12), new(CardKind.LionBelt, Suit.Club, 2)])], [],
                    VictoryGold: 200, HealAfterVictory: 2)
                { StoryText = "枪王凭动荡先行，雄乱可废除自己的装备区与判定区，压制一名目标本回合的手牌。" }),
            "huanyuan" => Add(node,
                new("huanyuan:sunce-source-native", "孙策 · 魂姿奋起",
                    [Leader(StandardShanheSourceSunCePackage.SunCeId, 3,
                        [new(CardKind.BlackLightArmor, Suit.Diamond, 2)])], [], VictoryGold: 200, HealAfterVictory: 2)
                { StoryText = "孙策在受伤后的准备阶段觉醒，获得英姿与英魂，并在该回合结束时摸三张牌或回复两点体力。" },
                new("huanyuan:xunyu-source-native", "荀彧与西凉卫士",
                    [Leader(StandardShanheSourceXunYuPackage.GeneralId, 3, [new(CardKind.GouXiang, Suit.Club, 2)]) with
                    {
                        // Quhu needs an opponent with greater current HP. Keep
                        // that window open; armor and a guard provide durability.
                        Hp = 3, InitialArmor = 2 + growth, ArmorRecoveryPerRound = 1, ArmorRecoveryCap = 2 + growth
                    }, new(StandardShanheLocalTroopPackage.GuardId, MaxHpBonus: growth, Hp: 6 + growth,
                        InitialHandBonus: growth)], [], VictoryGold: 200, HealAfterVictory: 2)
                { StoryText = "荀彧以三点体力、护甲和卫士迎战。驱虎借拼点对手造成伤害，节命可为同伴补牌。" }),
            "luoyang" => Add(node, ReturningMouDongZhuoEncounter(growth)),
            _ => node
        }).ToArray();
        return WithReturningNativeBossStages(ShanheCampaignContent.FreezeAndValidate(definition with
        {
            Nodes = nodes,
            IntroText = definition.IntroText + "\n第二章新增徐盛、孙策、狂暴枪王与荀彧卫队；黄巾余匪接入张燕阶段，西凉霸主接入篡天恶魂。洛阳另有逆京枭雄。"
        }), difficulty);

        ShanheCombatantDefinition Leader(string generalId, int baseHp, IReadOnlyList<ShanheStartingCard> equipment) =>
            new(generalId, MaxHpBonus: 3 + 2 * growth, Hp: baseHp + 3 + 2 * growth,
                InitialHandBonus: 1 + growth, DrawBonus: 1)
            { StartingCards = equipment };
        static ShanheNodeDefinition Add(ShanheNodeDefinition node, params ShanheEncounterDefinition[] encounters) => node with
        {
            Encounters = node.Encounters!.Concat(encounters).ToArray(),
            Description = node.Description + "另有" + string.Join("、", encounters.Select(encounter => encounter.Name)) + "。"
        };
    }

    private static ShanheEncounterDefinition ReturningMouDongZhuoEncounter(int growth)
    {
        var hp = 20 + 4 * growth;
        var general = StandardShanheSourceMouDongZhuoPackage.GeneralId;
        var second = StandardShanheSourceShenNuStagePackage.GeneralId;
        return new("luoyang:mou-dongzhuo-source-native", "逆京枭雄 · 谋董卓与篡天恶魂",
            [new(general, MaxHpBonus: 4 * growth, Hp: hp, InitialHandBonus: 1 + growth, DrawBonus: 1 + growth)
            {
                StartingCards = [new(CardKind.KunlunMirror, Suit.Spade, 12), new(CardKind.Duanhun, Suit.Spade, 5)]
            }], [], VictoryGold: 200, HealAfterVictory: 2)
        {
            BossTransition = new(0, second, hp, hp, 3 + growth)
            {
                Combatant = new(second, MaxHpBonus: 4 * growth, Hp: hp, DrawBonus: 1 + growth, SlashBonus: 1),
                StartingEquipment = [new(CardKind.ShuraInfernoHalberd, Suit.Heart, 12), new(CardKind.LionBelt, Suit.Club, 2)]
            },
            StoryText = $"谋董卓以{hp}点体力迎战，携带昆仑镜与断魂。凶逆迫使其他角色弃五张同花色牌，否则降低体力上限；执柄惩罚同一回合连续使用黑色牌。" +
                $"击败后，篡天恶魂以{hp}点体力接战。"
        };
    }
}
