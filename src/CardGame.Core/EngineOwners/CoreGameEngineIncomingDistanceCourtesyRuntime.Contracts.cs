#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreIncomingDistanceCourtesyCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
