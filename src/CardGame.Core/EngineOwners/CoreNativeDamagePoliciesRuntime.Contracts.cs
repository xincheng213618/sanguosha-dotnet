#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeDamagePoliciesCoreProgramDamageHooksOperations : IProgramRuntimeOperations
{
    ProgramDamageHooks DamageHooks { get; }
}

internal interface ICoreNativeDamagePoliciesCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}
