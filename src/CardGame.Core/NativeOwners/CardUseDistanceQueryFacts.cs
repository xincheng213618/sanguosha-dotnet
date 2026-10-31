#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct CardUseDistanceQueryFacts(global::System.Func<global::CardGame.Core.ProgramDistanceRules> getProgramDistanceRules, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, CharacterState player, CardKind effectiveCardKind) : IMatchCardUseDistanceQuery
{
    public bool HasExemption => getProgramDistanceRules().HasCardUseLimitExemption(player);
    public CardKind EffectiveCardKind => effectiveCardKind;
    public IReadOnlyList<RuleQueryContribution> Contributions => getCoreGameEngineRuleQueriesRuntime().CollectNumericRuleContributions(player, SkillRuleQuery.CardUseDistanceLimit, effectiveCardKind);
}
