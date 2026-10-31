namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_PublicPersistentPiles
{
    internal static string PublicPileCardLabel(Card card) => card.DisplayName + " " + RuntimeRules_Core_CommonRules.GetSuitDisplayName(card.Suit) + card.RankText;
}
