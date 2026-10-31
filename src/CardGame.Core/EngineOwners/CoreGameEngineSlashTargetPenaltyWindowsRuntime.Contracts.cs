#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreSlashTargetPenaltyWindowsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
