#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterBeiMiHuOlCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
