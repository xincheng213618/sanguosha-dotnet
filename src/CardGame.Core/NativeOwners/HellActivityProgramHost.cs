#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HellActivityProgramHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreHellActivityProgramsRuntime>> getCoreHellActivityProgramsRuntime) : IHellActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteHellActivity(ProgramSkillFrame f, SkillProgramEffectOp op) => getCoreHellActivityProgramsRuntime().Value.ExecuteHellActivity(f, op);
}
