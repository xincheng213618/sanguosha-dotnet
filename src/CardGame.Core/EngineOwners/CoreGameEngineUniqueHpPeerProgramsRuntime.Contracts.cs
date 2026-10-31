#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreUniqueHpPeerProgramsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
