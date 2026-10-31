#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCuiYanOlCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}
