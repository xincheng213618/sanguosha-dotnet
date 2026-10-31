#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WenheActivityProgramHost(Func<RuntimeEntries.CoreWenheActivityProgramsEntries> getCoreWenheActivityProgramsRuntime) : IWenheActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteWenheActivity(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCoreWenheActivityProgramsRuntime().ExecuteWenheActivity(frame, op);
}
