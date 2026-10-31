#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreFrozenHandRosterCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
