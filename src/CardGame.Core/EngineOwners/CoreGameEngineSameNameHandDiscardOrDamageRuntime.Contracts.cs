#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreSameNameHandDiscardOrDamageCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
