#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterLiuXieMouMouLiuXieOperations : IProgramRuntimeOperations
{
    MouLiuXieRuntime Get();
}

internal interface ICharacterLiuXieMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
