#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTableOrderEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<int> CurrentTableOrder() => _resolve().CurrentTableOrder();
    internal int GetAdjacentLivingSeat(int from, int step) => _resolve().GetAdjacentLivingSeat(from, step);
    internal void MoveTableSeatBetween(int owner, int previous, int next, long parentFrameId) => _resolve().MoveTableSeatBetween(owner, previous, next, parentFrameId);
    internal int RelativeTableOrder(int from, int target) => _resolve().RelativeTableOrder(from, target);
    internal int TableStepSeat(int from, int offset) => _resolve().TableStepSeat(from, offset);
}
