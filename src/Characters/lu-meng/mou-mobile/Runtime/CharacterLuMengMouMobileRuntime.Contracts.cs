#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterLuMengMouMobileCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
