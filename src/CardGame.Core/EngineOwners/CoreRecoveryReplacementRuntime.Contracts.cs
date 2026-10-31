#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreRecoveryReplacementCoreProgramCardUsePaymentsOperations : IProgramRuntimeOperations
{
    bool ObservesPaidCardUseChildren(CardUseFrame use, RecoveryPaidCardUseContinuation continuation);
    ProgramCardUsePayments CardUsePayments { get; }
}

internal interface ICoreRecoveryReplacementCoreProgramRuntimeHooksOperations : IProgramRuntimeOperations
{
    ProgramRuntimeHooks RuntimeHooks { get; }
}
