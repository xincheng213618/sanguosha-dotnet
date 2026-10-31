#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreShownEntityTurnPoliciesCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
