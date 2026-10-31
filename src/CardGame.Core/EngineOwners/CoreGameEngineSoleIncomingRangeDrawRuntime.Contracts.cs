#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreSoleIncomingRangeDrawCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
}
