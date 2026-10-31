#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreSharedSlashOffersCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
