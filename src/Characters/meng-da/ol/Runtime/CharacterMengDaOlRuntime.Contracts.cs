#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterMengDaOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
