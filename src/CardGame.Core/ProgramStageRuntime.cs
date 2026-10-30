namespace CardGame.Core;

internal enum ProgramStageResult { Continue, Yield }
internal enum ProgramStageKind { Run, Prompt, AwaitChildren, AwaitExternalReturn, Complete }

// A recipe contains only immutable code and stage metadata. It never contains
// a match instance, frame, receipt, prompt, or a second pending-state store.
internal sealed class ProgramStageNode<TRuntime, TReceipt, TStage>
    where TRuntime : class where TReceipt : class where TStage : struct, Enum
{
    private ProgramStageNode(TStage stage, ProgramStageKind kind,
        Func<TRuntime, ProgramSkillFrame, TReceipt, ProgramStageResult>? advance = null,
        Func<TRuntime, ProgramSkillFrame, TReceipt, bool>? drain = null,
        bool clearMovement = false, Action<TRuntime, ProgramSkillFrame, TReceipt>? afterDrain = null,
        TStage? next = null, Func<TReceipt, TReceipt>? update = null, bool completeOnReturn = false,
        Func<TRuntime, ProgramSkillFrame, TReceipt, bool>? completeWhen = null,
        Action<TRuntime, ProgramSkillFrame, TReceipt>? issueAction = null, bool commitBefore = true)
    {
        if (!Enum.IsDefined(stage) || next is { } target && !Enum.IsDefined(target))
            throw new InvalidOperationException("A program stage requires defined stage keys.");
        Stage = stage; Kind = kind; Advance = advance; Drain = drain;
        ClearMovement = clearMovement; AfterDrain = afterDrain;
        Next = next; Update = update; CompleteOnReturn = completeOnReturn; CompleteWhen = completeWhen;
        IssueAction = issueAction; CommitBefore = commitBefore;
    }

    internal TStage Stage { get; }
    internal ProgramStageKind Kind { get; }
    internal Func<TRuntime, ProgramSkillFrame, TReceipt, ProgramStageResult>? Advance { get; }
    internal Func<TRuntime, ProgramSkillFrame, TReceipt, bool>? Drain { get; }
    internal bool ClearMovement { get; }
    internal Action<TRuntime, ProgramSkillFrame, TReceipt>? AfterDrain { get; }
    internal TStage? Next { get; }
    internal Func<TReceipt, TReceipt>? Update { get; }
    internal bool CompleteOnReturn { get; }
    internal Func<TRuntime, ProgramSkillFrame, TReceipt, bool>? CompleteWhen { get; }
    internal Action<TRuntime, ProgramSkillFrame, TReceipt>? IssueAction { get; }
    internal bool CommitBefore { get; }

    internal static ProgramStageNode<TRuntime, TReceipt, TStage> Run(TStage stage,
        Func<TRuntime, ProgramSkillFrame, TReceipt, ProgramStageResult> advance) =>
        new(stage, ProgramStageKind.Run, advance ?? throw new ArgumentNullException(nameof(advance)));

    internal static ProgramStageNode<TRuntime, TReceipt, TStage> Prompt(TStage stage,
        Action<TRuntime, ProgramSkillFrame, TReceipt> publish) => Terminal(stage, ProgramStageKind.Prompt, publish);

    internal static ProgramStageNode<TRuntime, TReceipt, TStage> Complete(TStage stage,
        Action<TRuntime, ProgramSkillFrame, TReceipt> complete) => Terminal(stage, ProgramStageKind.Complete, complete);

    private static ProgramStageNode<TRuntime, TReceipt, TStage> Terminal(TStage stage, ProgramStageKind kind,
        Action<TRuntime, ProgramSkillFrame, TReceipt> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new(stage, kind, (runtime, frame, receipt) =>
        { action(runtime, frame, receipt); return ProgramStageResult.Yield; });
    }

    internal static ProgramStageNode<TRuntime, TReceipt, TStage> AwaitExternalReturn(TStage stage) =>
        new(stage, ProgramStageKind.AwaitExternalReturn);

    // Some protocols publish their completion fact before clearing the movement
    // return. Others retain that return through a non-movement child. Both are
    // explicit, rather than a global cleanup or owner-death policy.
    internal static ProgramStageNode<TRuntime, TReceipt, TStage> AwaitChildren(TStage stage,
        Func<TRuntime, ProgramSkillFrame, TReceipt, bool> drain,
        Func<TRuntime, ProgramSkillFrame, TReceipt, ProgramStageResult> returned,
        bool clearMovement = false, Action<TRuntime, ProgramSkillFrame, TReceipt>? afterDrain = null) =>
        new(stage, ProgramStageKind.AwaitChildren, returned ?? throw new ArgumentNullException(nameof(returned)),
            drain ?? throw new ArgumentNullException(nameof(drain)), clearMovement, afterDrain);

    internal static ProgramStageNode<TRuntime, TReceipt, TStage> ChildrenTo(TStage stage, TStage next,
        Func<TReceipt, TReceipt>? update = null, bool clearMovement = false,
        Func<TRuntime, ProgramSkillFrame, TReceipt, bool>? completeWhen = null) =>
        new(stage, ProgramStageKind.AwaitChildren, next: next, update: update,
            clearMovement: clearMovement, completeWhen: completeWhen);

    internal static ProgramStageNode<TRuntime, TReceipt, TStage> ChildrenComplete(TStage stage, bool clearMovement = false) =>
        new(stage, ProgramStageKind.AwaitChildren, completeOnReturn: true, clearMovement: clearMovement);

    // commitBefore=false is for an original synchronous mutation whose returned
    // frame is updated afterwards. It never authorizes paying while suspended.
    internal static ProgramStageNode<TRuntime, TReceipt, TStage> RunTo(TStage stage, TStage next,
        Action<TRuntime, ProgramSkillFrame, TReceipt> action, bool commitBefore = true) =>
        new(stage, ProgramStageKind.Run, next: next,
            issueAction: action ?? throw new ArgumentNullException(nameof(action)), commitBefore: commitBefore);
}

internal sealed class ProgramStagePlan<TRuntime, TReceipt, TStage>
    where TRuntime : class where TReceipt : class where TStage : struct, Enum
{
    private readonly IReadOnlyDictionary<TStage, ProgramStageNode<TRuntime, TReceipt, TStage>> _nodes;
    internal Func<ProgramSkillFrame, TReceipt?> Read { get; }
    internal Func<TReceipt, TStage> Stage { get; }
    internal Action<TRuntime, ProgramSkillFrame, TReceipt> Validate { get; }
    internal Func<TRuntime, ProgramSkillFrame, TReceipt, bool>? StopBeforeStage { get; }
    internal Func<ProgramSkillFrame, TReceipt, TStage, ProgramSkillFrame>? WithStage { get; }
    internal Func<TRuntime, ProgramSkillFrame, TReceipt, bool>? Drain { get; }
    internal Action<TRuntime, ProgramSkillFrame>? Complete { get; }
    internal Action<TRuntime, ProgramSkillFrame, TReceipt>? AfterChildren { get; }

    internal ProgramStagePlan(Func<ProgramSkillFrame, TReceipt?> read, Func<TReceipt, TStage> stage,
        Action<TRuntime, ProgramSkillFrame, TReceipt> validate,
        IEnumerable<ProgramStageNode<TRuntime, TReceipt, TStage>> nodes,
        Func<TRuntime, ProgramSkillFrame, TReceipt, bool>? stopBeforeStage = null,
        Func<ProgramSkillFrame, TReceipt, TStage, ProgramSkillFrame>? withStage = null,
        Func<TRuntime, ProgramSkillFrame, TReceipt, bool>? drain = null,
        Action<TRuntime, ProgramSkillFrame>? complete = null,
        Action<TRuntime, ProgramSkillFrame, TReceipt>? afterChildren = null)
    {
        Read = read ?? throw new ArgumentNullException(nameof(read));
        Stage = stage ?? throw new ArgumentNullException(nameof(stage));
        Validate = validate ?? throw new ArgumentNullException(nameof(validate));
        ArgumentNullException.ThrowIfNull(nodes);
        var entries = nodes.ToArray();
        if (entries.Length == 0 || entries.Any(node => node is null) ||
            entries.Select(node => node.Stage).Distinct().Count() != entries.Length)
            throw new InvalidOperationException("A program stage recipe requires nonempty unique stage nodes.");
        var keys = entries.Select(node => node.Stage).ToHashSet();
        if (entries.Any(node => node.Next is { } next && !keys.Contains(next)))
            throw new InvalidOperationException("A program stage transition requires a registered target node.");
        if (entries.Any(node => node.Next is not null) && withStage is null ||
            entries.Any(node => node.Kind == ProgramStageKind.AwaitChildren && node.Drain is null) && drain is null ||
            entries.Any(node => node.CompleteOnReturn || node.CompleteWhen is not null) && complete is null)
            throw new InvalidOperationException("A declarative program stage lost its state, child-drain or completion port.");
        _nodes = new System.Collections.ObjectModel.ReadOnlyDictionary<TStage, ProgramStageNode<TRuntime, TReceipt, TStage>>(
            entries.ToDictionary(node => node.Stage));
        StopBeforeStage = stopBeforeStage;
        WithStage = withStage; Drain = drain; Complete = complete; AfterChildren = afterChildren;
    }

    internal ProgramStageNode<TRuntime, TReceipt, TStage> Resolve(TReceipt receipt) =>
        _nodes.TryGetValue(Stage(receipt), out var node) ? node :
            throw new InvalidOperationException("The active program receipt has no registered stage node.");
}

// This is the native-operation substep driver of SkillProgramExecutor. The
// executor still owns the instruction cursor and typed parent return. Character
// recipes retain their original receipt, rule actions, and payment validation.
internal static class ProgramStageRuntime
{
    internal static void Run<TRuntime, TReceipt, TStage>(long id, IProgramRuntimeFrames frames,
        IProgramRuntimePrompts prompts, TRuntime runtime, ProgramStagePlan<TRuntime, TReceipt, TStage> plan)
        where TRuntime : class where TReceipt : class where TStage : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(frames);
        ArgumentNullException.ThrowIfNull(prompts);
        ArgumentNullException.ThrowIfNull(runtime);
        ArgumentNullException.ThrowIfNull(plan);
        while (TryRead(id, frames, prompts, plan, out var frame, out var receipt))
        {
            plan.Validate(runtime, frame, receipt);
            if (plan.StopBeforeStage?.Invoke(runtime, frame, receipt) == true) return;
            if (!TryRead(id, frames, prompts, plan, out frame, out receipt)) return;
            var node = plan.Resolve(receipt);
            if (node.Kind == ProgramStageKind.AwaitExternalReturn) return;
            if (node.Kind == ProgramStageKind.AwaitChildren)
            {
                var drain = node.Drain ?? plan.Drain!;
                if (drain(runtime, frame, receipt)) return;
                // A declined drain can replace the original frame. Never use its
                // old amount, cursor, or receipt for the subsequent transition.
                (frame, receipt) = ReadSynchronous(id, frames, prompts, plan, frame, node.Stage);
                plan.AfterChildren?.Invoke(runtime, frame, receipt);
                (frame, receipt) = ReadSynchronous(id, frames, prompts, plan, frame, node.Stage);
                node.AfterDrain?.Invoke(runtime, frame, receipt);
                (frame, receipt) = ReadSynchronous(id, frames, prompts, plan, frame, node.Stage);
                if (node.ClearMovement)
                {
                    frames.ReplaceTop(frame with { PendingMovementContinuation = null });
                    (frame, receipt) = ReadSynchronous(id, frames, prompts, plan, frame, node.Stage);
                }
                var complete = node.CompleteOnReturn || node.CompleteWhen?.Invoke(runtime, frame, receipt) == true;
                (frame, receipt) = ReadSynchronous(id, frames, prompts, plan, frame, node.Stage);
                if (complete)
                { plan.Complete!(runtime, frame); return; }
            }
            var before = frame;
            ProgramStageResult result;
            if (node.Next is { } next)
            {
                var nextReceipt = node.Update?.Invoke(receipt) ?? receipt;
                if (node.CommitBefore)
                {
                    frames.ReplaceTop(TransitionFrame(plan, frame, nextReceipt, next));
                    if (node.IssueAction is { } action)
                    {
                        var committed = ReadSynchronous(id, frames, prompts, plan, frame, next);
                        action(runtime, committed.Frame, committed.Receipt);
                    }
                }
                else
                {
                    node.IssueAction!(runtime, frame, receipt);
                    var returned = ReadSynchronous(id, frames, prompts, plan, frame, node.Stage);
                    frames.ReplaceTop(TransitionFrame(plan, returned.Frame, nextReceipt, next));
                }
                result = ProgramStageResult.Continue;
            }
            else result = node.Advance!(runtime, frame, receipt);
            if (result == ProgramStageResult.Yield) return;
            if (result != ProgramStageResult.Continue)
                throw new InvalidOperationException("A program stage returned an invalid transition outcome.");
            if (prompts.Current is null && ReferenceEquals(frames.Top, before))
                throw new InvalidOperationException("A continuing program stage did not advance its original frame.");
        }
    }

    private static ProgramSkillFrame TransitionFrame<TRuntime, TReceipt, TStage>(
        ProgramStagePlan<TRuntime, TReceipt, TStage> plan, ProgramSkillFrame frame, TReceipt receipt, TStage next)
        where TRuntime : class where TReceipt : class where TStage : struct, Enum
    {
        var result = plan.WithStage!(frame, receipt, next);
        RequireIdentity(frame, result);
        if (plan.Read(result) is not { } written || !EqualityComparer<TStage>.Default.Equals(plan.Stage(written), next))
            throw new InvalidOperationException("A declared stage transition did not commit its actual target before the action.");
        return result;
    }

    private static (ProgramSkillFrame Frame, TReceipt Receipt) ReadSynchronous<TRuntime, TReceipt, TStage>(long id,
        IProgramRuntimeFrames frames, IProgramRuntimePrompts prompts, ProgramStagePlan<TRuntime, TReceipt, TStage> plan,
        ProgramSkillFrame expected, TStage stage)
        where TRuntime : class where TReceipt : class where TStage : struct, Enum
    {
        if (!TryRead(id, frames, prompts, plan, out var frame, out var receipt))
            throw new InvalidOperationException("A synchronous stage callback cannot suspend, retire, or replace its original program.");
        RequireIdentity(expected, frame);
        if (!EqualityComparer<TStage>.Default.Equals(plan.Stage(receipt), stage))
            throw new InvalidOperationException("A child drain or completion callback changed its paused stage without a declared return.");
        return (frame, receipt);
    }

    private static void RequireIdentity(ProgramSkillFrame expected, ProgramSkillFrame actual)
    {
        if (actual is null || actual.Id != expected.Id || actual.GetType() != expected.GetType() ||
            actual.OwnerSeat != expected.OwnerSeat || actual.SkillId != expected.SkillId ||
            actual.SkillInstanceId != expected.SkillInstanceId || actual.ActivationId != expected.ActivationId ||
            actual.TriggerId != expected.TriggerId || actual.GameplayHash != expected.GameplayHash ||
            actual.InstructionIndex != expected.InstructionIndex)
            throw new InvalidOperationException("A stage transition changed its original instruction or binding identity.");
    }

    internal static void Issue(IProgramRuntimeFrames frames, ProgramSkillFrame committedFrame,
        Action<ProgramSkillFrame> issue)
    {
        ArgumentNullException.ThrowIfNull(issue);
        // ReplaceTop enforces original frame ID and type. This happens before a
        // native action can publish an event or suspend into a paid child.
        RequireIdentity(frames.ActiveProgram(committedFrame.Id), committedFrame);
        frames.ReplaceTop(committedFrame);
        issue(committedFrame);
    }

    private static bool TryRead<TRuntime, TReceipt, TStage>(long id, IProgramRuntimeFrames frames,
        IProgramRuntimePrompts prompts, ProgramStagePlan<TRuntime, TReceipt, TStage> plan,
        out ProgramSkillFrame frame, out TReceipt receipt)
        where TRuntime : class where TReceipt : class where TStage : struct, Enum
    {
        if (prompts.Current is null && frames.Top is ProgramSkillFrame active && active.Id == id &&
            plan.Read(active) is { } current)
        { frame = active; receipt = current; return true; }
        frame = null!; receipt = null!; return false;
    }
}
