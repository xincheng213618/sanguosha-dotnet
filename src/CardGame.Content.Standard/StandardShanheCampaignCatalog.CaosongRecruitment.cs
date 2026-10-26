using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CaosongRecruitmentStoryId = "story:xiangyinweigai";

    private static ShanheStoryEventDefinition CreateCaosongRecruitmentStory()
    {
        const string support = "曹嵩 · 体力4\n礼赂：摸牌阶段可改为将手牌摸至体力上限（至多5），交给一名其他角色至少一张手牌；首次交出或多于上次时，体力上限+1并回复1点体力。\n" +
            "翊正：结束阶段可选择一名其他角色，直到你的下回合开始，其造成伤害或回复体力时，若其体力上限小于你，你减少1点体力上限，使该伤害或回复值+1。";
        const string accompany = "加入后在本次征程的后续战斗助战，每战重新登场；满员八人时暂不入战。";
        return new(CaosongRecruitmentStoryId, "乡音未改", "小卒来报：‘门外有人自称是将军的同乡，前来投奔。’你决定？",
        [
            new("recruit-caosong-normal", "招纳此人 · 普通帮手 · 免费")
            { Companion = new("ol:cao-song"), Description = support + "\n" + accompany },
            new("recruit-caosong-elite", "奉为上宾 · 精英帮手", GoldCost: 400)
            {
                Companion = new(StandardShanheCaosongCompanionPackage.EliteId),
                Description = support + "\n额外技能【惶汗】：" + StandardShanheCaosongCompanionPackage.HuanghanDescription + "\n" + accompany
            },
            new("decline-caosong", "婉言谢绝 · 获得200铜币", RewardId: "story-decline-recruit")
            { Description = "谢绝帮手，获得200铜币，继续行军。" }
        ]);
    }
}
