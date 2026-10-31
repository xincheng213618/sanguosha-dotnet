namespace CardGame.Core;
/// <summary>Seat identifies a participant; this public fact changes only their position on the table.</summary>
public sealed record TableSeatMovedEvent(int Seat, int PreviousSeat, int NextSeat, long ParentFrameId) : IGameEvent;
public sealed partial class GameEngine
{
    private ref global::CardGame.Core.TableSeatMovedEvent? _cachedTableMovement => ref _tableOrder.ReferenceToCachedTableMovement;
    private ref global::System.Collections.Generic.IReadOnlyList<int>? _cachedTableOrder => ref _tableOrder.ReferenceToCachedTableOrder;
}
