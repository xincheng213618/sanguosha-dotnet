#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardUsePaymentsEntries
{
    internal void CaptureNativeUsePaymentBatch(CardMovementBatchContext batch) => _resolve().CaptureNativeUsePaymentBatch(batch);
    internal bool CardUsePaymentMaterialsMatch(CardUseFrame use, CardMovementBatchContext batch) => _resolve().CardUsePaymentMaterialsMatch(use, batch);
    internal CardMovementBatchContext CompleteMovementBatchCaptureNativeUsePaymentBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureNativeUsePaymentBatch(batch, completed);
    internal bool HasSharedCardUsePaymentProcessing(IReadOnlyList<Card> processing) => _resolve().HasSharedCardUsePaymentProcessing(processing);
    internal bool IsCardUsePaymentBatch(CardUseFrame use, CardMovementBatchContext batch) => _resolve().IsCardUsePaymentBatch(use, batch);
    internal bool IsPaidCardUseMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement) => _resolve().IsPaidCardUseMovementReturn(use, movement);
    internal void PaySlashUseMaterials(IReadOnlyList<Card> cards, int providerSeat, CardLocation? sourceLocation) => _resolve().PaySlashUseMaterials(cards, providerSeat, sourceLocation);
}
