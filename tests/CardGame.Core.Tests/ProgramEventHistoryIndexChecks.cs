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
        OpenActualTurnFollowsTheOriginalCursorReturn();
        ActualUsesKeepAcceptedAndFinalizedOrdersSeparate();
        LatestFactsKeepAcceptedDesignationsAcrossStages();
        ObservationsKeepNativeScopeAndFrozenSnapshots();
        NativeUseVersionsKeepEndCutoffsAndLegacyIdentity();
        LateActionAliasesPreserveEarlierNativeDeclarations();
        ConfirmationsKeepOriginalTargetsAndFirstUseOrder();
        ActualPhaseCountsKeepOccurrencesAndAsOfCutoffs();
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

    private static void OpenActualTurnFollowsTheOriginalCursorReturn()
    {
        var first = new TurnStartedEvent(1, 0);
        var history = new ProgramEventHistoryIndex([new(new EventId(1), null, 1, 1, "r1", first)], []);
        history.Append(new TurnEndedEvent(1, 0));
        Require(history.FindOpenActualTurn(1, 0) is { Ordinal: 1 } active && ReferenceEquals(active.Start, first),
            "A turn that emitted TurnEnded still owns its original cursor during a late phase.");

        history.Append(new ActualTurnCursorReturnedEvent(1, 0));
        Require(history.FindOpenActualTurn(1, 0) is null,
            "A pending native cursor return closes the previous turn immediately.");

        var next = new TurnStartedEvent(2, 1);
        history.Append(next);
        Require(history.FindOpenActualTurn(2, 1) is { Ordinal: 2 } started && ReferenceEquals(started.Start, next) &&
                history.FindOpenActualTurn(1, 0) is null && history.FindOpenActualTurn(2, 0) is null,
            "The latest pending start uses the whole-table ordinal and requires the original turn and actor.");
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
        history.Append(new ActualCardUseTargetsFinalizedEvent(20, second, [2], 1, 0));
        history.Append(new ActualCardUseTargetsFinalizedEvent(10, first, [2], 1, 0));
        history.Append(new ActualCardActionRecordedEvent(first, 1, 0, TurnPhase.Play, 7, false));
        var acceptedPoint = new CardUseHistoryPoint(1, 0, history.CardUseHistoryCursor);
        var accepted = history.ReadCardUseFacts(acceptedPoint, CardUseHistoryView.Latest);
        var beforeCompletion = history.FindFinalizedUse(20)!;
        Require(accepted.Select(e => e.ActionId!.Value).SequenceEqual([1L, 2L]) &&
            history.ReadCardUseFacts(history.FindFinalizedUse(10)!.HistoryPoint!, CardUseHistoryView.Finalizations)
                .Select(e => e.NativeUseId).SequenceEqual(new long?[] { 20, 10 }) &&
            history.ReadCardUseFacts(history.FindFinalizedUse(20)!.HistoryPoint!, CardUseHistoryView.Finalizations)
                .Select(e => e.NativeUseId).SequenceEqual(new long?[] { 20 }),
            "A nested use may finalize first; repeated acceptance neither reorders nor duplicates the independent accepted prefix.");
        history.Append(new CardUseFinishedEvent(20, 0, CardKind.IronChain));
        Require(history.FindFinalizedUse(20) is { Completed: true } && history.FindFinalizedUse(10) is { Completed: false } &&
            !beforeCompletion.Completed && accepted is System.Collections.IList { IsReadOnly: true },
            "Actual completion updates its own entry while the earlier exposed accepted snapshot remains immutable.");
        history.Append(new ActualCardActionRecordedEvent(response, 1, 0, TurnPhase.Play, 7, true));
        Require(history.FindAction(3) is { NativeUseId: null, PrimaryTargets: null, Completed: false },
            "An accepted native response has no fabricated CardUse frame or target set.");
        history.Append(new CardResponseCompletedEvent(3, 30, 40, 0, 0, CardKind.Dodge,
            ProgramCardContinuation.Dodge, 1, 0, 0));
        Require(history.FindAction(3) is { NativeUseId: null, PrimaryTargets.Count: 0, Completed: true } &&
            history.ReadCardUseFacts(new(1, 0, history.CardUseHistoryCursor), CardUseHistoryView.Latest)
                .TakeLast(2).Select(e => e.ActionId!.Value).SequenceEqual([2L, 3L]),
            "The real response completion finalizes an empty target set and cuts the current accepted-use pair without a synthetic native ID.");
        history.Append(new ActualCardUseTargetsCompletedEvent(20, second, [2], 1, 0));
        var completionPoint = history.FindCompletedUse(20)!.HistoryPoint!;
        var completed = history.ReadCardUseFacts(completionPoint, CardUseHistoryView.Accepted).TakeLast(2).ToArray();
        history.Append(new ActualCardActionRecordedEvent(Action(4), 1, 0, TurnPhase.Play, 7, false));
        Require(completed.Select(e => e.ActionId).SequenceEqual(new long?[] { 2, 3 }) &&
            completed.All(e => e.Finalized && e.Completed) &&
            history.FindAction(2)!.CompletionPoint == completionPoint &&
            history.ReadCardUseFacts(completionPoint, CardUseHistoryView.Accepted).TakeLast(2)
                .Select(e => e.ActionId).SequenceEqual(new long?[] { 2, 3 }) &&
            history.ReadCardUseFacts(acceptedPoint, CardUseHistoryView.Latest).Select(e => e.ActionId!.Value).SequenceEqual([1L, 2L]),
            "Completion freezes the actual accepted tail after an intervening response; neither a later action nor the earlier acceptance prefix replaces it.");
        var changedTargets = new[] { 1 };
        var completion = new ActualCardUseTargetsCompletedEvent(10, first, changedTargets, 1, 0);
        var copied = completion with { PrimaryTargets = changedTargets };
        changedTargets[0] = 0;
        history.Append(copied);
        Require(history.FindCompletedUse(10)!.PrimaryTargets!.SequenceEqual([1]) &&
            history.FindFinalizedUse(10)!.PrimaryTargets!.SequenceEqual([2]) &&
            ((ActualCardUseTargetsCompletedEvent)CommittedEventProjection.Freeze(completion)).PrimaryTargets.SequenceEqual([1]) &&
            copied.PrimaryTargets is System.Collections.IList { IsReadOnly: true },
            "Completion targets stay detached from constructor and with inputs, without overwriting the earlier finalized view.");
        var live = new CardUseHistoryPoint(1, 0, history.CardUseHistoryCursor, 10, 1);
        Require(history.ReadCardUseFacts(live, CardUseHistoryView.Accepted).Single(f => f.ActionId == 1).PrimaryTargets!.SequenceEqual([1]) &&
            history.ReadCardUseFacts(live, CardUseHistoryView.Finalizations).Single(f => f.ActionId == 1).PrimaryTargets!.SequenceEqual([2]),
            "Accepted stage reads use the actual completion targets while finalization reads keep their original targets.");
        var world = new CardUseHistoryTestWorld(history, live);
        Require(world.Bool("""["sameUse",["last",["cardUses","window","finalizations"]],"currentUse"]""") == true &&
            world.Bool("""["all",["takeLast",["cardUses","window","accepted"],2],["useCompleted","use"]]""") == false,
            "Last follows finalization order, and taking the accepted tail before checking completion cannot skip an unfinished action.");
        history.Append(new ActualCardActionRecordedEvent(Action(5), 1, 0, TurnPhase.Play, 8, false));
        history.Append(new ActualCardUseTargetsFinalizedEvent(50, Action(5), [2], 1, 0));
        var inserted = new CardUseHistoryTestWorld(history, history.FindFinalizedUse(50)!.HistoryPoint!);
        Require(inserted.Int("""["count",["cardUses","window","finalizations"],["samePlayPhase","use","currentUse"]]""") == 1 &&
            inserted.Bool("""["usePhaseIs","currentUse","play","accepted"]""") == true,
            "A distinct inserted Play instance does not inherit the previous instance's consecutive finalizations.");
    }

    private static void LatestFactsKeepAcceptedDesignationsAcrossStages()
    {
        static CardActionContext Use(long id, int actor, int[] targets, CardKind kind = CardKind.IronChain) =>
            new(id, null, CardActionType.Use, actor, actor, null, null, null, kind, targets, [], [], designatedTargetSeats: targets);
        var history = new ProgramEventHistoryIndex([], []);
        var outer = Use(1, 1, [2]); var inner = Use(2, 1, [2]);
        history.Append(new ActualCardActionRecordedEvent(outer, 5, 0, TurnPhase.Draw, 7, false));
        history.Append(new ActualCardActionRecordedEvent(inner, 5, 0, TurnPhase.Finished, 8, false));
        history.Append(new ActualCardUseTargetsFinalizedEvent(20, inner, [2], 5, 0));
        var innerPoint = new CardUseHistoryPoint(5, 0, history.CardUseHistoryCursor, 20, 2);
        Require(history.FindFinalizedUse(10) is null &&
            new CardUseHistoryTestWorld(history, innerPoint, target: 2).Int(CardUseHistoryTestWorld.DirectedCount) == 2,
            "A real foreign actor's unfinalized outer designation counts in the same actual turn across non-Play phases.");
        history.Append(new ActualCardUseActionUpdatedEvent(10, Use(1, 1, []), 5, 0));
        var third = Use(3, 1, [2]);
        history.Append(new ActualCardActionRecordedEvent(third, 5, 0, TurnPhase.Play, 9, false));
        history.Append(new ActualCardUseTargetsFinalizedEvent(30, third, [2], 5, 0));
        Require(new CardUseHistoryTestWorld(history, new(5, 0, history.CardUseHistoryCursor, 30, 3), target: 2).Int(CardUseHistoryTestWorld.DirectedCount) == 2 &&
            new CardUseHistoryTestWorld(history, innerPoint, target: 2).Int(CardUseHistoryTestWorld.DirectedCount) == 2 &&
            history.FindAction(1) is { Completed: false, Finalized: false },
            "A target update changes future counts without finalizing its outer action or rewriting an already frozen qualification.");

        history = new([], []);
        var borrowed = Use(1, 0, [1, 2], CardKind.BorrowedSword);
        history.Append(new ActualCardActionRecordedEvent(borrowed, 5, 0, TurnPhase.Play, 9, false));
        history.Append(new ActualCardUseTargetsFinalizedEvent(10, borrowed, [1], 5, 0));
        var victim = Use(2, 0, [2]);
        history.Append(new ActualCardActionRecordedEvent(victim, 5, 0, TurnPhase.Play, 9, false));
        history.Append(new ActualCardUseTargetsFinalizedEvent(20, victim, [2], 5, 0));
        Require(history.FindFinalizedUse(10)!.PrimaryTargets!.SequenceEqual([1]) &&
            new CardUseHistoryTestWorld(history, new(5, 0, history.CardUseHistoryCursor, 20, 2), target: 2).Int(CardUseHistoryTestWorld.DirectedCount) == 2,
            "Directed designation counts Borrowed Sword's paired victim even though consecutive primary-target history contains only its holder.");

        history = new([], []);
        history.Append(new ActualCardActionRecordedEvent(Use(1, 0, [2]), 5, 0, TurnPhase.Play, 9, false));
        history.Append(new ActualCardUseActionUpdatedEvent(10, Use(1, 1, [2]), 5, 0));
        history.Append(new ActualCardActionRecordedEvent(Use(2, 1, [2]), 5, 0, TurnPhase.Play, 9, false));
        history.Append(new ActualCardUseTargetsFinalizedEvent(20, Use(2, 1, [2]), [2], 5, 0));
        history.Append(new ActualCardActionRecordedEvent(Use(3, 0, [2]), 5, 0, TurnPhase.Play, 9, false));
        history.Append(new ActualCardUseTargetsFinalizedEvent(30, Use(3, 0, [2]), [2], 5, 0));
        Require(history.FindAction(1)!.AcceptedActorSeat == 0 &&
            new CardUseHistoryTestWorld(history, new(5, 0, history.CardUseHistoryCursor, 20, 2), target: 2).Int(CardUseHistoryTestWorld.DirectedCount) == 1 &&
            new CardUseHistoryTestWorld(history, new(5, 0, history.CardUseHistoryCursor, 30, 3), target: 2).Int(CardUseHistoryTestWorld.DirectedCount) == 2,
            "An issued actor replacement never moves the already accepted action into another actor's history bucket.");
    }

    private static void ObservationsKeepNativeScopeAndFrozenSnapshots()
    {
        static CardActionContext Use(long id, int actor, int[] targets) =>
            new(id, null, CardActionType.Use, actor, 0, null, null, null, CardKind.IronChain, targets, [], []);
        var history = new ProgramEventHistoryIndex([], []);
        history.Append(new ActualCardActionRecordedEvent(Use(1, 0, [2]), 1, 0, TurnPhase.Play, 1, false));
        history.Append(new ActualCardUseActionUpdatedEvent(10, Use(1, 0, [3]), 2, 1));
        Require(new CardUseHistoryTestWorld(history, new(1, 0, history.CardUseHistoryCursor)).Seats(CardUseHistoryTestWorld.EstablishedOtherTargets)!.Count == 0 &&
            new CardUseHistoryTestWorld(history, new(2, 1, history.CardUseHistoryCursor)).Seats(CardUseHistoryTestWorld.EstablishedOtherTargets)!.Count == 0,
            "Acceptance and pre-finalized target edits do not claim that a target was actually observed.");
        history.Append(new ActualCardUseTargetsFinalizedEvent(10, Use(1, 0, [3]), [3], 2, 1));
        var frozen = new CardUseHistoryTestWorld(history, new(2, 1, history.CardUseHistoryCursor)).Seats(CardUseHistoryTestWorld.EstablishedOtherTargets)!;
        history.Append(new ActualCardUseActionUpdatedEvent(10, Use(1, 0, [4]), 2, 1));
        history.Append(new ActualCardUseTargetsCompletedEvent(10, Use(1, 0, []), [], 2, 1));
        Require(frozen.SequenceEqual([3]) && frozen is System.Collections.IList { IsReadOnly: true } &&
            new CardUseHistoryTestWorld(history, new(2, 1, history.CardUseHistoryCursor)).Seats(CardUseHistoryTestWorld.EstablishedOtherTargets)!.Order().SequenceEqual([3, 4]) &&
            new CardUseHistoryTestWorld(history, new(1, 0, history.CardUseHistoryCursor)).Seats(CardUseHistoryTestWorld.EstablishedOtherTargets)!.Count == 0,
            "Observed targets remain after later cancellation, use the native observation scope and never mutate a captured snapshot.");
        history.Append(new ActualCardUseActionUpdatedEvent(10, Use(1, 2, [2, 5]), 2, 1));
        history.Append(new ActualCardActionRecordedEvent(Use(2, 2, [6]), 1, 0, TurnPhase.Finished, 1, false));
        history.Append(new ActualCardUseTargetsCompletedEvent(11, Use(2, 2, [6]), [6], 2, 1));
        Require(new CardUseHistoryTestWorld(history, new(2, 1, history.CardUseHistoryCursor)).Seats(CardUseHistoryTestWorld.EstablishedOtherTargets)!.Order().SequenceEqual([3, 4]) &&
            new CardUseHistoryTestWorld(history, new(2, 1, history.CardUseHistoryCursor), owner: 2).Seats(CardUseHistoryTestWorld.EstablishedOtherTargets)!.Order().SequenceEqual([5, 6]) && frozen.SequenceEqual([3]),
            "Actor rewrites sample new native targets without moving the old actor's union; self targets are excluded and completed targets participate.");
    }

    private static void NativeUseVersionsKeepEndCutoffsAndLegacyIdentity()
    {
        var committed = new List<EventEnvelope>(); var pending = new List<IGameEvent>();
        var history = new ProgramEventHistoryIndex(committed, pending);
        var targets = new[] { 1 };
        var original = new NativeCardUseHistorySnapshot(10, 1, 0, 0, CardKind.Slash, null, targets);
        var captured = original with { DesignatedTargets = targets }; targets[0] = 3;
        history.Append(new GameStartedEvent(4));
        history.Append(new NativeCardUseHistoryRecordedEvent(captured));
        history.Append(new OtherFact());
        Require(history.FindNativeUse(10) is { NativeUseId: 10, ActionId: null } legacy && legacy.DesignatedTargets.SequenceEqual([1]) &&
            captured.DesignatedTargets is System.Collections.IList { IsReadOnly: true } && new CardUseHistoryTestWorld(history, new(1, 0, 3), owner: 0).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == true,
            "A declared legacy identity needs no Action; detached native targets participate at the complete journal cursor including unrelated facts.");
        history.Append(new NativeCardUseHistoryRecordedEvent(original with { DesignatedTargets = [] }));
        Require(new CardUseHistoryTestWorld(history, new(1, 0, 3), owner: 0).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == true && new CardUseHistoryTestWorld(history, new(1, 0, 4), owner: 0).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == false,
            "An end cutoff retains the former target while a later cutoff sees cancellation, not the historical ever union.");
        history.Append(new NativeCardUseHistoryRecordedEvent(original with { ActorSeat = 2, ActionId = 99 }));
        Require(new CardUseHistoryTestWorld(history, new(1, 0, 5), owner: 0).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == false && new CardUseHistoryTestWorld(history, new(1, 0, 5), owner: 2).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == true &&
            history.FindNativeUse(10) is { NativeUseId: 10, ActionId: 99, ActorSeat: 2 },
            "A later genuine action identity and actor role update the same native use; original material ownership does not count as the actor.");
        history.Append(new NativeCardUseHistoryRecordedEvent(new(20, 2, 1, 0, CardKind.Alcohol, null, [0])));
        Require(new CardUseHistoryTestWorld(history, new(2, 1, 6), owner: 0).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == false && new CardUseHistoryTestWorld(history, new(2, 1, 6), owner: 2).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == false &&
            new CardUseHistoryTestWorld(history, new(1, 0, 5), owner: 2).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == true, "A separate actual-turn scope stays independent and self Wine is never another-player targeting.");
        var frozen = pending.Select(CommittedEventProjection.Freeze).ToArray();
        for (var index = 0; index < frozen.Length; index++)
            committed.Add(new(new(index + 1), null, 100 + index * 10, 1, "native-history-cutoff", frozen[index]));
        history.ClearPending();
        Require(new CardUseHistoryTestWorld(history, new(1, 0, 3), owner: 0).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == true && new CardUseHistoryTestWorld(history, new(1, 0, 4), owner: 0).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == false &&
            new CardUseHistoryTestWorld(history, new(1, 0, 5), owner: 2).Bool(CardUseHistoryTestWorld.OtherDeclaredTarget) == true && history.FindNativeUse(10)!.DesignatedTargets.SequenceEqual([1]),
            "Pending-to-commit replacement preserves original journal-count cutoffs even when envelope sequences use a different scale.");
    }

    private static void LateActionAliasesPreserveEarlierNativeDeclarations()
    {
        var history = new ProgramEventHistoryIndex([], []);
        var native = new NativeCardUseHistorySnapshot(10, 1, 0, 0, CardKind.Slash, null, [1]);
        var action = new CardActionContext(99, null, CardActionType.Use, 0, 0, null, null, null, CardKind.Slash, [1], [], []);
        history.Append(new NativeCardUseHistoryRecordedEvent(native));
        history.Append(new ActualCardActionRecordedEvent(action, 1, 0, TurnPhase.Play, 1, false));
        var before = new CardUseHistoryPoint(1, 0, 2);
        history.Append(new NativeCardUseHistoryRecordedEvent(native with { ActionId = 99 }));
        var old = history.ReadCardUseFacts(before, CardUseHistoryView.Latest);
        var linked = history.ReadCardUseFacts(new(1, 0, 3), CardUseHistoryView.Latest);
        Require(old.Count == 2 && old.Any(f => f.NativeUseId == 10 && f.ActionId is null) &&
            linked is [{ NativeUseId: 10, ActionId: 99, DeclarerSeat: 0 }],
            "A later real alias cannot erase the already-known native declaration at an earlier cutoff; only the linked view collapses the two identities.");

        history = new([], []);
        history.Append(new NativeCardUseHistoryRecordedEvent(native));
        history.Append(new ActualCardActionRecordedEvent(action, 2, 1, TurnPhase.Play, 1, false));
        history.Append(new NativeCardUseHistoryRecordedEvent(native with { ActionId = 99 }, 2, 1));
        Require(history.ReadCardUseFacts(new(1, 0, 2), CardUseHistoryView.Latest) is [{ NativeUseId: 10, ActionId: null }] &&
            history.ReadCardUseFacts(new(2, 1, 3), CardUseHistoryView.Latest) is [{ NativeUseId: 10, ActionId: 99 }],
            "Linking in the accepted scope leaves the former scope's historical declaration in place and yields one canonical current identity.");
    }

    private static void ConfirmationsKeepOriginalTargetsAndFirstUseOrder()
    {
        const string firstSlash = """["sameUse",["first",["filter",["cardUses","window","confirmations"],["and",["usePhaseIs","use","play"],["useKindIs","use",["slash","fireSlash","thunderSlash"]]]]],"currentUse"]""";
        var history = new ProgramEventHistoryIndex([], []);
        var targets = new[] { 1 };
        var legacy = new NativeCardUseHistorySnapshot(10, 1, 0, 0, CardKind.ColdSlash, null, [1]);
        history.Append(new NativeCardUseHistoryRecordedEvent(legacy, Confirmation: new(targets, TurnPhase.Play, 7)));
        targets[0] = 3;
        var firstPoint = new CardUseHistoryPoint(1, 0, 1, 10);
        var frozen = history.ReadCardUseFacts(firstPoint, CardUseHistoryView.Confirmations);
        history.Append(new NativeCardUseHistoryRecordedEvent(legacy with { DesignatedTargets = [] }));
        history.Append(new NativeCardUseHistoryRecordedEvent(legacy with { EffectiveKind = CardKind.Duel, DesignatedTargets = [2] }));
        var currentPoint = new CardUseHistoryPoint(1, 0, history.CardUseHistoryCursor, 10);
        var latest = history.ReadCardUseFacts(currentPoint, CardUseHistoryView.Latest).Single();
        Require(frozen is [{ NativeUseId: 10, ActionId: null, Phase: TurnPhase.Play, ConfirmationCursor: 1 }] &&
            frozen[0].ConfirmedTargets!.SequenceEqual([1]) && frozen[0].PrimaryTargets!.SequenceEqual([1]) &&
            latest.Targets.SequenceEqual([2]) && latest.DeclaredKind == CardKind.ColdSlash && latest.EffectiveKind == CardKind.Duel &&
            history.ReadCardUseFacts(currentPoint, CardUseHistoryView.Confirmations).Single().ConfirmedTargets!.SequenceEqual([1]),
            "Confirmation is a detached original snapshot; cancellation, later target additions and effective-kind changes only update the live use.");
        var world = new CardUseHistoryTestWorld(history, currentPoint);
        Require(world.Bool(firstSlash) == true && world.Bool("""["useKindIs","currentUse",["slash"]]""") == false &&
            world.Seats("""["useConfirmedTargets","currentUse"]""")!.SequenceEqual([1]) &&
            world.Seats("""["usePrimaryTargets","currentUse"]""")!.SequenceEqual([1]),
            "The existing three-kind Slash matcher includes Cold and reads the original declared kind; the single Slash selector stays exact.");

        var second = new CardActionContext(99, null, CardActionType.Use, 1, 1, null, null, null, CardKind.Slash, [2], [], []);
        history.Append(new ActualCardActionRecordedEvent(second, 1, 0, TurnPhase.Play, 7, false));
        history.Append(new NativeCardUseHistoryRecordedEvent(new(20, 1, 0, 1, CardKind.Slash, 99, [2]),
            Confirmation: new([2], TurnPhase.Play, 7)));
        Require(new CardUseHistoryTestWorld(history, new(1, 0, history.CardUseHistoryCursor, 20, 99), owner: 1).Bool(firstSlash) == false &&
            new CardUseHistoryTestWorld(history, firstPoint).Bool(firstSlash) == true && frozen.Count == 1 &&
            history.ReadCardUseFacts(new(1, 0, 0), CardUseHistoryView.Confirmations).Count == 0 &&
            history.ReadCardUseFacts(new(2, 0, history.CardUseHistoryCursor), CardUseHistoryView.Confirmations).Count == 0,
            "An Action-null use consumes the global first confirmation across actors; later facts and other actual turns cannot alter an earlier cursor.");
        Require(new CardUseHistoryTestWorld(history, new(2, 0, history.CardUseHistoryCursor)).Bool(firstSlash) is null,
            "An empty first or missing current use remains unknown through identity comparison.");

        history = new([], []);
        var outer = new NativeCardUseHistorySnapshot(30, 1, 0, 0, CardKind.Slash, null, [1]);
        var inner = new NativeCardUseHistorySnapshot(40, 1, 0, 1, CardKind.Slash, null, [2]);
        history.Append(new NativeCardUseHistoryRecordedEvent(outer));
        history.Append(new NativeCardUseHistoryRecordedEvent(inner));
        history.Append(new NativeCardUseHistoryRecordedEvent(inner, Confirmation: new([2], TurnPhase.Play, 7)));
        history.Append(new NativeCardUseHistoryRecordedEvent(outer, Confirmation: new([1], TurnPhase.Play, 7)));
        Require(history.ReadCardUseFacts(new(1, 0, 4), CardUseHistoryView.Confirmations).Select(f => f.NativeUseId).SequenceEqual(new long?[] { 40, 30 }) &&
            new CardUseHistoryTestWorld(history, new(1, 0, 4, 40)).Bool(firstSlash) == true &&
            new CardUseHistoryTestWorld(history, new(1, 0, 4, 30)).Bool(firstSlash) == false,
            "First means actual confirmation order when an inner use confirms before the earlier-declared outer use.");

        history = new([], []);
        history.Append(new NativeCardUseHistoryRecordedEvent(new(50, 1, 0, 0, CardKind.BorrowedSword, null, [1]),
            Confirmation: new([1, 2], TurnPhase.Play, 7)));
        var borrowedPoint = new CardUseHistoryPoint(1, 0, 1, 50);
        var borrowedConfirmation = history.ReadCardUseFacts(borrowedPoint, CardUseHistoryView.Confirmations).Single();
        Require(history.ReadCardUseFacts(borrowedPoint, CardUseHistoryView.Latest).Single().Targets.SequenceEqual([1]) &&
            borrowedConfirmation.Targets.SequenceEqual([1, 2]) && borrowedConfirmation.PrimaryTargets!.SequenceEqual([1]) &&
            new CardUseHistoryTestWorld(history, borrowedPoint).Seats("""["targets",["filter",["cardUses","window","confirmations"],["same",["useActor","use"],"turnOwner"]]]""")!.SequenceEqual([1, 2]),
            "The confirmations projection includes Borrowed Sword's raw paired victim even when designated Latest targets contain only the holder; its principal target remains only the holder.");
    }

    private static void ActualPhaseCountsKeepOccurrencesAndAsOfCutoffs()
    {
        var committed = new List<EventEnvelope>(); var pending = new List<IGameEvent>();
        var history = new ProgramEventHistoryIndex(committed, pending);
        IActualPhaseHistoryQueries phases = history;
        var play = new ActualDiscardRecoveryPhaseKey(10, 1, 0, 0, ActualDiscardRecoveryPhaseKind.Play);
        history.Append(new ActualDiscardRecoveryPhaseStartedEvent(play));
        history.Append(new ActualDiscardRecoveryPhaseRestoredEvent(50, play));
        history.Append(new ProgramTurnPhaseSubstitutedEvent(60, "fixture:request", "replace", "instance", 0, 1,
            SkillProgramTriggerWindow.PlayPhaseStarting, SkillProgramTurnPhase.Play));
        var enteredPoint = new CardUseHistoryPoint(1, 0, history.CardUseHistoryCursor);
        const string eligible = """["and",[">",["phaseCount","window","owner","play","entered"],0],["==",["phaseCount","window","owner","play","skipped"],0]]""";
        Require(new CardUseHistoryTestWorld(history, enteredPoint).Bool(eligible) == true &&
            phases.CountPhases(1, 0, 0, ActualDiscardRecoveryPhaseKind.Play, ActualPhaseHistoryOutcome.Entered, 3) == 1 &&
            phases.CountPhases(1, 0, 0, null, ActualPhaseHistoryOutcome.Skipped, 3) == 0,
            "Restoring an entered phase and requesting a substitution create neither a new entry nor an actual skip.");
        history.Append(new ActualDiscardRecoveryPhaseStartedEvent(play with { Token = 11 }));
        history.Append(new ActualPhaseSkippedEvent(1, 0, 0, ActualDiscardRecoveryPhaseKind.Play, null, 0, false));
        Require(new CardUseHistoryTestWorld(history, new(1, 0, 5)).Bool(eligible) == false &&
            new CardUseHistoryTestWorld(history, enteredPoint).Bool(eligible) == true &&
            phases.CountPhases(1, 0, 0, ActualDiscardRecoveryPhaseKind.Play, ActualPhaseHistoryOutcome.Entered, 5) == 2,
            "Any actual Play skip excludes the turn even with two genuine entries, without changing the earlier eligibility cutoff.");
        history.Append(new ActualPhaseSkippedEvent(1, 0, 0, ActualDiscardRecoveryPhaseKind.Draw, 100, 0, true));
        history.Append(new ActualPhaseSkippedEvent(1, 0, 0, ActualDiscardRecoveryPhaseKind.Draw, 101, 0, true));
        history.Append(new ActualPhaseSkippedEvent(1, 0, 1, ActualDiscardRecoveryPhaseKind.Draw, 102, 0, true));
        history.Append(new ActualPhaseSkippedEvent(2, 1, 0, ActualDiscardRecoveryPhaseKind.Draw, null, 0, false));
        Require(phases.CountPhases(1, 0, 0, ActualDiscardRecoveryPhaseKind.Draw, ActualPhaseHistoryOutcome.Skipped, 9) == 2 &&
            phases.CountPhases(1, 0, 0, null, ActualPhaseHistoryOutcome.Skipped, 9) == 3 &&
            phases.CountPhases(1, 0, 1, ActualDiscardRecoveryPhaseKind.Draw, ActualPhaseHistoryOutcome.Skipped, 9) == 1 &&
            phases.CountPhases(2, 1, 0, null, ActualPhaseHistoryOutcome.Skipped, 9) == 1,
            "Distinct extra Draw occurrences count independently while actor and actual-turn scopes remain separate.");
        var payloads = pending.ToArray();
        for (var index = 0; index < payloads.Length; index++)
            committed.Add(new(new(index + 1), null, 1000 + index * 10, 1, "phase-history-cutoff", payloads[index]));
        history.ClearPending();
        Require(new CardUseHistoryTestWorld(history, enteredPoint).Bool(eligible) == true &&
            phases.CountPhases(1, 0, 0, null, ActualPhaseHistoryOutcome.Skipped, 7) == 3,
            "Pending commit keeps full-journal count cutoffs, independent of envelope sequence values.");
    }

    private sealed record BoundaryFact : IGameEvent;
    private sealed record Fact(int Value) : IGameEvent;
    private sealed record OtherFact : IGameEvent;
    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
