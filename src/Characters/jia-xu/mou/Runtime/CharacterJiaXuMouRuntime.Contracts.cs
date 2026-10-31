#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterJiaXuMouCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterJiaXuMouMouJiaXuOperations : IProgramRuntimeOperations
{
    MouJiaXuRuntime Get();
}
