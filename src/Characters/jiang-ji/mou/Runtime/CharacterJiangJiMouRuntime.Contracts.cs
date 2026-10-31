#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterJiangJiMouMouJiangJiOperations : IProgramRuntimeOperations
{
    MouJiangJiRuntime Get();
}

internal interface ICharacterJiangJiMouCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
