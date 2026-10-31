#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActionDiscardColorsEntries
{
    internal bool? CaptureActionColor(IReadOnlyList<CardActionCost> costs, Suit? effectiveSuit = default) => _resolve().CaptureActionColor(costs, effectiveSuit);
    internal void CaptureActionDiscardFact(long batchId, int turn, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureActionDiscardFact(batchId, turn, movements);
    internal void CaptureMovementBatchFactsCaptureActionDiscardFact(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureActionDiscardFact(batch, movements);
    internal bool? CapturePhysicalCardColor(int seat, Card card) => _resolve().CapturePhysicalCardColor(seat, card);
    internal bool TracksActionDiscardColor { get => _resolve().TracksActionDiscardColor; }
}
