using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string WuguotaiRecruitmentStoryId = "story:xiangyinweigai-wuguotai";

    private static ShanheStoryEventDefinition CreateWuguotaiRecruitmentStory()
    {
        const string support = "吴国太 · 体力3\n甘露：出牌阶段限一次，令两名角色交换装备区里的牌。若装备数差X大于你已损失的体力，选择角色时须弃置X张牌。\n" +
            "补益：当一名角色进入濒死状态时，可选择其一张牌；若非基本牌，其弃置此牌并回复1点体力。";
        const string accompany = "加入后在本次征程的后续战斗助战，每战重新登场；满员八人时暂不入战。";
        return new(WuguotaiRecruitmentStoryId, "乡音未改 · 吴国太", "小卒来报：‘门外有人自称是将军的同乡，前来投奔。’你决定？",
        [
            new("recruit-wuguotai-normal", "招纳此人 · 普通帮手 · 免费")
            {
                Companion = new(StandardShanheWuguotaiCompanionPackage.NormalId),
                Description = support + "\n" + accompany
            },
            new("recruit-wuguotai-elite", "奉为上宾 · 精英帮手", GoldCost: 400)
            {
                Companion = new(StandardShanheWuguotaiCompanionPackage.EliteId),
                Description = support + "\n额外技能【丰姿】：出牌阶段限一次，当你使用基本牌或普通锦囊牌时，可弃置一张类型相同的手牌，令此牌效果额外结算一次。\n" + accompany
            },
            new("decline-wuguotai", "婉言谢绝 · 获得200铜币", RewardId: "story-decline-recruit")
            { Description = "谢绝帮手，获得200铜币，继续行军。" }
        ]);
    }
}
