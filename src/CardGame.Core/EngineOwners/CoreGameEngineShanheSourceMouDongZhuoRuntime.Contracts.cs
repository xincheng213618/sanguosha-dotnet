#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheSourceMouDongZhuoCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
