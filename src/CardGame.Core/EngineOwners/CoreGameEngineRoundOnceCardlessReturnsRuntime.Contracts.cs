#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreRoundOnceCardlessReturnsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
