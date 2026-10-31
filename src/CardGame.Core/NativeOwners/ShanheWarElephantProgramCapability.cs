#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheWarElephantProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreProgramShanheSourceWarElephantRuntime>> getCoreProgramShanheSourceWarElephantRuntime) : IShanheWarElephantProgramHost
{
    public SkillProgramStepOutcome CounterSlash(ProgramSkillFrame frame) => getCoreProgramShanheSourceWarElephantRuntime().Value.UseShanheWarElephantCounterSlash(frame);
}
