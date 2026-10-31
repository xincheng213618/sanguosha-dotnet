#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramCardUsePaymentsEntries
{
    internal bool HasPaidCardUseMovementTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => _resolve().HasPaidCardUseMovementTail(use, continuation);
    internal bool ObservesPaidCardUseChildren(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => _resolve().ObservesPaidCardUseChildren(use, continuation);
}
