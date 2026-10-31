#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreAssistedPhysicalCardsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
