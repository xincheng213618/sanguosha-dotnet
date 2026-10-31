#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterChenGongMouMobileCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
