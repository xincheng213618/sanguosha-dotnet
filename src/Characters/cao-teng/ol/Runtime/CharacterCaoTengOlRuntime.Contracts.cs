#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCaoTengOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
