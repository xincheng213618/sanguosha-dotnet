#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterKeBiNengOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
