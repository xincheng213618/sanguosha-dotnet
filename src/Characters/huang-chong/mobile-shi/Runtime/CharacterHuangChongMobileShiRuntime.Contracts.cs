#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuangChongMobileShiCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
