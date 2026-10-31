#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeTrickUseCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
