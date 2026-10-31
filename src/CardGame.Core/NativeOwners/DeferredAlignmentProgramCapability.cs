#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DeferredAlignmentProgramCapability(Func<RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries> getCoreGameEngineDeferredHandAlignmentsRuntime) : IDeferredAlignmentProgramHost
{
    public void ScheduleDeferredHandAlignment(ProgramSkillFrame frame, string continuationId) => getCoreGameEngineDeferredHandAlignmentsRuntime().ScheduleDeferredHandAlignment(frame, continuationId);
    public SkillProgramStepOutcome ResolveDeferredHandAlignment(ProgramSkillFrame frame) => getCoreGameEngineDeferredHandAlignmentsRuntime().ResolveDeferredHandAlignment(frame);
}
