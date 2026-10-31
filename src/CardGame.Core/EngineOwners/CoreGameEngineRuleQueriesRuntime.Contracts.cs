#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreRuleQueriesMatchCardTargetQueriesOperations : IProgramRuntimeOperations
{
    MatchCardTargetQueries Get();
}

internal interface ICoreRuleQueriesMatchHandLimitQueriesOperations : IProgramRuntimeOperations
{
    MatchHandLimitQueries Get();
}

internal interface ICoreRuleQueriesProgramNumericContributionOperations : IProgramRuntimeOperations
{
    ProgramNumericContributionRuntime Get();
}

internal interface ICoreRuleQueriesMatchTurnNumericQueriesOperations : IProgramRuntimeOperations
{
    MatchTurnNumericQueries Get();
}

internal interface ICoreRuleQueriesCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICoreRuleQueriesMatchDistanceQueriesOperations : IProgramRuntimeOperations
{
    MatchDistanceQueries Get();
}
