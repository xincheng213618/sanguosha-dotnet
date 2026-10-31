#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreEndRefillNextTurnCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
