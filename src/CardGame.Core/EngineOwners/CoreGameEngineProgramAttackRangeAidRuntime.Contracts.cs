#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramAttackRangeAidCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
