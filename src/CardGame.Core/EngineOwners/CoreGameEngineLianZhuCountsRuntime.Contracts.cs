#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreLianZhuCountsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
