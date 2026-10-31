#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramSlashRequestCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
