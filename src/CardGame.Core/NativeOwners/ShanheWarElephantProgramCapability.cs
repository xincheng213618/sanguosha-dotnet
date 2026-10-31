#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheWarElephantProgramCapability(Func<RuntimeEntries.CoreProgramShanheSourceWarElephantEntries> getCoreProgramShanheSourceWarElephantRuntime) : IShanheWarElephantProgramHost
{
    public SkillProgramStepOutcome CounterSlash(ProgramSkillFrame frame) => getCoreProgramShanheSourceWarElephantRuntime().UseShanheWarElephantCounterSlash(frame);
}
