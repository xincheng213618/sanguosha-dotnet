#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDualColorDuelProgramsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
