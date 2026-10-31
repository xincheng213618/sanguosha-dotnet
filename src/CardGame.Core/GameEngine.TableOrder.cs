namespace CardGame.Core;
/// <summary>Seat identifies a participant; this public fact changes only their position on the table.</summary>
public sealed record TableSeatMovedEvent(int Seat, int PreviousSeat, int NextSeat, long ParentFrameId) : IGameEvent;
