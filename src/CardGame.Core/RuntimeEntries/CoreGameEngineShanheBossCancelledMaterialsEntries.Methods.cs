#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheBossCancelledMaterialsEntries
{
    internal void CaptureShanheBossCancelledMaterials() => _resolve().CaptureShanheBossCancelledMaterials();
    internal bool TryDescribeShanheBossCancelledMaterial(int cardId, global::CardGame.Core.CardMovementRecord entry, global::CardGame.Core.CardMoveReason reason, scoped out global::CardGame.Core.ShanheBossMaterialCancelledEvent cancelled) => _resolve().TryDescribeShanheBossCancelledMaterial(cardId, entry, reason, out cancelled);
}
