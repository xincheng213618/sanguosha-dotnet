#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreEndingHistoricalUsesCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
