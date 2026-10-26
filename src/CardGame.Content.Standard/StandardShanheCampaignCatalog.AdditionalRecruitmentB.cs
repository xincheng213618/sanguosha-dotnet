using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    private static IEnumerable<ShanheStoryEventDefinition> CreateAdditionalRecruitmentBStories(int chapterNumber)
    {
        foreach (var entry in StandardShanheAdditionalRecruitmentBPackage.Entries)
        {
            if (entry.Key == "cao-ang" && chapterNumber != 1) continue;
            const string accompany = "加入后在本次征程的后续战斗助战，每战重新登场；满员八人时暂不入战。";
            var support = $"{entry.Name} · 体力{entry.Hp} · " + (entry.Key switch
            {
                "wang-rong" => "丰姿、吉占、赋颂。",
                "liu-hong" => "鬻爵、图兴。",
                "hua-tuo" => "急救、青囊。",
                "cao-ang" => "慷忾。",
                "lu-ji" => "怀橘、遗礼、整论。",
                "liu-shan" => "享乐、放权。",
                _ => throw new InvalidOperationException("Unknown recruitment companion.")
            });
            yield return new("story:recruit-" + entry.Key, "同行相助 · " + entry.Name,
                $"行军途中，{entry.Name}愿随军相助。请选择普通帮手或支付400铜币招募精英帮手。",
                [
                    new("recruit-" + entry.Key + "-normal", "招纳此人 · 普通帮手 · 免费")
                    { Companion = new(entry.NormalId), Description = support + "\n" + accompany },
                    new("recruit-" + entry.Key + "-elite", "奉为上宾 · 精英帮手", GoldCost: 400)
                    { Companion = new(entry.EliteId), Description = support + "\n额外技能：" + entry.ExtraDescription + "\n" + accompany },
                    new("decline-" + entry.Key, "婉言谢绝 · 获得200铜币", RewardId: "story-decline-recruit")
                    { Description = "谢绝帮手，获得200铜币，继续行军。" }
                ]);
        }
    }
}
