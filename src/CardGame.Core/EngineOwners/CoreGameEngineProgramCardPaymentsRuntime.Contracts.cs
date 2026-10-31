#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramCardPaymentsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
}

internal interface ICoreProgramCardPaymentsProgramPaidCardOperations : IProgramRuntimeOperations
{
    ProgramPaidCardRuntime Get();
}
