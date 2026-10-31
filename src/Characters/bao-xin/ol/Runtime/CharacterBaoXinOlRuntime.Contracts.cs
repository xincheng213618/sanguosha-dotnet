#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterBaoXinOlCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
