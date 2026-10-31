#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterYuJinMouMobileCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
