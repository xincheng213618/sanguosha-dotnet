#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreHellDizangProgramsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
