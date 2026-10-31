#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenwuExpandedProgramHost(Func<RuntimeEntries.CoreShenwuExpandedProgramsEntries> getCoreShenwuExpandedProgramsRuntime) : IShenwuExpandedProgramHost
{
    public SkillProgramStepOutcome ExecuteShenwuExpanded(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCoreShenwuExpandedProgramsRuntime().ExecuteShenwuExpanded(frame, op);
}
