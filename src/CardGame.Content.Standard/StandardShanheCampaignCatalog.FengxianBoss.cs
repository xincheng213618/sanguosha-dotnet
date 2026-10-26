using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    private static ShanheEncounterDefinition ChapterOneFengxianBoss(ShanheNodeDefinition node) => new(
        node.Id + ":wushuang-fengxian", "虎牢关 · 无双奉先",
        [new(StandardShanheFengxianBossPackage.FirstId, Hp: 6, InitialHandBonus: 3, DrawBonus: 3, SlashBonus: 1)],
        [], VictoryGold: 500, HealAfterVictory: 2)
    {
        StoryText = "吕布横戟守关。击破吕布的第一形态后，他将化为暴怒的战神再战，须击败第二形态方能通关。",
        BossTransition = new(0, StandardShanheFengxianBossPackage.RageId, 6, 6)
        {
            StartingEquipment = [new(CardKind.WushuangHalberd, Suit.Diamond, 12),
                new(CardKind.PurpleGoldCrown, Suit.Diamond, 1)]
        }
    };
}
