#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramCardPoliciesCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}
