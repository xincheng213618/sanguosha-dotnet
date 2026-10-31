#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramCardUseRulesCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
