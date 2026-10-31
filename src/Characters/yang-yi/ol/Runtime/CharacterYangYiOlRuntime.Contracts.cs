#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterYangYiOlCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
