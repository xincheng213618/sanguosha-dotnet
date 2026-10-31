#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterDengAiMouOlCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
