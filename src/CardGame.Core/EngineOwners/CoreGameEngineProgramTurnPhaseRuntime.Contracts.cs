#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramTurnPhaseCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetSeatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}
