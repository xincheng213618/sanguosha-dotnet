#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePublicPilePreparationCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
