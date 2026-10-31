#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActionDiscardColorsEntries
{
    internal bool? CaptureActionColor(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.Suit? effectiveSuit = default) => _resolve().CaptureActionColor(costs, effectiveSuit);
    internal void CaptureActionDiscardFact(long batchId, int turn, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureActionDiscardFact(batchId, turn, movements);
    internal void CaptureMovementBatchFactsCaptureActionDiscardFact(global::CardGame.Core.CardMovementBatchBuilder batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureActionDiscardFact(batch, movements);
    internal bool? CapturePhysicalCardColor(int seat, global::CardGame.Core.Card card) => _resolve().CapturePhysicalCardColor(seat, card);
    internal bool TracksActionDiscardColor { get => _resolve().TracksActionDiscardColor; }
}
