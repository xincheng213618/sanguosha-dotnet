#nullable enable
namespace CardGame.Core.NativeOwners;
internal abstract class ProgramTriggerWindowHost<TFrame>(Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, Func<RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime) : ProgramTriggerWindowHost(), IProgramTriggerWindowHost<TFrame> where TFrame : ResolutionFrame
{
    public bool TryGetFrame(out TFrame frame)
    {
        if (getFrameStore().LastOrDefault()is TFrame current)
        {
            frame = current;
            return true;
        }

        frame = null !;
        return false;
    }

    public virtual ProgramTriggerWindowPreparation<TFrame> Prepare(TFrame frame) => new(ProgramTriggerWindowFlow.Ready, frame);
    public abstract bool IsComplete(TFrame frame);
    public abstract ProgramTriggerWindowFlow Close(TFrame frame);
    protected abstract ProgramTriggerCandidate Candidate(TFrame frame);
    protected virtual bool MatchesSkipCandidate(TFrame frame, ProgramTriggerCandidate candidate) => Candidate(frame) == candidate;
    public abstract ProgramTriggerWindowEntry Read(TFrame frame);
    protected virtual bool CanReadPending(TFrame frame) => true;
    protected virtual ProgramTriggerWindowEntry ReadPending(TFrame frame) => Read(frame);
    public bool CanRun(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => getCoreGameEngineProgramLifecycleRuntime().CanRunProgramTrigger(candidate, context);
    public SkillProgramTrigger ResolveTrigger(ProgramTriggerCandidate candidate) => getCoreGameEngineProgramLifecycleRuntime().GetProgramTrigger2(candidate);
    public abstract void Skip(TFrame frame, ProgramTriggerCandidate candidate);
    public virtual IReadOnlyList<ProgramTriggerCandidate>? ChoiceGroup(TFrame frame, ProgramTriggerCandidate first, string group) => null;
    protected virtual bool SupportsChoiceGroups => false;

    public virtual void BeforePrompt(TFrame frame)
    {
    }

    public void Expose(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => getCoreGameEngineProgramLifecycleRuntime().ExposeProgramTriggerDecision(candidate, context);
    public void ExposeGroup(IReadOnlyList<ProgramTriggerCandidate> members, ProgramTriggerCandidate first, ProgramSkillWindowContext context, string group) => getCoreGameEngineProgramLifecycleRuntime().ExposeProgramTriggerGroupDecision(members, first, context, group);
    public void Begin(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => getCoreGameEngineProgramLifecycleRuntime().BeginProgramBinding(candidate, context);
    public virtual int CandidateIndex(TFrame frame) => throw new InvalidOperationException("This window has no program choice-group cursor.");
    public void RecordGroupSkip(TFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => advanceEventRulesAndQueueFact(new ProgramBindingResolvedEvent(frame.Id, candidate.SkillId, candidate.BindingId, candidate.SkillInstanceId, candidate.OwnerSeat, context.Window, Activated: false, Completed: false));
    public virtual void MovePastGroup(TFrame frame, int nextIndex) => throw new InvalidOperationException("This window has no program choice-group cursor.");
    public void Continue() => getCoreGameEngineRuntimeRuntime().AdvanceRuntimeTop<TFrame>();
    internal override bool TryReadPending(out ProgramTriggerWindowEntry entry)
    {
        if (TryGetFrame(out var frame) && CanReadPending(frame))
        {
            entry = ReadPending(frame);
            return true;
        }

        entry = default;
        return false;
    }

    internal override bool TrySkip(ProgramTriggerCandidate candidate)
    {
        if (!TryGetFrame(out var frame) || !MatchesSkipCandidate(frame, candidate))
            return false;
        Skip(frame, candidate);
        Continue();
        return true;
    }

    internal override bool TryResolveGroup(ProgramTriggerCandidate first, ProgramSkillWindowContext context, PromptChoice selected, string? action)
    {
        if (!TryGetFrame(out var frame) || !SupportsChoiceGroups || ResolveTrigger(first).ChoiceGroup is not { } group)
            return false;
        ProgramTriggerWindowDriver.ResolveChoiceGroup(this, frame, first, context, group, selected, action, getCoreNativePromptBoundaryRuntime().ClearPendingDecision);
        return true;
    }
}
