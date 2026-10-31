#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTableOrderRuntime
{
    internal IReadOnlyList<int> CurrentTableOrder()
    {
        var movements = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TableSeatMovedEvent>();
        var last = movements.LastOrDefault();
        if (_coreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess._cachedTableOrder is not null && ReferenceEquals(last, _coreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess._cachedTableMovement))
            return _coreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess._cachedTableOrder;
        var order = Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).ToList();
        foreach (var movement in movements)
        {
            order.Remove(movement.Seat);
            order.Insert(order.IndexOf(movement.PreviousSeat) + 1, movement.Seat);
        }

        _coreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess._cachedTableMovement = last;
        return _coreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess._cachedTableOrder = Array.AsReadOnly(order.ToArray());
    }

    internal int TableStepSeat(int from, int offset)
    {
        _coreTableSeatQueriesOperations.ValidatePlayerSeat(from, "from");
        var order = CurrentTableOrder();
        var index = IndexOfTableSeat(order, from);
        return order[((index + offset) % order.Count + order.Count) % order.Count];
    }

    internal static int IndexOfTableSeat(IReadOnlyList<int> order, int seat)
    {
        for (var index = 0; index < order.Count; index++)
            if (order[index] == seat)
                return index;
        throw new InvalidOperationException("The public table order lost a participant.");
    }

    internal int RelativeTableOrder(int from, int target)
    {
        _coreTableSeatQueriesOperations.ValidatePlayerSeat(from, "from");
        _coreTableSeatQueriesOperations.ValidatePlayerSeat(target, "target");
        var order = CurrentTableOrder();
        return (IndexOfTableSeat(order, target) - IndexOfTableSeat(order, from) + order.Count) % order.Count;
    }

    internal int GetAdjacentLivingSeat(int from, int step)
    {
        if (step is not (1 or -1))
            throw new ArgumentOutOfRangeException("step");
        for (var offset = 1; offset <= _coreMatchDefinitionPlayerCountAccess._playerCount; offset++)
        {
            var seat = TableStepSeat(from, step * offset);
            if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                return seat;
        }

        return from;
    }

    internal void MoveTableSeatBetween(int owner, int previous, int next, long parentFrameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != parentFrameId || frame.OwnerSeat != owner || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(previous) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(next) || previous == next || previous == owner || next == owner || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreMatchParticipantsPlayersAccess._players[previous].IsAlive || !_coreMatchParticipantsPlayersAccess._players[next].IsAlive)
            throw new InvalidOperationException("A table move requires its owning live program frame and two other living neighbors.");
        var remaining = CurrentTableOrder().Where(seat => seat != owner && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).ToArray();
        var previousIndex = Array.IndexOf(remaining, previous);
        if (remaining[(previousIndex + 1) % remaining.Length] != next)
            throw new InvalidOperationException("The selected table gap is no longer adjacent.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TableSeatMovedEvent(owner, previous, next, parentFrameId));
    }
}
