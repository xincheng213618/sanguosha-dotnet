#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeTargetLegalityCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICoreNativeTargetLegalityCoreProgramDistanceRulesOperations : IProgramRuntimeOperations
{
    ProgramDistanceRules DistanceRules { get; }
}
