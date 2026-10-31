#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICharacterWuYiMobileCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
