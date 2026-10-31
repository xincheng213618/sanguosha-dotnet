namespace CardGame.Core;

internal interface IProgramSkillsChangedWindows : IProgramRuntimeOperations
{
    bool Begin(long parentFrameId);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class SkillOwnershipWindowRuntime(IProgramRuntimeFrames frames,
    IMatchParticipantStateAccess participants, ISkillGrantRevisionObservations observations,
    IProgramWindowCandidateQueries candidates) : IProgramSkillsChangedWindows
{
    public bool Begin(long parentFrameId)
    {
        foreach (var owner in participants.Players.Where(player => player.IsAlive))
        {
            if (!observations.Observe(owner)) continue;
            var facts = candidates.Capture(owner);
            var eligible = candidates.Eligible(owner, SkillProgramTriggerWindow.SkillsChanged, facts);
            if (eligible.Count == 0) continue;
            frames.Push(new ProgramLifecycleTriggerWindowFrame(frames.NextId(), owner.Seat,
                SkillProgramTriggerWindow.SkillsChanged, eligible, frames.Top is CardDeclarationFrame
                    ? ProgramLifecycleContinuation.ResumeCardDeclaration : ProgramLifecycleContinuation.ResumeParentProgram, facts)
                { ResumeProgramFrameId = parentFrameId });
            frames.AdvanceTop<ProgramLifecycleTriggerWindowFrame>();
            return true;
        }
        return false;
    }
}
