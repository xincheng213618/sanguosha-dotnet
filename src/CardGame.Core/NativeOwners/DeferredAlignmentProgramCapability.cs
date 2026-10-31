#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class DeferredAlignmentProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries> getCoreGameEngineDeferredHandAlignmentsRuntime) : IDeferredAlignmentProgramHost
{
    public void ScheduleDeferredHandAlignment(ProgramSkillFrame frame, string continuationId) => getCoreGameEngineDeferredHandAlignmentsRuntime().ScheduleDeferredHandAlignment(frame, continuationId);
    public SkillProgramStepOutcome ResolveDeferredHandAlignment(ProgramSkillFrame frame) => getCoreGameEngineDeferredHandAlignmentsRuntime().ResolveDeferredHandAlignment(frame);
}
