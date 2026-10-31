#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreColdDamageCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
