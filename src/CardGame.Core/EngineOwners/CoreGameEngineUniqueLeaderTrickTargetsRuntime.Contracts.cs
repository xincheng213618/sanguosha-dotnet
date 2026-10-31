#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreUniqueLeaderTrickTargetsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
