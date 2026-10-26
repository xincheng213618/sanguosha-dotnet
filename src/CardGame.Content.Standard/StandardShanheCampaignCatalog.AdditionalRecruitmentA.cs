using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string QinmiRecruitmentStoryId = "story:recruit-qinmi";
    public const string SunluyuRecruitmentStoryId = "story:recruit-sunluyu";
    public const string LusuRecruitmentStoryId = "story:recruit-lusu";
    public const string SunruRecruitmentStoryId = "story:recruit-sunru";
    public const string XizhicaiRecruitmentStoryId = "story:recruit-xizhicai";

    private static IEnumerable<ShanheStoryEventDefinition> CreateAdditionalRecruitmentAStories(int chapterNumber)
    {
        // The guide only encountered Qin Mi and Lu Su in the first chapter.
        if (chapterNumber == 1)
        {
            yield return Recruit(QinmiRecruitmentStoryId, "qinmi", "秦宓", "classic:qin-mi",
                StandardShanheAdditionalRecruitmentAPackage.QinmiEliteId,
                "谏征：其他角色使用杀指定其他角色为目标时，若你在使用者攻击范围内，可将一张手牌置于牌堆顶，取消所有目标；非黑色杀改以你为目标。\n" +
                "专对：使用杀或成为杀的目标后可与对方拼点，赢则令对方不能响应此杀，或令此杀对你无效。\n天辩：拼点可用牌堆顶牌，红桃点数视为K。",
                "纵适：拼点赢时可获得点数较小的拼点牌；拼点没赢时可获得自己的拼点牌。");
            yield return Recruit(LusuRecruitmentStoryId, "lusu", "界鲁肃", "boundary:lu-su",
                StandardShanheAdditionalRecruitmentAPackage.LusuEliteId,
                "好施：摸牌阶段可多摸两张，手牌大于5时将一半手牌交给手牌最少的一名其他角色；直到下回合开始，你成为杀或普通锦囊目标后，其可交给你一张手牌。\n" +
                "缔盟：出牌阶段限一次，令手牌数差不大于你的牌数的两名其他角色交换手牌，出牌阶段结束时弃置冻结差值数量的牌。",
                "弘德：一次获得至少两张手牌或失去至少两张牌后，可令一名其他角色摸一张牌。");
        }
        yield return Recruit(SunluyuRecruitmentStoryId, "sunluyu", "孙鲁育", "ol:sun-lu-yu",
            StandardShanheAdditionalRecruitmentAPackage.SunluyuEliteId,
            "魅步：其他角色出牌阶段开始时，若你在其攻击范围内，可弃一牌令其本回合获得止息；弃牌非杀或黑色锦囊时，本回合其与你距离视为1。\n" +
            "穆穆：出牌阶段开始时可弃一名其他角色的一张装备，或获得一名角色的防具；选择后者本回合不能使用或打出杀。",
            "八阵：装备区没有防具时，视为装备八卦阵。");
        yield return Recruit(SunruRecruitmentStoryId, "sunru", "孙茹", "ol:sun-ru",
            StandardShanheAdditionalRecruitmentAPackage.SunruEliteId,
            "持室：每回合限一次，当前回合角色失去一个区域内最后一张牌后，可令其摸两张牌且本回合手牌上限+2。\n" +
            "慰勉：出牌阶段限一次，废除至多三个装备栏，令一名角色选择等量项：恢复装备栏、回复1点体力、弃所有手牌并摸四张牌。",
            "奋锐：结束阶段可弃一牌并复原一个装备栏，随机使用对应装备；随后可对装备数更少的角色造成装备数之差的伤害，以此法造成伤害后不能再以此法造成伤害。");
        yield return Recruit(XizhicaiRecruitmentStoryId, "xizhicai", "戏志才", "ol:xi-zhi-cai",
            StandardShanheAdditionalRecruitmentAPackage.XizhicaiEliteId,
            "天妒：判定牌生效后可获得该牌。\n先辅：游戏开始时选择一名其他角色，其受伤或回复体力后，你受到或回复等量体力。\n" +
            "筹策：每受到1点伤害后可判定，红色令角色摸一张（先辅目标改为两张），黑色弃一名角色区域内一牌。",
            "忠允：锁定技，每回合各限一次，受到伤害或回复体力、手牌数变化后，若体力值与手牌数相同，分别可回复1点或对范围内角色造成1点伤害、摸一张或弃其他角色一牌。");

        static ShanheStoryEventDefinition Recruit(string storyId, string choiceKey, string name, string normalId, string eliteId,
            string skills, string eliteSkill)
        {
            const string accompany = "加入后在本次征程的后续战斗助战，每战重新登场；满员八人时暂不入战。";
            var support = name + " · 体力3\n" + skills;
            return new(storyId, "求贤若渴 · " + name, "行军途中，" + name + "前来相助。你决定？",
            [
                new("recruit-" + choiceKey + "-normal", "招纳此人 · 普通帮手 · 免费")
                { Companion = new(normalId), Description = support + "\n" + accompany },
                new("recruit-" + choiceKey + "-elite", "奉为上宾 · 精英帮手", GoldCost: 400)
                { Companion = new(eliteId), Description = support + "\n额外技能·" + eliteSkill + "\n" + accompany },
                new("decline-" + choiceKey, "婉言谢绝 · 获得200铜币", RewardId: "story-decline-recruit")
                { Description = "谢绝帮手，获得200铜币，继续行军。" }
            ]);
        }
    }
}
