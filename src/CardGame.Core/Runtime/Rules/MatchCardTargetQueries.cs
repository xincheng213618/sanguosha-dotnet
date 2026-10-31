namespace CardGame.Core;

internal interface IMatchCardTargetCountQuery : IProgramSelectionQuery
{
    CardKind EffectiveCardKind { get; }
    IReadOnlyList<RuleQueryContribution> Contributions { get; }
    IEnumerable<RuleQueryContribution> FirstSlashContributions { get; }
    IReadOnlyList<TurnRuleModifier> TurnModifiers { get; }
    RuleQueryContribution[] ProjectContributions(RuleQueryContribution[] contributions);
}

internal interface IMatchCardUseDistanceQuery : IProgramSelectionQuery
{
    bool HasExemption { get; }
    CardKind EffectiveCardKind { get; }
    IReadOnlyList<RuleQueryContribution> Contributions { get; }
}

[ProgramRuntimeModule]
internal sealed class MatchCardTargetQueries
{
    internal RuleQueryEvaluation TargetCount<TQuery>(TQuery query) where TQuery : IMatchCardTargetCountQuery
    {
        var contributions = query.Contributions
            .Concat(query.FirstSlashContributions)
            .Concat(query.TurnModifiers
                .Where(item => item.Operation == SkillRuleOperation.Add)
                .Select(item => (RuleQueryContribution)new FiniteRuleQueryContribution(
                    $"turn:{item.GrantSequence}:card-target-count", SkillRuleOperation.Add, item.Amount))).ToArray();
        contributions = query.ProjectContributions(contributions);
        return RuleQueryService.Evaluate(SkillRuleQuery.CardTargetCount, new RuleQueryBounds(1, int.MaxValue),
            [new RuleQueryBaseTerm($"card:{query.EffectiveCardKind}:base-target-count", 1)], contributions);
    }

    internal int UseDistanceLimit<TQuery>(TQuery query) where TQuery : IMatchCardUseDistanceQuery =>
        query.HasExemption ? int.MaxValue : RuntimeSkills.RuntimeRules_Core_RuleQueries.ConvertRuleValue(RuleQueryService.Evaluate(
            SkillRuleQuery.CardUseDistanceLimit,
            new RuleQueryBounds(1, int.MaxValue),
            [new RuleQueryBaseTerm($"card:{query.EffectiveCardKind}:base-use-distance", 1)],
            query.Contributions));
}
