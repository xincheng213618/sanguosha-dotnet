#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramCardUseRolesCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
