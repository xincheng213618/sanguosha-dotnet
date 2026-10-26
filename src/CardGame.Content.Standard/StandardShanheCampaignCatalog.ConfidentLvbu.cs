using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // The supplied chapter-two ordinary-enemy panel shows HP 6, Draw 5 and Slash 2.
    // It does not establish an opening hand, equipment grant or transformation.
    private static ShanheEncounterDefinition ChapterTwoConfidentLvbu(ShanheNodeDefinition node) => new(
        node.Id + ":confident-lvbu", "自信的吕布",
        [new(StandardShanheConfidentLvbuPackage.LvbuId, Hp: 6, DrawBonus: 3, SlashBonus: 1)],
        [], VictoryGold: 200, HealAfterVictory: 1)
    {
        StoryText = "吕布自恃武勇，率先迎战。骁锐令友军在自己的回合以杀造成伤害后增加本回合出杀次数；奔袭随用牌缩短距离，并可强化单目标杀或普通锦囊。"
    };
}
