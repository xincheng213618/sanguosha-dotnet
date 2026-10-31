#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterTengFangLanOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
