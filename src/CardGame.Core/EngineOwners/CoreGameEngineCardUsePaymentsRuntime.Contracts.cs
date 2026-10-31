#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreCardUsePaymentsCoreProgramCardUsePaymentsOperations : IProgramRuntimeOperations
{
    ProgramCardUsePayments CardUsePayments { get; }

    bool HasPaidCardUseMovementTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation);
}
