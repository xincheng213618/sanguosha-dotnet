using CardGame.Core;

internal static class ProgramEventHistoryIndexChecks
{
    public static void IndependentIndexTracksPendingReplacementAndCommittedIdentities()
    {
        var committed = new List<EventEnvelope>();
        var pending = new List<IGameEvent>();
        var history = new ProgramEventHistoryIndex(committed, pending);
        IProgramEventHistoryQueries queries = history;
        var first = new Fact(1);
        history.Append(first);
        var initial = queries.Read<Fact>();
        history.Append(new OtherFact());
        Require(ReferenceEquals(initial, queries.Read<Fact>()), "Unrelated facts must preserve the typed snapshot.");

        var replacement = new Fact(2);
        pending[0] = replacement;
        var replaced = queries.Read<Fact>();
        Require(replaced.Count == 1 && ReferenceEquals(replaced[0], replacement) && ReferenceEquals(initial[0], first),
            "A same-count pending replacement must be visible without rewriting a previously exposed snapshot.");

        var frozenCommit = new Fact(2);
        committed.Add(new(new EventId(1), null, 1, 1, "revision-1", frozenCommit));
        history.ClearPending();
        var afterCommit = queries.Read<Fact>();
        Require(afterCommit.Count == 1 && ReferenceEquals(afterCommit[0], frozenCommit),
            "Commit must replace the pending identity with its committed payload exactly once.");
        Require(afterCommit is System.Collections.IList { IsReadOnly: true }, "History snapshots must remain frozen.");
        IProgramEventHistoryQueries separate = new ProgramEventHistoryIndex([], []);
        Require(separate.Read<Fact>().Count == 0,
            "A separate match must not share typed history or cached pending state.");
        BoundaryQueriesKeepJournalOrderAndLiveTails();
    }

    private static void BoundaryQueriesKeepJournalOrderAndLiveTails()
    {
        var before = new Fact(10);
        var committedBoundary = new BoundaryFact();
        var after = new Fact(11);
        var committed = new List<EventEnvelope>
        {
            new(new EventId(1), null, 1, 1, "r1", before),
            new(new EventId(2), null, 1, 1, "r1", committedBoundary),
            new(new EventId(3), null, 1, 1, "r1", after)
        };
        var pendingBoundary = new BoundaryFact();
        var pendingFact = new Fact(12);
        var pending = new List<IGameEvent> { pendingBoundary, pendingFact };
        IProgramEventBoundaryQueries queries = new ProgramEventHistoryIndex(committed, pending);
        IProgramEventJournalQueries journal = (IProgramEventJournalQueries)queries;
        var complete = journal.Complete();
        var visited = new List<IGameEvent>();
        var tail = queries.SinceLastBoundary(fact => { visited.Add(fact); return fact is BoundaryFact; });
        Require(visited.SequenceEqual(new IGameEvent[] { pendingFact, pendingBoundary }),
            "Boundary lookup searches pending facts backwards before consulting committed history.");
        visited.Clear();
        Require(ReferenceEquals(journal.LastMatching(fact => { visited.Add(fact); return fact is BoundaryFact; }), pendingBoundary) &&
                visited.SequenceEqual(new IGameEvent[] { pendingFact, pendingBoundary }),
            "The latest matching fact is the original pending payload, with no older committed search after a match.");
        var appended = new Fact(13);
        pending.Add(appended);
        Require(tail.SequenceEqual(new IGameEvent[] { pendingFact, appended }) && ReferenceEquals(tail.First(), pendingFact),
            "The returned pending tail remains a live view of the original journal and retains payload identity.");
        Require(complete.SequenceEqual(new IGameEvent[] { before, committedBoundary, after, pendingBoundary, pendingFact, appended }),
            "Complete journal access keeps committed then pending order and observes later appends without keeping a second log.");
        pending.Clear();
        pending.Add(appended);
        Require(queries.SinceLastBoundary(fact => fact is BoundaryFact).SequenceEqual(new IGameEvent[] { after, appended }),
            "A committed boundary excludes itself and keeps committed then pending order.");
        Require(queries.SinceLastBoundary(_ => false).SequenceEqual(new IGameEvent[] { before, committedBoundary, after, appended }),
            "An absent boundary retains the complete original journal order.");
        visited.Clear();
        Require(ReferenceEquals(journal.LastMatching(fact => { visited.Add(fact); return fact is BoundaryFact; }), committedBoundary) &&
                visited.SequenceEqual(new IGameEvent[] { appended, after, committedBoundary }) &&
                journal.LastMatching(_ => false) is null,
            "Without a pending match, latest-fact lookup searches the committed journal backwards and returns null when absent.");
        IProgramEventJournalQueries arrayJournal = new ProgramEventHistoryIndex(committed.ToArray(), []);
        Require(ReferenceEquals(arrayJournal.LastMatching(fact => fact is BoundaryFact), committedBoundary) &&
                arrayJournal.Complete().SequenceEqual(new IGameEvent[] { before, committedBoundary, after }),
            "The read-only committed-journal fallback retains the same order and exact payload identities.");
    }

    private sealed record BoundaryFact : IGameEvent;
    private sealed record Fact(int Value) : IGameEvent;
    private sealed record OtherFact : IGameEvent;
    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
