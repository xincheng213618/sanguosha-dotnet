#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDyingSuitsPeachProgramsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
