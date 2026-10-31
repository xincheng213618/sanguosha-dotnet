using CardGame.Core;

internal static class MatchExecutionBoundaryChecks
{
    public static void IndependentLoopPreservesHumanChildAndSafetyBoundaries()
    {
        var host = new LoopHost { HasHumanDecision = true };
        var loop = new MatchExecutionLoop(host);
        loop.AdvanceToHumanBoundary();
        loop.AdvanceOneStep();
        Require(host.Steps == 0 && host.EnsureCalls == 2, "Pending human input must stop both execution paths.");

        host.HasHumanDecision = false;
        host.HasFrames = true;
        host.Winner = Winner.Rebels;
        host.Step = () => host.HasFrames = false;
        loop.AdvanceOneStep();
        Require(host.Steps == 1 && host.Status == EngineStatus.Completed,
            "An earned winner must wait for its child, then complete at the empty-stack boundary.");

        host = new LoopHost();
        loop = new(host);
        host.Step = () => { host.Winner = Winner.Rebels; host.HasPendingDecision = true; };
        loop.AdvanceOneStep();
        Require(host.Status == EngineStatus.Running && host.Steps == 1,
            "Single-step completion must preserve a newly issued decision.");

        host = new LoopHost { HasFrames = true, HasUnfinishedAutomaticDecision = true };
        loop = new(host);
        host.Step = () =>
        {
            if (host.Steps == 20_002) host.HasUnfinishedAutomaticDecision = false;
        };
        loop.AdvanceToHumanBoundary();
        Require(host.Steps == 20_002 && host.Status == EngineStatus.Completed && host.Winner == Winner.Draw,
            "The loop limit must allow an in-flight automatic decision to finish before ending as a draw.");
    }

    public static void IndependentSchedulerPreservesIssuedParentsAndLifecycleOrder()
    {
        var lifecycle = new LifecycleSpy();
        var scheduler = new ResolutionScheduler(lifecycle);
        lifecycle.Scheduler = scheduler;
        var action = new CardActionContext(1, null, CardActionType.Use, 0, 0, null, null, null,
            CardKind.Slash, [1], [], []);
        var parent = new CardUseFrame(1, 0, 17, CardKind.Slash, [1]) { Action = action };
        scheduler.Push(parent);
        Require(lifecycle.Calls.SequenceEqual(["use:1"]) && scheduler.Frames.HasIssuedLink(0),
            "Use policies must observe the already issued parent.");

        var damage = new DamageFrame(2, parent.Id, 0, 1, 1);
        scheduler.Push(damage);
        Reject(() => scheduler.ReplaceTop(parent with { CausedDamage = true }));
        Reject(() => scheduler.Replace(parent.Id, parent with { Id = 9 }));
        Reject(() => scheduler.Complete(parent.Id, parent.Kind));
        Require(scheduler.Frames.Count == 2 && scheduler.Frames.HasIssuedLink(1),
            "Rejected parent transitions must retain the original owned child.");
        scheduler.Complete(damage.Id, damage.Kind);
        scheduler.Push(new RecoveryFrame(3, parent.Id, 0, 1, 1));
        scheduler.Complete(3, ResolutionFrameKind.Recovery);
        Require(lifecycle.Calls.SequenceEqual(["use:1", "damage:2", "recovery:3"]),
            "Damage runs its pre-completion policy, while recovery reports only after returning to its parent.");
        scheduler.Complete(parent.Id, parent.Kind);
        Reject(() => scheduler.Push(new DamageFrame(4, parent.Id, 0, 1, 1)));
        Require(scheduler.Frames.Count == 0, "A child cannot be issued after its owner retires.");
    }

    private sealed class LoopHost : IMatchExecutionHost
    {
        public EngineStatus Status { get; set; } = EngineStatus.Running;
        public Winner Winner { get; set; }
        public bool HasFrames { get; set; }
        public bool HasPendingDecision { get; set; }
        public bool HasHumanDecision { get; set; }
        public bool HasUnfinishedAutomaticDecision { get; set; }
        internal int Steps;
        internal int EnsureCalls;
        internal Action? Step;
        public void EnsureStarted() => EnsureCalls++;
        public void RunStep() { Steps++; Step?.Invoke(); }
        public void CompleteGame() => Status = EngineStatus.Completed;
        public void EndAsDraw(string reason) { Winner = Winner.Draw; Status = EngineStatus.Completed; }
    }

    private sealed class LifecycleSpy : IResolutionLifecycle
    {
        internal ResolutionScheduler Scheduler = null!;
        internal List<string> Calls { get; } = [];
        public void CardUsePushed(CardUseFrame frame, CardActionContext action)
        {
            Require(ReferenceEquals(Scheduler.Frames.LastOrDefault(), frame), "Use must already be the active frame.");
            Calls.Add($"use:{frame.Id}");
        }
        public void BeforeDamageCompleted(DamageFrame frame)
        {
            Require(ReferenceEquals(Scheduler.Frames.LastOrDefault(), frame), "Damage must still be active before completion.");
            Calls.Add($"damage:{frame.Id}");
        }
        public void RecoveryCompleted(RecoveryFrame frame)
        {
            Require(Scheduler.Frames.LastOrDefault()?.Id == frame.ParentFrameId,
                "Recovery must return to its original parent before publishing its application.");
            Calls.Add($"recovery:{frame.Id}");
        }
    }

    private static void Reject(Action operation)
    {
        try { operation(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("An invalid frame transition was accepted.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
