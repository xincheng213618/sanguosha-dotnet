#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class GuanduEventCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGuanduEventProgramsEntries> getCoreGuanduEventProgramsRuntime) : IGuanduEventHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCoreGuanduEventProgramsRuntime().ExecuteGuanduEvent(f, op);
}
