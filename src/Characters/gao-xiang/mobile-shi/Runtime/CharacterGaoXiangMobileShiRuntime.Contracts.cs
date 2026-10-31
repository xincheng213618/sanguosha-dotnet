#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterGaoXiangMobileShiCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
