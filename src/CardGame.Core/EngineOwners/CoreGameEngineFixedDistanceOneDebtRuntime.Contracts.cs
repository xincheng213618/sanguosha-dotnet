#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreFixedDistanceOneDebtCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
