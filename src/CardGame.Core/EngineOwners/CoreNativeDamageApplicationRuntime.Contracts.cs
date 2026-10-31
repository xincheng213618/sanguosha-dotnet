#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeDamageApplicationCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICoreNativeDamageApplicationCoreProgramDamageHooksOperations : IProgramRuntimeOperations
{
    ProgramDamageHooks DamageHooks { get; }
}
