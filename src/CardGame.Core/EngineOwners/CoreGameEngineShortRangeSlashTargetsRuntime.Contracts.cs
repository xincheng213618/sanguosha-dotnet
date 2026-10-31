#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreShortRangeSlashTargetsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreShortRangeSlashTargetsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
