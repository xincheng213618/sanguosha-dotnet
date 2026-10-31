#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterXiahouShiMouMobileCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
