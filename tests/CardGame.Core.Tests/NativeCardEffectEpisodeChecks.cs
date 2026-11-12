using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class NativeCardEffectEpisodeChecks
{
    public static void OriginalUseQueuesIndependentSourcesAndConsumesEmptyEpisodesOnce()
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var participants = capabilities.Get<IMatchParticipantStateAccess>();
        var events = new Facts();
        var execution = new Execution();
        var runtime = new NativeCardEffectEpisodeRuntime(frames, events, participants, execution);
        var depth = frames.OfType<ResolutionFrame>().Count;
        var useId = frames.NextId();
        var action = new CardActionContext(useId * 100, null, CardActionType.Use, 0, 0, null, null, null,
            CardKind.Peach, [1, 2], [], []);
        frames.Push(new CardUseFrame(useId, 0, 0, CardKind.Peach, [1, 2], PhysicalCardIds: []) { Action = action });
        var first = new ProgramSkillFrame(frames.NextId(), 0, "fixture:episode-first", "first", "fixture:hash", 1, [], []);
        var second = new ProgramSkillFrame(frames.NextId(), 0, "fixture:episode-second", "second", "fixture:hash", 1, [], []);
        frames.Push(first); frames.Push(second);
        try
        {
            var policies = new List<ProgramRecoveryPolicySource> { new("fixture:recovery", "issued") };
            var simple = new ProgramSimpleCardContinuation(0, SimpleCardUseEffect.Recovery, RecoveryPolicySources: policies);
            var commonIssue = Issue(first, 0, new(0, ContinueOriginalAfterActorDeath: true));
            var common = runtime.Issue(useId, commonIssue, simple: simple);
            var empty = runtime.Issue(useId, Issue(second, 0, new(20, NativeCardEffectEpisodeScope.FrozenTarget, TargetSeat: 3)));
            var local = runtime.Issue(useId, Issue(second, 1, new(30, NativeCardEffectEpisodeScope.FrozenTarget, Count: 2, TargetSeat: 1)));
            var issued = frames.Find(useId);
            Need(runtime.Issue(useId, commonIssue, simple: simple) == common && ReferenceEquals(issued, frames.Find(useId)) &&
                events.Items.OfType<NativeCardEffectEpisodeIssuedEvent>().Count() == 3,
                "Each actual issuer and slot owns one original-use episode; identical reentry neither replaces the frame nor publishes again.");
            Reject(() => runtime.Issue(long.MaxValue, commonIssue));
            Reject(() => runtime.Issue(first.Id, commonIssue));
            Reject(() => runtime.Issue(useId, commonIssue with { Spec = new(99) }));
            Reject(() => runtime.Issue(useId, commonIssue with { IssuanceSlot = 9, Spec = new(1, Count: 0) }));
            Reject(() => runtime.Issue(useId, commonIssue with { IssuanceSlot = 9, Spec = new(1, (NativeCardEffectEpisodeScope)99) }));
            Reject(() => runtime.Issue(useId, commonIssue, simple: simple with { RecoveryAmount = 2 }));
            Need(ReferenceEquals(issued, frames.Find(useId)), "Invalid owners, conflicting source keys and continuations reject atomically.");
            policies.Add(new("fixture:later", "not-issued"));
            frames.Pop(second.Id, second.Kind); frames.Pop(first.Id, first.Kind);
            var original = (CardUseFrame)frames.Find(useId)!;
            frames.Replace(useId, original with { Action = new CardActionContext(action.ActionId + 1, null,
                CardActionType.Use, 0, 0, null, null, null, CardKind.Peach, [1, 2], [], []) });
            Reject(() => runtime.Issue(useId, commonIssue));
            frames.Replace(useId, original);
            Need(original.EffectEpisodes!.Episodes is System.Collections.IList { IsReadOnly: true } &&
                runtime.Original(useId)!.Simple!.RecoveryPolicySources is [var policy] && policy.PolicyId == "issued",
                "The original continuation and issued collections freeze their caller-owned input before later source changes.");
            participants.At(0).IsAlive = false;
            execution.PendingTargets = true;
            Need(runtime.CanContinueAfterActorDeath(useId) && !runtime.TryBegin(useId) && ReferenceEquals(original, frames.Find(useId)),
                "An issued original-use death permission survives while future copies await unfinished native target effects.");
            execution.PendingTargets = false;
            Need(runtime.TryBegin(useId) && runtime.Current(useId) is { EpisodeId: var current, Ordinal: 1 } && current == common.Id &&
                execution.Cursors.Single().TargetSeats.SequenceEqual([1, 2]) && !runtime.IsTargetScoped(useId),
                "The whole-use episode retains both real execution targets and its accepted right after the actor dies.");
            var child = new ProgramSkillFrame(frames.NextId(), 1, "fixture:episode-child", "child", "fixture:hash", 1, [], []);
            frames.Push(child);
            var beforeChild = frames.Find(useId);
            Reject(() => runtime.CompleteCurrent(useId));
            Need(!runtime.TryBegin(useId) && ReferenceEquals(beforeChild, frames.Find(useId)),
                "An active episode cannot complete or dispatch another copy through its actual live child.");
            frames.Pop(child.Id, child.Kind);
            Need(runtime.TryBegin(useId) && runtime.IsCompleted(useId, common.Id) && runtime.IsCompleted(useId, empty.Id) &&
                runtime.Current(useId) is { EpisodeId: var next, CopyIndex: 1, Ordinal: 3 } && next == local.Id &&
                runtime.IsTargetScoped(useId) && execution.Cursors.Count == 2 && execution.Cursors[^1].TargetSeats.SequenceEqual([1]),
                "An empty first local episode is consumed once before the next issuer, without replaying the completed common episode.");
            var frozen = (CardUseFrame)frames.Find(useId)!;
            frames.Replace(useId, JsonSerializer.Deserialize<CardUseFrame>(JsonSerializer.Serialize(frozen))!);
            Need(runtime.Current(useId)!.TargetSeats is System.Collections.IList { IsReadOnly: true } &&
                runtime.TryBegin(useId) && runtime.Current(useId) is { CopyIndex: 2, Ordinal: 4 } &&
                !runtime.HasPending(useId) && runtime.IsActive(useId) &&
                !runtime.TryBegin(useId) && !runtime.TryBegin(useId) && !runtime.HasPending(useId) && !runtime.IsActive(useId) &&
                runtime.IsCompleted(useId, local.Id) && execution.Cursors.Count == 3,
                "Immutable JSON recovery keeps the exact active copy; all sources finish once and repeated completion cannot restart an older episode.");
            var cancelled = runtime.Issue(useId, Issue(second, 2, new(40, Count: 3)));
            execution.Ineffective = true;
            Need(!runtime.TryBegin(useId) && runtime.IsSettled(useId, cancelled.Id) && !runtime.IsCompleted(useId, cancelled.Id) &&
                runtime.Get(useId, cancelled.Id) is { Started: 0, Completed: 0, Cancelled: true },
                "Cancelling an unstarted use settles its issued obligation without inventing executed copies.");
            execution.Ineffective = false;
            var interrupted = runtime.Issue(useId, Issue(second, 3, new(50, Count: 2)));
            Need(runtime.TryBegin(useId), "A later independent original-use declaration starts its own copy.");
            execution.Ineffective = true;
            Need(!runtime.TryBegin(useId) && !runtime.IsActive(useId) && runtime.IsSettled(useId, interrupted.Id) &&
                !runtime.IsCompleted(useId, interrupted.Id) && runtime.Get(useId, interrupted.Id) is { Started: 1, Completed: 1, Cancelled: true } &&
                events.Items.OfType<NativeCardEffectEpisodeCompletedEvent>().Last().Executed == false && execution.Cursors.Count == 4,
                "Cancellation after a native child returns clears the active cursor and cancels only its unstarted remainder.");
            execution.Ineffective = false;
            participants.At(0).IsAlive = true;
            var constrained = runtime.Issue(useId, Issue(second, 4, new(60, ContinueAfterActorDeath: false)));
            Need(runtime.TryBegin(useId) && runtime.Current(useId)?.EpisodeId == constrained.Id,
                "A living actor can start an independently constrained extra copy.");
            participants.At(0).IsAlive = false;
            Need(runtime.IsActive(useId) && !runtime.CanContinueAfterActorDeath(useId),
                "The active extra copy cannot borrow the original use's independent permission after its actor dies.");
            runtime.CompleteCurrent(useId);
            Need(runtime.CanContinueAfterActorDeath(useId),
                "Returning the constrained extra copy leaves the original-use permission on its own native scope.");
        }
        finally
        {
            while (frames.OfType<ResolutionFrame>().Count > depth) { var top = frames.Top!; frames.Pop(top.Id, top.Kind); }
        }
    }

    private static NativeCardEffectEpisodeIssue Issue(ProgramSkillFrame frame, int slot, NativeCardEffectEpisodeSpec spec) =>
        new(frame.Id, 0, slot, new(frame.SkillId, frame.ActivationId, frame.OwnerSeat, "fixture:instance"), frame.GameplayHash, spec);
    private sealed class Facts : IProgramRuntimeEvents
    {
        internal List<IGameEvent> Items { get; } = [];
        public void AdvanceFact(IGameEvent fact) => Items.Add(fact);
    }
    private sealed class Execution : INativeCardEffectEpisodeExecution
    {
        internal bool PendingTargets { get; set; }
        internal bool Ineffective { get; set; }
        internal List<NativeCardEffectEpisodeCursor> Cursors { get; } = [];
        public bool IsMatchCompleted => false;
        public bool HasPendingTargets(CardUseFrame use) => PendingTargets;
        public bool IsUseIneffective(CardUseFrame use) => Ineffective;
        public bool TargetsRemainLegal(CardUseFrame use) => true;
        public bool ContinueSlashTargets(CardUseFrame use, NativeCardEffectOriginalState original,
            NativeCardEffectEpisodeCursor cursor) => throw new InvalidOperationException("The queue probe does not execute native Slash targets.");
        public bool Begin(CardUseFrame use, NativeCardEffectOriginalState original, NativeCardEffectEpisode episode,
            NativeCardEffectEpisodeCursor cursor, bool afterAttack) { Cursors.Add(cursor); return true; }
    }
    private static void Reject(Action action)
    {
        try { action(); } catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("An invalid native episode was accepted.");
    }
    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
