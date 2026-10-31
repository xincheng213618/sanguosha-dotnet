#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramCardUseNullificationCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
