#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardUsePaymentsEntries
{
    internal void CaptureSlashUsePaymentBatch(CardMovementBatchContext batch) => _resolve().CaptureSlashUsePaymentBatch(batch);
    internal bool CardUsePaymentMaterialsMatch(CardUseFrame use, CardMovementBatchContext batch) => _resolve().CardUsePaymentMaterialsMatch(use, batch);
    internal CardMovementBatchContext CompleteMovementBatchCaptureSlashUsePaymentBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureSlashUsePaymentBatch(batch, completed);
    internal bool HasSharedCardUsePaymentProcessing(IReadOnlyList<Card> processing) => _resolve().HasSharedCardUsePaymentProcessing(processing);
    internal bool IsCardUsePaymentBatch(CardUseFrame use, CardMovementBatchContext batch) => _resolve().IsCardUsePaymentBatch(use, batch);
    internal bool IsPaidCardUseMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement) => _resolve().IsPaidCardUseMovementReturn(use, movement);
    internal bool IsPaidDestroyedGeneralWeaponUseMaterial(long useId, int cardId) => _resolve().IsPaidDestroyedGeneralWeaponUseMaterial(useId, cardId);
    internal void PaySlashUseMaterials(IReadOnlyList<Card> cards, int providerSeat, CardLocation? sourceLocation) => _resolve().PaySlashUseMaterials(cards, providerSeat, sourceLocation);
}
