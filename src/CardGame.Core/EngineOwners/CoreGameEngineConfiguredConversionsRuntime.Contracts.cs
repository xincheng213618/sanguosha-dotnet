#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreConfiguredConversionsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
