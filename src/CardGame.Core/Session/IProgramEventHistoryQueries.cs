namespace CardGame.Core;

/// <summary>Read-only typed facts from this match's committed and pending journal.</summary>
internal interface IProgramEventHistoryQueries : IProgramSelectionQuery
{
    IReadOnlyList<T> Read<T>() where T : IGameEvent;
}

internal static class ProgramEventHistoryQueries
{
    internal static int CountActualTurnStarts(this IProgramEventHistoryQueries history, int actorSeat) =>
        history.Read<TurnStartedEvent>().Count(entry => entry.ActorSeat == actorSeat);
}

internal interface IProgramEventBoundaryQueries : IProgramSelectionQuery
{
    IEnumerable<IGameEvent> SinceLastBoundary(Func<IGameEvent, bool> boundary);
}

// Ordered access to the actual journal, including its current pending tail.
// Callers choose their own snapshot boundary; this service keeps no second log.
internal interface IProgramEventJournalQueries : IProgramSelectionQuery
{
    long Cursor => Complete().LongCount();
    IEnumerable<IGameEvent> Complete();
    IGameEvent? LastMatching(Func<IGameEvent, bool> predicate);
}
