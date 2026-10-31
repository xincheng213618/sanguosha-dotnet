#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuangQuanMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterHuangQuanMouBoundaryYiJiOperations : IProgramRuntimeOperations
{
    BoundaryYiJiRuntime Get();
}

internal interface ICharacterHuangQuanMouMouHuangQuanOperations : IProgramRuntimeOperations
{
    MouHuangQuanRuntime Get();
}
