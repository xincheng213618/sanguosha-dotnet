#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreCurrentCardEnhancementsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
