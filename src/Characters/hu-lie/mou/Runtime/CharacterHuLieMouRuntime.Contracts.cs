#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuLieMouCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterHuLieMouMouHuLieOperations : IProgramRuntimeOperations
{
    MouHuLieRuntime Get();
}

internal interface ICharacterHuLieMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
