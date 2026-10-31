#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDynamicDiscardDamagePaymentsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}
