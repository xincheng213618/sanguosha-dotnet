#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenhePoisonEntries
{
    internal void AssertWenhePoisonFireAttackPayment(CardUseFrame use) => _resolve().AssertWenhePoisonFireAttackPayment(use);
    internal void AssertWenhePoisonMovementWindow(CardsMovedTriggerWindowFrame frame) => _resolve().AssertWenhePoisonMovementWindow(frame);
    internal void BeginWenhePoisonDying(CardsMovedTriggerWindowFrame frame, WenhePoisonDeparture departure) => _resolve().BeginWenhePoisonDying(frame, departure);
    internal void CaptureMovementBatchStartWenhePoisonRound(CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartWenhePoisonRound(batch);
    internal CardMovementBatchContext CaptureWenhePoisonDepartures(CardMovementBatchContext batch, int? poisonRound) => _resolve().CaptureWenhePoisonDepartures(batch, poisonRound);
    internal int? CaptureWenhePoisonRound() => _resolve().CaptureWenhePoisonRound();
    internal void CaptureWenhePoisonUsePayment(CardMovementBatchContext batch) => _resolve().CaptureWenhePoisonUsePayment(batch);
    internal CardMovementBatchContext CompleteMovementBatchCaptureWenhePoisonDepartures(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureWenhePoisonDepartures(batch, completed);
    internal CardMovementBatchContext CompleteMovementBatchCaptureWenhePoisonUsePayment(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureWenhePoisonUsePayment(batch, completed);
    internal void ContinueWenhePoisonFireAttackPayment(long useId) => _resolve().ContinueWenhePoisonFireAttackPayment(useId);
    internal CardKind EffectiveConversionInputKind(CharacterState owner, Card card) => _resolve().EffectiveConversionInputKind(owner, card);
    internal bool HasIssuedWenhePoisonPaymentBatch(CardUseFrame use, CardMovementBatchContext batch) => _resolve().HasIssuedWenhePoisonPaymentBatch(use, batch);
    internal bool HasWenhePoisonDepartures(CardMovementBatchContext batch) => _resolve().HasWenhePoisonDepartures(batch);
    internal bool HasWenhePoisonUsePaymentTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => _resolve().HasWenhePoisonUsePaymentTail(use, continuation);
    internal bool IsCurrentWenhePoisonDying() => _resolve().IsCurrentWenhePoisonDying();
    internal bool IsOriginalDyingSuspendedByWenhePoison(DyingFrame original) => _resolve().IsOriginalDyingSuspendedByWenhePoison(original);
    internal bool IsPendingWenhePoisonWindow(CardsMovedTriggerWindowFrame frame) => _resolve().IsPendingWenhePoisonWindow(frame);
    internal bool IsWenhePhysicalPoison(CharacterState owner, Card card) => _resolve().IsWenhePhysicalPoison(owner, card);
    internal bool IsWenhePoisonDyingChild(ResolutionFrame? parent, DyingFrame child) => _resolve().IsWenhePoisonDyingChild(parent, child);
    internal bool IsWenhePoisonFireAttackMovement(CardUseFrame parent, CardsMovedTriggerWindowFrame child) => _resolve().IsWenhePoisonFireAttackMovement(parent, child);
    internal bool IsWenhePoisonHealthChild(ResolutionFrame? parent, HpChangedTriggerWindowFrame child) => _resolve().IsWenhePoisonHealthChild(parent, child);
    internal bool IsWenhePoisonUsePaymentBatch(CardUseFrame use, CardMovementBatchContext batch) => _resolve().IsWenhePoisonUsePaymentBatch(use, batch);
    internal bool IsWenhePoisonUsePaymentOrigin(CardUseFrame use, CardMovementBatchContext batch) => _resolve().IsWenhePoisonUsePaymentOrigin(use, batch);
    internal CardSnapshot ProjectWenhePoisonHandCard(CharacterState owner, Card card, CardSnapshot snapshot) => _resolve().ProjectWenhePoisonHandCard(owner, card, snapshot);
    internal void ReturnWenhePoisonDying(GameEngine.DyingCompletionReceipt returned) => _resolve().ReturnWenhePoisonDying(returned);
    internal bool ReturnWenhePoisonFireAttackMovement(CardsMovedTriggerWindowFrame child) => _resolve().ReturnWenhePoisonFireAttackMovement(child);
    internal bool TryAdvanceWenhePoisonLoss(CardsMovedTriggerWindowFrame supplied) => _resolve().TryAdvanceWenhePoisonLoss(supplied);
    internal bool TryBeginWenhePoisonFireAttackPayment(CardUseFrame use, Card revealed, Card cost, int sequenceBefore) => _resolve().TryBeginWenhePoisonFireAttackPayment(use, revealed, cost, sequenceBefore);
}
