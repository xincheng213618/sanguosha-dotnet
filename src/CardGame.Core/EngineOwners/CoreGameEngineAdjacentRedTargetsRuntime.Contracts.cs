#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreAdjacentRedTargetsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
