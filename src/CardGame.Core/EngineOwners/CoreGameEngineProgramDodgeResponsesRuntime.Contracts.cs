#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramDodgeResponsesCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}
