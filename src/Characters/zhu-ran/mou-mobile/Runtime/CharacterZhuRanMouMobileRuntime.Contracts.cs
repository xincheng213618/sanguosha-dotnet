#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhuRanMouMobileCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterZhuRanMouMobileMouZhuRanOperations : IProgramRuntimeOperations
{
    MouZhuRanRuntime Get();
}
