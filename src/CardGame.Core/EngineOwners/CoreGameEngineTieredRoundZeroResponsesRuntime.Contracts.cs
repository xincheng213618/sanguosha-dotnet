#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreTieredRoundZeroResponsesCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
