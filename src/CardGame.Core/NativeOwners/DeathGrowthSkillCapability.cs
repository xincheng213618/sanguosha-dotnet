#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DeathGrowthSkillCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDeathGrowthSkillsEntries> getCoreGameEngineDeathGrowthSkillsRuntime) : IDeathGrowthSkillHost
{
    public SkillProgramStepOutcome DrawAfterOtherDeathThenChooseMissingSkill(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineDeathGrowthSkillsRuntime().BeginDeathGrowthSkill(f, e);
}
