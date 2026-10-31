#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreKuangfuCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
