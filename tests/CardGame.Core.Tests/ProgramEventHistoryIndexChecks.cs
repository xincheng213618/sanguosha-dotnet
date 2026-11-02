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
        ActualUsesKeepAcceptedAndFinalizedOrdersSeparate();
        DirectedOrdinalsUseAcceptedDesignationsAcrossStages();
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

    private static void ActualUsesKeepAcceptedAndFinalizedOrdersSeparate()
    {
        var history = new ProgramEventHistoryIndex([], []);
        static CardActionContext Action(long id, CardActionType type = CardActionType.Use) =>
            new(id, null, type, 0, 0, null, type == CardActionType.Response ? 0 : null, null,
                type == CardActionType.Response ? CardKind.Dodge : CardKind.IronChain, [2], [], []);
        var first = Action(1); var second = Action(2); var response = Action(3, CardActionType.Response);
        history.Append(new ActualCardActionRecordedEvent(first, 1, 0, TurnPhase.Play, 7, false));
        history.Append(new ActualCardActionRecordedEvent(second, 1, 0, TurnPhase.Play, 7, false));
        Require(history.FindAction(1) is { NativeUseId: null, PrimaryTargets: null, Finalized: false, Completed: false },
            "Acceptance does not invent a finalized target snapshot or native return.");
        history.Append(new ActualCardUseTargetsFinalizedEvent(20, second, [2]));
        history.Append(new ActualCardUseTargetsFinalizedEvent(10, first, [2]));
        history.Append(new ActualCardActionRecordedEvent(first, 1, 0, TurnPhase.Play, 7, false));
        var accepted = history.AcceptedPairFor(2);
        Require(accepted.Select(e => e.Action.ActionId).SequenceEqual([1L, 2L]) &&
            history.PreviousFinalizedUse(10)?.NativeUseId == 20 && history.PreviousFinalizedUse(20) is null,
            "A nested use may finalize first; repeated acceptance neither reorders nor duplicates the independent accepted prefix.");
        history.Append(new CardUseFinishedEvent(20, 0, CardKind.IronChain));
        Require(history.FindFinalizedUse(20) is { Completed: true } && history.FindFinalizedUse(10) is { Completed: false } &&
            !accepted[1].Completed && accepted is System.Collections.IList { IsReadOnly: true },
            "Actual completion updates its own entry while the earlier exposed accepted snapshot remains immutable.");
        history.Append(new ActualCardActionRecordedEvent(response, 1, 0, TurnPhase.Play, 7, true));
        Require(history.FindAction(3) is { NativeUseId: null, PrimaryTargets: null, Completed: false },
            "An accepted native response has no fabricated CardUse frame or target set.");
        history.Append(new CardResponseCompletedEvent(3, 30, 40, 0, 0, CardKind.Dodge,
            ProgramCardContinuation.Dodge, 1, 0, 0));
        Require(history.FindAction(3) is { NativeUseId: null, PrimaryTargets.Count: 0, Completed: true } &&
            history.AcceptedPairFor(3).Select(e => e.Action.ActionId).SequenceEqual([2L, 3L]),
            "The real response completion finalizes an empty target set and cuts the current accepted-use pair without a synthetic native ID.");
        history.Append(new ActualCardUseTargetsCompletedEvent(20, second, [2]));
        var completed = history.AcceptedPairAtCompletionFor(2);
        history.Append(new ActualCardActionRecordedEvent(Action(4), 1, 0, TurnPhase.Play, 7, false));
        Require(completed.Select(e => e.Action.ActionId).SequenceEqual([2L, 3L]) &&
            history.AcceptedPairAtCompletionFor(2).Select(e => e.Action.ActionId).SequenceEqual([2L, 3L]) &&
            history.AcceptedPairFor(2).Select(e => e.Action.ActionId).SequenceEqual([1L, 2L]),
            "Completion freezes the actual accepted tail after an intervening response; neither a later action nor the earlier acceptance prefix replaces it.");
        var changedTargets = new[] { 1 };
        var completion = new ActualCardUseTargetsCompletedEvent(10, first, changedTargets);
        var copied = completion with { PrimaryTargets = changedTargets };
        changedTargets[0] = 0;
        history.Append(copied);
        Require(history.FindCompletedUse(10)!.PrimaryTargets!.SequenceEqual([1]) &&
            history.FindFinalizedUse(10)!.PrimaryTargets!.SequenceEqual([2]) &&
            ((ActualCardUseTargetsCompletedEvent)CommittedEventProjection.Freeze(completion)).PrimaryTargets.SequenceEqual([1]) &&
            copied.PrimaryTargets is System.Collections.IList { IsReadOnly: true },
            "Completion targets stay detached from constructor and with inputs, without overwriting the earlier finalized view.");
    }

    private static void DirectedOrdinalsUseAcceptedDesignationsAcrossStages()
    {
        static CardActionContext Use(long id, int actor, int[] targets, CardKind kind = CardKind.IronChain) =>
            new(id, null, CardActionType.Use, actor, actor, null, null, null, kind, targets, [], [], designatedTargetSeats: targets);
        var history = new ProgramEventHistoryIndex([], []);
        var outer = Use(1, 1, [2]); var inner = Use(2, 1, [2]);
        history.Append(new ActualCardActionRecordedEvent(outer, 5, 0, TurnPhase.Draw, 7, false));
        history.Append(new ActualCardActionRecordedEvent(inner, 5, 0, TurnPhase.Finished, 8, false));
        history.Append(new ActualCardUseTargetsFinalizedEvent(20, inner, [2]));
        Require(history.FindFinalizedUse(10) is null && history.FinalizedDirectedTargetOrdinal(20, 2) == 2,
            "A real foreign actor's unfinalized outer designation counts in the same actual turn across non-Play phases.");
        history.Append(new ActualCardUseActionUpdatedEvent(10, Use(1, 1, [])));
        var third = Use(3, 1, [2]);
        history.Append(new ActualCardActionRecordedEvent(third, 5, 0, TurnPhase.Play, 9, false));
        history.Append(new ActualCardUseTargetsFinalizedEvent(30, third, [2]));
        Require(history.FinalizedDirectedTargetOrdinal(30, 2) == 2 && history.FinalizedDirectedTargetOrdinal(20, 2) == 2 &&
            history.FindAction(1) is { Completed: false, Finalized: false },
            "A target update changes future counts without finalizing its outer action or rewriting an already frozen qualification.");

        history = new([], []);
        var borrowed = Use(1, 0, [1, 2], CardKind.BorrowedSword);
        history.Append(new ActualCardActionRecordedEvent(borrowed, 5, 0, TurnPhase.Play, 9, false));
        history.Append(new ActualCardUseTargetsFinalizedEvent(10, borrowed, [1]));
        var victim = Use(2, 0, [2]);
        history.Append(new ActualCardActionRecordedEvent(victim, 5, 0, TurnPhase.Play, 9, false));
        history.Append(new ActualCardUseTargetsFinalizedEvent(20, victim, [2]));
        Require(history.FindFinalizedUse(10)!.PrimaryTargets!.SequenceEqual([1]) && history.FinalizedDirectedTargetOrdinal(20, 2) == 2,
            "Directed designation counts Borrowed Sword's paired victim even though consecutive primary-target history contains only its holder.");

        history = new([], []);
        history.Append(new ActualCardActionRecordedEvent(Use(1, 0, [2]), 5, 0, TurnPhase.Play, 9, false));
        history.Append(new ActualCardUseActionUpdatedEvent(10, Use(1, 1, [2])));
        history.Append(new ActualCardActionRecordedEvent(Use(2, 1, [2]), 5, 0, TurnPhase.Play, 9, false));
        history.Append(new ActualCardUseTargetsFinalizedEvent(20, Use(2, 1, [2]), [2]));
        history.Append(new ActualCardActionRecordedEvent(Use(3, 0, [2]), 5, 0, TurnPhase.Play, 9, false));
        history.Append(new ActualCardUseTargetsFinalizedEvent(30, Use(3, 0, [2]), [2]));
        Require(history.FindAction(1)!.AcceptedActorSeat == 0 && history.FinalizedDirectedTargetOrdinal(20, 2) == 1 &&
            history.FinalizedDirectedTargetOrdinal(30, 2) == 2,
            "An issued actor replacement never moves the already accepted action into another actor's history bucket.");
    }

    private sealed record BoundaryFact : IGameEvent;
    private sealed record Fact(int Value) : IGameEvent;
    private sealed record OtherFact : IGameEvent;
    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
