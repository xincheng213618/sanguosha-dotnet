#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCaiMaoOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
