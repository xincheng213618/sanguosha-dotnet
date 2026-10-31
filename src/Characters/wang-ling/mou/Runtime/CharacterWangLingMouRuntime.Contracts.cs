#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterWangLingMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
