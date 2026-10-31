#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct CardUseDistanceQueryFacts(Func<ProgramDistanceRules> getProgramDistanceRules, Func<RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, CharacterState player, CardKind effectiveCardKind) : IMatchCardUseDistanceQuery
{
    public bool HasExemption => getProgramDistanceRules().HasCardUseLimitExemption(player);
    public CardKind EffectiveCardKind => effectiveCardKind;
    public IReadOnlyList<RuleQueryContribution> Contributions => getCoreGameEngineRuleQueriesRuntime().CollectNumericRuleContributions(player, SkillRuleQuery.CardUseDistanceLimit, effectiveCardKind);
}
