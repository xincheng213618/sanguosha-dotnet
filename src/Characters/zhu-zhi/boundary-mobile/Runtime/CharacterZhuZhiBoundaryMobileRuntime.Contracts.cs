#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhuZhiBoundaryMobileCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
