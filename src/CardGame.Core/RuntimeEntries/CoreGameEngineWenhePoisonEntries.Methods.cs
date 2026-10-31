#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenhePoisonEntries
{
    internal void AssertWenhePoisonFireAttackPayment(global::CardGame.Core.CardUseFrame use) => _resolve().AssertWenhePoisonFireAttackPayment(use);
    internal void AssertWenhePoisonMovementWindow(global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => _resolve().AssertWenhePoisonMovementWindow(frame);
    internal void BeginWenhePoisonDying(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.WenhePoisonDeparture departure) => _resolve().BeginWenhePoisonDying(frame, departure);
    internal void CaptureMovementBatchStartWenhePoisonRound(global::CardGame.Core.CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartWenhePoisonRound(batch);
    internal global::CardGame.Core.CardMovementBatchContext CaptureWenhePoisonDepartures(global::CardGame.Core.CardMovementBatchContext batch, int? poisonRound) => _resolve().CaptureWenhePoisonDepartures(batch, poisonRound);
    internal int? CaptureWenhePoisonRound() => _resolve().CaptureWenhePoisonRound();
    internal void CaptureWenhePoisonUsePayment(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureWenhePoisonUsePayment(batch);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureWenhePoisonDepartures(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureWenhePoisonDepartures(batch, completed);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureWenhePoisonUsePayment(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureWenhePoisonUsePayment(batch, completed);
    internal void ContinueWenhePoisonFireAttackPayment(long useId) => _resolve().ContinueWenhePoisonFireAttackPayment(useId);
    internal global::CardGame.Core.CardKind EffectiveConversionInputKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().EffectiveConversionInputKind(owner, card);
    internal bool HasExactWenhePoisonResponseContinuation() => _resolve().HasExactWenhePoisonResponseContinuation();
    internal bool HasIssuedWenhePoisonPaymentBatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().HasIssuedWenhePoisonPaymentBatch(use, batch);
    internal bool HasWenhePoisonDepartures(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().HasWenhePoisonDepartures(batch);
    internal bool HasWenhePoisonFireAttackPrompt() => _resolve().HasWenhePoisonFireAttackPrompt();
    internal bool HasWenhePoisonUsePaymentTail(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => _resolve().HasWenhePoisonUsePaymentTail(use, continuation);
    internal bool IsCurrentWenhePoisonDying() => _resolve().IsCurrentWenhePoisonDying();
    internal bool IsOriginalDyingSuspendedByWenhePoison(global::CardGame.Core.DyingFrame original) => _resolve().IsOriginalDyingSuspendedByWenhePoison(original);
    internal bool IsPendingWenhePoisonWindow(global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => _resolve().IsPendingWenhePoisonWindow(frame);
    internal bool IsWenhePhysicalPoison(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().IsWenhePhysicalPoison(owner, card);
    internal bool IsWenhePoisonColorFireAttackMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement) => _resolve().IsWenhePoisonColorFireAttackMovement(use, movement);
    internal bool IsWenhePoisonDyingChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.DyingFrame child) => _resolve().IsWenhePoisonDyingChild(parent, child);
    internal bool IsWenhePoisonFireAttackMovement(global::CardGame.Core.CardUseFrame parent, global::CardGame.Core.CardsMovedTriggerWindowFrame child) => _resolve().IsWenhePoisonFireAttackMovement(parent, child);
    internal bool IsWenhePoisonHealthChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame child) => _resolve().IsWenhePoisonHealthChild(parent, child);
    internal bool IsWenhePoisonUsePaymentBatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsWenhePoisonUsePaymentBatch(use, batch);
    internal bool IsWenhePoisonUsePaymentOrigin(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsWenhePoisonUsePaymentOrigin(use, batch);
    internal global::CardGame.Core.CardSnapshot ProjectWenhePoisonHandCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardSnapshot snapshot) => _resolve().ProjectWenhePoisonHandCard(owner, card, snapshot);
    internal void ReturnWenhePoisonDying(global::CardGame.Core.GameEngine.DyingCompletionReceipt returned) => _resolve().ReturnWenhePoisonDying(returned);
    internal bool ReturnWenhePoisonFireAttackMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => _resolve().ReturnWenhePoisonFireAttackMovement(child);
    internal bool TryAdvanceWenhePoisonLoss(global::CardGame.Core.CardsMovedTriggerWindowFrame supplied) => _resolve().TryAdvanceWenhePoisonLoss(supplied);
    internal bool TryBeginWenhePoisonFireAttackPayment(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card revealed, global::CardGame.Core.Card cost, int sequenceBefore) => _resolve().TryBeginWenhePoisonFireAttackPayment(use, revealed, cost, sequenceBefore);
}
