#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramCardUsePaymentsEntries
{
    internal bool HasPaidCardUseMovementTail(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => _resolve().HasPaidCardUseMovementTail(use, continuation);
    internal bool ObservesPaidCardUseChildren(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => _resolve().ObservesPaidCardUseChildren(use, continuation);
}
