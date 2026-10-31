#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuangYueyingOlMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
