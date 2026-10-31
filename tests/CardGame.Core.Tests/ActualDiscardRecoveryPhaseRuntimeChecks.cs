using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ActualDiscardRecoveryPhaseRuntimeChecks
{
    public static void OriginalJournalAndActionScopesPreserveLatestPhaseAndCachedNull()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var content = ComposedSkillContentRegistry.CreateShowcase(includeHellActivities: true);
        var game = GameEngine.CreateStandard(new GameOptions
        { Seed = 7, PlayerCount = 8, ModeId = "identity:classic-8" }, content);
        global::FixtureRuntimeAccess.Engine.GetField("_turnNumber", flags)!.SetValue(game, 1);
        global::FixtureRuntimeAccess.Engine.GetField("_phase", flags)!.SetValue(game, TurnPhase.Play);
        global::FixtureRuntimeAccess.Engine.GetField("_turnProgression", flags)!.SetValue(game, new ActualTurnProgression(ActualTurnKind.Normal, 0, 1));
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", flags)!.GetValue(game)!;
        var history = (ProgramEventHistoryIndex)capabilities.Get<IProgramEventHistoryQueries>();
        var committed = (List<EventEnvelope>)global::FixtureRuntimeAccess.Engine.GetField("_events", flags)!.GetValue(game)!;
        var pending = (List<IGameEvent>)global::FixtureRuntimeAccess.Engine.GetField("_pendingEvents", flags)!.GetValue(game)!;
        var runtime = game.GetRuntimeModule<ActualDiscardRecoveryPhaseRuntime>();
        var cache = capabilities.Get<IActualPhaseQueryCache>();
        var begin = global::FixtureRuntimeAccess.Engine.GetMethod("BeginActionQuery", flags)!;
        var original = new ActualDiscardRecoveryPhaseKey(1, 1, 0, 0, ActualDiscardRecoveryPhaseKind.Draw);
        var latest = new ActualDiscardRecoveryPhaseKey(2, 1, 0, 1, ActualDiscardRecoveryPhaseKind.Play);
        committed.Add(new(new EventId(1), null, 1, game.Revision, "actual-phase-query-fixture",
            new ActualDiscardRecoveryPhaseStartedEvent(original)));
        Require(ReferenceEquals(runtime.Current(), original), "An uncached read returns the original committed phase payload.");
        history.Append(new ActualDiscardRecoveryPhaseStartedEvent(latest));
        Require(ReferenceEquals(runtime.Current(), latest) && runtime.IsRecorded(original) && runtime.IsRecorded(latest),
            "The latest pending start takes precedence over committed facts, while both exact starts remain recorded.");
        history.Append(new ActualDiscardRecoveryPhaseClearedEvent(1));
        Require(runtime.Current() is null, "A pending clear overrides both pending and committed starts.");
        history.Append(new ActualDiscardRecoveryPhaseRestoredEvent(10, original));
        Require(ReferenceEquals(runtime.Current(), original) && runtime.IsRecorded(original with { }),
            "A pending restore returns the original phase object; recorded identity uses the complete immutable key.");
        Require(!runtime.IsRecorded(original with { Token = 0 }) && !runtime.IsRecorded(original with { Token = 99 }) &&
                !runtime.IsRecorded(original with { ActualTurnOwnerSeat = -1 }) && !runtime.IsRecorded(original with { ActorSeat = 8 }) &&
                !runtime.IsRecorded(original with { ActorSeat = 1 }),
            "Recorded proof rejects invalid tokens and seats, and cannot borrow a recorded token for a changed phase key.");
        history.Append(new ActualDiscardRecoveryPhaseStartedEvent(original with { }));
        Require(!runtime.IsRecorded(original), "Two equal started keys are ambiguous even when their payload objects differ.");
        pending.RemoveAt(pending.Count - 1);
        Require(runtime.IsRecorded(original), "Removing the duplicate restores the original journal's one exact start.");

        Require(cache.Current is null, "A read outside legal-action scope must not manufacture a cache owner.");
        IActualDiscardPhaseQueryCacheState first;
        using ((IDisposable)begin.Invoke(game, [])!)
        {
            first = cache.Current!;
            Require(first is not null && ReferenceEquals(first, global::FixtureRuntimeAccess.Engine.GetField("_actionQuery", flags)!.GetValue(game)) &&
                    !first.HasActualDiscardRecoveryPhase && ReferenceEquals(runtime.Current(), original) &&
                    first.HasActualDiscardRecoveryPhase && ReferenceEquals(first.ActualDiscardRecoveryPhase, original),
                "The first scoped read fills the original action-query object's fields with the original phase payload.");
            history.Append(new ActualDiscardRecoveryPhaseClearedEvent(1));
            using ((IDisposable)begin.Invoke(game, [])!)
                Require(ReferenceEquals(cache.Current, first) && ReferenceEquals(runtime.Current(), original),
                    "A nested action query shares its parent scope and retains that scope's already derived value.");
            Require(ReferenceEquals(cache.Current, first), "Disposing a nested query must leave the outer cache owner in place.");
        }
        Require(cache.Current is null, "Disposing the outer query releases its original scope.");
        IActualDiscardPhaseQueryCacheState second;
        using ((IDisposable)begin.Invoke(game, [])!)
        {
            second = cache.Current!;
            Require(!ReferenceEquals(first, second) && !second.HasActualDiscardRecoveryPhase && runtime.Current() is null &&
                    second.HasActualDiscardRecoveryPhase && second.ActualDiscardRecoveryPhase is null,
                "The next scope re-reads the clear and records a cached null separately from an unread cache.");
            history.Append(new ActualDiscardRecoveryPhaseRestoredEvent(11, latest));
            Require(runtime.Current() is null && ReferenceEquals(cache.Current, second),
                "A cached null stays null for the rest of the same scope after another phase fact arrives.");
        }
        Require(cache.Current is null, "A scope containing cached null must release its owner too.");
        using ((IDisposable)begin.Invoke(game, [])!)
            Require(!ReferenceEquals(cache.Current, second) && ReferenceEquals(runtime.Current(), latest),
                "A later scope re-reads the restored phase instead of inheriting the previous cached null.");
        Require(cache.Current is null && ReferenceEquals(runtime.Current(), latest),
            "Scope disposal returns to an uncached live journal read.");
        history.Append(new ActualDiscardRecoveryPhaseClearedEvent(1));
        Require(runtime.Current() is null && cache.Current is null,
            "Successive reads outside a scope observe new facts without retaining a phase answer.");
    }

    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
