#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDrawFundedDistinctBasicResponsesCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
