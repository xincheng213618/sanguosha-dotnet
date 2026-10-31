#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreShanheS5GongsunZanCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
}

internal interface ICoreShanheS5GongsunZanCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
