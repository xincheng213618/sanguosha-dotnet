#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreHalfHandTargetSupportCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
