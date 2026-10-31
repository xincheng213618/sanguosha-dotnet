#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuangGaiMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterHuangGaiMouMouHuangGaiOperations : IProgramRuntimeOperations
{
    MouHuangGaiRuntime Get();
}
