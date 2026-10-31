#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreTableSeatQueriesEntries
{
    internal int FindNextAliveSeat(int fromSeat) => _resolve().FindNextAliveSeat(fromSeat);
    internal bool IsValidPlayerSeat(int seat) => _resolve().IsValidPlayerSeat(seat);
    internal void ValidatePlayerSeat(int seat, string parameterName) => _resolve().ValidatePlayerSeat(seat, parameterName);
}
