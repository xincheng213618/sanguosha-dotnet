using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    private static ShanheStoryEventDefinition CreateCaoruiRecruitmentStory() => new("story:zhijinanpeng", "知己难逢",
        "听闻群雄起而讨董，乘氏李家递上拜帖，想率门下食客来助你一臂之力。",
        [
            new("recruit-caorui-normal", "划入军中 · 普通帮手 · 免费")
            { Companion = new(StandardShanheCaoruiCompanionPackage.NormalId), Description =
                "曹叡 · 体力3\n恢拓：受到伤害后，可令一名角色判定；红色回复1点体力，黑色摸与本次伤害值等量的牌。\n" +
                "明鉴：出牌阶段限一次，把所有手牌交给一名其他角色，其下回合手牌上限+1、杀的次数上限+1。\n加入后在本次征程后续战斗助战；满员时暂不入战。" },
            new("recruit-caorui-elite", "封为前锋 · 精英帮手", GoldCost: 400)
            { Companion = new(StandardShanheCaoruiCompanionPackage.EliteId), Description =
                "曹叡 · 体力3 · 恢拓、明鉴与普通版相同。\n额外技能【乱心】：被弃置手牌后手牌上限+1；受伤后体力上限+1；造成伤害后回复1点体力；获得其他角色手牌后，下个摸牌阶段摸牌数+1。\n加入后在本次征程后续战斗助战；满员时暂不入战。" },
            new("decline-caorui", "婉言谢绝 · 获得200铜币", RewardId: "story-decline-recruit")
            { Description = "谢绝帮手，获得200铜币，继续行军。" }
        ]);
}
