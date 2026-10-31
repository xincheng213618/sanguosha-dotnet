#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhouYuMouMouZhouYuOperations : IProgramRuntimeOperations
{
    MouZhouYuRuntime Get();
}

internal interface ICharacterZhouYuMouCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
