#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreLostHpOwnedGiftsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
