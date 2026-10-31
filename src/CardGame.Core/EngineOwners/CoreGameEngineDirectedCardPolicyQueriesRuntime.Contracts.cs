#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDirectedCardPolicyQueriesCoreProgramDistanceRulesOperations : IProgramRuntimeOperations
{
    ProgramDistanceRules DistanceRules { get; }
}

internal interface ICoreDirectedCardPolicyQueriesCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
