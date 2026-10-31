#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreUnequalHandHpEndingCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
