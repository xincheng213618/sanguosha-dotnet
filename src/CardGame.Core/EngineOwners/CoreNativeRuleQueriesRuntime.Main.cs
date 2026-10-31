#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeRuleQueriesRuntime
{
    internal int GetSeatDistance(int sourceSeat, int targetSeat)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat))
        {
            throw new ArgumentOutOfRangeException("sourceSeat", sourceSeat, "The source seat is outside this match.");
        }

        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat))
        {
            throw new ArgumentOutOfRangeException("targetSeat", targetSeat, "The target seat is outside this match.");
        }

        var directDistance = _coreTableOrderOperations.RelativeTableOrder(sourceSeat, targetSeat);
        return Math.Min(directDistance, _coreMatchDefinitionPlayerCountAccess._playerCount - directDistance);
    }

    internal int GetCombatDistance(int sourceSeat, int targetSeat)
    {
        _coreTableSeatQueriesOperations.ValidatePlayerSeat(sourceSeat, "sourceSeat");
        _coreTableSeatQueriesOperations.ValidatePlayerSeat(targetSeat, "targetSeat");
        if (sourceSeat == targetSeat)
        {
            return 0;
        }

        return global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateDistance(_coreMatchParticipantsPlayersAccess._players[sourceSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat]));
    }

    internal int GetAttackRange(int sourceSeat)
    {
        _coreTableSeatQueriesOperations.ValidatePlayerSeat(sourceSeat, "sourceSeat");
        return global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateAttackRange(_coreMatchParticipantsPlayersAccess._players[sourceSeat]));
    }

    internal int GetAliveSeatDistance(int sourceSeat, int targetSeat)
    {
        var clockwise = CountAliveSeatsOnPath(sourceSeat, targetSeat, step: 1);
        var counterClockwise = CountAliveSeatsOnPath(sourceSeat, targetSeat, step: -1);
        return Math.Min(clockwise, counterClockwise);
    }

    internal int CountAliveSeatsOnPath(int sourceSeat, int targetSeat, int step)
    {
        var distance = 0;
        var seat = sourceSeat;
        do
        {
            seat = _coreTableOrderOperations.TableStepSeat(seat, step);
            if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || seat == targetSeat)
            {
                distance++;
            }
        }
        while (seat != targetSeat);
        return distance;
    }

    internal int GetHandLimit(CharacterState player) => global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreRuleQueriesOperations.EvaluateHandLimit(player));
}
