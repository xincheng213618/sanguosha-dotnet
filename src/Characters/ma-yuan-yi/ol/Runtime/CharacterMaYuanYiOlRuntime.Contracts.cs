#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterMaYuanYiOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
