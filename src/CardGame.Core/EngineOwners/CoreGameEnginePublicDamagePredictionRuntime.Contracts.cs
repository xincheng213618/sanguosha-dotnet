#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePublicDamagePredictionCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICorePublicDamagePredictionCoreWushuangModeProjectionOperations : IProgramRuntimeOperations
{
    bool IsWushuangMode { get; }
}

internal interface ICorePublicDamagePredictionCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
