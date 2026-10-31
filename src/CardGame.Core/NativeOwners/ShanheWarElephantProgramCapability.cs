#nullable enable
using System.Text.Json.Serialization;
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheWarElephantProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramShanheSourceWarElephantEntries> getCoreProgramShanheSourceWarElephantRuntime) : IShanheWarElephantProgramHost
{
    public SkillProgramStepOutcome CounterSlash(ProgramSkillFrame frame) => getCoreProgramShanheSourceWarElephantRuntime().UseShanheWarElephantCounterSlash(frame);
}
