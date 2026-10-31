#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeDyingCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
