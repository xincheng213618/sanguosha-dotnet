#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterXuShengMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterXuShengMouMouXuShengOperations : IProgramRuntimeOperations
{
    MouXuShengRuntime Get();
}
