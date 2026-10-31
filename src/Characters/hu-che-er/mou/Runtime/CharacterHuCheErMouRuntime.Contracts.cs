#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuCheErMouMouHuCheErOperations : IProgramRuntimeOperations
{
    MouHuCheErRuntime Get();
}

internal interface ICharacterHuCheErMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterHuCheErMouCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
