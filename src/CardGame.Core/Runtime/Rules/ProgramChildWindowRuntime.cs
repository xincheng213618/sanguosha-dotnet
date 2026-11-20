namespace CardGame.Core;

internal interface IProgramChildWindows : IProgramRuntimeOperations
{
    bool Begin(long frameId, PostEventContinuation continuation);
}

internal interface IProgramChildWindowEntries : IProgramRuntimeOperations
{
    bool Recovery(long frameId, PostEventContinuation continuation);
    bool CharacterState(long frameId);
    bool HpChanged(long frameId, PostEventContinuation continuation);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramChildWindowRuntime(IProgramChildWindowEntries entries,
    IProgramMovementWindows movement, IProgramSkillsChangedWindows skills) : IProgramChildWindows
{
    public bool Begin(long frameId, PostEventContinuation continuation) =>
        entries.Recovery(frameId, continuation) || entries.CharacterState(frameId) ||
        entries.HpChanged(frameId, continuation) || movement.Begin(frameId) || skills.Begin(frameId);

    internal static bool MatchesSkillsChangedChild(long parentFrameId, ProgramLifecycleTriggerWindowFrame child) =>
        child.ResumeProgramFrameId == parentFrameId &&
        child.Window == SkillProgramTriggerWindow.SkillsChanged &&
        child.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;

    internal static bool MatchesStateChangeChild(long parentFrameId, ResolutionFrame child, IProgramEventHistoryQueries history) =>
        child is ProgramLifecycleTriggerWindowFrame state &&
        (MatchesSkillsChangedChild(parentFrameId, state) ||
         (state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange &&
          state.ResumeProgramFrameId == parentFrameId &&
          history.Read<CharacterStateChangedEvent>().Any(entry => entry.Change.Id == state.Id &&
              entry.Change.ParentFrameId == parentFrameId)));
}
