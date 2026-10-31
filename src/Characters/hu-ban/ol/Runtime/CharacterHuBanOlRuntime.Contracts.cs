#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuBanOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
