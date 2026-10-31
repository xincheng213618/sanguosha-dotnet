#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheBossCancelledMaterialsEntries
{
    internal void CaptureShanheBossCancelledMaterials() => _resolve().CaptureShanheBossCancelledMaterials();
    internal bool TryDescribeShanheBossCancelledMaterial(int cardId, CardMovementRecord entry, CardMoveReason reason, scoped out ShanheBossMaterialCancelledEvent cancelled) => _resolve().TryDescribeShanheBossCancelledMaterial(cardId, entry, reason, out cancelled);
}
