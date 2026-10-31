#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShenwuExpandedProgramHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreShenwuExpandedProgramsEntries> getCoreShenwuExpandedProgramsRuntime) : IShenwuExpandedProgramHost
{
    public SkillProgramStepOutcome ExecuteShenwuExpanded(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCoreShenwuExpandedProgramsRuntime().ExecuteShenwuExpanded(frame, op);
}
