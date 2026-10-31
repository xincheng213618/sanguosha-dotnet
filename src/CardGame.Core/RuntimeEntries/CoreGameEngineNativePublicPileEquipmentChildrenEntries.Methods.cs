#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativePublicPileEquipmentChildrenEntries
{
    internal void AssertNativePublicPileEquipmentChildren() => _resolve().AssertNativePublicPileEquipmentChildren();
    internal global::CardGame.Core.RecoveryPaidCardUseContinuation FreezeNativePublicPileEquipmentChildren(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => _resolve().FreezeNativePublicPileEquipmentChildren(use, continuation);
    internal bool HasNativePublicPileEquipmentTail(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => _resolve().HasNativePublicPileEquipmentTail(use, continuation);
    internal bool IsNativePublicPileEquipmentBatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsNativePublicPileEquipmentBatch(use, batch);
    internal bool IsNativePublicPileEquipmentOwner(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => _resolve().IsNativePublicPileEquipmentOwner(use, continuation);
    internal bool NativePublicPileEquipmentMovementsMatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation, global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementBatchContext> issued) => _resolve().NativePublicPileEquipmentMovementsMatch(use, continuation, batch, issued);
}
