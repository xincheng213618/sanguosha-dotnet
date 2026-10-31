#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICharacterJuShouMouCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterJuShouMouMouJuShouOperations : IProgramRuntimeOperations
{
    MouJuShouRuntime Get();
}
