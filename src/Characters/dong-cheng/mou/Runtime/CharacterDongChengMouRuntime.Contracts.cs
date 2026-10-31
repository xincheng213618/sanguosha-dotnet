#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterDongChengMouCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterDongChengMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterDongChengMouMouDongChengOperations : IProgramRuntimeOperations
{
    MouDongChengRuntime Get();
}
