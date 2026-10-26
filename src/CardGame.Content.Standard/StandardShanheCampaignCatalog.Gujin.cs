using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // The in-progress 2/6 HP and zero hand cards are not an opening recipe.
    // Preserve the shown six-HP body, Draw6/Slash2, and ordinary opening hand.
    private static ShanheEncounterDefinition ChapterOneGujin(ShanheNodeDefinition node) => new(
        node.Id + ":gu-jin", "骨进",
        [new(StandardShanheGujinPackage.GujinId, Hp: 6, DrawBonus: 4, SlashBonus: 1)],
        [], VictoryGold: 200, HealAfterVictory: 1)
    {
        StoryText = "骨进以无双迎战，每回合造成伤害后可发动一次膂力，选择补手牌至体力值，或回复体力至手牌数。"
    };
}
