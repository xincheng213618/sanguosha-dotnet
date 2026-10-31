#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramCardUsePaymentsRuntime
{
    internal ProgramCardUsePayments CardUsePayments => _cardUsePaymentsOperations._cardUsePayments ??= new(CoreProgramCardUsePaymentsDefinitions.CardUsePaymentMetadata.Value, row => _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler(row.Method, row.DelegateType));

    internal bool HasPaidCardUseMovementTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => CardUsePayments.AllowsTail(ProgramCardUsePaymentStage.LeadingTail, use, continuation) || continuation.Kind == RecoveryPaidCardUseKind.CommittedSlash || CardUsePayments.AllowsTail(ProgramCardUsePaymentStage.TrailingTail, use, continuation);
    internal bool ObservesPaidCardUseChildren(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => continuation.Kind == RecoveryPaidCardUseKind.CommittedSlash && CoreGameEngineCardUsePaymentsRuntime.OwnsGenericSlashPaymentTail(use) || CardUsePayments.AllowsTail(ProgramCardUsePaymentStage.LeadingTail, use, continuation) || CardUsePayments.AllowsTail(ProgramCardUsePaymentStage.TrailingTail, use, continuation);
}
