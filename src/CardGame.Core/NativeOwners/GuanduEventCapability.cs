#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class GuanduEventCapability(Func<RuntimeEntries.CoreGuanduEventProgramsEntries> getCoreGuanduEventProgramsRuntime) : IGuanduEventHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCoreGuanduEventProgramsRuntime().ExecuteGuanduEvent(f, op);
}
