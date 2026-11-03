namespace CardGame.Core;

internal interface IAdvancingRuleFacts : IProgramSelectionQuery
{
    IEnumerable<IGameEvent> Facts { get; }
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class AdvancingRuleFacts(
    EngineStates.Contracts.ICoreGameEngineRuleProgressionRuleProgressionAccess state) : IAdvancingRuleFacts
{
    public IEnumerable<IGameEvent> Facts => state._ruleProgression?.AdvancingFacts ?? [];
}

internal interface INativeGiveScopeQueries : IProgramSelectionQuery
{
    long? CurrentCutoff(int turn, int turnOwner);
}

public sealed partial record TurnEndedEvent
{
    private long? _nativeGiveHistoryCutoff;
    private bool _nativeGiveHistoryFrozen;
    internal long? NativeGiveHistoryCutoff => _nativeGiveHistoryCutoff;
    public bool Equals(TurnEndedEvent? other) => other is not null &&
        TurnNumber == other.TurnNumber && ActorSeat == other.ActorSeat;
    public override int GetHashCode() => HashCode.Combine(typeof(TurnEndedEvent), TurnNumber, ActorSeat);
    internal void StampNativeGiveHistoryCutoff(long cursor)
    {
        if (_nativeGiveHistoryFrozen || _nativeGiveHistoryCutoff is not null || cursor < 0)
            throw new InvalidOperationException("A native Give boundary can only be captured once before commit.");
        _nativeGiveHistoryCutoff = cursor;
    }
    internal TurnEndedEvent FreezeNativeGiveHistory()
    {
        _nativeGiveHistoryFrozen = true;
        return this with { };
    }
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeGiveScopeRuntime(INativeFactDemand demand, IProgramEventHistoryQueries history,
    IProgramEventJournalQueries journal, IAdvancingRuleFacts advancing) : INativeGiveScopeQueries
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 35700)]
    internal void Close(TurnEndedEvent ended)
    {
        if (demand.RequiresFact(ProgramTrackedFact.NativeGive))
            ended.StampNativeGiveHistoryCutoff(journal.Cursor);
    }

    public long? CurrentCutoff(int turn, int turnOwner)
    {
        var cutoff = advancing.Facts.OfType<TurnEndedEvent>().Where(ended =>
            ended.TurnNumber == turn && ended.ActorSeat == turnOwner)
            .Select(ended => ended.NativeGiveHistoryCutoff).Max();
        var previous = history.Read<TurnEndedEvent>().LastOrDefault(ended =>
            ended.TurnNumber == turn && ended.ActorSeat == turnOwner);
        if (previous is null) return cutoff;
        if (previous.NativeGiveHistoryCutoff is { } captured)
            return cutoff is { } active ? Math.Max(active, captured) : captured;
        long cursor = 0;
        foreach (var fact in journal.Complete())
        {
            cursor++;
            if (ReferenceEquals(fact, previous))
                return cutoff is { } active ? Math.Max(active, cursor) : cursor;
        }
        return cutoff;
    }
}
