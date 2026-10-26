using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string GuanpingRecruitmentStoryId = "story:beijiu-fengjun";

    private static ShanheStoryEventDefinition CreateGuanpingRecruitmentStory()
    {
        const string support = "关平 · 体力4\n龙吟：当一名角色于其出牌阶段内使用杀时，你可以弃置一张牌，令此杀不计入此阶段使用次数；若此杀为红色，你摸一张牌。";
        const string accompany = "加入后在本次征程的后续战斗助战，每战重新登场；满员八人时暂不入战。";
        return new(GuanpingRecruitmentStoryId, "杯酒逢君", "大汉声称：‘仰慕将军已久，愿投入军中供将军差遣！’你决定？",
        [
            new("recruit-guanping-normal", "招纳此人 · 普通帮手 · 免费")
            { Companion = new(StandardShanheGuanpingCompanionPackage.NormalId), Description = support + "\n" + accompany },
            new("recruit-guanping-elite", "打造武备 · 精英帮手", GoldCost: 400)
            {
                Companion = new(StandardShanheGuanpingCompanionPackage.EliteId),
                Description = support + "\n额外技能【魔炎】：锁定技，回合外失去牌时判定；红色时对一名其他角色造成1点火焰伤害。\n" + accompany
            },
            new("decline-guanping", "婉言谢绝 · 获得200铜币", RewardId: "story-decline-recruit")
            { Description = "谢绝帮手，获得200铜币，继续行军。" }
        ]);
    }
}
