using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // User's 2026-10-27 medium-3 panel, battle 2665: one 9/9 general,
    // Draw5 / Slash2. Retain the source 4/4 body and its verified Shuangxiong.
    // Other difficulty bands and a later form were not shown; the three local
    // difficulties deliberately share this observed panel until further evidence.
    private static ShanheEncounterDefinition ChapterTwoHebeiScreenshotBoss(ShanheNodeDefinition node) => new(
        node.Id + ":hebei-twin-heroes", "河北双雄",
        [new(StandardShanheS5YanliangWenchouPackage.YanliangWenchouId,
            MaxHpBonus: 5, Hp: 9, DrawBonus: 3, SlashBonus: 1)],
        [], VictoryGold: 500, HealAfterVictory: 2)
    {
        StoryText = "颜良文丑纵马横槊，以双雄之势拦住去路。双雄在摸牌阶段结束弃牌后，可将异色牌当作本轮发动次数伤害的决斗使用；结束阶段收取本回合对其造成伤害的牌。"
    };
}
