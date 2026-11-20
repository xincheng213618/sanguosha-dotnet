#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterBuZhiOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
