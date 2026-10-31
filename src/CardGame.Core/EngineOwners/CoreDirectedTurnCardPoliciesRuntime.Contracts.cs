#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDirectedTurnCardPoliciesCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
