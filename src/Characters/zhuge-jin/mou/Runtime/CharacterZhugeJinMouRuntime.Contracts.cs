#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhugeJinMouMouZhugeJinOperations : IProgramRuntimeOperations
{
    MouZhugeJinRuntime Get();
}

internal interface ICharacterZhugeJinMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
