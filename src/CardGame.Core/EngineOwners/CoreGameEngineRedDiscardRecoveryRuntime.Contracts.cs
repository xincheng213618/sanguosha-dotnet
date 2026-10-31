#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreRedDiscardRecoveryCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
