#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterTaoQianOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
