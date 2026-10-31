#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeRuleQueriesEntries
{
    internal int CountAliveSeatsOnPath(int sourceSeat, int targetSeat, int step) => _resolve().CountAliveSeatsOnPath(sourceSeat, targetSeat, step);
    internal int GetAliveSeatDistance(int sourceSeat, int targetSeat) => _resolve().GetAliveSeatDistance(sourceSeat, targetSeat);
    internal int GetHandLimit(CharacterState player) => _resolve().GetHandLimit(player);
}
