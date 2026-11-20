#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreTableSeatQueriesRuntime
{
    internal bool IsValidPlayerSeat(int seat) => seat >= 0 && seat < _coreMatchDefinitionPlayerCountAccess._playerCount;
    internal void ValidatePlayerSeat(int seat, string parameterName)
    {
        if (!IsValidPlayerSeat(seat))
        {
            throw new ArgumentOutOfRangeException(parameterName, seat, $"Seat must be between 0 and {(_coreMatchDefinitionPlayerCountAccess._playerCount - 1)}.");
        }
    }

    internal int FindNextAliveSeat(int fromSeat) => FindNextSeat(fromSeat, includeResting: false);

    internal int FindNextNormalTurnSeat(int fromSeat) => FindNextSeat(fromSeat, includeResting: true);

    private int FindNextSeat(int fromSeat, bool includeResting)
    {
        for (var offset = 1; offset <= _coreMatchDefinitionPlayerCountAccess._playerCount; offset++)
        {
            var seat = _coreTableOrderOperations.TableStepSeat(fromSeat, offset);
            var player = _coreMatchParticipantsPlayersAccess._players[seat];
            if (player.IsAlive || (includeResting && player.IsResting))
            {
                return seat;
            }
        }

        return fromSeat;
    }
}
