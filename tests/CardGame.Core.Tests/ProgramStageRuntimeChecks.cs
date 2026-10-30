using CardGame.Core;

internal static class ProgramStageRuntimeChecks
{
    private enum Stage { Issue, Children, External, Complete }
    private sealed record Receipt(Stage Stage, int Actual);

    public static void PaidReturnsKeepOriginalFrameAndExplicitPolicies()
    {
        var first = new Fixture(["recovery", "state", "skills", "hp", "cards"]);
        first.Run();
        Need(first.Paid == 1 && first.Frames.Top?.Id == 52 && first.FrameReceipt.Stage == Stage.Children,
            "The next stage and original movement return must be committed before an action pushes its paid child.");
        first.Run();
        Need(first.Paid == 1 && first.Validations == 1,
            "A suspended parent must neither enter its recipe nor repeat payment while another frame is on top.");
        first.Frames.Pop(52, ResolutionFrameKind.ProgramSkill);
        while (first.Frames.Top?.Id == 41)
        {
            first.Run();
            if (first.Frames.Top?.Id == 52) first.Frames.Pop(52, ResolutionFrameKind.ProgramSkill);
            else break;
        }
        Need(first.Log.SequenceEqual(new[] { "recovery", "state", "skills", "hp", "cards", "fact-before-clear", "returned-7" }) &&
            first.Paid == 1 && first.FrameReceipt.Stage == Stage.External && first.Frames.ActiveProgram(41).PendingMovementContinuation is null,
            "The declared child order runs once, and a declined drain's fresh receipt is used after the completion fact and movement cleanup.");
        first.Run();
        Need(first.Completed == 0 && first.FrameReceipt.Stage == Stage.External,
            "An external native return is required; being the active frame does not authorize the next rule action.");
        first.Set(Stage.Complete, 7);
        first.Run();
        Need(first.Completed == 1 && first.Frames.Top is null && first.Paid == 1,
            "Consuming the typed external return can retire the same original frame exactly once.");

        var prompted = new Fixture([]);
        prompted.Prompts.Set(new PendingDecision(DecisionKind.ProgramTrigger, 0, "fixture", [], []), true);
        prompted.Run();
        Need(prompted.Paid == 0 && prompted.Validations == 0, "A pending player decision prevents automatic advancement.");
        prompted.Prompts.Clear();
        ProgramStageRuntime.Run(42, prompted.Frames, prompted.Prompts, prompted, prompted.Plan(
            [ProgramStageNode<Fixture, Receipt, Stage>.AwaitExternalReturn(Stage.Issue)]));
        Need(prompted.Validations == 0 && prompted.Paid == 0, "A recipe cannot enter a different active frame ID.");
    }

    public static void RecipesKeepTerminationOrderAndRejectInvalidTransitions()
    {
        var before = new Fixture(["cards"]) { Finished = true };
        before.Set(Stage.Children, 0);
        before.Run(stopBeforeChildren: true);
        Need(before.Completed == 1 && before.Log.Count == 0,
            "A recipe that terminates before its child drain retains that original boundary.");
        var after = new Fixture(["cards"]) { Finished = true };
        after.Set(Stage.Children, 0);
        after.Run();
        Need(after.Completed == 0 && after.Frames.Top?.Id == 52 && after.Log.SequenceEqual(new[] { "cards" }),
            "A different recipe must first finish its paid child even when the game has reached its termination condition.");
        after.Frames.Pop(52, ResolutionFrameKind.ProgramSkill);
        after.Run();
        Need(after.Completed == 1 && after.Log.Last() == "fact-before-clear", "The deferred stop occurs after child completion and cleanup.");

        var fixture = new Fixture([]);
        var node = ProgramStageNode<Fixture, Receipt, Stage>.AwaitExternalReturn(Stage.External);
        Reject(() => fixture.Plan([node, node]));
        Reject(() => ProgramStageNode<Fixture, Receipt, Stage>.AwaitExternalReturn((Stage)999));
        Reject(() => fixture.Plan([]));
        var source = new[] { node };
        var frozen = fixture.Plan(source);
        source[0] = ProgramStageNode<Fixture, Receipt, Stage>.AwaitExternalReturn(Stage.Issue);
        fixture.Set(Stage.External, 0);
        ProgramStageRuntime.Run(41, fixture.Frames, fixture.Prompts, fixture, frozen);
        Need(fixture.Paid == 0, "Recipe construction detaches its nodes from the caller's mutable list.");
        fixture.Set(Stage.Issue, 0);
        Reject(() => ProgramStageRuntime.Run(41, fixture.Frames, fixture.Prompts, fixture, frozen));
        var noProgress = fixture.Plan([ProgramStageNode<Fixture, Receipt, Stage>.Run(Stage.Issue,
            static (_, _, _) => ProgramStageResult.Continue)]);
        Reject(() => ProgramStageRuntime.Run(41, fixture.Frames, fixture.Prompts, fixture, noProgress));
        var invalid = fixture.Plan([ProgramStageNode<Fixture, Receipt, Stage>.Run(Stage.Issue,
            static (_, _, _) => (ProgramStageResult)999)]);
        Reject(() => ProgramStageRuntime.Run(41, fixture.Frames, fixture.Prompts, fixture, invalid));
        CheckDeclaredTransitions();
    }

    public static void MalformedMetadataAndSuspendingCallbacksCannotAdvancePaidStages()
    {
        var missingTarget = new Fixture([]);
        Reject(() => new ProgramStagePlan<Fixture, Receipt, Stage>(Read, static receipt => receipt.Stage,
            static (_, _, _) => { },
            [ProgramStageNode<Fixture, Receipt, Stage>.RunTo(Stage.Issue, Stage.External,
                static (runtime, _, _) => runtime.Paid++)],
            withStage: static (frame, receipt, next) => With(frame, next, receipt.Actual)));
        Need(missingTarget.Paid == 0, "A missing declared destination is rejected during recipe construction, before any native action.");

        Func<ProgramSkillFrame, Receipt, Stage, ProgramSkillFrame>[] invalidWriters =
        [
            static (frame, receipt, _) => With(frame, receipt.Stage, receipt.Actual),
            static (frame, receipt, next) => With(frame, next, receipt.Actual) with { OwnerSeat = 1 },
            static (frame, receipt, next) => With(frame, next, receipt.Actual) with { SkillId = "fixture:another-skill" },
            static (frame, receipt, next) => With(frame, next, receipt.Actual) with { GameplayHash = "another-hash" },
            static (frame, receipt, next) => With(frame, next, receipt.Actual) with { InstructionIndex = 1 }
        ];
        foreach (var writer in invalidWriters)
        {
            var fixture = new Fixture([]);
            var plan = new ProgramStagePlan<Fixture, Receipt, Stage>(Read, static receipt => receipt.Stage,
                static (_, _, _) => { },
            [
                ProgramStageNode<Fixture, Receipt, Stage>.RunTo(Stage.Issue, Stage.External, static (runtime, _, _) =>
                {
                    runtime.Paid++;
                    // A broken writer must fail before this action. The child
                    // bounds the negative case even against an unguarded driver.
                    runtime.Frames.Push(NewFrame(52));
                }),
                ProgramStageNode<Fixture, Receipt, Stage>.AwaitExternalReturn(Stage.External)
            ], withStage: writer);
            Reject(() => ProgramStageRuntime.Run(41, fixture.Frames, fixture.Prompts, fixture, plan));
            Need(fixture.Paid == 0 && fixture.Frames.Top?.Id == 41,
                "A declaration cannot pay after ignoring its destination or changing the owning player, skill, hash or instruction.");
        }

        (string Name, Action<Fixture> Change, Stage Result)[] disruptions =
        [
            ("prompt", static runtime => runtime.Prompts.Set(new PendingDecision(DecisionKind.ProgramTrigger, 0, "fixture", [], []), true), Stage.Issue),
            ("child", static runtime => runtime.Frames.Push(NewFrame(52)), Stage.Issue),
            ("stage", static runtime => runtime.Set(Stage.Children, 9), Stage.Children)
        ];
        foreach (var point in new[] { "drain", "after-children", "after-drain" })
        foreach (var disruption in disruptions)
        {
            var fixture = new Fixture([]);
            fixture.Set(Stage.Children, 0);
            fixture.Frames.ReplaceTop(fixture.Frames.ActiveProgram(41) with { PendingMovementContinuation = new(0, 0, null) });
            void Disrupt(Fixture runtime, ProgramSkillFrame _, Receipt receipt)
            {
                runtime.Log.Add(point + ":" + disruption.Name);
                // Changing to another valid node must still be rejected at this
                // handoff rather than running the old Children continuation.
                if (disruption.Name == "stage") runtime.Set(Stage.External, receipt.Actual);
                else disruption.Change(runtime);
            }
            var plan = new ProgramStagePlan<Fixture, Receipt, Stage>(Read, static receipt => receipt.Stage,
                static (_, _, _) => { },
            [
                ProgramStageNode<Fixture, Receipt, Stage>.AwaitChildren(Stage.Children,
                    (runtime, frame, receipt) => { if (point == "drain") Disrupt(runtime, frame, receipt); return false; },
                    static (runtime, _, receipt) =>
                    {
                        runtime.Paid++;
                        runtime.Set(Stage.External, receipt.Actual);
                        return ProgramStageResult.Yield;
                    }, clearMovement: true, afterDrain: point == "after-drain" ? Disrupt : null),
                ProgramStageNode<Fixture, Receipt, Stage>.AwaitExternalReturn(Stage.External)
            ], afterChildren: point == "after-children" ? Disrupt : null);
            Reject(() => ProgramStageRuntime.Run(41, fixture.Frames, fixture.Prompts, fixture, plan));
            Need(fixture.Log.SequenceEqual(new[] { point + ":" + disruption.Name }) && fixture.Paid == 0 &&
                ((ProgramSkillFrame)fixture.Frames.Find(41)!).PendingMovementContinuation is not null,
                "A synchronous " + point + " callback must reject a " + disruption.Name +
                " boundary once, before clearing the original movement or invoking the old returned action.");
        }

        foreach (var disruption in disruptions)
        {
            var fixture = new Fixture([]);
            var plan = new ProgramStagePlan<Fixture, Receipt, Stage>(Read, static receipt => receipt.Stage,
                static (_, _, _) => { },
            [
                ProgramStageNode<Fixture, Receipt, Stage>.RunTo(Stage.Issue, Stage.External,
                    (runtime, _, _) => { runtime.Paid++; disruption.Change(runtime); }, commitBefore: false),
                ProgramStageNode<Fixture, Receipt, Stage>.AwaitExternalReturn(Stage.External)
            ], withStage: static (frame, receipt, next) => With(frame, next, receipt.Actual));
            Reject(() => ProgramStageRuntime.Run(41, fixture.Frames, fixture.Prompts, fixture, plan));
            Need(fixture.Paid == 1 && fixture.FrameReceipt.Stage == disruption.Result && fixture.Completed == 0,
                "A post-action transition cannot overwrite the original " + disruption.Name +
                " suspension or advance its cursor after an action that was required to remain synchronous.");
        }
    }

    private static void CheckDeclaredTransitions()
    {
        var fixture = new Fixture([]);
        var plan = new ProgramStagePlan<Fixture, Receipt, Stage>(Read, static receipt => receipt.Stage,
            static (_, frame, _) => Need(frame.Id == 41, "The declared sequence still validates its own original frame."),
        [
            ProgramStageNode<Fixture, Receipt, Stage>.RunTo(Stage.Issue, Stage.Children, static (runtime, frame, receipt) =>
            {
                Need(receipt.Stage == Stage.Children && Read(runtime.Frames.ActiveProgram(41))!.Stage == Stage.Children,
                    "A paid native action must observe the declared next stage before it suspends.");
                runtime.Paid++;
                runtime.Frames.ReplaceTop(frame with { PendingMovementContinuation = new(0, 0, null) });
                runtime.Frames.Push(NewFrame(52));
            }),
            ProgramStageNode<Fixture, Receipt, Stage>.ChildrenTo(Stage.Children, Stage.External,
                update: static receipt => receipt with { Actual = receipt.Actual + 3 }, clearMovement: true),
            ProgramStageNode<Fixture, Receipt, Stage>.RunTo(Stage.External, Stage.Complete, static (runtime, frame, receipt) =>
            {
                Need(receipt is { Stage: Stage.External, Actual: 7 } && frame.PendingMovementContinuation is null,
                    "Declared child transitions use the newly returned amount, apply the result update, then clear movement before the next action.");
                runtime.Log.Add("synchronous-before-stage");
                runtime.Frames.ReplaceTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
            }, commitBefore: false),
            ProgramStageNode<Fixture, Receipt, Stage>.Complete(Stage.Complete, static (runtime, frame, _) =>
            {
                Need(frame.Step == ResolutionFrameStep.AwaitingResponse,
                    "A synchronous action's returned frame fields must survive its declared post-action transition.");
                runtime.Finish(frame);
            })
        ], withStage: static (frame, receipt, next) => With(frame, next, receipt.Actual),
            drain: static (runtime, _, _) => { runtime.Set(Stage.Children, 4); return false; },
            afterChildren: static (runtime, frame, _) =>
            { Need(frame.PendingMovementContinuation is not null, "The declared completion fact retains the original movement return."); runtime.Log.Add("fact"); });
        ProgramStageRuntime.Run(41, fixture.Frames, fixture.Prompts, fixture, plan);
        Need(fixture.Paid == 1 && fixture.Frames.Top?.Id == 52, "The declared action yields to its actual child.");
        fixture.Frames.Pop(52, ResolutionFrameKind.ProgramSkill);
        ProgramStageRuntime.Run(41, fixture.Frames, fixture.Prompts, fixture, plan);
        Need(fixture.Completed == 1 && fixture.Paid == 1 && fixture.Log.SequenceEqual(new[] { "fact", "synchronous-before-stage" }),
            "Declarative transition and synchronous ordering do not repeat the original paid action.");

        var completing = new Fixture([]);
        completing.Set(Stage.Children, 0);
        var finish = new ProgramStagePlan<Fixture, Receipt, Stage>(Read, static receipt => receipt.Stage,
            static (_, _, _) => { }, [ProgramStageNode<Fixture, Receipt, Stage>.ChildrenComplete(Stage.Children)],
            drain: static (_, _, _) => false, complete: static (runtime, frame) => runtime.Finish(frame));
        ProgramStageRuntime.Run(41, completing.Frames, completing.Prompts, completing, finish);
        Need(completing.Completed == 1, "A completed paid child can use the recipe's completion port without a per-character wrapper.");
        Reject(() => completing.Plan([ProgramStageNode<Fixture, Receipt, Stage>.ChildrenTo(Stage.Children, Stage.Complete)]));
        Reject(() => completing.Plan([ProgramStageNode<Fixture, Receipt, Stage>.ChildrenComplete(Stage.Children)]));
    }

    private sealed class Fixture(string[] windows)
    {
        private readonly string[] _windows = windows;
        internal readonly Frames Frames = new();
        internal readonly Prompts Prompts = new();
        internal readonly List<string> Log = [];
        internal int Paid, Completed, Validations;
        internal bool Finished;
        private int _window;
        internal Receipt FrameReceipt => Read(Frames.Find(41) as ProgramSkillFrame ?? throw new InvalidOperationException())!;
        internal void Set(Stage stage, int actual) => Frames.ReplaceTop(With(Frames.ActiveProgram(41), stage, actual));
        internal void Run(bool stopBeforeChildren = false) => ProgramStageRuntime.Run(41, Frames, Prompts, this, Plan(
        [
            ProgramStageNode<Fixture, Receipt, Stage>.Run(Stage.Issue, static (runtime, frame, _) =>
            {
                ProgramStageRuntime.Issue(runtime.Frames, With(frame, Stage.Children, 0) with
                    { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) }, committed =>
                {
                    Need(Read(committed)!.Stage == Stage.Children && committed.PendingMovementContinuation is not null,
                        "Native payment observes the already committed wait stage.");
                    runtime.Paid++;
                    runtime.Frames.Push(NewFrame(52));
                });
                return ProgramStageResult.Continue;
            }),
            ProgramStageNode<Fixture, Receipt, Stage>.AwaitChildren(Stage.Children,
                static (runtime, _, _) =>
                {
                    if (runtime._window < runtime._windows.Length)
                    {
                        runtime.Log.Add(runtime._windows[runtime._window++]);
                        runtime.Frames.Push(NewFrame(52));
                        return true;
                    }
                    runtime.Set(Stage.Children, 7);
                    return false;
                },
                static (runtime, frame, receipt) =>
                {
                    Need(frame.PendingMovementContinuation is null, "Cleanup must precede this recipe's returned rule action.");
                    if (runtime.Finished) { runtime.Finish(frame); return ProgramStageResult.Yield; }
                    runtime.Log.Add("returned-" + receipt.Actual);
                    runtime.Set(Stage.External, receipt.Actual);
                    return ProgramStageResult.Continue;
                }, clearMovement: true, afterDrain: static (runtime, frame, _) =>
                {
                    if (!runtime.Finished) Need(frame.PendingMovementContinuation is not null, "The completion fact precedes movement cleanup.");
                    runtime.Log.Add("fact-before-clear");
                }),
            ProgramStageNode<Fixture, Receipt, Stage>.AwaitExternalReturn(Stage.External),
            ProgramStageNode<Fixture, Receipt, Stage>.Complete(Stage.Complete, static (runtime, frame, _) => runtime.Finish(frame))
        ], stopBeforeChildren));
        internal ProgramStagePlan<Fixture, Receipt, Stage> Plan(IEnumerable<ProgramStageNode<Fixture, Receipt, Stage>> nodes,
            bool stopBeforeChildren = false) => new(Read, static receipt => receipt.Stage,
                static (runtime, frame, _) =>
                { Need(frame.Id == 41 && frame.GameplayHash == "hash", "The original receipt identity must be validated before the stage."); runtime.Validations++; }, nodes,
                stopBeforeChildren ? static (runtime, frame, _) =>
                { if (!runtime.Finished) return false; runtime.Finish(frame); return true; } : null);
        internal void Finish(ProgramSkillFrame frame) { Completed++; Frames.Pop(frame.Id, frame.Kind); }
    }

    private static Receipt? Read(ProgramSkillFrame frame) => frame.NumberBindings.ToArray() is [var stage, var actual]
        ? new((Stage)stage.Value, actual.Value) : null;
    private static ProgramSkillFrame With(ProgramSkillFrame frame, Stage stage, int actual) => frame with
        { NumberBindings = [new("stage", (int)stage), new("actual", actual)] };
    private static ProgramSkillFrame NewFrame(long id) => new(id, 0, "fixture:stages", "run", "hash", 0, [], [])
        { NumberBindings = [new("stage", (int)Stage.Issue), new("actual", 0)] };
    private sealed class Frames : IProgramRuntimeFrames
    {
        private readonly List<ResolutionFrame> _frames = [NewFrame(41)];
        public ResolutionFrame? Top => _frames.LastOrDefault();
        public ResolutionFrame? Find(long id) => _frames.SingleOrDefault(frame => frame.Id == id);
        public ResolutionFrame? ParentOf(long id) => null;
        public ResolutionFrame? ChildOf(long id) => null;
        public IReadOnlyList<T> OfType<T>() where T : ResolutionFrame => _frames.OfType<T>().ToArray();
        public ProgramSkillFrame ActiveProgram(long id) => Top is ProgramSkillFrame frame && frame.Id == id ? frame : throw new InvalidOperationException();
        public long NextId() => throw new NotSupportedException();
        public void ReplaceTop(ResolutionFrame frame)
        {
            if (Top?.Id != frame.Id || Top.GetType() != frame.GetType()) throw new InvalidOperationException("Cannot replace another frame.");
            _frames[^1] = frame;
        }
        public void Replace(long id, ResolutionFrame frame) => throw new NotSupportedException();
        public void Push(ResolutionFrame frame) => _frames.Add(frame);
        public void Pop(long id, ResolutionFrameKind kind)
        { if (Top?.Id != id || Top.Kind != kind) throw new InvalidOperationException(); _frames.RemoveAt(_frames.Count - 1); }
        public void Advance(long id) => throw new NotSupportedException();
        public void AdvanceTop<T>() where T : ResolutionFrame => throw new NotSupportedException();
        public void AdvanceProgram(long id) => throw new NotSupportedException();
    }
    private sealed class Prompts : IProgramRuntimePrompts
    {
        public PendingDecision? Current { get; private set; }
        public PromptId NextId() => new(1);
        public void Clear() => Current = null;
        public void Set(PendingDecision decision, bool isHuman) => Current = decision;
    }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static void Reject(Action action)
    { try { action(); } catch (InvalidOperationException) { return; } throw new InvalidOperationException("Invalid recipe transition was accepted."); }
}
