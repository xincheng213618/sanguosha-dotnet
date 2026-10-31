#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WenheActivityProgramHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreWenheActivityProgramsRuntime>> getCoreWenheActivityProgramsRuntime) : IWenheActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteWenheActivity(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCoreWenheActivityProgramsRuntime().Value.ExecuteWenheActivity(frame, op);
}
