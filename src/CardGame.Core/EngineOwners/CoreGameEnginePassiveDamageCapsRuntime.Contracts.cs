#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePassiveDamageCapsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
}
