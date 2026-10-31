#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreGainGiftProgramsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
