using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // User screenshots, 2026-10-26: chapter two / Xingyang / White Horse From Dragon.
    // Keep the raw S5 bodies intact; the observed scenario adds four maximum HP
    // and one draw to both enemies. Its difficulty and any later form are unknown.
    private static ShanheEncounterDefinition ChapterTwoWhiteHorseBoss(ShanheNodeDefinition node) => new(
        node.Id + ":white-horse-from-dragon", "荥阳 · 白马从龙",
        [new(StandardShanheS5GongsunZanPackage.GongsunZanId, MaxHpBonus: 4, Hp: 8, DrawBonus: 1),
         new(StandardShanheS5CaorenZhaoyunPackage.ZhaoyunId, MaxHpBonus: 4, Hp: 7, DrawBonus: 1)],
        [], VictoryGold: 500, HealAfterVictory: 2)
    {
        StoryText = "公孙瓒率白马义从迎战，赵云持银枪相随。两军并进，须击败公孙瓒与赵云方能通关。"
    };
}
