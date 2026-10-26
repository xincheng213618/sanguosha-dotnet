using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // User panels show Dong Zhuo 10/10 at stage 1/2 and Dong Min 8/8 at
    // stage 2/2. Their printed skills exactly match the already loaded forms.
    // Keep the raw source bodies and the separate local Cuanchao recipe intact.
    private static ShanheEncounterDefinition ChapterThreeDongClanBoss(ShanheNodeDefinition node) => new(
        node.Id + ":dong-clan-screenshot", "董氏凶傑",
        [new(StandardShanheS5BossPackage.DongZhuoLuoyangId, MaxHpBonus: 6, Hp: 10, DrawBonus: 4, SlashBonus: 1)
            { QualifiedPrintedLordSkillIds = [StandardShanheS5BossPackage.DongZhuoBaonueId] }],
        [], VictoryGold: 500, HealAfterVictory: 2)
    {
        BossTransition = new(0, StandardShanheS5BossPackage.DongMinBaseId, 8, 8),
        StoryText = "董卓以酒池、肉林、崩坏、暴虐迎战；第一阶段体力打空后，董旻以盾弓接战。" +
            "董卓10/10、董旻8/8，两形态摸6张、出杀2次。盾弓使单次伤害至多为3。"
    };
}
