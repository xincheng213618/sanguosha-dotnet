#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreRequestedDeckBasicsCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
