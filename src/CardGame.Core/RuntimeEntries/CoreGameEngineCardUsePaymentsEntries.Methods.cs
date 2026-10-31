#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardUsePaymentsEntries
{
    internal void CaptureSlashUsePaymentBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureSlashUsePaymentBatch(batch);
    internal bool CardUsePaymentMaterialsMatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CardUsePaymentMaterialsMatch(use, batch);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureSlashUsePaymentBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureSlashUsePaymentBatch(batch, completed);
    internal bool HasSharedCardUsePaymentProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().HasSharedCardUsePaymentProcessing(processing);
    internal bool IsCardUsePaymentBatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsCardUsePaymentBatch(use, batch);
    internal bool IsPaidCardUseMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement) => _resolve().IsPaidCardUseMovementReturn(use, movement);
    internal bool IsPaidDestroyedGeneralWeaponUseMaterial(long useId, int cardId) => _resolve().IsPaidDestroyedGeneralWeaponUseMaterial(useId, cardId);
    internal void PaySlashUseMaterials(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, int providerSeat, global::CardGame.Core.CardLocation? sourceLocation) => _resolve().PaySlashUseMaterials(cards, providerSeat, sourceLocation);
}
