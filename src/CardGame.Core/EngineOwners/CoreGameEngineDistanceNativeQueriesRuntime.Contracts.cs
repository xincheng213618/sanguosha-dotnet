#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreDistanceNativeQueriesCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
