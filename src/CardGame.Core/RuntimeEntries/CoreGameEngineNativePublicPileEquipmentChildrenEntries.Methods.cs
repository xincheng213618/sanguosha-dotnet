#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativePublicPileEquipmentChildrenEntries
{
    internal void AssertNativePublicPileEquipmentChildren() => _resolve().AssertNativePublicPileEquipmentChildren();
    internal RecoveryPaidCardUseContinuation FreezeNativePublicPileEquipmentChildren(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => _resolve().FreezeNativePublicPileEquipmentChildren(use, continuation);
    internal bool HasNativePublicPileEquipmentTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => _resolve().HasNativePublicPileEquipmentTail(use, continuation);
    internal bool IsNativePublicPileEquipmentBatch(CardUseFrame use, CardMovementBatchContext batch) => _resolve().IsNativePublicPileEquipmentBatch(use, batch);
    internal bool IsNativePublicPileEquipmentOwner(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => _resolve().IsNativePublicPileEquipmentOwner(use, continuation);
    internal bool NativePublicPileEquipmentMovementsMatch(CardUseFrame use, RecoveryPaidCardUseContinuation continuation, CardMovementBatchContext batch, IReadOnlyList<CardMovementBatchContext> issued) => _resolve().NativePublicPileEquipmentMovementsMatch(use, continuation, batch, issued);
}
