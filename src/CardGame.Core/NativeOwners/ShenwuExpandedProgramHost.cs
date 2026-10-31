#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenwuExpandedProgramHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreShenwuExpandedProgramsRuntime>> getCoreShenwuExpandedProgramsRuntime) : IShenwuExpandedProgramHost
{
    public SkillProgramStepOutcome ExecuteShenwuExpanded(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCoreShenwuExpandedProgramsRuntime().Value.ExecuteShenwuExpanded(frame, op);
}
